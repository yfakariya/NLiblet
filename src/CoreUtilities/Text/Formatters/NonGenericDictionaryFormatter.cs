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
using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Formatter for non-generic <see cref="IDictionary"/>
	/// </summary>
	internal sealed class NonGenericDictionaryFormatter : ItemFormatter<IDictionary>
	{
		public static readonly NonGenericDictionaryFormatter Instance = new NonGenericDictionaryFormatter();

		private NonGenericDictionaryFormatter() { { } }

		public sealed override void FormatTo( IDictionary item, FormattingContext context )
		{
			Debug.WriteLine( "NonGenericDictionaryFormatter::FormatTo( {0}, {1} )", item, context );

			FormattingLogics.FormatDictionary(
				item.Cast<DictionaryEntry>(),
				context,
				null,
				( element, context0, _ ) =>
				{
					if ( Object.ReferenceEquals( element.Key, null ) )
					{
						context0.Buffer.Append( FormattingLogics.NullRepresentation );
					}
					else
					{
						ItemFormatter.Get( element.Key.GetType() ).FormatObjectTo( element.Key, context0 );
					}

					context0.Buffer.Append( " : " );

					if ( Object.ReferenceEquals( element.Value, null ) )
					{
						context.Buffer.Append( FormattingLogics.NullRepresentation );
					}
					else
					{
						ItemFormatter.Get( element.Value.GetType() ).FormatObjectTo( element.Value, context0 );
					}
				}
			);
		}
	}
}
