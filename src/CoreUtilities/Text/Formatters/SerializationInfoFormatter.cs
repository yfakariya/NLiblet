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
using System.Runtime.Serialization;
using System.Diagnostics;

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

			context.Buffer.Append( '{' );
			context.EnterCollection();

			bool isFirstEntry = true;
			foreach ( SerializationEntry entry in ( item as SerializationInfo ) )
			{
				if ( !isFirstEntry )
				{
					context.Buffer.Append( ", " );
				}
				else
				{
					context.Buffer.Append( ' ' );
				}

				context.Buffer.Append( '"' ).Append( entry.Name ).Append( "\" : " );

				if ( Object.ReferenceEquals( entry.Value, null ) )
				{
					context.Buffer.Append( CommonCustomFormatter.NullRepresentation );
				}
				else
				{
					ItemFormatter.Get( entry.Value.GetType() ).FormatObjectTo( entry.Value, context );
				}

				isFirstEntry = false;
			}

			if ( !isFirstEntry )
			{
				context.Buffer.Append( ' ' );
			}

			context.LeaveCollection();
			context.Buffer.Append( '}' );
		}
	}
}
