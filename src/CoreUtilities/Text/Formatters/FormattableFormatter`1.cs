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
using System.Globalization;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Formatter for non-numeric <see cref="IFormattable"/>.
	/// </summary>
	internal sealed class FormattableFormatter<TFormattable> : ItemFormatter<TFormattable>
		where TFormattable : IFormattable
	{
		public FormattableFormatter() { }

		public sealed override void FormatTo( TFormattable item, FormattingContext context )
		{
			Debug.WriteLine( "FormattableFormatter<{0}>::FormatTo( {1}, {2} )", typeof( TFormattable ).FullName, item, context );

			if ( Object.ReferenceEquals( item, null ) )
			{
				context.Buffer.Append( FormattingLogics.NullRepresentation );
			}
			else
			{
				if ( context.IsInCollection )
				{
					context.Buffer.Append( '"' );

					// always tend to Json compat
					foreach ( var c in FormattingLogics.CollectionItemEscapingFilter.Escape( item.ToString( context.Format, CultureInfo.InvariantCulture ) ) )
					{
						context.Buffer.Append( c );
					}

					context.Buffer.Append( '"' );
				}
				else
				{
					foreach ( var c in FormattingLogics.CollectionItemEscapingFilter.Escape( item.ToString( context.Format, context.FallbackProvider ) ) )
					{
						context.Buffer.Append( c );
					}
				}
			}
		}
	}
}
