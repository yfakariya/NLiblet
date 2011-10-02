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

namespace NLiblet.Collections
{
	/// <summary>
	///		<see cref="ISet{T}"/> implementation which preserves inserting order.
	/// </summary>
	/// <typeparam name="T">Type of the item.</typeparam>
	/// <remarks>
	///		This class is similer to Java Collection Framework's LinkedHashSet.
	///		<note>
	///			For LINQ Last(), LastOrDefault(), and Reverse() operators, it is more efficient to use this class directly than via interface.
	///		</note>
	/// </remarks>
	[Serializable]
	[DebuggerDisplay( "Count={Count}" )]
	[DebuggerTypeProxy( typeof( CollectionDebuggerProxy<> ) )]
	public partial class LinkedHashSet<T> : ISet<T>
	{
		// For lookup, use LinkedDictionary<T,T> for backing store.
		private readonly Dictionary<T, LinkedSetNode<T>> _lookup;
		private int _version;

		// Bridge to bulk clear.
		private Bridge _currentBridge;

		#region -- Properties --

		#region ---- Head and Tail ----

		private LinkedSetNode<T> _head;

		/// <summary>
		///		Get the head node of this set.
		/// </summary>
		/// <value>
		///		The head node of this set. If this set is empty then <c>null</c>.
		/// </value>
		/// <remarks>
		///		Note that the node holds reference to other nodes and underlying collection, so it could cause unpected resource leak.
		/// </remarks>
		[Pure]
		public LinkedSetNode<T> Head
		{
			get
			{
				Contract.Ensures(
					this.Count == 0 && Contract.Result<LinkedSetNode<T>>() == null
					|| 0 < this.Count && Contract.Result<LinkedSetNode<T>>() != null
				);

				return this._head;
			}
		}

		private LinkedSetNode<T> _tail;

		/// <summary>
		///		Get the tail node of this set.
		/// </summary>
		/// <value>
		///		The tail node of this set. If this set is empty then <c>null</c>.
		/// </value>
		/// <remarks>
		///		Note that the node holds reference to other nodes and underlying collection, so it could cause unpected resource leak.
		/// </remarks>
		[Pure]
		public LinkedSetNode<T> Tail
		{
			get
			{
				Contract.Ensures(
					this.Count == 0 && Contract.Result<LinkedSetNode<T>>() == null
					|| 0 < this.Count && Contract.Result<LinkedSetNode<T>>() != null
				);

				return this._tail;
			}
		}

		#endregion ---- Head and Tail ----

		#region ---- Count and Comparer ----

		/// <summary>
		///		Gets the <see cref="IEqualityComparer{T}"/> that is used to determine equality for the values in the set. 
		/// </summary>
		/// <value>
		///		The <see cref="IEqualityComparer{T}"/> object that is used to determine equality for the values in the set. 
		/// </value>
		[Pure]
		public IEqualityComparer<T> Comparer
		{
			get
			{
				Contract.Ensures( Contract.Result<IEqualityComparer<T>>() != null );

				return this._lookup.Comparer;
			}
		}

		/// <summary>
		///		Gets the number of elements contained in this set.
		/// </summary>
		/// <returns>
		///		The number of elements contained in this set.
		///	</returns>
		public int Count
		{
			get
			{
				Contract.Ensures( 0 <= Contract.Result<int>() );

				return this._lookup.Count;
			}
		}

		#endregion ---- Count and Comparer ----

		#region ---- Interface Explicit Implementations ----

		bool ICollection<T>.IsReadOnly
		{
			get { return false; }
		}

		#endregion ---- Interface Explicit Implementations ----

		#endregion -- Properties --

		#region -- Constructors --

		/// <summary>
		///		Initializes a new empty instance of the <see cref="LinkedHashSet{T}"/> class.
		///		with default comparer.
		/// </summary>
		public LinkedHashSet()
		{
			this._lookup = new Dictionary<T, LinkedSetNode<T>>();
			this._currentBridge = new Bridge( this );
		}

		/// <summary>
		///		Initializes a new empty instance of the <see cref="LinkedHashSet{T}"/> class.
		///		with specified comparer.
		/// </summary>
		/// <param name="comparer">
		///		The <see cref="IEqualityComparer{T}"/> object that is used to determine equality for the values in the set. 
		/// </param>
		public LinkedHashSet( IEqualityComparer<T> comparer )
		{
			this._lookup = new Dictionary<T, LinkedSetNode<T>>( comparer );
			this._currentBridge = new Bridge( this );
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="LinkedHashSet{T}"/> class
		///		with specified initial items and comparer.
		/// </summary>
		/// <param name="initial">The initial collection to be copied.</param>
		/// <param name="comparer">
		///		The <see cref="IEqualityComparer{T}"/> object that is used to determine equality for the values in the set. 
		/// </param>
		public LinkedHashSet( IEnumerable<T> initial, IEqualityComparer<T> comparer )
		{
			Contract.Requires<ArgumentNullException>( initial != null );

			var asCollection = initial as ICollection<T>;
			this._lookup =
				asCollection != null
				? new Dictionary<T, LinkedSetNode<T>>( asCollection.Count, comparer )
				: new Dictionary<T, LinkedSetNode<T>>( comparer );

			this._currentBridge = new Bridge( this );

			foreach ( var item in initial )
			{
				this.AddItemCore( item );
			}
		}

		#endregion -- Constructors --

		#region -- Methods --

		#region ---- Template Methods ----

		/// <summary>
		///		Adds the specified element to a set.
		/// </summary>
		/// <param name="item">
		///		The element to add to the set.
		/// </param>
		/// <returns>
		///		New node if the element is added to this set; <c>null</c> if the element is already present.
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
		protected virtual LinkedSetNode<T> AddItem( T item )
		{
			Contract.Ensures(
				( Contract.OldValue( this.Contains( item ) ) && Contract.Result<LinkedSetNode<T>>() == null )
				|| ( !Contract.OldValue( this.Contains( item ) ) && Contract.Result<LinkedSetNode<T>>() != null )
			);
			Contract.Ensures( this.Contains( item ) );

			return this.AddItemCore( item );
		}

		private LinkedSetNode<T> AddItemCore( T item )
		{
			if ( this._lookup.ContainsKey( item ) )
			{
				return null;
			}

			var oldTail = this._tail;
			var node = new LinkedSetNode<T>( item, this._currentBridge, oldTail, null );
			if ( oldTail != null )
			{
				oldTail.InternalNext = node;
			}

			this._tail = node;

			if ( this._head == null )
			{
				this._head = node;
			}

			this._lookup.Add( item, node );

			this._version++;
			return node;
		}

		/// <summary>
		///		Removes the specified element from this set.
		/// </summary>
		/// <param name="item">
		///		The element to remove.
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
		protected virtual LinkedSetNode<T> RemoveItem( T item )
		{
			Contract.Ensures(
				( Contract.OldValue( this.Contains( item ) ) && Contract.Result<LinkedSetNode<T>>() != null )
				|| ( !Contract.OldValue( this.Contains( item ) ) && Contract.Result<LinkedSetNode<T>>() == null )
			);
			Contract.Ensures( !this.Contains( item ) );

			LinkedSetNode<T> existingNode;
			if ( !this._lookup.TryGetValue( item, out existingNode ) )
			{
				return null;
			}

			Contract.Assert( existingNode != null );

			var oldPrevious = existingNode.InternalPrevious;
			var oldNext = existingNode.InternalNext;
			this._lookup.Remove( item );

			existingNode.InternalBridge = null;
			existingNode.InternalNext = null;
			existingNode.InternalPrevious = null;

			if ( oldPrevious != null )
			{
				oldPrevious.InternalNext = oldNext;
			}
			else
			{
				this._head = oldNext;
			}

			if ( oldNext != null )
			{
				oldNext.InternalPrevious = oldPrevious;
			}
			else
			{
				this._tail = oldPrevious;
			}

			this._version++;
			return existingNode;
		}

		/// <summary>
		///		Removes all items from this set.
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
			oldBridge.Set = null;
			this._lookup.Clear();
			this._head = null;
			this._tail = null;
			this._version++;
		}

		#endregion ---- Template Methods ----

		#region ---- Node manipulation ----

		/// <summary>
		///		Get node for specified item.
		/// </summary>
		/// <param name="item">
		///		The element related to node.
		/// </param>
		/// <returns>
		///		If node for specifid item is present then the node;
		///		otherwise, <c>null</c>.
		/// </returns>
		[Pure]
		protected LinkedSetNode<T> GetNodeDirect( T item )
		{
			LinkedSetNode<T> existingNode;
			this._lookup.TryGetValue( item, out existingNode );
			return existingNode;
		}

		/// <summary>
		///		Replace specified node's value.
		/// </summary>
		/// <param name="node">The target node.</param>
		/// <param name="value">The value to be replaced.</param>
		/// <returns>The old value of <paramref name="node"/>.</returns>
		/// <exception cref="ArgumentException">
		///		<paramref name="value"/> is already owned by another node.
		/// </exception>
		protected T ReplaceValueDirect( LinkedSetNode<T> node, T value )
		{
			Contract.Requires<ArgumentNullException>( node != null );
			Contract.Requires<ArithmeticException>( node.Set == this );

			if ( this.Comparer.Equals( node.Value, value ) )
			{
				return node.Value;
			}

			if ( this._lookup.ContainsKey( value ) )
			{
				throw new ArgumentException( "value is owned by another node.", "value" );
			}

			var oldPrevious = node.Previous;
			var oldNext = node.Next;

			var oldValue = node.Value;

			LinkedSetNode<T> backingNode = this._lookup[ node.InternalValue ];
			this._lookup.Remove( node.InternalValue );
			backingNode.InternalValue = value;
			this._lookup.Add( value, node );

			Contract.Assert( node.Previous == oldPrevious );
			Contract.Assert( node.Next == oldNext );
			Contract.Assert( node == oldPrevious.Next );
			Contract.Assert( node == oldNext.Previous );

			return oldValue;
		}

		/// <summary>
		///		Move specified node to head.
		/// </summary>
		/// <param name="moving">
		///		Moving node.
		/// </param>
		protected void MoveNodeToHead( LinkedSetNode<T> moving )
		{
			Contract.Requires<ArgumentNullException>( moving != null );

			if ( moving == this.Head )
			{
				return;
			}

			this.MoveNodeToBeforeCore( this.Head, moving );
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
		protected void MoveNodeToBefore( LinkedSetNode<T> destination, LinkedSetNode<T> moving )
		{
			Contract.Requires<ArgumentNullException>( destination != null );
			Contract.Requires<ArgumentNullException>( moving != null );

			this.MoveNodeToBeforeCore( destination, moving );
		}

		private void MoveNodeToBeforeCore( LinkedSetNode<T> destination, LinkedSetNode<T> moving )
		{
			if ( destination == moving.InternalNext )
			{
				return;
			}

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
		protected void MoveNodeToTail( LinkedSetNode<T> moving )
		{
			Contract.Requires<ArgumentNullException>( moving != null );

			if ( moving == this.Tail )
			{
				return;
			}

			this.MoveNodeToAfterCore( this.Tail, moving );
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
		protected void MoveNodeToAfter( LinkedSetNode<T> destination, LinkedSetNode<T> moving )
		{
			Contract.Requires<ArgumentNullException>( destination != null );
			Contract.Requires<ArgumentNullException>( moving != null );

			this.MoveNodeToAfterCore( destination, moving );
		}

		private void MoveNodeToAfterCore( LinkedSetNode<T> destination, LinkedSetNode<T> moving )
		{
			if ( destination == moving.InternalPrevious )
			{
				return;
			} 
			
			this.MoveNode(
				moving,
				newPrevious: destination,
				newNext: destination.InternalNext
			);
		}

		/// <summary>
		///		Moves the node to specified destination.
		/// </summary>
		/// <param name="moving">The moving node.</param>
		/// <param name="newPrevious">The node to be previous of <paramref name="moving"/>.</param>
		/// <param name="newNext">The node to be next of <paramref name="moving"/>.</param>
		protected virtual void MoveNode( LinkedSetNode<T> moving, LinkedSetNode<T> newPrevious, LinkedSetNode<T> newNext )
		{
			Contract.Requires<ArgumentNullException>( moving != null );
			Contract.Requires<ArgumentException>( moving.Set == this );
			Contract.Requires<ArgumentException>( newPrevious == null || newPrevious.Set == this );
			Contract.Requires<ArgumentException>( newNext == null || newNext.Set == this );
			Contract.Requires<ArgumentException>( newPrevious == null || newPrevious.Next == newNext || newPrevious == moving.Previous);
			Contract.Requires<ArgumentException>( newNext == null || newNext.Previous == newPrevious || newNext == moving.Next );
			Contract.Requires<ArgumentException>( newPrevious == null || newPrevious.Previous != newNext );
			Contract.Requires<ArgumentException>( newNext == null || newNext.Next != newPrevious );

			if ( moving.InternalNext == newNext )
			{
				Contract.Assert( moving.InternalPrevious == newPrevious );
				// Nothing to do.
				return;
			}

			var oldPrevious = moving.InternalPrevious;
			var oldNext = moving.InternalNext;

			if ( newPrevious != null )
			{
				newPrevious.InternalNext = moving;
			}
			else
			{
				this._head = moving;
			}

			moving.InternalPrevious = newPrevious;

			if ( newNext != null )
			{
				newNext.InternalPrevious = moving;
			}
			else
			{
				this._tail = moving;
			}

			moving.InternalNext = newNext;

			if ( oldPrevious != null )
			{
				oldPrevious.InternalNext = oldNext;
			}
			else
			{
				this._head = oldNext;
			}

			if ( oldNext != null )
			{
				oldNext.InternalPrevious = oldPrevious;
			}
			else
			{
				this._tail = oldPrevious;
			}

			this._version++;
		}

		#endregion ---- Node manipulation ----

		#region ---- Add ----

		/// <summary>
		///		Adds the specified element to a set.
		/// </summary>
		/// <param name="item">
		///		The element to add to the set.
		/// </param>
		/// <returns>
		///		New node if the element is added to this set; <c>null</c> if the element is already present.
		/// </returns>
		public LinkedSetNode<T> Add( T item )
		{
			Contract.Ensures(
				( Contract.OldValue( this.Contains( item ) ) && Contract.Result<LinkedSetNode<T>>() == null )
				|| ( !Contract.OldValue( this.Contains( item ) ) && Contract.Result<LinkedSetNode<T>>() != null )
			);
			Contract.Ensures( this.Contains( item ) );

			return this.AddItem( item );
		}

		bool ISet<T>.Add( T item )
		{
			return this.Add( item ) != null;
		}

		void ICollection<T>.Add( T item )
		{
			this.Add( item );
		}

		#endregion ---- Add ----

		#region ---- Remove ----

		/// <summary>
		///		Removes the specified element from this set.
		/// </summary>
		/// <param name="item">
		///		The element to remove.
		///	</param>
		/// <returns>
		///		Removed node if the element is successfully found and removed;
		///		otherwise, <c>null</c>.
		/// </returns>
		public LinkedSetNode<T> Remove( T item )
		{
			Contract.Ensures(
				( Contract.OldValue( this.Contains( item ) ) && Contract.Result<LinkedSetNode<T>>() != null )
				|| ( !Contract.OldValue( this.Contains( item ) ) && Contract.Result<LinkedSetNode<T>>() == null )
			);
			Contract.Ensures( !this.Contains( item ) );

			return this.RemoveItem( item );
		}

		/// <summary>
		///		Removes the specified node from this set.
		/// </summary>
		/// <param name="node">
		///		The node to be removed.
		///	</param>
		public void Remove( LinkedSetNode<T> node )
		{
			Contract.Requires<ArgumentNullException>( node != null );
			Contract.Requires<ArgumentException>( node.Set == this );
			Contract.Ensures( node.Set == null );
			Contract.Ensures( !this.Contains( node.Value ) );

			this.RemoveItem( node.Value );
		}

		bool ICollection<T>.Remove( T item )
		{
			return this.RemoveItem( item ) != null;
		}

		#endregion ---- Remove ----

		#region ---- Clear ----

		/// <summary>
		///		Removes all items from this set.
		/// </summary>
		public void Clear()
		{
			this.ClearItems();
		}

		#endregion ---- Clear ----

		#region ---- Contains ----

		/// <summary>
		///		Determines whether this set contains an element.
		/// </summary>
		/// <param name="item">
		///		The item to locate in this set.</param>
		/// <returns>
		///		<c>true</c> if this set contains an element; otherwise, <c>false</c>.
		/// </returns>
		public bool Contains( T item )
		{
			return this._lookup.ContainsKey( item );
		}

		#endregion ---- Contains ----

		#region ---- Copy ----

		/// <summary>
		///		Copies the entire set to a compatible one-dimensional array, starting at the beginning of the target array. 
		/// </summary>
		/// <param name="array">
		///		The one-dimensional <see cref="Array"/> that is the destination of the elements copied from this set.
		///		The <see cref="Array"/> must have zero-based indexing.
		/// </param>
		[Pure]
		public void CopyTo( T[] array )
		{
			Contract.Requires<ArgumentNullException>( array != null );

			CollectionOperation.CopyTo( this, this.Count, 0, array, 0, this.Count );
		}

		/// <summary>
		///		Copies the entire set to a compatible one-dimensional array, 
		///		starting at the specified index of the target array. 
		/// </summary>
		/// <param name="array">
		///		The one-dimensional <see cref="Array"/> that is the destination of the elements copied from this set.
		///		The <see cref="Array"/> must have zero-based indexing.
		/// </param>
		/// <param name="arrayIndex">
		///		The zero-based index in <paramref name="array"/> at which copying begins. 
		/// </param>
		public void CopyTo( T[] array, int arrayIndex )
		{
			CollectionOperation.CopyTo( this, this.Count, 0, array, arrayIndex, this.Count );
		}

		/// <summary>
		///		Copies a range of elements from this set to a compatible one-dimensional array, 
		///		starting at the specified index of the target array. 
		/// </summary>
		/// <param name="index">
		///		The zero-based index in the source set at which copying begins. 
		///	</param>
		/// <param name="array">
		///		The one-dimensional <see cref="Array"/> that is the destination of the elements copied from this set.
		///		The <see cref="Array"/> must have zero-based indexing.
		/// </param>
		/// <param name="arrayIndex">
		///		The zero-based index in <paramref name="array"/> at which copying begins. 
		/// </param>
		/// <param name="count">
		///		The number of elements to copy.
		/// </param>
		[Pure]
		public void CopyTo( int index, T[] array, int arrayIndex, int count )
		{
			Contract.Requires<ArgumentNullException>( array != null );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= index );
			Contract.Requires<ArgumentException>( this.Count == 0 || index < this.Count );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= arrayIndex );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= count );
			Contract.Requires<ArgumentException>( arrayIndex < array.Length - count );

			CollectionOperation.CopyTo( this, this.Count, index, array, arrayIndex, count );
		}

		#endregion ---- Copy ----

		#region ---- GetEnumerator ----

		/// <summary>
		///		Returns an enumerator that iterates through the <see cref="LinkedHashSet{T}"/>.
		/// </summary>
		/// <returns>
		///		Returns an enumerator that iterates through the <see cref="LinkedHashSet{T}"/>.
		/// </returns>
		public Enumerator GetEnumerator()
		{
			return new Enumerator( this );
		}

		/// <summary>
		///		Returns an enumerator that iterates in reverse order through the <see cref="LinkedHashSet{T}"/>.
		/// </summary>
		/// <returns>
		///		Returns an enumerator that iterates in reverse order through the <see cref="LinkedHashSet{T}"/>.
		/// </returns>
		[Pure]
		public ReverseEnumerator GetReverseEnumerator()
		{
			return new ReverseEnumerator( this );
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return this.GetEnumerator();
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
		///		The first element in this set.
		/// </returns>
		[Pure]
		public T First()
		{
			// This API follows Enumerable.First<T>(IEnumerable<T>) contract.
			return Enumerable.First( this );
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
		public T First( Func<T, bool> predicate )
		{
			// This API follows Enumerable.First<T>(IEnumerable<T>,Func<T,bool>) contract.
			return Enumerable.First( this, predicate );
		}

		/// <summary>
		///		Returns the first element of a sequence, or a default value if no element is found.
		/// </summary>
		/// <returns>
		///		<c>default(<typeparamref name="T"/></c> if source is empty; otherwise, the first element in source. 
		/// </returns>
		[Pure]
		public T FirstOrDefault()
		{
			// This API follows Enumerable.FirstOrDefault<T>(IEnumerable<T>) contract.
			return Enumerable.FirstOrDefault( this );
		}

		/// <summary>
		///		Returns the first element of the sequence that satisfies a condition or a default value if no such element is found.
		/// </summary>
		/// <param name="predicate">
		///		A function to test each element for a condition.
		/// </param>
		/// <returns>
		///		<c>default(<typeparamref name="T"/></c> if source is empty or if no element passes the test specified by <paramref name="predicate"/>; 
		///		otherwise, the first element in source that passes the test specified by <paramref name="predicate"/>. 
		/// </returns>
		[Pure]
		public T FirstOrDefault( Func<T, bool> predicate )
		{
			// This API follows Enumerable.FirstOrDefault<T>(IEnumerable<T>,Func<T,bool>) contract.
			return Enumerable.FirstOrDefault( this, predicate );
		}

		/// <summary>
		///		Returns the last element of a sequence.
		/// </summary>
		/// <returns>
		///		The value at the last position in the source sequence.
		/// </returns>
		public T Last()
		{
			// This API follows Enumerable.Last<T>(IEnumerable<T>) contract.
			var node = this.Tail;
			if ( node == null )
			{
				throw new InvalidOperationException( "Sequence is empty." );
			}

			return node.Value;
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
		public T Last( Func<T, bool> predicate )
		{
			// This API follows Enumerable.Last<T>(IEnumerable<T>,Func<T,bool>) contract.
			return this.Reverse().First( predicate );
		}

		/// <summary>
		///		Returns the last element of a sequence, or a default value if the sequence contains no elements.
		/// </summary>
		/// <returns>
		///		<c>default(<typeparamref name="T"/></c> if source is empty; otherwise, the last element in source. 
		/// </returns>
		public T LastOrDefault()
		{
			// This API follows Enumerable.LastOrDefault<T>(IEnumerable<T>) contract.
			var node = this.Tail;
			if ( node == null )
			{
				return default( T );
			}

			return node.Value;
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
		public T LastOrDefault( Func<T, bool> predicate )
		{
			// This API follows Enumerable.LastOrDefault<T>(IEnumerable<T>,Func<T,bool>) contract.
			return this.Reverse().FirstOrDefault( predicate );
		}

		/// <summary>
		///		Inverts the order of the elements in this set.
		/// </summary>
		/// <returns>
		///		A sequence whose elements correspond to those of the input sequence in reverse order.
		/// </returns>
		public IEnumerable<T> Reverse()
		{
			// This API follows Enumerable.Reverse<T>(IEnumerable<T>) contract.
			Contract.Ensures( Contract.Result<IEnumerable<T>>() != null );

			return new ReverseView( this );
		}

		#endregion ---- LINQ support ----

		#region ---- Set Operators ----

		/// <summary>
		///		Removes all elements in the specified collection from the current set.
		/// </summary>
		/// <param name="other">
		///		The collection of items to remove from the set.
		/// </param>
		public void ExceptWith( IEnumerable<T> other )
		{
			foreach ( var item in other )
			{
				this.Remove( item );
			}
		}

		/// <summary>
		///		Modifies the current set so that it contains only elements that are present either in the current set or in the specified collection, but not both.
		/// </summary>
		/// <param name="other">
		///		The collection to compare to the current set.
		/// </param>
		public void SymmetricExceptWith( IEnumerable<T> other )
		{
			var additions = new HashSet<T>();
			var removals = new HashSet<T>();
			foreach ( var item in other )
			{
				if ( this.Contains( item ) )
				{
					removals.Add( item );
				}
				else
				{
					additions.Add( item );
				}
			}

			foreach ( var removal in removals )
			{
				this.Remove( removal );
			}

			foreach ( var addition in additions )
			{
				this.Add( addition );
			}
		}

		/// <summary>
		///		Modifies the current set so that it contains only elements that are also in a specified collection.
		/// </summary>
		/// <param name="other">
		///		The collection to compare to the current set.
		/// </param>
		public void IntersectWith( IEnumerable<T> other )
		{
			var otherSet = new HashSet<T>( other );

			var removals = new HashSet<T>();
			foreach ( var item in otherSet )
			{
				if ( !this.Contains( item ) )
				{
					removals.Add( item );
				}
			}

			foreach ( var item in this )
			{
				if ( !otherSet.Contains( item ) )
				{
					removals.Add( item );
				}
			}

			foreach ( var removal in removals )
			{
				this.Remove( removal );
			}
		}

		/// <summary>
		///		Modifies the current set so that it contains all elements that are present in both the current set and in the specified collection.
		/// </summary>
		/// <param name="other">
		///		The collection to compare to the current set.
		/// </param>
		public void UnionWith( IEnumerable<T> other )
		{
			foreach ( var item in other )
			{
				this.Add( item );
			}
		}

		/// <summary>
		///		Determines whether the current set is a property (strict) subset of a specified collection.
		/// </summary>
		/// <param name="other">
		///		The collection to compare to the current set.
		/// </param>
		/// <returns>
		///		<c>true</c> if the current set is a correct subset of <paramref name="other"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool IsProperSubsetOf( IEnumerable<T> other )
		{
			return SetOperation.IsProperSubsetOf( this, other );
		}

		/// <summary>
		///		Determines whether the current set is a property (strict) superset of a specified collection.
		/// </summary>
		/// <param name="other">
		///		The collection to compare to the current set.
		/// </param>
		/// <returns>
		///		<c>true</c> if the current set is a correct superset of <paramref name="other"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool IsProperSupersetOf( IEnumerable<T> other )
		{
			return SetOperation.IsProperSupersetOf( this, other );
		}

		/// <summary>
		///		Determines whether a set is a subset of a specified collection.
		/// </summary>
		/// <param name="other">
		///		The collection to compare to the current set.
		/// </param>
		/// <returns>
		///		<c>true</c> if the current set is a subset of <paramref name="other"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool IsSubsetOf( IEnumerable<T> other )
		{
			return SetOperation.IsSubsetOf( this, other );
		}

		/// <summary>
		///		Determines whether a set is a superset of a specified collection.
		/// </summary>
		/// <param name="other">
		///		The collection to compare to the current set.
		/// </param>
		/// <returns>
		///		<c>true</c> if the current set is a superset of <paramref name="other"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool IsSupersetOf( IEnumerable<T> other )
		{
			return SetOperation.IsSupersetOf( this, other );
		}

		/// <summary>
		///		Determines whether the current set overlaps with the specified collection.
		/// </summary>
		/// <param name="other">
		///		The collection to compare to the current set.
		/// </param>
		/// <returns>
		///		<c>true</c> if the current set and <paramref name="other"/> share at least one common element; otherwise, <c>false</c>.
		/// </returns>
		public bool Overlaps( IEnumerable<T> other )
		{
			return SetOperation.Overlaps( this, other );
		}

		/// <summary>
		///		Determines whether the current set and the specified collection contain the same elements.
		/// </summary>
		/// <param name="other">
		///		The collection to compare to the current set.
		/// </param>
		/// <returns>
		///		<c>true</c> if the current set is equal to <paramref name="other"/>; otherwise, <c>false</c>.
		/// </returns>
		public bool SetEquals( IEnumerable<T> other )
		{
			return SetOperation.SetEquals( this, other );
		}

		#endregion ---- Set Operators ----

		#endregion -- Method --

		#region -- Misc Nested Types --

		/// <summary>
		///		Support for bulk unlinking of nodes.
		/// </summary>
		internal sealed class Bridge
		{
			// This member is intentionally mutable for bulk unlinking on ClearItems().
			public LinkedHashSet<T> Set;

			public Bridge( LinkedHashSet<T> set )
			{
				this.Set = set;
			}
		}

		/// <summary>
		///		<see cref="IEnumerable{T}"/> for reserve order view.
		/// </summary>
		private sealed class ReverseView : IEnumerable<T>
		{
			private readonly LinkedHashSet<T> _set;

			public ReverseView( LinkedHashSet<T> set )
			{
				this._set = set;
			}

			public IEnumerator<T> GetEnumerator()
			{
				return this._set.GetReverseEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}
		}

		private sealed class LinkedSetNodeComparer : EqualityComparer<LinkedSetNode<T>>
		{
			private readonly IEqualityComparer<T> _valueComparer;

			public LinkedSetNodeComparer() : this( EqualityComparer<T>.Default ) { }

			public LinkedSetNodeComparer( IEqualityComparer<T> valueComparer )
			{
				Contract.Requires( valueComparer != null );
				this._valueComparer = valueComparer;
			}

			public sealed override bool Equals( LinkedSetNode<T> x, LinkedSetNode<T> y )
			{
				if ( x == null )
				{
					return y == null;
				}
				else if ( y == null )
				{
					return false;
				}

				return this._valueComparer.Equals( x.InternalValue, y.InternalValue );
			}

			public sealed override int GetHashCode( LinkedSetNode<T> obj )
			{
				return obj == null ? 0 : this._valueComparer.GetHashCode( obj.Value );
			}
		}


		#endregion -- Misc Nested Types --

	}
}
