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
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace NLiblet.Collections
{
	/// <summary>
	///		Provides the base class for a generic read-only set.
	/// </summary>
	/// <typeparam name="T">The type of items in the collection.</typeparam>
	[Serializable]
	[DebuggerDisplay( "Count = {Count}" )]
	[DebuggerTypeProxy( typeof( CollectionDebuggerProxy<> ) )]
	public class ReadOnlySet<T> : ISet<T>, ICollection
	{
		private readonly ISet<T> _items;

		/// <summary>
		///			Returns the <see cref="ISet{T}"/> that the <see cref="ReadOnlySet{T}"/> wraps.
		/// </summary>
		/// <value>The <see cref="ISet{T}"/> that the <see cref="ReadOnlySet{T}"/> wraps.</value>
		protected ISet<T> Items
		{
			get
			{
				Contract.Ensures( Contract.Result<ISet<T>>() != null );

				return this._items;
			}
		}

		/// <summary>
		///		Gets the number of elements contained in the <see cref="ReadOnlySet{T}"/>.
		/// </summary>
		/// <returns>
		///		The number of elements contained in the <see cref="ReadOnlySet{T}"/>.
		/// </returns>
		public int Count
		{
			get { return this.Items.Count; }
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
		///		Initializes a new instance of the <see cref="ReadOnlySet{T}"/> class.
		/// </summary>
		/// <param name="items">The <see cref="ISet{T}"/> that the <see cref="ReadOnlySet{T}"/> wraps.</param>
		public ReadOnlySet( ISet<T> items )
		{
			Contract.Requires<ArgumentNullException>( items != null );

			this._items = items;
		}

		/// <summary>
		///		Determines whether the current set is a property (strict) subset of a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>
		///		<c>true</c>if the current set is a correct subset of <paramref name="other"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool IsProperSubsetOf( IEnumerable<T> other )
		{
			return this.IsProperSubsetOf( other );
		}

		/// <summary>
		///		Determines whether the current set is a correct superset of a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set. </param>
		/// <returns>
		///		<c>true</c> if the current set is a correct superset of <paramref name="other"/>; otherwise, <c>false</c>.
		///	</returns>
		public bool IsProperSupersetOf( IEnumerable<T> other )
		{
			return this.IsProperSupersetOf( other );
		}

		/// <summary>
		///		Determines whether a set is a subset of a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>
		///		<c>true</c> if the current set is a subset of <paramref name="other"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool IsSubsetOf( IEnumerable<T> other )
		{
			return this.IsSubsetOf( other );
		}

		/// <summary>
		///		Determines whether a set is a superset of a specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>
		///		<c>true</c> if the current set is a superset of <paramref name="other"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool IsSupersetOf( IEnumerable<T> other )
		{
			return this.Items.IsSupersetOf( other );
		}

		/// <summary>
		///		Determines whether the current set overlaps with the specified collection.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>
		///		<c>true</c> if the current set and <paramref name="other"/> share at least one common element; otherwise, <c>false</c>.
		/// </returns>
		public bool Overlaps( IEnumerable<T> other )
		{
			return this.Items.Overlaps( other );
		}

		/// <summary>
		///		Determines whether the current set and the specified collection contain the same elements.
		/// </summary>
		/// <param name="other">The collection to compare to the current set.</param>
		/// <returns>
		///		<c>true</c> if the current set is equal to <paramref name="other"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool SetEquals( IEnumerable<T> other )
		{
			return this.Items.SetEquals( other );
		}

		/// <summary>
		///		Determines whether the <see cref="ReadOnlySet{T}"/> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="ReadOnlySet{T}"/>.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="item"/> is found in the <see cref="ReadOnlySet{T}"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains( T item )
		{
			return this.Items.Contains( item );
		}

		/// <summary>
		///		Copies the elements of the <see cref="ReadOnlySet{T}"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
		/// </summary>
		/// <param name="array">
		///		The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="ReadOnlySet{T}"/>. 
		///		The <see cref="Array"/> must have zero-based indexing.
		///	</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		public void CopyTo( T[] array, int arrayIndex )
		{
			this.Items.CopyTo( array, arrayIndex );
		}

		/// <summary>
		///		Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		///		A <see cref="IEnumerator{T}"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return this.Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		bool ISet<T>.Add( T item )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		void ISet<T>.ExceptWith( IEnumerable<T> other )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		void ISet<T>.IntersectWith( IEnumerable<T> other )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		void ISet<T>.SymmetricExceptWith( IEnumerable<T> other )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		void ISet<T>.UnionWith( IEnumerable<T> other )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		void ICollection<T>.Add( T item )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		void ICollection<T>.Clear()
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		bool ICollection<T>.Remove( T item )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Just ICollection compatibility, there are very few use-cases to use this method." )]
		void ICollection.CopyTo( Array array, int index )
		{
			this._items.CopyTo( ( T[] )array, index );
		}
	}
}
