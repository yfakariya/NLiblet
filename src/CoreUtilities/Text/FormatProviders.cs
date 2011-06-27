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
using System.Globalization;

namespace NLiblet.Text
{
	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	///		<list type="table">
	///			<listheader>
	///				<term>Format indicator</term>
	///				<description>Behavior</description>
	///			</listheader>
	/// 		<item>
	/// 			<term>a, A</term>
	/// 			<description>
	/// 				<strong>A</strong>SCII; all non-ascii charcters will be escaped with \uxxxx syntax.
	/// 				Note that alphabet characeters in hexadecimal is always uppercase.
	/// 			</description>
	/// 		</item>
	/// 		<item>
	/// 			<term>b, B</term>
	/// 			<description>
	/// 				<em>This indicator is only valid to <see cref="Char"/>, <see cref="int32"/>(considered as UTF-32).</em>
	/// 				Unicode <strong>b</strong>lock name.
	/// 				<note>
	/// 					Currently not supported.
	/// 				</note>
	/// 			</description>
	/// 		</item>
	/// 		<item>
	/// 			<term>c, C</term>
	/// 			<description>
	/// 				<em>This indicator is only valid to <see cref="Char"/>, <see cref="int32"/>(considered as UTF-32).</em>
	/// 				Unicode <strong>c</strong>ategory
	/// 			</description>
	/// 		</item>
	/// 		<item>
	/// 			<term>d, D</term>
	/// 			<description>
	/// 				<em>This indicator is only valid to <see cref="Char"/>, <see cref="int32"/>(considered as UTF-32).</em>
	/// 				<strong>D</strong>ecimal representation of unicode codepoint.
	/// 			</description>
	/// 		</item>
	/// 		<item>
	/// 			<term>e, E</term>
	/// 			<description>
	/// 				<strong>E</strong>scaping non printable chars with U+FFFD.
	/// 			</description>
	/// 		</item>
	/// 		<item>
	/// 			<term>g, G</term>
	/// 			<description>
	/// 				<strong>G</strong>eneral; same as 'm'.
	/// 			</description>
	/// 		</item>
	/// 		<item>
	/// 			<term>l, L</term>
	/// 			<description>
	/// 				<strong>L</strong>iteral style.
	/// 				It is similar to 's' style, but additionaly escape '"' to '\"'.
	/// 				<note>
	/// 					String entity in collections will be always escaped using this style.
	/// 				</note>
	/// 			</description>
	/// 		</item>
	/// 		<item>
	/// 			<term>m, M</term>
	/// 			<description>
	/// 				<strong>M</strong>ulti line escaped char with \uxxxx notation.
	/// 				All control chars without line breaks, orphen surrogate, non-assinged code points will be escaped.
	/// 			</description>
	/// 		</item>
	/// 		<item>
	/// 			<term>r, R</term>
	/// 			<description>
	/// 				Raw-char without any escaping. It means that no escaping will not be performed.
	/// 			</description>
	/// 		</item>
	/// 		<item>
	/// 			<term>s, S</term>
	/// 			<description>
	/// 				<strong>S</strong>ulti line escaped char with \uxxxx notation.
	/// 				All control chars with line breaks, orphen surrogate, non-assinged code points will be escaped.
	/// 			</description>
	/// 		</item>
	/// 		<item>
	/// 			<term>u, U</term>
	/// 			<description>
	/// 				<em>This indicator is only valid to <see cref="int32"/>.</em>
	/// 				Consider specified <see cref="Int32"/> value as <strong>U</strong>tf-32.
	/// 				You can specify additional format sepcifier following this like 'l', 'm', 's', 'x', etc. in this table to control format of UTF-32 value.
	/// 				Note that you cannot specify additional 'u' specifier since it is nonsense.
	/// 				For example, value '1' with format "uc" will be "Control", '0x61' with format "ub" will be "BasicLatin", and '0x1F0A1' with format "ur" will be spade ace mark.
	/// 			</description>
	/// 		</item>
	/// 		<item>
	/// 			<term>x</term>
	/// 			<description>
	/// 				<em>This indicator is only valid to <see cref="Char"/>, <see cref="int32"/>(considered as UTF-32).</em>
	/// 				Utf-16 he<strong>x</strong>, with alphabets in hex representation will be uppercase.
	/// 			</description>
	/// 		</item>
	/// 		<item>
	/// 			<term>X</term>
	/// 			<description>
	/// 				<em>This indicator is only valid to <see cref="Char"/>, <see cref="StringInfo"/>, <see cref="int32"/>(considered as UTF-32).</em>
	/// 				Utf-16 he<strong>x</strong>, with alphabets in hex representation will be uppercase.
	/// 			</description>
	/// 		</item>
	/// 	</list>
	/// </remarks>
	public static class FormatProviders
	{
		// TODO: caching

		public static IFormatProvider CurrentCulture
		{
			get { return new CommonCustomFormatter( CultureInfo.CurrentCulture ); }
		}

		public static IFormatProvider CurrentUICulture
		{
			get { return new CommonCustomFormatter( CultureInfo.CurrentUICulture ); }
		}

		public static IFormatProvider InvariantCulture
		{
			get { return new CommonCustomFormatter( CultureInfo.InvariantCulture ); }
		}
	}

}
