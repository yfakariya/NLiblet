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
using System.Text;

namespace NLiblet.Text
{
	/// <summary>
	///		Extends <see cref="StringBuilder"/> with extension methods.
	/// </summary>
	public static class StringBuilderExtensions
	{
		/// <summary>
		///		Get <see cref="IEnumerable{Char}"/> to enumerate characters of this <see cref="StringBuilder"/>.
		/// </summary>
		/// <param name="source"><see cref="StringBuilder"/>.</param>
		/// <returns><see cref="IEnumerable{Char}"/> to enumerate characters of <paramref name="source"/>.</returns>
		public static IEnumerable<char> AsEnumerable( this StringBuilder source )
		{
			Contract.Requires<ArgumentNullException>( source != null );
			Contract.Ensures( Contract.Result<IEnumerable<char>>() != null );

			for ( int i = 0; i < source.Length; i++ )
			{
				yield return source[ i ];
			}
		}

		/// <summary>
		///		Append bytes as hexdecimal representation to this <see cref="StringBuilder"/>.
		/// </summary>
		/// <param name="source"><see cref="StringBuilder"/>.</param>
		/// <param name="bytes">Bytes. This value can be null.</param>
		/// <returns><paramref name="source"/> to be used for chaining.</returns>
		public static StringBuilder AppendHex( this StringBuilder source, IEnumerable<byte> bytes )
		{
			Contract.Requires<ArgumentNullException>( source != null );
			Contract.Ensures( Contract.Result<StringBuilder>() != null );
			Contract.Ensures( Object.ReferenceEquals( Contract.Result<StringBuilder>(), source ) );

			return AppendChars( source, HexFormat.ToHex( bytes ) );
		}

		/// <summary>
		///		Append characters to this <see cref="StringBuilder"/>.
		/// </summary>
		/// <param name="source"><see cref="StringBuilder"/>.</param>
		/// <param name="chars">Characters. This value can be null.</param>
		/// <returns><paramref name="source"/> to be used for chaining.</returns>
		public static StringBuilder AppendChars( this StringBuilder source, IEnumerable<char> chars )
		{
			Contract.Requires<ArgumentNullException>( source != null );
			Contract.Ensures( Contract.Result<StringBuilder>() != null );
			Contract.Ensures( Object.ReferenceEquals( Contract.Result<StringBuilder>(), source ) );

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
