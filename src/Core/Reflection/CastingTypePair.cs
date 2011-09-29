 

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

// This code is generated from T4Template CastingTypePair.tt.
// Do not modify this source code directly.

using System;
using System.Globalization;

namespace NLiblet
{
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	internal partial struct CastingTypePair : global::System.IEquatable<CastingTypePair>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static CastingTypePair Null { get { return default( CastingTypePair ); } }
		
		/// <summary>
		/// 	<see cref="RuntimeTypeHandle" /> of cast source.
		/// </summary>
		private readonly RuntimeTypeHandle _source;
		
		/// <summary>
		/// 	Get <see cref="RuntimeTypeHandle" /> of cast source.
		/// </summary>
		/// <value>
		/// 	<see cref="RuntimeTypeHandle" /> of cast source.
		/// </value>
		public RuntimeTypeHandle Source
		{
			get
			{
				return this._source;
			}
		}
				
		/// <summary>
		/// 	<see cref="RuntimeTypeHandle" /> of cast target.
		/// </summary>
		private readonly RuntimeTypeHandle _target;
		
		/// <summary>
		/// 	Get <see cref="RuntimeTypeHandle" /> of cast target.
		/// </summary>
		/// <value>
		/// 	<see cref="RuntimeTypeHandle" /> of cast target.
		/// </value>
		public RuntimeTypeHandle Target
		{
			get
			{
				return this._target;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="source">
		/// 	<see cref="RuntimeTypeHandle" /> of cast source.
		/// </param>
		/// <param name="target">
		/// 	<see cref="RuntimeTypeHandle" /> of cast target.
		/// </param>
		public CastingTypePair(
			RuntimeTypeHandle source,
			RuntimeTypeHandle target
		)
		{
			this._source = source;
			this._target = target;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( CultureInfo.InvariantCulture, "'{0}'({1:x})->'{2}'({3:x})", Type.GetTypeFromHandle( Source ).AssemblyQualifiedName, Source.Value, Type.GetTypeFromHandle( Target ).AssemblyQualifiedName, Target.Value );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return this._source.GetHashCode() ^ this._target.GetHashCode();
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="CastingTypePair"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="CastingTypePair"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public override bool Equals( object obj )
		{
			if( Object.ReferenceEquals( obj, null ) )
			{
				return false;
			}
			
			if( !( obj is CastingTypePair ) )
			{
				return false;
			}
			
			return this.Equals( ( CastingTypePair )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="CastingTypePair"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( CastingTypePair other )
		{
			return this._source.Equals( other._source ) && this._target.Equals( other._target );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="CastingTypePair"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="CastingTypePair"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( CastingTypePair left, CastingTypePair right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="CastingTypePair"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="CastingTypePair"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( CastingTypePair left, CastingTypePair right )
		{
			return !left.Equals( right );
		}		
	}
}