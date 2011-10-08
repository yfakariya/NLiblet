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
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace NLiblet.Collections
{
	/// <summary>
	///		Provides the base class for a read-only collection of key/value pairs.
	/// </summary>
	/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
	/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
	[Serializable]
	[DebuggerDisplay( "Count = {Count}" )]
	[DebuggerTypeProxy( typeof( DictionaryDebuggerProxy<,> ) )]
	public class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary
	{
		private readonly IDictionary<TKey, TValue> _underlying;
		private readonly ReadOnlyNonListCollection<TKey> _keys;
		private readonly ReadOnlyNonListCollection<TValue> _values;

		/// <summary>
		///		Gets the number of elements contained in the <see cref="ReadOnlyDictionary{TKey,TValue}"/>.
		/// </summary>
		/// <returns>
		///		The number of elements contained in the <see cref="ReadOnlyDictionary{TKey,TValue}"/>.
		/// </returns>
		public int Count
		{
			get { return this._underlying.Count; }
		}

		/// <summary>
		///		Gets or sets the element with the specified key.
		/// </summary>
		/// <returns>
		///		The element with the specified key.
		/// </returns>
		public TValue this[ TKey key ]
		{
			get
			{
				return this._underlying[ key ];
			}
		}

		/// <summary>
		///		Gets an <see cref="ReadOnlyNonListCollection{TKey}"/> containing the keys of the <see cref="ReadOnlyDictionary{TKey,TValue}"/>.
		/// </summary>
		/// <returns>
		///		An <see cref="ReadOnlyNonListCollection{TKey}"/> containing the keys of the <see cref="ReadOnlyDictionary{TKey,TValue}"/>.
		///   </returns>
		public ReadOnlyNonListCollection<TKey> Keys
		{
			get { return this._keys; }
		}

		/// <summary>
		///		Gets an <see cref="ReadOnlyNonListCollection{TValue}"/> containing the values of the <see cref="ReadOnlyDictionary{TKey,TValue}"/>.
		/// </summary>
		/// <returns>
		///		An <see cref="ReadOnlyNonListCollection{TValue}"/> containing the values of the <see cref="ReadOnlyDictionary{TKey,TValue}"/>.
		///   </returns>
		public ReadOnlyNonListCollection<TValue> Values
		{
			get { return this._values; }
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Easy to re-declare." )]
		TValue IDictionary<TKey, TValue>.this[ TKey key ]
		{
			get
			{
				return this[ key ];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Easy to re-declare." )]
		ICollection<TKey> IDictionary<TKey, TValue>.Keys
		{
			get { return this.Keys; }
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Easy to re-declare." )]
		ICollection<TValue> IDictionary<TKey, TValue>.Values
		{
			get { return this.Values; }
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Easy to re-declare." )]
		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
		{
			get { return true; }
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Easy to re-declare." )]
		ICollection IDictionary.Keys
		{
			get { return this.Keys; }
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Easy to re-declare." )]
		ICollection IDictionary.Values
		{
			get { return this.Values; }
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Easy to re-declare." )]
		object IDictionary.this[ object key ]
		{
			get
			{
				return ( this._underlying as IDictionary )[ key ];
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Easy to re-declare." )]
		bool IDictionary.IsFixedSize
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

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Easy to re-declare." )]
		bool IDictionary.IsReadOnly
		{
			get { return true; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="ReadOnlyDictionary&lt;TKey, TValue&gt;"/> class.
		/// </summary>
		/// <param name="dictionary">The dictionary.</param>
		public ReadOnlyDictionary( IDictionary<TKey, TValue> dictionary )
		{
			Contract.Requires<ArgumentNullException>( dictionary != null );

			this._underlying = dictionary;
			this._keys = new ReadOnlyNonListCollection<TKey>( dictionary.Keys );
			this._values = new ReadOnlyNonListCollection<TValue>( dictionary.Values );
		}

		/// <summary>
		///		Determines whether the <see cref="ReadOnlyDictionary{TKey,TValue}"/> contains an element with the specified key.
		/// </summary>
		/// <param name="key">The key to locate in the <see cref="ReadOnlyDictionary{TKey,TValue}"/>.</param>
		/// <returns>
		///		<c>true</c> if the <see cref="ReadOnlyDictionary{TKey,TValue}"/> contains an element with the key; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsKey( TKey key )
		{
			return this._underlying.ContainsKey( key );
		}

		/// <summary>
		///		Gets the value associated with the specified key.
		/// </summary>
		/// <param name="key">The key whose value to get.</param>
		/// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the default value for the type of the <paramref name="value"/> parameter. This parameter is passed uninitialized.</param>
		/// <returns>
		///		<c>true</c> if the object that implements <see cref="ReadOnlyDictionary{TKey,TValue}"/> contains an element with the specified key; otherwise, <c>false</c>.
		/// </returns>
		public bool TryGetValue( TKey key, out TValue value )
		{
			return this._underlying.TryGetValue( key, out value );
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>
		/// true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.
		/// </returns>
		public bool Contains( KeyValuePair<TKey, TValue> item )
		{
			return this._underlying.Contains( item );
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
		/// </returns>
		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return this._underlying.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		void IDictionary<TKey, TValue>.Add( TKey key, TValue value )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		bool IDictionary<TKey, TValue>.Remove( TKey key )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		void ICollection<KeyValuePair<TKey, TValue>>.Add( KeyValuePair<TKey, TValue> item )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo( KeyValuePair<TKey, TValue>[] array, int arrayIndex )
		{
			this._underlying.CopyTo( array, arrayIndex );
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		void ICollection<KeyValuePair<TKey, TValue>>.Clear()
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		bool ICollection<KeyValuePair<TKey, TValue>>.Remove( KeyValuePair<TKey, TValue> item )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		void IDictionary.Add( object key, object value )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Just ICollection compatibility, there are very few use-cases to use this method." )]
		bool IDictionary.Contains( object key )
		{
			return this._underlying.ContainsKey( ( TKey )key );
		}

		IDictionaryEnumerator IDictionary.GetEnumerator()
		{
			return new NonGenericDictionaryEnumerator( this );
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		void IDictionary.Clear()
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Mutation methods on ReadOnly collection must not be overrided." )]
		void IDictionary.Remove( object key )
		{
			throw new NotSupportedException();
		}

		[SuppressMessage( "Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes", Justification = "Just ICollection compatibility, there are very few use-cases to use this method." )]
		void ICollection.CopyTo( Array array, int index )
		{
			Array.Copy( this.ToArray(), 0, array, index, this.Count );
		}

		private sealed class NonGenericDictionaryEnumerator : IDictionaryEnumerator
		{
			private readonly IEnumerator<KeyValuePair<TKey, TValue>> _underlying;

			public DictionaryEntry Entry
			{
				get { return new DictionaryEntry( this._underlying.Current.Key, this._underlying.Current.Value ); }
			}

			public object Key
			{
				get { return this._underlying.Current.Key; }
			}

			public object Value
			{
				get { return this._underlying.Current.Value; }
			}

			public object Current
			{
				get { return this._underlying.Current; }
			}

			public NonGenericDictionaryEnumerator( ReadOnlyDictionary<TKey, TValue> enclosing )
			{
				this._underlying = enclosing.GetEnumerator();
			}

			public bool MoveNext()
			{
				return this._underlying.MoveNext();
			}

			public void Reset()
			{
				this._underlying.Reset();
			}
		}

	}
}