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

// This code is generated from T4Template GeneratedCodeHelper.primitiveconversions.tt.
// Do not modify this source code directly.

using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

using NLiblet.Properties;

namespace NLiblet.Reflection
{
#pragma warning disable 1572, 1587, 1591

	partial class GeneratedCodeHelper
	{
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static readonly MethodInfo UnboxPrimitiveIntegerMethod =
			typeof( GeneratedCodeHelper ).GetMethod( "UnboxPrimitiveInteger" );

		[CLSCompliant( false )]
		[EditorBrowsable( EditorBrowsableState.Never )]
		public ulong UnboxPrimitiveInteger( object value )
		{
			unchecked
			{
				if ( value is System.UInt64 )
				{
					return ( ulong )( System.UInt64 )value;
				}
				if ( value is System.Int64 )
				{
					return ( ulong )( System.Int64 )value;
				}
				if ( value is System.UInt32 )
				{
					return ( ulong )( System.UInt32 )value;
				}
				if ( value is System.Int32 )
				{
					return ( ulong )( System.Int32 )value;
				}
				if ( value is System.UInt16 )
				{
					return ( ulong )( System.UInt16 )value;
				}
				if ( value is System.Int16 )
				{
					return ( ulong )( System.Int16 )value;
				}
				if ( value is System.Byte )
				{
					return ( ulong )( System.Byte )value;
				}
				if ( value is System.SByte )
				{
					return ( ulong )( System.SByte )value;
				}
				throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, Resources.CastCode_UnknownPrimitive, value == null ? typeof( object ) : value.GetType() ) );
			}
		}
		
		[EditorBrowsable( EditorBrowsableState.Never )]
		public static readonly MethodInfo UnboxPrimitiveRealMethod =
			typeof( GeneratedCodeHelper ).GetMethod( "UnboxPrimitiveReal" );

		[EditorBrowsable( EditorBrowsableState.Never )]
		public double UnboxPrimitiveReal( object value )
		{
			unchecked
			{
				if ( value is System.Double )
				{
					return ( ulong )( System.Double )value;
				}
				if ( value is System.Single )
				{
					return ( ulong )( System.Single )value;
				}
				throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, Resources.CastCode_UnknownPrimitive, value == null ? typeof( object ) : value.GetType() ) );
			}
		}
	}

#pragma warning restore 1572, 1587, 1591
}