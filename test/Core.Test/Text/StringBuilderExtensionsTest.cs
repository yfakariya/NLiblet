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

namespace NLiblet.Text
{
	[TestFixture]
	public class StringBuilderExtensionsTest
	{
		[Test]
		public void TestAppendHex()
		{
			var target = new StringBuilder();
			Assert.AreSame(
				target,
				target.AppendHex( new byte[] { 0x0, 0x1, 0xf, 0x10, 0x7f, 0x80, 0xff } )
			);
			Assert.AreSame(
				target,
				target.AppendHex( null )
			);
			Assert.AreSame(
				target,
				target.AppendHex( new byte[ 0 ] )
			);

			Assert.AreEqual(
				"00010f107f80ff",
				target.ToString()
			);
		}

		[Test]
		public void TestAppendChars()
		{
			var target = new StringBuilder();
			Assert.AreSame(
				target,
				target.AppendChars( ( new char[] { 'a', 'b', 'c', 'd' } ) as IEnumerable<char> )
			);

			Assert.AreSame(
				target,
				target.AppendChars( null )
			);
			Assert.AreSame(
				target,
				target.AppendChars( new char[ 0 ] )
			);

			Assert.AreEqual(
				"abcd",
				target.ToString()
			);
		}

		[Test]
		public void TestAsEnumerable()
		{
			var target = new StringBuilder();

			Assert.IsFalse( target.AsEnumerable().Any() );

			target.Append( "0123456789" );
			Assert.AreEqual(
				"0123456789",
				new String( target.AsEnumerable().ToArray() )
			);

			target.Clear();

			Assert.IsFalse( target.AsEnumerable().Any() );
		}
	}
}
