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
using System.Globalization;

namespace NLiblet.Text
{
	/// <summary>
	/// 
	/// </summary>
	/// <include file='Remarks.xml' path='doc/NLiblet.Text/FormatProviders/remarks'/>
	public static class FormatProviders
	{
		// TODO: caching

		public static IFormatProvider CurrentCulture
		{
			get { return new CommonCustomFormatter( CultureInfo.CurrentCulture ); }
		}

		public static IFormatProvider CurrentUICulture
		{
			get { return new CommonCustomFormatter( CultureInfo.CurrentUICulture ); }
		}

		public static IFormatProvider InvariantCulture
		{
			get { return new CommonCustomFormatter( CultureInfo.InvariantCulture ); }
		}
	}

}
