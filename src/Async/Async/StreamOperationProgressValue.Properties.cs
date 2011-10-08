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

namespace NLiblet.Async
{
	partial struct StreamOperationProgressValue
	{
		/// <summary>
		///		Gets the current position of underlying stream.
		/// </summary>
		/// <value>
		///		The current position of underlying stream.
		///		If underlying stream is <c>null</c> or its <see cref="System.IO.Stream.CanSeek"/> is <c>false</c> then <c>-1</c>.
		/// </value>
		public long Position
		{
			get { return ( this.Stream == null || this.Stream.CanSeek ) ? -1 : this.Stream.Position; }
		}

		/// <summary>
		///		Gets the length of underlying stream.
		/// </summary>
		/// <value>
		///		The length of underlying stream.
		///		If underlying stream is <c>null</c> or its <see cref="System.IO.Stream.CanSeek"/> is <c>false</c> then <c>-1</c>.
		/// </value>
		public long Length
		{
			get { return ( this.Stream == null || this.Stream.CanSeek ) ? -1 : this.Stream.Length; }
		}
	}
}
