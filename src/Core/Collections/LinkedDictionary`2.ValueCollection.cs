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
		///		Represents the collection of values in a <see cref="LinkedDictionary{TKey,TValue}"/>.
		/// </summary>
		[Serializable]
		[DebuggerDisplay( "Count={Count}" )]
		[DebuggerTypeProxy( typeof( CollectionDebuggerProxy<> ) )]
		public sealed partial class ValueCollection : ICollection<TValue>, ICollection
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

			bool ICollection<TValue>.IsReadOnly
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

			internal ValueCollection( LinkedDictionary<TKey, TValue> dictionary )
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
			public void CopyTo( TValue[] array )
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
			public void CopyTo( TValue[] array, int arrayIndex )
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
			public void CopyTo( int index, TValue[] array, int arrayIndex, int count )
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
			bool ICollection<TValue>.Contains( TValue item )
			{
				return this._dictionary.ContainsValue( item );
			}

			void ICollection<TValue>.Add( TValue item )
			{
				throw new NotSupportedException();
			}

			void ICollection<TValue>.Clear()
			{
				throw new NotSupportedException();
			}

			bool ICollection<TValue>.Remove( TValue item )
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

			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
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