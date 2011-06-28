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
using System.Collections;
using System.Collections.ObjectModel;

namespace NLiblet.Text
{
	[TestFixture]
	public class CommonCustomFormatterTest
	{
		[Test]
		public void TestStringEscaping()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void TestFormatSpecifier()
		{
			Assert.Inconclusive();
		}

		[Test]
		[ExpectedException( typeof( FormatException ) )]
		public void TestFormatSpecifier_Unknown()
		{
			Assert.Inconclusive();
		}

		[Test]
		[ExpectedException( typeof( FormatException ) )]
		public void TestFormatSpecifier_InvalidCodePoint()
		{
			Assert.Inconclusive();
		}

		[Test]
		public void TestArrayToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new object[] { 1, true, false, null, "5", String.Empty, "\t\r\n\a" };
			Assert.AreEqual(
				"[ 1, true, false, null, \"5\", \"\", \"\\t\r\n\\a\" ]",
				String.Format( target, "{0}", sequence as object )
			);
		}

		[Test]
		public void TestSequenceToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new Queue<object>( new object[] { 1, true, false, null, "5", String.Empty, "\t\r\n\a" } );
			Assert.AreEqual(
					"[ 1, true, false, null, \"5\", \"\", \"\\t\r\n\\a\" ]",
					String.Format( target, "{0}", sequence )
				);
		}

		[Test]
		public void TestNonGenericToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new ArrayList( new object[] { 1, true, false, null, "5", String.Empty, "\t\r\n\a" } );
			Assert.AreEqual(
					"[ 1, true, false, null, \"5\", \"\", \"\\t\r\n\\a\" ]",
					String.Format( target, "{0}", sequence )
				);
		}

		[Test]
		public void TestGenericListToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new List<object>( new object[] { 1, true, false, null, "5", String.Empty, "\t\r\n\a" } );
			Assert.AreEqual(
					"[ 1, true, false, null, \"5\", \"\", \"\\t\r\n\\a\" ]",
					String.Format( target, "{0}", sequence )
				);
		}

		[Test]
		public void TestNonGenericDictionaryToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var dictionary = new Hashtable()
			{
				{ "a", 1 }, { "b", true }, { "c", false }, { "d", null }, { "e", "5" }, { "f", String.Empty }, { "g", "\t\r\n\a" }
			};
			Assert.AreEqual(
					"{ \"a\" : 1, \"b\" : true, \"c\" : false, \"d\" : null, \"e\" : \"5\", \"f\" : \"\", \"g\" : \"\\t\r\n\\a\" }",
					String.Format( target, "{0}", dictionary )
				);
		}

		[Test]
		public void TestGenericDictionaryToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var dictionary = new Dictionary<string, object>()
			{
				{ "a", 1 }, { "b", true }, { "c", false }, { "d", null }, { "e", "5" }, { "f", String.Empty }, { "g", "\t\r\n\a" }
			};
			Assert.AreEqual(
					"{ \"a\" : 1, \"b\" : true, \"c\" : false, \"d\" : null, \"e\" : \"5\", \"f\" : \"\", \"g\" : \"\\t\r\n\\a\" }",
					String.Format( target, "{0}", dictionary )
				);
		}

		// Nested
		[Test]
		public void TestNestedCollectionToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var dictionary = new Dictionary<string, object>()
			{
				{ "array", new object[] { 1, true, false, null, "5", String.Empty, "\t\r\n\a" } },
				{ "map", new Hashtable() { { "a", "b" } } }
			};

			Assert.AreEqual(
				"{ \"array\" : [ 1, true, false, null, \"5\", \"\", \"\\t\r\n\\a\" ], \"map\" : { \"a\" : \"b\" } }",
				String.Format( target, "{0}", dictionary )
			);
		}
	}
}
