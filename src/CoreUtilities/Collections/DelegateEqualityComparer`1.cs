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
using System.Diagnostics.Contracts;

namespace NLiblet.Collections
{
	/// <summary>
	/// Implements <see cref="IEqualityComparer{T}"/> with delegate.
	/// </summary>
	/// <typeparam name="T">Type of target.</typeparam>
	public sealed class DelegateEqualityComparer<T> : IEqualityComparer<T>
	{
		private readonly Func<T, T, bool> _equalityComparison;
		private readonly Func<T, int> _hashCodeProvider;

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateEqualityComparer&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="equalityComparison">The equality comparison.</param>
		/// <param name="hashCodeProvider">The hash code provider.</param>
		public DelegateEqualityComparer( [Pure]Func<T, T, bool> equalityComparison, [Pure]Func<T, int> hashCodeProvider )
		{
			Contract.Requires<ArgumentNullException>( equalityComparison != null );
			Contract.Requires<ArgumentNullException>( hashCodeProvider != null );

			this._equalityComparison = equalityComparison;
			this._hashCodeProvider = hashCodeProvider;
		}

		/// <summary>
		///		Determines whether the specified objects are equal.
		/// </summary>
		/// <param name="x">The first object of type <typeparamref name="T"/> to compare.</param>
		/// <param name="y">The second object of type <typeparamref name="T"/> to compare.</param>
		/// <returns>
		///		true if the specified objects are equal; otherwise, false.
		/// </returns>
		[Pure]
		public bool Equals( T x, T y )
		{
			return this._equalityComparison( x, y );
		}

		/// <summary>
		///		Returns a hash code <typeparamref name="T"/> instance..
		/// </summary>
		/// <param name="obj">Target.</param>
		/// <returns>
		///		A hash code for <paramref name="obj"/> instance, suitable for use in hashing algorithms and data structures like a hash table. 
		/// </returns>
		[Pure]
		public int GetHashCode( T obj )
		{
			return this._hashCodeProvider( obj );
		}
	}
}
