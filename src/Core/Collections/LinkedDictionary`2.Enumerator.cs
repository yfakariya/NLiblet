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
using System.Diagnostics.Contracts;

namespace NLiblet.Collections
{
	partial class LinkedDictionary<TKey, TValue>
	{
		/// <summary>
		///		Enumerates the elements of a <see cref="LinkedDictionary{TKey,TValue}"/> in order.
		/// </summary>
		public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IDictionaryEnumerator
		{
			private readonly LinkedDictionary<TKey, TValue> _underlying;
			private LinkedDictionaryNode<TKey, TValue> _current;
			private int _initialVersion;

			/// <summary>
			///		Gets the element at the current position of the enumerator.
			/// </summary>
			/// <value>
			///		The element in the underlying collection at the current position of the enumerator.
			/// </value>
			public KeyValuePair<TKey, TValue> Current
			{
				get
				{
					this.VerifyVersion();
					return this._current == null ? default( KeyValuePair<TKey, TValue> ) : new KeyValuePair<TKey, TValue>( this._current.Key, this._current.Value );
				}
			}

			/// <summary>
			///		Gets the element at the current position of the enumerator.
			/// </summary>
			/// <value>
			///		The element in the collection at the current position of the enumerator, as an <see cref="Object"/>.
			/// </value>
			/// <exception cref="InvalidOperationException">
			///		The enumerator is positioned before the first element of the collection or after the last element. 
			/// </exception>
			object IEnumerator.Current
			{
				get { return this.GetCurrentStrict(); }
			}

			DictionaryEntry IDictionaryEnumerator.Entry
			{
				get
				{
					var entry = this.GetCurrentStrict();
					return new DictionaryEntry( entry.Key, entry.Value );
				}
			}

			object IDictionaryEnumerator.Key
			{
				get { return this.GetCurrentStrict().Key; }
			}

			object IDictionaryEnumerator.Value
			{
				get { return this.GetCurrentStrict().Value; }
			}

			internal KeyValuePair<TKey, TValue> GetCurrentStrict()
			{
				this.VerifyVersion();

				if ( this._current == null )
				{
					throw new InvalidOperationException( "The enumerator is positioned before the first element of the collection or after the last element." );
				}

				return new KeyValuePair<TKey, TValue>( this._current.Key, this._current.Value );
			}

			internal Enumerator( LinkedDictionary<TKey, TValue> dictionary )
				: this()
			{
				Contract.Requires( dictionary != null );

				this._underlying = dictionary;
				this._initialVersion = dictionary._version;
			}

			internal void VerifyVersion()
			{
				if ( this._underlying != null && this._underlying._version != this._initialVersion )
				{
					throw new InvalidOperationException( "The collection was modified after the enumerator was created." );
				}
			}

			/// <summary>
			///		Releases all resources used by the this instance.
			/// </summary>
			public void Dispose()
			{
				// nop
			}

			/// <summary>
			///		Advances the enumerator to the next element of the underlying collection.
			/// </summary>
			/// <returns>
			///		<c>true</c> if the enumerator was successfully advanced to the next element; 
			///		<c>false</c> if the enumerator has passed the end of the collection.
			/// </returns>
			/// <exception cref="T:System.InvalidOperationException">
			///		The collection was modified after the enumerator was created. 
			///	</exception>
			public bool MoveNext()
			{
				if ( this._underlying == null )
				{
					return false;
				}

				if ( this._current == null )
				{
					// First
					this._current = this._underlying._head;
					return this._current != null;
				}
				
				if ( this._current.Next == null )
				{
					// Last
					return false;
				}
				else
				{
					this._current = this._current.Next;
					return true;
				}
			}

			/// <summary>
			///		Sets the enumerator to its initial position, which is before the first element in the collection.
			/// </summary>
			/// <exception cref="T:System.InvalidOperationException">
			///		The collection was modified after the enumerator was created. 
			///	</exception>
			void IEnumerator.Reset()
			{
				this.ResetCore();
			}

			internal void ResetCore()
			{
				this._initialVersion = this._underlying._version;
				this._current = null;
			}
		}

		/// <summary>
		///		Enumerates the elements of a <see cref="LinkedDictionary{TKey,TValue}"/> in order.
		/// </summary>
		private struct DictionaryEnumerator : IDictionaryEnumerator
		{
			private readonly LinkedDictionary<TKey, TValue> _underlying;
			private LinkedDictionaryNode<TKey, TValue> _current;
			private int _initialVersion;

			/// <summary>
			///		Gets the element at the current position of the enumerator.
			/// </summary>
			/// <value>
			///		The element in the collection at the current position of the enumerator, as an <see cref="Object"/>.
			/// </value>
			/// <exception cref="InvalidOperationException">
			///		The enumerator is positioned before the first element of the collection or after the last element. 
			/// </exception>
			public object Current
			{
				get { return this.Entry; }
			}

			internal DictionaryEntry GetCurrentStrict()
			{
				this.VerifyVersion();

				if ( this._current == null )
				{
					throw new InvalidOperationException( "The enumerator is positioned before the first element of the collection or after the last element." );
				}

				return new DictionaryEntry( this._current.Key, this._current.Value );
			}

			/// <summary>
			///		Gets the element at the current position of the enumerator.
			/// </summary>
			/// <returns>
			///		The element in the dictionary at the current position of the enumerator, as a <see cref="T:System.Collections.DictionaryEntry"/>ÅB
			///	</returns>
			/// <exception cref="T:System.InvalidOperationException">
			///		The enumerator is positioned before the first element of the collection or after the last element. 
			///	</exception>
			public DictionaryEntry Entry
			{
				get { return this.GetCurrentStrict(); }
			}

			/// <summary>
			///		Gets the key of the element at the current position of the enumerator.
			/// </summary>
			/// <returns>
			///		The key of the element in the dictionary at the current position of the enumerator.
			/// </returns>
			/// <exception cref="T:System.InvalidOperationException">
			///		The enumerator is positioned before the first element of the collection or after the last element. 
			///	</exception>
			public object Key
			{
				get { return this.Entry.Key; }
			}

			/// <summary>
			///		Gets the value of the element at the current position of the enumerator.
			/// </summary>
			/// <returns>
			///		The value of the element in the dictionary at the current position of the enumerator.
			/// </returns>
			/// <exception cref="T:System.InvalidOperationException">
			///		The enumerator is positioned before the first element of the collection or after the last element. 
			///	</exception>
			public object Value
			{
				get { return this.Entry.Value; }
			}

			internal DictionaryEnumerator( LinkedDictionary<TKey, TValue> dictionary )
				: this()
			{
				Contract.Requires( dictionary != null );

				this._underlying = dictionary;
				this._initialVersion = dictionary._version;
			}

			private void VerifyVersion()
			{
				if ( this._underlying._version != this._initialVersion )
				{
					throw new InvalidOperationException( "The collection was modified after the enumerator was created." );
				}
			}

			/// <summary>
			///		Releases all resources used by the this instance.
			/// </summary>
			public void Dispose()
			{
				// nop
			}

			/// <summary>
			///		Advances the enumerator to the next element of the underlying collection.
			/// </summary>
			/// <returns>
			///		<c>true</c> if the enumerator was successfully advanced to the next element; 
			///		<c>false</c> if the enumerator has passed the end of the collection.
			/// </returns>
			/// <exception cref="T:System.InvalidOperationException">
			///		The collection was modified after the enumerator was created. 
			///	</exception>
			public bool MoveNext()
			{
				if ( this._current == null )
				{
					// First
					this._current = this._underlying._head;
					return this._current != null;
				}
				else if ( this._current.Next == null )
				{
					// Last
					return false;
				}
				else
				{
					this._current = this._current.Next;
					return true;
				}
			}

			/// <summary>
			///		Sets the enumerator to its initial position, which is before the first element in the collection.
			/// </summary>
			/// <exception cref="T:System.InvalidOperationException">
			///		The collection was modified after the enumerator was created. 
			///	</exception>
			void IEnumerator.Reset()
			{
				this.ResetCore();
			}

			internal void ResetCore()
			{
				this._initialVersion = this._underlying._version;
				this._current = null;
			}
		}
	}
}