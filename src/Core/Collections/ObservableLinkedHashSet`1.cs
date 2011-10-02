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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Contracts;

namespace NLiblet.Collections
{
	/// <summary>
	///		Implements <see cref="INotifyCollectionChanged"/> for <see cref="LinkedHashSet{T}"/>.
	/// </summary>
	/// <typeparam name="T">Type of the elements.</typeparam>
	/// <remarks>
	///		This class implements <see cref="INotifyCollectionChanged"/> for mainly use on GUIs.
	///		To support it, this class also implements <see cref="IList{T}"/>, but some operations are not straightforward:
	///		<list type="bullet">
	///			<item>
	///				<see cref="this"/> getter is O(n) operation, which n is a half of <see cref="LinkedHashSet{T}.Count"/>, not O(1).
	///				This should be slower than standard <see cref="IList{T}"/> operation.
	///			</item>
	///			<item>
	///				<see cref="IList{T}.this"/> setter is O(n) operation, which n is <see cref="LinkedHashSet{T}.Count"/>,
	///				behaves like <see cref="M:RemoveAt"/> and <see cref="M:Insert"/>.
	///				This should be slower than standard <see cref="IList{T}"/> operation.
	///			</item>
	///			<item>
	///				<see cref="IndexOf"/> is O(n) operation, which n is a half of <see cref="LinkedHashSet{T}.Count"/>, not O(1).
	///				This should be slower than standard <see cref="IList{T}"/> operation.
	///			</item>
	///			<item>
	///				<see cref="IList{T}.Insert"/> is O(n) operation, which n is a half of <see cref="LinkedHashSet{T}.Count"/>.
	///				This might be faster than array based <see cref="IList{T}"/> implementation like <see cref="List{T}"/>.
	///			</item>
	///			<item>
	///				<see cref="IList{T}.RemoveAt"/> is O(n) operation, which n is a half of <see cref="LinkedHashSet{T}.Count"/>.
	///				This might be faster than array based <see cref="IList{T}"/> implementation like <see cref="List{T}"/>.
	///			</item>
	///		</list>
	/// </remarks>
	public class ObservableLinkedHashSet<T> : LinkedHashSet<T>, IList<T>, INotifyCollectionChanged, INotifyPropertyChanged
	// NET_4_5: IReadOnlyList<T>
	{
		internal static readonly PropertyChangedEventArgs CountPropertyChangedEventArgs = new PropertyChangedEventArgs( "Count" );
		internal static readonly PropertyChangedEventArgs HeadPropertyChangedEventArgs = new PropertyChangedEventArgs( "Head" );
		internal static readonly PropertyChangedEventArgs TailPropertyChangedEventArgs = new PropertyChangedEventArgs( "Tail" );
		internal static readonly PropertyChangedEventArgs ItemPropertyChangedEventArgs = new PropertyChangedEventArgs( "Item[]" );

		private readonly ReentrantBlocker _reentrantBlocker = new ReentrantBlocker();

		/// <summary>
		///		Occurs when the property value changes.
		/// </summary>
		protected virtual event PropertyChangedEventHandler PropertyChanged;

		event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
		{
			add { this.PropertyChanged += value; }
			remove { this.PropertyChanged -= value; }
		}

		/// <summary>
		///		Raises the <see cref="E:PropertyChanged"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.ComponentModel.PropertyChangedEventArgs"/> instance containing the event data.</param>
		protected virtual void OnPropertyChanged( PropertyChangedEventArgs e )
		{
			Contract.Requires<ArgumentNullException>( e != null );

			var handler = this.PropertyChanged;
			if ( handler != null )
			{
				using ( this._reentrantBlocker )
				{
					this._reentrantBlocker.Enter();

					handler( this, e );
				}
			}
		}

		/// <summary>
		///		Occurs when the collection changes.
		/// </summary>
		public event NotifyCollectionChangedEventHandler CollectionChanged;

		/// <summary>
		///		Raises the <see cref="E:CollectionChanged"/> event.
		/// </summary>
		/// <param name="e">
		///		The <see cref="System.Collections.Specialized.NotifyCollectionChangedEventArgs"/> instance containing the event data.
		///	</param>
		protected virtual void OnCollectionChanged( NotifyCollectionChangedEventArgs e )
		{
			Contract.Requires<ArgumentNullException>( e != null );

			var handler = this.CollectionChanged;
			if ( handler != null )
			{
				using ( this._reentrantBlocker )
				{
					this._reentrantBlocker.Enter();

					handler( this, e );
				}
			}
		}

		/// <summary>
		///		Gets the value at specified index.
		/// </summary>
		/// <param name="index">The index of the node located.</param>
		/// <value>
		///		Value at specified index.
		/// </value>
		/// <remarks>
		///		This operation is O(n) operation, not O(1); n is a half of <see cref="LinkedHashSet{T}.Count"/>.
		/// </remarks>
		[Pure]
		public T this[ int index ]
		{
			get
			{
				return this.GetNodeAt( index ).Value;
			}
		}

		T IList<T>.this[ int index ]
		{
			get { return this[ index ]; }
			set
			{
				this.CheckReentrancy();

				var node = this.GetNodeAt( index );
				var oldValue = base.ReplaceValueDirect( node, value );
				this.OnCollectionChanged(
					new NotifyCollectionChangedEventArgs(
						NotifyCollectionChangedAction.Replace,
						newItem: value,
						oldItem: oldValue,
						index: this.IndexOfNode( node )
					)
				);

				if ( index == 0 )
				{
					this.OnPropertyChanged( HeadPropertyChangedEventArgs );
				}

				if ( index == this.Count )
				{
					this.OnPropertyChanged( TailPropertyChangedEventArgs );
				}

				this.OnPropertyChanged( ItemPropertyChangedEventArgs );
			}
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="ObservableLinkedHashSet{T}"/> class.
		/// </summary>
		public ObservableLinkedHashSet() : base() { }

		/// <summary>
		///		Initializes a new empty instance of the <see cref="ObservableLinkedHashSet{T}"/> class.
		///		with specified comparer.
		/// </summary>
		/// <param name="comparer">
		///		The <see cref="IEqualityComparer{T}"/> object that is used to determine equality for the values in the set. 
		/// </param>
		public ObservableLinkedHashSet( IEqualityComparer<T> comparer ) : base( comparer ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="ObservableLinkedHashSet{T}"/> class
		///		with specified initial items and comparer.
		/// </summary>
		/// <param name="initial">The initial collection to be copied.</param>
		/// <param name="comparer">
		///		The <see cref="IEqualityComparer{T}"/> object that is used to determine equality for the values in the set. 
		/// </param>
		public ObservableLinkedHashSet( IEnumerable<T> initial, IEqualityComparer<T> comparer ) : base( initial, comparer ) { }

		#region -- Reentrancy --

		// Thanks to Mono's ObservableCollection implementation.

		/// <summary>
		///		Disallows reentrant attempts to change this collection.
		/// </summary>
		/// <returns>
		///		An <see cref="IDisposable"/> object that can be used to dispose of the object. 
		/// </returns>
		protected IDisposable BlockReentrancy()
		{
			this._reentrantBlocker.Enter();
			return this._reentrantBlocker;
		}

		/// <summary>
		///		Checks for reentrant attempts to change this collection.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		///		If there was a call to <see cref="BlockReentrancy"/> of which the <see cref="IDisposable"/> return value has not yet been disposed of. Typically, this means when there are additional attempts to change this collection during a <see cref="CollectionChanged"/> event. 
		///		However, it depends on when derived classes choose to call BlockReentrancy.
		/// </exception>
		protected void CheckReentrancy()
		{
			var handler = this.CollectionChanged;

			// Only have a problem if we have more than one event listener.
			if ( this._reentrantBlocker.IsBusy && handler != null && 1 < handler.GetInvocationList().Length )
			{
				throw new InvalidOperationException( "Cannot modify the collection while reentrancy is blocked." );
			}
		}

		#endregion -- Reentrancy --

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
		///			This method raises <see cref="CollectionChanged"/> event with <see cref="NotifyCollectionChangedAction.Add"/> via <see cref="OnCollectionChanged"/>.
		///		</para>
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
		protected override LinkedSetNode<T> AddItem( T item )
		{
			this.CheckReentrancy();

			var result = base.AddItem( item );
			if ( result != null )
			{
				this.OnCollectionChanged(
					new NotifyCollectionChangedEventArgs(
						NotifyCollectionChangedAction.Add,
						changedItem: result.Value,
						index: this.IndexOfNode( result )
					)
				);

				if ( this.Count == 1 )
				{
					this.OnPropertyChanged( HeadPropertyChangedEventArgs );
				}

				this.OnPropertyChanged( TailPropertyChangedEventArgs );
				this.OnPropertyChanged( CountPropertyChangedEventArgs );
				this.OnPropertyChanged( ItemPropertyChangedEventArgs );
			}

			return result;
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
		///			This method raises <see cref="CollectionChanged"/> event with <see cref="NotifyCollectionChangedAction.Remove"/> via <see cref="OnCollectionChanged"/>.
		///		</para>
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
		protected override LinkedSetNode<T> RemoveItem( T item )
		{
			this.CheckReentrancy();

			var oldHead = this.Head;
			var oldTail = this.Tail;

			var result = base.RemoveItem( item );
			if ( result != null )
			{
				this.OnCollectionChanged(
					new NotifyCollectionChangedEventArgs(
						NotifyCollectionChangedAction.Remove,
						changedItem: result.Value,
						index: this.IndexOfNode( result )
					)
				);

				if ( result == oldHead )
				{
					this.OnPropertyChanged( HeadPropertyChangedEventArgs );
				}

				if ( result == oldTail )
				{
					this.OnPropertyChanged( TailPropertyChangedEventArgs );
				}

				this.OnPropertyChanged( CountPropertyChangedEventArgs );
				this.OnPropertyChanged( ItemPropertyChangedEventArgs );
			}

			return result;
		}

		/// <summary>
		///		Removes all items from this set.
		/// </summary>
		/// <remarks>
		///		<para>
		///			This method raises <see cref="CollectionChanged"/> event with <see cref="NotifyCollectionChangedAction.Reset"/> via <see cref="OnCollectionChanged"/>.
		///		</para>
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
		protected override void ClearItems()
		{
			this.CheckReentrancy();

			base.ClearItems();
			this.OnCollectionChanged( new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
			this.OnPropertyChanged( HeadPropertyChangedEventArgs );
			this.OnPropertyChanged( TailPropertyChangedEventArgs );
			this.OnPropertyChanged( CountPropertyChangedEventArgs );
			this.OnPropertyChanged( ItemPropertyChangedEventArgs );
		}

		/// <summary>
		///		Moves the node to specified destination.
		/// </summary>
		/// <param name="moving">The moving node.</param>
		/// <param name="newPrevious">The node to be previous of <paramref name="moving"/>.</param>
		/// <param name="newNext">The node to be next of <paramref name="moving"/>.</param>
		/// <remarks>
		///		<para>
		///			This method raises <see cref="CollectionChanged"/> event with <see cref="NotifyCollectionChangedAction.Move"/> via <see cref="OnCollectionChanged"/>.
		///		</para>
		/// </remarks>
		protected override void MoveNode( LinkedSetNode<T> moving, LinkedSetNode<T> newPrevious, LinkedSetNode<T> newNext )
		{
			this.CheckReentrancy();

			int oldIndex = this.IndexOfNode( moving );
			base.MoveNode( moving, newPrevious, newNext );

#if SILVELIGHT
			// TODO: Not implemented yet
			throw new NotImplementedException();
#else
			var newIndex = this.IndexOfNode( moving );
			this.OnCollectionChanged(
				new NotifyCollectionChangedEventArgs(
					NotifyCollectionChangedAction.Move,
					changedItem: moving.Value,
					index: newIndex,
					oldIndex: oldIndex
				)
			);
#endif
			if ( ( oldIndex == 0 || newIndex == 0 ) && oldIndex != newIndex )
			{
				this.OnPropertyChanged( HeadPropertyChangedEventArgs );
			}

			if ( ( oldIndex == this.Count || newIndex == this.Count ) && oldIndex != newIndex )
			{
				this.OnPropertyChanged( TailPropertyChangedEventArgs );
			}

			if ( oldIndex != newIndex )
			{
				this.OnPropertyChanged( ItemPropertyChangedEventArgs );
			}
		}

		private int IndexOfNode( LinkedSetNode<T> moving )
		{
			return this.IndexOf( moving.Value );
		}

		/// <summary>
		///		Gets the node at specified index.
		/// </summary>
		/// <param name="index">The index of the node located.</param>
		/// <returns>
		///		Node at specified index.
		/// </returns>
		/// <remarks>
		///		This operation is O(n) operation, not O(1); n is a half of <see cref="LinkedHashSet{T}.Count"/>.
		/// </remarks>
		[Pure]
		public LinkedSetNode<T> GetNodeAt( int index )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= index );
			Contract.Requires<ArgumentOutOfRangeException>( index < this.Count );
			Contract.Ensures( Contract.Result<LinkedSetNode<T>>() != null );

			if ( index <= ( this.Count / 2 ) )
			{
				// forward linear search
				int position = 0;
				for ( var node = this.Head; node != null; node = node.Next, position++ )
				{
					if ( position == index )
					{
						return node;
					}
				}
			}
			else
			{
				// backward linear search
				int position = this.Count - 1;
				for ( var node = this.Tail; node != null; node = node.Previous, position-- )
				{
					if ( position == index )
					{
						return node;
					}
				}
			}

			Contract.Assert( false, "Must not to be here." );
			throw new IndexOutOfRangeException( "Index is out of bounds of this collection." );
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
		/// <returns>
		/// The index of <paramref name="item"/> if found in the list; otherwise, -1.
		/// </returns>
		public int IndexOf( T item )
		{
			// Assume that newer added node should be accessed more frequently.
			int position = this.Count - 1;
			for ( var node = this.Tail; node != null; node = node.Previous, position-- )
			{
				if ( this.Comparer.Equals( item, node.Value ) )
				{
					return position;
				}
			}

			return -1;
		}

		void IList<T>.Insert( int index, T item )
		{
			this.InsertCore( index, item );
		}

		private void InsertCore( int index, T item )
		{
			var node = this.Add( item ) ?? this.GetNodeDirect( item );
			var destination = this.GetNodeAt( index );
			this.MoveNodeToBefore( destination, node );
		}

		void IList<T>.RemoveAt( int index )
		{
			this.RemoveAtCore( index );
		}

		private void RemoveAtCore( int index )
		{
			this.Remove( this.GetNodeAt( index ) );
		}

		private sealed class ReentrantBlocker : IDisposable
		{
			private int _counter;

			public bool IsBusy
			{
				get { return 0 < this._counter; }
			}

			public ReentrantBlocker() { }

			public void Dispose()
			{
				this._counter--;
			}

			public void Enter()
			{
				this._counter++;
			}
		}
	}
}
