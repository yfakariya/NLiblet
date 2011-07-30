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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using NLiblet.Reflection;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Type specific <see cref="ItemFormatter"/> implementation.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	internal sealed partial class GenericItemFormatter<T> : ItemFormatter<T>
	{
		public static readonly Action<T, FormattingContext> Action;

		/// <summary>
		///		Initialize closed type specialized for <typeparamref name="T"/> type.
		/// </summary>
		static GenericItemFormatter()
		{
			const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.NonPublic;

			if ( typeof( T ).TypeHandle.Equals( typeof( object ).TypeHandle )
				|| typeof( T ).TypeHandle.Equals( typeof( ValueType ).TypeHandle ) )
			{
				throw new NotImplementedException();
			}

			if ( typeof( T ).TypeHandle.Equals( typeof( string ).TypeHandle )
				|| typeof( T ).TypeHandle.Equals( typeof( StringBuilder ).TypeHandle ) )
			{
				throw new NotImplementedException();
			}

			if ( typeof( T ).TypeHandle.Equals( typeof( bool ).TypeHandle ) )
			{
				throw new NotImplementedException();
			}

			if ( typeof( byte[] ).TypeHandle.Equals( typeof( T ).TypeHandle ) )
			{
				throw new NotImplementedException();
			}

			if ( typeof( char[] ).TypeHandle.Equals( typeof( T ).TypeHandle ) )
			{
				throw new NotImplementedException();
			}

			if ( typeof( T ).IsArray )
			{
				Action =
					Delegate.CreateDelegate(
						typeof( Action<T, FormattingContext> ),
						typeof( GenericItemFormatter<T> ).GetMethod( "FormatGenericEnumerableTo", bindingFlags )
					) as Action<T, FormattingContext>;
				return;
			}

			if ( typeof( DateTimeOffset ).TypeHandle.Equals( typeof( T ).TypeHandle )
				|| typeof( DateTime ).TypeHandle.Equals( typeof( T ).TypeHandle ) )
			{
				throw new NotImplementedException();
			}

			if ( typeof( TimeSpan ).TypeHandle.Equals( typeof( T ).TypeHandle ) )
			{
				throw new NotImplementedException();
			}

			bool isFormattable;
			if ( CommonCustomFormatter.IsNumerics( typeof( T ).TypeHandle, out isFormattable ) )
			{
				Action =
					Delegate.CreateDelegate(
						typeof( Action<T, FormattingContext> ),
						isFormattable
						? typeof( GenericItemFormatter<T> ).GetMethod( "FormatFormattableNumericTo", bindingFlags ).MakeGenericMethod( typeof( T ) )
						: typeof( GenericItemFormatter<T> ).GetMethod( "FormatNumericTo", bindingFlags )
					) as Action<T, FormattingContext>;
				return;
			}

			if ( typeof( IFormattable ).IsAssignableFrom( typeof( T ) ) )
			{
				Action =
					Delegate.CreateDelegate(
						typeof( Action<T, FormattingContext> ),
						typeof( GenericItemFormatter<T> ).GetMethod( "FormatFormattableTo", bindingFlags )
					) as Action<T, FormattingContext>;
				return;
			}

			if ( typeof( SerializationInfo ).TypeHandle.Equals( typeof( T ).TypeHandle ) )
			{
				throw new NotImplementedException();
			}

			if ( typeof( T ).IsClosedTypeOf( typeof( ArraySegment<> ) ) )
			{
				Action =
					Delegate.CreateDelegate(
						typeof( Action<T, FormattingContext> ),
						typeof( GenericItemFormatter<T> ).GetMethod( "FormatArraySegmentTo", bindingFlags )
					) as Action<T, FormattingContext>;
				return;
			}

			if ( typeof( T ).IsClosedTypeOf( typeof( Tuple<> ) )
				|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,> ) )
				|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,> ) )
				|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,> ) )
				|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,,> ) )
				|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,,,> ) )
				|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,,,,> ) )
				|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,,,,,> ) )
			)
			{
				Action =
					Delegate.CreateDelegate(
						typeof( Action<T, FormattingContext> ),
						typeof( GenericItemFormatter<T> ).GetMethod( "FormatTuple" + typeof( T ).GetGenericArguments().Length + "To", bindingFlags )
					) as Action<T, FormattingContext>;
				return;
			}

			var toString = typeof( T ).GetMethod( "ToString", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null );
			if ( toString != null
				&& !typeof( object ).TypeHandle.Equals( toString.DeclaringType.TypeHandle )
				&& !typeof( ValueType ).TypeHandle.Equals( toString.DeclaringType.TypeHandle )
			)
			{
				Action =
					Delegate.CreateDelegate(
						typeof( Action<T, FormattingContext> ),
						typeof( GenericItemFormatter<T> ).GetMethod( "FormatObjectTo", bindingFlags )
					) as Action<T, FormattingContext>;
				return;
			}

			if ( typeof( T ).Implements( typeof( IDictionary<,> ) ) )
			{
				Action =
					Delegate.CreateDelegate(
						typeof( Action<T, FormattingContext> ),
						typeof( GenericItemFormatter<T> ).GetMethod( "FormatGenericDictionaryTo", bindingFlags )
					) as Action<T, FormattingContext>;
				return;
			}

			if ( typeof( T ).Implements( typeof( IEnumerable<> ) ) )
			{
				if ( typeof( ICollection ).IsAssignableFrom( typeof( T ) ) )
				{
					var genericArgument = typeof( T ).GetGenericArguments()[ 0 ];

					if ( typeof( byte ).TypeHandle.Equals( genericArgument.TypeHandle ) )
					{
						throw new NotImplementedException();
					}
					else if ( typeof( char ).TypeHandle.Equals( genericArgument.TypeHandle ) )
					{
						throw new NotImplementedException();
					}
					else
					{
						Action =
							Delegate.CreateDelegate(
								typeof( Action<T, FormattingContext> ),
								typeof( GenericItemFormatter<T> ).GetMethod( "FormatGenericEnumerableTo", bindingFlags )
							) as Action<T, FormattingContext>;
					}

					return;
				}
			}
			else if ( typeof( IDictionary ).IsAssignableFrom( typeof( T ) ) )
			{
				Action =
					Delegate.CreateDelegate(
						typeof( Action<T, FormattingContext> ),
						typeof( GenericItemFormatter<T> ).GetMethod( "FormatNonGenericDictionaryTo", bindingFlags )
					) as Action<T, FormattingContext>;
				return;
			}
			else if ( typeof( IEnumerable ).IsAssignableFrom( typeof( T ) ) )
			{
				Action =
					Delegate.CreateDelegate(
						typeof( Action<T, FormattingContext> ),
						typeof( GenericItemFormatter<T> ).GetMethod( "FormatNonGenericEnumerableTo", bindingFlags )
					) as Action<T, FormattingContext>;
				return;
			}

			Action =
				Delegate.CreateDelegate(
					typeof( Action<T, FormattingContext> ),
					typeof( GenericItemFormatter<T> ).GetMethod( "FormatObjectTo", bindingFlags )
				) as Action<T, FormattingContext>;
			return;
		}

		// Note: These methods are invoked via delegate.

		private static void FormatNumericTo( T item, FormattingContext context )
		{
			Debug.WriteLine( "ItemFormatter<{0}>::FormatNumericTo( {1}, {2} )", typeof( T ).FullName, item, context );
			context.Buffer.Append( item );
		}

		private static void FormatFormattableNumericTo<TItem>( TItem item, FormattingContext context )
			where TItem : IFormattable
		{
			Contract.Assert( typeof( TItem ).TypeHandle.Equals( typeof( T ).TypeHandle ) );

			Debug.WriteLine( "ItemFormatter<{0}>::FormatFormattableNumericTo<{3}>( {1}, {2} )", typeof( T ).FullName, item, context, typeof( TItem ).FullName );

			if ( context.IsInCollection )
			{
				context.Buffer.Append( item );
			}
			else
			{
				context.Buffer.Append( item.ToString( context.Format, context.FallbackProvider ) );
			}
		}

		private static void FormatBoxedFormattableNumericTo( IFormattable item, FormattingContext context )
		{
			Debug.WriteLine( "ItemFormatter<{0}>::FormatBoxedFormattableNumericTo( {1}, {2} )", typeof( T ).FullName, item, context );

			if ( context.IsInCollection )
			{
				context.Buffer.Append( item );
			}
			else
			{
				context.Buffer.Append( item.ToString( context.Format, context.FallbackProvider ) );
			}
		}

		private static void FormatFormattableTo( T item, FormattingContext context )
		{
			Debug.WriteLine( "ItemFormatter<{0}>::FormatFormattableTo( {1}, {2} )", typeof( T ).FullName, item, context );

			if ( item == null )
			{
				context.Buffer.Append( CommonCustomFormatter.NullRepresentation );
			}
			else
			{
				if ( context.IsInCollection )
				{
					context.Buffer.Append( '"' );

					// always tend to Json compat
					foreach ( var c in CommonCustomFormatter.CollectionItemFilter.Escape( ( ( IFormattable )item ).ToString( context.Format, CultureInfo.InvariantCulture ) ) )
					{
						context.Buffer.Append( c );
					}

					context.Buffer.Append( '"' );
				}
				else
				{
					foreach ( var c in CommonCustomFormatter.CollectionItemFilter.Escape( ( ( IFormattable )item ).ToString( context.Format, context.FallbackProvider ) ) )
					{
						context.Buffer.Append( c );
					}
				}
			}
		}

		private static void FormatArraySegmentTo( T item, FormattingContext context )
		{
			Debug.WriteLine( "ItemFormatter<{0}>::FormatArraySegmentTo( {1}, {2} )", typeof( T ).FullName, item, context );

			ArraySegmentFormatter.Get<T>().FormatTo( item, context );

		}

		private static void FormatObjectTo( T item, FormattingContext context )
		{
			Debug.WriteLine( "ItemFormatter<{0}>::FormatObjectTo( {1}, {2} )", typeof( T ).FullName, item, context );

			if ( Object.ReferenceEquals( item, null ) )
			{
				context.Buffer.Append( CommonCustomFormatter.NullRepresentation );
				return;
			}

			if ( IsRuntimeCheckNeeded( typeof( T ) ) )
			{
				// T is Object or ValueType, so it might be boxed type.
				ItemFormatter.Get( item.GetType() ).FormatObjectTo( item, context );
				return;
			}

			if ( context.IsInCollection )
			{
				context.Buffer.Append( '"' );
			}

			context.Buffer.Append( item );

			if ( context.IsInCollection )
			{
				context.Buffer.Append( '"' );
			}
		}

		private static void FormatNonGenericEnumerableTo( T item, FormattingContext context )
		{
			Contract.Assert( item is IEnumerable );

			Debug.WriteLine( "ItemFormatter<{0}>::FormatNonGenericEnumerableTo( {1}, {2} )", typeof( T ).FullName, item, context );

			context.Buffer.Append( "[ " );

			context.EnterCollection();

			bool isFirstEntry = true;
			foreach ( var entry in ( item as IEnumerable ) )
			{
				if ( !isFirstEntry )
				{
					context.Buffer.Append( ", " );
				}

				if ( Object.ReferenceEquals( entry, null ) )
				{
					context.Buffer.Append( CommonCustomFormatter.NullRepresentation );
				}
				else
				{
					ItemFormatter.Get( entry.GetType() ).FormatObjectTo( entry, context );
				}

				isFirstEntry = false;
			}

			context.LeaveCollection();
			context.Buffer.Append( " ]" );
		}

		private static void FormatGenericEnumerableTo( T item, FormattingContext context )
		{
			Contract.Assert( typeof( T ).Implements( typeof( IEnumerable<> ) ) );

			Debug.WriteLine( "ItemFormatter<{0}>::FormatGenericEnumerableTo( {1}, {2} )", typeof( T ).FullName, item, context );

			var ienumerable =
				typeof( T ).GetInterfaces()
				.First( @interface => @interface.IsGenericType
					&& !@interface.IsGenericTypeDefinition
					&& @interface.Name == typeof( IEnumerable<> ).Name
				);

			var genericArguments = ienumerable.GetGenericArguments();
			Contract.Assert( genericArguments.Length == 1 );

			if ( ItemFormatter.IsRuntimeCheckNeeded( genericArguments[ 0 ] ) )
			{
				FormatNonGenericEnumerableTo( item, context );
			}
			else
			{
				SequenceFormatter.Get<T>( genericArguments[ 0 ] ).FormatTo( item, context );
			}
		}

		private static void FormatNonGenericDictionaryTo( T item, FormattingContext context )
		{
			Contract.Assert( item is IEnumerable );

			Debug.WriteLine( "ItemFormatter<{0}>::FormatNonGenericDictionaryTo( {1}, {2} )", typeof( T ).FullName, item, context );

			context.Buffer.Append( "{ " );
			context.EnterCollection();

			bool isFirstEntry = true;
			foreach ( DictionaryEntry entry in ( item as IEnumerable ) )
			{
				if ( !isFirstEntry )
				{
					context.Buffer.Append( ", " );
				}

				if ( Object.ReferenceEquals( entry.Key, null ) )
				{
					context.Buffer.Append( CommonCustomFormatter.NullRepresentation );
				}
				else
				{
					ItemFormatter.Get( entry.Key.GetType() ).FormatObjectTo( entry.Key, context );
				}

				context.Buffer.Append( " : " );

				if ( Object.ReferenceEquals( entry.Value, null ) )
				{
					context.Buffer.Append( CommonCustomFormatter.NullRepresentation );
				}
				else
				{
					ItemFormatter.Get( entry.Value.GetType() ).FormatObjectTo( entry.Value, context );
				}

				isFirstEntry = false;
			}

			context.LeaveCollection();
			context.Buffer.Append( " }" );
		}

		private static void FormatGenericDictionaryTo( T item, FormattingContext context )
		{
			Contract.Assert( typeof( T ).Implements( typeof( IDictionary<,> ) ) );

			Debug.WriteLine( "ItemFormatter<{0}>::FormatGenericDictionaryTo( {1}, {2} )", typeof( T ).FullName, item, context );

			var idictionary =
				typeof( T ).GetInterfaces()
				.First( @interface => @interface.IsGenericType
					&& !@interface.IsGenericTypeDefinition
					&& @interface.Name == typeof( IDictionary<,> ).Name
				);

			var genericArguments = idictionary.GetGenericArguments();
			Contract.Assert( genericArguments.Length == 2 );

			DictionaryFormatter.Get<T>( genericArguments[ 0 ], genericArguments[ 1 ] ).FormatTo( item, context );
		}

		public GenericItemFormatter() { }

		public override void FormatTo( T item, FormattingContext context )
		{
			Action( ( T )item, context );
		}
	}
}
