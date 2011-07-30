#region -- License Terms --
//
// NLiblet
//
// Copyright (C) 2011 FUJIWARA, Yusuke
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
//
#endregion -- License Terms --

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection.Emit;
using System.Text;
using NLiblet.Collections;
using NLiblet.Reflection;

namespace NLiblet.Text
{
	// FIXME: Not nested
	partial class CommonCustomFormatter
	{
		/// <summary>
		///		Defines common base class of item formatter.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		internal abstract class ItemFormatter<T>
		{
			public abstract void FormatTo( T item, FormattingContext context );
		}

		/// <summary>
		///		Non-generic entry point for array segment formatter.
		/// </summary>
		internal static class ArraySegmentFormatter
		{
			public static ItemFormatter<T> Get<T>()
			{
				Contract.Assert( typeof( T ).IsClosedTypeOf( typeof( ArraySegment<> ) ) );

				// TODO: caching
				return Activator.CreateInstance( typeof( ArraySegmentFormatter<> ).MakeGenericType( typeof( T ).GetGenericArguments()[ 0 ] ) ) as ItemFormatter<T>;
			}
		}

		/// <summary>
		///		Item formatter for <see cref="ArraySegment{T}"/>.
		/// </summary>
		/// <typeparam name="TItem">Type of array element.</typeparam>
		internal sealed class ArraySegmentFormatter<TItem> : ItemFormatter<ArraySegment<TItem>>
		{
			private readonly Action<ArraySegment<TItem>, FormattingContext> _action;
			private readonly Action<TItem, FormattingContext> _itemFormatter;

			public ArraySegmentFormatter()
			{
				if ( typeof( byte ).TypeHandle.Equals( typeof( TItem ).TypeHandle ) )
				{
					this._action = CreateShim<ArraySegment<byte>>( FormatBytesTo );
					this._itemFormatter = null;
				}
				else if ( typeof( char ).TypeHandle.Equals( typeof( TItem ).TypeHandle ) )
				{
					this._action = CreateShim<ArraySegment<char>>( FormatCharsTo );
					this._itemFormatter = null;
				}
				else
				{
					this._action = GenericFormatTo;
					this._itemFormatter = GenericItemFormatter<TItem>.Action;
				}
			}

			private static Action<ArraySegment<TItem>, FormattingContext> CreateShim<T>( Action<T, FormattingContext> target )
			{
				Contract.Assert( target.Target == null );

				var dm =
					new DynamicMethod(
						"FormatToShimFor_" + target.Method.Name,
						returnType: null,
						parameterTypes: new[] { typeof( ArraySegment<TItem> ), typeof( FormattingContext ) },
						owner: typeof( ArraySegmentFormatter<TItem> ),
						skipVisibility: false
					);

				var buffer = new StringBuilder();
				using ( var tracer = new StringWriter( buffer ) )
				{
					var il = new TracingILGenerator( dm, tracer );
					il.EmitLdarg_0();
					//il.EmitCastclass( typeof( T ) );
					il.EmitLdarg_1();
					il.EmitAnyCall( target.Method );
					il.EmitRet();
					tracer.Flush();
					Debug.WriteLine( dm );
					Debug.WriteLine( buffer );
				}

				return dm.CreateDelegate( typeof( Action<ArraySegment<TItem>, FormattingContext> ) ) as Action<ArraySegment<TItem>, FormattingContext>;
			}

			public sealed override void FormatTo( ArraySegment<TItem> arraySegment, FormattingContext context )
			{
				this._action( arraySegment, context );
			}

			private static void FormatBytesTo( ArraySegment<byte> arraySegment, FormattingContext context )
			{
				Debug.WriteLine( "ArraySegmentFormatter<{0}>::FormatBytesTo( {1}, {2} )", typeof( TItem ).FullName, arraySegment, context );

				if ( context.IsInCollection )
				{
					context.Buffer.Append( '\"' );
				}

				context.Buffer.AppendHex( arraySegment.AsEnumerable() );

				if ( context.IsInCollection )
				{
					context.Buffer.Append( '\"' );
				}
			}

			private static void FormatCharsTo( ArraySegment<char> arraySegment, FormattingContext context )
			{
				Debug.WriteLine( "ArraySegmentFormatter<{0}>::FormatCharsTo( {1}, {2} )", typeof( TItem ).FullName, arraySegment, context );

				if ( context.IsInCollection )
				{
					context.Buffer.Append( '\"' );
				}

				foreach ( var c in context.IsInCollection ? ItemFormatter.EscapeChars( arraySegment.AsEnumerable() ) : arraySegment.AsEnumerable() )
				{
					context.Buffer.Append( c );
				}

				if ( context.IsInCollection )
				{
					context.Buffer.Append( '\"' );
				}
			}

			private void GenericFormatTo( ArraySegment<TItem> arraySegment, FormattingContext context )
			{
				Debug.WriteLine( "ArraySegmentFormatter<{0}>::GenericFormatTo( {1}, {2} )", typeof( TItem ).FullName, arraySegment, context );

				if ( context.IsInCollection )
				{
					context.Buffer.Append( '\"' );
				}

				context.Buffer.Append( '[' );
				context.EnterCollection();

				bool isFirstEntry = true;
				foreach ( var entry in arraySegment.AsEnumerable() )
				{
					if ( !isFirstEntry )
					{
						context.Buffer.Append( ", " );
					}
					else
					{
						context.Buffer.Append( ' ' );
					}

					this._itemFormatter( entry, context );

					isFirstEntry = false;
				}

				if ( !isFirstEntry )
				{
					context.Buffer.Append( ' ' );
				}

				context.LeaveCollection();
				context.Buffer.Append( ']' );
				if ( context.IsInCollection )
				{
					context.Buffer.Append( '\"' );
				}
			}

		}
	}
}
