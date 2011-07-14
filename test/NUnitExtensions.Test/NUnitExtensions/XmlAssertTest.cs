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
using System.Xml.Linq;

namespace NLiblet.NUnitExtensions
{
	[TestFixture]
	public class XmlAssertTest
	{
		[Test]
		public void TestAttributes()
		{
			XmlAssert.AreEqual(
				XElement.Parse( "<elem attr1='1' attr2='2' />" ),
				XElement.Parse( "<elem attr1='1' attr2='2' />" ),
				"Same"
			);

			// Order unmatch
			XmlAssert.AreEqual(
				XElement.Parse( "<elem attr1='1' attr2='2' />" ),
				XElement.Parse( "<elem attr2='2' attr1='1' />" ),
				"Unordered"
			);

			// lack
			try
			{
				XmlAssert.AreEqual(
					XElement.Parse( "<elem attr1='1' attr2='2' />" ),
					XElement.Parse( "<elem attr1='1' />" ),
					"Lack"
				);
				throw new Exception( "Fail" );
			}
			catch ( AssertionException ex )
			{
				Console.WriteLine( "Lack" );
				Console.WriteLine( ex.Message );
			}

			// extra
			try
			{
				XmlAssert.AreEqual(
					XElement.Parse( "<elem attr1='1' />" ),
					XElement.Parse( "<elem attr1='1' attr2='2' />" ),
					"Extra"
				);
				throw new Exception( "Fail" );
			}
			catch ( AssertionException ex )
			{
				Console.WriteLine( "Extra" );
				Console.WriteLine( ex.Message );
			}

			// different attribute
			try
			{
				XmlAssert.AreEqual(
					XElement.Parse( "<elem attr1='1' />" ),
					XElement.Parse( "<elem attr2='1' />" ),
					"Different Name"
				);
				throw new Exception( "Fail" );
			}
			catch ( AssertionException ex )
			{
				Console.WriteLine( "Different Name" );
				Console.WriteLine( ex.Message );
			}

			// value not equal
			try
			{
				XmlAssert.AreEqual(
					XElement.Parse( "<elem attr1='1' />" ),
					XElement.Parse( "<elem attr1='2' />" ),
					"Different Value"
				);
				throw new Exception( "Fail" );
			}
			catch ( AssertionException ex )
			{
				Console.WriteLine( "Different Value" );
				Console.WriteLine( ex.Message );
			}
		}

		[Test]
		public void TestElements()
		{
			// same
			XmlAssert.AreEqual(
				XDocument.Parse( "<?xml version='1.0' ?><doc><!--comment--><elem attr1='1' attr2='2'><empty/><text>TEXT</text><cdata><![CDATA[CDATA]]></cdata></elem></doc>" ),
				XDocument.Parse( "<?xml version='1.0' ?><doc><!--comment--><elem attr1='1' attr2='2'><empty/><text>TEXT</text><cdata><![CDATA[CDATA]]></cdata></elem></doc>" ),
				"Same"
			);

			// attribute order only
			XmlAssert.AreEqual(
				XDocument.Parse( "<?xml version='1.0' ?><doc><!--comment--><elem attr1='1' attr2='2'><empty/><text>TEXT</text><cdata><![CDATA[CDATA]]></cdata></elem></doc>" ),
				XDocument.Parse( "<?xml version='1.0' ?><doc><!--comment--><elem attr2='2' attr1='1'><empty/><text>TEXT</text><cdata><![CDATA[CDATA]]></cdata></elem></doc>" ),
				"Attributes unordered"
			);

			// same
			XmlAssert.AreEqual(
				XDocument.Parse( "<?xml version='1.0' ?><doc><!--comment--><elem attr1='1' attr2='2'><empty/><text>TEXT</text><cdata><![CDATA[CDATA]]></cdata></elem></doc>" ),
				XDocument.Parse( "<?xml version='1.0' ?><doc><!--comment--><elem attr1='1' attr2='2'><empty></empty><text>TEXT</text><cdata><![CDATA[CDATA]]></cdata></elem></doc>" ),
				"Empty representation."
			);

			// child unordered
			try
			{
				XmlAssert.AreEqual(
					XElement.Parse( "<parent><child1/><child2/></parent>" ),
					XElement.Parse( "<parent><child3/><child1/></parent>" ),
					"Children unordered"
				);
				throw new Exception( "Fail" );
			}
			catch ( AssertionException ex )
			{
				Console.WriteLine( "Children unordered" );
				Console.WriteLine( ex.Message );
			}

			// CData-Text
			try
			{
				XmlAssert.AreEqual(
					XElement.Parse( "<elem>TEXT</elem>" ),
					XElement.Parse( "<elem><![CDATA[TEXT]]></elem>" ),
					"Text != CData"
				);
				throw new Exception( "Fail" );
			}
			catch ( AssertionException ex )
			{
				Console.WriteLine( "Text != CData" );
				Console.WriteLine( ex.Message );
			}

			// Unmatch Text
			try
			{
				XmlAssert.AreEqual(
					XElement.Parse( "<elem>TExT</elem>" ),
					XElement.Parse( "<elem>TEXT</elem>" ),
					"Text unmatch"
				);
				throw new Exception( "Fail" );
			}
			catch ( AssertionException ex )
			{
				Console.WriteLine( "Text unmatch" );
				Console.WriteLine( ex.Message );
			}

			// Unmatch CData
			try
			{
				XmlAssert.AreEqual(
					XElement.Parse( "<elem><![CDATA[TExT]]></elem>" ),
					XElement.Parse( "<elem><![CDATA[TEXT]]></elem>" ),
					"CData unmatch"
				);
				throw new Exception( "Fail" );
			}
			catch ( AssertionException ex )
			{
				Console.WriteLine( "CData unmatch" );
				Console.WriteLine( ex.Message );
			}

			// Unmatch Comment
			try
			{
				XmlAssert.AreEqual(
					XElement.Parse( "<elem><!--comment--></elem>" ),
					XElement.Parse( "<elem><!--comment --></elem>" ),
					"Comment unmatch"
				);
				throw new Exception( "Fail" );
			}
			catch ( AssertionException ex )
			{
				Console.WriteLine( "Comment unmatch" );
				Console.WriteLine( ex.Message );
			}

			// lack child
			try
			{
				XmlAssert.AreEqual(
					XElement.Parse( "<parent><child1/><child2/></parent>" ),
					XElement.Parse( "<parent><child1/></parent>" ),
					"Lack child"
				);
				throw new Exception( "Fail" );
			}
			catch ( AssertionException ex )
			{
				Console.WriteLine( "Lack child" );
				Console.WriteLine( ex.Message );
			}

			// extra child
			try
			{
				XmlAssert.AreEqual(
					XElement.Parse( "<parent><child1/></parent>" ),
					XElement.Parse( "<parent><child1/><child2/></parent>" ),
					"Extra child"
				);
				throw new Exception( "Fail" );
			}
			catch ( AssertionException ex )
			{
				Console.WriteLine( "Extra child" );
				Console.WriteLine( ex.Message );
			}

		}
	}
}
