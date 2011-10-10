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
using System.Diagnostics.Contracts;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NLiblet.Async
{
	/// <summary>
	///		Provide async stream copying with progress reporting.
	/// </summary>
	public sealed class AsyncStream
	{
		private readonly byte[] _buffer;
		private readonly Stream _baseStream;
		private readonly CancellationToken _cancellationToken;
		private readonly IProgress<StreamOperationProgressValue> _progress;

		/// <summary>
		///		Initializes a new instance of the <see cref="AsyncStream"/> class.
		/// </summary>
		/// <param name="baseStream">The base stream which will be copy source.</param>
		/// <param name="buffer">The buffer to be used on copying.</param>
		/// <param name="cancellationToken">The <see cref="CancellationToken"/> to coordinate cancellation.</param>
		/// <param name="progress">The <see cref="IProgress{T}"/> to report progress.</param>
		public AsyncStream( Stream baseStream, byte[] buffer, CancellationToken cancellationToken, IProgress<StreamOperationProgressValue> progress )
		{
			Contract.Requires<ArgumentNullException>( baseStream != null, "baseStream" );
			Contract.Requires<ArgumentException>( baseStream.CanRead );
			Contract.Requires<ArgumentNullException>( buffer != null, "buffer" );

			this._baseStream = baseStream;
			this._buffer = buffer;
			this._cancellationToken = cancellationToken;
			this._progress = progress ?? Progress.Empty<StreamOperationProgressValue>();
		}

		/// <summary>
		///		Start copying from wrapped <see cref="Stream"/> to specified <see cref="Stream"/>.
		/// </summary>
		/// <param name="destination">The destination <see cref="Stream"/>.</param>
		/// <returns>
		///		<see cref="Task"/> to represent async copying operation.
		/// </returns>
		public Task CopyTo( Stream destination )
		{
			Contract.Requires<ArgumentNullException>( destination != null, "destination" );
			Contract.Requires<ArgumentException>( destination.CanWrite );
			Contract.Ensures( Contract.Result<Task>() != null );

			var taskComplectionSource = new TaskCompletionSource<object>();

			if ( this._cancellationToken.IsCancellationRequested )
			{
				taskComplectionSource.SetCanceled();
			}
			else
			{
				this._baseStream.BeginRead(
					this._buffer,
					0,
					this._buffer.Length,
					this.AsyncCopyToReadCallback,
					new AsyncCopyState( destination, taskComplectionSource )
				);
			}

			return taskComplectionSource.Task;
		}

		[SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfered via TaskCompletionSource.SetException" )]
		private void AsyncCopyToReadCallback( IAsyncResult asyncResult )
		{
			var state = asyncResult.AsyncState as AsyncCopyState;
			try
			{
				int read = this._baseStream.EndRead( asyncResult );

				if ( this._cancellationToken.IsCancellationRequested )
				{
					state.TaskComplectionSource.SetCanceled();
					return;
				}

				state.CurrentlyProcessing = read;

				if ( read == this._buffer.Length )
				{
					state.Destination.BeginWrite( this._buffer, 0, read, this.AsyncCopyToWriteCallback, state );
				}
				else
				{
					state.Destination.BeginWrite( this._buffer, 0, read, this.AsyncCopyToCompletedCallback, state );
				}
			}
			catch ( Exception ex )
			{
				state.TaskComplectionSource.SetException( ex );
			}
		}

		[SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfered via TaskCompletionSource.SetException" )]
		private void AsyncCopyToWriteCallback( IAsyncResult asyncResult )
		{
			var state = asyncResult.AsyncState as AsyncCopyState;
			try
			{
				state.Destination.EndWrite( asyncResult );
				state.ReportProgress( this._progress );

				if ( this._cancellationToken.IsCancellationRequested )
				{
					state.TaskComplectionSource.SetCanceled();
					return;
				}

				this._baseStream.BeginRead( this._buffer, 0, this._buffer.Length, this.AsyncCopyToReadCallback, state );
			}
			catch ( Exception ex )
			{
				state.TaskComplectionSource.SetException( ex );
			}
		}

		[SuppressMessage( "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Transfered via TaskCompletionSource.SetException" )]
		private void AsyncCopyToCompletedCallback( IAsyncResult asyncResult )
		{
			var state = asyncResult.AsyncState as AsyncCopyState;
			try
			{
				state.Destination.EndWrite( asyncResult );
				state.ReportProgress( this._progress );

				state.TaskComplectionSource.SetResult( null );
			}
			catch ( Exception ex )
			{
				state.TaskComplectionSource.SetException( ex );
			}
		}

		private sealed class AsyncCopyState
		{
			public readonly Stream Destination;
			public readonly TaskCompletionSource<object> TaskComplectionSource;
			public int CurrentlyProcessing;
			public long TotallyProcessed;

			public AsyncCopyState( Stream destination, TaskCompletionSource<object> taskComplectionSource)
			{
				this.Destination = destination;
				this.TaskComplectionSource = taskComplectionSource;
			}

			public void ReportProgress( IProgress<StreamOperationProgressValue> progress )
			{
				Contract.Assert( progress != null );

				this.TotallyProcessed += CurrentlyProcessing;
				progress.Report( new StreamOperationProgressValue( this.Destination, this.CurrentlyProcessing, this.TotallyProcessed ) );
				this.CurrentlyProcessing = 0;
			}
		}
	}
}
