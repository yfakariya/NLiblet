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
using System.Linq;
using System.Reflection;

using NLiblet.Reflection;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Define non-geneneric entry points for item formatting.
	/// </summary>
	internal abstract partial class ItemFormatter
	{
		/// <summary>
		///		Get appropriate formatter.
		/// </summary>
		/// <param name="itemType">Type of item.</param>
		/// <returns>Appropriate formatter.</returns>
		public static ItemFormatter Get( Type itemType )
		{
			Contract.Requires( itemType != null );
			Contract.Ensures( Contract.Result<ItemFormatter>() != null );

			return GetCore( itemType );
		}

		/// <summary>
		///		Get appropriate formatter.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <returns>Appropriate formatter.</returns>
		public static IItemFormatter<T> Get<T>()
		{
			Contract.Ensures( Contract.Result<IItemFormatter<T>>() != null );

			var result = GetCore( typeof( T ) );
			// Due to rewriter bug, use Debug instead of Contract
			Debug.Assert( result is IItemFormatter<T>, String.Format( "{0} is {1}", result == null ? "(null)" : result.GetType().GetFullName(), typeof( ItemFormatter<T> ).GetFullName() ) );
			return result as IItemFormatter<T>;
		}

		/// <summary>
		///		Get appropriate formatter.
		/// </summary>
		/// <param name="itemType">Type of item.</param>
		/// <returns>Appropriate formatter.</returns>
		private static ItemFormatter GetCore( Type itemType )
		{
			ItemFormatter result;
			if ( !_itemFormatters.TryGetValue( itemType.TypeHandle, out result )
				&& !TryGetNullableFormatter( itemType, out result )
				&& !TryGetArrayFormatter( itemType, out result )
				&& !TryGetTupleFormatter( itemType, out result )
				&& !TryGetFormattableFormatter( itemType, out result )
				&& !TryGetArraySegmentFormatter( itemType, out result )
				&& !TryGetToStringFormatter( itemType, out result )
				&& !TryGetCollectionFormatter( itemType, out result ) )
			{
				// TODO: caching
				result = ObjectFormatter.Instance;
			}

			Debug.WriteLine(
				"ItemFormatter::Get( {0} ) -> {1}(Action:{2})",
				itemType.GetFullName(),
				result.GetType().GetName(),
				( result.GetType().GetField( "Action" ) == null ) ? "n/a" : ( result.GetType().GetField( "Action" ).GetValue( null ) as Delegate ).Method.ToString()
			);

			return result;
		}

		private static bool TryGetNullableFormatter( Type itemType, out ItemFormatter formatter )
		{
			if ( itemType.IsClosedTypeOf( typeof( Nullable<> ) ) )
			{
				formatter = NullableFormatter.Get( itemType );
				Debug.WriteLine( "ItemFormatter::TryGetNullableFormatter( {0} ) -> {1}", itemType.GetFullName(), formatter.GetType().GetFullName() );
				return true;
			}

			formatter = null;
			return false;
		}

		private static bool TryGetArrayFormatter( Type itemType, out ItemFormatter formatter )
		{
			if ( itemType.IsArray )
			{
				formatter = SequenceFormatter.Get( itemType, itemType.GetElementType() );
				Debug.WriteLine( "ItemFormatter::TryGetArrayFormatter( {0} ) -> {1}", itemType.GetFullName(), formatter.GetType().GetFullName() );
				return true;
			}

			formatter = null;
			return false;
		}

		private static bool TryGetTupleFormatter( Type itemType, out ItemFormatter formatter )
		{
			if ( itemType.IsClosedTypeOf( typeof( Tuple<> ) )
				|| itemType.IsClosedTypeOf( typeof( Tuple<,> ) )
				|| itemType.IsClosedTypeOf( typeof( Tuple<,,> ) )
				|| itemType.IsClosedTypeOf( typeof( Tuple<,,,> ) )
				|| itemType.IsClosedTypeOf( typeof( Tuple<,,,,> ) )
				|| itemType.IsClosedTypeOf( typeof( Tuple<,,,,,> ) )
				|| itemType.IsClosedTypeOf( typeof( Tuple<,,,,,,> ) )
				|| itemType.IsClosedTypeOf( typeof( Tuple<,,,,,,,> ) )
			)
			{
				formatter = TupleFormatter.Get( itemType );
				Debug.WriteLine( "ItemFormatter::TryGetTupleFormatter( {0} ) -> {1}", itemType.GetFullName(), formatter.GetType().GetFullName() );
				return true;
			}
			else
			{
				formatter = null;
				return false;
			}
		}

		private static bool TryGetFormattableFormatter( Type itemType, out ItemFormatter formatter )
		{
			if ( typeof( IFormattable ).IsAssignableFrom( itemType ) )
			{
				formatter = FormattableFormatter.Get( itemType );
				Debug.WriteLine( "ItemFormatter::TryGetFormattableFormatter( {0} ) -> {1}", itemType.GetFullName(), formatter.GetType().GetFullName() );
				return true;
			}
			else
			{
				formatter = null;
				return false;
			}
		}

		private static bool TryGetArraySegmentFormatter( Type itemType, out ItemFormatter formatter )
		{
			if ( itemType.IsClosedTypeOf( typeof( ArraySegment<> ) ) )
			{
				formatter = ArraySegmentFormatter.Get( itemType );
				Debug.WriteLine( "ItemFormatter::TryGetArraySegmentFormatter( {0} ) -> {1}", itemType.GetFullName(), formatter.GetType().GetFullName() );
				return true;
			}
			else
			{
				formatter = null;
				return false;
			}
		}

		private static bool TryGetToStringFormatter( Type itemType, out ItemFormatter formatter )
		{
			var toString = itemType.GetMethod( "ToString", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null );
			if ( toString != null
				&& !typeof( object ).TypeHandle.Equals( toString.DeclaringType.TypeHandle ) // NOT desired implementation
				&& !typeof( ValueType ).TypeHandle.Equals( toString.DeclaringType.TypeHandle ) // NOT desired implementation
			)
			{
				formatter = ObjectFormatter.Instance;
				Debug.WriteLine( "ItemFormatter::TryGetToStringFormatter( {0} ) -> {1}", itemType.GetFullName(), formatter.GetType().GetFullName() );
				return true;
			}
			else
			{
				formatter = null;
				return false;
			}
		}

		private static bool TryGetCollectionFormatter( Type itemType, out ItemFormatter formatter )
		{
			if ( itemType.Implements( typeof( IDictionary<,> ) ) )
			{
				formatter = DictionaryFormatter.Get( itemType );
				Debug.WriteLine( "ItemFormatter::TryGetCollectionFormatter( {0} ) -> {1}", itemType.GetFullName(), formatter.GetType().GetFullName() );
				return true;
			}

			if ( itemType.Implements( typeof( IEnumerable<> ) ) )
			{
				if ( typeof( ICollection ).IsAssignableFrom( itemType ) )
				{
					var ienumerableTypeArguments =
						itemType
						.GetInterfaces()
						.Where( item => item.IsGenericType )
						.Where( item => item.IsInterface )
						.Where( item => item.GetGenericTypeDefinition().TypeHandle.Equals( typeof( IEnumerable<> ).TypeHandle ) )
						.Select( item => item.GetGenericArguments()[ 0 ] );

					if ( ienumerableTypeArguments.Any( item => typeof( char ).TypeHandle.Equals( item.TypeHandle ) ) )
					{
						formatter = StringFormatter.Instance;
						Debug.WriteLine( "ItemFormatter::TryGetCollectionFormatter( {0} ) -> {1}", itemType.GetFullName(), formatter.GetType().GetFullName() );
						return true;
					}
					else if ( ienumerableTypeArguments.Any( item => typeof( byte ).TypeHandle.Equals( item.TypeHandle ) ) )
					{
						formatter = BytesFormatter.Instance;
						Debug.WriteLine( "ItemFormatter::TryGetCollectionFormatter( {0} ) -> {1}", itemType.GetFullName(), formatter.GetType().GetFullName() );
						return true;
					}
					else
					{
						formatter = SequenceFormatter.Get( itemType, ienumerableTypeArguments.First() );
						Debug.WriteLine( "ItemFormatter::TryGetCollectionFormatter( {0} ) -> {1}", itemType.GetFullName(), formatter.GetType().GetFullName() );
						return true;
					}
				}
			}
			else if ( typeof( IDictionary ).IsAssignableFrom( itemType ) )
			{
				formatter = NonGenericDictionaryFormatter.Instance;
				Debug.WriteLine( "ItemFormatter::TryGetCollectionFormatter( {0} ) -> {1}", itemType.GetFullName(), formatter.GetType().GetFullName() );
				return true;
			}
			else if ( typeof( IEnumerable ).IsAssignableFrom( itemType ) )
			{
				formatter = NonGenericSequenceFormatter.Instance;
				Debug.WriteLine( "ItemFormatter::TryGetCollectionFormatter( {0} ) -> {1}", itemType.GetFullName(), formatter.GetType().GetFullName() );
				return true;
			}

			formatter = null;
			return false;
		}

		/// <summary>
		///		Format specified item using context.
		/// </summary>
		/// <param name="item">Item to be formatted.</param>
		/// <param name="context">Context information.</param>
		public abstract void FormatObjectTo( object item, FormattingContext context );
	}
}
