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
	///		Reperesets node of <see cref="LinkedHashSet{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of the element of underlying set.</typeparam>
	/// <remarks>
	///		Note that the node holds reference to other nodes and underlying collection, so it could cause unpected resource leak,
	///		even if all of <see cref="Previous"/>, <see cref="Next"/>, and <see cref="Set"/> return <c>null</c>.
	/// </remarks>
	public sealed class LinkedSetNode<T>
	{
		internal LinkedSetNode<T> InternalPrevious;

		/// <summary>
		///		Gets the previous node on the set.
		/// </summary>
		/// <value>
		///		The previous node on the set.
		///		If this node is head of the set, then <c>null</c>.
		/// </value>
		public LinkedSetNode<T> Previous
		{
			get
			{
				if ( this.Set == null )
				{
					return null;
				}

				return this.InternalPrevious;
			}
		}

		/// <summary>
		///		Gets a value indicating whether this node is head of the set.
		/// </summary>
		/// <value>
		///		<c>true</c> if this node is head of the set; otherwise, <c>false</c>.
		/// </value>
		public bool IsHead
		{
			get { return this.Previous == null; }
		}

		internal LinkedSetNode<T> InternalNext;

		/// <summary>
		///		Gets the next node on the set.
		/// </summary>
		/// <value>
		///		The next node on the set.
		///		If this node is tail of the set, then <c>null</c>.
		/// </value>
		public LinkedSetNode<T> Next
		{
			get
			{
				if ( this.Set == null )
				{
					return null;
				}

				return this.InternalNext;
			}
		}

		/// <summary>
		///		Gets a value indicating whether this node is tail of the set.
		/// </summary>
		/// <value>
		///		<c>true</c> if this node is tail of the set; otherwise, <c>false</c>.
		/// </value>
		public bool IsTail
		{
			get { return this.Next == null; }
		}
		
		internal T InternalValue;

		/// <summary>
		///		Gets the element value of this node.
		/// </summary>
		/// <value>
		///		The element value of this node.
		/// </value>
		public T Value
		{
			get { return this.InternalValue; }
		}

		internal LinkedHashSet<T>.Bridge InternalBridge;

		/// <summary>
		///		Gets the underlying dictionary.
		/// </summary>
		/// <value>
		///		The underlying dictionary.
		///		If this node is unlinked from the dictionary then <c>null</c>.
		/// </value>
		public LinkedHashSet<T> Set
		{
			get { return this.InternalBridge == null ? null : this.InternalBridge.Set; }
		}

		internal LinkedSetNode( T value, LinkedHashSet<T>.Bridge bridge, LinkedSetNode<T> previous, LinkedSetNode<T> next )
		{
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
			return this.InternalValue == null ? String.Empty : this.InternalValue.ToString();
		}
	}
}
