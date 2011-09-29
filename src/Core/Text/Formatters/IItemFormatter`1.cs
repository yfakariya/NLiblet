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

namespace NLiblet.Text.Formatters
{
	// Contra-variance is necessary for StringFormatter, BytesFormatter etc. to support polymorphism.
	/// <summary>
	///		Define strongly typed interface of item formatter with contravariance.
	/// </summary>
	/// <typeparam name="T">Type of item.</typeparam>
	internal interface IItemFormatter<in T>
	{
		void FormatTo( T item, FormattingContext context );
	}
}
