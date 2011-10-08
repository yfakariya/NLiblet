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
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NLiblet.Async
{
	[TestFixture]
	public class AsyncStreamTest
	{
		[Test]
		public void TestCopyTo()
		{
			var oldContext = SynchronizationContext.Current;
			try
			{
				SynchronizationContext.SetSynchronizationContext( new SynchrnousSynchronizationContext() );
				var text = String.Join( String.Empty, Enumerable.Repeat( ( Func<Guid> )Guid.NewGuid, 10 ).Select( func => func().ToString() ) );
				var tempFile = Path.GetTempFileName();
				try
				{
					int sum = 0;
					var progress =
						new Progress<StreamOperationProgressValue>(
							item => sum += item.CurrentlyProcessed // Due to SynchrnousSynchronizationContext, this call back occurs on current thread.
							);
					File.WriteAllText( tempFile, text );
					using ( var fileStream = new FileStream( tempFile, FileMode.Open, FileAccess.Read, FileShare.Read, 8, FileOptions.Asynchronous ) )
					using ( var memoryStream = new MemoryStream() )
					{
						var target = new AsyncStream( fileStream, new byte[ 8 ], CancellationToken.None, progress );
						var task = target.CopyTo( memoryStream );
						task.Wait();
						// There are no ways to wait progress event definitely...
						Assert.That( Encoding.UTF8.GetString( memoryStream.ToArray() ), Is.EqualTo( text ) );
						Assert.That( sum, Is.EqualTo( fileStream.Length ) );
					}
				}
				finally
				{
					File.Delete( tempFile );
				}
			}
			finally
			{
				SynchronizationContext.SetSynchronizationContext( oldContext );
			}
		}

		[Test]
		public void TestCopyToFailToRead()
		{
			var text = String.Join( String.Empty, Enumerable.Repeat( ( Func<Guid> )Guid.NewGuid, 10 ).Select( func => func().ToString() ) );
			var tempFile = Path.GetTempFileName();
			try
			{
				int sum = 0;
				var progress =
					new Progress<StreamOperationProgressValue>(
						item => sum += item.CurrentlyProcessed
						);
				File.WriteAllText( tempFile, text );
				using ( var fileStream = new FileStream( tempFile, FileMode.Open, FileAccess.Read, FileShare.Read, 8, FileOptions.Asynchronous ) )
				using ( var memoryStream = new MemoryStream( new byte[ 8 ] ) ) // not extendable
				{
					var target = new AsyncStream( fileStream, new byte[ 8 ], CancellationToken.None, progress );
					var task = target.CopyTo( memoryStream );
					try
					{
						task.Wait();
						Assert.Fail();
					}
					catch ( AggregateException ex )
					{
						Assert.That( ex.InnerException is NotSupportedException, ex.ToString() );
					}
				}
			}
			finally
			{
				File.Delete( tempFile );
			}
		}

		[Test]
		public void TestCopyToFailCancelled()
		{
			var text = String.Join( String.Empty, Enumerable.Repeat( ( Func<Guid> )Guid.NewGuid, 10 ).Select( func => func().ToString() ) );
			var tempFile = Path.GetTempFileName();
			try
			{
				var cts = new CancellationTokenSource();
				using ( var waitHandle = new ManualResetEventSlim() )
				{
					var progress =
						new Progress<StreamOperationProgressValue>(
							item => waitHandle.Wait()
							);
					File.WriteAllText( tempFile, text );
					using ( var fileStream = new FileStream( tempFile, FileMode.Open, FileAccess.Read, FileShare.Read, 8, FileOptions.Asynchronous ) )
					using ( var memoryStream = new MemoryStream() )
					{
						var target = new AsyncStream( fileStream, new byte[ 8 ], cts.Token, progress );
						var task = target.CopyTo( memoryStream );
						try
						{
							cts.Cancel();
							waitHandle.Set();
							task.Wait();
							Assert.Fail();
						}
						catch ( AggregateException ex )
						{
							Assert.That( ex.InnerException is TaskCanceledException || ex.InnerException is OperationCanceledException, ex.ToString() );
						}
					}
				}
			}
			finally
			{
				File.Delete( tempFile );
			}
		}

		private sealed class SynchrnousSynchronizationContext : SynchronizationContext
		{
			public SynchrnousSynchronizationContext() { }

			public override void Post( SendOrPostCallback d, object state )
			{
				d( state );
			}

			public override void Send( SendOrPostCallback d, object state )
			{
				d( state );
			}
		}
	}
}
