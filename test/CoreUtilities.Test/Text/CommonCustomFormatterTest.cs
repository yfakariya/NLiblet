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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;
using NUnit.Framework;

namespace NLiblet.Text
{
	[TestFixture]
	public class CommonCustomFormatterTest
	{
		private static readonly bool _disableTrace = true;

		[TestFixtureSetUp]
		public static void SetUpClass()
		{
			if ( _disableTrace )
			{
				Debug.Listeners.Clear();
			}
		}

		[TestFixtureTearDown]
		public static void CleanUpClass()
		{
			if ( _disableTrace )
			{
				Debug.Listeners.Add( new DefaultTraceListener() );
			}
		}

#if DEBUG
		[Test]
		public void TestSringEscaping_None()
		{
			Assert.AreEqual(
				"\t\r\n\a",
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0}",
					"\t\r\n\a"
				)
			);
		}

		[Test]
		public void TestStringEscaping_a()
		{
			Assert.AreEqual(
				"aA\\uff21\\U" + ( ( int )( Char.ConvertFromUtf32( 0x10000 )[ 0 ] ) ).ToString( "x4" ) + ( ( int )( Char.ConvertFromUtf32( 0x10000 )[ 1 ] ) ).ToString( "x4" ),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:a}{1:a}{2:a}{3:ua}",
					'a',
					'A',
					'\uff21', // Full width 'A'
					0x10000
				)
			);

			Assert.AreEqual(
				"aA\\uff21\\U" + ( ( int )( Char.ConvertFromUtf32( 0x10000 )[ 0 ] ) ).ToString( "x4" ) + ( ( int )( Char.ConvertFromUtf32( 0x10000 )[ 1 ] ) ).ToString( "x4" ),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:a}",
					"aA" +
					'\uff21' + // Full width 'A'
					Char.ConvertFromUtf32( 0x10000 )
				)
			);

			Assert.AreEqual(
				"aA\\uff21\\U" + ( ( int )( Char.ConvertFromUtf32( 0x10000 )[ 0 ] ) ).ToString( "x4" ) + ( ( int )( Char.ConvertFromUtf32( 0x10000 )[ 1 ] ) ).ToString( "x4" ),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:a}",
					new StringBuilder(
						"aA" +
						'\uff21' + // Full width 'A'
						Char.ConvertFromUtf32( 0x10000 )
					)
				)
			);

			Assert.AreEqual(
				"aA\\uFF21\\U" + ( ( int )( Char.ConvertFromUtf32( 0x10000 )[ 0 ] ) ).ToString( "X4" ) + ( ( int )( Char.ConvertFromUtf32( 0x10000 )[ 1 ] ) ).ToString( "X4" ),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:A}{1:A}{2:A}{3:uA}",
					'a',
					'A',
					'\uff21', // Full width 'A'
					0x10000
				)
			);

			Assert.AreEqual(
				"aA\\uFF21\\U" + ( ( int )( Char.ConvertFromUtf32( 0x10000 )[ 0 ] ) ).ToString( "X4" ) + ( ( int )( Char.ConvertFromUtf32( 0x10000 )[ 1 ] ) ).ToString( "X4" ),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:A}",
					"aA" +
					'\uff21' + // Full width 'A'
					Char.ConvertFromUtf32( 0x10000 )
				)
			);

			Assert.AreEqual(
				"aA\\uFF21\\U" + ( ( int )( Char.ConvertFromUtf32( 0x10000 )[ 0 ] ) ).ToString( "X4" ) + ( ( int )( Char.ConvertFromUtf32( 0x10000 )[ 1 ] ) ).ToString( "X4" ),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:A}",
					new StringBuilder(
						"aA" +
						'\uff21' + // Full width 'A'
						Char.ConvertFromUtf32( 0x10000 )
					)
				)
			);
		}

		[Test]
		public void TestStringEscaping_b()
		{
			Assert.AreEqual(
				"Basic Latin;Halfwidth and Fullwidth Forms;Specials;Linear B Syllabary",
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:b};{1:b};{2:b};{3:ub}",
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);

			Assert.AreEqual(
				"Basic Latin;Halfwidth and Fullwidth Forms;Specials;Linear B Syllabary",
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:B};{1:B};{2:B};{3:UB}",
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);
		}

		[Test]
		public void TestStringEscaping_c()
		{
			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"{0};{1};{2};{3}",
					UnicodeCategory.LowercaseLetter,
					UnicodeCategory.UppercaseLetter,
					UnicodeCategory.OtherNotAssigned,
					CharUnicodeInfo.GetUnicodeCategory( Char.ConvertFromUtf32( 0x10000 ), 0 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:c};{1:c};{2:c};{3:uc}",
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"{0};{1};{2};{3}",
					UnicodeCategory.LowercaseLetter,
					UnicodeCategory.UppercaseLetter,
					UnicodeCategory.OtherNotAssigned,
					CharUnicodeInfo.GetUnicodeCategory( Char.ConvertFromUtf32( 0x10000 ), 0 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:C};{1:C};{2:C};{3:UC}",
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);
		}

		[Test]
		public void TestStringEscaping_d()
		{
			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"{0};{1};{2}",
					( int )'a',
					( int )'\uff21',
					0x10000
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:d};{1:d};{2:ud}",
					'a',
					'\uff21', // Full width 'A'
					0x10000
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"{0};{1};{2}",
					( int )'a',
					( int )'\uff21',
					0x10000
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:D};{1:D};{2:UD}",
					'a',
					'\uff21', // Full width 'A'
					0x10000
				)
			);
		}

		[Test]
		public void TestStringEscaping_e()
		{
			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\t;\r;\n;\";a;\uff21;\ufffd;{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:e};{1:e};{2:e};{3:e};{4:e};{5:e};{6:e};{7:ue}",
					'\t',
					'\r',
					'\n',
					'"',
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\t\r\n\"a\uff21\ufffd{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:e}",
					"\t" +
					'\r' +
					'\n' +
					'"' +
					'a' +
					'\uff21' + // Full width 'A'
					'\uffff' +
					Char.ConvertFromUtf32( 0x10000 )
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\t\r\n\"a\uff21\ufffd{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:e}",
					new StringBuilder(
						"\t" +
						'\r' +
						'\n' +
						'"' +
						'a' +
						'\uff21' + // Full width 'A'
						'\uffff' +
						Char.ConvertFromUtf32( 0x10000 )
					)
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\t;\r;\n;\";a;\uff21;\ufffd;{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:E};{1:E};{2:E};{3:E};{4:E};{5:E};{6:E};{7:UE}",
					'\t',
					'\r',
					'\n',
					'"',
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\t\r\n\"a\uff21\ufffd{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:E}",
					"\t" +
					'\r' +
					'\n' +
					'"' +
					'a' +
					'\uff21' + // Full width 'A'
					'\uffff' +
					Char.ConvertFromUtf32( 0x10000 )
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\t\r\n\"a\uff21\ufffd{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:E}",
					new StringBuilder(
						"\t" +
						'\r' +
						'\n' +
						'"' +
						'a' +
						'\uff21' + // Full width 'A'
						'\uffff' +
						Char.ConvertFromUtf32( 0x10000 )
					)
				)
			);
		}

		[Test]
		public void TestStringEscaping_g()
		{
			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t;\r;\n;\";a;\uff21;\\uffff;{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:g};{1:g};{2:g};{3:g};{4:g};{5:g};{6:g};{7:ug}",
					'\t',
					'\r',
					'\n',
					'"',
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\r\n\"a\uff21\\uffff{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:g}",
					"\t" +
					'\r' +
					'\n' +
					'"' +
					'a' +
					'\uff21' + // Full width 'A'
					'\uffff' +
					Char.ConvertFromUtf32( 0x10000 )
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\r\n\"a\uff21\\uffff{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:g}",
					new StringBuilder(
						"\t" +
						'\r' +
						'\n' +
						'"' +
						'a' +
						'\uff21' + // Full width 'A'
						'\uffff' +
						Char.ConvertFromUtf32( 0x10000 )
					)
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t;\r;\n;\";a;\uff21;\\uFFFF;{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:G};{1:G};{2:G};{3:G};{4:G};{5:G};{6:G};{7:UG}",
					'\t',
					'\r',
					'\n',
					'"',
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\r\n\"a\uFF21\\uFFFF{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:G}",
					"\t" +
					'\r' +
					'\n' +
					'"' +
					'a' +
					'\uff21' + // Full width 'A'
					'\uffff' +
					Char.ConvertFromUtf32( 0x10000 )
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\r\n\"a\uFF21\\uFFFF{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:G}",
					new StringBuilder(
						"\t" +
						'\r' +
						'\n' +
						'"' +
						'a' +
						'\uff21' + // Full width 'A'
						'\uffff' +
						Char.ConvertFromUtf32( 0x10000 )
					)
				)
			);
		}

		[Test]
		public void TestStringEscaping_l()
		{
			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t;\\r;\\n;\\\";a;\uff21;\\uffff;{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:l};{1:l};{2:l};{3:l};{4:l};{5:l};{6:l};{7:ul}",
					'\t',
					'\r',
					'\n',
					'"',
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\\r\\n\\\"a\uff21\\uffff{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:l}",
					"\t" +
					'\r' +
					'\n' +
					'"' +
					'a' +
					'\uff21' + // Full width 'A'
					'\uffff' +
					Char.ConvertFromUtf32( 0x10000 )
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\\r\\n\\\"a\uff21\\uffff{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:l}",
					new StringBuilder(
						"\t" +
						'\r' +
						'\n' +
						'"' +
						'a' +
						'\uff21' + // Full width 'A'
						'\uffff' +
						Char.ConvertFromUtf32( 0x10000 )
					)
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t;\\r;\\n;\\\";a;\uff21;\\uFFFF;{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:L};{1:L};{2:L};{3:L};{4:L};{5:L};{6:L};{7:UL}",
					'\t',
					'\r',
					'\n',
					'"',
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\\r\\n\\\"a\uFF21\\uFFFF{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:L}",
					"\t" +
					'\r' +
					'\n' +
					'"' +
					'a' +
					'\uff21' + // Full width 'A'
					'\uffff' +
					Char.ConvertFromUtf32( 0x10000 )
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\\r\\n\\\"a\uFF21\\uFFFF{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:L}",
					new StringBuilder(
						"\t" +
						'\r' +
						'\n' +
						'"' +
						'a' +
						'\uff21' + // Full width 'A'
						'\uffff' +
						Char.ConvertFromUtf32( 0x10000 )
					)
				)
			);
		}

		[Test]
		public void TestStringEscaping_m()
		{
			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t;\r;\n;\";a;\uff21;\\uffff;{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:m};{1:m};{2:m};{3:m};{4:m};{5:m};{6:m};{7:um}",
					'\t',
					'\r',
					'\n',
					'"',
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\r\n\"a\uff21\\uffff{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:m}",
					"\t" +
					'\r' +
					'\n' +
					'"' +
					'a' +
					'\uff21' + // Full width 'A'
					'\uffff' +
					Char.ConvertFromUtf32( 0x10000 )
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\r\n\"a\uff21\\uffff{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:m}",
					new StringBuilder(
						"\t" +
						'\r' +
						'\n' +
						'"' +
						'a' +
						'\uff21' + // Full width 'A'
						'\uffff' +
						Char.ConvertFromUtf32( 0x10000 )
					)
				)
			);
			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t;\r;\n;\";a;\uff21;\\uFFFF;{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:M};{1:M};{2:M};{3:M};{4:M};{5:M};{6:M};{7:UM}",
					'\t',
					'\r',
					'\n',
					'"',
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\r\n\"a\uFF21\\uFFFF{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:M}",
					"\t" +
					'\r' +
					'\n' +
					'"' +
					'a' +
					'\uff21' + // Full width 'A'
					'\uffff' +
					Char.ConvertFromUtf32( 0x10000 )
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\r\n\"a\uFF21\\uFFFF{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:M}",
					new StringBuilder(
						"\t" +
						'\r' +
						'\n' +
						'"' +
						'a' +
						'\uff21' + // Full width 'A'
						'\uffff' +
						Char.ConvertFromUtf32( 0x10000 )
					)
				)
			);
		}

		[Test]
		public void TestStringEscaping_r()
		{
			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\t;\r;\n;\";a;\uff21;\uffff;{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:r};{1:r};{2:r};{3:r};{4:r};{5:r};{6:r};{7:ur}",
					'\t',
					'\r',
					'\n',
					'"',
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\t\r\n\"a\uff21\uffff{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:r}",
					"\t" +
					'\r' +
					'\n' +
					'"' +
					'a' +
					'\uff21' + // Full width 'A'
					'\uffff' +
					Char.ConvertFromUtf32( 0x10000 )
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\t\r\n\"a\uff21\uffff{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:r}",
					new StringBuilder(
						"\t" +
						'\r' +
						'\n' +
						'"' +
						'a' +
						'\uff21' + // Full width 'A'
						'\uffff' +
						Char.ConvertFromUtf32( 0x10000 )
					)
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\t;\r;\n;\";a;\uff21;\uffff;{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:R};{1:R};{2:R};{3:R};{4:R};{5:R};{6:R};{7:UR}",
					'\t',
					'\r',
					'\n',
					'"',
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\t\r\n\"a\uff21\uffff{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:R}",
					"\t" +
					'\r' +
					'\n' +
					'"' +
					'a' +
					'\uff21' + // Full width 'A'
					'\uffff' +
					Char.ConvertFromUtf32( 0x10000 )
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\t\r\n\"a\uff21\uffff{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:R}",
					new StringBuilder(
						"\t" +
						'\r' +
						'\n' +
						'"' +
						'a' +
						'\uff21' + // Full width 'A'
						'\uffff' +
						Char.ConvertFromUtf32( 0x10000 )
					)
				)
			);
		}

		[Test]
		public void TestStringEscaping_s()
		{
			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t;\\r;\\n;\";a;\uff21;\\uffff;{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:s};{1:s};{2:s};{3:s};{4:s};{5:s};{6:s};{7:us}",
					'\t',
					'\r',
					'\n',
					'"',
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\\r\\n\"a\uff21\\uffff{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:s}",
					"\t" +
					'\r' +
					'\n' +
					'"' +
					'a' +
					'\uff21' + // Full width 'A'
					'\uffff' +
					Char.ConvertFromUtf32( 0x10000 )
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\\r\\n\"a\uff21\\uffff{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:s}",
					new StringBuilder(
						"\t" +
						'\r' +
						'\n' +
						'"' +
						'a' +
						'\uff21' + // Full width 'A'
						'\uffff' +
						Char.ConvertFromUtf32( 0x10000 )
					)
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t;\\r;\\n;\";a;\uff21;\\uFFFF;{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:S};{1:S};{2:S};{3:S};{4:S};{5:S};{6:S};{7:US}",
					'\t',
					'\r',
					'\n',
					'"',
					'a',
					'\uff21', // Full width 'A'
					'\uffff',
					0x10000
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\\r\\n\"a\uFF21\\uFFFF{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:S}",
					"\t" +
					'\r' +
					'\n' +
					'"' +
					'a' +
					'\uff21' + // Full width 'A'
					'\uffff' +
					Char.ConvertFromUtf32( 0x10000 )
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"\\t\\r\\n\"a\uFF21\\uFFFF{0}",
					Char.ConvertFromUtf32( 0x10000 )
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:S}",
					new StringBuilder(
						"\t" +
						'\r' +
						'\n' +
						'"' +
						'a' +
						'\uff21' + // Full width 'A'
						'\uffff' +
						Char.ConvertFromUtf32( 0x10000 )
					)
				)
			);
		}

		[Test]
		public void TestStringEscaping_u()
		{
			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"{0:x4};{1:x4};1000a",
					( int )'a',
					( int )'\uff21'
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:u};{1:u};{2:uu}",
					'a',
					'\uff21', // Full width 'A'
					0x1000a
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"{0:X4};{1:X4};1000A",
					( int )'a',
					( int )'\uff21'
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:U};{1:U};{2:UU}",
					'a',
					'\uff21', // Full width 'A'
					0x1000A
				)
			);
		}

		[Test]
		public void TestStringEscaping_x()
		{
			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"{0:x};{1:x};1000a",
					( int )'a',
					( int )'\uff21'
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:x};{1:x};{2:ux}",
					'a',
					'\uff21', // Full width 'A'
					0x1000a
				)
			);

			Assert.AreEqual(
				String.Format(
					CultureInfo.InvariantCulture,
					"{0:X};{1:X};1000A",
					( int )'a',
					( int )'\uff21'
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0:X};{1:X};{2:UX}",
					'a',
					'\uff21', // Full width 'A'
					0x1000A
				)
			);
		}

		[Test]
		[ExpectedException( typeof( FormatException ) )]
		public void TestFormatSpecifier_Unknown()
		{
			String.Format( new CommonCustomFormatter( CultureInfo.InvariantCulture ), "{0:z}", 'a' );
		}

		[Test]
		[ExpectedException( typeof( FormatException ) )]
		public void TestFormatSpecifier_InvalidCodePoint()
		{
			String.Format( new CommonCustomFormatter( CultureInfo.InvariantCulture ), "{0:ur}", 0x110000 );
		}

		[Test]
		public void TestDefaultFormatProvider()
		{
			var floating = 1234.56789m;
			var date = new DateTimeOffset( new DateTime( 2001, 6, 5 ) );
			foreach ( var culture in new[] { CultureInfo.InvariantCulture, CultureInfo.GetCultureInfo( "fr-FR" ), CultureInfo.GetCultureInfo( "ja-JP" ), CultureInfo.GetCultureInfo( "he-IL" ), CultureInfo.GetCultureInfo( "ru-RU" ), CultureInfo.GetCultureInfo( "vi-VN" ) } )
			{
				// NOTE: Respecting JSON, numerics formats are not customizable.
				Assert.AreEqual(
					String.Format( CultureInfo.InvariantCulture, "[ \"{0}\", \"{1}\" ]", floating.ToString( CultureInfo.InvariantCulture ), date.ToString( "O", culture ) ),
					String.Format( new CommonCustomFormatter( culture ), "{0}", new object[] { floating, date } as object )
				);

				Assert.AreEqual(
					String.Format( CultureInfo.InvariantCulture, "{0};{1}", floating.ToString( "#,###.00000", culture ), date.ToString( "F", culture ) ),
					String.Format( new CommonCustomFormatter( culture ), "{0:#,###.00000};{1:F}", floating, date )
				);
			}
		}

		[Test]
		public void TestArrayToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new object[] { 1, true, false, null, "5", String.Empty, "\"\t\r\n\a", TimeSpan.FromSeconds( 1 ), new object() };
			Assert.AreEqual(
				"[ 1, true, false, null, \"5\", \"\", \"\\\"\\t\\r\\n\\a\", \"00:00:01\", \"System.Object\" ]",
				String.Format( target, "{0}", sequence as object )
			);
		}

		[Test]
		public void TestByteArrayToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new byte[] { 0, 1, 0x0f, 0x10, 0x7f, 0x80, 0xff };
			Assert.AreEqual(
				"00010f107f80ff",
				String.Format( target, "{0}", sequence )
			);
		}

		[Test]
		public void TestCharArrayToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new char[] { 'a', 'b', 'c' };
			Assert.AreEqual(
				"abc",
				String.Format( target, "{0}", sequence )
			);
		}

		[Test]
		public void TestSequenceToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new Queue<object>( new object[] { 1, true, false, null, "5", String.Empty, "\"\t\r\n\a", TimeSpan.FromSeconds( 1 ), new object() } );
			Assert.AreEqual(
					"[ 1, true, false, null, \"5\", \"\", \"\\\"\\t\\r\\n\\a\", \"00:00:01\", \"System.Object\" ]",
					String.Format( target, "{0}", sequence )
				);
		}

		[Test]
		public void TestByteEnumerableToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new Queue<byte>( new byte[] { 0, 1, 0x0f, 0x10, 0x7f, 0x80, 0xff } );
			Assert.AreEqual(
				"00010f107f80ff",
				String.Format( target, "{0}", sequence )
			);
		}

		[Test]
		public void TestCharEnumerableToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new Queue<char>( new char[] { 'a', 'b', 'c' } );
			Assert.AreEqual(
				"abc",
				String.Format( target, "{0}", sequence )
			);
		}

		[Test]
		public void TestSequenceNotCollectionToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new TestEnumerableNonCollection<int>( 1, 2, 3 );
			Assert.AreEqual(
					typeof( TestEnumerableNonCollection<int> ).ToString(),
					String.Format( target, "{0}", sequence )
				);
		}

		[Test]
		public void TestByteEnumerableNotCollectionToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new TestEnumerableNonCollection<byte>( 0x0, 0x0f, 0x10, 0x7f, 0x80, 0xff );
			Assert.AreEqual(
				typeof( TestEnumerableNonCollection<byte> ).ToString(),
				String.Format( target, "{0}", sequence )
			);
		}

		[Test]
		public void TestCharEnumerableNotCollectionToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new TestEnumerableNonCollection<char>( 'a', 'b', 'c' );
			Assert.AreEqual(
				typeof( TestEnumerableNonCollection<char> ).ToString(),
				String.Format( target, "{0}", sequence )
			);
		}

		[Test]
		public void TestByteListToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new List<byte>() { 0x0, 0x1, 0x0f, 0x10, 0x7f, 0x80, 0xff };
			Assert.AreEqual(
				"00010f107f80ff",
				String.Format( target, "{0}", sequence )
			);
		}

		[Test]
		public void TestCharListToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new List<char>() { 'a', 'b', 'c' };
			Assert.AreEqual(
				"abc",
				String.Format( target, "{0}", sequence )
			);
		}

		[Test]
		public void TestArraySegmentToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new object[] { 1, true, false, null, "5", String.Empty, "\"\t\r\n\a", TimeSpan.FromSeconds( 1 ), new object() };
			Assert.AreEqual(
				"[ 1, true, false, null, \"5\", \"\", \"\\\"\\t\\r\\n\\a\", \"00:00:01\", \"System.Object\" ]",
				String.Format( target, "{0}", new ArraySegment<object>( sequence, 0, sequence.Length ) )
			);
			Assert.AreEqual(
				"[ 1, true, false ]",
				String.Format( target, "{0}", new ArraySegment<object>( sequence, 0, 3 ) )
			);
			Assert.AreEqual(
				"[ null, \"5\", \"\" ]",
				String.Format( target, "{0}", new ArraySegment<object>( sequence, 3, 3 ) )
			);
			Assert.AreEqual(
				"[ \"\\\"\\t\\r\\n\\a\", \"00:00:01\", \"System.Object\" ]",
				String.Format( target, "{0}", new ArraySegment<object>( sequence, 6, 3 ) )
			);
		}

		[Test]
		public void TestByteArraySegmentToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new byte[] { 0, 1, 0x0f, 0x10, 0x7f, 0x80, 0xff };
			Assert.AreEqual(
				"00010f107f80ff",
				String.Format( target, "{0}", new ArraySegment<byte>( sequence, 0, sequence.Length ) )
			);
			Assert.AreEqual(
				"00010f",
				String.Format( target, "{0}", new ArraySegment<byte>( sequence, 0, 3 ) )
			);
			Assert.AreEqual(
				"107f80",
				String.Format( target, "{0}", new ArraySegment<byte>( sequence, 3, 3 ) )
			);
			Assert.AreEqual(
				"7f80ff",
				String.Format( target, "{0}", new ArraySegment<byte>( sequence, 4, 3 ) )
			);
		}

		[Test]
		public void TestCharArraySegmentToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new char[] { 'a', 'b', 'c', 'd', 'e' };
			Assert.AreEqual(
				"abcde",
				String.Format( target, "{0}", new ArraySegment<char>( sequence, 0, sequence.Length ) )
			);
			Assert.AreEqual(
				"abc",
				String.Format( target, "{0}", new ArraySegment<char>( sequence, 0, 3 ) )
			);
			Assert.AreEqual(
				"bcd",
				String.Format( target, "{0}", new ArraySegment<char>( sequence, 1, 3 ) )
			);
			Assert.AreEqual(
				"cde",
				String.Format( target, "{0}", new ArraySegment<char>( sequence, 2, 3 ) )
			);
		}

		[Test]
		public void TestSerializationToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var info = new SerializationInfo( typeof( object ), new FormatterConverter() );
			info.AddValue( "int", 1 );
			info.AddValue( "true", true );
			info.AddValue( "false", false );
			info.AddValue( "null", null );
			info.AddValue( "NumberString", "5" );
			info.AddValue( "EmptyString", String.Empty );
			info.AddValue( "MustBeEscaped", "\"\t\r\n\a" );
			info.AddValue( "TimeSpan", TimeSpan.FromSeconds( 1 ) );
			info.AddValue( "object", new object() );
			Assert.AreEqual(
					"{ \"int\" : 1, \"true\" : true, \"false\" : false, \"null\" : null," +
					" \"NumberString\" : \"5\", \"EmptyString\" : \"\", \"MustBeEscaped\" : \"\\\"\\t\\r\\n\\a\"," +
					" \"TimeSpan\" : \"00:00:01\", \"object\" : \"System.Object\" }",
					String.Format( target, "{0}", info )
				);
		}

		[Test]
		public void TestTuple()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var tuple =
				new Tuple<int, bool, bool, object, string, string, string, Tuple<TimeSpan, object>>(
					1, true, false, default( object ), "5", String.Empty, "\"\t\r\n\a", Tuple.Create( TimeSpan.FromSeconds( 1 ), new object() )
				);
			Assert.AreEqual(
					"[ 1, true, false, null, \"5\", \"\", \"\\\"\\t\\r\\n\\a\", [ \"00:00:01\", \"System.Object\" ] ]",
					String.Format( target, "{0}", tuple )
				);
		}

		[Test]
		public void TestSequenceHasToStringToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new HasToString<int>( 1, 2, 3 );
			Assert.AreEqual(
					sequence.ToString(),
					String.Format( target, "{0}", sequence )
				);
		}

		[Test]
		public void TestNonGenericToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new ArrayList( new object[] { 1, true, false, null, "5", String.Empty, "\"\t\r\n\a", TimeSpan.FromSeconds( 1 ), new object() } );
			Assert.AreEqual(
					"[ 1, true, false, null, \"5\", \"\", \"\\\"\\t\\r\\n\\a\", \"00:00:01\", \"System.Object\" ]",
					String.Format( target, "{0}", sequence )
				);
		}

		[Test]
		public void TestGenericListOfObjectToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new List<object>( new object[] { 1, true, false, null, "5", String.Empty, "\"\t\r\n\a", TimeSpan.FromSeconds( 1 ), new object() } );
			Assert.AreEqual(
					"[ 1, true, false, null, \"5\", \"\", \"\\\"\\t\\r\\n\\a\", \"00:00:01\", \"System.Object\" ]",
					String.Format( target, "{0}", sequence )
				);
		}

		[Test]
		public void TestGenericListOfInt32ToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var sequence = new List<int>() { 1, 2, 3 };
			Assert.AreEqual(
					"[ 1, 2, 3 ]",
					String.Format( target, "{0}", sequence )
				);
		}

		[Test]
		public void TestNonGenericDictionaryToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var objKey = new object();
			var dictionary = new Hashtable()
			{
				{ 1, 1 }, { true, true }, { false, false }, { "null", null }, { "5", "5" }, { String.Empty, String.Empty }, { "g", "\"\t\r\n\a" }, { "time", TimeSpan.FromSeconds( 1 ) }, { objKey, new object() }
			};
			var expecteds = new Dictionary<object, string>()
			{
				{ 1, "1 : 1" }, { true, "true : true" }, { false, "false : false" }, { "null", "\"null\" : null" }, { "5", "\"5\" : \"5\"" }, { String.Empty, "\"\" : \"\"" }, { "g", "\"g\" : \"\\\"\\t\\r\\n\\a\"" }, { "time", "\"time\" : \"00:00:01\""}, { objKey, "\"System.Object\" : \"System.Object\"" }
			};
			var buffer = new StringBuilder();
			buffer.Append( "{ " );
			bool isFirst = true;
			foreach ( DictionaryEntry item in dictionary )
			{
				if ( isFirst )
				{
					isFirst = false;
				}
				else
				{
					buffer.Append( ", " );
				}

				buffer.Append( expecteds[ item.Key ] );
			}
			buffer.Append( " }" );
			Assert.AreEqual(
					buffer.ToString(),
					String.Format( target, "{0}", dictionary )
				);
		}

		[Test]
		public void TestGenericDictionaryToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var dictionary = new Dictionary<string, object>()
			{
				{ "a", 1 }, { "b", true }, { "c", false }, { "d", null }, { "e", "5" }, { "f", String.Empty }, { "g", "\"\t\r\n\a" }, { "h", TimeSpan.FromSeconds( 1 ) }, { "i", new object() }
			};
			Assert.AreEqual(
					"{ \"a\" : 1, \"b\" : true, \"c\" : false, \"d\" : null, \"e\" : \"5\", \"f\" : \"\", \"g\" : \"\\\"\\t\\r\\n\\a\", \"h\" : \"00:00:01\", \"i\" : \"System.Object\" }",
					String.Format( target, "{0}", dictionary )
				);
		}

		// Nested
		[Test]
		public void TestNestedCollectionToString()
		{
			var target = new CommonCustomFormatter( CultureInfo.InvariantCulture );
			var buffer = new StringBuilder();
			var objKey = new object();
			var innerDictionary = new Hashtable()
			{
				{ 1, 1 }, { true, true }, { false, false }, { "null", null }, { "5", "5" }, { String.Empty, String.Empty }, { "g", "\"\t\r\n\a" }, { "time", TimeSpan.FromSeconds( 1 ) }, { objKey, new object() }
			};
			var expecteds = new Dictionary<object, string>()
			{
				{ 1, "1 : 1" }, { true, "true : true" }, { false, "false : false" }, { "null", "\"null\" : null" }, { "5", "\"5\" : \"5\"" }, { String.Empty, "\"\" : \"\"" }, { "g", "\"g\" : \"\\\"\\t\\r\\n\\a\"" }, { "time", "\"time\" : \"00:00:01\""}, { objKey, "\"System.Object\" : \"System.Object\"" }
			};
			buffer.Append( "{ " );
			bool isFirst = true;
			foreach ( DictionaryEntry item in innerDictionary )
			{
				if ( isFirst )
				{
					isFirst = false;
				}
				else
				{
					buffer.Append( ", " );
				}

				buffer.Append( expecteds[ item.Key ] );
			}
			buffer.Append( " }" );

			var outerDictionary = new Dictionary<string, object>()
			{
				{ "array", new object[] { 1, true, false, null, "5", String.Empty, "\"\t\r\n\a" } },
				{ "map", innerDictionary }
			};

			Assert.AreEqual(
				"{ \"array\" : [ 1, true, false, null, \"5\", \"\", \"\\\"\\t\\r\\n\\a\" ], \"map\" : " + buffer + " }",
				String.Format( target, "{0}", outerDictionary )
			);
		}

		[Test]
		public void TestNonPrimitives()
		{
			var item1 = new Uri( "http://example.com" );
			var item2 = DateTimeOffset.Now;
			var item3 = DateTime.Now;
			var item4 = TimeSpan.FromSeconds( 1 );
			var item5 = new Version( "1.0" );
			var item6 = XElement.Parse( "<elem attr='value'>text</elem>" );
			var item7 = new BigInteger( ulong.MaxValue ) * 2;
			var item8 = new Complex( 1.2, 3.4 );
			var item9 = 12345678901234567890.123456789m;
			Assert.AreEqual(
				String.Format(
					CultureInfo.CurrentCulture,
					"[ \"{0}\", \"{1:o}\", \"{2:o}\", \"{3}\", \"{4}\", \"{5}\", \"{6}\", \"{7}\", \"{8}\" ]",
					item1,
					item2,
					item3,
					item4,
					item5,
					item6,
					item7,
					item8,
					item9
				),
				String.Format(
					new CommonCustomFormatter( CultureInfo.InvariantCulture ),
					"{0}",
					new object[] { item1, item2, item3, item4, item5, item6, item7, item8, item9 } as object
				)
			);
		}

#endif
		[Test]
		[Explicit]
		public void TestPerformance()
		{
			var buffer = new StringBuilder();
			var objKey = new object();
			var innerDictionary = new Hashtable()
			{
				{ 1, 1 }, { true, true }, { false, false }, { "null", null }, { "5", "5" }, { String.Empty, String.Empty }, { "g", "\"\t\r\n\a" }, { "time", TimeSpan.FromSeconds( 1 ) }, { objKey, new object() }
			};
			var expecteds = new Dictionary<object, string>()
			{
				{ 1, "1 : 1" }, { true, "true : true" }, { false, "false : false" }, { "null", "\"null\" : null" }, { "5", "\"5\" : \"5\"" }, { String.Empty, "\"\" : \"\"" }, { "g", "\"g\" : \"\\\"\\t\\r\\n\\a\"" }, { "time", "\"time\" : \"00:00:01\""}, { objKey, "\"System.Object\" : \"System.Object\"" }
			};
			buffer.Append( "{ " );
			bool isFirst = true;
			foreach ( DictionaryEntry item in innerDictionary )
			{
				if ( isFirst )
				{
					isFirst = false;
				}
				else
				{
					buffer.Append( ", " );
				}

				buffer.Append( expecteds[ item.Key ] );
			}
			buffer.Append( " }" );

			var outerDictionary = new Dictionary<string, object>()
			{
				{ "array", new object[] { 1, true, false, null, "5", String.Empty, "\"\t\r\n\a" } },
				{ "map", innerDictionary }
			};

			{
				var str = String.Format( CultureInfo.InvariantCulture, "{0}", outerDictionary );
				str = String.Format( CultureInfo.CurrentCulture, "{0}", outerDictionary );
				str = String.Format( FormatProviders.InvariantCulture, "{0}", outerDictionary );
				str = String.Format( FormatProviders.CurrentCulture, "{0}", outerDictionary );
			}

			GC.Collect();

			const int iteration = 100000;
			var sw = Stopwatch.StartNew();
			for ( int i = 0; i < iteration; i++ )
			{
				var str = String.Format( CultureInfo.InvariantCulture, "{0}", outerDictionary );
			}

			sw.Stop();
			var cultureInfoInvariant = sw.Elapsed;
			sw.Reset();
			GC.Collect();
			sw.Start();

			for ( int i = 0; i < iteration; i++ )
			{
				var str = String.Format( CultureInfo.CurrentCulture, "{0}", outerDictionary );
			}

			sw.Stop();
			var cultureInfoCurrent = sw.Elapsed;
			sw.Reset();
			GC.Collect();
			sw.Start();

			for ( int i = 0; i < iteration; i++ )
			{
				var str = String.Format( FormatProviders.InvariantCulture, "{0}", outerDictionary );
			}

			sw.Stop();
			var customInvariant = sw.Elapsed;
			sw.Reset();
			GC.Collect();
			sw.Start();

			for ( int i = 0; i < iteration; i++ )
			{
				var str = String.Format( FormatProviders.CurrentCulture, "{0}", outerDictionary );
			}

			sw.Stop();
			var customCurrent = sw.Elapsed;

			Console.WriteLine( "Iteraqtion: {0:#,##0}", iteration );
			Console.WriteLine( "CulureInfo.InvariantCulture     :{0}(x1.00)", new TimeSpan( cultureInfoInvariant.Ticks / iteration ) );
			Console.WriteLine( "CulureInfo.CurrentCulture       :{0}(x{1:#,##0.00})", new TimeSpan( cultureInfoCurrent.Ticks / iteration ), ( double )cultureInfoCurrent.Ticks / cultureInfoInvariant.Ticks );
			Console.WriteLine( "CustomFormatter.InvariantCulture:{0}(x{1:#,##0.00})", new TimeSpan( customInvariant.Ticks / iteration ), ( double )customInvariant.Ticks / cultureInfoInvariant.Ticks );
			Console.WriteLine( "CustomFormatter.CurrentCulture  :{0}(x{1:#,##0.00})", new TimeSpan( customCurrent.Ticks / iteration ), ( double )customCurrent.Ticks / cultureInfoInvariant.Ticks );
		}

		private sealed class TestEnumerableNonCollection<T> : IEnumerable<T>
		{
			private readonly T[] _array;

			public TestEnumerableNonCollection( params T[] array )
			{
				this._array = array;
			}

			public IEnumerator<T> GetEnumerator()
			{
				foreach ( var item in this._array )
				{
					yield return item;
				}
			}

			System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}

		private sealed class HasToString<T> : Collection<T>
		{
			public HasToString( params T[] array )
				: base( array ) { }

			public override string ToString()
			{
				if ( this.Items == null || this.Items.Count == 0 )
				{
					return "<>";
				}
				else
				{
					return "< " + String.Join( ", ", this.Items ) + " >";
				}
			}
		}
	}
}
