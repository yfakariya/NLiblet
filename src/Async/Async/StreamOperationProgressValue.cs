 

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
using System.IO;

namespace NLiblet.Async
{
	/// <summary>
	/// 	Represents async stream operation progress value.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	public partial struct StreamOperationProgressValue : global::System.IEquatable<StreamOperationProgressValue>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static StreamOperationProgressValue Null { get { return default( StreamOperationProgressValue ); } }
		
		/// <summary>
		/// 	The underlying <see cref="Stream" />.
		/// </summary>
		private readonly Stream _stream;
		
		/// <summary>
		/// 	Get the underlying <see cref="Stream" />.
		/// </summary>
		/// <value>
		/// 	The underlying <see cref="Stream" />.
		/// </value>
		private Stream Stream
		{
			get
			{
				return this._stream;
			}
		}
				
		/// <summary>
		/// 	The current progressed bytes from last progress reporting.
		/// </summary>
		private readonly int _currentlyProcessed;
		
		/// <summary>
		/// 	Get the current progressed bytes from last progress reporting.
		/// </summary>
		/// <value>
		/// 	The current progressed bytes from last progress reporting.
		/// </value>
		public int CurrentlyProcessed
		{
			get
			{
				return this._currentlyProcessed;
			}
		}
				
		/// <summary>
		/// 	The total progressed bytes from first progress reporting of this operation.
		/// </summary>
		private readonly long _totallyProcessed;
		
		/// <summary>
		/// 	Get the total progressed bytes from first progress reporting of this operation.
		/// </summary>
		/// <value>
		/// 	The total progressed bytes from first progress reporting of this operation.
		/// </value>
		public long TotallyProcessed
		{
			get
			{
				return this._totallyProcessed;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="stream">
		/// 	The underlying <see cref="Stream" />.
		/// </param>
		/// <param name="currentlyProcessed">
		/// 	The current progressed bytes from last progress reporting.
		/// </param>
		/// <param name="totallyProcessed">
		/// 	The total progressed bytes from first progress reporting of this operation.
		/// </param>
		internal StreamOperationProgressValue(
			Stream stream,
			int currentlyProcessed,
			long totallyProcessed
		)
		{
			this._stream = stream;
			this._currentlyProcessed = currentlyProcessed;
			this._totallyProcessed = totallyProcessed;
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._stream == null ? 0 : this._stream.GetHashCode() ) ^ this._currentlyProcessed.GetHashCode() ^ this._totallyProcessed.GetHashCode();
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="StreamOperationProgressValue"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="StreamOperationProgressValue"/> to compare.
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
			
			if( !( obj is StreamOperationProgressValue ) )
			{
				return false;
			}
			
			return this.Equals( ( StreamOperationProgressValue )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="StreamOperationProgressValue"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( StreamOperationProgressValue other )
		{
			return ( this._stream == null ? other._stream == null : this._stream.Equals( other._stream ) ) && this._currentlyProcessed.Equals( other._currentlyProcessed ) && this._totallyProcessed.Equals( other._totallyProcessed );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="StreamOperationProgressValue"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="StreamOperationProgressValue"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( StreamOperationProgressValue left, StreamOperationProgressValue right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="StreamOperationProgressValue"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="StreamOperationProgressValue"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( StreamOperationProgressValue left, StreamOperationProgressValue right )
		{
			return !left.Equals( right );
		}		
	}
}