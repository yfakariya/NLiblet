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

namespace NLiblet.Text
{
	public sealed class DefaultCharEscapingFilter : CharEscapingFilter
	{
		public static DefaultCharEscapingFilter CreateCSharp( bool allowNonAscii, bool allowLineBreak, bool allowQuotation, bool isUpper )
		{
			return
				new DefaultCharEscapingFilter(
					allowNonAscii,
					allowLineBreak,
					allowQuotation,
					isUpper,
					'\\',
					'"',
					'u',
					'U',
					EscapingSequence.CSharp,
					EscapingSequence.CSharpLinebreak
				);
		}

		public static DefaultCharEscapingFilter CreatePowerShell( bool allowNonAscii, bool allowLineBreak, bool allowQuotation, bool isUpper )
		{
			return
				new DefaultCharEscapingFilter(
					allowNonAscii,
					allowLineBreak,
					allowQuotation,
					isUpper,
					'`',
					'"',
					'u',
					'U',
					EscapingSequence.PowerShell,
					EscapingSequence.PowerShellLinebreak
				);
		}
		private readonly bool _allowNonAscii;
		private readonly bool _allowLineBreak;
		private readonly bool _allowQuotation;
		private readonly string _hexIndicator4;
		private readonly IDictionary<char, char> _escapingSeqences;
		private readonly IDictionary<char, char> _lineBreakEscapingSequences;
		private readonly char _escapingIndicator;
		private readonly char _qutation;
		private readonly char _utf16Indicator;
		private readonly char _utf16SurrogatesIndicator;

		public DefaultCharEscapingFilter(
			bool allowNonAscii,
			bool allowLineBreak,
			bool allowQuotation,
			bool isUpper,
			char escapingIndicator,
			char quotation,
			char utf16Indicator,
			char utf16SurrogatesIndicator,
			Dictionary<char, char> escapingSeqences,
			Dictionary<char, char> lineBreakEscapingSequences
		)
		{
			Contract.Requires<ArgumentNullException>( escapingSeqences != null );
			Contract.Requires<ArgumentNullException>( lineBreakEscapingSequences != null );

			this._allowQuotation = allowQuotation;
			this._allowLineBreak = allowLineBreak;
			this._allowNonAscii = allowNonAscii;
			this._hexIndicator4 = isUpper ? "X4" : "x4";
			this._escapingIndicator = escapingIndicator;
			this._qutation = quotation;
			this._utf16Indicator = utf16Indicator;
			this._utf16SurrogatesIndicator = utf16SurrogatesIndicator;
			this._escapingSeqences = escapingSeqences;
			this._lineBreakEscapingSequences = lineBreakEscapingSequences;
		}

		protected sealed override IEnumerable<char> EscapeCore( IEnumerable<char> source )
		{
			char highSurrogate = default( char );
			foreach ( var c in source )
			{
				char escaped;
				if ( this._escapingSeqences.TryGetValue( c, out escaped ) )
				{
					yield return this._escapingIndicator;
					yield return escaped;
				}
				else if ( !this._allowLineBreak && this._lineBreakEscapingSequences.TryGetValue( c, out escaped ) )
				{
					yield return this._escapingIndicator;
					yield return escaped;
				}
				else if ( !this._allowQuotation && c == this._qutation )
				{
					yield return this._escapingIndicator;
					yield return c;
				}
				else if ( Char.IsHighSurrogate( c ) )
				{
					if ( highSurrogate == default( char ) )
					{
						highSurrogate = c;
					}
					else
					{
						yield return this._escapingIndicator;
						yield return 'u';

						foreach ( var x in "\\u" + ( ( ushort )highSurrogate ).ToString( this._hexIndicator4 ) )
						{
							yield return x;
						}

						highSurrogate = c;
					}
				}
				else if ( Char.IsLowSurrogate( c ) )
				{
					if ( highSurrogate == default( char ) )
					{
						foreach ( var x in "\\u" + ( ( ushort )c ).ToString( this._hexIndicator4 ) )
						{
							yield return x;
						}
					}
					else
					{
						yield return '\\';
						yield return 'U';

						foreach ( var x in ( ( ushort )highSurrogate ).ToString( this._hexIndicator4 ) )
						{
							yield return x;
						}

						foreach ( var x in ( ( ushort )c ).ToString( this._hexIndicator4 ) )
						{
							yield return x;
						}
					}
				}
				else if ( ( !this._allowNonAscii && 0x7f < c )
					|| UnicodeUtility.ShouldEscape( c ) )
				{
					if ( this._allowLineBreak
						&& ( c == '\r' || c == '\n' ) )
					{
						yield return c;
						continue;
					}

					yield return '\\';
					yield return 'u';

					foreach ( var x in ( ( ushort )c ).ToString( this._hexIndicator4 ) )
					{
						yield return x;
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
				char escaped;
				if ( c <= 0xffff )
				{
					if ( this._escapingSeqences.TryGetValue( ( char )c, out escaped ) )
					{
						yield return this._escapingIndicator;
						yield return escaped;
					}
					else if ( !this._allowLineBreak && this._lineBreakEscapingSequences.TryGetValue( ( char )c, out escaped ) )
					{
						yield return this._escapingIndicator;
						yield return escaped;
					}
					else if ( !this._allowQuotation && c == this._qutation )
					{
						yield return this._escapingIndicator;
						yield return unchecked( ( char )c );
					}
				}
				else if ( !this._allowNonAscii
					|| UnicodeUtility.ShouldEscape( c ) )
				{
					yield return '\\';
					if ( c <= 0xffff )
					{
						yield return 'u';

						foreach ( var x in ( c ).ToString( this._hexIndicator4 ) )
						{
							yield return x;
						}
					}
					else
					{
						yield return 'U';

						// TODO: Allow raw codepoint?
						foreach ( var utf16 in Char.ConvertFromUtf32( c ) )
						{
							foreach ( var x in ( ( int )utf16 ).ToString( this._hexIndicator4 ) )
							{
								yield return x;
							}
						}
					}
				}
				else
				{
					foreach ( var x in Char.ConvertFromUtf32( c ) )
					{
						yield return x;
					}
				}
			}
		}
	}
}
