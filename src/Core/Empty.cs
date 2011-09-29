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
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;

namespace NLiblet
{
	/// <summary>
	///		Utlities for empty collections.
	/// </summary>
	static partial class Empty
	{
		/// <summary>
		///		Get singleton empty array for specified type.
		/// </summary>
		/// <typeparam name="T">Type of array.</typeparam>
		/// <returns>Singleton instance of empty array.</returns>
		/// <remarks>
		///		Empty array is immutable.
		/// </remarks>
		public static T[] Array<T>()
		{
			Contract.Ensures( Contract.Result<T[]>() != null );
			Contract.Ensures( Contract.Result<T[]>().Length == 0 );

			return Holder<T>.Array;
		}

		/// <summary>
		///		Get singleton empty <see cref="ReadOnlyCollection&lt;T&gt;"/> for specified type.
		/// </summary>
		/// <typeparam name="T">Item type of <see cref="ReadOnlyCollection&lt;T&gt;"/>.</typeparam>
		/// <returns>Singleton instance of empty <see cref="ReadOnlyCollection&lt;T&gt;"/>.</returns>
		public static ReadOnlyCollection<T> ReadOnlyCollection<T>()
		{
			Contract.Ensures( Contract.Result<ReadOnlyCollection<T>>() != null );
			Contract.Ensures( Contract.Result<ReadOnlyCollection<T>>().Count == 0 );

			return Holder<T>.ReadOnlyCollection;
		}

		private static class Holder<T>
		{
			public static readonly T[] Array = new T[ 0 ];
			public static readonly ReadOnlyCollection<T> ReadOnlyCollection = new ReadOnlyCollection<T>( Array );
		}
	}
}
