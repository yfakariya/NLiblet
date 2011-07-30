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

			if ( typeof( object ).TypeHandle.Equals( itemType.TypeHandle ) )
			{
				// Avoid infinite recursion.
				Debug.WriteLine( "ItemFormatter::Get( {0} ) -> {1}", itemType, typeof( ObjectFormatter ) );
				return ObjectFormatter.Instance;
			}

			if ( typeof( DateTimeOffset ).TypeHandle.Equals( itemType.TypeHandle ) )
			{
				Debug.WriteLine( "ItemFormatter::Get( {0} ) -> {1}", itemType, typeof( DateTimeOffsetFormatter ) );
				return DateTimeOffsetFormatter.Instance;
			}

			if ( typeof( DateTime ).TypeHandle.Equals( itemType.TypeHandle ) )
			{
				Debug.WriteLine( "ItemFormatter::Get( {0} ) -> {1}", itemType, typeof( DateTimeFormatter ) );
				return DateTimeFormatter.Instance;
			}

			// TODO: caching
			var result = Activator.CreateInstance( typeof( GenericItemFormatter<> ).MakeGenericType( itemType ) ) as ItemFormatter;
			Debug.WriteLine( "ItemFormatter::Get( {0} ) -> {1}(Action:{2})", itemType.GetFullName(), result.GetType().GetName(), ( result.GetType().GetField( "Action" ).GetValue( null ) as Delegate ).Method );
			return result;
		}

		/// <summary>
		///		Get appropriate formatter.
		/// </summary>
		/// <param name="itemType">Type of item.</param>
		/// <returns>Appropriate formatter.</returns>
		public static ItemFormatter<T> Get<T>()
		{
			Contract.Ensures( Contract.Result<ItemFormatter>() != null );

			return Get( typeof( T ) ) as ItemFormatter<T>;
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

		// FIXME: Should be other class
		internal static IEnumerable<char> EscapeChars( object item )
		{
			// TODO: Consider custom escaping... ?

			Contract.Assert(
				item is string
				|| item is StringBuilder
				|| item is IEnumerable<char>
			);

			var asEnumerable = item as IEnumerable<char>;
			if ( asEnumerable != null )
			{
				return CommonCustomFormatter.CollectionItemFilter.Escape( asEnumerable );
			}

			var asStringBuilder = item as StringBuilder;
			if ( asStringBuilder != null )
			{
				return asStringBuilder.AsEnumerable();
			}

			return Empty.Array<char>();
		}
	}


}
