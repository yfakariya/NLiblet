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
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using System.Threading;

namespace NLiblet.IO
{
	/// <summary>
	///		Wrap <see cref="IEnumerable{T}">IEnumerable</see>&lt;<see cref="Byte"/>&gt; as read-only <see cref="Stream"/>.
	/// </summary>
	public sealed class EnumerableStream : Stream
	{
		private readonly Stream _underlying;

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
		/// <value>
		///		<c>true</c> if the underlying collection is array; otherwise, <c>false</c>.
		/// </value>
		public sealed override bool CanSeek
		{
			get { return this._underlying.CanSeek; }
		}

		/// <summary>
		///		Gets a value indicating whether the current stream supports writing.
		/// </summary>
		/// <value>Always <c>false</c>.</value>
		public sealed override bool CanWrite
		{
			get { return false; }
		}

		/// <summary>
		///		Gets the length in bytes of the stream.
		/// </summary>
		/// <value>
		///		A long value representing the length of the stream in bytes.
		/// </value>
		/// <exception cref="NotSupportedException">
		///		The underlying collection is not array.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		public sealed override long Length
		{
			get { return this._underlying.Length; }
		}

		/// <summary>
		///		Gets the position within the current stream.
		/// </summary>
		/// <value>
		///		The current position within the stream.
		/// </value>
		/// <exception cref="NotSupportedException">
		///		The underlying collection is not array.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///		Resulting position will exceed underlying array length.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		public sealed override long Position
		{
			get { return this._underlying.Position; }
			set { this._underlying.Position = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EnumerableStream"/> class.
		/// </summary>
		/// <param name="source">The source collection.</param>
		public EnumerableStream( IEnumerable<byte> source )
		{
			Contract.Requires<ArgumentNullException>( source != null );

			var asArray = source as byte[];
			if ( asArray != null )
			{
				this._underlying = new MemoryStream( asArray, writable: false );
			}
			else
			{
				this._underlying = new ForwardOnlyEnumerableStream( source );
			}
		}

		/// <summary>
		///		Releases the unmanaged resources used by the <see cref="EnumerableStream"/> class and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		///		<c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
		/// </param>
		protected sealed override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				this._underlying.Dispose();
			}

			base.Dispose( disposing );
		}

		/// <summary>
		///		No operation will be executed.
		/// </summary>
		public sealed override void Flush()
		{
			// nop
		}

		/// <summary>
		///		Reads a sequence of bytes from the underlying stream and advances the position within the stream by the number of bytes read.
		/// </summary>
		/// <param name="buffer">
		///		An array of bytes. 
		///		When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source. 
		///		</param>
		/// <param name="offset">
		///		The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream. 
		///	</param>
		/// <param name="count">
		///		The maximum number of bytes to be read from the current stream. 
		///	</param>
		/// <returns>
		///		The total number of bytes read into the buffer. 
		///		This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached. 
		/// </returns>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		public sealed override int Read( byte[] buffer, int offset, int count )
		{
			return this._underlying.Read( buffer, offset, count );
		}

		/// <summary>
		///		Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.
		/// </summary>
		/// <returns>
		///		The unsigned byte cast to an <see cref="Int32"/>, or -1 if at the end of the stream.
		/// </returns>
		/// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
		public sealed override int ReadByte()
		{
			return this._underlying.ReadByte();
		}

		/// <summary>
		///		Sets the position within the current stream.
		/// </summary>
		/// <param name="offset">A byte offset relative to the origin parameter. </param>
		/// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position. </param>
		/// <returns>The new position within the current stream.</returns>
		/// <exception cref="ArgumentOutOfRangeException">
		///		Resulting position will exceed underlying array length.
		/// </exception>
		/// <exception cref="NotSupportedException">
		///		The stream does not support seeking, because source is not array of byte.
		/// </exception>
		/// <exception cref="ObjectDisposedException">
		///		Methods were called after the stream was closed. 
		/// </exception>
		public sealed override long Seek( long offset, SeekOrigin origin )
		{
			return this._underlying.Seek( offset, origin );
		}

		/// <summary>
		///		Always throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <param name="value">Ignored.</param>
		/// <exception cref="NotSupportedException">Always thrown.</exception>
		public sealed override void SetLength( long value )
		{
			throw new NotSupportedException();
		}

		/// <summary>
		///		Always throws <see cref="NotSupportedException"/>.
		/// </summary>
		/// <param name="buffer">Ignored.</param>
		/// <param name="offset">Ignored.</param>
		/// <param name="count">Ignored.</param>
		/// <exception cref="NotSupportedException">Always thrown.</exception>
		public sealed override void Write( byte[] buffer, int offset, int count )
		{
			throw new NotSupportedException();
		}

		private sealed class ForwardOnlyEnumerableStream : Stream
		{
			private readonly IEnumerator<byte> _iterator;
			private readonly Func<int, byte[], int, int, int> _bulkCopy;
			private int _isDisposed;
			private long _position;

			public sealed override bool CanRead
			{
				get { return true; }
			}

			public sealed override bool CanSeek
			{
				get { return false; }
			}

			public sealed override bool CanWrite
			{
				get { return false; }
			}

			public sealed override long Length
			{
				get { throw new NotSupportedException(); }
			}

			public sealed override long Position
			{
				get
				{
					this.VerifyIsNotDisposed();
					return this._position;
				}
				set { throw new NotSupportedException(); }
			}

			private static readonly Type[] _copyToParameterTypes = new[] { typeof( int ), typeof( byte[] ), typeof( int ), typeof( int ) };

			public ForwardOnlyEnumerableStream( IEnumerable<byte> underlying )
			{
				// Finalization is never needed.
				GC.SuppressFinalize( this );

				this._iterator = underlying.GetEnumerator();
				var asCollection = underlying as ICollection<byte>;
				if ( asCollection != null )
				{
					var copyTo = underlying.GetType().GetMethod( "CopyTo", BindingFlags.Public | BindingFlags.Instance, null, _copyToParameterTypes, null );
					if ( copyTo != null && typeof( void ).TypeHandle.Equals( copyTo.ReturnType.TypeHandle ) )
					{
						var copyToAction = Delegate.CreateDelegate( typeof( Action<int, byte[], int, int> ), underlying, copyTo, true ) as Action<int, byte[], int, int>;
						this._bulkCopy =
							( start, array, offset, count ) =>
							{
								long maximumAllowed = asCollection.Count - start;
								if ( maximumAllowed <= 0 )
								{
									return 0;
								}

								copyToAction( start, array, offset, count );
								return maximumAllowed < count ? unchecked( ( int )maximumAllowed ) : count;
							};
					}
					else if ( copyTo != null && typeof( int ).TypeHandle.Equals( copyTo.ReturnType.TypeHandle ) )
					{
						this._bulkCopy = Delegate.CreateDelegate( typeof( Func<int, byte[], int, int, int> ), underlying, copyTo, true ) as Func<int, byte[], int, int, int>;
					}
					else
					{
						this._bulkCopy = null;
					}
				}
			}

			protected sealed override void Dispose( bool disposing )
			{
				if ( this._isDisposed != 0 )
				{
					return;
				}

				if ( disposing )
				{
					try { }
					finally
					{
						// avoiding ThreadAbortException.
						if ( Interlocked.CompareExchange( ref this._isDisposed, 1, 0 ) == 0 )
						{
							this._iterator.Dispose();
						}
					}
				}

				base.Dispose( disposing );
			}

			public sealed override void Flush()
			{
				this.VerifyIsNotDisposed();
				// nop
			}

			private void VerifyIsNotDisposed()
			{
				if ( this._isDisposed != 0 )
				{
					throw new ObjectDisposedException( typeof( EnumerableStream ).FullName );
				}
			}

			public sealed override int Read( byte[] buffer, int offset, int count )
			{
				this.VerifyIsNotDisposed();

				if ( this._bulkCopy != null && this._position < Int32.MaxValue )
				{
					return this._bulkCopy( unchecked( ( int )this._position ), buffer, offset, count );
				}

				int readCount = 0;
				for ( int readValue = this.ReadByte(); 0 <= readValue; readValue = this.ReadByte() )
				{
					buffer[ offset + readCount ] = unchecked( ( byte )readValue );
					this._position++;
					readCount++;
				}

				return readCount;
			}

			public sealed override int ReadByte()
			{
				this.VerifyIsNotDisposed();

				if ( this._iterator.MoveNext() )
				{
					return this._iterator.Current;
				}
				else
				{
					return -1;
				}
			}

			public sealed override long Seek( long offset, SeekOrigin origin )
			{
				throw new NotSupportedException();
			}

			public sealed override void SetLength( long value )
			{
				throw new NotSupportedException();
			}

			public sealed override void Write( byte[] buffer, int offset, int count )
			{
				throw new NotSupportedException();
			}
		}
	}
}
