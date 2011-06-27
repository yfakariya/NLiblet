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
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace NLiblet.Text
{
	partial class UnicodeUtility
	{
		public static string GetUnicodeBlockName( int utf32 )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= utf32 && utf32 <= 0x10FFFF );
			
			if ( 0x0000 <= utf32 && utf32 <= 0x007F )
			{
				return "Basic Latin";
			}
			if ( 0x0080 <= utf32 && utf32 <= 0x00FF )
			{
				return "Latin-1 Supplement";
			}
			if ( 0x0100 <= utf32 && utf32 <= 0x017F )
			{
				return "Latin Extended-A";
			}
			if ( 0x0180 <= utf32 && utf32 <= 0x024F )
			{
				return "Latin Extended-B";
			}
			if ( 0x0250 <= utf32 && utf32 <= 0x02AF )
			{
				return "IPA Extensions";
			}
			if ( 0x02B0 <= utf32 && utf32 <= 0x02FF )
			{
				return "Spacing Modifier Letters";
			}
			if ( 0x0300 <= utf32 && utf32 <= 0x036F )
			{
				return "Combining Diacritical Marks";
			}
			if ( 0x0370 <= utf32 && utf32 <= 0x03FF )
			{
				return "Greek and Coptic";
			}
			if ( 0x0400 <= utf32 && utf32 <= 0x04FF )
			{
				return "Cyrillic";
			}
			if ( 0x0500 <= utf32 && utf32 <= 0x052F )
			{
				return "Cyrillic Supplement";
			}
			if ( 0x0530 <= utf32 && utf32 <= 0x058F )
			{
				return "Armenian";
			}
			if ( 0x0590 <= utf32 && utf32 <= 0x05FF )
			{
				return "Hebrew";
			}
			if ( 0x0600 <= utf32 && utf32 <= 0x06FF )
			{
				return "Arabic";
			}
			if ( 0x0700 <= utf32 && utf32 <= 0x074F )
			{
				return "Syriac";
			}
			if ( 0x0750 <= utf32 && utf32 <= 0x077F )
			{
				return "Arabic Supplement";
			}
			if ( 0x0780 <= utf32 && utf32 <= 0x07BF )
			{
				return "Thaana";
			}
			if ( 0x07C0 <= utf32 && utf32 <= 0x07FF )
			{
				return "NKo";
			}
			if ( 0x0800 <= utf32 && utf32 <= 0x083F )
			{
				return "Samaritan";
			}
			if ( 0x0840 <= utf32 && utf32 <= 0x085F )
			{
				return "Mandaic";
			}
			if ( 0x0900 <= utf32 && utf32 <= 0x097F )
			{
				return "Devanagari";
			}
			if ( 0x0980 <= utf32 && utf32 <= 0x09FF )
			{
				return "Bengali";
			}
			if ( 0x0A00 <= utf32 && utf32 <= 0x0A7F )
			{
				return "Gurmukhi";
			}
			if ( 0x0A80 <= utf32 && utf32 <= 0x0AFF )
			{
				return "Gujarati";
			}
			if ( 0x0B00 <= utf32 && utf32 <= 0x0B7F )
			{
				return "Oriya";
			}
			if ( 0x0B80 <= utf32 && utf32 <= 0x0BFF )
			{
				return "Tamil";
			}
			if ( 0x0C00 <= utf32 && utf32 <= 0x0C7F )
			{
				return "Telugu";
			}
			if ( 0x0C80 <= utf32 && utf32 <= 0x0CFF )
			{
				return "Kannada";
			}
			if ( 0x0D00 <= utf32 && utf32 <= 0x0D7F )
			{
				return "Malayalam";
			}
			if ( 0x0D80 <= utf32 && utf32 <= 0x0DFF )
			{
				return "Sinhala";
			}
			if ( 0x0E00 <= utf32 && utf32 <= 0x0E7F )
			{
				return "Thai";
			}
			if ( 0x0E80 <= utf32 && utf32 <= 0x0EFF )
			{
				return "Lao";
			}
			if ( 0x0F00 <= utf32 && utf32 <= 0x0FFF )
			{
				return "Tibetan";
			}
			if ( 0x1000 <= utf32 && utf32 <= 0x109F )
			{
				return "Myanmar";
			}
			if ( 0x10A0 <= utf32 && utf32 <= 0x10FF )
			{
				return "Georgian";
			}
			if ( 0x1100 <= utf32 && utf32 <= 0x11FF )
			{
				return "Hangul Jamo";
			}
			if ( 0x1200 <= utf32 && utf32 <= 0x137F )
			{
				return "Ethiopic";
			}
			if ( 0x1380 <= utf32 && utf32 <= 0x139F )
			{
				return "Ethiopic Supplement";
			}
			if ( 0x13A0 <= utf32 && utf32 <= 0x13FF )
			{
				return "Cherokee";
			}
			if ( 0x1400 <= utf32 && utf32 <= 0x167F )
			{
				return "Unified Canadian Aboriginal Syllabics";
			}
			if ( 0x1680 <= utf32 && utf32 <= 0x169F )
			{
				return "Ogham";
			}
			if ( 0x16A0 <= utf32 && utf32 <= 0x16FF )
			{
				return "Runic";
			}
			if ( 0x1700 <= utf32 && utf32 <= 0x171F )
			{
				return "Tagalog";
			}
			if ( 0x1720 <= utf32 && utf32 <= 0x173F )
			{
				return "Hanunoo";
			}
			if ( 0x1740 <= utf32 && utf32 <= 0x175F )
			{
				return "Buhid";
			}
			if ( 0x1760 <= utf32 && utf32 <= 0x177F )
			{
				return "Tagbanwa";
			}
			if ( 0x1780 <= utf32 && utf32 <= 0x17FF )
			{
				return "Khmer";
			}
			if ( 0x1800 <= utf32 && utf32 <= 0x18AF )
			{
				return "Mongolian";
			}
			if ( 0x18B0 <= utf32 && utf32 <= 0x18FF )
			{
				return "Unified Canadian Aboriginal Syllabics Extended";
			}
			if ( 0x1900 <= utf32 && utf32 <= 0x194F )
			{
				return "Limbu";
			}
			if ( 0x1950 <= utf32 && utf32 <= 0x197F )
			{
				return "Tai Le";
			}
			if ( 0x1980 <= utf32 && utf32 <= 0x19DF )
			{
				return "New Tai Lue";
			}
			if ( 0x19E0 <= utf32 && utf32 <= 0x19FF )
			{
				return "Khmer Symbols";
			}
			if ( 0x1A00 <= utf32 && utf32 <= 0x1A1F )
			{
				return "Buginese";
			}
			if ( 0x1A20 <= utf32 && utf32 <= 0x1AAF )
			{
				return "Tai Tham";
			}
			if ( 0x1B00 <= utf32 && utf32 <= 0x1B7F )
			{
				return "Balinese";
			}
			if ( 0x1B80 <= utf32 && utf32 <= 0x1BBF )
			{
				return "Sundanese";
			}
			if ( 0x1BC0 <= utf32 && utf32 <= 0x1BFF )
			{
				return "Batak";
			}
			if ( 0x1C00 <= utf32 && utf32 <= 0x1C4F )
			{
				return "Lepcha";
			}
			if ( 0x1C50 <= utf32 && utf32 <= 0x1C7F )
			{
				return "Ol Chiki";
			}
			if ( 0x1CD0 <= utf32 && utf32 <= 0x1CFF )
			{
				return "Vedic Extensions";
			}
			if ( 0x1D00 <= utf32 && utf32 <= 0x1D7F )
			{
				return "Phonetic Extensions";
			}
			if ( 0x1D80 <= utf32 && utf32 <= 0x1DBF )
			{
				return "Phonetic Extensions Supplement";
			}
			if ( 0x1DC0 <= utf32 && utf32 <= 0x1DFF )
			{
				return "Combining Diacritical Marks Supplement";
			}
			if ( 0x1E00 <= utf32 && utf32 <= 0x1EFF )
			{
				return "Latin Extended Additional";
			}
			if ( 0x1F00 <= utf32 && utf32 <= 0x1FFF )
			{
				return "Greek Extended";
			}
			if ( 0x2000 <= utf32 && utf32 <= 0x206F )
			{
				return "General Punctuation";
			}
			if ( 0x2070 <= utf32 && utf32 <= 0x209F )
			{
				return "Superscripts and Subscripts";
			}
			if ( 0x20A0 <= utf32 && utf32 <= 0x20CF )
			{
				return "Currency Symbols";
			}
			if ( 0x20D0 <= utf32 && utf32 <= 0x20FF )
			{
				return "Combining Diacritical Marks for Symbols";
			}
			if ( 0x2100 <= utf32 && utf32 <= 0x214F )
			{
				return "Letterlike Symbols";
			}
			if ( 0x2150 <= utf32 && utf32 <= 0x218F )
			{
				return "Number Forms";
			}
			if ( 0x2190 <= utf32 && utf32 <= 0x21FF )
			{
				return "Arrows";
			}
			if ( 0x2200 <= utf32 && utf32 <= 0x22FF )
			{
				return "Mathematical Operators";
			}
			if ( 0x2300 <= utf32 && utf32 <= 0x23FF )
			{
				return "Miscellaneous Technical";
			}
			if ( 0x2400 <= utf32 && utf32 <= 0x243F )
			{
				return "Control Pictures";
			}
			if ( 0x2440 <= utf32 && utf32 <= 0x245F )
			{
				return "Optical Character Recognition";
			}
			if ( 0x2460 <= utf32 && utf32 <= 0x24FF )
			{
				return "Enclosed Alphanumerics";
			}
			if ( 0x2500 <= utf32 && utf32 <= 0x257F )
			{
				return "Box Drawing";
			}
			if ( 0x2580 <= utf32 && utf32 <= 0x259F )
			{
				return "Block Elements";
			}
			if ( 0x25A0 <= utf32 && utf32 <= 0x25FF )
			{
				return "Geometric Shapes";
			}
			if ( 0x2600 <= utf32 && utf32 <= 0x26FF )
			{
				return "Miscellaneous Symbols";
			}
			if ( 0x2700 <= utf32 && utf32 <= 0x27BF )
			{
				return "Dingbats";
			}
			if ( 0x27C0 <= utf32 && utf32 <= 0x27EF )
			{
				return "Miscellaneous Mathematical Symbols-A";
			}
			if ( 0x27F0 <= utf32 && utf32 <= 0x27FF )
			{
				return "Supplemental Arrows-A";
			}
			if ( 0x2800 <= utf32 && utf32 <= 0x28FF )
			{
				return "Braille Patterns";
			}
			if ( 0x2900 <= utf32 && utf32 <= 0x297F )
			{
				return "Supplemental Arrows-B";
			}
			if ( 0x2980 <= utf32 && utf32 <= 0x29FF )
			{
				return "Miscellaneous Mathematical Symbols-B";
			}
			if ( 0x2A00 <= utf32 && utf32 <= 0x2AFF )
			{
				return "Supplemental Mathematical Operators";
			}
			if ( 0x2B00 <= utf32 && utf32 <= 0x2BFF )
			{
				return "Miscellaneous Symbols and Arrows";
			}
			if ( 0x2C00 <= utf32 && utf32 <= 0x2C5F )
			{
				return "Glagolitic";
			}
			if ( 0x2C60 <= utf32 && utf32 <= 0x2C7F )
			{
				return "Latin Extended-C";
			}
			if ( 0x2C80 <= utf32 && utf32 <= 0x2CFF )
			{
				return "Coptic";
			}
			if ( 0x2D00 <= utf32 && utf32 <= 0x2D2F )
			{
				return "Georgian Supplement";
			}
			if ( 0x2D30 <= utf32 && utf32 <= 0x2D7F )
			{
				return "Tifinagh";
			}
			if ( 0x2D80 <= utf32 && utf32 <= 0x2DDF )
			{
				return "Ethiopic Extended";
			}
			if ( 0x2DE0 <= utf32 && utf32 <= 0x2DFF )
			{
				return "Cyrillic Extended-A";
			}
			if ( 0x2E00 <= utf32 && utf32 <= 0x2E7F )
			{
				return "Supplemental Punctuation";
			}
			if ( 0x2E80 <= utf32 && utf32 <= 0x2EFF )
			{
				return "CJK Radicals Supplement";
			}
			if ( 0x2F00 <= utf32 && utf32 <= 0x2FDF )
			{
				return "Kangxi Radicals";
			}
			if ( 0x2FF0 <= utf32 && utf32 <= 0x2FFF )
			{
				return "Ideographic Description Characters";
			}
			if ( 0x3000 <= utf32 && utf32 <= 0x303F )
			{
				return "CJK Symbols and Punctuation";
			}
			if ( 0x3040 <= utf32 && utf32 <= 0x309F )
			{
				return "Hiragana";
			}
			if ( 0x30A0 <= utf32 && utf32 <= 0x30FF )
			{
				return "Katakana";
			}
			if ( 0x3100 <= utf32 && utf32 <= 0x312F )
			{
				return "Bopomofo";
			}
			if ( 0x3130 <= utf32 && utf32 <= 0x318F )
			{
				return "Hangul Compatibility Jamo";
			}
			if ( 0x3190 <= utf32 && utf32 <= 0x319F )
			{
				return "Kanbun";
			}
			if ( 0x31A0 <= utf32 && utf32 <= 0x31BF )
			{
				return "Bopomofo Extended";
			}
			if ( 0x31C0 <= utf32 && utf32 <= 0x31EF )
			{
				return "CJK Strokes";
			}
			if ( 0x31F0 <= utf32 && utf32 <= 0x31FF )
			{
				return "Katakana Phonetic Extensions";
			}
			if ( 0x3200 <= utf32 && utf32 <= 0x32FF )
			{
				return "Enclosed CJK Letters and Months";
			}
			if ( 0x3300 <= utf32 && utf32 <= 0x33FF )
			{
				return "CJK Compatibility";
			}
			if ( 0x3400 <= utf32 && utf32 <= 0x4DBF )
			{
				return "CJK Unified Ideographs Extension A";
			}
			if ( 0x4DC0 <= utf32 && utf32 <= 0x4DFF )
			{
				return "Yijing Hexagram Symbols";
			}
			if ( 0x4E00 <= utf32 && utf32 <= 0x9FFF )
			{
				return "CJK Unified Ideographs";
			}
			if ( 0xA000 <= utf32 && utf32 <= 0xA48F )
			{
				return "Yi Syllables";
			}
			if ( 0xA490 <= utf32 && utf32 <= 0xA4CF )
			{
				return "Yi Radicals";
			}
			if ( 0xA4D0 <= utf32 && utf32 <= 0xA4FF )
			{
				return "Lisu";
			}
			if ( 0xA500 <= utf32 && utf32 <= 0xA63F )
			{
				return "Vai";
			}
			if ( 0xA640 <= utf32 && utf32 <= 0xA69F )
			{
				return "Cyrillic Extended-B";
			}
			if ( 0xA6A0 <= utf32 && utf32 <= 0xA6FF )
			{
				return "Bamum";
			}
			if ( 0xA700 <= utf32 && utf32 <= 0xA71F )
			{
				return "Modifier Tone Letters";
			}
			if ( 0xA720 <= utf32 && utf32 <= 0xA7FF )
			{
				return "Latin Extended-D";
			}
			if ( 0xA800 <= utf32 && utf32 <= 0xA82F )
			{
				return "Syloti Nagri";
			}
			if ( 0xA830 <= utf32 && utf32 <= 0xA83F )
			{
				return "Common Indic Number Forms";
			}
			if ( 0xA840 <= utf32 && utf32 <= 0xA87F )
			{
				return "Phags-pa";
			}
			if ( 0xA880 <= utf32 && utf32 <= 0xA8DF )
			{
				return "Saurashtra";
			}
			if ( 0xA8E0 <= utf32 && utf32 <= 0xA8FF )
			{
				return "Devanagari Extended";
			}
			if ( 0xA900 <= utf32 && utf32 <= 0xA92F )
			{
				return "Kayah Li";
			}
			if ( 0xA930 <= utf32 && utf32 <= 0xA95F )
			{
				return "Rejang";
			}
			if ( 0xA960 <= utf32 && utf32 <= 0xA97F )
			{
				return "Hangul Jamo Extended-A";
			}
			if ( 0xA980 <= utf32 && utf32 <= 0xA9DF )
			{
				return "Javanese";
			}
			if ( 0xAA00 <= utf32 && utf32 <= 0xAA5F )
			{
				return "Cham";
			}
			if ( 0xAA60 <= utf32 && utf32 <= 0xAA7F )
			{
				return "Myanmar Extended-A";
			}
			if ( 0xAA80 <= utf32 && utf32 <= 0xAADF )
			{
				return "Tai Viet";
			}
			if ( 0xAB00 <= utf32 && utf32 <= 0xAB2F )
			{
				return "Ethiopic Extended-A";
			}
			if ( 0xABC0 <= utf32 && utf32 <= 0xABFF )
			{
				return "Meetei Mayek";
			}
			if ( 0xAC00 <= utf32 && utf32 <= 0xD7AF )
			{
				return "Hangul Syllables";
			}
			if ( 0xD7B0 <= utf32 && utf32 <= 0xD7FF )
			{
				return "Hangul Jamo Extended-B";
			}
			if ( 0xD800 <= utf32 && utf32 <= 0xDB7F )
			{
				return "High Surrogates";
			}
			if ( 0xDB80 <= utf32 && utf32 <= 0xDBFF )
			{
				return "High Private Use Surrogates";
			}
			if ( 0xDC00 <= utf32 && utf32 <= 0xDFFF )
			{
				return "Low Surrogates";
			}
			if ( 0xE000 <= utf32 && utf32 <= 0xF8FF )
			{
				return "Private Use Area";
			}
			if ( 0xF900 <= utf32 && utf32 <= 0xFAFF )
			{
				return "CJK Compatibility Ideographs";
			}
			if ( 0xFB00 <= utf32 && utf32 <= 0xFB4F )
			{
				return "Alphabetic Presentation Forms";
			}
			if ( 0xFB50 <= utf32 && utf32 <= 0xFDFF )
			{
				return "Arabic Presentation Forms-A";
			}
			if ( 0xFE00 <= utf32 && utf32 <= 0xFE0F )
			{
				return "Variation Selectors";
			}
			if ( 0xFE10 <= utf32 && utf32 <= 0xFE1F )
			{
				return "Vertical Forms";
			}
			if ( 0xFE20 <= utf32 && utf32 <= 0xFE2F )
			{
				return "Combining Half Marks";
			}
			if ( 0xFE30 <= utf32 && utf32 <= 0xFE4F )
			{
				return "CJK Compatibility Forms";
			}
			if ( 0xFE50 <= utf32 && utf32 <= 0xFE6F )
			{
				return "Small Form Variants";
			}
			if ( 0xFE70 <= utf32 && utf32 <= 0xFEFF )
			{
				return "Arabic Presentation Forms-B";
			}
			if ( 0xFF00 <= utf32 && utf32 <= 0xFFEF )
			{
				return "Halfwidth and Fullwidth Forms";
			}
			if ( 0xFFF0 <= utf32 && utf32 <= 0xFFFF )
			{
				return "Specials";
			}
			if ( 0x10000 <= utf32 && utf32 <= 0x1007F )
			{
				return "Linear B Syllabary";
			}
			if ( 0x10080 <= utf32 && utf32 <= 0x100FF )
			{
				return "Linear B Ideograms";
			}
			if ( 0x10100 <= utf32 && utf32 <= 0x1013F )
			{
				return "Aegean Numbers";
			}
			if ( 0x10140 <= utf32 && utf32 <= 0x1018F )
			{
				return "Ancient Greek Numbers";
			}
			if ( 0x10190 <= utf32 && utf32 <= 0x101CF )
			{
				return "Ancient Symbols";
			}
			if ( 0x101D0 <= utf32 && utf32 <= 0x101FF )
			{
				return "Phaistos Disc";
			}
			if ( 0x10280 <= utf32 && utf32 <= 0x1029F )
			{
				return "Lycian";
			}
			if ( 0x102A0 <= utf32 && utf32 <= 0x102DF )
			{
				return "Carian";
			}
			if ( 0x10300 <= utf32 && utf32 <= 0x1032F )
			{
				return "Old Italic";
			}
			if ( 0x10330 <= utf32 && utf32 <= 0x1034F )
			{
				return "Gothic";
			}
			if ( 0x10380 <= utf32 && utf32 <= 0x1039F )
			{
				return "Ugaritic";
			}
			if ( 0x103A0 <= utf32 && utf32 <= 0x103DF )
			{
				return "Old Persian";
			}
			if ( 0x10400 <= utf32 && utf32 <= 0x1044F )
			{
				return "Deseret";
			}
			if ( 0x10450 <= utf32 && utf32 <= 0x1047F )
			{
				return "Shavian";
			}
			if ( 0x10480 <= utf32 && utf32 <= 0x104AF )
			{
				return "Osmanya";
			}
			if ( 0x10800 <= utf32 && utf32 <= 0x1083F )
			{
				return "Cypriot Syllabary";
			}
			if ( 0x10840 <= utf32 && utf32 <= 0x1085F )
			{
				return "Imperial Aramaic";
			}
			if ( 0x10900 <= utf32 && utf32 <= 0x1091F )
			{
				return "Phoenician";
			}
			if ( 0x10920 <= utf32 && utf32 <= 0x1093F )
			{
				return "Lydian";
			}
			if ( 0x10A00 <= utf32 && utf32 <= 0x10A5F )
			{
				return "Kharoshthi";
			}
			if ( 0x10A60 <= utf32 && utf32 <= 0x10A7F )
			{
				return "Old South Arabian";
			}
			if ( 0x10B00 <= utf32 && utf32 <= 0x10B3F )
			{
				return "Avestan";
			}
			if ( 0x10B40 <= utf32 && utf32 <= 0x10B5F )
			{
				return "Inscriptional Parthian";
			}
			if ( 0x10B60 <= utf32 && utf32 <= 0x10B7F )
			{
				return "Inscriptional Pahlavi";
			}
			if ( 0x10C00 <= utf32 && utf32 <= 0x10C4F )
			{
				return "Old Turkic";
			}
			if ( 0x10E60 <= utf32 && utf32 <= 0x10E7F )
			{
				return "Rumi Numeral Symbols";
			}
			if ( 0x11000 <= utf32 && utf32 <= 0x1107F )
			{
				return "Brahmi";
			}
			if ( 0x11080 <= utf32 && utf32 <= 0x110CF )
			{
				return "Kaithi";
			}
			if ( 0x12000 <= utf32 && utf32 <= 0x123FF )
			{
				return "Cuneiform";
			}
			if ( 0x12400 <= utf32 && utf32 <= 0x1247F )
			{
				return "Cuneiform Numbers and Punctuation";
			}
			if ( 0x13000 <= utf32 && utf32 <= 0x1342F )
			{
				return "Egyptian Hieroglyphs";
			}
			if ( 0x16800 <= utf32 && utf32 <= 0x16A3F )
			{
				return "Bamum Supplement";
			}
			if ( 0x1B000 <= utf32 && utf32 <= 0x1B0FF )
			{
				return "Kana Supplement";
			}
			if ( 0x1D000 <= utf32 && utf32 <= 0x1D0FF )
			{
				return "Byzantine Musical Symbols";
			}
			if ( 0x1D100 <= utf32 && utf32 <= 0x1D1FF )
			{
				return "Musical Symbols";
			}
			if ( 0x1D200 <= utf32 && utf32 <= 0x1D24F )
			{
				return "Ancient Greek Musical Notation";
			}
			if ( 0x1D300 <= utf32 && utf32 <= 0x1D35F )
			{
				return "Tai Xuan Jing Symbols";
			}
			if ( 0x1D360 <= utf32 && utf32 <= 0x1D37F )
			{
				return "Counting Rod Numerals";
			}
			if ( 0x1D400 <= utf32 && utf32 <= 0x1D7FF )
			{
				return "Mathematical Alphanumeric Symbols";
			}
			if ( 0x1F000 <= utf32 && utf32 <= 0x1F02F )
			{
				return "Mahjong Tiles";
			}
			if ( 0x1F030 <= utf32 && utf32 <= 0x1F09F )
			{
				return "Domino Tiles";
			}
			if ( 0x1F0A0 <= utf32 && utf32 <= 0x1F0FF )
			{
				return "Playing Cards";
			}
			if ( 0x1F100 <= utf32 && utf32 <= 0x1F1FF )
			{
				return "Enclosed Alphanumeric Supplement";
			}
			if ( 0x1F200 <= utf32 && utf32 <= 0x1F2FF )
			{
				return "Enclosed Ideographic Supplement";
			}
			if ( 0x1F300 <= utf32 && utf32 <= 0x1F5FF )
			{
				return "Miscellaneous Symbols And Pictographs";
			}
			if ( 0x1F600 <= utf32 && utf32 <= 0x1F64F )
			{
				return "Emoticons";
			}
			if ( 0x1F680 <= utf32 && utf32 <= 0x1F6FF )
			{
				return "Transport And Map Symbols";
			}
			if ( 0x1F700 <= utf32 && utf32 <= 0x1F77F )
			{
				return "Alchemical Symbols";
			}
			if ( 0x20000 <= utf32 && utf32 <= 0x2A6DF )
			{
				return "CJK Unified Ideographs Extension B";
			}
			if ( 0x2A700 <= utf32 && utf32 <= 0x2B73F )
			{
				return "CJK Unified Ideographs Extension C";
			}
			if ( 0x2B740 <= utf32 && utf32 <= 0x2B81F )
			{
				return "CJK Unified Ideographs Extension D";
			}
			if ( 0x2F800 <= utf32 && utf32 <= 0x2FA1F )
			{
				return "CJK Compatibility Ideographs Supplement";
			}
			if ( 0xE0000 <= utf32 && utf32 <= 0xE007F )
			{
				return "Tags";
			}
			if ( 0xE0100 <= utf32 && utf32 <= 0xE01EF )
			{
				return "Variation Selectors Supplement";
			}
			if ( 0xF0000 <= utf32 && utf32 <= 0xFFFFF )
			{
				return "Supplementary Private Use Area-A";
			}
			if ( 0x100000 <= utf32 && utf32 <= 0x10FFFF )
			{
				return "Supplementary Private Use Area-B";
			}
			// dummy
			return null;
		}
	}
}