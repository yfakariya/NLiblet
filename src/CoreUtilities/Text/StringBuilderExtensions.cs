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
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;

namespace NLiblet.Text
{
#warning IMPL
	public static class StringBuilderExtensions
	{
		public static IEnumerable<char> EnumerateChars( this StringBuilder source )
		{
			Contract.Requires<ArgumentNullException>( source != null );

			for ( int i = 0; i < source.Length; i++ )
			{
				yield return source[ i ];
			}
		}

		public static StringBuilder AppendHex( this StringBuilder source, IEnumerable<byte> bytes )
		{
			Contract.Requires<ArgumentNullException>( source != null );

			return Append( source, HexFormat.ToHex( bytes ) );
		}

		public static StringBuilder Append( this StringBuilder source, IEnumerable<char> chars )
		{
			Contract.Requires<ArgumentNullException>( source != null );

			if ( chars != null )
			{
				var asCollection = chars as ICollection<char>;
				if ( asCollection != null && source.Capacity < source.Length + asCollection.Count )
				{
					source.Capacity = source.Length + asCollection.Count;
				}

				foreach ( var c in chars )
				{
					source.Append( c );
				}
			}

			return source;
		}
	}
}
