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

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		<see cref="ItemFormatter"/> specialized for <see cref="Object"/>.
	/// </summary>
	internal sealed class ObjectFormatter : ItemFormatter<object>
	{
		public static readonly ObjectFormatter Instance = new ObjectFormatter();

		private ObjectFormatter() { }

		public override void FormatTo( object item, FormattingContext context )
		{
			Debug.WriteLine( "ObjectFormatter::FormatTo( {0} : {1}, {2} )", item, item == null ? "(unknown)" : item.GetType().FullName, context );

			if ( Object.ReferenceEquals( item, null ) )
			{
				context.Buffer.Append( CommonCustomFormatter.NullRepresentation );
				return;
			}

			if ( context.IsInCollection )
			{
				context.Buffer.Append( '\"' );
			}

			context.Buffer.Append( item.ToString() );

			if ( context.IsInCollection )
			{
				context.Buffer.Append( '\"' );
			}
		}
	}
}
