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
	public class HexFormatTest
	{
		[Test]
		public void TestToHex()
		{
			Assert.AreEqual(
				"00010f107f80ff",
				String.Join( String.Empty, HexFormat.ToHex( new byte[] { 0, 1, 0xf, 0x10, 0x7f, 0x80, 0xff } ) )
			);
			Assert.AreEqual(
				String.Empty,
				String.Join( String.Empty, HexFormat.ToHex( new byte[ 0 ] ) )
			);
			Assert.AreEqual(
				String.Empty,
				String.Join( String.Empty, HexFormat.ToHex( null ) )
			);
		}

		[Test]
		public void TestToHexString()
		{
			Assert.AreEqual(
				"00010f107f80ff",
				HexFormat.ToHexString( new byte[] { 0, 1, 0xf, 0x10, 0x7f, 0x80, 0xff } )
			);
			Assert.AreEqual(
				String.Empty,
				HexFormat.ToHexString( new byte[ 0 ] )
			);
			Assert.AreEqual(
				String.Empty,
				HexFormat.ToHexString( null )
			);
		}

		[Test]
		public void TestGetBytesFromHex_Valid()
		{
			Assert.AreEqual(
				new byte[] { 0, 1, 0xf, 0x10, 0x7f, 0x80, 0xff },
				HexFormat.GetBytesFromHex( "00010f107f80ff" ).ToArray()
			);
			Assert.AreEqual(
				new byte[] { 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF },
				HexFormat.GetBytesFromHex( "aAbBcCdDeEfF" ).ToArray()
			);
			Assert.IsFalse( HexFormat.GetBytesFromHex( String.Empty ).Any() );
			Assert.IsFalse( HexFormat.GetBytesFromHex( null ).Any() );
		}

		[Test]
		[ExpectedException(typeof(FormatException))]
		public void TestGetBytesFromHex_Invalid()
		{
			// Full-width 0A
			HexFormat.GetBytesFromHex( "\uff10\uff21" ).ToArray();
		}

		[Test]
		[ExpectedException( typeof( FormatException ) )]
		public void TestGetBytesFromHex_Odd_1()
		{
			// Full-width 0A
			HexFormat.GetBytesFromHex( "0" ).ToArray();
		}

		[Test]
		[ExpectedException( typeof( FormatException ) )]
		public void TestGetBytesFromHex_Odd_3()
		{
			// Full-width 0A
			HexFormat.GetBytesFromHex( "012" ).ToArray();
		}

		[Test]
		public void TestGetByteArrayFromHex_Valid()
		{
			Assert.AreEqual(
				new byte[] { 0, 1, 0xf, 0x10, 0x7f, 0x80, 0xff },
				HexFormat.GetByteArrayFromHex( "00010f107f80ff" )
			);
			Assert.AreEqual(
				new byte[] { 0xAA, 0xBB, 0xCC, 0xDD, 0xEE, 0xFF },
				HexFormat.GetByteArrayFromHex( "aAbBcCdDeEfF" )
			);
			Assert.IsFalse( HexFormat.GetByteArrayFromHex( String.Empty ).Any() );
			Assert.IsFalse( HexFormat.GetByteArrayFromHex( null ).Any() );
		}

		[Test]
		[ExpectedException( typeof( FormatException ) )]
		public void TestGetByteArrayFromHex_Invalid()
		{
			// Full-width 0A
			HexFormat.GetByteArrayFromHex( "\uff10\uff21" );
		}

		[Test]
		[ExpectedException( typeof( FormatException ) )]
		public void TestGetByteArrayFromHex_Odd_1()
		{
			// Full-width 0A
			HexFormat.GetByteArrayFromHex( "0" );
		}

		[Test]
		[ExpectedException( typeof( FormatException ) )]
		public void TestGetByteArrayFromHex_Odd_3()
		{
			// Full-width 0A
			HexFormat.GetByteArrayFromHex( "012" );
		}
	}
}
