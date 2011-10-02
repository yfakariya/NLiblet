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

namespace NLiblet.Collections
{
	partial class LinkedDictionary<TKey, TValue>
	{
		/// <summary>
		///		Represents the set of <see cref="LinkedDictionary{TKey,TValue}"/> keys.
		/// </summary>
		[Serializable]
		[DebuggerDisplay( "Count={Count}" )]
		[DebuggerTypeProxy( typeof( CollectionDebuggerProxy<> ) )]
		public sealed partial class KeySet : ISet<TKey>, ICollection<TKey>, ICollection
		{
			private readonly LinkedDictionary<TKey, TValue> _dictionary;

			/// <summary>
			///		Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
			/// </summary>
			/// <returns>
			///		The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
			///   </returns>
			public int Count
			{
				get { return this._dictionary.Count; }
			}

			bool ICollection<TKey>.IsReadOnly
			{
				get { return true; }
			}

			bool ICollection.IsSynchronized
			{
				get { return false; }
			}

			object ICollection.SyncRoot
			{
				get { return this; }
			}

			internal KeySet( LinkedDictionary<TKey, TValue> dictionary )
			{
				Contract.Requires( dictionary != null );

				this._dictionary = dictionary;
			}

			/// <summary>
			///		Copies the entire collection to a compatible one-dimensional array, starting at the beginning of the target array. 
			/// </summary>
			/// <param name="array">
			///		The one-dimensional <see cref="Array"/> that is the destination of the elements copied from this dictionary.
			///		The <see cref="Array"/> must have zero-based indexing.
			/// </param>
			public void CopyTo( TKey[] array )
			{
				Contract.Requires<ArgumentNullException>( array != null );

				CollectionOperation.CopyTo( this, this.Count, 0, array, 0, this.Count );
			}

			/// <summary>
			///		Copies the entire collection to a compatible one-dimensional array, 
			///		starting at the specified index of the target array. 
			/// </summary>
			/// <param name="array">
			///		The one-dimensional <see cref="Array"/> that is the destination of the elements copied from this dictionary.
			///		The <see cref="Array"/> must have zero-based indexing.
			/// </param>
			/// <param name="arrayIndex">
			///		The zero-based index in <paramref name="array"/> at which copying begins. 
			/// </param>
			public void CopyTo( TKey[] array, int arrayIndex )
			{
				CollectionOperation.CopyTo( this, this.Count, 0, array, arrayIndex, this.Count );
			}

			/// <summary>
			///		Copies a range of elements from this collection to a compatible one-dimensional array, 
			///		starting at the specified index of the target array. 
			/// </summary>
			/// <param name="index">
			///		The zero-based index in the source dictionary at which copying begins. 
			///	</param>
			/// <param name="array">
			///		The one-dimensional <see cref="Array"/> that is the destination of the elements copied from this dictionary.
			///		The <see cref="Array"/> must have zero-based indexing.
			/// </param>
			/// <param name="arrayIndex">
			///		The zero-based index in <paramref name="array"/> at which copying begins. 
			/// </param>
			/// <param name="count">
			///		The number of elements to copy.
			/// </param>
			public void CopyTo( int index, TKey[] array, int arrayIndex, int count )
			{
				Contract.Requires<ArgumentNullException>( array != null );
				Contract.Requires<ArgumentOutOfRangeException>( 0 <= index );
				Contract.Requires<ArgumentException>( this.Count == 0 || index < this.Count );
				Contract.Requires<ArgumentOutOfRangeException>( 0 <= arrayIndex );
				Contract.Requires<ArgumentOutOfRangeException>( 0 <= count );
				Contract.Requires<ArgumentException>( arrayIndex < array.Length - count );

				CollectionOperation.CopyTo( this, this.Count, index, array, arrayIndex, count );
			}

			void ICollection.CopyTo( Array array, int arrayIndex )
			{
				CollectionOperation.CopyTo( this, this.Count, array, arrayIndex );
			}

			/// <summary>
			///		Determines whether this collection contains a specific value.
			/// </summary>
			/// <param name="item">
			///		The object to locate in this collection.</param>
			/// <returns>
			///		<c>true</c> if <paramref name="item"/> is found in this collection; otherwise, <c>false</c>.
			/// </returns>
			bool ICollection<TKey>.Contains( TKey item )
			{
				return this._dictionary.ContainsKey( item );
			}

			void ICollection<TKey>.Add( TKey item )
			{
				throw new NotSupportedException();
			}

			void ICollection<TKey>.Clear()
			{
				throw new NotSupportedException();
			}

			bool ICollection<TKey>.Remove( TKey item )
			{
				throw new NotSupportedException();
			}

			bool ISet<TKey>.Add( TKey item )
			{
				throw new NotSupportedException();
			}

			void ISet<TKey>.ExceptWith( IEnumerable<TKey> other )
			{
				throw new NotSupportedException();
			}

			void ISet<TKey>.IntersectWith( IEnumerable<TKey> other )
			{
				throw new NotSupportedException();
			}

			/// <summary>
			///		Determines whether this set is proper subset of the specified collection.
			/// </summary>
			/// <param name="other">
			///		The collection to compare to the current set.
			///	</param>
			/// <returns>
			///   <c>true</c> if this set is proper subset of the specified collection; otherwise, <c>false</c>.
			/// </returns>
			/// <exception cref="ArgumentNullException">
			///		<paramref name="other"/> is Nothing.
			/// </exception>
			public bool IsProperSubsetOf( IEnumerable<TKey> other )
			{
				return SetOperation.IsProperSubsetOf( this, other );
			}

			/// <summary>
			///		Determines whether this set is proper superset of the specified collection.
			/// </summary>
			/// <param name="other">
			///		The collection to compare to the current set.
			///	</param>
			/// <returns>
			///   <c>true</c> if this set is proper superset of the specified collection; otherwise, <c>false</c>.
			/// </returns>
			/// <exception cref="ArgumentNullException">
			///		<paramref name="other"/> is Nothing.
			/// </exception>
			public bool IsProperSupersetOf( IEnumerable<TKey> other )
			{
				return SetOperation.IsProperSupersetOf( this, other );
			}

			/// <summary>
			///		Determines whether this set is subset of the specified collection.
			/// </summary>
			/// <param name="other">
			///		The collection to compare to the current set.
			///	</param>
			/// <returns>
			///   <c>true</c> if this set is subset of the specified collection; otherwise, <c>false</c>.
			/// </returns>
			/// <exception cref="ArgumentNullException">
			///		<paramref name="other"/> is Nothing.
			/// </exception>
			public bool IsSubsetOf( IEnumerable<TKey> other )
			{
				return SetOperation.IsSubsetOf( this, other );
			}

			/// <summary>
			///		Determines whether this set is superset of the specified collection.
			/// </summary>
			/// <param name="other">
			///		The collection to compare to the current set.
			///	</param>
			/// <returns>
			///   <c>true</c> if this set is superset of the specified collection; otherwise, <c>false</c>.
			/// </returns>
			/// <exception cref="ArgumentNullException">
			///		<paramref name="other"/> is Nothing.
			/// </exception>
			public bool IsSupersetOf( IEnumerable<TKey> other )
			{
				return SetOperation.IsSupersetOf( this, other );
			}

			/// <summary>
			///		Determines whether the current set and a specified collection share common elements. 
			/// </summary>
			/// <param name="other">
			///		The collection to compare to the current set.
			/// </param>
			/// <returns>
			///   <c>true</c> if this set and <paramref name="other"/> share at least one common element; otherwise, <c>false</c>.
			/// </returns>
			/// <exception cref="ArgumentNullException">
			///		<paramref name="other"/> is Nothing.
			/// </exception>
			public bool Overlaps( IEnumerable<TKey> other )
			{
				return SetOperation.Overlaps( this, other );
			}

			/// <summary>
			///		Determines whether this set and the specified collection contain the same elements. 
			/// </summary>
			/// <param name="other">
			///		The collection to compare to the current set.
			/// </param>
			/// <returns>
			///   <c>true</c> if this set is equal to  <paramref name="other"/>; otherwise, <c>false</c>.
			/// </returns>
			/// <exception cref="ArgumentNullException">
			///		<paramref name="other"/> is Nothing.
			/// </exception>
			public bool SetEquals( IEnumerable<TKey> other )
			{
				return SetOperation.SetEquals( this, other );
			}

			void ISet<TKey>.SymmetricExceptWith( IEnumerable<TKey> other )
			{
				throw new NotSupportedException();
			}

			void ISet<TKey>.UnionWith( IEnumerable<TKey> other )
			{
				throw new NotSupportedException();
			}

			/// <summary>
			///		Returns an enumerator that iterates through this collction.
			/// </summary>
			/// <returns>
			///		Returns an enumerator that iterates through this collction.
			/// </returns>
			public Enumerator GetEnumerator()
			{
				return new Enumerator( this._dictionary );
			}

			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}
	}
}