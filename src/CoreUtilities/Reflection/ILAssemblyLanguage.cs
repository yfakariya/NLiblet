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
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace NLiblet.Reflection
{
	/// <summary>
	///		Define utility methods related to the IL Assembly language.
	/// </summary>
	public static class ILAssemblyLanguage
	{
		private static readonly Regex _validIdPattern =
			new Regex( "^[A-Za-z_$@`?][0-9A-Za-z_$@`?]*$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.Singleline );

		/// <summary>
		///		Determines specified string is valid as ID of ECMA-CLI IL-Asm ID.
		/// </summary>
		/// <param name="value">Value to be determined.</param>
		/// <returns>If <paramref name="value"/> is valid ID then <c>true</c>; otherwise <c>false</c>.</returns>
		[Pure]
		public static bool IsValidId( string value )
		{
			return _validIdPattern.IsMatch( value );
		}
	}
}
