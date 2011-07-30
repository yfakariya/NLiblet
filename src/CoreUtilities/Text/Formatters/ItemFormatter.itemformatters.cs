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

// This code is generated from T4Template ItemFormatter.itemformatters.tt.
// Do not modify this source code directly.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace NLiblet.Text.Formatters
{
	partial class ItemFormatter
	{
		private static readonly Dictionary<RuntimeTypeHandle, ItemFormatter> _itemFormatters =
			new Dictionary<RuntimeTypeHandle, ItemFormatter>()
			{
				{ typeof( Object ).TypeHandle, PolymorphicObjectFormatter.Instance },
				{ typeof( ValueType ).TypeHandle, PolymorphicObjectFormatter.Instance },
				{ typeof( bool ).TypeHandle, BooleanFormatter.Instance },
				{ typeof( DateTime ).TypeHandle, DateTimeFormatter.Instance },
				{ typeof( DateTimeOffset ).TypeHandle, DateTimeOffsetFormatter.Instance },
				{ typeof( TimeSpan ).TypeHandle, TimeSpanFormatter.Instance },
				{ typeof( String ).TypeHandle, StringFormatter.Instance },
				{ typeof( StringBuilder ).TypeHandle, StringBuilderFormatter.Instance },
				{ typeof( SerializationInfo ).TypeHandle, SerializationInfoFormatter.Instance },
				{ typeof( byte[] ).TypeHandle, BytesFormatter.Instance },
				{ typeof( char[] ).TypeHandle, StringFormatter.Instance },
				{ typeof( Byte ).TypeHandle, FormattableNumericsFormatter<Byte>.Instance },
				{ typeof( SByte ).TypeHandle, FormattableNumericsFormatter<SByte>.Instance },
				{ typeof( Int16 ).TypeHandle, FormattableNumericsFormatter<Int16>.Instance },
				{ typeof( UInt16 ).TypeHandle, FormattableNumericsFormatter<UInt16>.Instance },
				{ typeof( Int32 ).TypeHandle, FormattableNumericsFormatter<Int32>.Instance },
				{ typeof( UInt32 ).TypeHandle, FormattableNumericsFormatter<UInt32>.Instance },
				{ typeof( Int64 ).TypeHandle, FormattableNumericsFormatter<Int64>.Instance },
				{ typeof( UInt64 ).TypeHandle, FormattableNumericsFormatter<UInt64>.Instance },
				{ typeof( Single ).TypeHandle, FormattableNumericsFormatter<Single>.Instance },
				{ typeof( Double ).TypeHandle, FormattableNumericsFormatter<Double>.Instance },
				{ typeof( IntPtr ).TypeHandle, NumericsFormatter<IntPtr>.Instance },
				{ typeof( UIntPtr ).TypeHandle, NumericsFormatter<UIntPtr>.Instance },
			};
	}
}