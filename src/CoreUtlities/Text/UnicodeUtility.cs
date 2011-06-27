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
		public static int CombineSurrogatePair( char highSurrogate, char lowSurrogate )
		{
			throw new NotImplementedException();
		}

		public static bool IsPrintable( char value )
		{
			return IsPrintable( ( int )value );
		}

		public static bool IsPrintable( char highSurrogate, char lowSurrogate )
		{
			return IsPrintable( CombineSurrogatePair( highSurrogate, lowSurrogate ) );
		}

		public static bool IsPrintable( string value, int index )
		{
			if ( value.Length <= index )
			{
				throw new ArgumentOutOfRangeException( "index" );
			}

			char c = value[ index ];
			if ( Char.IsHighSurrogate( c ) )
			{
				if ( value.Length <= index + 1 )
				{
					return false;
				}
			}

			return IsPrintable( c, value[ index + 1 ] );
		}

		public static bool IsPrintable( StringBuilder value, int index )
		{
			if ( value.Length <= index )
			{
				throw new ArgumentOutOfRangeException( "index" );
			}

			char c = value[ index ];
			if ( Char.IsHighSurrogate( c ) )
			{
				if ( value.Length <= index + 1 )
				{
					return false;
				}
			}

			return IsPrintable( c, value[ index + 1 ] );
		}

		public static bool IsPrintable( IList<char> chars, int index )
		{
			if ( chars.Count <= index )
			{
				throw new ArgumentOutOfRangeException( "index" );
			}

			char c = chars[ index ];
			if ( Char.IsHighSurrogate( c ) )
			{
				if ( chars.Count <= index + 1 )
				{
					return false;
				}
			}

			return IsPrintable( c, chars[ index + 1 ] );
		}

		public static bool IsPrintable( int codePoint )
		{
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

		public static bool ShouldEscape( char c )
		{
			return ShouldEscape( ( int )c );
		}

		public static bool ShouldEscape( char highSurrogate, char lowSurrogate )
		{
			return ShouldEscape( CombineSurrogatePair( highSurrogate, lowSurrogate ) );
		}

		public static bool ShouldEscape( string value, int index )
		{
			if ( value.Length <= index )
			{
				throw new ArgumentOutOfRangeException( "index" );
			}

			char c = value[ index ];
			if ( Char.IsHighSurrogate( c ) )
			{
				if ( value.Length <= index + 1 )
				{
					return true;
				}
			}

			return ShouldEscape( c, value[ index + 1 ] );
		}

		public static bool ShouldEscape( StringBuilder value, int index )
		{
			if ( value.Length <= index )
			{
				throw new ArgumentOutOfRangeException( "index" );
			}

			char c = value[ index ];
			if ( Char.IsHighSurrogate( c ) )
			{
				if ( value.Length <= index + 1 )
				{
					return true;
				}
			}

			return ShouldEscape( c, value[ index + 1 ] );
		}

		public static bool ShouldEscape( IList<char> chars, int index )
		{
			if ( chars.Count <= index )
			{
				throw new ArgumentOutOfRangeException( "index" );
			}

			char c = chars[ index ];
			if ( Char.IsHighSurrogate( c ) )
			{
				if ( chars.Count <= index + 1 )
				{
					return true;
				}
			}

			return ShouldEscape( c, chars[ index + 1 ] );
		}

		public static bool ShouldEscape( int codePoint )
		{
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

		public static IEnumerable<char> ConvertFromUtf32( int utf32 )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= utf32 && utf32 <= 0x10ffff );

			return Char.ConvertFromUtf32( utf32 );
		}

		public static UnicodeCategory GetUnicodeCategory( int utf32 )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= utf32 && utf32 <= 0x10ffff );

			if ( 0xffff < utf32 )
			{
#warning TODO: IMPL
				throw new NotImplementedException();
			}

			return CharUnicodeInfo.GetUnicodeCategory( unchecked( ( char )utf32 ) );
		}

		public static string GetUnicodeBlockName( char utf16 )
		{
			return GetUnicodeBlockName( ( int )utf16 );
		}
	}
}
