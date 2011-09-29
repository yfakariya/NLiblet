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
using System.Linq;

namespace NLiblet.Collections
{
	/// <summary>
	///		Extension methods to use <see cref="ArraySegment{T}"/> more easily.
	/// </summary>
	public static class ArraySegmentExtensions
	{
		/// <summary>
		///		Get <see cref="IEnumerable{T}"/> to enumerent items in the segment.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		/// <param name="source"><see cref="ArraySegment{T}"/></param>
		/// <returns>
		///		<see cref="IEnumerable{T}"/> to enumerent items in the segment.
		///		If the segment of <paramref name="source"/> is empty or is not available then empty.
		/// </returns>
		[Pure]
		public static IEnumerable<T> AsEnumerable<T>( this ArraySegment<T> source )
		{
			Contract.Ensures( Contract.Result<IEnumerable<T>>() != null );

			if ( source.Array == null
				|| source.Offset < 0
				|| source.Array.Length <= source.Offset
				|| source.Count <= 0
				|| ( source.Array.Length - source.Offset ) < source.Count
			)
			{
				return Empty.Array<T>();
			}
			else
			{
				return source.Array.Skip( source.Offset ).Take( source.Count );
			}
		}

		/// <summary>
		///		Get item of <see cref="ArraySegment{T}"/> at specified <paramref name="relativeIndex"/>.
		/// </summary>
		/// <typeparam name="T">Type of item of <see cref="ArraySegment{T}"/>.</typeparam>
		/// <param name="source"><see cref="ArraySegment{T}"/>.</param>
		/// <param name="relativeIndex">
		///		Relative index of the item from <see cref="ArraySegment{T}.Offset"/>.
		/// </param>
		/// <returns>
		///		Item at <paramref name="relativeIndex"/>,
		///		thus item at <see cref="ArraySegment{T}.Offset"/> + <paramref name="relativeIndex"/>.
		/// </returns>
		public static T GetItemAt<T>( this ArraySegment<T> source, int relativeIndex )
		{
			Contract.Requires<ArgumentException>( source.Array != null );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= relativeIndex );
			Contract.Requires<ArgumentOutOfRangeException>( relativeIndex < source.Count );

			return source.Array[ source.Offset + relativeIndex ];
		}

		/// <summary>
		///		Set item of <see cref="ArraySegment{T}"/> at specified <paramref name="relativeIndex"/>.
		/// </summary>
		/// <typeparam name="T">Type of item of <see cref="ArraySegment{T}"/>.</typeparam>
		/// <param name="source"><see cref="ArraySegment{T}"/>.</param>
		/// <param name="relativeIndex">
		///		Relative index of the item from <see cref="ArraySegment{T}.Offset"/>.
		/// </param>
		/// <param name="value">
		///		Value to be set at <paramref name="relativeIndex"/>,
		///		thus item at <see cref="ArraySegment{T}.Offset"/> + <paramref name="relativeIndex"/>.
		/// </param>
		public static void SetItemAt<T>( this ArraySegment<T> source, int relativeIndex, T value )
		{
			Contract.Requires<ArgumentException>( source.Array != null );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= relativeIndex );
			Contract.Requires<ArgumentOutOfRangeException>( relativeIndex < source.Count );

			source.Array[ source.Offset + relativeIndex ] = value;
		}
	}
}
