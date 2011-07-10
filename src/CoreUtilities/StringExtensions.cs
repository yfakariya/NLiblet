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

		/// <summary>
		///		Get substring of this <see cref="String"/>.
		///		When length of <see cref="String"/> is lessor than required length, this method returns shorter string.
		/// </summary>
		/// <param name="source"><see cref="String"/>.</param>
		/// <param name="startIndex">
		///		Index of starting substring.
		/// </param>
		/// <param name="length">
		///		Requested length of substring.
		/// </param>
		/// <returns>
		///		When sum of <paramref name="startIndex"/> and <paramref name="length"/> are lessor or equal to length of <paramref name="source"/>, then substring of it;
		///		else, returns string which starts with requested substring, and its length will be shorter than requested <paramref name="length"/>.
		/// </returns>
		public static string SubstringLoosely( this string source, int startIndex, int length )
		{
			Contract.Requires<ArgumentNullException>( source != null );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= startIndex );
			Contract.Requires<ArgumentOutOfRangeException>( startIndex < source.Length );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= length );

			return SubstringLoosely( source, startIndex, length, null );
		}

		/// <summary>
		///		Get substring of this <see cref="String"/>.
		///		When length of <see cref="String"/> is lessor than required length, this method returns whether alternative string which is padded by padding charactor or shorter string.
		/// </summary>
		/// <param name="source"><see cref="String"/>.</param>
		/// <param name="startIndex">
		///		Index of starting substring.
		/// </param>
		/// <param name="length">
		///		Requested length of substring.
		/// </param>
		/// <param name="padding">
		///		Padding character. If this value is null, return string may be shorter than <paramref name="length"/>.
		/// </param>
		/// <returns>
		///		When sum of <paramref name="startIndex"/> and <paramref name="length"/> are lessor or equal to length of <paramref name="source"/>, then substring of it;
		///		else, if <paramref name="padding"/> is speciffied, returns string which starts with requested substring and its tail is padded with <paramref name="padding"/>;
		///		else, returns string which starts with requested substring, and its length will be shorter than requested <paramref name="length"/>.
		/// </returns>
		public static string SubstringLoosely( this string source, int startIndex, int length, char? padding )
		{
			Contract.Requires<ArgumentNullException>( source != null );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= startIndex );
			Contract.Requires<ArgumentOutOfRangeException>( startIndex < source.Length );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= length );

			int actualLength = source.Length - startIndex;
			if ( length <= actualLength )
			{
				return source.Substring( startIndex, length );
			}
			else
			{
				int paddingCount = length - actualLength;

				if ( padding == null )
				{
					return source.Substring( startIndex, actualLength );
				}
				else
				{
					return source.Substring( startIndex, actualLength ) + new String( padding.Value, paddingCount );
				}
			}
		}
	}
}
