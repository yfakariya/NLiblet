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
using System.Globalization;

namespace NLiblet.Text
{
	public abstract class CharEscapingFilter
	{
		private static readonly CharEscapingFilter _null = new NullCharEscapingFilter();

		public static CharEscapingFilter Null
		{
			get { return CharEscapingFilter._null; }
		}

		private static readonly CharEscapingFilter _unicodeStandard = new UnicodeStandardCharEscapingFilter();

		public static CharEscapingFilter UnicodeStandard
		{
			get { return _unicodeStandard; }
		}

		private static readonly CharEscapingFilter _lowerCaseNonAsciiCSharpStyle =
			DefaultCharEscapingFilter.CreateCSharp( allowNonAscii: false, allowLineBreak: true, allowQuotation: true, isUpper: false );

		public static CharEscapingFilter LowerCaseNonAsciiCSharpStyle
		{
			get { return _lowerCaseNonAsciiCSharpStyle; }
		}

		private static readonly CharEscapingFilter _upperCaseNonAsciiCSharpStyle =
			DefaultCharEscapingFilter.CreateCSharp( allowNonAscii : false, allowLineBreak : true, allowQuotation: true, isUpper : true );

		public static CharEscapingFilter UpperCaseNonAsciiCSharpStyle
		{
			get { return _upperCaseNonAsciiCSharpStyle; }
		}

		private static readonly CharEscapingFilter _lowerCaseDefaultCSharpStyle =
			DefaultCharEscapingFilter.CreateCSharp( allowNonAscii : true, allowLineBreak : true, allowQuotation : true, isUpper : false );

		public static CharEscapingFilter LowerCaseDefaultCSharpStyle
		{
			get { return _lowerCaseDefaultCSharpStyle; }
		}

		private static readonly CharEscapingFilter _upperCaseDefaultCSharpStyle =
			DefaultCharEscapingFilter.CreateCSharp( allowNonAscii: true, allowLineBreak: true, allowQuotation: true, isUpper: true );

		public static CharEscapingFilter UpperCaseDefaultCSharpStyle
		{
			get { return _upperCaseDefaultCSharpStyle; }
		}

		private static readonly CharEscapingFilter _lowerCaseDefaultCSharpStyleSingleLine =
			DefaultCharEscapingFilter.CreateCSharp( allowNonAscii : true, allowLineBreak : false, allowQuotation : true, isUpper : false );

		public static CharEscapingFilter LowerCaseDefaultCSharpStyleSingleLine
		{
			get { return _lowerCaseDefaultCSharpStyleSingleLine; }
		}

		private static readonly CharEscapingFilter _upperCaseDefaultCSharpStyleSingleLine =
			DefaultCharEscapingFilter.CreateCSharp( allowNonAscii: true, allowLineBreak: false, allowQuotation: true, isUpper: true );

		public static CharEscapingFilter UpperCaseDefaultCSharpStyleSingleLine
		{
			get { return _upperCaseDefaultCSharpStyleSingleLine; }
		}

		private static readonly CharEscapingFilter _lowerCaseDefaultCSharpLiteralStyle =
			DefaultCharEscapingFilter.CreateCSharp( allowNonAscii : true, allowLineBreak : false, allowQuotation : false, isUpper : false );

		public static CharEscapingFilter LowerCaseDefaultCSharpLiteralStyle
		{
			get { return _lowerCaseDefaultCSharpLiteralStyle; }
		}

		private static readonly CharEscapingFilter _upperCaseDefaultCSharpLiteralStyle =
			DefaultCharEscapingFilter.CreateCSharp( allowNonAscii: true, allowLineBreak: false, allowQuotation: false, isUpper: true );

		public static CharEscapingFilter UpperCaseDefaultCSharpLiteralStyle
		{
			get { return _upperCaseDefaultCSharpLiteralStyle; }
		}


		public IEnumerable<char> Escape( IEnumerable<char> source )
		{
			if ( source == null )
			{
				return null;
			}

			return this.EscapeCore( source );
		}

		public IEnumerable<char> Escape( IEnumerable<int> source )
		{
			if ( source == null )
			{
				return null;
			}

			return this.EscapeCore( source );
		}

		protected abstract IEnumerable<char> EscapeCore( IEnumerable<char> source );
		protected abstract IEnumerable<char> EscapeCore( IEnumerable<int> source );

		private sealed class NullCharEscapingFilter : CharEscapingFilter
		{
			public NullCharEscapingFilter() { }

			protected sealed override IEnumerable<char> EscapeCore( IEnumerable<char> source )
			{
				return source;
			}

			protected override IEnumerable<char> EscapeCore( IEnumerable<int> source )
			{
				foreach ( var c in source )
				{
					if ( c > 0xffff )
					{
						foreach ( var x in Char.ConvertFromUtf32( c ) )
						{
							yield return x;
						}
					}
					else
					{
						yield return unchecked( ( char )c );
					}
				}
			}
		}

		private sealed class UnicodeStandardCharEscapingFilter : CharEscapingFilter
		{
			public UnicodeStandardCharEscapingFilter() { }

			protected sealed override IEnumerable<char> EscapeCore( IEnumerable<char> source )
			{
				char highSurrogate = default( char );
				foreach ( var c in source )
				{
					if ( Char.IsHighSurrogate( c ) )
					{
						if ( highSurrogate == default( char ) )
						{
							highSurrogate = c;
						}
						else
						{
							yield return '\ufffd';

							highSurrogate = c;
						}
					}
					else if ( Char.IsLowSurrogate( c ) )
					{
						if ( highSurrogate == default( char ) )
						{
							yield return '\ufffd';
						}
						else
						{
							yield return highSurrogate;
							yield return c;
						}
					}
					else if ( UnicodeUtility.ShouldEscape( c ) )
					{
						switch ( c )
						{
							case '\t':
							case '\r':
							case '\n':
							{
								// Tab, line breaks should not be escaped.
								yield return c;
								break;
							}
							default:
							{
								yield return '\ufffd'; 
								break;
							}
						}
					}
					else
					{
						yield return c;
					}
				}
			}
			protected sealed override IEnumerable<char> EscapeCore( IEnumerable<int> source )
			{
				foreach ( var c in source )
				{
					if ( UnicodeUtility.ShouldEscape( c ) )
					{
						yield return '\ufffd';
					}
					else
					{
						foreach ( var utf16 in Char.ConvertFromUtf32( c ) )
						{
							yield return utf16;
						}
					}
				}
			}
		}
	}
}
