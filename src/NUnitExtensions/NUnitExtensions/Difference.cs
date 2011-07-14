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
		internal const int DisplayRangeOffset = 10;
		private const string _ellipsis = "..";

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

			int differentPosition;
			if ( !FindFirstDifference( expected, actual, out differentPosition ) )
			{
				// OK.
				return null;
			}

			if ( expected.Length != actual.Length )
			{
				int displayStartAt = Math.Max( 0, differentPosition - DisplayRangeOffset );

				bool hasLeadingEllipsis = ( DisplayRangeOffset < differentPosition );
				bool hasFollowingEllipsis = ( DisplayRangeOffset < Math.Abs( expected.Length - actual.Length ) );

				return
					String.Format(
						FormatProviders.CurrentCulture,
						"Strings length are different.{0}" +
						"Expected :{1:###,0}{0}" +
						"Actual   :{2:###,0}{0}" +
						"Expected string :{6}{3:e}{7}{0}" +
						"Actual string   :{8}{4:e}{9}{0}" +
						"                 {5}^",
						Environment.NewLine,
						expected.Length,
						actual.Length,
						expected.Length == 0 ? String.Empty : expected.SubstringLoosely( displayStartAt, DisplayRangeOffset + 1 ),
						actual.Length == 0 ? String.Empty : actual.SubstringLoosely( displayStartAt, DisplayRangeOffset + 1 ),
						( differentPosition == 0
							? String.Empty
							: new String( ' ',
								( ( differentPosition < DisplayRangeOffset ) ? differentPosition : DisplayRangeOffset ) +
								( hasLeadingEllipsis ? _ellipsis.Length : 0 ) )
						),
						hasLeadingEllipsis ? _ellipsis : String.Empty,
						( ( differentPosition + DisplayRangeOffset ) < expected.Length && hasFollowingEllipsis ) ? _ellipsis : String.Empty,
						hasLeadingEllipsis ? _ellipsis : String.Empty,
						( ( differentPosition + DisplayRangeOffset ) < actual.Length && hasFollowingEllipsis ) ? _ellipsis : String.Empty
					);
			}
			else
			{
				int displayStartAt = Math.Max( 0, differentPosition - DisplayRangeOffset );
				int displayEndAt = Math.Min( differentPosition + DisplayRangeOffset, expected.Length - 1 );

				bool hasLeadingEllipsis = ( 0 < displayStartAt );
				bool hasFollowingEllipsis = ( displayEndAt < expected.Length - 1 );

				return
					String.Format(
						FormatProviders.CurrentCulture,
						"Strings are different at index {1}.{0}" +
						"Expected:'{2:e}'(\\u{2:U}, {2:c}, {2:b}){0}" +
						"Actual  :'{3:e}'(\\u{3:U}, {3:c}, {3:b}){0}" +
						"Expected string :{7}{4:e}{8}{0}" +
						"Actual string   :{7}{5:e}{8}{0}" +
						"                 {6}^",
						Environment.NewLine,
						differentPosition,
						expected[ differentPosition ],
						actual[ differentPosition ],
						expected.Slice( displayStartAt, displayEndAt ),
						actual.Slice( displayStartAt, displayEndAt ),
						new String(
							' ',
							( differentPosition < DisplayRangeOffset ? differentPosition : DisplayRangeOffset ) + ( hasLeadingEllipsis ? _ellipsis.Length : 0 )
						),
						hasLeadingEllipsis ? _ellipsis : String.Empty,
						hasFollowingEllipsis ? _ellipsis : String.Empty
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
