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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text;
using NLiblet.Properties;

namespace NLiblet.Text.Formatters
{
	/*
	 * format
	 *	a : ascii; non-ascii chars are escaped
	 *	b : Unicode block name
	 *	c : Unicode Category
	 *	d : utf-32 Decimal
	 *	e : Escaping non printable chars with U+FFFD
	 *  g : General; same as 'm'
	 *	l : Literal style
	 *	m : Multi line escaped char with \uxxxx notation
	 *	r : Raw-char without any escaping
	 *	s : Single line escaped char with \uxxxx notation
	 *	u : Treat integer as utf-32 hex, a-f will be lowercase, or utf-16 hex with 0 padding and a-f will be lowercase.
	 *	u : Treat integer as utf-32 hex, a-f will be lowercase, or utf-16 hex with 0 padding and a-f will be uppercase.
	 *	x : utf-16 heX, a-f will be lowercase
	 *	X : utf-16 heX, a-f will be uppercase
	 *	
	 * Collection items are always escaped with 'L'
	 */

	/// <summary>
	///		Implementing <see cref="ICustomFormatter"/> and <see cref="IFormatProvider"/>.
	/// </summary>
	internal sealed class CommonCustomFormatter : ICustomFormatter, IFormatProvider
	{
		private readonly IFormatProvider _defaultFormatProvider;

		internal IFormatProvider DefaultFormatProvider
		{
			get { return this._defaultFormatProvider; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="CommonCustomFormatter"/> class.
		/// </summary>
		/// <param name="defaultFormatProvider">Format provider to format <see cref="IFormattable"/> items.</param>
		public CommonCustomFormatter( IFormatProvider defaultFormatProvider )
		{
			Contract.Requires( defaultFormatProvider != null );

			this._defaultFormatProvider = defaultFormatProvider;
		}

		object IFormatProvider.GetFormat( Type formatType )
		{
			if ( typeof( ICustomFormatter ) == formatType )
			{
				return this;
			}
			else
			{
				return this._defaultFormatProvider.GetFormat( formatType );
			}
		}

		public string Format( string format, object arg, IFormatProvider formatProvider )
		{
			if ( Object.ReferenceEquals( arg, null ) )
			{
				return String.Empty;
			}

			if ( arg is char )
			{
				return FormatChar( format, ( char )arg );
			}

			var asString = arg as string;
			if ( asString != null )
			{
				return String.Join( String.Empty, GetCharEscapingFilter( format ).Escape( asString ) );
			}

			var asStringBuilder = arg as StringBuilder;
			if ( asStringBuilder != null )
			{
				return String.Join( String.Empty, GetCharEscapingFilter( format ).Escape( asStringBuilder.AsEnumerable() ) );
			}

			if ( ( format ?? String.Empty ).Length > 1 && ( format[ 0 ] == 'u' || format[ 0 ] == 'U' ) && ( arg is Int32 ) )
			{
				var asInt32 = ( int )arg;
				if ( 0 <= asInt32 && asInt32 <= 0x10FFFF )
				{
					return FormatUtf32Char( format.Substring( 1 ), asInt32 );
				}
				else
				{
					throw new FormatException( String.Format( CultureInfo.CurrentCulture, Properties.Resources.Error_InvalidCodePoint, asInt32 ) );
				}
			}

			var buffer = new StringBuilder();
			ItemFormatter.Get( arg.GetType() ).FormatObjectTo( arg, new FormattingContext( this, format, buffer ) );
			return buffer.ToString();
		}


		private static CharEscapingFilter GetCharEscapingFilter( string format )
		{
			if ( String.IsNullOrWhiteSpace( format ) )
			{
				// Avoid normal placeholders are escaped.
				return CharEscapingFilter.Null;
			}

			switch ( format )
			{
				case "a":
				{
					return CharEscapingFilter.LowerCaseNonAsciiCSharpStyle;
				}
				case "A":
				{
					return CharEscapingFilter.UpperCaseNonAsciiCSharpStyle;
				}
				case "e":
				case "E":
				{
					return CharEscapingFilter.UnicodeStandard;
				}
				case "g":
				{
					goto case "m";
				}
				case "G":
				{
					goto case "M";
				}
				case "l":
				{
					return CharEscapingFilter.LowerCaseDefaultCSharpLiteralStyle;
				}
				case "L":
				{
					return CharEscapingFilter.UpperCaseDefaultCSharpLiteralStyle;
				}
				case "m":
				{
					return CharEscapingFilter.LowerCaseDefaultCSharpStyle;
				}
				case "M":
				{
					return CharEscapingFilter.UpperCaseDefaultCSharpStyle;
				}
				case "r":
				case "R":
				{
					return CharEscapingFilter.Null;
				}
				case "s":
				{
					return CharEscapingFilter.LowerCaseDefaultCSharpStyleSingleLine;
				}
				case "S":
				{
					return CharEscapingFilter.UpperCaseDefaultCSharpStyleSingleLine;
				}
				default:
				{
					throw new FormatException( String.Format( CultureInfo.CurrentCulture, Resources.Formatter_UnknownFormat, format ) );
				}
			}
		}

		private static string FormatUtf32Char( string format, int c )
		{
			switch ( format ?? String.Empty )
			{
				case "b":
				case "B":
				{
					return UnicodeUtility.GetUnicodeBlockName( c );
				}
				case "c":
				case "C":
				{
					return UnicodeUtility.GetUnicodeCategory( c ).ToString();
				}
				case "d":
				case "D":
				{
					return ( c ).ToString( "d", CultureInfo.InvariantCulture );
				}
				case "u":
				{
					return ( c ).ToString( "x4", CultureInfo.InvariantCulture );
				}
				case "U":
				{
					return ( c ).ToString( "X4", CultureInfo.InvariantCulture );
				}
				case "x":
				{
					return ( c ).ToString( "x", CultureInfo.InvariantCulture );
				}
				case "X":
				{
					return ( c ).ToString( "X", CultureInfo.InvariantCulture );
				}
			}

			return String.Join( String.Empty, GetCharEscapingFilter( format ).Escape( Enumerable.Repeat( c, 1 ) ) );
		}

		private static string FormatChar( string format, char c )
		{
			switch ( format ?? String.Empty )
			{
				case "b":
				case "B":
				{
					return UnicodeUtility.GetUnicodeBlockName( c );
				}
				case "c":
				case "C":
				{
					return CharUnicodeInfo.GetUnicodeCategory( c ).ToString();
				}
				case "d":
				case "D":
				{
					return ( ( int )c ).ToString( "d", CultureInfo.InvariantCulture );
				}
				case "u":
				{
					return ( ( int )c ).ToString( "x4", CultureInfo.InvariantCulture );
				}
				case "x":
				{
					return ( ( int )c ).ToString( "x", CultureInfo.InvariantCulture );
				}
				case "U":
				{
					return ( ( int )c ).ToString( "X4", CultureInfo.InvariantCulture );
				}
				case "X":
				{
					return ( ( int )c ).ToString( "X", CultureInfo.InvariantCulture );
				}
				default:
				{
					return String.Join( String.Empty, GetCharEscapingFilter( format ).Escape( Enumerable.Repeat( c, 1 ) ) );
				}
			}
		}
	}
}