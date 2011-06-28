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

namespace NLiblet.Text
{
	/// <summary>
	///		Defines NLiblet <see cref="IFormatProvider"/>.
	/// </summary>
	/// <include file='Remarks.xml' path='doc/NLiblet.Text/FormatProviders/remarks'/>
	public static class FormatProviders
	{
		[ThreadStatic]
		private static CommonCustomFormatter _currentCulture;

		/// <summary>
		///		Get <see cref="IFormatProvider"/> bounds to <see cref="Thread.CurrentCulture"/>.
		/// </summary>
		/// <value><see cref="IFormatProvider"/> bounds to <see cref="Thread.CurrentCulture"/>.</value>
		public static IFormatProvider CurrentCulture
		{
			get
			{
				Contract.Ensures( Contract.Result<IFormatProvider>() != null );

				if ( !CultureInfo.CurrentCulture.Equals( _currentCulture.DefaultFormatProvider ) )
				{
					_currentCulture = new CommonCustomFormatter( CultureInfo.CurrentCulture );
				}

				return _currentCulture;
			}
		}

		private static readonly CommonCustomFormatter _invariantCulture = new CommonCustomFormatter( CultureInfo.InvariantCulture );

		/// <summary>
		///		Get <see cref="IFormatProvider"/> bounds to <see cref="CultureInfo.InvariantCulture"/>.
		/// </summary>
		/// <value><see cref="IFormatProvider"/> bounds to <see cref="CultureInfo.InvariantCulture"/>.</value>
		public static IFormatProvider InvariantCulture
		{
			get
			{
				Contract.Ensures( Contract.Result<IFormatProvider>() != null );
				return _invariantCulture;
			}
		}
	}
}
