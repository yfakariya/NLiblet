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
using System.Globalization;
using NLiblet.Text;

namespace NLiblet.NUnitExtensions
{
	internal static class Difference
	{
		private const int _displayRangeOffset = 4;

		public static string Compare( string expected, string actual )
		{
			if ( expected == null )
			{
				if ( actual == null )
				{
					return null;
				}
				else
				{
					return "Expected is null, but actual is not null.";
				}
			}
			else if ( actual == null )
			{
				return "Expecte is not null, but actual is null.";
			}

			int position;
			if ( !FindFirstDifference( expected, actual, out position ) )
			{
				// OK.
				return null;
			}

			if ( expected.Length != actual.Length )
			{
				int startPosition =
					Math.Min(
						Math.Max( 0, position - _displayRangeOffset ),
						Math.Min( expected.Length, actual.Length )
					);

				return
					String.Format(
						FormatProviders.CurrentCulture,
						"Strings length are different.{0}" +
						"Expected :{1:###,0}{0}" +
						"Actual   :{2:###,0}{0}" +
						"Expected string :{3:e}{0}" +
						"Actual string   :{4:e}{0}" +
						"                 {5}^{6}",
						Environment.NewLine,
						expected.Length,
						actual.Length,
						expected.Length == 0 ? String.Empty : expected.SubstringLoosely( startPosition, 8 ),
						actual.Length == 0 ? String.Empty : actual.SubstringLoosely( startPosition, 8 ),
						( Math.Min( expected.Length, actual.Length ) == 0
							? String.Empty
							: new String( ' ', ( startPosition < _displayRangeOffset ? startPosition : _displayRangeOffset ) + 1 )
						),
						new String( ' ', _displayRangeOffset * 2 - ( startPosition < _displayRangeOffset ? startPosition : _displayRangeOffset ) )
					);
			}
			else
			{
				int displayStart = Math.Max( 0, position - _displayRangeOffset );
				int displayEnd = Math.Min( displayStart + _displayRangeOffset * 2, expected.Length - 1 );

				return
					String.Format(
						FormatProviders.CurrentCulture,
						"Strings are different at index {1}.{0}" +
						"Expected:'{2:e}'(\\u{2:U}, {2:c}, {2:b}){0}" +
						"Actual  :'{3:e}'(\\u{3:U}, {3:c}, {3:b}){0}" +
						"{0}" +
						"Expected string :{4:e}{0}" +
						"Actual string   :{5:e}{0}" +
						"                 {6}^{7}",
						Environment.NewLine,
						position,
						expected[ position ],
						actual[ position ],
						expected.Slice( displayStart, displayEnd ),
						actual.Slice( displayStart, displayEnd ),
						new String( ' ', position < _displayRangeOffset ? position : _displayRangeOffset ),
						new String( ' ', _displayRangeOffset * 2 - ( position < _displayRangeOffset ? position : _displayRangeOffset ) )
					);

			}
		}

		private static bool FindFirstDifference( string expected, string actual, out int position )
		{
			int i = 0;
			for ( ; i < expected.Length && i < actual.Length; i++ )
			{
				if ( expected[ i ] != actual[ i ] )
				{
					position = i;
					return true;
				}
			}

			if ( expected.Length == actual.Length )
			{
				position = -1;
				return false;
			}
			else
			{
				position = i;
				return true;
			}
		}
	}
}
