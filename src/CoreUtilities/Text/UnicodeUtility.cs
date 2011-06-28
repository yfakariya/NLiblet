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
using System.Globalization;
using System.Text;

namespace NLiblet.Text
{
	/// <summary>
	///		Defines utility methods related to unicode charactors handling.
	/// </summary>
	public static partial class UnicodeUtility
	{
		#region -- CombineSurrogatePair --

		/// <summary>
		///		Combine two surrgate pair into single UTF-32 code point.
		/// </summary>
		/// <param name="highSurrogate">High surrogate char of UTF-16.</param>
		/// <param name="lowSurrogate">Low surrogate char of UTF-16.</param>
		/// <returns>UTF-32 code point.</returns>
		public static int CombineSurrogatePair( char highSurrogate, char lowSurrogate )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0xd800 <= highSurrogate );
			Contract.Requires<ArgumentOutOfRangeException>( highSurrogate <= 0xdbff );
			Contract.Requires<ArgumentOutOfRangeException>( 0xdc00 <= lowSurrogate );
			Contract.Requires<ArgumentOutOfRangeException>( lowSurrogate <= 0xdfff );
			Contract.Ensures( 0x10000 <= Contract.Result<int>() );
			Contract.Ensures( Contract.Result<int>() <= 0x10ffff );

			int highSurrogateBits = highSurrogate;
			int lowSurrogateBits = lowSurrogate;
			int codePoint = ( 0x3ff & lowSurrogate );
			codePoint |= ( ( 0x3f & highSurrogate ) << 10 );
			codePoint |= ( ( ( ( 0x3c0 & highSurrogate ) >> 6 ) + 1 ) << 16 );
			return codePoint;
		}

		#endregion

		#region -- IsPrintable --

		/// <summary>
		///		Determine that whether specified charactor is printable.
		/// </summary>
		/// <param name="value">Char to be determined.</param>
		/// <returns><c>true</c> if <paramref name="value"/> is printable; otherwise, <c>false</c>.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/IsPrintable/remarks'/>
		public static bool IsPrintable( char value )
		{
			return IsPrintable( ( int )value );
		}

		/// <summary>
		///		Determine that whether specified surrogate pair is printable.
		/// </summary>
		/// <param name="highSurrogate">High surrogate char.</param>
		/// <param name="lowSurrogate">Low surrogate char.</param>
		/// <returns><c>true</c> if the codepoint represented by specified surrogate pair is printable; otherwise, <c>false</c>.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/IsPrintable/remarks'/>
		public static bool IsPrintable( char highSurrogate, char lowSurrogate )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0xd800 <= highSurrogate );
			Contract.Requires<ArgumentOutOfRangeException>( highSurrogate <= 0xdbff );
			Contract.Requires<ArgumentOutOfRangeException>( 0xdc00 <= lowSurrogate );
			Contract.Requires<ArgumentOutOfRangeException>( lowSurrogate <= 0xdfff );

			return IsPrintable( CombineSurrogatePair( highSurrogate, lowSurrogate ) );
		}

		/// <summary>
		///		Determine that whether the codepoint at the index in specified <see cref="String"/> is printable.
		/// </summary>
		/// <param name="value"><see cref="String"/> holds target charactor.</param>
		/// <param name="index">Index of determining charactor. You can specify the position of high surrogate char for surrogate pair.</param>
		/// <returns><c>true</c> if charactor at <paramref name="index"/> in <paramref name="value"/> is printable; otherwise, <c>false</c>.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/IsPrintable/remarks'/>
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

		/// <summary>
		///		Determine that whether the codepoint at the index in specified <see cref="StringBuilder"/> is printable.
		/// </summary>
		/// <param name="value"><see cref="StringBuilder"/> holds target charactor.</param>
		/// <param name="index">Index of determining charactor. You can specify the position of high surrogate char for surrogate pair.</param>
		/// <returns><c>true</c> if charactor at <paramref name="index"/> in <paramref name="value"/> is printable; otherwise, <c>false</c>.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/IsPrintable/remarks'/>
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

		/// <summary>
		///		Determine that whether the codepoint at the index in specified charactor collection is printable.
		/// </summary>
		/// <param name="chars"><see cref="IList&lt;Char&gt;"></see> holds target charactor.</param>
		/// <param name="index">Index of determining charactor. You can specify the position of high surrogate char for surrogate pair.</param>
		/// <returns><c>true</c> if charactor at <paramref name="index"/> in <paramref name="chars"/> is printable; otherwise, <c>false</c>.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/IsPrintable/remarks'/>
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

		/// <summary>
		///		Determine that whether specified UTF-32 code point is printable.
		/// </summary>
		/// <param name="codePoint">UTF-32 code point to be determined.</param>
		/// <returns><c>true</c> if <paramref name="codePoint"/> is printable; otherwise, <c>false</c>.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/IsPrintable/remarks'/>
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

		/// <summary>
		///		Determine that whether specified charactor should be escaped.
		/// </summary>
		/// <param name="value">Char to be determined.</param>
		/// <returns><c>true</c> if <paramref name="value"/> should be escaped; otherwise, <c>false</c>.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/ShouldEscape/remarks'/>
		public static bool ShouldEscape( char value )
		{
			return ShouldEscape( ( int )value );
		}

		/// <summary>
		///		Determine that whether specified surrogate pair should be escaped.
		/// </summary>
		/// <param name="highSurrogate">High surrogate char.</param>
		/// <param name="lowSurrogate">Low surrogate char.</param>
		/// <returns><c>true</c> if the codepoint represented by specified surrogate pair should be escaped; otherwise, <c>false</c>.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/ShouldEscape/remarks'/>
		public static bool ShouldEscape( char highSurrogate, char lowSurrogate )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0xd800 <= highSurrogate );
			Contract.Requires<ArgumentOutOfRangeException>( highSurrogate <= 0xdbff );
			Contract.Requires<ArgumentOutOfRangeException>( 0xdc00 <= lowSurrogate );
			Contract.Requires<ArgumentOutOfRangeException>( lowSurrogate <= 0xdfff );

			return ShouldEscape( CombineSurrogatePair( highSurrogate, lowSurrogate ) );
		}

		/// <summary>
		///		Determine that whether the codepoint at the index in specified <see cref="String"/> should be escaped.
		/// </summary>
		/// <param name="value"><see cref="String"/> holds target charactor.</param>
		/// <param name="index">Index of determining charactor. You can specify the position of high surrogate char for surrogate pair.</param>
		/// <returns><c>true</c> if charactor at <paramref name="index"/> in <paramref name="value"/> should be escaped; otherwise, <c>false</c>.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/ShouldEscape/remarks'/>
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

		/// <summary>
		///		Determine that whether the codepoint at the index in specified <see cref="StringBuilder"/> should be escaped.
		/// </summary>
		/// <param name="value"><see cref="StringBuilder"/> holds target charactor.</param>
		/// <param name="index">Index of determining charactor. You can specify the position of high surrogate char for surrogate pair.</param>
		/// <returns><c>true</c> if charactor at <paramref name="index"/> in <paramref name="value"/> should be escaped; otherwise, <c>false</c>.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/ShouldEscape/remarks'/>
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

		/// <summary>
		///		Determine that whether the codepoint at the index in specified charactor collection should be escaped.
		/// </summary>
		/// <param name="chars"><see cref="IList&lt;Char&gt;"></see> holds target charactor.</param>
		/// <param name="index">Index of determining charactor. You can specify the position of high surrogate char for surrogate pair.</param>
		/// <returns><c>true</c> if charactor at <paramref name="index"/> in <paramref name="chars"/> should be escaped; otherwise, <c>false</c>.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/ShouldEscape/remarks'/>
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

		/// <summary>
		///		Determine that whether specified UTF-32 code point should be escaped.
		/// </summary>
		/// <param name="codePoint">UTF-32 code point to be determined.</param>
		/// <returns><c>true</c> if <paramref name="codePoint"/> should be escaped; otherwise, <c>false</c>.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/ShouldEscape/remarks'/>
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

		/// <summary>
		///		Converts from specified UTF-32 code point to sequence of <see cref="Char"/>.
		/// </summary>
		/// <param name="utf32">UTF-32 code point.</param>
		/// <returns>Sequence of <see cref="Char"/>.</returns>
		public static IEnumerable<char> ConvertFromUtf32( int utf32 )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= utf32 && utf32 <= 0x10ffff );

			return Char.ConvertFromUtf32( utf32 );
		}

		#endregion

		#region -- GetUnicodeCategory --

		/// <summary>
		///		Get <see cref="UnicodeCategory"/> for specified UTF-32 code point.
		/// </summary>
		/// <param name="utf32">UTF-32 code point.</param>
		/// <returns><see cref="UnicodeCategory"/>.</returns>
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

		/// <summary>
		///		Get unicode block name of specified UTF-16 charctor.
		/// </summary>
		/// <param name="utf16">Charactor.</param>
		/// <returns>Unicode block name.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/GetUnicodeBlockName/remarks'/>
		public static string GetUnicodeBlockName( char utf16 )
		{
			return GetUnicodeBlockName( ( int )utf16 );
		}

		#endregion
	}
}
