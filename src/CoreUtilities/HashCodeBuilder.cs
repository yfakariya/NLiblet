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

namespace NLiblet
{
	/// <summary>
	///		Provide hash code building feature.
	/// </summary>
	partial struct HashCodeBuilder : IEquatable<HashCodeBuilder>
	{
		private readonly int _hashCode;

		private HashCodeBuilder( int hashCode )
		{
			this._hashCode = hashCode;
		}

		/// <summary>
		///		Append hash code for specified value and returns new <see cref="HashCodeBuilder"/> to chain.
		/// </summary>
		/// <typeparam name="T">Type of value.</typeparam>
		/// <param name="value">Value to append its hash code. This value can be <c>null</c>.</param>
		/// <returns>New <see cref="HashCodeBuilder"/> to chain.</returns>
		public HashCodeBuilder Append<T>( T value )
		{
			if ( value == null )
			{
				return new HashCodeBuilder( this._hashCode );
			}
			else
			{
				return new HashCodeBuilder( this._hashCode ^ value.GetHashCode() );
			}
		}

		/// <summary>
		///		Get hash code built.
		/// </summary>
		/// <returns>Hash code built.</returns>
		public int BuildHashCode()
		{
			return this._hashCode;
		}

		/// <summary>
		///		Get hash code built.
		/// </summary>
		/// <returns>Hash code built.</returns>
		public override int GetHashCode()
		{
			return this.BuildHashCode();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			return this.BuildHashCode().ToString();
		}

		/// <summary>
		///		Determines whether the specified <see cref="System.Object"/> is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		///		<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals( object obj )
		{
			if ( !( obj is HashCodeBuilder ) )
			{
				return false;
			}

			return this.Equals( ( HashCodeBuilder )obj );
		}

		/// <summary>
		///		Determines whether the specified same type other <see cref="HashCodeBuilder"/> is equal to this instance.
		/// </summary>
		/// <param name="other">The <see cref="HashCodeBuilder"/> to compare with this instance.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="other"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public bool Equals( HashCodeBuilder other )
		{
			return this._hashCode == other._hashCode;
		}

		/// <summary>
		///		Implements the operator ==.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>
		///		The result of the operator.
		/// </returns>
		public static bool operator ==( HashCodeBuilder left, HashCodeBuilder right )
		{
			return left.Equals( right );
		}

		/// <summary>
		///		Implements the operator !=.
		/// </summary>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>
		///		The result of the operator.
		/// </returns>
		public static bool operator !=( HashCodeBuilder left, HashCodeBuilder right )
		{
			return !left.Equals( right );
		}
	}
}
