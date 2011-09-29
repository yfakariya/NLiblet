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

using NLiblet.Reflection;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Formatter for nullable types.
	/// </summary>
	/// <typeparam name="T">Underlying value type.</typeparam>
	internal sealed class NullableFormatter<T> : ItemFormatter<Nullable<T>>
		where T : struct
	{
		private readonly IItemFormatter<T> _valueFormatter;

		public NullableFormatter()
		{
			this._valueFormatter = ItemFormatter.Get<T>();
		}

		public sealed override void FormatTo( T? item, FormattingContext context )
		{
			Debug.WriteLine( "NullableFormatter<{0}>::( {1}, {2} )", typeof( T ).GetFullName(), item, context );

			if ( !item.HasValue )
			{
				FormattingLogics.FormatToNull( context );
			}
			else
			{
				this._valueFormatter.FormatTo( item.Value, context );
			}
		}
	}
}
