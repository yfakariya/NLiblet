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
using System.Globalization;

namespace NLiblet.Text
{
	[TestFixture]
	public class UnicodeUtilityTest
	{
		#region -- CombineSurrogatePair --
		[Test]
		public void TestCombineSurrogatePair()
		{
			foreach ( var cp in new[] { 0x10000, 0x10ffff } )
			{
				var str = Char.ConvertFromUtf32( cp );
				Assert.AreEqual( cp, UnicodeUtility.CombineSurrogatePair( str[ 0 ], str[ 1 ] ) );
			}
		}
		#endregion

		#region -- ConvertFromUtf32 --
		[Test]
		public void TestConvertFromUtf32()
		{
			foreach ( var cp in new[] { 0x0, 0xffff, 0x10000, 0x10ffff } )
			{
				var c = UnicodeUtility.ConvertFromUtf32( cp ).ToArray();
				CollectionAssert.AreEqual( Char.ConvertFromUtf32( cp ).ToArray(), c );
			}
		}
		#endregion

		#region -- GetUnicodeBlockName --
		[Test]
		public void TestGetUnicodeBlockName()
		{
			Assert.AreEqual( "Basic Latin", UnicodeUtility.GetUnicodeBlockName( '\u0000' ) );
			Assert.AreEqual( "Basic Latin", UnicodeUtility.GetUnicodeBlockName( '\u007f' ) );
			Assert.AreEqual( "Latin-1 Supplement", UnicodeUtility.GetUnicodeBlockName( '\u0080' ) );
			Assert.AreEqual( "Specials", UnicodeUtility.GetUnicodeBlockName( '\uffff' ) );
			Assert.AreEqual( "Basic Latin", UnicodeUtility.GetUnicodeBlockName( 0 ) );
			Assert.AreEqual( "Basic Latin", UnicodeUtility.GetUnicodeBlockName( 0x7f ) );
			Assert.AreEqual( "Latin-1 Supplement", UnicodeUtility.GetUnicodeBlockName( 0x80 ) );
			Assert.AreEqual( "Specials", UnicodeUtility.GetUnicodeBlockName( 0xffff ) );
			Assert.AreEqual( "Linear B Syllabary", UnicodeUtility.GetUnicodeBlockName( 0x10000 ) );
			Assert.AreEqual( "Linear B Syllabary", UnicodeUtility.GetUnicodeBlockName( 0x1007f ) );
			Assert.AreEqual( "Linear B Ideograms", UnicodeUtility.GetUnicodeBlockName( 0x10080 ) );
		}
		#endregion

		#region -- GetUnicodeCategory --
		[Test]
		public void TestGetUnicodeCategory()
		{
			Assert.AreEqual( UnicodeCategory.Control, UnicodeUtility.GetUnicodeCategory( 0 ) );
			Assert.AreEqual( UnicodeCategory.OtherNotAssigned, UnicodeUtility.GetUnicodeCategory( 0xffff ) );
			Assert.AreEqual( UnicodeCategory.OtherLetter, UnicodeUtility.GetUnicodeCategory( 0x10000 ) );
			Assert.AreEqual( UnicodeCategory.OtherNotAssigned, UnicodeUtility.GetUnicodeCategory( 0x10ffff ) );
		}
		#endregion

		#region -- IsPrintable --
		[Test]
		public void TestIsPrintable_Char()
		{
			Assert.IsFalse( UnicodeUtility.IsPrintable( '\u0000' ) ); // null
			Assert.IsFalse( UnicodeUtility.IsPrintable( '\u0020' ) ); // Space
			Assert.IsTrue( UnicodeUtility.IsPrintable( '\u0021' ) ); // Non space ascii
			Assert.IsFalse( UnicodeUtility.IsPrintable( '\u2028' ) ); // Line separator
			Assert.IsFalse( UnicodeUtility.IsPrintable( '\u2029' ) ); // Paragraph separator
			Assert.IsFalse( UnicodeUtility.IsPrintable( '\u3000' ) ); // Full width space
			Assert.IsFalse( UnicodeUtility.IsPrintable( '\ud800' ) ); // Least significant surrogate
			Assert.IsFalse( UnicodeUtility.IsPrintable( '\udfff' ) ); // Most significant surrogate
			Assert.IsTrue( UnicodeUtility.IsPrintable( '\ufffd' ) ); // Replacement
			Assert.IsFalse( UnicodeUtility.IsPrintable( '\uffff' ) ); // Not assigned
		}

		[Test]
		public void TestIsPrintable_Char_Char()
		{
			Assert.IsTrue( UnicodeUtility.IsPrintable( Char.ConvertFromUtf32( 0x10000 )[ 0 ], Char.ConvertFromUtf32( 0x10000 )[ 1 ] ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( Char.ConvertFromUtf32( 0x10ffff )[ 0 ], Char.ConvertFromUtf32( 0x10ffff )[ 1 ] ) ); // Not assigned
		}

		[Test]
		public void TestIsPrintable_String()
		{
			var str =
				"\u0000" + // null
				"\u0020" + // Space
				"\u0021" + // Non space ascii
				"\u2028" + // Line separator
				"\u2029" + // Paragraph separator
				"\u3000" + // Full width space
				"\ud800" + // Least significant surrogate
				"\udfff" + // Most significant surrogate
				"\ufffd" + // Replacement
				"\uffff" + // Not assigned
				Char.ConvertFromUtf32( 0x10000 ) +
				Char.ConvertFromUtf32( 0x10ffff );

			Assert.AreEqual( 14, str.Length );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 0 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 1 ) );
			Assert.IsTrue( UnicodeUtility.IsPrintable( str, 2 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 3 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 4 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 5 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 6 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 7 ) );
			Assert.IsTrue( UnicodeUtility.IsPrintable( str, 8 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 9 ) );
			Assert.IsTrue( UnicodeUtility.IsPrintable( str, 10 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 12 ) );
		}

		[Test]
		public void TestIsPrintable_StringBuilder()
		{
			var str =
				new StringBuilder(
					"\u0000" + // null
					"\u0020" + // Space
					"\u0021" + // Non space ascii
					"\u2028" + // Line separator
					"\u2029" + // Paragraph separator
					"\u3000" + // Full width space
					"\ud800" + // Least significant surrogate
					"\udfff" + // Most significant surrogate
					"\ufffd" + // Replacement
					"\uffff" + // Not assigned
					Char.ConvertFromUtf32( 0x10000 ) +
					Char.ConvertFromUtf32( 0x10ffff )
				);

			Assert.AreEqual( 14, str.Length );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 0 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 1 ) );
			Assert.IsTrue( UnicodeUtility.IsPrintable( str, 2 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 3 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 4 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 5 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 6 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 7 ) );
			Assert.IsTrue( UnicodeUtility.IsPrintable( str, 8 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 9 ) );
			Assert.IsTrue( UnicodeUtility.IsPrintable( str, 10 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( str, 12 ) );
		}

		[Test]
		public void TestIsPrintable_Int32()
		{
			Assert.IsFalse( UnicodeUtility.IsPrintable( 0x0 ) ); // null
			Assert.IsFalse( UnicodeUtility.IsPrintable( 0x20 ) ); // Space
			Assert.IsTrue( UnicodeUtility.IsPrintable( 0x21 ) ); // Non space ascii
			Assert.IsFalse( UnicodeUtility.IsPrintable( 0x2028 ) ); // Line separator
			Assert.IsFalse( UnicodeUtility.IsPrintable( 0x2029 ) ); // Paragraph separator
			Assert.IsFalse( UnicodeUtility.IsPrintable( 0x3000 ) ); // Full width space
			Assert.IsFalse( UnicodeUtility.IsPrintable( 0xd800 ) ); // Least significant surrogate
			Assert.IsFalse( UnicodeUtility.IsPrintable( 0xdfff ) ); // Most significant surrogate
			Assert.IsTrue( UnicodeUtility.IsPrintable( 0xfffd ) ); // Replacement
			Assert.IsFalse( UnicodeUtility.IsPrintable( 0xffff ) ); // Not assigned
			Assert.IsTrue( UnicodeUtility.IsPrintable( 0x10000 ) );
			Assert.IsFalse( UnicodeUtility.IsPrintable( 0x10ffff ) ); // Not assigned
		}
		#endregion

		#region -- ShouldEscape --
		[Test]
		public void TestShouldEscape_Char()
		{
			Assert.IsTrue( UnicodeUtility.ShouldEscape( '\u0000' ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( '\u0020' ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( '\ud7a3' ) ); // \ud7fb is not assigned in the framework.
			Assert.IsTrue( UnicodeUtility.ShouldEscape( '\ud800' ) );
			Assert.IsTrue( UnicodeUtility.ShouldEscape( '\udfff' ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( '\ue000' ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( '\ufffd' ) );
			Assert.IsTrue( UnicodeUtility.ShouldEscape( '\uffff' ) );
		}

		[Test]
		public void TestShouldEscape_Char_Char()
		{
			Assert.IsFalse( UnicodeUtility.ShouldEscape( Char.ConvertFromUtf32( 0x10000 )[ 0 ], Char.ConvertFromUtf32( 0x10000 )[ 1 ] ) );
			Assert.IsTrue( UnicodeUtility.ShouldEscape( Char.ConvertFromUtf32( 0x10ffff )[ 0 ], Char.ConvertFromUtf32( 0x10ffff )[ 1 ] ) );
		}

		[Test]
		public void TestShouldEscape_String()
		{
			var str =
				"\u0000" +
				"\u0020" +
				"\ud7a3" + // \ud7fb is not assigned in the framework.
				"\ud800" +
				"\udfff" +
				"\ue000" +
				"\ufffd" +
				"\uffff" +
				Char.ConvertFromUtf32( 0x10000 ) +
				Char.ConvertFromUtf32( 0x10ffff );

			Assert.IsTrue( UnicodeUtility.ShouldEscape( str, 0 ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( str, 1 ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( str, 2 ) );
			Assert.IsTrue( UnicodeUtility.ShouldEscape( str, 3 ) );
			Assert.IsTrue( UnicodeUtility.ShouldEscape( str, 4 ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( str, 5 ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( str, 6 ) );
			Assert.IsTrue( UnicodeUtility.ShouldEscape( str, 7 ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( str, 8 ) );
			Assert.IsTrue( UnicodeUtility.ShouldEscape( str, 10 ) );
		}

		[Test]
		public void TestShouldEscape_StringBuilder()
		{
			var str =
				new StringBuilder(
					"\u0000" +
					"\u0020" +
					"\ud7a3" + // \ud7fb is not assigned in the framework.
					"\ud800" +
					"\udfff" +
					"\ue000" +
					"\ufffd" +
					"\uffff" +
					Char.ConvertFromUtf32( 0x10000 ) +
					Char.ConvertFromUtf32( 0x10ffff )
				);

			Assert.IsTrue( UnicodeUtility.ShouldEscape( str, 0 ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( str, 1 ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( str, 2 ) );
			Assert.IsTrue( UnicodeUtility.ShouldEscape( str, 3 ) );
			Assert.IsTrue( UnicodeUtility.ShouldEscape( str, 4 ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( str, 5 ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( str, 6 ) );
			Assert.IsTrue( UnicodeUtility.ShouldEscape( str, 7 ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( str, 8 ) );
			Assert.IsTrue( UnicodeUtility.ShouldEscape( str, 10 ) );
		}

		[Test]
		public void TestShouldEscape_Int32()
		{
			Assert.IsTrue( UnicodeUtility.ShouldEscape( 0x0 ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( 0x20 ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( 0xd7a3 ) ); // \ud7fb is not assigned in the framework.
			Assert.IsTrue( UnicodeUtility.ShouldEscape( 0xd800 ) );
			Assert.IsTrue( UnicodeUtility.ShouldEscape( 0xdfff ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( 0xe000 ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( 0xfffd ) );
			Assert.IsTrue( UnicodeUtility.ShouldEscape( 0xffff ) );
			Assert.IsFalse( UnicodeUtility.ShouldEscape( 0x10000 ) );
			Assert.IsTrue( UnicodeUtility.ShouldEscape( 0x10ffff ) );
		}
		#endregion
	}
}
