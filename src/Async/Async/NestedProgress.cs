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
using System.Globalization;
using NLiblet.Properties;

namespace NLiblet.Async
{
	/// <summary>
	///		Support nested progress reporting.
	/// </summary>
	public static class NestedProgress
	{
		/// <summary>
		///		Create nested <see cref="IProgress{T}"/> for numeric progress reporting.
		/// </summary>
		/// <param name="parent">The parent <see cref="IProgress{T}"/>.</param>
		/// <param name="factor">The factor to apply to the value of nested operation progress reporting.</param>
		/// <returns>
		///		<see cref="IProgress{T}"/> to report nested operation.
		///	</returns>
		public static IProgress<double> Numeric( IProgress<double> parent, double factor )
		{
			Contract.Requires<ArgumentNullException>( parent != null, "parent" );
			Contract.Ensures( Contract.Result<IProgress<double>>() != null );

			return new NestedNumericProgress( parent, factor );
		}

		/// <summary>
		///		Create nested <see cref="IProgress{T}"/> for progress reporting.
		/// </summary>
		/// <typeparam name="T">
		///		The type of the value of progress reporting.
		///	</typeparam>
		/// <param name="parent">The parent <see cref="IProgress{T}"/>.</param>
		/// <param name="factor">The factor to apply to the value of nested operation progress reporting.</param>
		/// <returns><see cref="IProgress{T}"/> to report nested operation.</returns>
		///	<exception cref="InvalidOperationException">
		///		<typeparamref name="T"/> does not declare multiply operator.
		///	</exception>
		///	<remarks>
		///		<typeparamref name="T"/> must declare method which matches to any of following signature (and former is preferred).
		///		<list type="number">
		///			<item>
		///				<c>public static <typeparamref name="T"/> op_Multiply( <typeparamref name="T"/> left, <typeparamref name="T"/> right )</c>
		///				with the 'specialname' attribute.
		///				<note>
		///					The name of parameters are ignored.
		///				</note>
		///				<note>
		///					It is operator method of some language. 
		///				</note>
		///			</item>
		///			<item>
		///				<c>public static <typeparamref name="T"/> Multiply( <typeparamref name="T"/> left, <typeparamref name="T"/> right )</c>
		///				<note>
		///					The name of parameters are ignored.
		///				</note>
		///			</item>
		///			<item>
		///				<c>public static <typeparamref name="T"/> Multiply( <typeparamref name="T"/> value )</c>
		///				<note>
		///					The name of parameters are ignored.
		///				</note>
		///			</item>
		///		</list>
		///	</remarks>
		public static IProgress<T> Of<T>( IProgress<T> parent, T factor )
		{
			Contract.Requires<ArgumentNullException>( parent != null, "parent" );
			Contract.Ensures( Contract.Result<IProgress<T>>() != null );

			if ( !DefaultNestedProgress<T>.CanMultily )
			{
				throw new InvalidOperationException(
					String.Format( CultureInfo.CurrentCulture, Resources.TypeDoesNotDeclareMultiplyOperator, typeof( T ) )
				);
			}

			return new DefaultNestedProgress<T>( parent, factor );
		}

		/// <summary>
		///		Create nested <see cref="IProgress{T}"/> for progress reporting.
		/// </summary>
		/// <typeparam name="T">
		///		The type of the value of progress reporting.
		///	</typeparam>
		/// <param name="parent">The parent <see cref="IProgress{T}"/>.</param>
		/// <param name="factor">The factor to apply to the value of nested operation progress reporting.</param>
		/// <param name="adjuster">The delegate to multiply <paramref name="factor"/> and the value.</param>
		/// <returns><see cref="IProgress{T}"/> to report nested operation.</returns>
		public static IProgress<T> Of<T>( IProgress<T> parent, T factor, Func<T, T, T> adjuster )
		{
			Contract.Requires<ArgumentNullException>( parent != null, "parent" );
			Contract.Requires<ArgumentNullException>( adjuster != null, "adjuster" );
			Contract.Ensures( Contract.Result<IProgress<T>>() != null );

			return new CustomNestedProgress<T>( parent, factor, adjuster );
		}
	}
}
