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
using System.Diagnostics.Contracts;
using System.Text;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Formatter for <see cref="String"/> (and its equivalents). This class does escaping.
	/// </summary>
	internal sealed class StringFormatter : ItemFormatter<IEnumerable<char>>
	{
		public static readonly StringFormatter Instance = new StringFormatter();

		private StringFormatter() { }

		public sealed override void FormatTo( IEnumerable<char> item, FormattingContext context )
		{
			Debug.WriteLine( "StringFormatter::FormatTo( {0}, {1} )", item, context );

			FeedStringWithEscapingIfInCollection( item, context );
		}

		public static void FeedStringWithEscapingIfInCollection( IEnumerable<char> charSequence, FormattingContext context )
		{
			Debug.WriteLine( "StringFormatter::FeedStringWithEscapingIfInCollection( {0}, {1} )", charSequence, context );

			if ( Object.ReferenceEquals( charSequence, null ) )
			{
				context.Buffer.Append( FormattingLogics.NullRepresentation );
				return;
			}

			if ( context.IsInCollection )
			{
				context.Buffer.Append( '\"' );
			}

			foreach ( var c in context.IsInCollection ? EscapeChars( charSequence ) : charSequence )
			{
				context.Buffer.Append( c );
			}

			if ( context.IsInCollection )
			{
				context.Buffer.Append( '\"' );
			}
		}

		internal static IEnumerable<char> EscapeChars( object item )
		{
			// TODO: Consider custom escaping... ?

			Contract.Assert(
				item is string
				|| item is StringBuilder
				|| item is IEnumerable<char>
			);

			var asEnumerable = item as IEnumerable<char>;
			if ( asEnumerable != null )
			{
				return FormattingLogics.CollectionItemEscapingFilter.Escape( asEnumerable );
			}

			var asStringBuilder = item as StringBuilder;
			if ( asStringBuilder != null )
			{
				return asStringBuilder.AsEnumerable();
			}

			return Empty.Array<char>();
		}

	}
}
