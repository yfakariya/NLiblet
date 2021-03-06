﻿#region -- License Terms --
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
using System.Diagnostics;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Formatter for <see cref="DateTime"/>.
	/// </summary>
	internal sealed class DateTimeFormatter : ItemFormatter<DateTime>
	{
		public static readonly DateTimeFormatter Instance = new DateTimeFormatter();

		private DateTimeFormatter() { }

		public sealed override void FormatTo( DateTime item, FormattingContext context )
		{
			Debug.WriteLine( "DateTimeFormatter::FormatTo( {0}, {1} )", item, context );
			
			FormattingLogics.FormatDateTimeTo( item, context );
		}
	}
}
