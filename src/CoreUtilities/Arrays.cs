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

namespace NLiblet
{
	/// <summary>
	///		Utilities for array.
	/// </summary>
	partial class Arrays
	{
		/// <summary>
		///		Get singleton empty array for specified type.
		/// </summary>
		/// <typeparam name="T">Type of array.</typeparam>
		/// <returns>Singleton instance of empty array.</returns>
		/// <remarks>
		///		Empty array is immutable.
		/// </remarks>
		public static T[] Empty<T>()
		{
			Contract.Ensures( Contract.Result<T[]>() != null );
			return TypedArrays<T>.Empty;
		}

		private static class TypedArrays<T>
		{
			public static readonly T[] Empty = new T[ 0 ];
		}
	}
}
