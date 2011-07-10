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

namespace NLiblet
{
	[TestFixture]
	public class StringExtensions
	{
		[Test]
		public void TestSlice()
		{
			Assert.AreEqual( "ABCDE", "ABCDE".Slice( 0, 4 ) );
			Assert.AreEqual( "A", "ABCDE".Slice( 0, 0 ) );
			Assert.AreEqual( "E", "ABCDE".Slice( 4, 4 ) );
			Assert.AreEqual( "BCD", "ABCDE".Slice( 1, 3 ) );
		}

		[Test]
		public void TestSubstringLoosely_WithoutPadding()
		{
			Assert.AreEqual( "ABCDE", "ABCDE".SubstringLoosely( 0, 5 ) );
			Assert.AreEqual( "ABCD", "ABCDE".SubstringLoosely( 0, 4 ) );
			Assert.AreEqual( "BCDE", "ABCDE".SubstringLoosely( 1, 4 ) );
			Assert.AreEqual( "BCD", "ABCDE".SubstringLoosely( 1, 3 ) );
			Assert.AreEqual( "BCDE", "ABCDE".SubstringLoosely( 1, 5 ) );
			Assert.AreEqual( "E", "ABCDE".SubstringLoosely( 4, 5 ) );
		}

		[Test]
		public void TestSubstringLoosely_WithPadding()
		{
			Assert.AreEqual( "ABCDE", "ABCDE".SubstringLoosely( 0, 5, '_' ) );
			Assert.AreEqual( "ABCD", "ABCDE".SubstringLoosely( 0, 4, '_' ) );
			Assert.AreEqual( "BCDE", "ABCDE".SubstringLoosely( 1, 4, '_' ) );
			Assert.AreEqual( "BCD", "ABCDE".SubstringLoosely( 1, 3, '_' ) );
			Assert.AreEqual( "BCDE_", "ABCDE".SubstringLoosely( 1, 5, '_' ) );
			Assert.AreEqual( "E____", "ABCDE".SubstringLoosely( 4, 5, '_' ) );
		}
	}
}
