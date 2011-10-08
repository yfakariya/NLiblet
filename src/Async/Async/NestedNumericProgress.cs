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
	///		Floating point based <see cref="IProgress{T}"/> implementation.
	/// </summary>
	internal sealed class NestedNumericProgress : IProgress<double>
	{
		private readonly double _factor;
		private readonly IProgress<double> _parent;

		public NestedNumericProgress( IProgress<double> parent, double factor )
		{
			Contract.Requires( parent != null );

			this._parent = parent;
			this._factor = factor;
		}

		public void Report( double value )
		{
			this._parent.Report( value * this._factor );
		}
	}
}
