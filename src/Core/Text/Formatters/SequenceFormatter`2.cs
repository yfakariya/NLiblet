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
	///		Formatter for generic sequence.
	/// </summary>
	internal sealed class SequenceFormatter<TCollection, TItem> : ItemFormatter<TCollection>
		where TCollection : IEnumerable<TItem>
	{
		private readonly IItemFormatter<TItem> _itemFormatter;

		public SequenceFormatter()
		{
			this._itemFormatter = ItemFormatter.Get<TItem>();
		}

		public override void FormatTo( TCollection sequence, FormattingContext context )
		{
			Debug.WriteLine( "SequenceFormatter<{0}>::FormatTo( {1}, {2} )", typeof( TItem ).FullName, sequence, context );

			FormattingLogics.FormatSequence(
				sequence,
				context,
				this._itemFormatter,
				( element, context0, state ) => ( state as IItemFormatter<TItem> ).FormatTo( element, context0 )
			);
		}
	}
}
