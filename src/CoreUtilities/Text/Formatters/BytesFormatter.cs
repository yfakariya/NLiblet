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
	///		Formatter for binary stream (Byte[] etc.).
	/// </summary>
	internal sealed class BytesFormatter : ItemFormatter<IEnumerable<byte>>
	{
		public static readonly BytesFormatter Instance = new BytesFormatter();

		private BytesFormatter() { }

		public sealed override void FormatTo( IEnumerable<byte> item, FormattingContext context )
		{
			Debug.WriteLine( "BytesFormatter::FormatBytesTo( {0}, {1} )", item, context );

			if ( context.IsInCollection )
			{
				context.Buffer.Append( '\"' );
			}

			context.Buffer.AppendHex( item );

			if ( context.IsInCollection )
			{
				context.Buffer.Append( '\"' );
			}
		}
	}
}
