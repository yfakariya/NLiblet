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

namespace NLiblet.Collections
{
	/// <summary>
	///		Reperesets node of <see cref="LinkedDictionary{TKey,TValue}"/>.
	/// </summary>
	/// <typeparam name="TKey">The type of the key of underlying dictionary.</typeparam>
	/// <typeparam name="TValue">The type of the value of underlying dictionary.</typeparam>
	/// <remarks>
	///		Note that the node holds reference to other nodes and underlying collection, so it could cause unpected resource leak,
	///		even if all of <see cref="Previous"/>, <see cref="Next"/>, and <see cref="Dictionary"/> return <c>null</c>.
	/// </remarks>
	[Serializable]
	public sealed class LinkedDictionaryNode<TKey, TValue>
	{
		internal LinkedDictionaryNode<TKey, TValue> InternalPrevious;

		/// <summary>
		///		Gets the previous node on the dictionary.
		/// </summary>
		/// <value>
		///		The previous node on the dictionary.
		///		If this node is head of the dictionry, then <c>null</c>.
		/// </value>
		public LinkedDictionaryNode<TKey, TValue> Previous
		{
			get
			{
				if ( this.Dictionary == null )
				{
					return null;
				}

				return this.InternalPrevious;
			}
		}

		/// <summary>
		///		Gets a value indicating whether this node is head of the dictionary.
		/// </summary>
		/// <value>
		///		<c>true</c> if this node is head of the dictionary; otherwise, <c>false</c>.
		/// </value>
		public bool IsHead
		{
			get { return this.Previous == null; }
		}

		internal LinkedDictionaryNode<TKey, TValue> InternalNext;

		/// <summary>
		///		Gets the next node on the dictionary.
		/// </summary>
		/// <value>
		///		The next node on the dictionary.
		///		If this node is tail of the dictionry, then <c>null</c>.
		/// </value>
		public LinkedDictionaryNode<TKey, TValue> Next
		{
			get
			{
				if ( this.Dictionary == null )
				{
					return null;
				}

				return this.InternalNext;
			}
		}

		/// <summary>
		///		Gets a value indicating whether this node is tail of the dictionary.
		/// </summary>
		/// <value>
		///		<c>true</c> if this node is tail of the dictionary; otherwise, <c>false</c>.
		/// </value>
		public bool IsTail
		{
			get { return this.Next == null; }
		}

		private readonly TKey _key;

		/// <summary>
		///		Gets the key of this node.
		/// </summary>
		/// <value>
		///		The key of this node.
		/// </value>
		public TKey Key
		{
			get { return this._key; }
		}

		internal TValue InternalValue;

		/// <summary>
		///		Gets the value of this node.
		/// </summary>
		/// <value>
		///		The value of this node.
		/// </value>
		public TValue Value
		{
			get { return this.InternalValue; }
		}

		internal LinkedDictionary<TKey, TValue>.Bridge InternalBridge;

		/// <summary>
		///		Gets the underlying dictionary.
		/// </summary>
		/// <value>
		///		The underlying dictionary.
		///		If this node is unlinked from the dictionary then <c>null</c>.
		/// </value>
		public LinkedDictionary<TKey, TValue> Dictionary
		{
			get { return this.InternalBridge == null ? null : this.InternalBridge.Dictionary; }
		}

		internal LinkedDictionaryNode( TKey key, TValue value, LinkedDictionary<TKey, TValue>.Bridge bridge, LinkedDictionaryNode<TKey,TValue> previous,LinkedDictionaryNode<TKey,TValue> next )
		{
			this._key = key;
			this.InternalValue = value;
			this.InternalBridge = bridge;
			this.InternalPrevious = previous;
			this.InternalNext = next;
		}

		/// <summary>
		///		Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		///		A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public sealed override string ToString()
		{
			return "{" + this._key + ":" + this.InternalValue + "}";
		}
	}
}
