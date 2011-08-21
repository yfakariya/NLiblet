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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Threading;

using NLiblet.Collections;

namespace NLiblet.IO
{
	/// <summary>
	///		Wraps <see cref="IList{T}"/> of <see cref="ArraySegment{T}"/> of <see cref="Byte"/> as <see cref="Stream"/>.
	/// </summary>
	public sealed class ByteArraySegmentsStream : Stream
	{
		/*
		 * Each fields are following (cells are _buffer):
		 * 
		 * [In use segments]
		 * |_|_|_|_|_|_|_|_| 
		 * |_|_|_|_|_|_|_|_| <- _currentSegmentIndex
		 *       ^ _offsetInCurrentSegment
		 * |_|_|_|_|_|_|_|_|
		 * |_|_|_|_|_|_|_|_| <- _lastUsedSegmentIndex
		 *       ^ offsetInLastSegment
		 * [Reserved segments]
		 * |_|_|_|_|_|_|_|_| 
		 *                 ^ _capacity
		 * 
		 * Position = seguments.Take( _currentSegmentIndex ).Sum( segment => segment.Length ) + _offsetInCurrentSegment
		 * Length = seguments.Take( _lastUsedSegmentIndex ).Sum( segment => segment.Length ) + offsetInLastSegment
		 */

		#region -- Fields --

		#region ---- Disposition ----

		/// <summary>
		///		Flag that indicates this instance is disposed. 
		///		This field shall be updated via atomic operation.
		/// </summary>
		private int _isDisposed;

		#endregion

		#region ---- Buffer Management ----

		// TODO: Use LinkedList to improve performance for large data set (order of hundred KBs or above).
		/// <summary>
		///		Chunked buffer, each segment size will be <see cref="_allocationSize"/>.
		/// </summary>
		private readonly List<ArraySegment<byte>> _buffer;

		/// <summary>
		///		Sum of lengths of all segments in <see cref="_buffer"/> to avoid costly sumuation.
		/// </summary>
		private long _capacity;

		// Maybe platform dependent...
		private static readonly int _sizeOfArraySegment = IntPtr.Size + sizeof( int ) * 2;

		/// <summary>
		///		Default allocation size for a segment.
		///		To avoid LOH allocation, this size is 64KiB now.
		/// </summary>
		private const int _defaultAllocationSize = 64 * 1024;

		/// <summary>
		///		User specified allocation size.
		///		At least, it is necessary for testing.
		/// </summary>
		private readonly int _allocationSize;

		#endregion ---- Buffer Management ----

		#region ---- Effective Data Management ----

		/// <summary>
		///		Total length of stream, that is lengths of effective segments.
		///		This field is necessary to avoid costly calculation.
		/// </summary>
		private long _length;

		/// <summary>
		///		Index of last segment of effective segments on <see cref="_buffer"/>.
		/// </summary>
		private int _lastUsedSegmentIndex;

		/// <summary>
		///		Index of last effective element of last effective segment on <see cref="_buffer"/>.
		/// </summary>
		private int _offsetInLastSegment;

		#endregion ---- Effective Data Management ----

		#region ---- Position Management ----

		/// <summary>
		///		Current position of this stream to avoid costly calculation.
		/// </summary>
		private long _position;

		/// <summary>
		///		Index of current segment on <see cref="_buffer"/>.
		///		This value always valid.
		/// </summary>
		private int _currentSegmentIndex;

		/// <summary>
		///		Index of element of current segment of <see cref="_buffer"/>.
		///		This value will be invalid to indicate that this stream position is in tail.
		/// </summary>
		private int _offsetInCurrentSegment;

		#endregion ---- Effective Data Management ----

		#endregion -- Fields --

		#region -- Properties --

		/// <summary>
		///		Gets a value indicating whether the current stream supports reading.
		/// </summary>
		/// <value>Always <c>true</c>.</value>
		public sealed override bool CanRead
		{
			get { return true; }
		}

		/// <summary>
		///		Gets a value indicating whether the current stream supports seeking.
		/// </summary>
		/// <value>Always <c>true</c>.</value>
		public sealed override bool CanSeek
		{
			get { return true; }
		}

		/// <summary>
		///		Gets a value indicating whether the current stream supports writing.
		/// </summary>
		/// <value>Always <c>true</c>.</value>
		public sealed override bool CanWrite
		{
			get { return true; }
		}

		/// <summary>
		///		Gets the length in bytes of the stream.
		/// </summary>
		/// <value>
		///		A long value representing the length of the stream in bytes.
		///		It equals to sum of each <see cref="ArraySegment{T}.Count"/> of return value of <see cref="ToList"/>.
		/// </value>
		public sealed override long Length
		{
			get { return this._length; }
		}

		/// <summary>
		///		Gets or sets the position within the current stream.
		/// </summary>
		/// <value>
		///		The current position within the stream.
		/// </value>
		public sealed override long Position
		{
			get { return this._position; }
			set { this.Seek( value - this._position, SeekOrigin.Current ); }
		}

		#endregion -- Properties --

		#region -- Constructors --

		/// <summary>
		///		Initializes a new instance of the <see cref="ByteArraySegmentsStream"/> class with default capacity.
		/// </summary>
		public ByteArraySegmentsStream() : this( _defaultAllocationSize ) { }

		/// <summary>
		///		Initializes a new instance of the <see cref="ByteArraySegmentsStream"/> class with specified capacity.
		/// </summary>
		/// <param name="initialCapacity">The initial capacity of backing store.</param>
		public ByteArraySegmentsStream( long initialCapacity )
			: this( initialCapacity, _defaultAllocationSize ) { }

		/// <summary>
		///		Testing purpose only.
		///		Initializes a new instance of the <see cref="ByteArraySegmentsStream"/> class.
		/// </summary>
		/// <param name="initialCapacity">The initial capacity of backing store.</param>
		/// <param name="allocationSize">Size of the allocation of backing store.</param>
		internal ByteArraySegmentsStream( long initialCapacity, int allocationSize )
		{
			this._allocationSize = allocationSize;
			this._buffer = new List<ArraySegment<byte>>( unchecked( ( int )( ( initialCapacity / allocationSize + ( initialCapacity % allocationSize == 0 ? 0 : 1 ) ) & 0x7fffffff ) ) );
			this.AssertInternalInvariant();
		}

		#endregion -- Constructors --

		#region -- Dispose --

		/// <summary>
		///			Releases the unmanaged resources used by the <see cref="Stream"/> 
		///			and optionally releases the managed resources. 
		/// </summary>
		/// <param name="disposing">
		///		<c>true</c> to release both managed and unmanaged resources;
		///		<c>false</c> to release only unmanaged resources.
		///	</param>
		protected sealed override void Dispose( bool disposing )
		{
			Interlocked.Exchange( ref this._isDisposed, 1 );
			base.Dispose( disposing );
		}

		#endregion -- Dispose --

		#region -- Contracts --

		/// <summary>
		///		Assert internal invariant on debug build.
		/// </summary>
		[Conditional( "DEBUG" )]
		private void AssertInternalInvariant()
		{
			// This method asserts internal fields, so we cannot use ObjectInvariant.
			Contract.Assert(
				0 <= this._position,
				"0 <= " + this._position
			);
			Contract.Assert(
				0 <= this._length,
				"0 <= " + this._length
			);
			Contract.Assert(
				0 <= this._currentSegmentIndex,
				"0 <= " + this._currentSegmentIndex
			);
			Contract.Assert(
				0 <= this._lastUsedSegmentIndex,
				"0 <= " + this._lastUsedSegmentIndex
			);
			Contract.Assert(
				0 <= this._offsetInCurrentSegment,
				"0 <= " + this._offsetInCurrentSegment
			);
			Contract.Assert(
				0 <= this._offsetInLastSegment,
				"0 <= " + this._offsetInLastSegment
			);
			Contract.Assert(
				this._position <= this._length,
				this._position + " <= " + this._length
			);
			Contract.Assert(
				this._currentSegmentIndex <= this._lastUsedSegmentIndex,
				this._currentSegmentIndex + " <= " + this._lastUsedSegmentIndex
			);
			if ( this._buffer.Count == 0 )
			{
				Contract.Assert(
					this._currentSegmentIndex == 0,
					this._currentSegmentIndex.ToString()
				);
				Contract.Assert(
					this._lastUsedSegmentIndex == 0,
					this._lastUsedSegmentIndex.ToString()
				);
				Contract.Assert(
					this._length == 0,
					this._length.ToString()
				);
			}
			else
			{
				Contract.Assert(
					this._currentSegmentIndex < this._buffer.Count,
					this._currentSegmentIndex + " < " + this._buffer.Count
				);
				Contract.Assert(
					this._lastUsedSegmentIndex < this._buffer.Count,
					this._lastUsedSegmentIndex + " < " + this._buffer.Count
				);
			}

			Contract.Assert(
				this._capacity == this._buffer.Sum( item => ( long )item.Count ),
				this._capacity + " == " + this._buffer.Sum( item => ( long )item.Count )
			);
			Contract.Assert(
				this._position == this._buffer.Take( this._currentSegmentIndex ).Sum( item => ( long )item.Count ) + this._offsetInCurrentSegment,
				this._position + " == " + this._buffer.Take( this._currentSegmentIndex ).Sum( item => ( long )item.Count ) + " + " + this._offsetInCurrentSegment
			);
			Contract.Assert(
				this._length == this._buffer.Take( this._lastUsedSegmentIndex ).Sum( item => ( long )item.Count ) + this._offsetInLastSegment,
				this._length + " == " + this._buffer.Take( this._lastUsedSegmentIndex ).Sum( item => ( long )item.Count ) + " + " + this._offsetInLastSegment
			);
		}

		private void VerifyIsNotDisposed()
		{
			if ( this._isDisposed != 0 )
			{
				throw new ObjectDisposedException( typeof( ByteArraySegmentsStream ).FullName );
			}
		}

		#endregion

		#region -- Allocation --

		private IList<ArraySegment<byte>> AllocateFromGCHeap( long requiredSize )
		{
			long temp = ( requiredSize / this._allocationSize ) + ( requiredSize % this._allocationSize == 0 ? 0 : 1 );
			if ( Int32.MaxValue < temp )
			{
				throw new InsufficientMemoryException( "Required size is too large." );
			}

			int iteration = unchecked( ( int )temp );
			long actualRequiredSize = requiredSize + _sizeOfArraySegment * iteration;

			int requiredSizeInMB = unchecked( ( int )( ( actualRequiredSize / ( 1024 * 1024 ) ) + ( ( actualRequiredSize % ( 1024 * 1024 ) ) == 0 ? 0 : 1 ) ) ) + 1;

			using ( var barriar = new MemoryFailPoint( requiredSizeInMB ) )
			{
				var result = new ArraySegment<byte>[ iteration ];

				for ( int i = 0; i < iteration; i++ )
				{
					result[ i ] = new ArraySegment<byte>( new byte[ this._allocationSize ] );
				}

				return result;
			}
		}

		#endregion -- Allocation --

		#region -- Flush --

		/// <summary>
		///		Overrides <see cref="Stream.Flush"/> so that no action is performed.
		/// </summary>
		public sealed override void Flush()
		{
			// nop
		}

		#endregion -- Flush --

		#region -- Seeking --

		/// <summary>
		///		Sets the position within the current stream to the specified value.
		/// </summary>
		/// <param name="offset">
		///		The new position within the stream. 
		///		This is relative to the <paramref name="origin"/> parameter, and can be positive or negative. 
		///	</param>
		/// <param name="origin">
		///		A value of type <see cref="SeekOrigin"/>, which acts as the seek reference point. 
		///	</param>
		/// <returns>
		///		The new position within the stream, calculated by combining the initial reference point and the offset.
		/// </returns>
		/// <exception cref="IOException">
		///		Destination position will be lessor than 0.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		Destination position will be greator than <see cref="Int32.MaxValue"/>.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///		The current stream instance is closed.
		///	</exception>
		public sealed override long Seek( long offset, SeekOrigin origin )
		{
			if ( Int32.MaxValue < offset )
			{
				throw new NotSupportedException( "offset cannot be grator than Int32.MaxValue." );
			}

			switch ( origin )
			{
				case SeekOrigin.Begin:
				{
					if ( offset < 0 )
					{
						throw new IOException( "Cannot be before position 0." );
					}

					break;
				}
				case SeekOrigin.Current:
				{
					if ( this._position + offset < 0 )
					{
						throw new IOException( "Cannot be before position 0." );
					}

					if ( Int32.MaxValue < this._position + offset )
					{
						throw new NotSupportedException( "Length cannot be grator than Int32.MaxValue." );
					}

					break;
				}
				case SeekOrigin.End:
				{
					if ( this._length + offset < 0 )
					{
						throw new IOException( "Cannot be before position 0." );
					}

					if ( Int32.MaxValue < this._length + offset )
					{
						throw new NotSupportedException( "Length cannot be grator than Int32.MaxValue." );
					}

					break;
				}
				default:
				{
					throw new ArgumentOutOfRangeException( "origin" );
				}
			}

			this.VerifyIsNotDisposed();

			long destination;
			switch ( origin )
			{
				case SeekOrigin.Begin:
				{
					destination = offset;
					break;
				}
				case SeekOrigin.End:
				{
					destination = this._length + offset;
					break;
				}
				default:
				{
					destination = this._position + offset;
					break;
				}
			}

			long offsetFromCurrent = destination - this._position;
			if ( offsetFromCurrent < 0 )
			{
				this.Back( offsetFromCurrent );
			}
			else if ( 0 < offsetFromCurrent )
			{
				this.Forward( offsetFromCurrent );
			}

			this.AssertInternalInvariant();
			return this._position;
		}

		/// <summary>
		///		Moves position backwardly.
		/// </summary>
		/// <param name="offsetFromCurrent">Offset from current position. This value must be negative.</param>
		private void Back( long offsetFromCurrent )
		{
			Contract.Assert( offsetFromCurrent < 0 );
			long movingLength = -offsetFromCurrent;
			while ( true )
			{
				if ( movingLength <= this._offsetInCurrentSegment )
				{
					// Not preceed current segment.
					this._position -= movingLength;
					this._offsetInCurrentSegment -= unchecked( ( int )movingLength );

					return;
				}
				else
				{
					// We need to move previous segment.
					movingLength -= this._offsetInCurrentSegment;
					this._position -= this._offsetInCurrentSegment;
					this._currentSegmentIndex--;
					this._offsetInCurrentSegment = this._buffer[ this._currentSegmentIndex ].Count;

					this.AssertInternalInvariant();
				}
			}
		}

		/// <summary>
		///		Moves position forwardly.
		/// </summary>
		/// <param name="offsetFromCurrent"></param>
		private void Forward( long offsetFromCurrent )
		{
			Contract.Assert( 0 < offsetFromCurrent );

			long movingLength = offsetFromCurrent;
			while ( true )
			{
				if ( movingLength + this._offsetInCurrentSegment <= this._buffer[ this._currentSegmentIndex ].Count )
				{
					// Current segment has enough size.
					this._position += movingLength;
					this._offsetInCurrentSegment += unchecked( ( int )movingLength );

					if ( this._length < this._position )
					{
						// Now extra buffer after _length is consumed, just adjust _length.
						this._lastUsedSegmentIndex = this._currentSegmentIndex;
						this._offsetInLastSegment = this._offsetInCurrentSegment;
						this._length = this._position;
					}

					return;
				}
				else
				{
					// More segments are needed.
					int remain = this._buffer[ this._currentSegmentIndex ].Count - this._offsetInCurrentSegment;
					int moved = unchecked( ( int )( remain < movingLength ? remain : remain - movingLength ) );

					// Subtract consumed size with current segment.
					this._position += moved;
					movingLength -= moved;

					if ( this._length < this._position )
					{
						// Now extra buffer after _length is consumed, just adjust _length.
						var exceeded = this._position - this._length;
						Contract.Assert( exceeded <= moved );
						this._length += exceeded;
						this._offsetInLastSegment += unchecked( ( int )exceeded );
					}

					if ( 0 < movingLength && this._capacity == this._position )
					{
						// All buffer was taken, so allocation is needed.
						this.Expand( movingLength );
					}

					// We move to next section so reflect it.
					this._currentSegmentIndex++;
					this._offsetInCurrentSegment = 0;

					this.AssertInternalInvariant();
				}
			}
		}

		/// <summary>
		///		Ensure required sized buffer is allocated.
		///		If there is not enough size then new segments will be allocated.
		/// </summary>
		/// <param name="requiredSize"></param>
		private void EnsureAllocated( long requiredSize )
		{
			long allocating = ( this._position + requiredSize ) - this._capacity;
			if ( allocating <= 0 )
			{
				// OK
				return;
			}

			//var forwarding = this._capacity - this._position;
			//if ( 0 < forwarding )
			//{
			//    this.Forward( forwarding );
			//}

			this.Expand( allocating );
			this.AssertInternalInvariant();
		}

		/// <summary>
		///		Expand (i.e. allocate) buffer to satisfy specified size.
		/// </summary>
		/// <param name="requiredSize">Required buffer size to be allocated.</param>
		private void Expand( long requiredSize )
		{
			bool isInitialAllocation = this._buffer.Count == 0;
			var expanded = this.AllocateFromGCHeap( requiredSize );
			var expandedSize = expanded.Sum( segment => ( long )segment.Count );

			this._buffer.AddRange( expanded );

			// Modify states.
			var effectiveExpandedSize = requiredSize + ( this._capacity - this._length );
			this._length += effectiveExpandedSize;
			this._capacity += expandedSize;

			// Calculate new offsets.
			int actualLastSegmentIndex = this._lastUsedSegmentIndex;
			int actualOffsetInLastSegmentIndex = this._offsetInLastSegment;
			for ( long remain = effectiveExpandedSize; 0 < remain; actualLastSegmentIndex++ )
			{
				var segmentRemain = this._buffer[ actualLastSegmentIndex ].Count;
				if ( actualLastSegmentIndex == this._lastUsedSegmentIndex )
				{
					segmentRemain -= this._offsetInLastSegment;
				}

				if ( remain <= segmentRemain )
				{
					actualOffsetInLastSegmentIndex = unchecked( ( int )remain );
					break;
				}

				remain -= segmentRemain;

				if ( remain == 0 )
				{
					break;
				}
			}

			this._lastUsedSegmentIndex = actualLastSegmentIndex;
			this._offsetInLastSegment = actualOffsetInLastSegmentIndex;
		}

		/// <summary>
		///		Sets the length of the current stream to the specified value.
		/// </summary>
		/// <param name="value">
		///		The value at which to set the length. 
		/// </param>
		/// <exception cref="NotSupportedException">
		///		Length will be greator than <see cref="Int32.MaxValue"/>.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///		The current stream instance is closed.
		///	</exception>
		public sealed override void SetLength( long value )
		{
			if ( Int32.MaxValue < value )
			{
				throw new NotSupportedException( "Length cannot be grator than Int32.MaxValue." );
			}

			this.VerifyIsNotDisposed();

			if ( value == this._length )
			{
				return;
			}

			if ( this._length < value )
			{
				this.EnsureAllocated( value - this._length );
			}

			this._length = value;
			this.AssertInternalInvariant();
		}

		#endregion -- Seeking --

		#region -- Reading --

		/// <summary>
		///		Reads a block of bytes from the current stream and writes the data to <paramref name="buffer"/>.
		/// </summary>
		/// <param name="buffer">
		///		When this method returns, contains the specified byte array 
		///		with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) 
		///		replaced by the characters read from the current stream. 
		/// </param>
		/// <param name="offset">
		///		The byte offset in <paramref name="buffer"/> at which to begin reading. 
		/// </param>
		/// <param name="count">
		///		The maximum number of bytes to read. 
		/// </param>
		/// <returns>
		///		The total number of bytes written into the buffer. 
		///		This can be less than the number of bytes requested if that number of bytes are not currently available, 
		///		or zero if the end of the stream is reached before any bytes are read. 
		/// </returns>
		/// <exception cref="T:System.ObjectDisposedException">
		///		The current stream instance is closed.
		///	</exception>
		/// <remarks>
		///		This method causes byte array copying.
		/// </remarks>
		public sealed override int Read( byte[] buffer, int offset, int count )
		{
			this.VerifyIsNotDisposed();

			int readCount = 0;

			foreach ( var segment in this.Read( count ) )
			{
				Buffer.BlockCopy( segment.Array, segment.Offset, buffer, offset + readCount, segment.Count );
				readCount += segment.Count;
			}

			this.AssertInternalInvariant();

			return readCount;
		}

		/// <summary>
		///		Reads a block of bytes from the current stream as list of <see cref="ArraySegment{T}"/> of byte.
		/// </summary>
		/// <param name="count">
		///		The maximum number of bytes to read. 
		/// </param>
		/// <returns>
		///		List of <see cref="ArraySegment{T}"/>.
		///		Each segment represents a piece of read buffer.
		///		The total length can be less than the number of bytes requested 
		///		if that number of bytes are not currently available, 
		///		or zero if the end of the stream is reached before any bytes are read. 
		/// </returns>
		/// <exception cref="T:System.ObjectDisposedException">
		///		The current stream instance is closed.
		///	</exception>
		/// <remarks>
		///		This method does not cause byte copying.
		///		<note>
		///			You should not modify contents of <see cref="ArraySegment{T}.Array"/>,
		///			because it causes unexpected behavior when multiple consumer uses underlying stream.
		///		</note>
		/// </remarks>
		public IList<ArraySegment<byte>> Read( int count )
		{
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= count );
			Contract.Ensures( Contract.Result<IList<ArraySegment<byte>>>() != null );
			Contract.Ensures( Contract.Result<IList<ArraySegment<byte>>>().Sum( item => item.Count ) <= count );

			this.VerifyIsNotDisposed();

			if ( this._position == this._length || count == 0 )
			{
				return Empty.Array<ArraySegment<byte>>();
			}

			var result = new List<ArraySegment<byte>>();
			int readCount = 0;
			while ( readCount < count )
			{
				var segment = this._buffer[ this._currentSegmentIndex ];
				int remainInCurrentSegment;
				if ( this._currentSegmentIndex == this._lastUsedSegmentIndex )
				{
					remainInCurrentSegment = this._offsetInLastSegment - this._offsetInCurrentSegment;
				}
				else
				{
					remainInCurrentSegment = segment.Count - this._offsetInCurrentSegment;
				}

				if ( remainInCurrentSegment <= 0 )
				{
					if ( this._currentSegmentIndex == this._buffer.Count - 1 )
					{
						// Now on tail.
						break;
					}
					else
					{
						// Move to next segment.
						this._currentSegmentIndex++;
						this._offsetInCurrentSegment = 0;
						continue;
					}
				}

				int reading = Math.Min( Math.Min( segment.Count, remainInCurrentSegment ), count - readCount );
				readCount += reading;
				result.Add( new ArraySegment<byte>( segment.Array, this._offsetInCurrentSegment, reading ) );

				// Adjust position.
				this._position += reading;
				this._offsetInCurrentSegment += reading;
			}

			this.AssertInternalInvariant();

			return result;
		}

		/// <summary>
		///		Reads a byte from the current stream.
		/// </summary>
		/// <returns>
		///		The byte cast to a <see cref="Int32"/>, or -1 if the end of the stream has been reached.
		/// </returns>
		/// <exception cref="T:System.ObjectDisposedException">
		///		The current stream instance is closed.
		///	</exception>
		public sealed override int ReadByte()
		{
			this.VerifyIsNotDisposed();

			if ( this._length <= this._position )
			{
				return -1;
			}

			if ( this._offsetInCurrentSegment == this._buffer[ this._currentSegmentIndex ].Count )
			{
				this._currentSegmentIndex++;
				this._offsetInCurrentSegment = 0;
			}

			var result = this._buffer[ this._currentSegmentIndex ].GetItemAt( this._offsetInCurrentSegment );

			this.Forward( 1 );

			this.AssertInternalInvariant();

			return result;
		}

		#endregion -- Reading --

		#region -- Writing --

		/// <summary>
		///		Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
		/// </summary>
		/// <param name="buffer">
		///		An array of bytes. 
		///		This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream. 
		///	</param>
		/// <param name="offset">
		///		The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream. 
		///	</param>
		/// <param name="count">
		///		The number of bytes to be written to the current stream. 
		///	</param>
		/// <exception cref="NotSupportedException">
		///		Length will be greator than <see cref="Int32.MaxValue"/>.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///		The current stream instance is closed.
		///	</exception>
		/// <remarks>
		///		This method causes byte array copying.
		/// </remarks>
		public sealed override void Write( byte[] buffer, int offset, int count )
		{
			if ( Int32.MaxValue < this._length + count )
			{
				throw new NotSupportedException( "Length cannot be grator than Int32.MaxValue." );
			}

			this.VerifyIsNotDisposed();

			for (
				int currentOffset = offset, currentCount = count;
				0 < currentCount;
			)
			{
				this.EnsureAllocated( currentCount );
				this.WriteToCurrentSegment( buffer, ref currentOffset, ref currentCount );
			}

			this.AssertInternalInvariant();
		}

		/// <summary>
		///		Write data to current segment.
		/// </summary>
		/// <param name="buffer">Buffer.</param>
		/// <param name="offset">Offset reference. This value will be incremented.</param>
		/// <param name="count">Count reference. This value will be decremented.</param>
		private void WriteToCurrentSegment( byte[] buffer, ref int offset, ref int count )
		{
			var segment = this._buffer[ this._currentSegmentIndex ];
			int remainInSegment = segment.Count - this._offsetInCurrentSegment;
			int writing = Math.Min( count, remainInSegment );

			Buffer.BlockCopy( buffer, offset, segment.Array, segment.Offset + this._offsetInCurrentSegment, writing );

			if ( count <= remainInSegment )
			{
				this._offsetInCurrentSegment += writing;
			}
			else
			{
				// Next segment.
				this._offsetInCurrentSegment = 0;
				this._currentSegmentIndex++;
			}

			offset += writing;
			count -= writing;

			// FowardPointers
			this._position += writing;
			if ( this._length < this._position )
			{
				this._lastUsedSegmentIndex = this._currentSegmentIndex;
				this._offsetInLastSegment = this._offsetInCurrentSegment;
				this._length = this._position;
			}

			this.AssertInternalInvariant();
		}

		/// <summary>
		///		Writes a byte to the current stream at the current position.
		/// </summary>
		/// <param name="value">
		///		The byte to write. 
		///	</param>
		/// <exception cref="NotSupportedException">
		///		Length will be greator than <see cref="Int32.MaxValue"/>.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		///		The current stream instance is closed.
		///	</exception>
		public sealed override void WriteByte( byte value )
		{
			if ( this._length == Int32.MaxValue )
			{
				throw new NotSupportedException( "Length cannot be grator than Int32.MaxValue." );
			}

			this.VerifyIsNotDisposed();

			this.EnsureAllocated( 1 );

			if ( this._buffer[ this._currentSegmentIndex ].Count == this._offsetInCurrentSegment )
			{
				this._currentSegmentIndex++;
				this._offsetInCurrentSegment = 0;
			}

			this._buffer[ this._currentSegmentIndex ].SetItemAt( this._offsetInCurrentSegment, value );

			this.Forward( 1 );

			this.AssertInternalInvariant();
		}

		#endregion -- Writing --

		#region -- Inserting --

		/// <summary>
		///		Insert specifid bytes to current position without copying.
		/// </summary>
		/// <param name="value">
		///		Bytes to be inserted.
		/// </param>
		/// <param name="offset">
		///		The zero-based byte offset in <paramref name="value"/> at which to begin copying bytes to the current stream. 
		///	</param>
		/// <param name="count">
		///		The number of bytes to be written to the current stream. 
		///	</param>
		/// <exception cref="T:System.ObjectDisposedException">
		///		The current stream instance is closed.
		///	</exception>
		/// <remarks>
		///		This method does not cause byte copying.
		///		<note>
		///			You should not modify contents of <see cref="Byte"/>[],
		///			because it causes unexpected behavior when multiple consumer uses underlying stream.
		///		</note>
		/// </remarks>
		public void Insert( byte[] value, int offset, int count )
		{
			Contract.Requires<IOException>( this.Length + count < Int32.MaxValue );

			this.Insert( new ArraySegment<byte>( value, offset, count ) );
		}

		/// <summary>
		///		Insert specifid bytes to current position without copying.
		/// </summary>
		/// <param name="value">
		///		Bytes to be inserted.
		/// </param>
		/// <exception cref="T:System.ObjectDisposedException">
		///		The current stream instance is closed.
		///	</exception>
		/// <remarks>
		///		This method does not cause byte copying.
		///		<note>
		///			You should not modify contents of <see cref="ArraySegment{T}"/>,
		///			because it causes unexpected behavior when multiple consumer uses underlying stream.
		///		</note>
		/// </remarks>
		public void Insert( ArraySegment<byte> value )
		{
			Contract.Requires<IOException>( this.Length + value.Count < Int32.MaxValue );

			this.VerifyIsNotDisposed();

			if ( value.Count == 0 )
			{
				return;
			}

			if ( this._buffer.Count == 0 )
			{
				// Newly append.
				this._buffer.Add( value );
				this._length = value.Count;
				this._position = value.Count;
				this._capacity = value.Count;
				this._offsetInCurrentSegment = value.Count;
				this._offsetInLastSegment = value.Count;
				this._currentSegmentIndex = 0;
				this._lastUsedSegmentIndex = 0;
				return;
			}

			var currentSegment = this._buffer[ this._currentSegmentIndex ];

			// Split current segment.
			var previousSegment = new ArraySegment<byte>( currentSegment.Array, currentSegment.Offset, this._offsetInCurrentSegment );
			var nextSegment = new ArraySegment<byte>( currentSegment.Array, currentSegment.Offset + this._offsetInCurrentSegment, currentSegment.Count - this._offsetInCurrentSegment );

			if ( previousSegment.Count == 0 )
			{
				// Insert before current segment.
				this._buffer[ this._currentSegmentIndex ] = value;
				this._position += value.Count;

				if ( 0 < nextSegment.Count )
				{
					this._buffer.Insert( this._currentSegmentIndex + 1, nextSegment );
					this._lastUsedSegmentIndex++;
					this._currentSegmentIndex++;
					if ( this._currentSegmentIndex == this._lastUsedSegmentIndex )
					{
						// Now splits last segment, modify offset-in-last to reflect splitting.
						this._offsetInLastSegment -= this._offsetInCurrentSegment;
					}
					this._offsetInCurrentSegment = 0;
				}
				else
				{
					// magic island?
					this._offsetInCurrentSegment = value.Count;
					this._offsetInLastSegment = value.Count;
				}
			}
			else
			{
				this._buffer[ this._currentSegmentIndex ] = previousSegment;

				if ( 0 < nextSegment.Count )
				{
					// Avoid array shift as long as possible...
					this._buffer.InsertRange( this._currentSegmentIndex + 1, new ArraySegment<byte>[] { value, nextSegment } );
					this._position += value.Count;
					this._lastUsedSegmentIndex += 2;
					this._currentSegmentIndex += 2;
					if ( this._currentSegmentIndex == this._lastUsedSegmentIndex )
					{
						// Now splits last segment, modify offset-in-last to reflect splitting.
						this._offsetInLastSegment -= this._offsetInCurrentSegment;
					}
					this._offsetInCurrentSegment = 0;
				}
				else
				{
					// Insert after current segment.
					this._buffer.Insert( this._currentSegmentIndex + 1, value );
					this._position += value.Count;
					this._lastUsedSegmentIndex++;
					this._currentSegmentIndex++;
					this._offsetInCurrentSegment = value.Count;
					this._offsetInLastSegment = value.Count;
				}
			}

			this._capacity += value.Count;
			this._length += value.Count;

			this.AssertInternalInvariant();
		}

		#endregion -- Inserting --

		#region -- ToList --

		/// <summary>
		///		Writes the stream contents to a list of <see cref="ArraySegment{T}"/>, regardless of the <see cref="Position"/> property.
		/// </summary>
		/// <returns>A new list of <see cref="ArraySegment{T}"/>.</returns>
		/// <remarks>
		///		Actual type of return value might be changed in the future.
		/// </remarks>
		[Pure]
		public IList<ArraySegment<byte>> ToList()
		{
			this.VerifyIsNotDisposed();

			int resultCount = this._lastUsedSegmentIndex + 1;
			if ( this._lastUsedSegmentIndex == this._buffer.Count )
			{
				Contract.Assert( this._offsetInLastSegment == 0 );
				resultCount--;
			}

			var result = new ArraySegment<byte>[ resultCount ];
			for ( int i = 0; i < this._lastUsedSegmentIndex; i++ )
			{
				result[ i ] = this._buffer[ i ];
			}

			if ( this._lastUsedSegmentIndex < this._buffer.Count
				&& 0 < this._offsetInLastSegment )
			{
				var currentSegment = this._buffer[ this._lastUsedSegmentIndex ];
				result[ result.Length - 1 ] = new ArraySegment<byte>( currentSegment.Array, currentSegment.Offset, this._offsetInLastSegment );
			}

			this.AssertInternalInvariant();
			return result;
		}

		#endregion -- ToList --
	}
}
