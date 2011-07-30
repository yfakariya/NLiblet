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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace NLiblet.Text
{
	/// <summary>
	///		Consolidates context information.
	/// </summary>
	internal sealed class FormattingContext
	{
		private readonly string _format;

		/// <summary>
		///		Get format string specified to Format().
		/// </summary>
		public string Format
		{
			get { return this._format; }
		}

		/// <summary>
		///		Get fallback provider which was passed on constructor. This value may be CultureInfo.
		/// </summary>
		public IFormatProvider FallbackProvider
		{
			get { return this._formatter.DefaultFormatProvider; }
		}

		private readonly StringBuilder _buffer;

		/// <summary>
		///		Get buffer to append formatting result.
		/// </summary>
		public StringBuilder Buffer
		{
			get { return this._buffer; }
		}

		private readonly CommonCustomFormatter _formatter;

		/// <summary>
		///		Get the reference to current <see cref="CommonCustomFormatter"/>.
		/// </summary>
		public CommonCustomFormatter Formatter
		{
			get { return this._formatter; }
		}

		private int _isInCollection;

		public bool IsInCollection
		{
			get { return this._isInCollection > 0; }
		}

		public void EnterCollection()
		{
			this._isInCollection++;
		}

		public void LeaveCollection()
		{
			this._isInCollection--;
		}

		public FormattingContext( CommonCustomFormatter formatter, string format, StringBuilder buffer )
		{
			Contract.Requires( formatter != null );

			this._formatter = formatter;
			this._format = format;
			this._buffer = buffer;
		}

		public sealed override string ToString()
		{
			return String.Format( CultureInfo.InvariantCulture, "{{ Format:'{0}', Formatter:@{1:x8}, Buffer.Length:{2:#,##0}, IsInCollection:{3} }}", this._format, RuntimeHelpers.GetHashCode( this._formatter ), this._buffer.Length, this._isInCollection );
		}
	}
}
