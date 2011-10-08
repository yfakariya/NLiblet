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

namespace NLiblet.Async
{
	/// <summary>
	///		Implements user specific delegate based <see cref="IProgress{T}"/>.
	/// </summary>
	/// <typeparam name="T">Type of value of progress reporting.</typeparam>
	internal sealed class CustomNestedProgress<T> : IProgress<T>
	{
		private readonly T _factor;
		private readonly Func<T, T, T> _adjuster;
		private readonly IProgress<T> _parent;

		public CustomNestedProgress( IProgress<T> parent, T factor, Func<T, T, T> adjuster )
		{
			Contract.Requires( parent != null, "parent" );
			Contract.Requires( adjuster != null, "adjuster" );

			this._parent = parent;
			this._factor = factor;
			this._adjuster = adjuster;
		}

		public void Report( T value )
		{
			this._parent.Report( this._adjuster( value, this._factor ) );
		}
	}
}
