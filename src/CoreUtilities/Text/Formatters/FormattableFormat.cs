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
using System.Diagnostics;
using System.Globalization;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Define common functins to format <see cref="IFormattable"/>.
	/// </summary>
	internal static class FormattableFormat
	{
		public static void FormatDateTimeTo<TDateTime>( TDateTime dateTime, FormattingContext context )
			where TDateTime : IFormattable
		{
			Debug.WriteLine( "FormattableFormatter<{0}>::FormatDateTimeTo( {1}, {2} )", typeof( TDateTime ).FullName, dateTime, context );

			if ( Object.ReferenceEquals( dateTime, null ) )
			{
				context.Buffer.Append( CommonCustomFormatter.NullRepresentation );
			}
			else
			{
				if ( context.IsInCollection )
				{
					context.Buffer.Append( '"' );
					// always JSON compatible
					context.Buffer.Append( dateTime.ToString( "o", CultureInfo.InvariantCulture ) );
					context.Buffer.Append( '"' );
				}
				else
				{
					context.Buffer.Append( dateTime.ToString( context.Format, context.FallbackProvider ) );
				}
			}
		}

		public static void FormatTimeSpanTo<TTimeSpan>( TTimeSpan timeSpan, FormattingContext context )
			where TTimeSpan : IFormattable
		{
			Debug.WriteLine( "FormattableFormatter<{0}>::FormatTimeSpanTo( {1}, {2} )", typeof( TTimeSpan ).FullName, timeSpan, context );

			if ( Object.ReferenceEquals( timeSpan, null ) )
			{
				context.Buffer.Append( CommonCustomFormatter.NullRepresentation );
			}
			else
			{
				if ( context.IsInCollection )
				{
					context.Buffer.Append( '"' );
				}

				context.Buffer.Append( timeSpan.ToString( "c", context.FallbackProvider ) );

				if ( context.IsInCollection )
				{
					context.Buffer.Append( '"' );
				}
			}
		}
	}
}
