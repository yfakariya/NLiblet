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
	/// <summary>
	///		Defines utility methods to <see cref="IProgress{T}"/>.
	/// </summary>
	public static class Progress
	{
		/// <summary>
		///		Get the empty instance of <see cref="IProgress{T}"/>.
		/// </summary>
		/// <typeparam name="T">Type of the progress value the producer requested.</typeparam>
		/// <returns>The empty instance of <see cref="IProgress{T}"/>.</returns>
		/// <remarks>
		///		Empty instance is often referred as 'null object', which is actually NOT <c>null</c>,
		///		but it does not any action. 
		///		Teherefore, client safely pass the empty instance to the async methods which requests <see cref="IProgress{T}"/>
		///		without warring about <see cref="NullReferenceException"/>.
		/// </remarks>
		public static IProgress<T> Empty<T>()
		{
			return NullProgress<T>.Instance;
		}
	}
}
