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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace NLiblet.Collections
{
	/// <summary>
	///		Provides the base class for a generic read-only collection but it does not have index.
	/// </summary>
	/// <typeparam name="T">The type of elements in the collection.</typeparam>
	[Serializable]
	[DebuggerDisplay( "Count = {Count}" )]
	[DebuggerTypeProxy( typeof( CollectionDebuggerProxy<> ) )]
	public class ReadOnlyNonListCollection<T> : ICollection<T>, ICollection
	{
		private readonly ICollection<T> _items;

		/// <summary>
		///		Gets modifiable underlying collection of this instance.
		/// </summary>
		/// <value>
		///		Modifiable underlying collection of this instance.
		/// </value>
		protected ICollection<T> Items
		{
			get { return this._items; }
		}

		/// <summary>
		///		Gets the number of elements contained in the <see cref="ReadOnlyNonListCollection{T}"/>.
		/// </summary>
		/// <returns>
		///		The number of elements contained in the <see cref="ReadOnlyNonListCollection{T}"/>.
		///   </returns>
		public int Count
		{
			get { return this._items.Count; }
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Easy to re-declare." )]
		bool ICollection<T>.IsReadOnly
		{
			get { return true; }
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Easy to re-declare." )]
		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Easy to re-declare." )]
		object ICollection.SyncRoot
		{
			get { return this; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="ReadOnlyNonListCollection{T}"/> class.
		/// </summary>
		/// <param name="collection">Collection to be wrapped.</param>
		public ReadOnlyNonListCollection( ICollection<T> collection )
		{
			Contract.Requires<ArgumentNullException>( collection != null );

			this._items = collection;
		}

		/// <summary>
		///		Determines whether the <see cref="ReadOnlyNonListCollection{T}"/> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="ReadOnlyNonListCollection{T}"/>.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="item"/> is found in the <see cref="ReadOnlyNonListCollection{T}"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains( T item )
		{
			return this._items.Contains( item );
		}

		/// <summary>
		///		Copies items of this instance to specified array.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Start index of the array to be copied.</param>
		public void CopyTo( T[] array, int arrayIndex )
		{
			this._items.CopyTo( array, arrayIndex );
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return this._items.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Just ICollection compatibility, there are very few use-cases to use this method." )]
		void ICollection<T>.Add( T item )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Just ICollection compatibility, there are very few use-cases to use this method." )]
		void ICollection<T>.Clear()
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Just ICollection compatibility, there are very few use-cases to use this method." )]
		bool ICollection<T>.Remove( T item )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Just ICollection compatibility, there are very few use-cases to use this method." )]
		void ICollection.CopyTo( Array array, int index )
		{
			Array.Copy( this._items.ToArray(), 0, array, index, this._items.Count );
		}
	}
}
