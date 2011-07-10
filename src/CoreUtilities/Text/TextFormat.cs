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

namespace NLiblet.Text
{
//FIXME: Devide into HexFormat, Base64Format, EncodingFormat
#warning IMPL
	public static class TextFormat
	{
		public static IEnumerable<char> EscapeCSharp( IEnumerable<char> chars )
		{
			if ( chars == null )
			{
				return null;
			}
			else
			{
				return CharEscapingFilter.UpperCaseDefaultCSharpLiteralStyle.Escape( chars );
			}
		}

		public static string EscapeToCSharpString( IEnumerable<char> chars )
		{
			return String.Join( String.Empty, EscapeCSharp( chars ) );
		}

		private static readonly CharEscapingFilter _powerShellLiteral =
			DefaultCharEscapingFilter.CreatePowerShell( allowNonAscii: true, allowLineBreak: false, allowQuotation: false, isUpper: true );

		public static IEnumerable<char> EscapePowerShell( IEnumerable<char> chars )
		{
			if ( chars == null )
			{
				return null;
			}
			else
			{
				return _powerShellLiteral.Escape( chars );
			}
		}

		public static string EscapeToPowerShellString( IEnumerable<char> chars )
		{
			return String.Join( String.Empty, EscapePowerShell( chars ) );
		}

		public static IEnumerable<char> ToBase64( IEnumerable<byte> bytes )
		{
			throw new NotImplementedException();
		}

		public static IEnumerable<byte> FromBase64( IEnumerable<char> base64Chars )
		{
			throw new NotImplementedException();
		}
	}
}
