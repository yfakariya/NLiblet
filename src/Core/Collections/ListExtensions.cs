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
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;

namespace NLiblet.Collections
{
#warning IMPL
	/// <summary>
	///		Defines extension methods for <see cref="IList{T}"/>.
	/// </summary>
	public static class ListExtensions
	{
		/// <summary>
		///		Enumerate specified data source with sequence number.
		/// </summary>
		/// <typeparam name="T">Type of the elements.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>
		///		The collection of <see cref="SequenceItem{T}"/> which contains the sequence number and the source item.
		/// </returns>
		/// <remarks>
		///		The sequence number is zero-based <see cref="Int64"/> value.
		/// </remarks>
		[SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Generic type collection." )]
		public static IEnumerable<SequenceItem<T>> WithSequenceNumber<T>( this IEnumerable<T> source )
		{
			Contract.Requires<ArgumentNullException>( source != null );

			long sequenceNumber = 0L;
			foreach ( var item in source )
			{
				yield return new SequenceItem<T>( sequenceNumber, item );
				sequenceNumber++;
			}
		}

		//public static T Last<T>( this IList<T> source )
		//{
		//    Contract.Requires<ArgumentNullException>( source != null );

		//    if ( source.Count == 0 )
		//    {
		//        throw new InvalidOperationException( "List is empty." );
		//    }

		//    return source[ source.Count - 1 ];
		//}

		//public static T LastOrDefault<T>( this IList<T> source )
		//{
		//    Contract.Requires<ArgumentNullException>( source != null );

		//    if ( source.Count == 0 )
		//    {
		//        return default( T );
		//    }

		//    return source[ source.Count - 1 ];
		//}

		//public static void SortWith<T, TComparer>( this IList<T> source, Action<IList<T>, DelegateComparer<T>> algorithm, Func<T, T, int> comparison )
		//{
		//    SortWith( source, algorithm, new DelegateComparer<T>( comparison ) );
		//}

		//public static void SortWith<T, TComparer>( this IList<T> source, Action<IList<T>, TComparer> algorithm, TComparer comparer )
		//    where TComparer : IComparer<T>
		//{
		//    Contract.Requires<ArgumentNullException>( source != null );
		//    Contract.Requires<ArgumentNullException>( algorithm != null );
		//    Contract.Requires<ArgumentNullException>( comparer != null );

		//    algorithm( source, comparer );
		//}
	}

	//public static class SortAlgorithms
	//{
	//    public static Action<IList<T>, TComparer> QuickSort<T, TComparer>()
	//        where TComparer : IComparer<T>
	//    {
	//        return DoQuickSort<T, TComparer>;
	//    }

	//    public static void DoQuickSort<T, TComparer>( IList<T> target, TComparer comparer )
	//        where TComparer : IComparer<T>
	//    {
	//        throw new NotImplementedException();
	//    }

	//    public static Action<IList<T>, TComparer> MergeSort<T, TComparer>()
	//        where TComparer : IComparer<T>
	//    {
	//        return DoMergeSort<T, TComparer>;
	//    }

	//    public static void DoMergeSort<T, TComparer>( IList<T> target, TComparer comparer )
	//        where TComparer : IComparer<T>
	//    {
	//        throw new NotImplementedException();
	//    }
	//}

	//// TODO: T4
	//public struct DelegateComparer<T> : IComparer<T>
	//{
	//    private readonly Func<T, T, int> _comparison;

	//    public static DelegateComparer<T> ForComparison( Comparison<T> comparison )
	//    {
	//        return new DelegateComparer<T>( ( x, y ) => comparison( x, y ) );
	//    }

	//    public DelegateComparer( Func<T, T, int> comparison )
	//    {
	//        this._comparison = comparison;
	//    }

	//    public int Compare( T x, T y )
	//    {
	//        return this._comparison( x, y );
	//    }
	//}
}
