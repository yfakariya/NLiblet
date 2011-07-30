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

namespace NLiblet.Text
{
	/// <summary>
	///		Generic formatter for dictionary/map.
	/// </summary>
	internal sealed class DictionaryFormatter<TDictionary, TKey, TValue> : ItemFormatter<TDictionary>
		where TDictionary : IDictionary<TKey,TValue>
	{
		private readonly Action<TKey, FormattingContext> _keyFormatter = GenericItemFormatter<TKey>.Action;
		private readonly Action<TValue, FormattingContext> _valueFormatter = GenericItemFormatter<TValue>.Action;

		public DictionaryFormatter() { }

		public override void FormatTo( TDictionary dictionary, FormattingContext context )
		{
			Debug.WriteLine( "DictionaryFormatter<{0}, {1}>::FormatTo( {2}, {3} )", typeof( TKey ).FullName, typeof( TValue ).FullName, dictionary, context );

			context.Buffer.Append( "{ " );
			context.EnterCollection();

			if ( dictionary != null )
			{
				bool isFirstEntry = true;
				foreach ( var entry in dictionary )
				{
					if ( !isFirstEntry )
					{
						context.Buffer.Append( ", " );
					}

					this._keyFormatter( entry.Key, context );
					context.Buffer.Append( " : " );
					this._valueFormatter( entry.Value, context );
					isFirstEntry = false;
				}
			}

			context.LeaveCollection();
			context.Buffer.Append( " }" );
		}
	}
}
