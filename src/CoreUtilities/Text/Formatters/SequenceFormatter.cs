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

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Non-generic entrypoint for sequence formatter.
	/// </summary>
	internal static class SequenceFormatter
	{
		public static ItemFormatter<T> Get<T>( Type itemType )
		{
			Contract.Assert( typeof( T ).Implements( typeof( IEnumerable<> ) ) );
			Contract.Assert( typeof( T ).IsGenericType && typeof( T ).GetGenericArguments().Length == 1, typeof( T ).GetFullName() );
			Contract.Assert( typeof( T ).GetGenericArguments()[ 0 ] == itemType );

			// TODO: caching
			return Activator.CreateInstance( typeof( SequenceFormatter<,> ).MakeGenericType( typeof( T ), itemType ) ) as ItemFormatter<T>;
		}

		internal static ItemFormatter Get( Type sequenceType, Type itemType )
		{
			Contract.Assert( sequenceType.Implements( typeof( IEnumerable<> ) ) );

			// TODO: caching
			return Activator.CreateInstance( typeof( SequenceFormatter<,> ).MakeGenericType( sequenceType, itemType ) ) as ItemFormatter;
		}
	}
}
