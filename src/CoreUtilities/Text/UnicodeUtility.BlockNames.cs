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

// This code is generated from T4Template UnicodeUtility.BlockNames.tt.
// Do not modify this source code directly.

using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace NLiblet.Text
{
	partial class UnicodeUtility
	{
		/// <summary>
		///		Get unicode block name of specified UTF-32 code point.
		/// </summary>
		/// <param name="codePoint">UTF-32 code point.</param>
		/// <returns>Unicode block name.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/GetUnicodeBlockName/remarks'/>
		public static string GetUnicodeBlockName( int codePoint )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= codePoint );
			Contract.Requires<ArgumentOutOfRangeException>( codePoint <= 0x10FFFF );
			
			if ( 0x0000 <= codePoint && codePoint <= 0x007F )
			{
				return "Basic Latin";
			}
			if ( 0x0080 <= codePoint && codePoint <= 0x00FF )
			{
				return "Latin-1 Supplement";
			}
			if ( 0x0100 <= codePoint && codePoint <= 0x017F )
			{
				return "Latin Extended-A";
			}
			if ( 0x0180 <= codePoint && codePoint <= 0x024F )
			{
				return "Latin Extended-B";
			}
			if ( 0x0250 <= codePoint && codePoint <= 0x02AF )
			{
				return "IPA Extensions";
			}
			if ( 0x02B0 <= codePoint && codePoint <= 0x02FF )
			{
				return "Spacing Modifier Letters";
			}
			if ( 0x0300 <= codePoint && codePoint <= 0x036F )
			{
				return "Combining Diacritical Marks";
			}
			if ( 0x0370 <= codePoint && codePoint <= 0x03FF )
			{
				return "Greek and Coptic";
			}
			if ( 0x0400 <= codePoint && codePoint <= 0x04FF )
			{
				return "Cyrillic";
			}
			if ( 0x0500 <= codePoint && codePoint <= 0x052F )
			{
				return "Cyrillic Supplement";
			}
			if ( 0x0530 <= codePoint && codePoint <= 0x058F )
			{
				return "Armenian";
			}
			if ( 0x0590 <= codePoint && codePoint <= 0x05FF )
			{
				return "Hebrew";
			}
			if ( 0x0600 <= codePoint && codePoint <= 0x06FF )
			{
				return "Arabic";
			}
			if ( 0x0700 <= codePoint && codePoint <= 0x074F )
			{
				return "Syriac";
			}
			if ( 0x0750 <= codePoint && codePoint <= 0x077F )
			{
				return "Arabic Supplement";
			}
			if ( 0x0780 <= codePoint && codePoint <= 0x07BF )
			{
				return "Thaana";
			}
			if ( 0x07C0 <= codePoint && codePoint <= 0x07FF )
			{
				return "NKo";
			}
			if ( 0x0800 <= codePoint && codePoint <= 0x083F )
			{
				return "Samaritan";
			}
			if ( 0x0840 <= codePoint && codePoint <= 0x085F )
			{
				return "Mandaic";
			}
			if ( 0x0900 <= codePoint && codePoint <= 0x097F )
			{
				return "Devanagari";
			}
			if ( 0x0980 <= codePoint && codePoint <= 0x09FF )
			{
				return "Bengali";
			}
			if ( 0x0A00 <= codePoint && codePoint <= 0x0A7F )
			{
				return "Gurmukhi";
			}
			if ( 0x0A80 <= codePoint && codePoint <= 0x0AFF )
			{
				return "Gujarati";
			}
			if ( 0x0B00 <= codePoint && codePoint <= 0x0B7F )
			{
				return "Oriya";
			}
			if ( 0x0B80 <= codePoint && codePoint <= 0x0BFF )
			{
				return "Tamil";
			}
			if ( 0x0C00 <= codePoint && codePoint <= 0x0C7F )
			{
				return "Telugu";
			}
			if ( 0x0C80 <= codePoint && codePoint <= 0x0CFF )
			{
				return "Kannada";
			}
			if ( 0x0D00 <= codePoint && codePoint <= 0x0D7F )
			{
				return "Malayalam";
			}
			if ( 0x0D80 <= codePoint && codePoint <= 0x0DFF )
			{
				return "Sinhala";
			}
			if ( 0x0E00 <= codePoint && codePoint <= 0x0E7F )
			{
				return "Thai";
			}
			if ( 0x0E80 <= codePoint && codePoint <= 0x0EFF )
			{
				return "Lao";
			}
			if ( 0x0F00 <= codePoint && codePoint <= 0x0FFF )
			{
				return "Tibetan";
			}
			if ( 0x1000 <= codePoint && codePoint <= 0x109F )
			{
				return "Myanmar";
			}
			if ( 0x10A0 <= codePoint && codePoint <= 0x10FF )
			{
				return "Georgian";
			}
			if ( 0x1100 <= codePoint && codePoint <= 0x11FF )
			{
				return "Hangul Jamo";
			}
			if ( 0x1200 <= codePoint && codePoint <= 0x137F )
			{
				return "Ethiopic";
			}
			if ( 0x1380 <= codePoint && codePoint <= 0x139F )
			{
				return "Ethiopic Supplement";
			}
			if ( 0x13A0 <= codePoint && codePoint <= 0x13FF )
			{
				return "Cherokee";
			}
			if ( 0x1400 <= codePoint && codePoint <= 0x167F )
			{
				return "Unified Canadian Aboriginal Syllabics";
			}
			if ( 0x1680 <= codePoint && codePoint <= 0x169F )
			{
				return "Ogham";
			}
			if ( 0x16A0 <= codePoint && codePoint <= 0x16FF )
			{
				return "Runic";
			}
			if ( 0x1700 <= codePoint && codePoint <= 0x171F )
			{
				return "Tagalog";
			}
			if ( 0x1720 <= codePoint && codePoint <= 0x173F )
			{
				return "Hanunoo";
			}
			if ( 0x1740 <= codePoint && codePoint <= 0x175F )
			{
				return "Buhid";
			}
			if ( 0x1760 <= codePoint && codePoint <= 0x177F )
			{
				return "Tagbanwa";
			}
			if ( 0x1780 <= codePoint && codePoint <= 0x17FF )
			{
				return "Khmer";
			}
			if ( 0x1800 <= codePoint && codePoint <= 0x18AF )
			{
				return "Mongolian";
			}
			if ( 0x18B0 <= codePoint && codePoint <= 0x18FF )
			{
				return "Unified Canadian Aboriginal Syllabics Extended";
			}
			if ( 0x1900 <= codePoint && codePoint <= 0x194F )
			{
				return "Limbu";
			}
			if ( 0x1950 <= codePoint && codePoint <= 0x197F )
			{
				return "Tai Le";
			}
			if ( 0x1980 <= codePoint && codePoint <= 0x19DF )
			{
				return "New Tai Lue";
			}
			if ( 0x19E0 <= codePoint && codePoint <= 0x19FF )
			{
				return "Khmer Symbols";
			}
			if ( 0x1A00 <= codePoint && codePoint <= 0x1A1F )
			{
				return "Buginese";
			}
			if ( 0x1A20 <= codePoint && codePoint <= 0x1AAF )
			{
				return "Tai Tham";
			}
			if ( 0x1B00 <= codePoint && codePoint <= 0x1B7F )
			{
				return "Balinese";
			}
			if ( 0x1B80 <= codePoint && codePoint <= 0x1BBF )
			{
				return "Sundanese";
			}
			if ( 0x1BC0 <= codePoint && codePoint <= 0x1BFF )
			{
				return "Batak";
			}
			if ( 0x1C00 <= codePoint && codePoint <= 0x1C4F )
			{
				return "Lepcha";
			}
			if ( 0x1C50 <= codePoint && codePoint <= 0x1C7F )
			{
				return "Ol Chiki";
			}
			if ( 0x1CD0 <= codePoint && codePoint <= 0x1CFF )
			{
				return "Vedic Extensions";
			}
			if ( 0x1D00 <= codePoint && codePoint <= 0x1D7F )
			{
				return "Phonetic Extensions";
			}
			if ( 0x1D80 <= codePoint && codePoint <= 0x1DBF )
			{
				return "Phonetic Extensions Supplement";
			}
			if ( 0x1DC0 <= codePoint && codePoint <= 0x1DFF )
			{
				return "Combining Diacritical Marks Supplement";
			}
			if ( 0x1E00 <= codePoint && codePoint <= 0x1EFF )
			{
				return "Latin Extended Additional";
			}
			if ( 0x1F00 <= codePoint && codePoint <= 0x1FFF )
			{
				return "Greek Extended";
			}
			if ( 0x2000 <= codePoint && codePoint <= 0x206F )
			{
				return "General Punctuation";
			}
			if ( 0x2070 <= codePoint && codePoint <= 0x209F )
			{
				return "Superscripts and Subscripts";
			}
			if ( 0x20A0 <= codePoint && codePoint <= 0x20CF )
			{
				return "Currency Symbols";
			}
			if ( 0x20D0 <= codePoint && codePoint <= 0x20FF )
			{
				return "Combining Diacritical Marks for Symbols";
			}
			if ( 0x2100 <= codePoint && codePoint <= 0x214F )
			{
				return "Letterlike Symbols";
			}
			if ( 0x2150 <= codePoint && codePoint <= 0x218F )
			{
				return "Number Forms";
			}
			if ( 0x2190 <= codePoint && codePoint <= 0x21FF )
			{
				return "Arrows";
			}
			if ( 0x2200 <= codePoint && codePoint <= 0x22FF )
			{
				return "Mathematical Operators";
			}
			if ( 0x2300 <= codePoint && codePoint <= 0x23FF )
			{
				return "Miscellaneous Technical";
			}
			if ( 0x2400 <= codePoint && codePoint <= 0x243F )
			{
				return "Control Pictures";
			}
			if ( 0x2440 <= codePoint && codePoint <= 0x245F )
			{
				return "Optical Character Recognition";
			}
			if ( 0x2460 <= codePoint && codePoint <= 0x24FF )
			{
				return "Enclosed Alphanumerics";
			}
			if ( 0x2500 <= codePoint && codePoint <= 0x257F )
			{
				return "Box Drawing";
			}
			if ( 0x2580 <= codePoint && codePoint <= 0x259F )
			{
				return "Block Elements";
			}
			if ( 0x25A0 <= codePoint && codePoint <= 0x25FF )
			{
				return "Geometric Shapes";
			}
			if ( 0x2600 <= codePoint && codePoint <= 0x26FF )
			{
				return "Miscellaneous Symbols";
			}
			if ( 0x2700 <= codePoint && codePoint <= 0x27BF )
			{
				return "Dingbats";
			}
			if ( 0x27C0 <= codePoint && codePoint <= 0x27EF )
			{
				return "Miscellaneous Mathematical Symbols-A";
			}
			if ( 0x27F0 <= codePoint && codePoint <= 0x27FF )
			{
				return "Supplemental Arrows-A";
			}
			if ( 0x2800 <= codePoint && codePoint <= 0x28FF )
			{
				return "Braille Patterns";
			}
			if ( 0x2900 <= codePoint && codePoint <= 0x297F )
			{
				return "Supplemental Arrows-B";
			}
			if ( 0x2980 <= codePoint && codePoint <= 0x29FF )
			{
				return "Miscellaneous Mathematical Symbols-B";
			}
			if ( 0x2A00 <= codePoint && codePoint <= 0x2AFF )
			{
				return "Supplemental Mathematical Operators";
			}
			if ( 0x2B00 <= codePoint && codePoint <= 0x2BFF )
			{
				return "Miscellaneous Symbols and Arrows";
			}
			if ( 0x2C00 <= codePoint && codePoint <= 0x2C5F )
			{
				return "Glagolitic";
			}
			if ( 0x2C60 <= codePoint && codePoint <= 0x2C7F )
			{
				return "Latin Extended-C";
			}
			if ( 0x2C80 <= codePoint && codePoint <= 0x2CFF )
			{
				return "Coptic";
			}
			if ( 0x2D00 <= codePoint && codePoint <= 0x2D2F )
			{
				return "Georgian Supplement";
			}
			if ( 0x2D30 <= codePoint && codePoint <= 0x2D7F )
			{
				return "Tifinagh";
			}
			if ( 0x2D80 <= codePoint && codePoint <= 0x2DDF )
			{
				return "Ethiopic Extended";
			}
			if ( 0x2DE0 <= codePoint && codePoint <= 0x2DFF )
			{
				return "Cyrillic Extended-A";
			}
			if ( 0x2E00 <= codePoint && codePoint <= 0x2E7F )
			{
				return "Supplemental Punctuation";
			}
			if ( 0x2E80 <= codePoint && codePoint <= 0x2EFF )
			{
				return "CJK Radicals Supplement";
			}
			if ( 0x2F00 <= codePoint && codePoint <= 0x2FDF )
			{
				return "Kangxi Radicals";
			}
			if ( 0x2FF0 <= codePoint && codePoint <= 0x2FFF )
			{
				return "Ideographic Description Characters";
			}
			if ( 0x3000 <= codePoint && codePoint <= 0x303F )
			{
				return "CJK Symbols and Punctuation";
			}
			if ( 0x3040 <= codePoint && codePoint <= 0x309F )
			{
				return "Hiragana";
			}
			if ( 0x30A0 <= codePoint && codePoint <= 0x30FF )
			{
				return "Katakana";
			}
			if ( 0x3100 <= codePoint && codePoint <= 0x312F )
			{
				return "Bopomofo";
			}
			if ( 0x3130 <= codePoint && codePoint <= 0x318F )
			{
				return "Hangul Compatibility Jamo";
			}
			if ( 0x3190 <= codePoint && codePoint <= 0x319F )
			{
				return "Kanbun";
			}
			if ( 0x31A0 <= codePoint && codePoint <= 0x31BF )
			{
				return "Bopomofo Extended";
			}
			if ( 0x31C0 <= codePoint && codePoint <= 0x31EF )
			{
				return "CJK Strokes";
			}
			if ( 0x31F0 <= codePoint && codePoint <= 0x31FF )
			{
				return "Katakana Phonetic Extensions";
			}
			if ( 0x3200 <= codePoint && codePoint <= 0x32FF )
			{
				return "Enclosed CJK Letters and Months";
			}
			if ( 0x3300 <= codePoint && codePoint <= 0x33FF )
			{
				return "CJK Compatibility";
			}
			if ( 0x3400 <= codePoint && codePoint <= 0x4DBF )
			{
				return "CJK Unified Ideographs Extension A";
			}
			if ( 0x4DC0 <= codePoint && codePoint <= 0x4DFF )
			{
				return "Yijing Hexagram Symbols";
			}
			if ( 0x4E00 <= codePoint && codePoint <= 0x9FFF )
			{
				return "CJK Unified Ideographs";
			}
			if ( 0xA000 <= codePoint && codePoint <= 0xA48F )
			{
				return "Yi Syllables";
			}
			if ( 0xA490 <= codePoint && codePoint <= 0xA4CF )
			{
				return "Yi Radicals";
			}
			if ( 0xA4D0 <= codePoint && codePoint <= 0xA4FF )
			{
				return "Lisu";
			}
			if ( 0xA500 <= codePoint && codePoint <= 0xA63F )
			{
				return "Vai";
			}
			if ( 0xA640 <= codePoint && codePoint <= 0xA69F )
			{
				return "Cyrillic Extended-B";
			}
			if ( 0xA6A0 <= codePoint && codePoint <= 0xA6FF )
			{
				return "Bamum";
			}
			if ( 0xA700 <= codePoint && codePoint <= 0xA71F )
			{
				return "Modifier Tone Letters";
			}
			if ( 0xA720 <= codePoint && codePoint <= 0xA7FF )
			{
				return "Latin Extended-D";
			}
			if ( 0xA800 <= codePoint && codePoint <= 0xA82F )
			{
				return "Syloti Nagri";
			}
			if ( 0xA830 <= codePoint && codePoint <= 0xA83F )
			{
				return "Common Indic Number Forms";
			}
			if ( 0xA840 <= codePoint && codePoint <= 0xA87F )
			{
				return "Phags-pa";
			}
			if ( 0xA880 <= codePoint && codePoint <= 0xA8DF )
			{
				return "Saurashtra";
			}
			if ( 0xA8E0 <= codePoint && codePoint <= 0xA8FF )
			{
				return "Devanagari Extended";
			}
			if ( 0xA900 <= codePoint && codePoint <= 0xA92F )
			{
				return "Kayah Li";
			}
			if ( 0xA930 <= codePoint && codePoint <= 0xA95F )
			{
				return "Rejang";
			}
			if ( 0xA960 <= codePoint && codePoint <= 0xA97F )
			{
				return "Hangul Jamo Extended-A";
			}
			if ( 0xA980 <= codePoint && codePoint <= 0xA9DF )
			{
				return "Javanese";
			}
			if ( 0xAA00 <= codePoint && codePoint <= 0xAA5F )
			{
				return "Cham";
			}
			if ( 0xAA60 <= codePoint && codePoint <= 0xAA7F )
			{
				return "Myanmar Extended-A";
			}
			if ( 0xAA80 <= codePoint && codePoint <= 0xAADF )
			{
				return "Tai Viet";
			}
			if ( 0xAB00 <= codePoint && codePoint <= 0xAB2F )
			{
				return "Ethiopic Extended-A";
			}
			if ( 0xABC0 <= codePoint && codePoint <= 0xABFF )
			{
				return "Meetei Mayek";
			}
			if ( 0xAC00 <= codePoint && codePoint <= 0xD7AF )
			{
				return "Hangul Syllables";
			}
			if ( 0xD7B0 <= codePoint && codePoint <= 0xD7FF )
			{
				return "Hangul Jamo Extended-B";
			}
			if ( 0xD800 <= codePoint && codePoint <= 0xDB7F )
			{
				return "High Surrogates";
			}
			if ( 0xDB80 <= codePoint && codePoint <= 0xDBFF )
			{
				return "High Private Use Surrogates";
			}
			if ( 0xDC00 <= codePoint && codePoint <= 0xDFFF )
			{
				return "Low Surrogates";
			}
			if ( 0xE000 <= codePoint && codePoint <= 0xF8FF )
			{
				return "Private Use Area";
			}
			if ( 0xF900 <= codePoint && codePoint <= 0xFAFF )
			{
				return "CJK Compatibility Ideographs";
			}
			if ( 0xFB00 <= codePoint && codePoint <= 0xFB4F )
			{
				return "Alphabetic Presentation Forms";
			}
			if ( 0xFB50 <= codePoint && codePoint <= 0xFDFF )
			{
				return "Arabic Presentation Forms-A";
			}
			if ( 0xFE00 <= codePoint && codePoint <= 0xFE0F )
			{
				return "Variation Selectors";
			}
			if ( 0xFE10 <= codePoint && codePoint <= 0xFE1F )
			{
				return "Vertical Forms";
			}
			if ( 0xFE20 <= codePoint && codePoint <= 0xFE2F )
			{
				return "Combining Half Marks";
			}
			if ( 0xFE30 <= codePoint && codePoint <= 0xFE4F )
			{
				return "CJK Compatibility Forms";
			}
			if ( 0xFE50 <= codePoint && codePoint <= 0xFE6F )
			{
				return "Small Form Variants";
			}
			if ( 0xFE70 <= codePoint && codePoint <= 0xFEFF )
			{
				return "Arabic Presentation Forms-B";
			}
			if ( 0xFF00 <= codePoint && codePoint <= 0xFFEF )
			{
				return "Halfwidth and Fullwidth Forms";
			}
			if ( 0xFFF0 <= codePoint && codePoint <= 0xFFFF )
			{
				return "Specials";
			}
			if ( 0x10000 <= codePoint && codePoint <= 0x1007F )
			{
				return "Linear B Syllabary";
			}
			if ( 0x10080 <= codePoint && codePoint <= 0x100FF )
			{
				return "Linear B Ideograms";
			}
			if ( 0x10100 <= codePoint && codePoint <= 0x1013F )
			{
				return "Aegean Numbers";
			}
			if ( 0x10140 <= codePoint && codePoint <= 0x1018F )
			{
				return "Ancient Greek Numbers";
			}
			if ( 0x10190 <= codePoint && codePoint <= 0x101CF )
			{
				return "Ancient Symbols";
			}
			if ( 0x101D0 <= codePoint && codePoint <= 0x101FF )
			{
				return "Phaistos Disc";
			}
			if ( 0x10280 <= codePoint && codePoint <= 0x1029F )
			{
				return "Lycian";
			}
			if ( 0x102A0 <= codePoint && codePoint <= 0x102DF )
			{
				return "Carian";
			}
			if ( 0x10300 <= codePoint && codePoint <= 0x1032F )
			{
				return "Old Italic";
			}
			if ( 0x10330 <= codePoint && codePoint <= 0x1034F )
			{
				return "Gothic";
			}
			if ( 0x10380 <= codePoint && codePoint <= 0x1039F )
			{
				return "Ugaritic";
			}
			if ( 0x103A0 <= codePoint && codePoint <= 0x103DF )
			{
				return "Old Persian";
			}
			if ( 0x10400 <= codePoint && codePoint <= 0x1044F )
			{
				return "Deseret";
			}
			if ( 0x10450 <= codePoint && codePoint <= 0x1047F )
			{
				return "Shavian";
			}
			if ( 0x10480 <= codePoint && codePoint <= 0x104AF )
			{
				return "Osmanya";
			}
			if ( 0x10800 <= codePoint && codePoint <= 0x1083F )
			{
				return "Cypriot Syllabary";
			}
			if ( 0x10840 <= codePoint && codePoint <= 0x1085F )
			{
				return "Imperial Aramaic";
			}
			if ( 0x10900 <= codePoint && codePoint <= 0x1091F )
			{
				return "Phoenician";
			}
			if ( 0x10920 <= codePoint && codePoint <= 0x1093F )
			{
				return "Lydian";
			}
			if ( 0x10A00 <= codePoint && codePoint <= 0x10A5F )
			{
				return "Kharoshthi";
			}
			if ( 0x10A60 <= codePoint && codePoint <= 0x10A7F )
			{
				return "Old South Arabian";
			}
			if ( 0x10B00 <= codePoint && codePoint <= 0x10B3F )
			{
				return "Avestan";
			}
			if ( 0x10B40 <= codePoint && codePoint <= 0x10B5F )
			{
				return "Inscriptional Parthian";
			}
			if ( 0x10B60 <= codePoint && codePoint <= 0x10B7F )
			{
				return "Inscriptional Pahlavi";
			}
			if ( 0x10C00 <= codePoint && codePoint <= 0x10C4F )
			{
				return "Old Turkic";
			}
			if ( 0x10E60 <= codePoint && codePoint <= 0x10E7F )
			{
				return "Rumi Numeral Symbols";
			}
			if ( 0x11000 <= codePoint && codePoint <= 0x1107F )
			{
				return "Brahmi";
			}
			if ( 0x11080 <= codePoint && codePoint <= 0x110CF )
			{
				return "Kaithi";
			}
			if ( 0x12000 <= codePoint && codePoint <= 0x123FF )
			{
				return "Cuneiform";
			}
			if ( 0x12400 <= codePoint && codePoint <= 0x1247F )
			{
				return "Cuneiform Numbers and Punctuation";
			}
			if ( 0x13000 <= codePoint && codePoint <= 0x1342F )
			{
				return "Egyptian Hieroglyphs";
			}
			if ( 0x16800 <= codePoint && codePoint <= 0x16A3F )
			{
				return "Bamum Supplement";
			}
			if ( 0x1B000 <= codePoint && codePoint <= 0x1B0FF )
			{
				return "Kana Supplement";
			}
			if ( 0x1D000 <= codePoint && codePoint <= 0x1D0FF )
			{
				return "Byzantine Musical Symbols";
			}
			if ( 0x1D100 <= codePoint && codePoint <= 0x1D1FF )
			{
				return "Musical Symbols";
			}
			if ( 0x1D200 <= codePoint && codePoint <= 0x1D24F )
			{
				return "Ancient Greek Musical Notation";
			}
			if ( 0x1D300 <= codePoint && codePoint <= 0x1D35F )
			{
				return "Tai Xuan Jing Symbols";
			}
			if ( 0x1D360 <= codePoint && codePoint <= 0x1D37F )
			{
				return "Counting Rod Numerals";
			}
			if ( 0x1D400 <= codePoint && codePoint <= 0x1D7FF )
			{
				return "Mathematical Alphanumeric Symbols";
			}
			if ( 0x1F000 <= codePoint && codePoint <= 0x1F02F )
			{
				return "Mahjong Tiles";
			}
			if ( 0x1F030 <= codePoint && codePoint <= 0x1F09F )
			{
				return "Domino Tiles";
			}
			if ( 0x1F0A0 <= codePoint && codePoint <= 0x1F0FF )
			{
				return "Playing Cards";
			}
			if ( 0x1F100 <= codePoint && codePoint <= 0x1F1FF )
			{
				return "Enclosed Alphanumeric Supplement";
			}
			if ( 0x1F200 <= codePoint && codePoint <= 0x1F2FF )
			{
				return "Enclosed Ideographic Supplement";
			}
			if ( 0x1F300 <= codePoint && codePoint <= 0x1F5FF )
			{
				return "Miscellaneous Symbols And Pictographs";
			}
			if ( 0x1F600 <= codePoint && codePoint <= 0x1F64F )
			{
				return "Emoticons";
			}
			if ( 0x1F680 <= codePoint && codePoint <= 0x1F6FF )
			{
				return "Transport And Map Symbols";
			}
			if ( 0x1F700 <= codePoint && codePoint <= 0x1F77F )
			{
				return "Alchemical Symbols";
			}
			if ( 0x20000 <= codePoint && codePoint <= 0x2A6DF )
			{
				return "CJK Unified Ideographs Extension B";
			}
			if ( 0x2A700 <= codePoint && codePoint <= 0x2B73F )
			{
				return "CJK Unified Ideographs Extension C";
			}
			if ( 0x2B740 <= codePoint && codePoint <= 0x2B81F )
			{
				return "CJK Unified Ideographs Extension D";
			}
			if ( 0x2F800 <= codePoint && codePoint <= 0x2FA1F )
			{
				return "CJK Compatibility Ideographs Supplement";
			}
			if ( 0xE0000 <= codePoint && codePoint <= 0xE007F )
			{
				return "Tags";
			}
			if ( 0xE0100 <= codePoint && codePoint <= 0xE01EF )
			{
				return "Variation Selectors Supplement";
			}
			if ( 0xF0000 <= codePoint && codePoint <= 0xFFFFF )
			{
				return "Supplementary Private Use Area-A";
			}
			if ( 0x100000 <= codePoint && codePoint <= 0x10FFFF )
			{
				return "Supplementary Private Use Area-B";
			}
			// dummy
			Contract.Assert( false, "Never reached." );
			return null;
		}
		
		/// <summary>
		///		Get localized unicode block name of specified UTF-32 code point with <see cref="CultureInfo.CurrentUICulture"/>.
		/// </summary>
		/// <param name="codePoint">UTF-32 code point.</param>
		/// <returns>Localized unicode block name.</returns>
		/// <include file='Remarks.xml' path='doc/NLiblet.Text/UnicodeUtility/GetUnicodeBlockName/remarks'/>
		public static string GetLocalizedUnicodeBlockName( int codePoint )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= codePoint );
			Contract.Requires<ArgumentOutOfRangeException>( codePoint <= 0x10FFFF );
			
			if ( 0x0000 <= codePoint && codePoint <= 0x007F )
			{
				return LocalizedBlockName.UnicoceBlockName_Basic_Latin; // Basic Latin
			}
			if ( 0x0080 <= codePoint && codePoint <= 0x00FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Latin_1_Supplement; // Latin-1 Supplement
			}
			if ( 0x0100 <= codePoint && codePoint <= 0x017F )
			{
				return LocalizedBlockName.UnicoceBlockName_Latin_Extended_A; // Latin Extended-A
			}
			if ( 0x0180 <= codePoint && codePoint <= 0x024F )
			{
				return LocalizedBlockName.UnicoceBlockName_Latin_Extended_B; // Latin Extended-B
			}
			if ( 0x0250 <= codePoint && codePoint <= 0x02AF )
			{
				return LocalizedBlockName.UnicoceBlockName_IPA_Extensions; // IPA Extensions
			}
			if ( 0x02B0 <= codePoint && codePoint <= 0x02FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Spacing_Modifier_Letters; // Spacing Modifier Letters
			}
			if ( 0x0300 <= codePoint && codePoint <= 0x036F )
			{
				return LocalizedBlockName.UnicoceBlockName_Combining_Diacritical_Marks; // Combining Diacritical Marks
			}
			if ( 0x0370 <= codePoint && codePoint <= 0x03FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Greek_and_Coptic; // Greek and Coptic
			}
			if ( 0x0400 <= codePoint && codePoint <= 0x04FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Cyrillic; // Cyrillic
			}
			if ( 0x0500 <= codePoint && codePoint <= 0x052F )
			{
				return LocalizedBlockName.UnicoceBlockName_Cyrillic_Supplement; // Cyrillic Supplement
			}
			if ( 0x0530 <= codePoint && codePoint <= 0x058F )
			{
				return LocalizedBlockName.UnicoceBlockName_Armenian; // Armenian
			}
			if ( 0x0590 <= codePoint && codePoint <= 0x05FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Hebrew; // Hebrew
			}
			if ( 0x0600 <= codePoint && codePoint <= 0x06FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Arabic; // Arabic
			}
			if ( 0x0700 <= codePoint && codePoint <= 0x074F )
			{
				return LocalizedBlockName.UnicoceBlockName_Syriac; // Syriac
			}
			if ( 0x0750 <= codePoint && codePoint <= 0x077F )
			{
				return LocalizedBlockName.UnicoceBlockName_Arabic_Supplement; // Arabic Supplement
			}
			if ( 0x0780 <= codePoint && codePoint <= 0x07BF )
			{
				return LocalizedBlockName.UnicoceBlockName_Thaana; // Thaana
			}
			if ( 0x07C0 <= codePoint && codePoint <= 0x07FF )
			{
				return LocalizedBlockName.UnicoceBlockName_NKo; // NKo
			}
			if ( 0x0800 <= codePoint && codePoint <= 0x083F )
			{
				return LocalizedBlockName.UnicoceBlockName_Samaritan; // Samaritan
			}
			if ( 0x0840 <= codePoint && codePoint <= 0x085F )
			{
				return LocalizedBlockName.UnicoceBlockName_Mandaic; // Mandaic
			}
			if ( 0x0900 <= codePoint && codePoint <= 0x097F )
			{
				return LocalizedBlockName.UnicoceBlockName_Devanagari; // Devanagari
			}
			if ( 0x0980 <= codePoint && codePoint <= 0x09FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Bengali; // Bengali
			}
			if ( 0x0A00 <= codePoint && codePoint <= 0x0A7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Gurmukhi; // Gurmukhi
			}
			if ( 0x0A80 <= codePoint && codePoint <= 0x0AFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Gujarati; // Gujarati
			}
			if ( 0x0B00 <= codePoint && codePoint <= 0x0B7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Oriya; // Oriya
			}
			if ( 0x0B80 <= codePoint && codePoint <= 0x0BFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Tamil; // Tamil
			}
			if ( 0x0C00 <= codePoint && codePoint <= 0x0C7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Telugu; // Telugu
			}
			if ( 0x0C80 <= codePoint && codePoint <= 0x0CFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Kannada; // Kannada
			}
			if ( 0x0D00 <= codePoint && codePoint <= 0x0D7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Malayalam; // Malayalam
			}
			if ( 0x0D80 <= codePoint && codePoint <= 0x0DFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Sinhala; // Sinhala
			}
			if ( 0x0E00 <= codePoint && codePoint <= 0x0E7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Thai; // Thai
			}
			if ( 0x0E80 <= codePoint && codePoint <= 0x0EFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Lao; // Lao
			}
			if ( 0x0F00 <= codePoint && codePoint <= 0x0FFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Tibetan; // Tibetan
			}
			if ( 0x1000 <= codePoint && codePoint <= 0x109F )
			{
				return LocalizedBlockName.UnicoceBlockName_Myanmar; // Myanmar
			}
			if ( 0x10A0 <= codePoint && codePoint <= 0x10FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Georgian; // Georgian
			}
			if ( 0x1100 <= codePoint && codePoint <= 0x11FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Hangul_Jamo; // Hangul Jamo
			}
			if ( 0x1200 <= codePoint && codePoint <= 0x137F )
			{
				return LocalizedBlockName.UnicoceBlockName_Ethiopic; // Ethiopic
			}
			if ( 0x1380 <= codePoint && codePoint <= 0x139F )
			{
				return LocalizedBlockName.UnicoceBlockName_Ethiopic_Supplement; // Ethiopic Supplement
			}
			if ( 0x13A0 <= codePoint && codePoint <= 0x13FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Cherokee; // Cherokee
			}
			if ( 0x1400 <= codePoint && codePoint <= 0x167F )
			{
				return LocalizedBlockName.UnicoceBlockName_Unified_Canadian_Aboriginal_Syllabics; // Unified Canadian Aboriginal Syllabics
			}
			if ( 0x1680 <= codePoint && codePoint <= 0x169F )
			{
				return LocalizedBlockName.UnicoceBlockName_Ogham; // Ogham
			}
			if ( 0x16A0 <= codePoint && codePoint <= 0x16FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Runic; // Runic
			}
			if ( 0x1700 <= codePoint && codePoint <= 0x171F )
			{
				return LocalizedBlockName.UnicoceBlockName_Tagalog; // Tagalog
			}
			if ( 0x1720 <= codePoint && codePoint <= 0x173F )
			{
				return LocalizedBlockName.UnicoceBlockName_Hanunoo; // Hanunoo
			}
			if ( 0x1740 <= codePoint && codePoint <= 0x175F )
			{
				return LocalizedBlockName.UnicoceBlockName_Buhid; // Buhid
			}
			if ( 0x1760 <= codePoint && codePoint <= 0x177F )
			{
				return LocalizedBlockName.UnicoceBlockName_Tagbanwa; // Tagbanwa
			}
			if ( 0x1780 <= codePoint && codePoint <= 0x17FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Khmer; // Khmer
			}
			if ( 0x1800 <= codePoint && codePoint <= 0x18AF )
			{
				return LocalizedBlockName.UnicoceBlockName_Mongolian; // Mongolian
			}
			if ( 0x18B0 <= codePoint && codePoint <= 0x18FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Unified_Canadian_Aboriginal_Syllabics_Extended; // Unified Canadian Aboriginal Syllabics Extended
			}
			if ( 0x1900 <= codePoint && codePoint <= 0x194F )
			{
				return LocalizedBlockName.UnicoceBlockName_Limbu; // Limbu
			}
			if ( 0x1950 <= codePoint && codePoint <= 0x197F )
			{
				return LocalizedBlockName.UnicoceBlockName_Tai_Le; // Tai Le
			}
			if ( 0x1980 <= codePoint && codePoint <= 0x19DF )
			{
				return LocalizedBlockName.UnicoceBlockName_New_Tai_Lue; // New Tai Lue
			}
			if ( 0x19E0 <= codePoint && codePoint <= 0x19FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Khmer_Symbols; // Khmer Symbols
			}
			if ( 0x1A00 <= codePoint && codePoint <= 0x1A1F )
			{
				return LocalizedBlockName.UnicoceBlockName_Buginese; // Buginese
			}
			if ( 0x1A20 <= codePoint && codePoint <= 0x1AAF )
			{
				return LocalizedBlockName.UnicoceBlockName_Tai_Tham; // Tai Tham
			}
			if ( 0x1B00 <= codePoint && codePoint <= 0x1B7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Balinese; // Balinese
			}
			if ( 0x1B80 <= codePoint && codePoint <= 0x1BBF )
			{
				return LocalizedBlockName.UnicoceBlockName_Sundanese; // Sundanese
			}
			if ( 0x1BC0 <= codePoint && codePoint <= 0x1BFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Batak; // Batak
			}
			if ( 0x1C00 <= codePoint && codePoint <= 0x1C4F )
			{
				return LocalizedBlockName.UnicoceBlockName_Lepcha; // Lepcha
			}
			if ( 0x1C50 <= codePoint && codePoint <= 0x1C7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Ol_Chiki; // Ol Chiki
			}
			if ( 0x1CD0 <= codePoint && codePoint <= 0x1CFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Vedic_Extensions; // Vedic Extensions
			}
			if ( 0x1D00 <= codePoint && codePoint <= 0x1D7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Phonetic_Extensions; // Phonetic Extensions
			}
			if ( 0x1D80 <= codePoint && codePoint <= 0x1DBF )
			{
				return LocalizedBlockName.UnicoceBlockName_Phonetic_Extensions_Supplement; // Phonetic Extensions Supplement
			}
			if ( 0x1DC0 <= codePoint && codePoint <= 0x1DFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Combining_Diacritical_Marks_Supplement; // Combining Diacritical Marks Supplement
			}
			if ( 0x1E00 <= codePoint && codePoint <= 0x1EFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Latin_Extended_Additional; // Latin Extended Additional
			}
			if ( 0x1F00 <= codePoint && codePoint <= 0x1FFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Greek_Extended; // Greek Extended
			}
			if ( 0x2000 <= codePoint && codePoint <= 0x206F )
			{
				return LocalizedBlockName.UnicoceBlockName_General_Punctuation; // General Punctuation
			}
			if ( 0x2070 <= codePoint && codePoint <= 0x209F )
			{
				return LocalizedBlockName.UnicoceBlockName_Superscripts_and_Subscripts; // Superscripts and Subscripts
			}
			if ( 0x20A0 <= codePoint && codePoint <= 0x20CF )
			{
				return LocalizedBlockName.UnicoceBlockName_Currency_Symbols; // Currency Symbols
			}
			if ( 0x20D0 <= codePoint && codePoint <= 0x20FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Combining_Diacritical_Marks_for_Symbols; // Combining Diacritical Marks for Symbols
			}
			if ( 0x2100 <= codePoint && codePoint <= 0x214F )
			{
				return LocalizedBlockName.UnicoceBlockName_Letterlike_Symbols; // Letterlike Symbols
			}
			if ( 0x2150 <= codePoint && codePoint <= 0x218F )
			{
				return LocalizedBlockName.UnicoceBlockName_Number_Forms; // Number Forms
			}
			if ( 0x2190 <= codePoint && codePoint <= 0x21FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Arrows; // Arrows
			}
			if ( 0x2200 <= codePoint && codePoint <= 0x22FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Mathematical_Operators; // Mathematical Operators
			}
			if ( 0x2300 <= codePoint && codePoint <= 0x23FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Miscellaneous_Technical; // Miscellaneous Technical
			}
			if ( 0x2400 <= codePoint && codePoint <= 0x243F )
			{
				return LocalizedBlockName.UnicoceBlockName_Control_Pictures; // Control Pictures
			}
			if ( 0x2440 <= codePoint && codePoint <= 0x245F )
			{
				return LocalizedBlockName.UnicoceBlockName_Optical_Character_Recognition; // Optical Character Recognition
			}
			if ( 0x2460 <= codePoint && codePoint <= 0x24FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Enclosed_Alphanumerics; // Enclosed Alphanumerics
			}
			if ( 0x2500 <= codePoint && codePoint <= 0x257F )
			{
				return LocalizedBlockName.UnicoceBlockName_Box_Drawing; // Box Drawing
			}
			if ( 0x2580 <= codePoint && codePoint <= 0x259F )
			{
				return LocalizedBlockName.UnicoceBlockName_Block_Elements; // Block Elements
			}
			if ( 0x25A0 <= codePoint && codePoint <= 0x25FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Geometric_Shapes; // Geometric Shapes
			}
			if ( 0x2600 <= codePoint && codePoint <= 0x26FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Miscellaneous_Symbols; // Miscellaneous Symbols
			}
			if ( 0x2700 <= codePoint && codePoint <= 0x27BF )
			{
				return LocalizedBlockName.UnicoceBlockName_Dingbats; // Dingbats
			}
			if ( 0x27C0 <= codePoint && codePoint <= 0x27EF )
			{
				return LocalizedBlockName.UnicoceBlockName_Miscellaneous_Mathematical_Symbols_A; // Miscellaneous Mathematical Symbols-A
			}
			if ( 0x27F0 <= codePoint && codePoint <= 0x27FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Supplemental_Arrows_A; // Supplemental Arrows-A
			}
			if ( 0x2800 <= codePoint && codePoint <= 0x28FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Braille_Patterns; // Braille Patterns
			}
			if ( 0x2900 <= codePoint && codePoint <= 0x297F )
			{
				return LocalizedBlockName.UnicoceBlockName_Supplemental_Arrows_B; // Supplemental Arrows-B
			}
			if ( 0x2980 <= codePoint && codePoint <= 0x29FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Miscellaneous_Mathematical_Symbols_B; // Miscellaneous Mathematical Symbols-B
			}
			if ( 0x2A00 <= codePoint && codePoint <= 0x2AFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Supplemental_Mathematical_Operators; // Supplemental Mathematical Operators
			}
			if ( 0x2B00 <= codePoint && codePoint <= 0x2BFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Miscellaneous_Symbols_and_Arrows; // Miscellaneous Symbols and Arrows
			}
			if ( 0x2C00 <= codePoint && codePoint <= 0x2C5F )
			{
				return LocalizedBlockName.UnicoceBlockName_Glagolitic; // Glagolitic
			}
			if ( 0x2C60 <= codePoint && codePoint <= 0x2C7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Latin_Extended_C; // Latin Extended-C
			}
			if ( 0x2C80 <= codePoint && codePoint <= 0x2CFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Coptic; // Coptic
			}
			if ( 0x2D00 <= codePoint && codePoint <= 0x2D2F )
			{
				return LocalizedBlockName.UnicoceBlockName_Georgian_Supplement; // Georgian Supplement
			}
			if ( 0x2D30 <= codePoint && codePoint <= 0x2D7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Tifinagh; // Tifinagh
			}
			if ( 0x2D80 <= codePoint && codePoint <= 0x2DDF )
			{
				return LocalizedBlockName.UnicoceBlockName_Ethiopic_Extended; // Ethiopic Extended
			}
			if ( 0x2DE0 <= codePoint && codePoint <= 0x2DFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Cyrillic_Extended_A; // Cyrillic Extended-A
			}
			if ( 0x2E00 <= codePoint && codePoint <= 0x2E7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Supplemental_Punctuation; // Supplemental Punctuation
			}
			if ( 0x2E80 <= codePoint && codePoint <= 0x2EFF )
			{
				return LocalizedBlockName.UnicoceBlockName_CJK_Radicals_Supplement; // CJK Radicals Supplement
			}
			if ( 0x2F00 <= codePoint && codePoint <= 0x2FDF )
			{
				return LocalizedBlockName.UnicoceBlockName_Kangxi_Radicals; // Kangxi Radicals
			}
			if ( 0x2FF0 <= codePoint && codePoint <= 0x2FFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Ideographic_Description_Characters; // Ideographic Description Characters
			}
			if ( 0x3000 <= codePoint && codePoint <= 0x303F )
			{
				return LocalizedBlockName.UnicoceBlockName_CJK_Symbols_and_Punctuation; // CJK Symbols and Punctuation
			}
			if ( 0x3040 <= codePoint && codePoint <= 0x309F )
			{
				return LocalizedBlockName.UnicoceBlockName_Hiragana; // Hiragana
			}
			if ( 0x30A0 <= codePoint && codePoint <= 0x30FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Katakana; // Katakana
			}
			if ( 0x3100 <= codePoint && codePoint <= 0x312F )
			{
				return LocalizedBlockName.UnicoceBlockName_Bopomofo; // Bopomofo
			}
			if ( 0x3130 <= codePoint && codePoint <= 0x318F )
			{
				return LocalizedBlockName.UnicoceBlockName_Hangul_Compatibility_Jamo; // Hangul Compatibility Jamo
			}
			if ( 0x3190 <= codePoint && codePoint <= 0x319F )
			{
				return LocalizedBlockName.UnicoceBlockName_Kanbun; // Kanbun
			}
			if ( 0x31A0 <= codePoint && codePoint <= 0x31BF )
			{
				return LocalizedBlockName.UnicoceBlockName_Bopomofo_Extended; // Bopomofo Extended
			}
			if ( 0x31C0 <= codePoint && codePoint <= 0x31EF )
			{
				return LocalizedBlockName.UnicoceBlockName_CJK_Strokes; // CJK Strokes
			}
			if ( 0x31F0 <= codePoint && codePoint <= 0x31FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Katakana_Phonetic_Extensions; // Katakana Phonetic Extensions
			}
			if ( 0x3200 <= codePoint && codePoint <= 0x32FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Enclosed_CJK_Letters_and_Months; // Enclosed CJK Letters and Months
			}
			if ( 0x3300 <= codePoint && codePoint <= 0x33FF )
			{
				return LocalizedBlockName.UnicoceBlockName_CJK_Compatibility; // CJK Compatibility
			}
			if ( 0x3400 <= codePoint && codePoint <= 0x4DBF )
			{
				return LocalizedBlockName.UnicoceBlockName_CJK_Unified_Ideographs_Extension_A; // CJK Unified Ideographs Extension A
			}
			if ( 0x4DC0 <= codePoint && codePoint <= 0x4DFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Yijing_Hexagram_Symbols; // Yijing Hexagram Symbols
			}
			if ( 0x4E00 <= codePoint && codePoint <= 0x9FFF )
			{
				return LocalizedBlockName.UnicoceBlockName_CJK_Unified_Ideographs; // CJK Unified Ideographs
			}
			if ( 0xA000 <= codePoint && codePoint <= 0xA48F )
			{
				return LocalizedBlockName.UnicoceBlockName_Yi_Syllables; // Yi Syllables
			}
			if ( 0xA490 <= codePoint && codePoint <= 0xA4CF )
			{
				return LocalizedBlockName.UnicoceBlockName_Yi_Radicals; // Yi Radicals
			}
			if ( 0xA4D0 <= codePoint && codePoint <= 0xA4FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Lisu; // Lisu
			}
			if ( 0xA500 <= codePoint && codePoint <= 0xA63F )
			{
				return LocalizedBlockName.UnicoceBlockName_Vai; // Vai
			}
			if ( 0xA640 <= codePoint && codePoint <= 0xA69F )
			{
				return LocalizedBlockName.UnicoceBlockName_Cyrillic_Extended_B; // Cyrillic Extended-B
			}
			if ( 0xA6A0 <= codePoint && codePoint <= 0xA6FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Bamum; // Bamum
			}
			if ( 0xA700 <= codePoint && codePoint <= 0xA71F )
			{
				return LocalizedBlockName.UnicoceBlockName_Modifier_Tone_Letters; // Modifier Tone Letters
			}
			if ( 0xA720 <= codePoint && codePoint <= 0xA7FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Latin_Extended_D; // Latin Extended-D
			}
			if ( 0xA800 <= codePoint && codePoint <= 0xA82F )
			{
				return LocalizedBlockName.UnicoceBlockName_Syloti_Nagri; // Syloti Nagri
			}
			if ( 0xA830 <= codePoint && codePoint <= 0xA83F )
			{
				return LocalizedBlockName.UnicoceBlockName_Common_Indic_Number_Forms; // Common Indic Number Forms
			}
			if ( 0xA840 <= codePoint && codePoint <= 0xA87F )
			{
				return LocalizedBlockName.UnicoceBlockName_Phags_pa; // Phags-pa
			}
			if ( 0xA880 <= codePoint && codePoint <= 0xA8DF )
			{
				return LocalizedBlockName.UnicoceBlockName_Saurashtra; // Saurashtra
			}
			if ( 0xA8E0 <= codePoint && codePoint <= 0xA8FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Devanagari_Extended; // Devanagari Extended
			}
			if ( 0xA900 <= codePoint && codePoint <= 0xA92F )
			{
				return LocalizedBlockName.UnicoceBlockName_Kayah_Li; // Kayah Li
			}
			if ( 0xA930 <= codePoint && codePoint <= 0xA95F )
			{
				return LocalizedBlockName.UnicoceBlockName_Rejang; // Rejang
			}
			if ( 0xA960 <= codePoint && codePoint <= 0xA97F )
			{
				return LocalizedBlockName.UnicoceBlockName_Hangul_Jamo_Extended_A; // Hangul Jamo Extended-A
			}
			if ( 0xA980 <= codePoint && codePoint <= 0xA9DF )
			{
				return LocalizedBlockName.UnicoceBlockName_Javanese; // Javanese
			}
			if ( 0xAA00 <= codePoint && codePoint <= 0xAA5F )
			{
				return LocalizedBlockName.UnicoceBlockName_Cham; // Cham
			}
			if ( 0xAA60 <= codePoint && codePoint <= 0xAA7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Myanmar_Extended_A; // Myanmar Extended-A
			}
			if ( 0xAA80 <= codePoint && codePoint <= 0xAADF )
			{
				return LocalizedBlockName.UnicoceBlockName_Tai_Viet; // Tai Viet
			}
			if ( 0xAB00 <= codePoint && codePoint <= 0xAB2F )
			{
				return LocalizedBlockName.UnicoceBlockName_Ethiopic_Extended_A; // Ethiopic Extended-A
			}
			if ( 0xABC0 <= codePoint && codePoint <= 0xABFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Meetei_Mayek; // Meetei Mayek
			}
			if ( 0xAC00 <= codePoint && codePoint <= 0xD7AF )
			{
				return LocalizedBlockName.UnicoceBlockName_Hangul_Syllables; // Hangul Syllables
			}
			if ( 0xD7B0 <= codePoint && codePoint <= 0xD7FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Hangul_Jamo_Extended_B; // Hangul Jamo Extended-B
			}
			if ( 0xD800 <= codePoint && codePoint <= 0xDB7F )
			{
				return LocalizedBlockName.UnicoceBlockName_High_Surrogates; // High Surrogates
			}
			if ( 0xDB80 <= codePoint && codePoint <= 0xDBFF )
			{
				return LocalizedBlockName.UnicoceBlockName_High_Private_Use_Surrogates; // High Private Use Surrogates
			}
			if ( 0xDC00 <= codePoint && codePoint <= 0xDFFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Low_Surrogates; // Low Surrogates
			}
			if ( 0xE000 <= codePoint && codePoint <= 0xF8FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Private_Use_Area; // Private Use Area
			}
			if ( 0xF900 <= codePoint && codePoint <= 0xFAFF )
			{
				return LocalizedBlockName.UnicoceBlockName_CJK_Compatibility_Ideographs; // CJK Compatibility Ideographs
			}
			if ( 0xFB00 <= codePoint && codePoint <= 0xFB4F )
			{
				return LocalizedBlockName.UnicoceBlockName_Alphabetic_Presentation_Forms; // Alphabetic Presentation Forms
			}
			if ( 0xFB50 <= codePoint && codePoint <= 0xFDFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Arabic_Presentation_Forms_A; // Arabic Presentation Forms-A
			}
			if ( 0xFE00 <= codePoint && codePoint <= 0xFE0F )
			{
				return LocalizedBlockName.UnicoceBlockName_Variation_Selectors; // Variation Selectors
			}
			if ( 0xFE10 <= codePoint && codePoint <= 0xFE1F )
			{
				return LocalizedBlockName.UnicoceBlockName_Vertical_Forms; // Vertical Forms
			}
			if ( 0xFE20 <= codePoint && codePoint <= 0xFE2F )
			{
				return LocalizedBlockName.UnicoceBlockName_Combining_Half_Marks; // Combining Half Marks
			}
			if ( 0xFE30 <= codePoint && codePoint <= 0xFE4F )
			{
				return LocalizedBlockName.UnicoceBlockName_CJK_Compatibility_Forms; // CJK Compatibility Forms
			}
			if ( 0xFE50 <= codePoint && codePoint <= 0xFE6F )
			{
				return LocalizedBlockName.UnicoceBlockName_Small_Form_Variants; // Small Form Variants
			}
			if ( 0xFE70 <= codePoint && codePoint <= 0xFEFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Arabic_Presentation_Forms_B; // Arabic Presentation Forms-B
			}
			if ( 0xFF00 <= codePoint && codePoint <= 0xFFEF )
			{
				return LocalizedBlockName.UnicoceBlockName_Halfwidth_and_Fullwidth_Forms; // Halfwidth and Fullwidth Forms
			}
			if ( 0xFFF0 <= codePoint && codePoint <= 0xFFFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Specials; // Specials
			}
			if ( 0x10000 <= codePoint && codePoint <= 0x1007F )
			{
				return LocalizedBlockName.UnicoceBlockName_Linear_B_Syllabary; // Linear B Syllabary
			}
			if ( 0x10080 <= codePoint && codePoint <= 0x100FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Linear_B_Ideograms; // Linear B Ideograms
			}
			if ( 0x10100 <= codePoint && codePoint <= 0x1013F )
			{
				return LocalizedBlockName.UnicoceBlockName_Aegean_Numbers; // Aegean Numbers
			}
			if ( 0x10140 <= codePoint && codePoint <= 0x1018F )
			{
				return LocalizedBlockName.UnicoceBlockName_Ancient_Greek_Numbers; // Ancient Greek Numbers
			}
			if ( 0x10190 <= codePoint && codePoint <= 0x101CF )
			{
				return LocalizedBlockName.UnicoceBlockName_Ancient_Symbols; // Ancient Symbols
			}
			if ( 0x101D0 <= codePoint && codePoint <= 0x101FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Phaistos_Disc; // Phaistos Disc
			}
			if ( 0x10280 <= codePoint && codePoint <= 0x1029F )
			{
				return LocalizedBlockName.UnicoceBlockName_Lycian; // Lycian
			}
			if ( 0x102A0 <= codePoint && codePoint <= 0x102DF )
			{
				return LocalizedBlockName.UnicoceBlockName_Carian; // Carian
			}
			if ( 0x10300 <= codePoint && codePoint <= 0x1032F )
			{
				return LocalizedBlockName.UnicoceBlockName_Old_Italic; // Old Italic
			}
			if ( 0x10330 <= codePoint && codePoint <= 0x1034F )
			{
				return LocalizedBlockName.UnicoceBlockName_Gothic; // Gothic
			}
			if ( 0x10380 <= codePoint && codePoint <= 0x1039F )
			{
				return LocalizedBlockName.UnicoceBlockName_Ugaritic; // Ugaritic
			}
			if ( 0x103A0 <= codePoint && codePoint <= 0x103DF )
			{
				return LocalizedBlockName.UnicoceBlockName_Old_Persian; // Old Persian
			}
			if ( 0x10400 <= codePoint && codePoint <= 0x1044F )
			{
				return LocalizedBlockName.UnicoceBlockName_Deseret; // Deseret
			}
			if ( 0x10450 <= codePoint && codePoint <= 0x1047F )
			{
				return LocalizedBlockName.UnicoceBlockName_Shavian; // Shavian
			}
			if ( 0x10480 <= codePoint && codePoint <= 0x104AF )
			{
				return LocalizedBlockName.UnicoceBlockName_Osmanya; // Osmanya
			}
			if ( 0x10800 <= codePoint && codePoint <= 0x1083F )
			{
				return LocalizedBlockName.UnicoceBlockName_Cypriot_Syllabary; // Cypriot Syllabary
			}
			if ( 0x10840 <= codePoint && codePoint <= 0x1085F )
			{
				return LocalizedBlockName.UnicoceBlockName_Imperial_Aramaic; // Imperial Aramaic
			}
			if ( 0x10900 <= codePoint && codePoint <= 0x1091F )
			{
				return LocalizedBlockName.UnicoceBlockName_Phoenician; // Phoenician
			}
			if ( 0x10920 <= codePoint && codePoint <= 0x1093F )
			{
				return LocalizedBlockName.UnicoceBlockName_Lydian; // Lydian
			}
			if ( 0x10A00 <= codePoint && codePoint <= 0x10A5F )
			{
				return LocalizedBlockName.UnicoceBlockName_Kharoshthi; // Kharoshthi
			}
			if ( 0x10A60 <= codePoint && codePoint <= 0x10A7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Old_South_Arabian; // Old South Arabian
			}
			if ( 0x10B00 <= codePoint && codePoint <= 0x10B3F )
			{
				return LocalizedBlockName.UnicoceBlockName_Avestan; // Avestan
			}
			if ( 0x10B40 <= codePoint && codePoint <= 0x10B5F )
			{
				return LocalizedBlockName.UnicoceBlockName_Inscriptional_Parthian; // Inscriptional Parthian
			}
			if ( 0x10B60 <= codePoint && codePoint <= 0x10B7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Inscriptional_Pahlavi; // Inscriptional Pahlavi
			}
			if ( 0x10C00 <= codePoint && codePoint <= 0x10C4F )
			{
				return LocalizedBlockName.UnicoceBlockName_Old_Turkic; // Old Turkic
			}
			if ( 0x10E60 <= codePoint && codePoint <= 0x10E7F )
			{
				return LocalizedBlockName.UnicoceBlockName_Rumi_Numeral_Symbols; // Rumi Numeral Symbols
			}
			if ( 0x11000 <= codePoint && codePoint <= 0x1107F )
			{
				return LocalizedBlockName.UnicoceBlockName_Brahmi; // Brahmi
			}
			if ( 0x11080 <= codePoint && codePoint <= 0x110CF )
			{
				return LocalizedBlockName.UnicoceBlockName_Kaithi; // Kaithi
			}
			if ( 0x12000 <= codePoint && codePoint <= 0x123FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Cuneiform; // Cuneiform
			}
			if ( 0x12400 <= codePoint && codePoint <= 0x1247F )
			{
				return LocalizedBlockName.UnicoceBlockName_Cuneiform_Numbers_and_Punctuation; // Cuneiform Numbers and Punctuation
			}
			if ( 0x13000 <= codePoint && codePoint <= 0x1342F )
			{
				return LocalizedBlockName.UnicoceBlockName_Egyptian_Hieroglyphs; // Egyptian Hieroglyphs
			}
			if ( 0x16800 <= codePoint && codePoint <= 0x16A3F )
			{
				return LocalizedBlockName.UnicoceBlockName_Bamum_Supplement; // Bamum Supplement
			}
			if ( 0x1B000 <= codePoint && codePoint <= 0x1B0FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Kana_Supplement; // Kana Supplement
			}
			if ( 0x1D000 <= codePoint && codePoint <= 0x1D0FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Byzantine_Musical_Symbols; // Byzantine Musical Symbols
			}
			if ( 0x1D100 <= codePoint && codePoint <= 0x1D1FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Musical_Symbols; // Musical Symbols
			}
			if ( 0x1D200 <= codePoint && codePoint <= 0x1D24F )
			{
				return LocalizedBlockName.UnicoceBlockName_Ancient_Greek_Musical_Notation; // Ancient Greek Musical Notation
			}
			if ( 0x1D300 <= codePoint && codePoint <= 0x1D35F )
			{
				return LocalizedBlockName.UnicoceBlockName_Tai_Xuan_Jing_Symbols; // Tai Xuan Jing Symbols
			}
			if ( 0x1D360 <= codePoint && codePoint <= 0x1D37F )
			{
				return LocalizedBlockName.UnicoceBlockName_Counting_Rod_Numerals; // Counting Rod Numerals
			}
			if ( 0x1D400 <= codePoint && codePoint <= 0x1D7FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Mathematical_Alphanumeric_Symbols; // Mathematical Alphanumeric Symbols
			}
			if ( 0x1F000 <= codePoint && codePoint <= 0x1F02F )
			{
				return LocalizedBlockName.UnicoceBlockName_Mahjong_Tiles; // Mahjong Tiles
			}
			if ( 0x1F030 <= codePoint && codePoint <= 0x1F09F )
			{
				return LocalizedBlockName.UnicoceBlockName_Domino_Tiles; // Domino Tiles
			}
			if ( 0x1F0A0 <= codePoint && codePoint <= 0x1F0FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Playing_Cards; // Playing Cards
			}
			if ( 0x1F100 <= codePoint && codePoint <= 0x1F1FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Enclosed_Alphanumeric_Supplement; // Enclosed Alphanumeric Supplement
			}
			if ( 0x1F200 <= codePoint && codePoint <= 0x1F2FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Enclosed_Ideographic_Supplement; // Enclosed Ideographic Supplement
			}
			if ( 0x1F300 <= codePoint && codePoint <= 0x1F5FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Miscellaneous_Symbols_And_Pictographs; // Miscellaneous Symbols And Pictographs
			}
			if ( 0x1F600 <= codePoint && codePoint <= 0x1F64F )
			{
				return LocalizedBlockName.UnicoceBlockName_Emoticons; // Emoticons
			}
			if ( 0x1F680 <= codePoint && codePoint <= 0x1F6FF )
			{
				return LocalizedBlockName.UnicoceBlockName_Transport_And_Map_Symbols; // Transport And Map Symbols
			}
			if ( 0x1F700 <= codePoint && codePoint <= 0x1F77F )
			{
				return LocalizedBlockName.UnicoceBlockName_Alchemical_Symbols; // Alchemical Symbols
			}
			if ( 0x20000 <= codePoint && codePoint <= 0x2A6DF )
			{
				return LocalizedBlockName.UnicoceBlockName_CJK_Unified_Ideographs_Extension_B; // CJK Unified Ideographs Extension B
			}
			if ( 0x2A700 <= codePoint && codePoint <= 0x2B73F )
			{
				return LocalizedBlockName.UnicoceBlockName_CJK_Unified_Ideographs_Extension_C; // CJK Unified Ideographs Extension C
			}
			if ( 0x2B740 <= codePoint && codePoint <= 0x2B81F )
			{
				return LocalizedBlockName.UnicoceBlockName_CJK_Unified_Ideographs_Extension_D; // CJK Unified Ideographs Extension D
			}
			if ( 0x2F800 <= codePoint && codePoint <= 0x2FA1F )
			{
				return LocalizedBlockName.UnicoceBlockName_CJK_Compatibility_Ideographs_Supplement; // CJK Compatibility Ideographs Supplement
			}
			if ( 0xE0000 <= codePoint && codePoint <= 0xE007F )
			{
				return LocalizedBlockName.UnicoceBlockName_Tags; // Tags
			}
			if ( 0xE0100 <= codePoint && codePoint <= 0xE01EF )
			{
				return LocalizedBlockName.UnicoceBlockName_Variation_Selectors_Supplement; // Variation Selectors Supplement
			}
			if ( 0xF0000 <= codePoint && codePoint <= 0xFFFFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Supplementary_Private_Use_Area_A; // Supplementary Private Use Area-A
			}
			if ( 0x100000 <= codePoint && codePoint <= 0x10FFFF )
			{
				return LocalizedBlockName.UnicoceBlockName_Supplementary_Private_Use_Area_B; // Supplementary Private Use Area-B
			}
			// dummy
			Contract.Assert( false, "Never reached." );
			return null;
		}
	}
}
