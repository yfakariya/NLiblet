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
	[TestFixture]
	public class DifferenceTest
	{
		[Test]
		[Explicit] // printf Test
		public void TestCompare()
		{
			foreach ( var testData in
				new string[][]
				{
					new [] { "Same", "AAAAAAAA", "AAAAAAAA" },
					new [] { "Both are empty", "", "" },
					new [] { "Left is empty", "", "AAAAAAAA" },
					new [] { "Right is empty","AAAAAAAA", "" },
					new [] { "Middles are differ short", "AAABAAA", "AAACAAA" },
					new [] { "Middles are differ just", "AAAABAAAA", "AAAACAAAA" },
					new [] { "Middles are differ long", "_AAAABAAAA_", "_AAAACAAAA_" },
					new [] { "Heads are differ short", "BAAA", "CAAA" },
					new [] { "Heads are differ just", "BAAAA", "CAAAA" },
					new [] { "Heads are differ long", "BAAAA_", "CAAAA_" },
					new [] { "Tails are differ short", "AAAB", "AAAC" },
					new [] { "Tails are differ just", "AAAAB", "AAAAC" },
					new [] { "Tails are differ long", "_AAAAB", "_AAAAC" },
					new [] { "Left is shorter", "AAAAAAA", "AAAAAAAA" },
					new [] { "Right is shorter", "AAAAAAAA", "AAAAAAA" },
					new [] { "Left is shorter tiny", "A", "AA" },
					new [] { "Right is shorter tiny", "AA", "A" },
					new [] { "Left is shorter just", "AAAA", "AAAAA" },
					new [] { "Right is shorter just", "AAAAA", "AAAA" },
					new [] { "Left is shorter long", "________AAAAA", "________AAAAAA" },
					new [] { "Right is shorter long", "________AAAAAA", "________AAAAA" },
					new [] { "Left is too shorter long", "A", "AAAAA_" },
					new [] { "Right is too shorter long", "AAAAA_", "A" },
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
}
