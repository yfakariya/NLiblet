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
using System.Diagnostics.Contracts;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

// This class is inspired from CLR via C# and implementation of Mono

namespace NLiblet
{
	// Although value type is lightweight, but it is tend to be used uncorrectly, so I beleive that GC does good job.

	/// <summary>
	/// Represents typed 'weak' reference.
	/// </summary>
	/// <typeparam name="T">Type of reference type to be wrapped.</typeparam>
	public sealed class WeakReference<T> : IDisposable, ISerializable
		where T : class
	{
		private const string _isTruckingResurrectionField = "TrackResurrection";
		private const string _gcHandleField = "TrackedObject";

		private readonly bool _isTruckingResurrection;


		/// <summary>
		///		Gets a value indicating whether this reference tracking resurrection.
		/// </summary>
		/// <value>
		///   <c>true</c> if this reference tracking resurrection; otherwise, <c>false</c>.
		/// </value>
		public bool TrackResurrection
		{
			get { return this._isTruckingResurrection; }
		}

		// not readonly due to disposing.
		private GCHandle _gcHandle;


		/// <summary>
		///		Gets the target object as strong reference.
		/// </summary>
		/// <value>
		///		Wrapped target value. This value may not be <c>null</c>.
		/// </value>
		/// <exception cref="ObjectDisposedException">
		///		<see cref="IsAlive"/> is <c>false</c>, thus target object has been already reclaimed by GC.
		/// </exception>
		/// <remarks>
		///		It is possible target object has been reclaimed since most recent <see cref="IsAlive"/> call
		///		because of the nature of multi-threaded environment.
		/// </remarks>
		public T Target
		{
			get
			{
				if ( !this._gcHandle.IsAllocated )
				{
					throw new ObjectDisposedException( this.ToString() );
				}

				try
				{
					return this._gcHandle.Target as T;
				}
				catch ( InvalidOperationException )
				{
					throw new ObjectDisposedException( this.ToString() );
				}
			}
		}

		/// <summary>
		///		Gets a value indicating whether <see cref="Target"/> instance is alive.
		/// </summary>
		/// <value>
		///   <c>true</c> if this <see cref="Target"/> instance is alive; otherwise, <c>false</c>.
		/// </value>
		public bool IsAlive
		{
			get { return this._gcHandle.IsAllocated; }
		}

		/// <summary>
		///		Initializes a new instance without resurrection tracking.
		/// </summary>
		/// <param name="target">The target. This value is non-<c>null</c> reference type object.</param>
		[ReliabilityContract( Consistency.MayCorruptInstance, Cer.MayFail )]
		public WeakReference( T target )
			: this( target, false )
		{
			// contract only
			Contract.Requires<ArgumentNullException>( target != null );
		}

		/// <summary>
		///		Initializes a new instance with specified resurrection tracking.
		/// </summary>
		/// <param name="target">The target. This value is non-<c>null</c> reference type object.</param>
		/// <param name="trackResurrection">If tracking resurrection then <c>true</c>; otherwise <c>false</c>.</param>
		[ReliabilityContract( Consistency.MayCorruptInstance, Cer.MayFail )]
		public WeakReference( T target, bool trackResurrection )
		{
			Contract.Requires<ArgumentNullException>( target != null );

			this._isTruckingResurrection = trackResurrection;

			// Just avoiding ThreadAbortException since GCHandle.Alloc might allocate resources so CER cannot be used.
			try { }
			finally
			{
				this._gcHandle = GCHandle.Alloc( target, ( trackResurrection ? GCHandleType.WeakTrackResurrection : GCHandleType.Weak ) );
			}
		}

		private WeakReference( SerializationInfo info, StreamingContext context )
		{
			Contract.Requires<ArgumentNullException>( info != null );

			this._isTruckingResurrection = info.GetBoolean( _isTruckingResurrectionField );
			var target = info.GetValue( _gcHandleField, typeof( System.Object ) );
			try { }
			finally
			{
				this._gcHandle = GCHandle.Alloc( target, ( this._isTruckingResurrection ? GCHandleType.WeakTrackResurrection : GCHandleType.Weak ) );
			}
		}

		/// <summary>
		///		Releases unmanaged resources and performs other cleanup operations before the
		///		<see cref="WeakReference&lt;T&gt;"/> is reclaimed by garbage collection.
		/// </summary>
		~WeakReference()
		{
			this.Dispose( false );
		}

		/// <summary>
		///		Releases unmanaged resources and performs other cleanup operations before the
		///		<see cref="WeakReference&lt;T&gt;"/> is reclaimed by garbage collection.
		/// </summary>
		public void Dispose()
		{
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		private void Dispose( bool disposing )
		{
			if ( !this._gcHandle.IsAllocated )
			{
				return;
			}

			try
			{
				this._gcHandle.Free();
			}
			catch ( InvalidOperationException ) { }
		}

		void ISerializable.GetObjectData( SerializationInfo info, StreamingContext context )
		{
			info.AddValue( _isTruckingResurrectionField, this._isTruckingResurrection );

			object target = null;
			try
			{
				target = this._gcHandle.Target;
			}
			catch ( InvalidOperationException ) { }

			info.AddValue( _gcHandleField, target );
		}
	}
}