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
using System.Globalization;
using System.Linq;

namespace NLiblet.Collections
{
	/// <summary>
	///		<see cref="IDictionary{TKey,TValue}"/> implementation which preserves inserting order.
	/// </summary>
	/// <typeparam name="TKey">Type of the key of the dictionary element.</typeparam>
	/// <typeparam name="TValue">Type of the value of the dictionary element.</typeparam>
	/// <remarks>
	///		This class is similer to Java Collection Framework's LinkedHashMap.
	///		<note>
	///			For LINQ Last(), LastOrDefault(), and Reverse() operators, it is more efficient to use this class directly than via interface.
	///		</note>
	/// </remarks>
	[Serializable]
	[DebuggerDisplay( "Count={Count}" )]
	[DebuggerTypeProxy( typeof( DictionaryDebuggerProxy<,> ) )]
	public partial class LinkedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary
	// NET_4_5: , IReadOnlyDictionary<TKey,TValue>
	{
		// We use standard Dictionary<TKey,TValue> here, because it should be optimized harder (see Mono's implementation for example.)
		private readonly Dictionary<TKey, LinkedDictionaryNode<TKey, TValue>> _dictionary;
		private int _version;

		// Bridge to bulk clear.
		private Bridge _currentBridge;

		#region -- Properties --

		#region ---- Head and Tail ----

		private LinkedDictionaryNode<TKey, TValue> _head;

		/// <summary>
		///		Get the head node of this dictionary.
		/// </summary>
		/// <value>
		///		The head node of this dictionary. If this dictionary is empty then <c>null</c>.
		/// </value>
		/// <remarks>
		///		Note that the node holds reference to other nodes and underlying collection, so it could cause unpected resource leak.
		/// </remarks>
		[Pure]
		public LinkedDictionaryNode<TKey, TValue> Head
		{
			get
			{
				Contract.Ensures(
					this.Count == 0 && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() == null
					|| 0 < this.Count && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() != null
				);

				return this._head;
			}
		}

		private LinkedDictionaryNode<TKey, TValue> _tail;

		/// <summary>
		///		Get the tail node of this dictionary.
		/// </summary>
		/// <value>
		///		The tail node of this dictionary. If this dictionary is empty then <c>null</c>.
		/// </value>
		/// <remarks>
		///		Note that the node holds reference to other nodes and underlying collection, so it could cause unpected resource leak.
		/// </remarks>
		[Pure]
		public LinkedDictionaryNode<TKey, TValue> Tail
		{
			get
			{
				Contract.Ensures(
					this.Count == 0 && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() == null
					|| 0 < this.Count && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() != null
				);

				return this._tail;
			}
		}

		#endregion ---- Head and Tail ----

		#region ---- Count and Comparer ----

		/// <summary>
		///		Gets the <see cref="IEqualityComparer{TKey}"/> that is used to determine equality of keys for this dictionary. 
		/// </summary>
		/// <value>
		///		The <see cref="IEqualityComparer{TKey}"/> generic interface implementation 
		///		that is used to determine equality of keys for the current dictionary and to provide hash values for the keys. 
		/// </value>
		[Pure]
		public IEqualityComparer<TKey> Comparer
		{
			get
			{
				Contract.Ensures( Contract.Result<IEqualityComparer<TKey>>() != null );

				return this._dictionary.Comparer;
			}
		}

		/// <summary>
		///		Gets the number of elements contained in this dictionary.
		/// </summary>
		/// <returns>
		///		The number of elements contained in this dictionary.
		///	</returns>
		public int Count
		{
			get
			{
				Contract.Ensures( 0 <= Contract.Result<int>() );

				return this._dictionary.Count;
			}
		}

		#endregion ---- Count and Comparer ----

		#region ---- Keys and Values ----

		/// <summary>
		///		Gets an <see cref="KeySet"/> containing the keys of this dictionary.
		/// </summary>
		/// <returns>
		///		An <see cref="KeySet"/> containing the keys of this dictionary.
		/// </returns>
		[Pure]
		public KeySet Keys
		{
			get
			{
				Contract.Ensures( Contract.Result<KeySet>() != null );
				Contract.Ensures( Contract.Result<KeySet>().Count == this.Count );

				return new KeySet( this );
			}
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get { return this.Keys; }
		}

		ICollection IDictionary.Keys
		{
			get { return this.Keys; }
		}

		/// <summary>
		///		Gets an <see cref="ValueCollection"/> containing the values of this dictionary.
		/// </summary>
		/// <returns>
		///		An <see cref="ValueCollection"/> containing the values of this dictionary.
		/// </returns>
		[Pure]
		public ValueCollection Values
		{
			get
			{
				Contract.Ensures( Contract.Result<ValueCollection>() != null );
				Contract.Ensures( Contract.Result<ValueCollection>().Count == this.Count );

				return new ValueCollection( this );
			}
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get { return this.Values; }
		}

		ICollection IDictionary.Values
		{
			get { return this.Values; }
		}

		#endregion ---- Keys and Values ----

		#region ---- Indexers ----

		/// <summary>
		///		Gets or sets the value with the specified key.
		/// </summary>
		/// <param name="key">
		///		The key of the value to get or set.
		/// </param>
		/// <returns>
		///		The value with the specified key.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///		<paramref name="key"/> is <c>null</c>.
		/// </exception>
		/// <exception cref="T:System.Collections.Generic.KeyNotFoundException">
		///		The property is retrieved and <paramref name="key"/> is not found in this dictionary.
		/// </exception>
		/// <remarks>
		///		To retrieve <see cref="LinkedDictionaryNode{TKey,TValue}"/> to operated linked list like operation,
		///		use <see cref="GetNode(TKey)"/>.
		/// </remarks>
		public TValue this[ TKey key ]
		{
			get
			{
				var result = this.GetItem( key );
				if ( result == null )
				{
					throw new KeyNotFoundException( "Specified key does not exist in the collection." );
				}

				return result.Value;
			}
			set
			{
				TValue dummy1;
				bool dummy2;
				this.SetItem( key, value, out dummy1, out dummy2 );
			}
		}

		object IDictionary.this[ object key ]
		{
			get
			{
				TKey typedKey;
				if ( !TryEnsureKeyType( key, out typedKey ) )
				{
					return null;
				}

				TValue value;
				if ( this.TryGetValue( typedKey, out value ) )
				{
					return value;
				}
				else
				{
					return null;
				}
			}
			set
			{
				this[ EnsureKeyType( key ) ] = EnsureValueType( value );
			}
		}

		#endregion ---- Indexers ----

		#region ---- Interface Explicit Implementations ----

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get { return false; }
		}

		bool IDictionary.IsFixedSize
		{
			get { return false; }
		}

		bool IDictionary.IsReadOnly
		{
			get { return false; }
		}

		bool ICollection.IsSynchronized
		{
			get { return false; }
		}

		object ICollection.SyncRoot
		{
			get { return this; }
		}

		#endregion ---- Interface Explicit Implementations ----

		#endregion -- Properties --

		#region -- Constructors --

		/// <summary>
		///		Initializes a new instance of the <see cref="LinkedDictionary&lt;TKey, TValue&gt;"/> class.
		///		with default capacity and default key comparer.
		/// </summary>
		public LinkedDictionary()
		{
			this._dictionary = new Dictionary<TKey, LinkedDictionaryNode<TKey, TValue>>();
			this._currentBridge = new Bridge( this );
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="LinkedDictionary&lt;TKey, TValue&gt;"/> class.
		///		with default capacity specified key comparer.
		/// </summary>
		/// <param name="comparer">
		///		The <see cref="IEqualityComparer{TKey}"/> generic interface implementation 
		///		that is used to determine equality of keys for the current dictionary and to provide hash values for the keys. 
		///		If <c>null</c> is specified, then <see cref="EqualityComparer{T}.Default"/> of <typeparamref name="TKey"/> will be used.
		/// </param>
		public LinkedDictionary( IEqualityComparer<TKey> comparer )
		{
			this._dictionary = new Dictionary<TKey, LinkedDictionaryNode<TKey, TValue>>( comparer );
			this._currentBridge = new Bridge( this );
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="LinkedDictionary&lt;TKey, TValue&gt;"/> class.
		///		with specified initial capacity and default key comparer.
		/// </summary>
		/// <param name="initialCapacity">The initial dictionary to be copied.</param>
		public LinkedDictionary( int initialCapacity )
		{
			this._dictionary = new Dictionary<TKey, LinkedDictionaryNode<TKey, TValue>>( initialCapacity );
			this._currentBridge = new Bridge( this );
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="LinkedDictionary&lt;TKey, TValue&gt;"/> class.
		///		with specified initial capacity and key comparer.
		/// </summary>
		/// <param name="initialCapacity">The initial dictionary to be copied.</param>
		/// <param name="comparer">
		///		The <see cref="IEqualityComparer{TKey}"/> generic interface implementation 
		///		that is used to determine equality of keys for the current dictionary and to provide hash values for the keys. 
		///		If <c>null</c> is specified, then <see cref="EqualityComparer{T}.Default"/> of <typeparamref name="TKey"/> will be used.
		/// </param>
		public LinkedDictionary( int initialCapacity, IEqualityComparer<TKey> comparer )
		{
			this._dictionary = new Dictionary<TKey, LinkedDictionaryNode<TKey, TValue>>( initialCapacity, comparer );
			this._currentBridge = new Bridge( this );
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="LinkedDictionary&lt;TKey, TValue&gt;"/> class
		///		with specified initial items and key comparer.
		/// </summary>
		/// <param name="dictionary">The initial dictionary to be copied.</param>
		/// <param name="comparer">
		///		The <see cref="IEqualityComparer{TKey}"/> generic interface implementation 
		///		that is used to determine equality of keys for the current dictionary and to provide hash values for the keys. 
		///		If <c>null</c> is specified, then <see cref="EqualityComparer{T}.Default"/> of <typeparamref name="TKey"/> will be used.
		/// </param>
		public LinkedDictionary( IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer )
		{
			Contract.Requires<ArgumentNullException>( dictionary != null );

			this._dictionary = new Dictionary<TKey, LinkedDictionaryNode<TKey, TValue>>( dictionary.Count, comparer );
			this._currentBridge = new Bridge( this );

			foreach ( var item in dictionary )
			{
				this.AddItemNonVirtual( item.Key, item.Value );
			}
		}

		#endregion -- Constructors --

		#region -- Methods --

		#region ---- Template Methods ----

		/// <summary>
		///		Get <see cref="LinkedDictionaryNode{TKey,TValue}"/> with specified key.
		/// </summary>
		/// <param name="key">
		///		The key of the value to get.
		/// </param>
		/// <returns>
		///		The <see cref="LinkedDictionaryNode{TKey,TValue}"/> with the specified key.
		/// </returns>
		/// <remarks>
		///		<para>
		///			Derived class can override this method to intercept manipulation, for example, move accessed node to tail of this dictionary.
		///		</para>
		///		<note>
		///			<strong>Note for implementers:</strong> You must call base implementation from overrides, but you can invoke in any timing as you want to do.
		///		</note>
		///		<para>
		///			Note that the node holds reference to other nodes and underlying collection, so it could cause unpected resource leak.
		///		</para>
		///		<para>
		///			This method calls <see cref="GetNodeDirect(TKey)"/>.
		///		</para>
		///		<note>
		///			This method is not pure because derived class may implement LRU algorithm.
		///		</note>
		/// </remarks>
		protected virtual LinkedDictionaryNode<TKey, TValue> GetItem( TKey key )
		{
			Contract.Requires<ArgumentNullException>( key != null );
			Contract.Ensures(
				this.ContainsKey( key ) && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() != null
				|| !this.ContainsKey( key ) && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() == null
			);

			return this.GetNodeDirect( key );
		}

		/// <summary>
		///		Get <see cref="LinkedDictionaryNode{TKey,TValue}"/> with specified key.
		/// </summary>
		/// <param name="key">
		///		The key of the value to get.
		/// </param>
		/// <returns>
		///		The <see cref="LinkedDictionaryNode{TKey,TValue}"/> with the specified key.
		/// </returns>
		/// <remarks>
		///		This method will not be affected from template method overriding.
		/// </remarks>
		[Pure]
		protected LinkedDictionaryNode<TKey, TValue> GetNodeDirect( TKey key )
		{
			LinkedDictionaryNode<TKey, TValue> node;
			if ( this._dictionary.TryGetValue( key, out node ) )
			{
				return node;
			}

			return null;
		}

		/// <summary>
		///		Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">
		///		The key of the element to add.
		/// </param>
		/// <param name="value">
		///		The value of the element to add. The value can be <c>null</c> for reference types.
		/// </param>
		/// <returns>
		///		An element with the same key already does not exist in the dictionary and sucess to add then newly added node;
		///		otherwise <c>null</c>.
		/// </returns>
		/// <remarks>
		///		<para>
		///			Derived class can override this method to intercept manipulation.
		///		</para>
		///		<note>
		///			<strong>Note for implementers:</strong> You must call base implementation from overrides, but you can invoke in any timing as you want to do.
		///		</note>
		///		<para>
		///			Note that the node holds reference to other nodes and underlying collection, so it could cause unpected resource leak.
		///		</para>
		/// </remarks>
		protected virtual LinkedDictionaryNode<TKey, TValue> AddItem( TKey key, TValue value )
		{
			Contract.Requires<ArgumentNullException>( key != null );
			Contract.Ensures(
				( Contract.OldValue( this.ContainsKey( key ) ) && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() == null )
				|| ( !Contract.OldValue( this.ContainsKey( key ) ) && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() != null )
			);
			Contract.Ensures( this.ContainsKey( key ) );

			return this.AddItemNonVirtual( key, value );
		}

		private LinkedDictionaryNode<TKey, TValue> AddItemNonVirtual( TKey key, TValue value )
		{
			if ( this._dictionary.ContainsKey( key ) )
			{
				return null;
			}

			return this.AddItemCore( key, value );
		}

		private LinkedDictionaryNode<TKey, TValue> AddItemCore( TKey key, TValue value )
		{
			var tail = this._tail;
			var node = new LinkedDictionaryNode<TKey, TValue>( key, value, this._currentBridge, tail, null );

			if ( tail != null )
			{
				tail.InternalNext = node;
			}

			if ( this._head == null )
			{
				this._head = node;
			}

			this._tail = node;
			this._dictionary.Add( key, node );
			this._version++;
			return node;
		}

		/// <summary>
		///		Sets the value with the specified key.
		/// </summary>
		/// <param name="key">
		///		The key of the value to set.
		/// </param>
		/// <param name="value">
		///		The value to be set.
		/// </param>
		/// <param name="oldValue">
		///		When this method is returned, the old value will be stored.
		///		This parameter is passed uninitialized.
		/// </param>
		/// <param name="addedNew">
		///		If node is newly added, then <c>true</c> is stored; otherwise, <c>false</c> is stored.
		///		This parameter is passed uninitialized.
		/// </param>
		/// <returns>
		///		Newly added or updated <see cref="LinkedDictionaryNode{TKey,TValue}"/>.
		/// </returns>
		/// <remarks>
		///		<para>
		///			Derived class can override this method to intercept manipulation, for example, move updated node to tail of this dictionary.
		///		</para>
		///		<note>
		///			<strong>Note for implementers:</strong> You must call base implementation from overrides, but you can invoke in any timing as you want to do.
		///		</note>
		///		<para>
		///			Note that the node holds reference to other nodes and underlying collection, so it could cause unpected resource leak.
		///		</para>
		/// </remarks>
		protected virtual LinkedDictionaryNode<TKey, TValue> SetItem( TKey key, TValue value, out TValue oldValue, out bool addedNew )
		{
			Contract.Requires<ArgumentNullException>( key != null );
			Contract.Ensures( Contract.Result<LinkedDictionaryNode<TKey, TValue>>() != null );
			Contract.Ensures(
				( Contract.OldValue( this.ContainsKey( key ) ) && EqualityComparer<TValue>.Default.Equals( Contract.ValueAtReturn( out oldValue ), Contract.OldValue( this[ key ] ) ) && Contract.ValueAtReturn( out addedNew ) == false )
				|| ( !Contract.OldValue( this.ContainsKey( key ) ) && Contract.ValueAtReturn( out addedNew ) == true )
			);
			Contract.Ensures( this.ContainsKey( key ) );

			var oldNode = this.GetNodeDirect( key );
			if ( oldNode == null )
			{
				var newNode = this.AddItemCore( key, value );
				addedNew = true;
				oldValue = default( TValue );
				this._version++;
				return newNode;
			}
			else
			{
				oldValue = oldNode.Value;
				addedNew = false;
				oldNode.InternalValue = value;
				this._version++;
				return oldNode;
			}
		}

		/// <summary>
		///		Removes the value with the specified key from this dictionary.
		/// </summary>
		/// <param name="key">
		///		The key of the element to remove.
		///	</param>
		/// <returns>
		///		Removed node if the element is successfully found and removed;
		///		otherwise, <c>null</c>.
		/// </returns>
		/// <remarks>
		///		<para>
		///			Derived class can override this method to intercept manipulation.
		///		</para>
		///		<note>
		///			<strong>Note for implementers:</strong> You must call base implementation from overrides, but you can invoke in any timing as you want to do.
		///		</note>
		///		<para>
		///			Note that the node holds reference to other nodes and underlying collection, so it could cause unpected resource leak.
		///		</para>
		/// </remarks>
		protected virtual LinkedDictionaryNode<TKey, TValue> RemoveItem( TKey key )
		{
			Contract.Requires<ArgumentNullException>( key != null );
			Contract.Ensures(
				( Contract.OldValue( this.ContainsKey( key ) ) && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() != null )
				|| ( !Contract.OldValue( this.ContainsKey( key ) ) && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() == null )
			);
			Contract.Ensures( !this.ContainsKey( key ) );


			var node = this.GetNodeDirect( key );
			if ( node == null )
			{
				return null;
			}

			this._dictionary.Remove( key );

			if ( node.InternalPrevious == null )
			{
				this._head = node.InternalNext;
			}
			else
			{
				node.InternalPrevious.InternalNext = node.InternalNext;
			}

			if ( node.InternalNext == null )
			{
				this._tail = node.InternalPrevious;
			}
			else
			{
				node.InternalNext.InternalPrevious = node.InternalPrevious;
			}

			node.InternalBridge = null;
			node.InternalNext = null;
			node.InternalPrevious = null;
			this._version++;
			return node;
		}

		/// <summary>
		///		Removes all items from this dictionary.
		/// </summary>
		/// <remarks>
		///		<para>
		///			Derived class can override this method to intercept manipulation.
		///		</para>
		///		<note>
		///			<strong>Note for implementers:</strong> You must call base implementation from overrides, but you can invoke in any timing as you want to do.
		///		</note>
		///		<para>
		///			Note that the node holds reference to other nodes and underlying collection, so it could cause unpected resource leak.
		///		</para>
		/// </remarks>
		protected virtual void ClearItems()
		{
			Contract.Ensures( this.Count == 0 );

			var oldBridge = this._currentBridge;
			this._currentBridge = new Bridge( this );
			// Bulk reject.
			oldBridge.Dictionary = null;

			this._dictionary.Clear();
			this._head = null;
			this._tail = null;
			this._version++;
		}

		#endregion ---- Template Methods ----

		#region ---- Node manipulation ----

		/// <summary>
		///		Move specified node to head.
		/// </summary>
		/// <param name="moving">
		///		Moving node.
		/// </param>
		protected void MoveNodeToHead( LinkedDictionaryNode<TKey, TValue> moving )
		{
			Contract.Requires<ArgumentNullException>( moving != null );

			if ( moving == this._head )
			{
				return;
			}

			this.MoveNodeToBeforeCore( this._head, moving );
		}

		/// <summary>
		///		Move specified node to before from specified destination node.
		/// </summary>
		/// <param name="destination">
		///		Destination node.
		/// </param>
		/// <param name="moving">
		///		Moving node.
		/// </param>
		protected void MoveNodeToBefore( LinkedDictionaryNode<TKey, TValue> destination, LinkedDictionaryNode<TKey, TValue> moving )
		{
			Contract.Requires<ArgumentNullException>( destination != null );
			Contract.Requires<ArgumentNullException>( moving != null );

			this.MoveNodeToBeforeCore( destination, moving );
		}

		private void MoveNodeToBeforeCore( LinkedDictionaryNode<TKey, TValue> destination, LinkedDictionaryNode<TKey, TValue> moving )
		{
			this.MoveNode(
				moving,
				newPrevious: destination.InternalPrevious,
				newNext: destination
			);
		}

		/// <summary>
		///		Move specified node to head.
		/// </summary>
		/// <param name="moving">
		///		Moving node.
		/// </param>
		protected void MoveNodeToTail( LinkedDictionaryNode<TKey, TValue> moving )
		{
			Contract.Requires<ArgumentNullException>( moving != null );

			if ( moving == this._tail )
			{
				return;
			}

			this.MoveNodeToAfterCore( this._tail, moving );
		}

		/// <summary>
		///		Move specified node to next to specified destination node.
		/// </summary>
		/// <param name="destination">
		///		Destination node.
		/// </param>
		/// <param name="moving">
		///		Moving node.
		/// </param>
		protected void MoveNodeToAfter( LinkedDictionaryNode<TKey, TValue> destination, LinkedDictionaryNode<TKey, TValue> moving )
		{
			Contract.Requires<ArgumentNullException>( destination != null );
			Contract.Requires<ArgumentNullException>( moving != null );

			this.MoveNodeToAfterCore( destination, moving );
		}

		private void MoveNodeToAfterCore( LinkedDictionaryNode<TKey, TValue> destination, LinkedDictionaryNode<TKey, TValue> moving )
		{
			this.MoveNode(
				moving,
				newPrevious: destination,
				newNext: destination.InternalNext
			);
		}

		private void MoveNode( LinkedDictionaryNode<TKey, TValue> moving, LinkedDictionaryNode<TKey, TValue> newPrevious, LinkedDictionaryNode<TKey, TValue> newNext )
		{
			var oldPrevious = moving.InternalPrevious;
			var oldNext = moving.InternalNext;

			newPrevious.InternalNext = moving;
			moving.InternalPrevious = newPrevious;
			newNext.InternalPrevious = moving;
			moving.InternalNext = newNext;
			oldPrevious.InternalNext = oldNext;
			oldNext.InternalPrevious = oldPrevious;
			this._version++;
		}

		#endregion ---- Node manipulation ----

		#region ---- Validation ----

		private static TValue EnsureValueType( object value )
		{
			if ( value == null && !typeof( TValue ).IsValueType )
			{
				return default( TValue );
			}

			if ( !( value is TValue ) )
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"value is of a type that is not assignable to the key type '{0}' of this dictionary.",
						typeof( TValue )
					),
					"value"
				);
			}

			return ( TValue )value;
		}

		private static TKey EnsureKeyType( object key )
		{
			TKey result;
			if ( !TryEnsureKeyType( key, out result ) )
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"key is of a type that is not assignable to the key type '{0}' of this dictionary.",
						typeof( TKey )
					),
					"key"
				);
			}

			return result;
		}

		private static bool TryEnsureKeyType( object key, out TKey typedKey )
		{
			if ( key is TKey )
			{
				typedKey = ( TKey )key;
				return true;
			}
			else
			{
				typedKey = default( TKey );
				return false;
			}
		}

		#endregion ---- Validation ----

		#region ---- Get ----

		/// <summary>
		///		Get <see cref="LinkedDictionaryNode{TKey,TValue}"/> with specified key.
		/// </summary>
		/// <param name="key">
		///		The key of the value to get.
		/// </param>
		/// <returns>
		///		The <see cref="LinkedDictionaryNode{TKey,TValue}"/> with the specified key.
		/// </returns>
		///	<remarks>
		///		<para>
		///			Note that the node holds reference to other nodes and underlying collection, so it could cause unpected resource leak.
		///		</para>
		///		<para>
		///			This method calls <see cref="GetItem"/> template method.
		///		</para>
		///		<note>
		///			This method is not pure due to <see cref="GetItem"/> may not be pure.
		///		</note>
		///	</remarks>
		public LinkedDictionaryNode<TKey, TValue> GetNode( TKey key )
		{
			Contract.Requires<ArgumentNullException>( key != null );
			Contract.Ensures(
				this.ContainsKey( key ) && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() != null
				|| !this.ContainsKey( key ) && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() == null
			);

			return this.GetItem( key );
		}

		/// <summary>
		///		Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">
		///		The key whose value to get.
		///	</param>
		/// <param name="value">
		///		When this method returns, the value associated with the specified key, if the key is found; 
		///		otherwise, the default value for the type of the <paramref name="value"/> parameter. 
		///		This parameter is passed uninitialized.
		///	</param>
		/// <returns>
		///		<c>true</c> if this dictionary contains an element with the specified key; otherwise, <c>false</c>.
		/// </returns>
		///	<remarks>
		///		This method calls <see cref="GetItem"/> template method.
		///	</remarks>
		public bool TryGetValue( TKey key, out TValue value )
		{
			var node = this.GetItem( key );
			value = node == null ? default( TValue ) : node.Value;
			return node != null;
		}

		#endregion ---- Get ----

		#region ---- Add ----

		/// <summary>
		///		Adds the specified key and value to the dictionary.
		/// </summary>
		/// <param name="key">
		///		The key of the element to add.
		/// </param>
		/// <param name="value">
		///		The value of the element to add. The value can be <c>null</c> for reference types.
		/// </param>
		/// <returns>
		///		An element with the same key already does not exist in the dictionary and sucess to add then newly added node;
		///		otherwise <c>null</c>.
		/// </returns>
		public LinkedDictionaryNode<TKey, TValue> Add( TKey key, TValue value )
		{
			Contract.Requires<ArgumentNullException>( key != null );
			Contract.Ensures(
				( Contract.OldValue( this.ContainsKey( key ) ) && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() == null )
				|| ( !Contract.OldValue( this.ContainsKey( key ) ) && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() != null )
			);
			Contract.Ensures( this.ContainsKey( key ) );

			return this.AddItem( key, value );
		}

		void IDictionary<TKey, TValue>.Add( TKey key, TValue value )
		{
			if ( this.AddItem( key, value ) == null )
			{
				throw new ArgumentException( "The same key already exist in the dictionary.", "key" );
			}
		}

		void IDictionary.Add( object key, object value )
		{
			( this as IDictionary<TKey, TValue> ).Add( EnsureKeyType( key ), EnsureValueType( value ) );
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add( KeyValuePair<TKey, TValue> item )
		{
			( this as IDictionary<TKey, TValue> ).Add( item.Key, item.Value );
		}

		#endregion ---- Add ----

		#region ---- Remove ----

		/// <summary>
		///		Removes the value with the specified key from this dictionary.
		/// </summary>
		/// <param name="key">
		///		The key of the element to remove.
		///	</param>
		/// <returns>
		///		Removed node if the element is successfully found and removed;
		///		otherwise, <c>null</c>.
		/// </returns>
		public LinkedDictionaryNode<TKey, TValue> Remove( TKey key )
		{
			Contract.Requires<ArgumentNullException>( key != null );
			Contract.Ensures(
				( Contract.OldValue( this.ContainsKey( key ) ) && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() != null )
				|| ( !Contract.OldValue( this.ContainsKey( key ) ) && Contract.Result<LinkedDictionaryNode<TKey, TValue>>() == null )
			);
			Contract.Ensures( !this.ContainsKey( key ) );

			return this.RemoveItem( key );
		}

		/// <summary>
		///		Removes the specified node from this dictionary.
		/// </summary>
		/// <param name="node">
		///		The node to be removed.
		///	</param>
		public void Remove( LinkedDictionaryNode<TKey, TValue> node )
		{
			Contract.Requires<ArgumentNullException>( node != null );
			Contract.Requires<ArgumentException>( node.Dictionary == this );
			Contract.Ensures( node.Dictionary == null );
			Contract.Ensures( !this.ContainsKey( node.Key ) );

			this.RemoveItem( node.Key );
		}

		bool IDictionary<TKey, TValue>.Remove( TKey key )
		{
			return this.RemoveItem( key ) != null;
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove( KeyValuePair<TKey, TValue> item )
		{
			var node = this.GetItem( item.Key );
			if ( node == null || !EqualityComparer<TValue>.Default.Equals( node.Value, item.Value ) )
			{
				return false;
			}

			return this.RemoveItem( node.Key ) != null;
		}

		void IDictionary.Remove( object key )
		{
			TKey typedKey;
			if ( !TryEnsureKeyType( key, out typedKey ) )
			{
				return;
			}

			this.Remove( typedKey );
		}

		#endregion ---- Remove ----

		#region ---- Clear ----

		/// <summary>
		///		Removes all items from this dictionary.
		/// </summary>
		public void Clear()
		{
			this.ClearItems();
		}

		#endregion ---- Clear ----

		#region ---- Contains ----

		/// <summary>
		///		Determines whether this dictionary contains an element with the specified key.
		/// </summary>
		/// <param name="key">
		///		The key to locate in this dictionary.</param>
		/// <returns>
		///		<c>true</c> if this dictionary contains an element with the <paramref name="key"/>; otherwise, <c>false</c>.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">
		///		<paramref name="key"/> is null.
		/// </exception>
		public bool ContainsKey( TKey key )
		{
			return this._dictionary.ContainsKey( key );
		}

		/// <summary>
		///		Determines whether this dictionary contains an element with the specified value.
		/// </summary>
		/// <param name="value">
		///		The value to locate in this dictionary.</param>
		/// <returns>
		///		<c>true</c> if this dictionary contains an element with the <paramref name="value"/>; otherwise, <c>false</c>.
		/// </returns>
		[Pure]
		public bool ContainsValue( TValue value )
		{
			Contract.Ensures( !Contract.Result<bool>() || 0 < this.Count );

			return this._dictionary.Values.Any( item => EqualityComparer<TValue>.Default.Equals( value, item.Value ) );
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains( KeyValuePair<TKey, TValue> item )
		{
			var node = this.GetNode( item.Key );
			return node != null && EqualityComparer<TValue>.Default.Equals( node.Value, item.Value );
		}

		bool IDictionary.Contains( object key )
		{
			if ( !( key is TKey ) || key == null )
			{
				return false;
			}

			return this.ContainsKey( ( TKey )key );
		}

		#endregion ---- Contains ----

		#region ---- Copy ----

		/// <summary>
		///		Copies the entire dictionary as collection of <see cref="KeyValuePair{TKey,TValue}"/> 
		///		to a compatible one-dimensional array, starting at the beginning of the target array. 
		/// </summary>
		/// <param name="array">
		///		The one-dimensional <see cref="Array"/> that is the destination of the elements copied from this dictionary.
		///		The <see cref="Array"/> must have zero-based indexing.
		/// </param>
		[Pure]
		public void CopyTo( KeyValuePair<TKey, TValue>[] array )
		{
			Contract.Requires<ArgumentNullException>( array != null );

			CollectionOperation.CopyTo( this, this.Count, 0, array, 0, this.Count );
		}

		/// <summary>
		///		Copies the entire dictionary as collection of <see cref="KeyValuePair{TKey,TValue}"/>
		///		to a compatible one-dimensional array, 
		///		starting at the specified index of the target array. 
		/// </summary>
		/// <param name="array">
		///		The one-dimensional <see cref="Array"/> that is the destination of the elements copied from this dictionary.
		///		The <see cref="Array"/> must have zero-based indexing.
		/// </param>
		/// <param name="arrayIndex">
		///		The zero-based index in <paramref name="array"/> at which copying begins. 
		/// </param>
		public void CopyTo( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
		{
			CollectionOperation.CopyTo( this, this.Count, 0, array, arrayIndex, this.Count );
		}

		/// <summary>
		///		Copies a range of elements from this dictionary as collection of <see cref="KeyValuePair{TKey,TValue}"/>
		///		to a compatible one-dimensional array, 
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
		[Pure]
		public void CopyTo( int index, KeyValuePair<TKey, TValue>[] array, int arrayIndex, int count )
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
			if ( array is DictionaryEntry[] )
			{
				CollectionOperation.CopyTo( this.Select( kv => new DictionaryEntry( kv.Key, kv.Value ) ), this.Count, array, arrayIndex );
			}
			else
			{
				CollectionOperation.CopyTo<KeyValuePair<TKey, TValue>>( this, this.Count, array, arrayIndex );
			}
		}

		#endregion ---- Copy ----

		#region ---- GetEnumerator ----

		/// <summary>
		///		Returns an enumerator that iterates through the <see cref="LinkedDictionary{Tkey,TValue}"/>.
		/// </summary>
		/// <returns>
		///		Returns an enumerator that iterates through the <see cref="LinkedDictionary{Tkey,TValue}"/>.
		/// </returns>
		public Enumerator GetEnumerator()
		{
			return new Enumerator( this );
		}

		/// <summary>
		///		Returns an enumerator that iterates in reverse order through the <see cref="LinkedDictionary{Tkey,TValue}"/>.
		/// </summary>
		/// <returns>
		///		Returns an enumerator that iterates in reverse order through the <see cref="LinkedDictionary{Tkey,TValue}"/>.
		/// </returns>
		[Pure]
		public ReverseEnumerator GetReverseEnumerator()
		{
			return new ReverseEnumerator( this );
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new DictionaryEnumerator( this );
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		#endregion ---- GetEnumerator ----

		#region ---- LINQ support ----

		/// <summary>
		///		Returns the first element of a sequence.
		/// </summary>
		/// <returns>
		///		The first element in this dictionary.
		/// </returns>
		[Pure]
		public KeyValuePair<TKey, TValue> First()
		{
			// This API follows Enumerable.First<T>(IEnumerable<T>) contract.
			return Enumerable.First<KeyValuePair<TKey, TValue>>( this );
		}

		/// <summary>
		///		Returns the first element in a sequence that satisfies a specified condition.
		/// </summary>
		/// <param name="predicate">
		///		A function to test each element for a condition.
		/// </param>
		/// <returns>
		///		The first element in the sequence that passes the test in the specified <paramref name="predicate"/> function.
		/// </returns>
		[Pure]
		public KeyValuePair<TKey, TValue> First( Func<KeyValuePair<TKey, TValue>, bool> predicate )
		{
			// This API follows Enumerable.First<T>(IEnumerable<T>,Func<T,bool>) contract.
			return Enumerable.First<KeyValuePair<TKey, TValue>>( this, predicate );
		}

		/// <summary>
		///		Returns the first element of a sequence, or a default value if no element is found.
		/// </summary>
		/// <returns>
		///		<c>default(<see cref="KeyValuePair{TKey,TValue}"/></c> if source is empty; otherwise, the first element in source. 
		/// </returns>
		[Pure]
		public KeyValuePair<TKey, TValue> FirstOrDefault()
		{
			// This API follows Enumerable.FirstOrDefault<T>(IEnumerable<T>) contract.
			return Enumerable.FirstOrDefault<KeyValuePair<TKey, TValue>>( this );
		}

		/// <summary>
		///		Returns the first element of the sequence that satisfies a condition or a default value if no such element is found.
		/// </summary>
		/// <param name="predicate">
		///		A function to test each element for a condition.
		/// </param>
		/// <returns>
		///		<c>default(<see cref="KeyValuePair{TKey,TValue}"/></c> if source is empty or if no element passes the test specified by <paramref name="predicate"/>; 
		///		otherwise, the first element in source that passes the test specified by <paramref name="predicate"/>. 
		/// </returns>
		[Pure]
		public KeyValuePair<TKey, TValue> FirstOrDefault( Func<KeyValuePair<TKey, TValue>, bool> predicate )
		{
			// This API follows Enumerable.FirstOrDefault<T>(IEnumerable<T>,Func<T,bool>) contract.
			return Enumerable.FirstOrDefault<KeyValuePair<TKey, TValue>>( this, predicate );
		}

		/// <summary>
		///		Returns the last element of a sequence.
		/// </summary>
		/// <returns>
		///		The value at the last position in the source sequence.
		/// </returns>
		[Pure]
		public KeyValuePair<TKey, TValue> Last()
		{
			// This API follows Enumerable.Last<T>(IEnumerable<T>) contract.
			var node = this._tail;
			if ( node == null )
			{
				throw new InvalidOperationException( "Sequence is empty." );
			}

			return new KeyValuePair<TKey, TValue>( node.Key, node.Value );
		}

		/// <summary>
		///		Returns the last element of a sequence that satisfies a specified condition.
		/// </summary>
		/// <param name="predicate">
		///		A function to test each element for a condition.
		/// </param>
		/// <returns>
		///		The last element in the sequence that passes the test in the specified <paramref name="predicate"/> function.
		/// </returns>
		[Pure]
		public KeyValuePair<TKey, TValue> Last( Func<KeyValuePair<TKey, TValue>, bool> predicate )
		{
			// This API follows Enumerable.Last<T>(IEnumerable<T>,Func<T,bool>) contract.
			return this.Reverse().First( predicate );
		}

		/// <summary>
		///		Returns the last element of a sequence, or a default value if the sequence contains no elements.
		/// </summary>
		/// <returns>
		///		<c>default(<see cref="KeyValuePair{TKey,TValue}"/></c> if source is empty; otherwise, the last element in source. 
		/// </returns>
		[Pure]
		public KeyValuePair<TKey, TValue> LastOrDefault()
		{
			// This API follows Enumerable.LastOrDefault<T>(IEnumerable<T>) contract.
			var node = this._tail;
			if ( node == null )
			{
				return default( KeyValuePair<TKey, TValue> );
			}

			return new KeyValuePair<TKey, TValue>( node.Key, node.Value );
		}

		/// <summary>
		///		Returns the last element of the sequence that satisfies a condition or a default value if no such element is found.
		/// </summary>
		/// <param name="predicate">
		///		A function to test each element for a condition.
		/// </param>
		/// <returns>
		///		<c>default(<see cref="KeyValuePair{TKey,TValue}"/></c> if source is empty or if no element passes the test specified by <paramref name="predicate"/>; 
		///		otherwise, the last element in source that passes the test specified by <paramref name="predicate"/>. 
		/// </returns>
		[Pure]
		public KeyValuePair<TKey, TValue> LastOrDefault( Func<KeyValuePair<TKey, TValue>, bool> predicate )
		{
			// This API follows Enumerable.LastOrDefault<T>(IEnumerable<T>,Func<T,bool>) contract.
			return this.Reverse().FirstOrDefault( predicate );
		}

		/// <summary>
		///		Inverts the order of the elements in this dictionary.
		/// </summary>
		/// <returns>
		///		A sequence whose elements correspond to those of the input sequence in reverse order.
		/// </returns>
		[Pure]
		public IEnumerable<KeyValuePair<TKey, TValue>> Reverse()
		{
			// This API follows Enumerable.Reverse<T>(IEnumerable<T>) contract.
			Contract.Ensures( Contract.Result<IEnumerable<KeyValuePair<TKey, TValue>>>() != null );

			return new ReverseView( this );
		}

		#endregion ---- LINQ support ----

		#endregion -- Methods --

		#region -- Misc Nested Types --

		/// <summary>
		///		Support for bulk unlinking of nodes.
		/// </summary>
		internal sealed class Bridge
		{
			// This member is intentionally mutable for bulk unlinking on ClearItems().
			public LinkedDictionary<TKey, TValue> Dictionary;

			public Bridge( LinkedDictionary<TKey, TValue> dictionary )
			{
				this.Dictionary = dictionary;
			}
		}

		/// <summary>
		///		<see cref="IEnumerable{T}"/> for reserve order view.
		/// </summary>
		private sealed class ReverseView : IEnumerable<KeyValuePair<TKey, TValue>>
		{
			private readonly LinkedDictionary<TKey, TValue> _dictionary;

			public ReverseView( LinkedDictionary<TKey, TValue> dictionary )
			{
				this._dictionary = dictionary;
			}

			public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
			{
				return this._dictionary.GetReverseEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}

		#endregion -- Misc Nested Types --
	}
}