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
using System.Linq;

namespace NLiblet.Text
{
	/// <summary>
	///		Text format utilities for hexadecimal binary text representation.
	/// </summary>
	public static class HexFormat
	{
		/// <summary>
		///		Returns hexadecimal text representation of specified bytes.
		/// </summary>
		/// <param name="bytes">Bytes to be string. This value can be null.</param>
		/// <returns>
		///		Hexadecimal text representation of specified <paramref name="bytes"/>;
		///		or empty string when <paramref name="bytes"/> is null or empty.
		/// </returns>
		[Pure]
		public static string ToHexString( IEnumerable<byte> bytes )
		{
			Contract.Ensures( Contract.Result<string>() != null );

			return String.Join( String.Empty, ToHex( bytes ) );
		}

		/// <summary>
		///		Returns hexadecimal text representation of specified bytes.
		/// </summary>
		/// <param name="bytes">Bytes to be string. This value can be null.</param>
		/// <returns>
		///		Charactor sequence of hexadecimal text representation of specified <paramref name="bytes"/>;
		///		or empty sequence when <paramref name="bytes"/> is null or empty.
		/// </returns>
		[Pure]
		public static IEnumerable<char> ToHex( IEnumerable<byte> bytes )
		{
			// TODO: casing
			Contract.Ensures( Contract.Result<IEnumerable<char>>() != null );

			if ( bytes != null )
			{
				foreach ( var b in bytes )
				{
					yield return ToHexChar( unchecked( ( byte )( b >> 4 ) ) );
					yield return ToHexChar( unchecked( ( byte )( b & 0xf ) ) );
				}
			}
		}

		private static char ToHexChar( byte b )
		{
			Contract.Assert( b < 0x10 );

			if ( b < 0xa )
			{
				return unchecked( ( char )( '0' + b ) );
			}
			else
			{
				return unchecked( ( char )( 'a' + ( b - 0xa ) ) );
			}
		}

		/// <summary>
		///		Returns binary representation of specified hexadicimal format text.
		/// </summary>
		/// <param name="hexChars">Chars to be binary. This value can be null.</param>
		/// <returns>
		///		Byte sequence of binary representation of specified <paramref name="hexChars"/>;
		///		or empty sequence when <paramref name="hexChars"/> is null or empty.
		/// </returns>
		/// <exception cref="FormatException">
		///		<paramref name="hexChars"/> contains invalid charactor. Valid charactor is ASCII numbers or ACSII 'a', 'b', 'c', 'd', 'e', or 'f' (cases are insensitive).
		/// </exception>
		[Pure]
		public static IEnumerable<byte> GetBytesFromHex( IEnumerable<char> hexChars )
		{
			Contract.Ensures( Contract.Result<IEnumerable<byte>>() != null );

			if ( hexChars != null )
			{
				ulong counter = 0;
				byte leading = 0;
				foreach ( char c in hexChars )
				{
					if ( '0' <= c && c <= '9' )
					{
						if ( counter % 2 == 0 )
						{
							leading = unchecked( ( byte )( c - '0' ) );
						}
						else
						{
							yield return unchecked( ( byte )( ( leading << 4 ) | ( byte )( c - '0' ) ) );
						}
					}
					else if ( 'A' <= c && c <= 'F' )
					{
						if ( counter % 2 == 0 )
						{
							leading = unchecked( ( byte )( ( c - 'A' ) + 0xa ) );
						}
						else
						{
							yield return unchecked( ( byte )( ( leading << 4 ) | ( byte )( ( c - 'A' ) + 0xa ) ) );
						}
					}
					else if ( 'a' <= c && c <= 'f' )
					{
						if ( counter % 2 == 0 )
						{
							leading = unchecked( ( byte )( ( c - 'a' ) + 0xa ) );
						}
						else
						{
							yield return unchecked( ( byte )( ( leading << 4 ) | ( byte )( ( c - 'a' ) + 0xa ) ) );
						}
					}
					else
					{
						throw new FormatException(
							String.Format(
								FormatProviders.CurrentCulture,
								"Character '{0:m}'(\\u{0:x}, {0:c}) at index {1:###,0} is invalid as hexadecimal.",
								c,
								counter
							)
						);
					}

					counter++;
				}// foreach

				if ( counter % 2 != 0 )
				{
					throw new FormatException(
						String.Format(
							"Input sequence is end unexpctedly at position {0:###,0}.",
							counter
						)
					);
				}
			}
		}

		/// <summary>
		///		Returns byte array for specified hexadicimal format text.
		/// </summary>
		/// <param name="hexChars">Chars to be binary. This value can be null.</param>
		/// <returns>
		///		Byte array of binary representation of specified <paramref name="hexChars"/>;
		///		or empty sequence when <paramref name="hexChars"/> is null or empty.
		/// </returns>
		/// <exception cref="FormatException">
		///		<paramref name="hexChars"/> contains invalid charactor. Valid charactor is ASCII numbers or ACSII 'a', 'b', 'c', 'd', 'e', or 'f' (cases are insensitive).
		/// </exception>
		[Pure]
		public static byte[] GetByteArrayFromHex( IEnumerable<char> hexChars )
		{
			Contract.Ensures( Contract.Result<byte[]>() != null );

			return GetBytesFromHex( hexChars ).ToArray();
		}
	}
}
