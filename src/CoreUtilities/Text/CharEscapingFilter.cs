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


		private static readonly CharEscapingFilter _nonAsciiCSharpStyle =
			DefaultCharEscapingFilter.CreateCSharp( false, true, true );

		public static CharEscapingFilter NonAsciiCSharpStyle
		{
			get { return _nonAsciiCSharpStyle; }
		}

		private static readonly CharEscapingFilter _defaultCSharpStyle =
			DefaultCharEscapingFilter.CreateCSharp( true, true, true );

		public static CharEscapingFilter DefaultCSharpStyle
		{
			get { return _defaultCSharpStyle; }
		}

		private static readonly CharEscapingFilter _defaultCSharpStyleSingleLine =
			DefaultCharEscapingFilter.CreateCSharp( true, false, true );

		public static CharEscapingFilter DefaultCSharpStyleSingleLine
		{
			get { return _defaultCSharpStyleSingleLine; }
		}

		private static readonly CharEscapingFilter _defaultCSharpLiteralStyle =
			DefaultCharEscapingFilter.CreateCSharp( true, false, false );

		public static CharEscapingFilter DefaultCSharpLiteralStyle
		{
			get { return _defaultCSharpLiteralStyle; }
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
						foreach ( var x in UnicodeUtility.ConvertFromUtf32( c ) )
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
						yield return '\ufffd';
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
					else if ( 0xffff < c )
					{
						yield return '\\';
						yield return 'U';
						// TODO: manually
						foreach ( var x in c.ToString( "X8" ) )
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
	}
}
