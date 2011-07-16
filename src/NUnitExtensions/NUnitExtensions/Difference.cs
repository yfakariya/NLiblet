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
					return Properties.Resources.StringDifference_ExpectedIsNullButActualIsNotNull;
				}
			}
			else if ( actual == null )
			{
				return Properties.Resources.StringDifference_ExpecteIsNotNullButActualIsNull;
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
						Properties.Resources.StringDifference_LengthAreNotEqual,
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
						Properties.Resources.StringDifference_DifferentAt,
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
