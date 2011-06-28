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
using System.Globalization;
using System.Diagnostics.Contracts;
using System.Collections;

namespace NLiblet.Text
{
	public static partial class UnicodeUtility
	{
		#region -- CombineSurrogatePair --

		public static int CombineSurrogatePair( char highSurrogate, char lowSurrogate )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0xd800 <= highSurrogate );
			Contract.Requires<ArgumentOutOfRangeException>( highSurrogate <= 0xdbff );
			Contract.Requires<ArgumentOutOfRangeException>( 0xdc00 <= lowSurrogate );
			Contract.Requires<ArgumentOutOfRangeException>( lowSurrogate <= 0xdfff );

			int highSurrogateBits = highSurrogate;
			int lowSurrogateBits = lowSurrogate;
			int codePoint = ( 0x3ff & lowSurrogate );
			codePoint |= ( ( 0x3f & highSurrogate ) << 10 );
			codePoint |= ( ( ( ( 0x3c0 & highSurrogate ) >> 6 ) + 1 ) << 16 );
			return codePoint;
		}

		#endregion

		#region -- IsPrintable --

		public static bool IsPrintable( char value )
		{
			return IsPrintable( ( int )value );
		}

		public static bool IsPrintable( char highSurrogate, char lowSurrogate )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0xd800 <= highSurrogate );
			Contract.Requires<ArgumentOutOfRangeException>( highSurrogate <= 0xdbff );
			Contract.Requires<ArgumentOutOfRangeException>( 0xdc00 <= lowSurrogate );
			Contract.Requires<ArgumentOutOfRangeException>( lowSurrogate <= 0xdfff );

			return IsPrintable( CombineSurrogatePair( highSurrogate, lowSurrogate ) );
		}

		public static bool IsPrintable( string value, int index )
		{
			Contract.Requires<ArgumentNullException>( value != null );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= index );
			Contract.Requires<ArgumentOutOfRangeException>( index < value.Length );

			char c = value[ index ];
			if ( Char.IsHighSurrogate( c ) )
			{
				if ( value.Length <= index + 1 )
				{
					return false;
				}

				return IsPrintable( c, value[ index + 1 ] );
			}
			else
			{
				return IsPrintable( c );
			}
		}

		public static bool IsPrintable( StringBuilder value, int index )
		{
			Contract.Requires<ArgumentNullException>( value != null );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= index );
			Contract.Requires<ArgumentOutOfRangeException>( index < value.Length );

			char c = value[ index ];
			if ( Char.IsHighSurrogate( c ) )
			{
				if ( value.Length <= index + 1 )
				{
					return false;
				}

				return IsPrintable( c, value[ index + 1 ] );
			}
			else
			{
				return IsPrintable( c );
			}
		}

		public static bool IsPrintable( IList<char> chars, int index )
		{
			Contract.Requires<ArgumentNullException>( chars != null );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= index );
			Contract.Requires<ArgumentOutOfRangeException>( index < chars.Count );

			char c = chars[ index ];
			if ( Char.IsHighSurrogate( c ) )
			{
				if ( chars.Count <= index + 1 )
				{
					return false;
				}

				return IsPrintable( c, chars[ index + 1 ] );
			}
			else
			{
				return IsPrintable( c );
			}
		}

		public static bool IsPrintable( int codePoint )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= codePoint );
			Contract.Requires<ArgumentOutOfRangeException>( codePoint <= 0x10ffff );

			if ( codePoint <= 0xffff )
			{
				return IsPrintable( CharUnicodeInfo.GetUnicodeCategory( ( char )codePoint ) );
			}
			else
			{
				return IsPrintable( CharUnicodeInfo.GetUnicodeCategory( Char.ConvertFromUtf32( codePoint ), 0 ) );
			}
		}

		private static bool IsPrintable( UnicodeCategory category )
		{
			switch ( category )
			{
				case UnicodeCategory.Control:
				case UnicodeCategory.OtherNotAssigned:
				case UnicodeCategory.LineSeparator:
				case UnicodeCategory.SpaceSeparator:
				case UnicodeCategory.ParagraphSeparator:
				case UnicodeCategory.Surrogate:
				{
					return false;
				}
				default:
				{
					// Modifiers are may be printable.
					return true;
				}
			}
		}

		#endregion

		#region -- ShouldEscape --

		public static bool ShouldEscape( char c )
		{
			return ShouldEscape( ( int )c );
		}

		public static bool ShouldEscape( char highSurrogate, char lowSurrogate )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0xd800 <= highSurrogate );
			Contract.Requires<ArgumentOutOfRangeException>( highSurrogate <= 0xdbff );
			Contract.Requires<ArgumentOutOfRangeException>( 0xdc00 <= lowSurrogate );
			Contract.Requires<ArgumentOutOfRangeException>( lowSurrogate <= 0xdfff );

			return ShouldEscape( CombineSurrogatePair( highSurrogate, lowSurrogate ) );
		}

		public static bool ShouldEscape( string value, int index )
		{
			Contract.Requires<ArgumentNullException>( value != null );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= index );
			Contract.Requires<ArgumentOutOfRangeException>( index < value.Length );

			char c = value[ index ];
			if ( Char.IsHighSurrogate( c ) )
			{
				if ( value.Length <= index + 1 )
				{
					return true;
				}

				return ShouldEscape( c, value[ index + 1 ] );
			}
			else
			{
				return ShouldEscape( c );
			}
		}

		public static bool ShouldEscape( StringBuilder value, int index )
		{
			Contract.Requires<ArgumentNullException>( value != null );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= index );
			Contract.Requires<ArgumentOutOfRangeException>( index < value.Length );

			char c = value[ index ];
			if ( Char.IsHighSurrogate( c ) )
			{
				if ( value.Length <= index + 1 )
				{
					return true;
				}

				return ShouldEscape( c, value[ index + 1 ] );
			}
			else
			{
				return ShouldEscape( c );
			}
		}

		public static bool ShouldEscape( IList<char> chars, int index )
		{
			Contract.Requires<ArgumentNullException>( chars != null );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= index );
			Contract.Requires<ArgumentOutOfRangeException>( index < chars.Count );

			char c = chars[ index ];
			if ( Char.IsHighSurrogate( c ) )
			{
				if ( chars.Count <= index + 1 )
				{
					return true;
				}

				return ShouldEscape( c, chars[ index + 1 ] );
			}
			else
			{
				return ShouldEscape( c );
			}
		}

		public static bool ShouldEscape( int codePoint )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= codePoint );
			Contract.Requires<ArgumentOutOfRangeException>( codePoint <= 0x10ffff );

			if ( codePoint <= 0xffff )
			{
				return ShouldEscape( CharUnicodeInfo.GetUnicodeCategory( ( char )codePoint ) );
			}
			else
			{
				return ShouldEscape( CharUnicodeInfo.GetUnicodeCategory( Char.ConvertFromUtf32( codePoint ), 0 ) );
			}
		}

		private static bool ShouldEscape( UnicodeCategory category )
		{
			switch ( category )
			{
				case UnicodeCategory.Control: // In-doubt
				case UnicodeCategory.OtherNotAssigned:
				case UnicodeCategory.Surrogate:
				{
					return true;
				}
				default:
				{
					// Modifiers are may be printable.
					return false;
				}
			}
		}

		#endregion

		#region -- ConvertFromUtf32 --

		public static IEnumerable<char> ConvertFromUtf32( int utf32 )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= utf32 && utf32 <= 0x10ffff );

			return Char.ConvertFromUtf32( utf32 );
		}

		#endregion

		#region -- GetUnicodeCategory --

		public static UnicodeCategory GetUnicodeCategory( int utf32 )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= utf32 && utf32 <= 0x10ffff );

			if ( 0xffff < utf32 )
			{
				return CharUnicodeInfo.GetUnicodeCategory( Char.ConvertFromUtf32( utf32 ), 0 );
			}

			return CharUnicodeInfo.GetUnicodeCategory( unchecked( ( char )utf32 ) );
		}

		#endregion

		#region -- GetUnicodeBlockName --

		public static string GetUnicodeBlockName( char utf16 )
		{
			return GetUnicodeBlockName( ( int )utf16 );
		}

		#endregion
	}
}
