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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace NLiblet.Collections
{
	/// <summary>
	///		Provides the base class for a read-only collection whose keys are embedded in the values.
	/// </summary>
	/// <typeparam name="TKey">The type of keys in the collection.</typeparam>
	/// <typeparam name="TItem">The type of items in the collection.</typeparam>
	[Serializable]
	[DebuggerDisplay( "Count = {Count}" )]
	[DebuggerTypeProxy( typeof( DictionaryDebuggerProxy<,> ) )]
	public class ReadOnlyKeyedCollection<TKey, TItem> : ICollection<TItem>
	{
		private const int _neverCreateDictionary = -1;
		private const int _defaultDictionaryCreationThreshold = 0;

		private readonly KeyedCollection<TKey, TItem> _underlying;

		/// <summary>
		///		Gets the number of elements contained in the <see cref="ReadOnlyKeyedCollection{TKey,TItem}"/>.
		/// </summary>
		/// <returns>
		///		The number of elements contained in the <see cref="ReadOnlyKeyedCollection{TKey,TItem}"/>.
		/// </returns>
		public int Count
		{
			get { return this._underlying.Count; }
		}

		bool ICollection<TItem>.IsReadOnly
		{
			get { return true; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="ReadOnlyKeyedCollection{TKey,TItem}"/> class which wraps specified <see cref="KeyedCollection{TKey,TItem}"/>.
		/// </summary>
		/// <param name="underlying"><see cref="KeyedCollection{TKey,TItem}"/> to be wrapped.</param>
		public ReadOnlyKeyedCollection( KeyedCollection<TKey, TItem> underlying )
		{
			Contract.Requires<ArgumentNullException>( underlying != null );

			this._underlying = underlying;
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="ReadOnlyKeyedCollection{TKey,TItem}"/> class which wraps specified <see cref="IEnumerable{TItem}"/> 
		///		and uses the default equality comparer.
		/// </summary>
		/// <param name="collection"><see cref="IEnumerable{TItem}"/> to be wrapped.</param>
		/// <param name="keyExtracter">Delegate to procedure which extract <typeparamref name="TKey"/> type keys from <typeparamref name="TItem"/> type items of <paramref name="collection"/>.</param>
		public ReadOnlyKeyedCollection( IEnumerable<TItem> collection, Func<TItem, TKey> keyExtracter )
			: this( collection, keyExtracter, null )
		{
			Contract.Requires<ArgumentNullException>( collection != null );
			Contract.Requires<ArgumentNullException>( keyExtracter != null );
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="ReadOnlyKeyedCollection{TKey,TItem}"/> class which wraps specified <see cref="IEnumerable{TItem}"/> 
		///		and uses the specified equality comparer.
		/// </summary>
		/// <param name="collection"><see cref="IEnumerable{TItem}"/> to be wrapped.</param>
		/// <param name="keyExtracter">Delegate to procedure which extract <typeparamref name="TKey"/> type keys from <typeparamref name="TItem"/> type items of <paramref name="collection"/>.</param>
		/// <param name="comparer">The implementation of the <see cref="IEqualityComparer{TKey}"/> generic interface to use when comparing keys, or <c>null</c> to use the default equality comparer for the type of the key, obtained from <see cref="EqualityComparer{T}.Default"/>.</param>
		public ReadOnlyKeyedCollection( IEnumerable<TItem> collection, Func<TItem, TKey> keyExtracter, IEqualityComparer<TKey> comparer )
			: this(
				collection,
				keyExtracter,
				null,
				typeof( TItem ).IsValueType ? _neverCreateDictionary : _defaultDictionaryCreationThreshold  // see http://msdn.microsoft.com/en-us/library/ms132438.aspx
			)
		{
			Contract.Requires<ArgumentNullException>( collection != null );
			Contract.Requires<ArgumentNullException>( keyExtracter != null );
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="ReadOnlyKeyedCollection{TKey,TItem}"/> class which wraps specified <see cref="IEnumerable{TItem}"/> 
		///		and uses specified equality comparer and and creates a lookup dictionary when the specified threshold is exceeded.
		/// </summary>
		/// <param name="collection"><see cref="IEnumerable{TItem}"/> to be wrapped.</param>
		/// <param name="keyExtracter">Delegate to procedure which extract <typeparamref name="TKey"/> type keys from <typeparamref name="TItem"/> type items of <paramref name="collection"/>.</param>
		/// <param name="comparer">The implementation of the <see cref="IEqualityComparer{TKey}"/> generic interface to use when comparing keys, or <c>null</c> to use the default equality comparer for the type of the key, obtained from <see cref="EqualityComparer{T}.Default"/>.</param>
		/// <param name="dictionaryCreationThreshold">The number of elements the collection can hold without creating a lookup dictionary (0 creates the lookup dictionary when the first item is added), or –1 to specify that a lookup dictionary is never created.</param>
		public ReadOnlyKeyedCollection( IEnumerable<TItem> collection, Func<TItem, TKey> keyExtracter, IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold )
		{
			Contract.Requires<ArgumentNullException>( collection != null );
			Contract.Requires<ArgumentNullException>( keyExtracter != null );

			this._underlying = new DelegatedKeyedCollection( keyExtracter, comparer, dictionaryCreationThreshold );
			foreach ( var item in collection )
			{
				this._underlying.Add( item );
			}
		}

		/// <summary>
		///		Determines whether the <see cref="ReadOnlyKeyedCollection{TKey,TItem}"/> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="ReadOnlyKeyedCollection{TKey,TItem}"/>.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="item"/> is found in the <see cref="ReadOnlyKeyedCollection{TKey,TItem}"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains( TItem item )
		{
			return this._underlying.Contains( item );
		}

		/// <summary>
		///		Copies the elements of the <see cref="ReadOnlyKeyedCollection{TKey,TItem}"/> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
		/// </summary>
		/// <param name="array">
		///		The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="ReadOnlySet{T}"/>. 
		///		The <see cref="Array"/> must have zero-based indexing.
		///	</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		public void CopyTo( TItem[] array, int arrayIndex )
		{
			this._underlying.CopyTo( array, arrayIndex );
		}

		/// <summary>
		///		Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		///		A <see cref="IEnumerator{TItem}"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<TItem> GetEnumerator()
		{
			return this._underlying.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		void ICollection<TItem>.Add( TItem item )
		{
			throw new NotSupportedException();
		}

		void ICollection<TItem>.Clear()
		{
			throw new NotSupportedException();
		}

		bool ICollection<TItem>.Remove( TItem item )
		{
			throw new NotSupportedException();
		}

		private sealed class DelegatedKeyedCollection : KeyedCollection<TKey, TItem>
		{
			private readonly Func<TItem, TKey> _keyExtracter;

			public DelegatedKeyedCollection( Func<TItem, TKey> keyExtracter, IEqualityComparer<TKey> comparer, int dictionaryCreationThreashold )
				: base( comparer, dictionaryCreationThreashold )
			{
				Contract.Requires( keyExtracter != null );

				this._keyExtracter = keyExtracter;
			}

			protected sealed override TKey GetKeyForItem( TItem item )
			{
				return this._keyExtracter( item );
			}
		}

	}
}
