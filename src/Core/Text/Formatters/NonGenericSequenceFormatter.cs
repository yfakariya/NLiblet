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
using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Formatter for non-generic <see cref="IEnumerable"/>
	/// </summary>
	internal sealed class NonGenericSequenceFormatter : ItemFormatter<IEnumerable>
	{
		public static readonly NonGenericSequenceFormatter Instance = new NonGenericSequenceFormatter();

		private NonGenericSequenceFormatter() { }

		public sealed override void FormatTo( IEnumerable item, FormattingContext context )
		{
			Debug.WriteLine( "NonGenericSequenceFormatter::FormatTo( {0}, {1} )", item, context );

			FormattingLogics.FormatSequence(
				item.Cast<object>(),
				context,
				null,
				( element, context0, _ ) =>
				{
					if ( Object.ReferenceEquals( element, null ) )
					{
						FormattingLogics.FormatToNull( context0 );
					}
					else
					{
						ItemFormatter.Get( element.GetType() ).FormatObjectTo( element, context0 );
					}

				}
			);
		}
	}
}
