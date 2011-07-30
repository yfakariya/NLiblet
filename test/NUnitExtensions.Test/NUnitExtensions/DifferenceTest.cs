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
using NUnit.Framework;

namespace NLiblet.NUnitExtensions
{
#if DEBUG
	[TestFixture]
	public class DifferenceTest
	{
		[Test]
		[Explicit] // printf Test
		public void TestCompare()
		{
			/*******************************************************************************************************
			 * verify:
			 *   - Carret indicates right position (first differential charactor).
			 *   - If there are some charactors outside of ( <differential position>> +/- <DisplayRangeOffset> ),
			 *     then ellipsises are placed.
			 *   - If length are not same, then it is explained in error message.
			 *   - If some characters are not equal, then it is explained with 
			 *     its escaped charactor itself, code point, category, and block name in error message.
			 *******************************************************************************************************/

			foreach ( var testData in
				new string[][]
				{
					new [] { "Same", "AAAAAAAA", "AAAAAAAA" },
					new [] { "Both are empty", "", "" },
					new [] { "Left is empty", "", "AAAAAAAA" },
					new [] { "Right is empty","AAAAAAAA", "" },
					new [] { "Middles are differ short", "AAABAAA", "AAACAAA" },
					new []
					{ 
						"Middles are differ just",
						new String( 'A', Difference.DisplayRangeOffset ) + "B" + new String( 'A', Difference.DisplayRangeOffset ),
						new String( 'A', Difference.DisplayRangeOffset ) + "C" + new String( 'A', Difference.DisplayRangeOffset )
					},
					new [] 
					{ 
						"Middles are differ long", 
						"_" + new String( 'A', Difference.DisplayRangeOffset ) + "B" + new String( 'A', Difference.DisplayRangeOffset ) + "_",
						"_" + new String( 'A', Difference.DisplayRangeOffset ) + "C" + new String( 'A', Difference.DisplayRangeOffset ) + "_"
					},
					new [] { "Heads are differ short", "BAAA", "CAAA" },
					new [] 
					{ 
						"Heads are differ just - 1", 
						"B" + new String( 'A', Difference.DisplayRangeOffset - 1 ),
						"C" + new String( 'A', Difference.DisplayRangeOffset - 1 )
					},
					new [] 
					{ 
						"Heads are differ just", 
						"B" + new String( 'A', Difference.DisplayRangeOffset ),
						"C" + new String( 'A', Difference.DisplayRangeOffset )
					},
					new []
					{
						"Heads are differ long",
						"B" + new String( 'A', Difference.DisplayRangeOffset ) + "_",
						"C" + new String( 'A', Difference.DisplayRangeOffset ) + "_" 
					},
					new [] { "Tails are differ short", "AAAB", "AAAC" },
					new [] 
					{ 
						"Tails are differ just - 1",
						new String( 'A', Difference.DisplayRangeOffset - 1 ) + "B", 
						new String( 'A', Difference.DisplayRangeOffset - 1 ) + "C"
					},
					new [] 
					{ 
						"Tails are differ just",
						new String( 'A', Difference.DisplayRangeOffset ) + "B", 
						new String( 'A', Difference.DisplayRangeOffset ) + "C"
					},
					new [] 
					{ 
						"Tails are differ long",
						"_" + new String( 'A', Difference.DisplayRangeOffset ) + "B", 
						"_" + new String( 'A', Difference.DisplayRangeOffset ) + "C" 
					},
					new [] { "Left is shorter", "AAAAAAA", "AAAAAAAA" },
					new [] { "Right is shorter", "AAAAAAAA", "AAAAAAA" },
					new [] { "Left is shorter tiny", "A", "AA" },
					new [] { "Right is shorter tiny", "AA", "A" },
					new []
					{ 
						"Left is shorter just - 1", 
						new String( 'A', Difference.DisplayRangeOffset - 1), 
						new String( 'A', Difference.DisplayRangeOffset ) 
					},
					new [] 
					{
						"Right is shorter just - 1", 
						new String( 'A', Difference.DisplayRangeOffset ), 
						new String( 'A', Difference.DisplayRangeOffset - 1 )
					},
					new []
					{ 
						"Left is shorter just", 
						new String( 'A', Difference.DisplayRangeOffset ), 
						new String( 'A', Difference.DisplayRangeOffset + 1 ) 
					},
					new [] 
					{
						"Right is shorter just", 
						new String( 'A', Difference.DisplayRangeOffset + 1 ), 
						new String( 'A', Difference.DisplayRangeOffset )
					},
					new [] 
					{
						"Left is shorter long", 
						"_" + new String( 'A', Difference.DisplayRangeOffset ),  
						"_" + new String( 'A', Difference.DisplayRangeOffset + 1 )
					},
					new []
					{ 
						"Right is shorter long", 
						"_" + new String( 'A', Difference.DisplayRangeOffset + 1 ),
						"_" + new String( 'A', Difference.DisplayRangeOffset )
					},
					new []
					{ 
						"Left is too shorter long",
						"A", 
						new String( 'A', Difference.DisplayRangeOffset + 1 ) + "_" 
					},
					new []
					{ 
						"Right is too shorter long", 
						new String( 'A', Difference.DisplayRangeOffset + 1 ) + "_",
						"A" 
					},
				}
			)
			{
				var actual = Difference.Compare( testData[ 1 ], testData[ 2 ] );
				Print( testData[ 0 ], testData[ 1 ], testData[ 2 ], actual );
			}
		}

		private static void Print( string label, string left, string right, string actual )
		{
			Console.WriteLine(
				"{1}{0}" +
				"Left :\"{2}\"{0}" +
				"Right:\"{3}\"{0}" +
				"Result:{0}{4}",
				Environment.NewLine,
				label,
				left,
				right,
				actual
			);
		}
	}
#endif
}
