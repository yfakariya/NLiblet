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
using System.Runtime.Serialization;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Formatter for <see cref="SerializationInfo"/>.
	/// </summary>
	internal sealed class SerializationInfoFormatter : ItemFormatter<SerializationInfo>
	{
		public static readonly SerializationInfoFormatter Instance = new SerializationInfoFormatter();

		private SerializationInfoFormatter() { }

		public sealed override void FormatTo( SerializationInfo item, FormattingContext context )
		{
			Debug.WriteLine( "SerializationInfoFormatter::FormatTo( {0}, {1} )", item, context );

			FormattingLogics.FormatDictionary<SerializationEntry>(
				Enumerate( item ),
				context,
				null,
				( element, context0, _ ) =>
				{
					context0.Buffer.Append( '"' ).Append( element.Name ).Append( "\" : " );

					if ( Object.ReferenceEquals( element.Value, null ) )
					{
						FormattingLogics.FormatToNull( context0 );
					}
					else
					{
						ItemFormatter.Get( element.Value.GetType() ).FormatObjectTo( element.Value, context0 );
					}
				}
			);
		}

		// SerializationInfo is NOT IEnumerable...
		private static IEnumerable<SerializationEntry> Enumerate( SerializationInfo info )
		{
			foreach ( SerializationEntry entry in info )
			{
				yield return entry;
			}
		}
	}
}
