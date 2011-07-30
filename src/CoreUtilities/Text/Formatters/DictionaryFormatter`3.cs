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
using System.Diagnostics;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Generic formatter for dictionary/map.
	/// </summary>
	internal sealed class DictionaryFormatter<TDictionary, TKey, TValue> : ItemFormatter<TDictionary>
		where TDictionary : IDictionary<TKey,TValue>
	{
		private readonly IItemFormatter<TKey> _keyFormatter = ItemFormatter.Get<TKey>();
		private readonly IItemFormatter<TValue> _valueFormatter = ItemFormatter.Get<TValue>();

		public DictionaryFormatter() { }

		public sealed override void FormatTo( TDictionary dictionary, FormattingContext context )
		{
			Debug.WriteLine( "DictionaryFormatter<{0}, {1}>::FormatTo( {2}, {3} )", typeof( TKey ).FullName, typeof( TValue ).FullName, dictionary, context );

			FormattingLogics.FormatDictionary(
				dictionary,
				context,
				this,
				( element, context0, state ) =>
				{
					var @this = state as DictionaryFormatter<TDictionary, TKey, TValue>;
					@this._keyFormatter.FormatTo( element.Key, context0 );
					context0.Buffer.Append( " : " );
					@this._valueFormatter.FormatTo( element.Value, context0 );
				}
			);
		}
	}
}
