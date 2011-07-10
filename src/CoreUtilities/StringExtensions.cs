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
using System.Diagnostics.Contracts;

namespace NLiblet
{
	/// <summary>
	///		Extends <see cref="String"/> with extension methods.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		///		Get slice of string.
		/// </summary>
		/// <param name="source"><see cref="String"/>.</param>
		/// <param name="start">Start index, inclusive.</param>
		/// <param name="end">End index, inclusive.</param>
		/// <returns>Slice of <paramref name="source"/>.</returns>
		public static string Slice( this string source, int start, int end )
		{
			Contract.Requires<ArgumentNullException>( source != null );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= start );
			Contract.Requires<ArgumentOutOfRangeException>( start < source.Length );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= end );
			Contract.Requires<ArgumentOutOfRangeException>( end < source.Length );
			Contract.Requires<InvalidOperationException>( start <= end );

			return new StringBuilder( source, start, end - start + 1, end - start + 1 ).ToString();
		}
	}
}
