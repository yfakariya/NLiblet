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
using System.Diagnostics;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Formatter for bool.
	///		This class convert boolean to lower case string.
	/// </summary>
	internal sealed class BooleanFormatter : ItemFormatter<bool>
	{
		public static readonly BooleanFormatter Instance = new BooleanFormatter();

		private BooleanFormatter() { }

		public sealed override void FormatTo( bool item, FormattingContext context )
		{
			Debug.WriteLine( "BooleanFormatter::FormatTo( {0}, {1} )", item, context );
			context.Buffer.Append( item ? "true" : "false" );
		}
	}
}
