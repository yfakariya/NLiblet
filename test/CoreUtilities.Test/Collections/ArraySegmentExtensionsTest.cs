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
using System.IO;
using System.Linq;
using NUnit.Framework;
using System.Collections;

namespace NLiblet.Collections
{
	[TestFixture]
	public class ArraySegmentExtensionsTest
	{
		[Test]
		public void TestAsEnumerable()
		{
			var array = new[] { 1, 2, 3, 4, 5 };
			CollectionAssert.AreEqual(
				new int[] { 1, 2, 3, 4, 5 },
				new ArraySegment<int>( array, 0, 5 ).AsEnumerable().ToArray()
			);
			CollectionAssert.AreEqual(
				new int[] { 1 },
				new ArraySegment<int>( array, 0, 1 ).AsEnumerable().ToArray()
			);
			CollectionAssert.AreEqual(
				new int[] { 5 },
				new ArraySegment<int>( array, 4, 1 ).AsEnumerable().ToArray()
			);
			CollectionAssert.AreEqual(
				new int[] { 2, 3, 4 },
				new ArraySegment<int>( array, 1, 3 ).AsEnumerable().ToArray()
			);
			Assert.That( new ArraySegment<int>( array, 0, 0 ).AsEnumerable(), Is.Empty );
			Assert.That( new ArraySegment<int>( array, 1, 0 ).AsEnumerable(), Is.Empty );
			Assert.That( new ArraySegment<int>( array, 4, 0 ).AsEnumerable(), Is.Empty );
		}
	}
}
