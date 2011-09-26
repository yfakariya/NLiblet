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
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using NLiblet.Reflection;
using System.Diagnostics;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Non-generic entrypoint for dictionary formatter.
	/// </summary>
	internal static class DictionaryFormatter
	{
		public static ItemFormatter<T> Get<T>( Type keyType, Type valueType )
		{
			Contract.Assert( typeof( T ).Implements( typeof( IDictionary<,> ) ) );
			Contract.Assert( typeof( T ).IsGenericType && typeof( T ).GetGenericArguments().Length == 2, typeof( T ).GetFullName() );
			Contract.Assert( typeof( T ).GetGenericArguments()[ 0 ] == keyType );
			Contract.Assert( typeof( T ).GetGenericArguments()[ 1 ] == valueType );

			// TODO: caching
			var result = Activator.CreateInstance( typeof( DictionaryFormatter<,,> ).MakeGenericType( typeof( T ), keyType, valueType ) );
			// Due to rewriter bug, use Debug instead of Contract
			Debug.Assert( result is ItemFormatter<T>, String.Format( "{0} is {1}", result == null ? "(null)" : result.GetType().GetFullName(), typeof( ItemFormatter<T> ).GetFullName() ) );
			return result as ItemFormatter<T>;
		}

		public static ItemFormatter Get( Type dictionaryType )
		{
			Contract.Assert(
				dictionaryType.IsClosedTypeOf( typeof( IDictionary<,> ) ) || dictionaryType.Implements( typeof( IDictionary<,> ) ),
				dictionaryType + " is not closed type of IDictionary`2"
			);

			// TODO: Caching.

			var genericArguments = dictionaryType.GetGenericArguments();
			Contract.Assert( genericArguments.Length == 2 );

			return Activator.CreateInstance( typeof( DictionaryFormatter<,,> ).MakeGenericType( dictionaryType, genericArguments[ 0 ], genericArguments[ 1 ] ) ) as ItemFormatter;
		}
	}
}
