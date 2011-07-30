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
using System.Linq;
using System.Text;
using System.Diagnostics;

using NLiblet.Reflection;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Formatter for numerics which implements <see cref="IFormattable"/>.
	/// </summary>
	internal sealed class FormattableNumericsFormatter<TFormattable> : ItemFormatter<TFormattable>
		where TFormattable : IFormattable
	{
		public static readonly FormattableNumericsFormatter<TFormattable> Instance = new FormattableNumericsFormatter<TFormattable>();

		private FormattableNumericsFormatter() { }

		public sealed override void FormatTo( TFormattable item, FormattingContext context )
		{
			Debug.WriteLine( "FormattableNumericsFormatter<{0}>::FormatTo( {1}, {2} )", typeof( TFormattable ).GetFullName(), item, context );

			if ( context.IsInCollection )
			{
				context.Buffer.Append( item );
			}
			else
			{
				context.Buffer.Append( item.ToString( context.Format, context.FallbackProvider ) );
			}
		}
	}
}
