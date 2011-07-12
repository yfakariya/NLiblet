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
using System.Linq;
using System.Text;
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
					new [] { "Middles are differ", "_AAAABAAAA_", "_AAAACAAAA_" },
					new [] { "Heads are differ", "BAAAAAAAA", "CAAAAAAAA" },
					new [] { "Tails are differ", "AAAAAAAAB", "AAAAAAAAC" },
					new [] { "Left is shorter", "AAAAAAA", "AAAAAAAA" },
					new [] { "Right is shorter", "AAAAAAAA", "AAAAAAA" },
					new [] { "Left is shorter tiny", "A", "AA" },
					new [] { "Right is shorter tiny", "AA", "A" },
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
