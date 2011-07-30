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

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Defines non-generic utlities for <see cref="NumericsFormatter{T}"/>.
	/// </summary>
	internal static class NumericsFormatter
	{
		// BigInteger, Decimal and Complex is not considered as numerics because ECMA Script spec specifies that numeric is Double,
		// so BigInteger will be overflowed, decimal will lose its precision, and complex will not be able to express.
		private static readonly Dictionary<RuntimeTypeHandle, bool> _numericTypes =
			new Dictionary<RuntimeTypeHandle, bool>()
			{
				{ typeof( byte ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( byte ) ) },
				{ typeof( sbyte ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( sbyte ) ) },
				{ typeof( short ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( short ) ) },
				{ typeof( ushort ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( ushort ) ) },
				{ typeof( int ).TypeHandle,	typeof( IFormattable ).IsAssignableFrom( typeof( int ) ) },
				{ typeof( uint ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( uint ) ) },
				{ typeof( long ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( long ) ) },
				{ typeof( ulong ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( ulong ) ) },
				{ typeof( float  ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( float  ) ) },
				{ typeof( double ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( double ) ) },
				{ typeof( IntPtr ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( IntPtr ) ) },
				{ typeof( UIntPtr ).TypeHandle,	typeof( IFormattable ).IsAssignableFrom( typeof( UIntPtr ) ) },
			};

		/// <summary>
		///		Determine whether specified type is numeric.
		/// </summary>
		/// <param name="type">Type.</param>
		/// <param name="isFormattable">Set true if <paramref name="typeHandle"/> is formattable.</param>
		/// <returns><c>true</c> if sepcified type is numerics.</returns>
		public static bool IsNumerics( Type type, out bool isFormattable )
		{
			return _numericTypes.TryGetValue( type.TypeHandle, out isFormattable );
		}
	}
}
