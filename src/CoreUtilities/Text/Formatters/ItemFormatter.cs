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
using System.Runtime.CompilerServices;
using System.Text;
using NLiblet.Reflection;
using System.Runtime.Serialization;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Define non-geneneric entry points for item formatting.
	/// </summary>
	internal abstract class ItemFormatter
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
			Contract.Assert( result is IItemFormatter<T>, String.Format( "{0} is {1}", result == null ? "(null)" : result.GetType().GetFullName(), typeof( ItemFormatter<T> ).GetFullName() ) );
			return result as IItemFormatter<T>;
		}

		private static readonly Dictionary<RuntimeTypeHandle, ItemFormatter> _itemFormatters =
			new Dictionary<RuntimeTypeHandle, ItemFormatter>()
			{
				{ typeof( Object ).TypeHandle, PolymorphicObjectFormatter.Instance },
				{ typeof( ValueType ).TypeHandle, PolymorphicObjectFormatter.Instance },
				{ typeof( bool ).TypeHandle, BooleanFormatter.Instance },
				{ typeof( DateTime ).TypeHandle, DateTimeFormatter.Instance },
				{ typeof( DateTimeOffset ).TypeHandle, DateTimeOffsetFormatter.Instance },
				{ typeof( TimeSpan ).TypeHandle, TimeSpanFormatter.Instance },
				{ typeof( String ).TypeHandle, StringFormatter.Instance },
				{ typeof( StringBuilder ).TypeHandle, StringBuilderFormatter.Instance },
				{ typeof( char[] ).TypeHandle, StringFormatter.Instance },
			};

		/// <summary>
		///		Get appropriate formatter.
		/// </summary>
		/// <param name="itemType">Type of item.</param>
		/// <returns>Appropriate formatter.</returns>
		private static ItemFormatter GetCore( Type itemType )
		{
			ItemFormatter result;
			if ( !_itemFormatters.TryGetValue( itemType.TypeHandle, out result )
				&& !TryGetCollectionFormatter( itemType, out result ) )
			{
				// TODO: caching
				result = Activator.CreateInstance( typeof( GenericItemFormatter<> ).MakeGenericType( itemType ) ) as ItemFormatter;
			}

			Debug.WriteLine(
				"ItemFormatter::Get( {0} ) -> {1}(Action:{2})",
				itemType.GetFullName(),
				result.GetType().GetName(),
				( result.GetType().GetField( "Action" ) == null ) ? "n/a" : ( result.GetType().GetField( "Action" ).GetValue( null ) as Delegate ).Method.ToString()
			);

			return result;
		}

		private static bool TryGetCollectionFormatter( Type itemType, out ItemFormatter formatter )
		{
			Debug.WriteLine( "ItemFormatter::TryGetCollectionFormatter( {0} )", itemType );

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
						return true;
					}
					else if ( ienumerableTypeArguments.Any( item => typeof( byte ).TypeHandle.Equals( item.TypeHandle ) ) )
					{
#warning NOT_IMPL
						formatter = null;
						return false;
					}
					else
					{
#warning NOT_IMPL
						formatter = null;
						return false;
					}
				}
			}
			else if ( typeof( IDictionary ).IsAssignableFrom( itemType ) )
			{
#warning NOT_IMPL
				formatter = null;
				return false;
			}
			else if ( typeof( IEnumerable ).IsAssignableFrom( itemType ) )
			{
#warning NOT_IMPL
				formatter = null;
				return false;
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

		public static bool IsRuntimeCheckNeeded( Type type )
		{
			return typeof( object ).TypeHandle.Equals( type.TypeHandle ) || typeof( ValueType ).TypeHandle.Equals( type.TypeHandle );
		}
	}


}
