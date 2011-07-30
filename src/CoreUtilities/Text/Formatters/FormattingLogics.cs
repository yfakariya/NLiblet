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
using System.Globalization;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Define common functins to format <see cref="IFormattable"/>.
	/// </summary>
	internal static class FormattingLogics
	{
		/// <summary>
		///		Common null representation.
		/// </summary>
		internal const string NullRepresentation = "null";

		/// <summary>
		///		Common escaping filter for collection.
		/// </summary>
		internal static readonly CharEscapingFilter CollectionItemEscapingFilter = CharEscapingFilter.UpperCaseDefaultCSharpLiteralStyle;

		/// <summary>
		///		Formats datetime equivalents and stores to the context buffer.
		/// </summary>
		/// <typeparam name="TDateTime">Type of datetime equivalent.</typeparam>
		/// <param name="dateTime">Formatting datetime equivalent.</param>
		/// <param name="context">Context information.</param>
		public static void FormatDateTimeTo<TDateTime>( TDateTime dateTime, FormattingContext context )
			where TDateTime : IFormattable
		{
			Debug.WriteLine( "FormattableFormatter<{0}>::FormatDateTimeTo( {1}, {2} )", typeof( TDateTime ).FullName, dateTime, context );

			if ( Object.ReferenceEquals( dateTime, null ) )
			{
				context.Buffer.Append( FormattingLogics.NullRepresentation );
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

		/// <summary>
		///		Formats timespan equivalents and stores to the context buffer.
		/// </summary>
		/// <typeparam name="TTimeSpan">Type of timespan equivalent.</typeparam>
		/// <param name="timeSpan">Formatting timespan equivalent.</param>
		/// <param name="context">Context information.</param>
		public static void FormatTimeSpanTo<TTimeSpan>( TTimeSpan timeSpan, FormattingContext context )
			where TTimeSpan : IFormattable
		{
			Debug.WriteLine( "FormattableFormatter<{0}>::FormatTimeSpanTo( {1}, {2} )", typeof( TTimeSpan ).FullName, timeSpan, context );

			if ( Object.ReferenceEquals( timeSpan, null ) )
			{
				context.Buffer.Append( FormattingLogics.NullRepresentation );
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

		/// <summary>
		///		Formats sequence and stores to the context buffer.
		/// </summary>
		/// <typeparam name="TElement">Type of element of sequence.</typeparam>
		/// <param name="sequence">Formatting sequence.</param>
		/// <param name="context">Context information.</param>
		/// <param name="state">Any object to be passed toward 3rd argument of <paramref name="elementFormatter"/>. This value can be null.</param>
		/// <param name="elementFormatter">
		///		Delegate to procedure which formats and stores each element of <paramref name="sequence"/>.
		///		1st argument is element of <paramref name="sequence"/>.
		///		2nd argument is context (same as <paramref name="context"/>).
		///		And 3rd argument is state object (same as <paramref name="state"/>) which will be null when <paramref name="state"/> is null.
		/// </param>
		public static void FormatSequence<TElement>( IEnumerable<TElement> sequence, FormattingContext context, object state, Action<TElement, FormattingContext, object> elementFormatter )
		{
			FormatCollection( sequence, context, state, elementFormatter, '[', ']' );
		}

		/// <summary>
		///		Formats dictionary and stores to the context buffer.
		/// </summary>
		/// <typeparam name="TElement">Type of element of dictionary.</typeparam>
		/// <param name="dictionary">Formatting dictionary.</param>
		/// <param name="context">Context information.</param>
		/// <param name="state">Any object to be passed toward 3rd argument of <paramref name="elementFormatter"/>. This value can be null.</param>
		/// <param name="elementFormatter">
		///		Delegate to procedure which formats and stores each element of <paramref name="dictionary"/>.
		///		1st argument is element of <paramref name="dictionary"/>.
		///		2nd argument is context (same as <paramref name="context"/>).
		///		And 3rd argument is state object (same as <paramref name="state"/>) which will be null when <paramref name="state"/> is null.
		/// </param>
		public static void FormatDictionary<TElement>( IEnumerable<TElement> dictionary, FormattingContext context, object state, Action<TElement, FormattingContext, object> elementFormatter )
		{
			FormatCollection( dictionary, context, state, elementFormatter, '{', '}' );
		}

		private static void FormatCollection<TElement>( IEnumerable<TElement> sequence, FormattingContext context, object state, Action<TElement, FormattingContext, object> elementFormatter, char opening, char closing )
		{
			context.Buffer.Append( opening );
			context.EnterCollection();

			if ( sequence != null )
			{
				bool isFirstEntry = true;
				foreach ( var entry in sequence )
				{
					if ( !isFirstEntry )
					{
						context.Buffer.Append( ", " );
					}
					else
					{
						context.Buffer.Append( ' ' );
					}

					elementFormatter( entry, context, state );

					isFirstEntry = false;
				}

				if ( !isFirstEntry )
				{
					context.Buffer.Append( ' ' );
				}
			}

			context.LeaveCollection();
			context.Buffer.Append( closing );
		}
	}
}
