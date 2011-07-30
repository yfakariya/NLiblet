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
using System.Text;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Formatter for <see cref="StringBuilder"/>. This class does escaping.
	/// </summary>
	internal sealed class StringBuilderFormatter : ItemFormatter<StringBuilder>
	{
		public static readonly StringBuilderFormatter Instance = new StringBuilderFormatter();

		private StringBuilderFormatter() { }

		public override void FormatTo( StringBuilder item, FormattingContext context )
		{
			Debug.WriteLine( "StringBuilderFormatter::FormatTo( {0}, {1} )", item, context );

			if ( item == null )
			{
				context.Buffer.Append( FormattingLogics.NullRepresentation );
			}
			else
			{
				StringFormatter.FeedStringWithEscapingIfInCollection( item.AsEnumerable(), context );
			}
		}
	}
}
