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
	partial class LinkedHashSet<T>
	{
		/// <summary>
		///		Enumerates the elements of a <see cref="LinkedHashSet{T}"/> in order.
		/// </summary>
		public struct ReverseEnumerator : IEnumerator<T>
		{
			private readonly LinkedHashSet<T> _underlying;
			private LinkedSetNode<T> _current;
			private int _initialVersion;

			/// <summary>
			///		Gets the element at the current position of the enumerator.
			/// </summary>
			/// <value>
			///		The element in the underlying collection at the current position of the enumerator.
			/// </value>
			public T Current
			{
				get
				{
					this.VerifyVersion();
					return this._current == null ? default( T ) : this._current.Value;
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
				get
				{
					this.VerifyVersion();

					if ( this._current == null )
					{
						throw new InvalidOperationException( "The enumerator is positioned before the first element of the collection or after the last element." );
					}

					return this._current.Value;
				}
			}

			private void VerifyVersion()
			{
				if ( this._underlying._version != this._initialVersion )
				{
					throw new InvalidOperationException( "The collection was modified after the enumerator was created." );
				}
			}

			internal ReverseEnumerator( LinkedHashSet<T> set )
				:this()
			{
				Contract.Requires( set != null );

				this._underlying = set;
				this._initialVersion = set._version;
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
					this._current = this._underlying.Tail;
					return this._current != null;
				}
				else if ( this._current.Previous == null )
				{
					// Last
					return false;
				}
				else
				{
					this._current = this._current.Previous;
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
				this._initialVersion = this._underlying._version;
				this._current = null;
			}
		}
	}
}