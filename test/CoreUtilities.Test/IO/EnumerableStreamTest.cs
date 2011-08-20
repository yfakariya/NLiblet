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
using System.IO;

namespace NLiblet.IO
{
	[TestFixture]
	public class EnumerableStreamTest
	{
		[Test]
		public void EnumerateTest()
		{
			foreach ( byte[] expected in new[] { new byte[ 0 ], new byte[] { 1 }, new byte[] { 1, 2 } } )
			{
				using ( var target = new EnumerableStream( expected ) )
				{
					Assert.AreEqual( expected.Length, target.Length );
					byte[] actual = new byte[ expected.Length ];
					Assert.AreEqual( expected.Length, target.Read( actual, 0, actual.Length ) );
					CollectionAssert.AreEqual( expected, actual );
				}
			}

			foreach ( var expected in new[] { 1, 2 } )
			{
				using ( var target = new EnumerableStream( Enumerable.Range( 1, expected ).Select( item => ( byte )item ) ) )
				{
					byte[] actual = new byte[ expected ];
					Assert.AreEqual( expected, target.Read( actual, 0, actual.Length ) );
					CollectionAssert.AreEqual( Enumerable.Range( 1, expected ).Select( item => ( byte )item ).ToArray(), actual );
				}
			}
		}
	}
}
