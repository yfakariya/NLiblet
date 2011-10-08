 

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
using System.Diagnostics.CodeAnalysis;

namespace NLiblet.Collections
{
#pragma warning disable 1570, 1574, 1574
	/// <summary>
	/// 	Represents an individual item in ordered sequence.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct SequenceItem<T> : global::System.IEquatable<SequenceItem<T>>, global::System.IFormattable
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		[SuppressMessage( "Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes" )]
		public static SequenceItem<T> Null { get { return default( SequenceItem<T> ); } }
		
		/// <summary>
		/// 	Number of this item in the source sequence.
		/// </summary>
		private readonly System.Int64 _sequenceNumber;
		
		/// <summary>
		/// 	Get number of this item in the source sequence.
		/// </summary>
		/// <value>
		/// 	Number of this item in the source sequence.
		/// </value>
		public System.Int64 SequenceNumber
		{
			get
			{
				return this._sequenceNumber;
			}
		}
				
		/// <summary>
		/// 	Item.
		/// </summary>
		private readonly T _item;
		
		/// <summary>
		/// 	Get an item.
		/// </summary>
		/// <value>
		/// 	Item.
		/// </value>
		public T Item
		{
			get
			{
				return this._item;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="sequenceNumber">
		/// 	Number of this item in the source sequence.
		/// </param>
		/// <param name="item">
		/// 	Item.
		/// </param>
		public SequenceItem(
			System.Int64 sequenceNumber,
			T item
		)
		{
			this._sequenceNumber = sequenceNumber;
			this._item = item;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return this.ToString( null, null );
		}
		
		/// <summary>
		/// 	Returns string representation of this instnace with specified format.
		/// </summary>
		/// <param name="format">Format string.</param>
		/// <returns>
		/// 	String representation of this instance with specified format.
		/// </returns>
		public string ToString( string format )
		{
			return this.ToString( format, null );
		}
		
		/// <summary>
		/// 	Returns string representation of this instnace with specified <see cref="IFormatProvider"/>.
		/// </summary>
		/// <param name="formatProvider">Format provider.</param>
		/// <returns>
		/// 	String representation of this instance with specified <see cref="IFormatProvider"/>.
		/// </returns>
		public string ToString( IFormatProvider formatProvider )
		{
			return this.ToString( null, formatProvider );
		}
		
		/// <summary>
		/// 	Returns string representation of this instnace with specified format and <see cref="IFormatProvider"/>.
		/// </summary>
		/// <param name="format">Format string.</param>
		/// <param name="formatProvider">Format provider.</param>
		/// <returns>
		/// 	String representation of this instance with specified format and <see cref="IFormatProvider"/>.
		/// </returns>
		public string ToString( string format, IFormatProvider formatProvider )
		{
			return
				"[" + this._sequenceNumber.ToString( format, formatProvider ) + "]" +
				( 
					( this._item is IFormattable )
					? ( ( IFormattable )this._item ).ToString( format, formatProvider )
					: ( this._item == null ? String.Empty : this._item.ToString() ) 
				);
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return this._sequenceNumber.GetHashCode() ^ ( this._item == null ? 0 : this._item.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="SequenceItem&lt;T&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="SequenceItem&lt;T&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public override bool Equals( object obj )
		{
			if( Object.ReferenceEquals( obj, null ) )
			{
				return false;
			}
			
			if( !( obj is SequenceItem<T> ) )
			{
				return false;
			}
			
			return this.Equals( ( SequenceItem<T> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="SequenceItem&lt;T&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( SequenceItem<T> other )
		{
			return this._sequenceNumber.Equals( other._sequenceNumber ) && ( this._item == null ? other._item == null : this._item.Equals( other._item ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="SequenceItem&lt;T&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="SequenceItem&lt;T&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( SequenceItem<T> left, SequenceItem<T> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="SequenceItem&lt;T&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="SequenceItem&lt;T&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( SequenceItem<T> left, SequenceItem<T> right )
		{
			return !left.Equals( right );
		}		
	}
#pragma warning restore 1570, 1574, 1574
}