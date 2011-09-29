 

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

using NLiblet.Text;

namespace NLiblet
{
	/// <summary>
	/// 	Represents pair of values.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct Pair<T1, T2> : global::System.IEquatable<Pair<T1, T2>>
				where T1 : IEquatable<T1>
				where T2 : IEquatable<T2>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static Pair<T1, T2> Null { get { return default( Pair<T1, T2> ); } }
		
		/// <summary>
		/// 	The item of #1.
		/// </summary>
		private readonly T1 _item1;
		
		/// <summary>
		/// 	Get the item of #1.
		/// </summary>
		/// <value>
		/// 	The item of #1.
		/// </value>
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
				
		/// <summary>
		/// 	The item of #2.
		/// </summary>
		private readonly T2 _item2;
		
		/// <summary>
		/// 	Get the item of #2.
		/// </summary>
		/// <value>
		/// 	The item of #2.
		/// </value>
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="item1">
		/// 	The item of #1.
		/// </param>
		/// <param name="item2">
		/// 	The item of #2.
		/// </param>
		public Pair(
			T1 item1,
			T2 item2
		)
		{
			this._item1 = item1;
			this._item2 = item2;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( FormatProviders.InvariantCulture, "{0}", new object[] { Item1, Item2 } as object );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._item1 == null ? 0 : this._item1.GetHashCode() ) ^ ( this._item2 == null ? 0 : this._item2.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="Pair&lt;T1, T2&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="Pair&lt;T1, T2&gt;"/> to compare.
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
			
			if( !( obj is Pair<T1, T2> ) )
			{
				return false;
			}
			
			return this.Equals( ( Pair<T1, T2> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="Pair&lt;T1, T2&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( Pair<T1, T2> other )
		{
			return ( this._item1 == null ? other._item1 == null : this._item1.Equals( other._item1 ) ) && ( this._item2 == null ? other._item2 == null : this._item2.Equals( other._item2 ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( Pair<T1, T2> left, Pair<T1, T2> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( Pair<T1, T2> left, Pair<T1, T2> right )
		{
			return !left.Equals( right );
		}		
	}
	/// <summary>
	/// 	Represents pair of values.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct Pair<T1, T2, T3> : global::System.IEquatable<Pair<T1, T2, T3>>
				where T1 : IEquatable<T1>
				where T2 : IEquatable<T2>
				where T3 : IEquatable<T3>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static Pair<T1, T2, T3> Null { get { return default( Pair<T1, T2, T3> ); } }
		
		/// <summary>
		/// 	The item of #1.
		/// </summary>
		private readonly T1 _item1;
		
		/// <summary>
		/// 	Get the item of #1.
		/// </summary>
		/// <value>
		/// 	The item of #1.
		/// </value>
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
				
		/// <summary>
		/// 	The item of #2.
		/// </summary>
		private readonly T2 _item2;
		
		/// <summary>
		/// 	Get the item of #2.
		/// </summary>
		/// <value>
		/// 	The item of #2.
		/// </value>
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
				
		/// <summary>
		/// 	The item of #3.
		/// </summary>
		private readonly T3 _item3;
		
		/// <summary>
		/// 	Get the item of #3.
		/// </summary>
		/// <value>
		/// 	The item of #3.
		/// </value>
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="item1">
		/// 	The item of #1.
		/// </param>
		/// <param name="item2">
		/// 	The item of #2.
		/// </param>
		/// <param name="item3">
		/// 	The item of #3.
		/// </param>
		public Pair(
			T1 item1,
			T2 item2,
			T3 item3
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( FormatProviders.InvariantCulture, "{0}", new object[] { Item1, Item2, Item3 } as object );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._item1 == null ? 0 : this._item1.GetHashCode() ) ^ ( this._item2 == null ? 0 : this._item2.GetHashCode() ) ^ ( this._item3 == null ? 0 : this._item3.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="Pair&lt;T1, T2, T3&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="Pair&lt;T1, T2, T3&gt;"/> to compare.
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
			
			if( !( obj is Pair<T1, T2, T3> ) )
			{
				return false;
			}
			
			return this.Equals( ( Pair<T1, T2, T3> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="Pair&lt;T1, T2, T3&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( Pair<T1, T2, T3> other )
		{
			return ( this._item1 == null ? other._item1 == null : this._item1.Equals( other._item1 ) ) && ( this._item2 == null ? other._item2 == null : this._item2.Equals( other._item2 ) ) && ( this._item3 == null ? other._item3 == null : this._item3.Equals( other._item3 ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( Pair<T1, T2, T3> left, Pair<T1, T2, T3> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( Pair<T1, T2, T3> left, Pair<T1, T2, T3> right )
		{
			return !left.Equals( right );
		}		
	}
	/// <summary>
	/// 	Represents pair of values.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct Pair<T1, T2, T3, T4> : global::System.IEquatable<Pair<T1, T2, T3, T4>>
				where T1 : IEquatable<T1>
				where T2 : IEquatable<T2>
				where T3 : IEquatable<T3>
				where T4 : IEquatable<T4>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static Pair<T1, T2, T3, T4> Null { get { return default( Pair<T1, T2, T3, T4> ); } }
		
		/// <summary>
		/// 	The item of #1.
		/// </summary>
		private readonly T1 _item1;
		
		/// <summary>
		/// 	Get the item of #1.
		/// </summary>
		/// <value>
		/// 	The item of #1.
		/// </value>
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
				
		/// <summary>
		/// 	The item of #2.
		/// </summary>
		private readonly T2 _item2;
		
		/// <summary>
		/// 	Get the item of #2.
		/// </summary>
		/// <value>
		/// 	The item of #2.
		/// </value>
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
				
		/// <summary>
		/// 	The item of #3.
		/// </summary>
		private readonly T3 _item3;
		
		/// <summary>
		/// 	Get the item of #3.
		/// </summary>
		/// <value>
		/// 	The item of #3.
		/// </value>
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
				
		/// <summary>
		/// 	The item of #4.
		/// </summary>
		private readonly T4 _item4;
		
		/// <summary>
		/// 	Get the item of #4.
		/// </summary>
		/// <value>
		/// 	The item of #4.
		/// </value>
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="item1">
		/// 	The item of #1.
		/// </param>
		/// <param name="item2">
		/// 	The item of #2.
		/// </param>
		/// <param name="item3">
		/// 	The item of #3.
		/// </param>
		/// <param name="item4">
		/// 	The item of #4.
		/// </param>
		public Pair(
			T1 item1,
			T2 item2,
			T3 item3,
			T4 item4
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( FormatProviders.InvariantCulture, "{0}", new object[] { Item1, Item2, Item3, Item4 } as object );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._item1 == null ? 0 : this._item1.GetHashCode() ) ^ ( this._item2 == null ? 0 : this._item2.GetHashCode() ) ^ ( this._item3 == null ? 0 : this._item3.GetHashCode() ) ^ ( this._item4 == null ? 0 : this._item4.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="Pair&lt;T1, T2, T3, T4&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4&gt;"/> to compare.
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
			
			if( !( obj is Pair<T1, T2, T3, T4> ) )
			{
				return false;
			}
			
			return this.Equals( ( Pair<T1, T2, T3, T4> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( Pair<T1, T2, T3, T4> other )
		{
			return ( this._item1 == null ? other._item1 == null : this._item1.Equals( other._item1 ) ) && ( this._item2 == null ? other._item2 == null : this._item2.Equals( other._item2 ) ) && ( this._item3 == null ? other._item3 == null : this._item3.Equals( other._item3 ) ) && ( this._item4 == null ? other._item4 == null : this._item4.Equals( other._item4 ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( Pair<T1, T2, T3, T4> left, Pair<T1, T2, T3, T4> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( Pair<T1, T2, T3, T4> left, Pair<T1, T2, T3, T4> right )
		{
			return !left.Equals( right );
		}		
	}
	/// <summary>
	/// 	Represents pair of values.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct Pair<T1, T2, T3, T4, T5> : global::System.IEquatable<Pair<T1, T2, T3, T4, T5>>
				where T1 : IEquatable<T1>
				where T2 : IEquatable<T2>
				where T3 : IEquatable<T3>
				where T4 : IEquatable<T4>
				where T5 : IEquatable<T5>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static Pair<T1, T2, T3, T4, T5> Null { get { return default( Pair<T1, T2, T3, T4, T5> ); } }
		
		/// <summary>
		/// 	The item of #1.
		/// </summary>
		private readonly T1 _item1;
		
		/// <summary>
		/// 	Get the item of #1.
		/// </summary>
		/// <value>
		/// 	The item of #1.
		/// </value>
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
				
		/// <summary>
		/// 	The item of #2.
		/// </summary>
		private readonly T2 _item2;
		
		/// <summary>
		/// 	Get the item of #2.
		/// </summary>
		/// <value>
		/// 	The item of #2.
		/// </value>
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
				
		/// <summary>
		/// 	The item of #3.
		/// </summary>
		private readonly T3 _item3;
		
		/// <summary>
		/// 	Get the item of #3.
		/// </summary>
		/// <value>
		/// 	The item of #3.
		/// </value>
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
				
		/// <summary>
		/// 	The item of #4.
		/// </summary>
		private readonly T4 _item4;
		
		/// <summary>
		/// 	Get the item of #4.
		/// </summary>
		/// <value>
		/// 	The item of #4.
		/// </value>
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
				
		/// <summary>
		/// 	The item of #5.
		/// </summary>
		private readonly T5 _item5;
		
		/// <summary>
		/// 	Get the item of #5.
		/// </summary>
		/// <value>
		/// 	The item of #5.
		/// </value>
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="item1">
		/// 	The item of #1.
		/// </param>
		/// <param name="item2">
		/// 	The item of #2.
		/// </param>
		/// <param name="item3">
		/// 	The item of #3.
		/// </param>
		/// <param name="item4">
		/// 	The item of #4.
		/// </param>
		/// <param name="item5">
		/// 	The item of #5.
		/// </param>
		public Pair(
			T1 item1,
			T2 item2,
			T3 item3,
			T4 item4,
			T5 item5
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( FormatProviders.InvariantCulture, "{0}", new object[] { Item1, Item2, Item3, Item4, Item5 } as object );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._item1 == null ? 0 : this._item1.GetHashCode() ) ^ ( this._item2 == null ? 0 : this._item2.GetHashCode() ) ^ ( this._item3 == null ? 0 : this._item3.GetHashCode() ) ^ ( this._item4 == null ? 0 : this._item4.GetHashCode() ) ^ ( this._item5 == null ? 0 : this._item5.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="Pair&lt;T1, T2, T3, T4, T5&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5&gt;"/> to compare.
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
			
			if( !( obj is Pair<T1, T2, T3, T4, T5> ) )
			{
				return false;
			}
			
			return this.Equals( ( Pair<T1, T2, T3, T4, T5> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( Pair<T1, T2, T3, T4, T5> other )
		{
			return ( this._item1 == null ? other._item1 == null : this._item1.Equals( other._item1 ) ) && ( this._item2 == null ? other._item2 == null : this._item2.Equals( other._item2 ) ) && ( this._item3 == null ? other._item3 == null : this._item3.Equals( other._item3 ) ) && ( this._item4 == null ? other._item4 == null : this._item4.Equals( other._item4 ) ) && ( this._item5 == null ? other._item5 == null : this._item5.Equals( other._item5 ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( Pair<T1, T2, T3, T4, T5> left, Pair<T1, T2, T3, T4, T5> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( Pair<T1, T2, T3, T4, T5> left, Pair<T1, T2, T3, T4, T5> right )
		{
			return !left.Equals( right );
		}		
	}
	/// <summary>
	/// 	Represents pair of values.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct Pair<T1, T2, T3, T4, T5, T6> : global::System.IEquatable<Pair<T1, T2, T3, T4, T5, T6>>
				where T1 : IEquatable<T1>
				where T2 : IEquatable<T2>
				where T3 : IEquatable<T3>
				where T4 : IEquatable<T4>
				where T5 : IEquatable<T5>
				where T6 : IEquatable<T6>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static Pair<T1, T2, T3, T4, T5, T6> Null { get { return default( Pair<T1, T2, T3, T4, T5, T6> ); } }
		
		/// <summary>
		/// 	The item of #1.
		/// </summary>
		private readonly T1 _item1;
		
		/// <summary>
		/// 	Get the item of #1.
		/// </summary>
		/// <value>
		/// 	The item of #1.
		/// </value>
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
				
		/// <summary>
		/// 	The item of #2.
		/// </summary>
		private readonly T2 _item2;
		
		/// <summary>
		/// 	Get the item of #2.
		/// </summary>
		/// <value>
		/// 	The item of #2.
		/// </value>
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
				
		/// <summary>
		/// 	The item of #3.
		/// </summary>
		private readonly T3 _item3;
		
		/// <summary>
		/// 	Get the item of #3.
		/// </summary>
		/// <value>
		/// 	The item of #3.
		/// </value>
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
				
		/// <summary>
		/// 	The item of #4.
		/// </summary>
		private readonly T4 _item4;
		
		/// <summary>
		/// 	Get the item of #4.
		/// </summary>
		/// <value>
		/// 	The item of #4.
		/// </value>
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
				
		/// <summary>
		/// 	The item of #5.
		/// </summary>
		private readonly T5 _item5;
		
		/// <summary>
		/// 	Get the item of #5.
		/// </summary>
		/// <value>
		/// 	The item of #5.
		/// </value>
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
				
		/// <summary>
		/// 	The item of #6.
		/// </summary>
		private readonly T6 _item6;
		
		/// <summary>
		/// 	Get the item of #6.
		/// </summary>
		/// <value>
		/// 	The item of #6.
		/// </value>
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="item1">
		/// 	The item of #1.
		/// </param>
		/// <param name="item2">
		/// 	The item of #2.
		/// </param>
		/// <param name="item3">
		/// 	The item of #3.
		/// </param>
		/// <param name="item4">
		/// 	The item of #4.
		/// </param>
		/// <param name="item5">
		/// 	The item of #5.
		/// </param>
		/// <param name="item6">
		/// 	The item of #6.
		/// </param>
		public Pair(
			T1 item1,
			T2 item2,
			T3 item3,
			T4 item4,
			T5 item5,
			T6 item6
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( FormatProviders.InvariantCulture, "{0}", new object[] { Item1, Item2, Item3, Item4, Item5, Item6 } as object );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._item1 == null ? 0 : this._item1.GetHashCode() ) ^ ( this._item2 == null ? 0 : this._item2.GetHashCode() ) ^ ( this._item3 == null ? 0 : this._item3.GetHashCode() ) ^ ( this._item4 == null ? 0 : this._item4.GetHashCode() ) ^ ( this._item5 == null ? 0 : this._item5.GetHashCode() ) ^ ( this._item6 == null ? 0 : this._item6.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="Pair&lt;T1, T2, T3, T4, T5, T6&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6&gt;"/> to compare.
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
			
			if( !( obj is Pair<T1, T2, T3, T4, T5, T6> ) )
			{
				return false;
			}
			
			return this.Equals( ( Pair<T1, T2, T3, T4, T5, T6> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( Pair<T1, T2, T3, T4, T5, T6> other )
		{
			return ( this._item1 == null ? other._item1 == null : this._item1.Equals( other._item1 ) ) && ( this._item2 == null ? other._item2 == null : this._item2.Equals( other._item2 ) ) && ( this._item3 == null ? other._item3 == null : this._item3.Equals( other._item3 ) ) && ( this._item4 == null ? other._item4 == null : this._item4.Equals( other._item4 ) ) && ( this._item5 == null ? other._item5 == null : this._item5.Equals( other._item5 ) ) && ( this._item6 == null ? other._item6 == null : this._item6.Equals( other._item6 ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( Pair<T1, T2, T3, T4, T5, T6> left, Pair<T1, T2, T3, T4, T5, T6> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( Pair<T1, T2, T3, T4, T5, T6> left, Pair<T1, T2, T3, T4, T5, T6> right )
		{
			return !left.Equals( right );
		}		
	}
	/// <summary>
	/// 	Represents pair of values.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct Pair<T1, T2, T3, T4, T5, T6, T7> : global::System.IEquatable<Pair<T1, T2, T3, T4, T5, T6, T7>>
				where T1 : IEquatable<T1>
				where T2 : IEquatable<T2>
				where T3 : IEquatable<T3>
				where T4 : IEquatable<T4>
				where T5 : IEquatable<T5>
				where T6 : IEquatable<T6>
				where T7 : IEquatable<T7>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static Pair<T1, T2, T3, T4, T5, T6, T7> Null { get { return default( Pair<T1, T2, T3, T4, T5, T6, T7> ); } }
		
		/// <summary>
		/// 	The item of #1.
		/// </summary>
		private readonly T1 _item1;
		
		/// <summary>
		/// 	Get the item of #1.
		/// </summary>
		/// <value>
		/// 	The item of #1.
		/// </value>
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
				
		/// <summary>
		/// 	The item of #2.
		/// </summary>
		private readonly T2 _item2;
		
		/// <summary>
		/// 	Get the item of #2.
		/// </summary>
		/// <value>
		/// 	The item of #2.
		/// </value>
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
				
		/// <summary>
		/// 	The item of #3.
		/// </summary>
		private readonly T3 _item3;
		
		/// <summary>
		/// 	Get the item of #3.
		/// </summary>
		/// <value>
		/// 	The item of #3.
		/// </value>
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
				
		/// <summary>
		/// 	The item of #4.
		/// </summary>
		private readonly T4 _item4;
		
		/// <summary>
		/// 	Get the item of #4.
		/// </summary>
		/// <value>
		/// 	The item of #4.
		/// </value>
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
				
		/// <summary>
		/// 	The item of #5.
		/// </summary>
		private readonly T5 _item5;
		
		/// <summary>
		/// 	Get the item of #5.
		/// </summary>
		/// <value>
		/// 	The item of #5.
		/// </value>
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
				
		/// <summary>
		/// 	The item of #6.
		/// </summary>
		private readonly T6 _item6;
		
		/// <summary>
		/// 	Get the item of #6.
		/// </summary>
		/// <value>
		/// 	The item of #6.
		/// </value>
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
				
		/// <summary>
		/// 	The item of #7.
		/// </summary>
		private readonly T7 _item7;
		
		/// <summary>
		/// 	Get the item of #7.
		/// </summary>
		/// <value>
		/// 	The item of #7.
		/// </value>
		public T7 Item7
		{
			get
			{
				return this._item7;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="item1">
		/// 	The item of #1.
		/// </param>
		/// <param name="item2">
		/// 	The item of #2.
		/// </param>
		/// <param name="item3">
		/// 	The item of #3.
		/// </param>
		/// <param name="item4">
		/// 	The item of #4.
		/// </param>
		/// <param name="item5">
		/// 	The item of #5.
		/// </param>
		/// <param name="item6">
		/// 	The item of #6.
		/// </param>
		/// <param name="item7">
		/// 	The item of #7.
		/// </param>
		public Pair(
			T1 item1,
			T2 item2,
			T3 item3,
			T4 item4,
			T5 item5,
			T6 item6,
			T7 item7
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
			this._item7 = item7;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( FormatProviders.InvariantCulture, "{0}", new object[] { Item1, Item2, Item3, Item4, Item5, Item6, Item7 } as object );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._item1 == null ? 0 : this._item1.GetHashCode() ) ^ ( this._item2 == null ? 0 : this._item2.GetHashCode() ) ^ ( this._item3 == null ? 0 : this._item3.GetHashCode() ) ^ ( this._item4 == null ? 0 : this._item4.GetHashCode() ) ^ ( this._item5 == null ? 0 : this._item5.GetHashCode() ) ^ ( this._item6 == null ? 0 : this._item6.GetHashCode() ) ^ ( this._item7 == null ? 0 : this._item7.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7&gt;"/> to compare.
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
			
			if( !( obj is Pair<T1, T2, T3, T4, T5, T6, T7> ) )
			{
				return false;
			}
			
			return this.Equals( ( Pair<T1, T2, T3, T4, T5, T6, T7> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( Pair<T1, T2, T3, T4, T5, T6, T7> other )
		{
			return ( this._item1 == null ? other._item1 == null : this._item1.Equals( other._item1 ) ) && ( this._item2 == null ? other._item2 == null : this._item2.Equals( other._item2 ) ) && ( this._item3 == null ? other._item3 == null : this._item3.Equals( other._item3 ) ) && ( this._item4 == null ? other._item4 == null : this._item4.Equals( other._item4 ) ) && ( this._item5 == null ? other._item5 == null : this._item5.Equals( other._item5 ) ) && ( this._item6 == null ? other._item6 == null : this._item6.Equals( other._item6 ) ) && ( this._item7 == null ? other._item7 == null : this._item7.Equals( other._item7 ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( Pair<T1, T2, T3, T4, T5, T6, T7> left, Pair<T1, T2, T3, T4, T5, T6, T7> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( Pair<T1, T2, T3, T4, T5, T6, T7> left, Pair<T1, T2, T3, T4, T5, T6, T7> right )
		{
			return !left.Equals( right );
		}		
	}
	/// <summary>
	/// 	Represents pair of values.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct Pair<T1, T2, T3, T4, T5, T6, T7, T8> : global::System.IEquatable<Pair<T1, T2, T3, T4, T5, T6, T7, T8>>
				where T1 : IEquatable<T1>
				where T2 : IEquatable<T2>
				where T3 : IEquatable<T3>
				where T4 : IEquatable<T4>
				where T5 : IEquatable<T5>
				where T6 : IEquatable<T6>
				where T7 : IEquatable<T7>
				where T8 : IEquatable<T8>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static Pair<T1, T2, T3, T4, T5, T6, T7, T8> Null { get { return default( Pair<T1, T2, T3, T4, T5, T6, T7, T8> ); } }
		
		/// <summary>
		/// 	The item of #1.
		/// </summary>
		private readonly T1 _item1;
		
		/// <summary>
		/// 	Get the item of #1.
		/// </summary>
		/// <value>
		/// 	The item of #1.
		/// </value>
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
				
		/// <summary>
		/// 	The item of #2.
		/// </summary>
		private readonly T2 _item2;
		
		/// <summary>
		/// 	Get the item of #2.
		/// </summary>
		/// <value>
		/// 	The item of #2.
		/// </value>
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
				
		/// <summary>
		/// 	The item of #3.
		/// </summary>
		private readonly T3 _item3;
		
		/// <summary>
		/// 	Get the item of #3.
		/// </summary>
		/// <value>
		/// 	The item of #3.
		/// </value>
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
				
		/// <summary>
		/// 	The item of #4.
		/// </summary>
		private readonly T4 _item4;
		
		/// <summary>
		/// 	Get the item of #4.
		/// </summary>
		/// <value>
		/// 	The item of #4.
		/// </value>
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
				
		/// <summary>
		/// 	The item of #5.
		/// </summary>
		private readonly T5 _item5;
		
		/// <summary>
		/// 	Get the item of #5.
		/// </summary>
		/// <value>
		/// 	The item of #5.
		/// </value>
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
				
		/// <summary>
		/// 	The item of #6.
		/// </summary>
		private readonly T6 _item6;
		
		/// <summary>
		/// 	Get the item of #6.
		/// </summary>
		/// <value>
		/// 	The item of #6.
		/// </value>
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
				
		/// <summary>
		/// 	The item of #7.
		/// </summary>
		private readonly T7 _item7;
		
		/// <summary>
		/// 	Get the item of #7.
		/// </summary>
		/// <value>
		/// 	The item of #7.
		/// </value>
		public T7 Item7
		{
			get
			{
				return this._item7;
			}
		}
				
		/// <summary>
		/// 	The item of #8.
		/// </summary>
		private readonly T8 _item8;
		
		/// <summary>
		/// 	Get the item of #8.
		/// </summary>
		/// <value>
		/// 	The item of #8.
		/// </value>
		public T8 Item8
		{
			get
			{
				return this._item8;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="item1">
		/// 	The item of #1.
		/// </param>
		/// <param name="item2">
		/// 	The item of #2.
		/// </param>
		/// <param name="item3">
		/// 	The item of #3.
		/// </param>
		/// <param name="item4">
		/// 	The item of #4.
		/// </param>
		/// <param name="item5">
		/// 	The item of #5.
		/// </param>
		/// <param name="item6">
		/// 	The item of #6.
		/// </param>
		/// <param name="item7">
		/// 	The item of #7.
		/// </param>
		/// <param name="item8">
		/// 	The item of #8.
		/// </param>
		public Pair(
			T1 item1,
			T2 item2,
			T3 item3,
			T4 item4,
			T5 item5,
			T6 item6,
			T7 item7,
			T8 item8
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
			this._item7 = item7;
			this._item8 = item8;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( FormatProviders.InvariantCulture, "{0}", new object[] { Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8 } as object );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._item1 == null ? 0 : this._item1.GetHashCode() ) ^ ( this._item2 == null ? 0 : this._item2.GetHashCode() ) ^ ( this._item3 == null ? 0 : this._item3.GetHashCode() ) ^ ( this._item4 == null ? 0 : this._item4.GetHashCode() ) ^ ( this._item5 == null ? 0 : this._item5.GetHashCode() ) ^ ( this._item6 == null ? 0 : this._item6.GetHashCode() ) ^ ( this._item7 == null ? 0 : this._item7.GetHashCode() ) ^ ( this._item8 == null ? 0 : this._item8.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;"/> to compare.
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
			
			if( !( obj is Pair<T1, T2, T3, T4, T5, T6, T7, T8> ) )
			{
				return false;
			}
			
			return this.Equals( ( Pair<T1, T2, T3, T4, T5, T6, T7, T8> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( Pair<T1, T2, T3, T4, T5, T6, T7, T8> other )
		{
			return ( this._item1 == null ? other._item1 == null : this._item1.Equals( other._item1 ) ) && ( this._item2 == null ? other._item2 == null : this._item2.Equals( other._item2 ) ) && ( this._item3 == null ? other._item3 == null : this._item3.Equals( other._item3 ) ) && ( this._item4 == null ? other._item4 == null : this._item4.Equals( other._item4 ) ) && ( this._item5 == null ? other._item5 == null : this._item5.Equals( other._item5 ) ) && ( this._item6 == null ? other._item6 == null : this._item6.Equals( other._item6 ) ) && ( this._item7 == null ? other._item7 == null : this._item7.Equals( other._item7 ) ) && ( this._item8 == null ? other._item8 == null : this._item8.Equals( other._item8 ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( Pair<T1, T2, T3, T4, T5, T6, T7, T8> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( Pair<T1, T2, T3, T4, T5, T6, T7, T8> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8> right )
		{
			return !left.Equals( right );
		}		
	}
	/// <summary>
	/// 	Represents pair of values.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9> : global::System.IEquatable<Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
				where T1 : IEquatable<T1>
				where T2 : IEquatable<T2>
				where T3 : IEquatable<T3>
				where T4 : IEquatable<T4>
				where T5 : IEquatable<T5>
				where T6 : IEquatable<T6>
				where T7 : IEquatable<T7>
				where T8 : IEquatable<T8>
				where T9 : IEquatable<T9>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9> Null { get { return default( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9> ); } }
		
		/// <summary>
		/// 	The item of #1.
		/// </summary>
		private readonly T1 _item1;
		
		/// <summary>
		/// 	Get the item of #1.
		/// </summary>
		/// <value>
		/// 	The item of #1.
		/// </value>
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
				
		/// <summary>
		/// 	The item of #2.
		/// </summary>
		private readonly T2 _item2;
		
		/// <summary>
		/// 	Get the item of #2.
		/// </summary>
		/// <value>
		/// 	The item of #2.
		/// </value>
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
				
		/// <summary>
		/// 	The item of #3.
		/// </summary>
		private readonly T3 _item3;
		
		/// <summary>
		/// 	Get the item of #3.
		/// </summary>
		/// <value>
		/// 	The item of #3.
		/// </value>
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
				
		/// <summary>
		/// 	The item of #4.
		/// </summary>
		private readonly T4 _item4;
		
		/// <summary>
		/// 	Get the item of #4.
		/// </summary>
		/// <value>
		/// 	The item of #4.
		/// </value>
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
				
		/// <summary>
		/// 	The item of #5.
		/// </summary>
		private readonly T5 _item5;
		
		/// <summary>
		/// 	Get the item of #5.
		/// </summary>
		/// <value>
		/// 	The item of #5.
		/// </value>
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
				
		/// <summary>
		/// 	The item of #6.
		/// </summary>
		private readonly T6 _item6;
		
		/// <summary>
		/// 	Get the item of #6.
		/// </summary>
		/// <value>
		/// 	The item of #6.
		/// </value>
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
				
		/// <summary>
		/// 	The item of #7.
		/// </summary>
		private readonly T7 _item7;
		
		/// <summary>
		/// 	Get the item of #7.
		/// </summary>
		/// <value>
		/// 	The item of #7.
		/// </value>
		public T7 Item7
		{
			get
			{
				return this._item7;
			}
		}
				
		/// <summary>
		/// 	The item of #8.
		/// </summary>
		private readonly T8 _item8;
		
		/// <summary>
		/// 	Get the item of #8.
		/// </summary>
		/// <value>
		/// 	The item of #8.
		/// </value>
		public T8 Item8
		{
			get
			{
				return this._item8;
			}
		}
				
		/// <summary>
		/// 	The item of #9.
		/// </summary>
		private readonly T9 _item9;
		
		/// <summary>
		/// 	Get the item of #9.
		/// </summary>
		/// <value>
		/// 	The item of #9.
		/// </value>
		public T9 Item9
		{
			get
			{
				return this._item9;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="item1">
		/// 	The item of #1.
		/// </param>
		/// <param name="item2">
		/// 	The item of #2.
		/// </param>
		/// <param name="item3">
		/// 	The item of #3.
		/// </param>
		/// <param name="item4">
		/// 	The item of #4.
		/// </param>
		/// <param name="item5">
		/// 	The item of #5.
		/// </param>
		/// <param name="item6">
		/// 	The item of #6.
		/// </param>
		/// <param name="item7">
		/// 	The item of #7.
		/// </param>
		/// <param name="item8">
		/// 	The item of #8.
		/// </param>
		/// <param name="item9">
		/// 	The item of #9.
		/// </param>
		public Pair(
			T1 item1,
			T2 item2,
			T3 item3,
			T4 item4,
			T5 item5,
			T6 item6,
			T7 item7,
			T8 item8,
			T9 item9
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
			this._item7 = item7;
			this._item8 = item8;
			this._item9 = item9;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( FormatProviders.InvariantCulture, "{0}", new object[] { Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, Item9 } as object );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._item1 == null ? 0 : this._item1.GetHashCode() ) ^ ( this._item2 == null ? 0 : this._item2.GetHashCode() ) ^ ( this._item3 == null ? 0 : this._item3.GetHashCode() ) ^ ( this._item4 == null ? 0 : this._item4.GetHashCode() ) ^ ( this._item5 == null ? 0 : this._item5.GetHashCode() ) ^ ( this._item6 == null ? 0 : this._item6.GetHashCode() ) ^ ( this._item7 == null ? 0 : this._item7.GetHashCode() ) ^ ( this._item8 == null ? 0 : this._item8.GetHashCode() ) ^ ( this._item9 == null ? 0 : this._item9.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9&gt;"/> to compare.
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
			
			if( !( obj is Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9> ) )
			{
				return false;
			}
			
			return this.Equals( ( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9> other )
		{
			return ( this._item1 == null ? other._item1 == null : this._item1.Equals( other._item1 ) ) && ( this._item2 == null ? other._item2 == null : this._item2.Equals( other._item2 ) ) && ( this._item3 == null ? other._item3 == null : this._item3.Equals( other._item3 ) ) && ( this._item4 == null ? other._item4 == null : this._item4.Equals( other._item4 ) ) && ( this._item5 == null ? other._item5 == null : this._item5.Equals( other._item5 ) ) && ( this._item6 == null ? other._item6 == null : this._item6.Equals( other._item6 ) ) && ( this._item7 == null ? other._item7 == null : this._item7.Equals( other._item7 ) ) && ( this._item8 == null ? other._item8 == null : this._item8.Equals( other._item8 ) ) && ( this._item9 == null ? other._item9 == null : this._item9.Equals( other._item9 ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9> right )
		{
			return !left.Equals( right );
		}		
	}
	/// <summary>
	/// 	Represents pair of values.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> : global::System.IEquatable<Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
				where T1 : IEquatable<T1>
				where T2 : IEquatable<T2>
				where T3 : IEquatable<T3>
				where T4 : IEquatable<T4>
				where T5 : IEquatable<T5>
				where T6 : IEquatable<T6>
				where T7 : IEquatable<T7>
				where T8 : IEquatable<T8>
				where T9 : IEquatable<T9>
				where T10 : IEquatable<T10>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> Null { get { return default( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ); } }
		
		/// <summary>
		/// 	The item of #1.
		/// </summary>
		private readonly T1 _item1;
		
		/// <summary>
		/// 	Get the item of #1.
		/// </summary>
		/// <value>
		/// 	The item of #1.
		/// </value>
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
				
		/// <summary>
		/// 	The item of #2.
		/// </summary>
		private readonly T2 _item2;
		
		/// <summary>
		/// 	Get the item of #2.
		/// </summary>
		/// <value>
		/// 	The item of #2.
		/// </value>
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
				
		/// <summary>
		/// 	The item of #3.
		/// </summary>
		private readonly T3 _item3;
		
		/// <summary>
		/// 	Get the item of #3.
		/// </summary>
		/// <value>
		/// 	The item of #3.
		/// </value>
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
				
		/// <summary>
		/// 	The item of #4.
		/// </summary>
		private readonly T4 _item4;
		
		/// <summary>
		/// 	Get the item of #4.
		/// </summary>
		/// <value>
		/// 	The item of #4.
		/// </value>
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
				
		/// <summary>
		/// 	The item of #5.
		/// </summary>
		private readonly T5 _item5;
		
		/// <summary>
		/// 	Get the item of #5.
		/// </summary>
		/// <value>
		/// 	The item of #5.
		/// </value>
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
				
		/// <summary>
		/// 	The item of #6.
		/// </summary>
		private readonly T6 _item6;
		
		/// <summary>
		/// 	Get the item of #6.
		/// </summary>
		/// <value>
		/// 	The item of #6.
		/// </value>
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
				
		/// <summary>
		/// 	The item of #7.
		/// </summary>
		private readonly T7 _item7;
		
		/// <summary>
		/// 	Get the item of #7.
		/// </summary>
		/// <value>
		/// 	The item of #7.
		/// </value>
		public T7 Item7
		{
			get
			{
				return this._item7;
			}
		}
				
		/// <summary>
		/// 	The item of #8.
		/// </summary>
		private readonly T8 _item8;
		
		/// <summary>
		/// 	Get the item of #8.
		/// </summary>
		/// <value>
		/// 	The item of #8.
		/// </value>
		public T8 Item8
		{
			get
			{
				return this._item8;
			}
		}
				
		/// <summary>
		/// 	The item of #9.
		/// </summary>
		private readonly T9 _item9;
		
		/// <summary>
		/// 	Get the item of #9.
		/// </summary>
		/// <value>
		/// 	The item of #9.
		/// </value>
		public T9 Item9
		{
			get
			{
				return this._item9;
			}
		}
				
		/// <summary>
		/// 	The item of #10.
		/// </summary>
		private readonly T10 _item10;
		
		/// <summary>
		/// 	Get the item of #10.
		/// </summary>
		/// <value>
		/// 	The item of #10.
		/// </value>
		public T10 Item10
		{
			get
			{
				return this._item10;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="item1">
		/// 	The item of #1.
		/// </param>
		/// <param name="item2">
		/// 	The item of #2.
		/// </param>
		/// <param name="item3">
		/// 	The item of #3.
		/// </param>
		/// <param name="item4">
		/// 	The item of #4.
		/// </param>
		/// <param name="item5">
		/// 	The item of #5.
		/// </param>
		/// <param name="item6">
		/// 	The item of #6.
		/// </param>
		/// <param name="item7">
		/// 	The item of #7.
		/// </param>
		/// <param name="item8">
		/// 	The item of #8.
		/// </param>
		/// <param name="item9">
		/// 	The item of #9.
		/// </param>
		/// <param name="item10">
		/// 	The item of #10.
		/// </param>
		public Pair(
			T1 item1,
			T2 item2,
			T3 item3,
			T4 item4,
			T5 item5,
			T6 item6,
			T7 item7,
			T8 item8,
			T9 item9,
			T10 item10
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
			this._item7 = item7;
			this._item8 = item8;
			this._item9 = item9;
			this._item10 = item10;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( FormatProviders.InvariantCulture, "{0}", new object[] { Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, Item9, Item10 } as object );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._item1 == null ? 0 : this._item1.GetHashCode() ) ^ ( this._item2 == null ? 0 : this._item2.GetHashCode() ) ^ ( this._item3 == null ? 0 : this._item3.GetHashCode() ) ^ ( this._item4 == null ? 0 : this._item4.GetHashCode() ) ^ ( this._item5 == null ? 0 : this._item5.GetHashCode() ) ^ ( this._item6 == null ? 0 : this._item6.GetHashCode() ) ^ ( this._item7 == null ? 0 : this._item7.GetHashCode() ) ^ ( this._item8 == null ? 0 : this._item8.GetHashCode() ) ^ ( this._item9 == null ? 0 : this._item9.GetHashCode() ) ^ ( this._item10 == null ? 0 : this._item10.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10&gt;"/> to compare.
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
			
			if( !( obj is Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> ) )
			{
				return false;
			}
			
			return this.Equals( ( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> other )
		{
			return ( this._item1 == null ? other._item1 == null : this._item1.Equals( other._item1 ) ) && ( this._item2 == null ? other._item2 == null : this._item2.Equals( other._item2 ) ) && ( this._item3 == null ? other._item3 == null : this._item3.Equals( other._item3 ) ) && ( this._item4 == null ? other._item4 == null : this._item4.Equals( other._item4 ) ) && ( this._item5 == null ? other._item5 == null : this._item5.Equals( other._item5 ) ) && ( this._item6 == null ? other._item6 == null : this._item6.Equals( other._item6 ) ) && ( this._item7 == null ? other._item7 == null : this._item7.Equals( other._item7 ) ) && ( this._item8 == null ? other._item8 == null : this._item8.Equals( other._item8 ) ) && ( this._item9 == null ? other._item9 == null : this._item9.Equals( other._item9 ) ) && ( this._item10 == null ? other._item10 == null : this._item10.Equals( other._item10 ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> right )
		{
			return !left.Equals( right );
		}		
	}
	/// <summary>
	/// 	Represents pair of values.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> : global::System.IEquatable<Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>>
				where T1 : IEquatable<T1>
				where T2 : IEquatable<T2>
				where T3 : IEquatable<T3>
				where T4 : IEquatable<T4>
				where T5 : IEquatable<T5>
				where T6 : IEquatable<T6>
				where T7 : IEquatable<T7>
				where T8 : IEquatable<T8>
				where T9 : IEquatable<T9>
				where T10 : IEquatable<T10>
				where T11 : IEquatable<T11>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> Null { get { return default( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ); } }
		
		/// <summary>
		/// 	The item of #1.
		/// </summary>
		private readonly T1 _item1;
		
		/// <summary>
		/// 	Get the item of #1.
		/// </summary>
		/// <value>
		/// 	The item of #1.
		/// </value>
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
				
		/// <summary>
		/// 	The item of #2.
		/// </summary>
		private readonly T2 _item2;
		
		/// <summary>
		/// 	Get the item of #2.
		/// </summary>
		/// <value>
		/// 	The item of #2.
		/// </value>
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
				
		/// <summary>
		/// 	The item of #3.
		/// </summary>
		private readonly T3 _item3;
		
		/// <summary>
		/// 	Get the item of #3.
		/// </summary>
		/// <value>
		/// 	The item of #3.
		/// </value>
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
				
		/// <summary>
		/// 	The item of #4.
		/// </summary>
		private readonly T4 _item4;
		
		/// <summary>
		/// 	Get the item of #4.
		/// </summary>
		/// <value>
		/// 	The item of #4.
		/// </value>
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
				
		/// <summary>
		/// 	The item of #5.
		/// </summary>
		private readonly T5 _item5;
		
		/// <summary>
		/// 	Get the item of #5.
		/// </summary>
		/// <value>
		/// 	The item of #5.
		/// </value>
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
				
		/// <summary>
		/// 	The item of #6.
		/// </summary>
		private readonly T6 _item6;
		
		/// <summary>
		/// 	Get the item of #6.
		/// </summary>
		/// <value>
		/// 	The item of #6.
		/// </value>
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
				
		/// <summary>
		/// 	The item of #7.
		/// </summary>
		private readonly T7 _item7;
		
		/// <summary>
		/// 	Get the item of #7.
		/// </summary>
		/// <value>
		/// 	The item of #7.
		/// </value>
		public T7 Item7
		{
			get
			{
				return this._item7;
			}
		}
				
		/// <summary>
		/// 	The item of #8.
		/// </summary>
		private readonly T8 _item8;
		
		/// <summary>
		/// 	Get the item of #8.
		/// </summary>
		/// <value>
		/// 	The item of #8.
		/// </value>
		public T8 Item8
		{
			get
			{
				return this._item8;
			}
		}
				
		/// <summary>
		/// 	The item of #9.
		/// </summary>
		private readonly T9 _item9;
		
		/// <summary>
		/// 	Get the item of #9.
		/// </summary>
		/// <value>
		/// 	The item of #9.
		/// </value>
		public T9 Item9
		{
			get
			{
				return this._item9;
			}
		}
				
		/// <summary>
		/// 	The item of #10.
		/// </summary>
		private readonly T10 _item10;
		
		/// <summary>
		/// 	Get the item of #10.
		/// </summary>
		/// <value>
		/// 	The item of #10.
		/// </value>
		public T10 Item10
		{
			get
			{
				return this._item10;
			}
		}
				
		/// <summary>
		/// 	The item of #11.
		/// </summary>
		private readonly T11 _item11;
		
		/// <summary>
		/// 	Get the item of #11.
		/// </summary>
		/// <value>
		/// 	The item of #11.
		/// </value>
		public T11 Item11
		{
			get
			{
				return this._item11;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="item1">
		/// 	The item of #1.
		/// </param>
		/// <param name="item2">
		/// 	The item of #2.
		/// </param>
		/// <param name="item3">
		/// 	The item of #3.
		/// </param>
		/// <param name="item4">
		/// 	The item of #4.
		/// </param>
		/// <param name="item5">
		/// 	The item of #5.
		/// </param>
		/// <param name="item6">
		/// 	The item of #6.
		/// </param>
		/// <param name="item7">
		/// 	The item of #7.
		/// </param>
		/// <param name="item8">
		/// 	The item of #8.
		/// </param>
		/// <param name="item9">
		/// 	The item of #9.
		/// </param>
		/// <param name="item10">
		/// 	The item of #10.
		/// </param>
		/// <param name="item11">
		/// 	The item of #11.
		/// </param>
		public Pair(
			T1 item1,
			T2 item2,
			T3 item3,
			T4 item4,
			T5 item5,
			T6 item6,
			T7 item7,
			T8 item8,
			T9 item9,
			T10 item10,
			T11 item11
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
			this._item7 = item7;
			this._item8 = item8;
			this._item9 = item9;
			this._item10 = item10;
			this._item11 = item11;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( FormatProviders.InvariantCulture, "{0}", new object[] { Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, Item9, Item10, Item11 } as object );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._item1 == null ? 0 : this._item1.GetHashCode() ) ^ ( this._item2 == null ? 0 : this._item2.GetHashCode() ) ^ ( this._item3 == null ? 0 : this._item3.GetHashCode() ) ^ ( this._item4 == null ? 0 : this._item4.GetHashCode() ) ^ ( this._item5 == null ? 0 : this._item5.GetHashCode() ) ^ ( this._item6 == null ? 0 : this._item6.GetHashCode() ) ^ ( this._item7 == null ? 0 : this._item7.GetHashCode() ) ^ ( this._item8 == null ? 0 : this._item8.GetHashCode() ) ^ ( this._item9 == null ? 0 : this._item9.GetHashCode() ) ^ ( this._item10 == null ? 0 : this._item10.GetHashCode() ) ^ ( this._item11 == null ? 0 : this._item11.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11&gt;"/> to compare.
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
			
			if( !( obj is Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> ) )
			{
				return false;
			}
			
			return this.Equals( ( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> other )
		{
			return ( this._item1 == null ? other._item1 == null : this._item1.Equals( other._item1 ) ) && ( this._item2 == null ? other._item2 == null : this._item2.Equals( other._item2 ) ) && ( this._item3 == null ? other._item3 == null : this._item3.Equals( other._item3 ) ) && ( this._item4 == null ? other._item4 == null : this._item4.Equals( other._item4 ) ) && ( this._item5 == null ? other._item5 == null : this._item5.Equals( other._item5 ) ) && ( this._item6 == null ? other._item6 == null : this._item6.Equals( other._item6 ) ) && ( this._item7 == null ? other._item7 == null : this._item7.Equals( other._item7 ) ) && ( this._item8 == null ? other._item8 == null : this._item8.Equals( other._item8 ) ) && ( this._item9 == null ? other._item9 == null : this._item9.Equals( other._item9 ) ) && ( this._item10 == null ? other._item10 == null : this._item10.Equals( other._item10 ) ) && ( this._item11 == null ? other._item11 == null : this._item11.Equals( other._item11 ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> right )
		{
			return !left.Equals( right );
		}		
	}
	/// <summary>
	/// 	Represents pair of values.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> : global::System.IEquatable<Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>>
				where T1 : IEquatable<T1>
				where T2 : IEquatable<T2>
				where T3 : IEquatable<T3>
				where T4 : IEquatable<T4>
				where T5 : IEquatable<T5>
				where T6 : IEquatable<T6>
				where T7 : IEquatable<T7>
				where T8 : IEquatable<T8>
				where T9 : IEquatable<T9>
				where T10 : IEquatable<T10>
				where T11 : IEquatable<T11>
				where T12 : IEquatable<T12>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> Null { get { return default( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ); } }
		
		/// <summary>
		/// 	The item of #1.
		/// </summary>
		private readonly T1 _item1;
		
		/// <summary>
		/// 	Get the item of #1.
		/// </summary>
		/// <value>
		/// 	The item of #1.
		/// </value>
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
				
		/// <summary>
		/// 	The item of #2.
		/// </summary>
		private readonly T2 _item2;
		
		/// <summary>
		/// 	Get the item of #2.
		/// </summary>
		/// <value>
		/// 	The item of #2.
		/// </value>
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
				
		/// <summary>
		/// 	The item of #3.
		/// </summary>
		private readonly T3 _item3;
		
		/// <summary>
		/// 	Get the item of #3.
		/// </summary>
		/// <value>
		/// 	The item of #3.
		/// </value>
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
				
		/// <summary>
		/// 	The item of #4.
		/// </summary>
		private readonly T4 _item4;
		
		/// <summary>
		/// 	Get the item of #4.
		/// </summary>
		/// <value>
		/// 	The item of #4.
		/// </value>
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
				
		/// <summary>
		/// 	The item of #5.
		/// </summary>
		private readonly T5 _item5;
		
		/// <summary>
		/// 	Get the item of #5.
		/// </summary>
		/// <value>
		/// 	The item of #5.
		/// </value>
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
				
		/// <summary>
		/// 	The item of #6.
		/// </summary>
		private readonly T6 _item6;
		
		/// <summary>
		/// 	Get the item of #6.
		/// </summary>
		/// <value>
		/// 	The item of #6.
		/// </value>
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
				
		/// <summary>
		/// 	The item of #7.
		/// </summary>
		private readonly T7 _item7;
		
		/// <summary>
		/// 	Get the item of #7.
		/// </summary>
		/// <value>
		/// 	The item of #7.
		/// </value>
		public T7 Item7
		{
			get
			{
				return this._item7;
			}
		}
				
		/// <summary>
		/// 	The item of #8.
		/// </summary>
		private readonly T8 _item8;
		
		/// <summary>
		/// 	Get the item of #8.
		/// </summary>
		/// <value>
		/// 	The item of #8.
		/// </value>
		public T8 Item8
		{
			get
			{
				return this._item8;
			}
		}
				
		/// <summary>
		/// 	The item of #9.
		/// </summary>
		private readonly T9 _item9;
		
		/// <summary>
		/// 	Get the item of #9.
		/// </summary>
		/// <value>
		/// 	The item of #9.
		/// </value>
		public T9 Item9
		{
			get
			{
				return this._item9;
			}
		}
				
		/// <summary>
		/// 	The item of #10.
		/// </summary>
		private readonly T10 _item10;
		
		/// <summary>
		/// 	Get the item of #10.
		/// </summary>
		/// <value>
		/// 	The item of #10.
		/// </value>
		public T10 Item10
		{
			get
			{
				return this._item10;
			}
		}
				
		/// <summary>
		/// 	The item of #11.
		/// </summary>
		private readonly T11 _item11;
		
		/// <summary>
		/// 	Get the item of #11.
		/// </summary>
		/// <value>
		/// 	The item of #11.
		/// </value>
		public T11 Item11
		{
			get
			{
				return this._item11;
			}
		}
				
		/// <summary>
		/// 	The item of #12.
		/// </summary>
		private readonly T12 _item12;
		
		/// <summary>
		/// 	Get the item of #12.
		/// </summary>
		/// <value>
		/// 	The item of #12.
		/// </value>
		public T12 Item12
		{
			get
			{
				return this._item12;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="item1">
		/// 	The item of #1.
		/// </param>
		/// <param name="item2">
		/// 	The item of #2.
		/// </param>
		/// <param name="item3">
		/// 	The item of #3.
		/// </param>
		/// <param name="item4">
		/// 	The item of #4.
		/// </param>
		/// <param name="item5">
		/// 	The item of #5.
		/// </param>
		/// <param name="item6">
		/// 	The item of #6.
		/// </param>
		/// <param name="item7">
		/// 	The item of #7.
		/// </param>
		/// <param name="item8">
		/// 	The item of #8.
		/// </param>
		/// <param name="item9">
		/// 	The item of #9.
		/// </param>
		/// <param name="item10">
		/// 	The item of #10.
		/// </param>
		/// <param name="item11">
		/// 	The item of #11.
		/// </param>
		/// <param name="item12">
		/// 	The item of #12.
		/// </param>
		public Pair(
			T1 item1,
			T2 item2,
			T3 item3,
			T4 item4,
			T5 item5,
			T6 item6,
			T7 item7,
			T8 item8,
			T9 item9,
			T10 item10,
			T11 item11,
			T12 item12
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
			this._item7 = item7;
			this._item8 = item8;
			this._item9 = item9;
			this._item10 = item10;
			this._item11 = item11;
			this._item12 = item12;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( FormatProviders.InvariantCulture, "{0}", new object[] { Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, Item9, Item10, Item11, Item12 } as object );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._item1 == null ? 0 : this._item1.GetHashCode() ) ^ ( this._item2 == null ? 0 : this._item2.GetHashCode() ) ^ ( this._item3 == null ? 0 : this._item3.GetHashCode() ) ^ ( this._item4 == null ? 0 : this._item4.GetHashCode() ) ^ ( this._item5 == null ? 0 : this._item5.GetHashCode() ) ^ ( this._item6 == null ? 0 : this._item6.GetHashCode() ) ^ ( this._item7 == null ? 0 : this._item7.GetHashCode() ) ^ ( this._item8 == null ? 0 : this._item8.GetHashCode() ) ^ ( this._item9 == null ? 0 : this._item9.GetHashCode() ) ^ ( this._item10 == null ? 0 : this._item10.GetHashCode() ) ^ ( this._item11 == null ? 0 : this._item11.GetHashCode() ) ^ ( this._item12 == null ? 0 : this._item12.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12&gt;"/> to compare.
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
			
			if( !( obj is Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> ) )
			{
				return false;
			}
			
			return this.Equals( ( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> other )
		{
			return ( this._item1 == null ? other._item1 == null : this._item1.Equals( other._item1 ) ) && ( this._item2 == null ? other._item2 == null : this._item2.Equals( other._item2 ) ) && ( this._item3 == null ? other._item3 == null : this._item3.Equals( other._item3 ) ) && ( this._item4 == null ? other._item4 == null : this._item4.Equals( other._item4 ) ) && ( this._item5 == null ? other._item5 == null : this._item5.Equals( other._item5 ) ) && ( this._item6 == null ? other._item6 == null : this._item6.Equals( other._item6 ) ) && ( this._item7 == null ? other._item7 == null : this._item7.Equals( other._item7 ) ) && ( this._item8 == null ? other._item8 == null : this._item8.Equals( other._item8 ) ) && ( this._item9 == null ? other._item9 == null : this._item9.Equals( other._item9 ) ) && ( this._item10 == null ? other._item10 == null : this._item10.Equals( other._item10 ) ) && ( this._item11 == null ? other._item11 == null : this._item11.Equals( other._item11 ) ) && ( this._item12 == null ? other._item12 == null : this._item12.Equals( other._item12 ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> right )
		{
			return !left.Equals( right );
		}		
	}
	/// <summary>
	/// 	Represents pair of values.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> : global::System.IEquatable<Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>>
				where T1 : IEquatable<T1>
				where T2 : IEquatable<T2>
				where T3 : IEquatable<T3>
				where T4 : IEquatable<T4>
				where T5 : IEquatable<T5>
				where T6 : IEquatable<T6>
				where T7 : IEquatable<T7>
				where T8 : IEquatable<T8>
				where T9 : IEquatable<T9>
				where T10 : IEquatable<T10>
				where T11 : IEquatable<T11>
				where T12 : IEquatable<T12>
				where T13 : IEquatable<T13>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> Null { get { return default( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ); } }
		
		/// <summary>
		/// 	The item of #1.
		/// </summary>
		private readonly T1 _item1;
		
		/// <summary>
		/// 	Get the item of #1.
		/// </summary>
		/// <value>
		/// 	The item of #1.
		/// </value>
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
				
		/// <summary>
		/// 	The item of #2.
		/// </summary>
		private readonly T2 _item2;
		
		/// <summary>
		/// 	Get the item of #2.
		/// </summary>
		/// <value>
		/// 	The item of #2.
		/// </value>
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
				
		/// <summary>
		/// 	The item of #3.
		/// </summary>
		private readonly T3 _item3;
		
		/// <summary>
		/// 	Get the item of #3.
		/// </summary>
		/// <value>
		/// 	The item of #3.
		/// </value>
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
				
		/// <summary>
		/// 	The item of #4.
		/// </summary>
		private readonly T4 _item4;
		
		/// <summary>
		/// 	Get the item of #4.
		/// </summary>
		/// <value>
		/// 	The item of #4.
		/// </value>
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
				
		/// <summary>
		/// 	The item of #5.
		/// </summary>
		private readonly T5 _item5;
		
		/// <summary>
		/// 	Get the item of #5.
		/// </summary>
		/// <value>
		/// 	The item of #5.
		/// </value>
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
				
		/// <summary>
		/// 	The item of #6.
		/// </summary>
		private readonly T6 _item6;
		
		/// <summary>
		/// 	Get the item of #6.
		/// </summary>
		/// <value>
		/// 	The item of #6.
		/// </value>
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
				
		/// <summary>
		/// 	The item of #7.
		/// </summary>
		private readonly T7 _item7;
		
		/// <summary>
		/// 	Get the item of #7.
		/// </summary>
		/// <value>
		/// 	The item of #7.
		/// </value>
		public T7 Item7
		{
			get
			{
				return this._item7;
			}
		}
				
		/// <summary>
		/// 	The item of #8.
		/// </summary>
		private readonly T8 _item8;
		
		/// <summary>
		/// 	Get the item of #8.
		/// </summary>
		/// <value>
		/// 	The item of #8.
		/// </value>
		public T8 Item8
		{
			get
			{
				return this._item8;
			}
		}
				
		/// <summary>
		/// 	The item of #9.
		/// </summary>
		private readonly T9 _item9;
		
		/// <summary>
		/// 	Get the item of #9.
		/// </summary>
		/// <value>
		/// 	The item of #9.
		/// </value>
		public T9 Item9
		{
			get
			{
				return this._item9;
			}
		}
				
		/// <summary>
		/// 	The item of #10.
		/// </summary>
		private readonly T10 _item10;
		
		/// <summary>
		/// 	Get the item of #10.
		/// </summary>
		/// <value>
		/// 	The item of #10.
		/// </value>
		public T10 Item10
		{
			get
			{
				return this._item10;
			}
		}
				
		/// <summary>
		/// 	The item of #11.
		/// </summary>
		private readonly T11 _item11;
		
		/// <summary>
		/// 	Get the item of #11.
		/// </summary>
		/// <value>
		/// 	The item of #11.
		/// </value>
		public T11 Item11
		{
			get
			{
				return this._item11;
			}
		}
				
		/// <summary>
		/// 	The item of #12.
		/// </summary>
		private readonly T12 _item12;
		
		/// <summary>
		/// 	Get the item of #12.
		/// </summary>
		/// <value>
		/// 	The item of #12.
		/// </value>
		public T12 Item12
		{
			get
			{
				return this._item12;
			}
		}
				
		/// <summary>
		/// 	The item of #13.
		/// </summary>
		private readonly T13 _item13;
		
		/// <summary>
		/// 	Get the item of #13.
		/// </summary>
		/// <value>
		/// 	The item of #13.
		/// </value>
		public T13 Item13
		{
			get
			{
				return this._item13;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="item1">
		/// 	The item of #1.
		/// </param>
		/// <param name="item2">
		/// 	The item of #2.
		/// </param>
		/// <param name="item3">
		/// 	The item of #3.
		/// </param>
		/// <param name="item4">
		/// 	The item of #4.
		/// </param>
		/// <param name="item5">
		/// 	The item of #5.
		/// </param>
		/// <param name="item6">
		/// 	The item of #6.
		/// </param>
		/// <param name="item7">
		/// 	The item of #7.
		/// </param>
		/// <param name="item8">
		/// 	The item of #8.
		/// </param>
		/// <param name="item9">
		/// 	The item of #9.
		/// </param>
		/// <param name="item10">
		/// 	The item of #10.
		/// </param>
		/// <param name="item11">
		/// 	The item of #11.
		/// </param>
		/// <param name="item12">
		/// 	The item of #12.
		/// </param>
		/// <param name="item13">
		/// 	The item of #13.
		/// </param>
		public Pair(
			T1 item1,
			T2 item2,
			T3 item3,
			T4 item4,
			T5 item5,
			T6 item6,
			T7 item7,
			T8 item8,
			T9 item9,
			T10 item10,
			T11 item11,
			T12 item12,
			T13 item13
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
			this._item7 = item7;
			this._item8 = item8;
			this._item9 = item9;
			this._item10 = item10;
			this._item11 = item11;
			this._item12 = item12;
			this._item13 = item13;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( FormatProviders.InvariantCulture, "{0}", new object[] { Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, Item9, Item10, Item11, Item12, Item13 } as object );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._item1 == null ? 0 : this._item1.GetHashCode() ) ^ ( this._item2 == null ? 0 : this._item2.GetHashCode() ) ^ ( this._item3 == null ? 0 : this._item3.GetHashCode() ) ^ ( this._item4 == null ? 0 : this._item4.GetHashCode() ) ^ ( this._item5 == null ? 0 : this._item5.GetHashCode() ) ^ ( this._item6 == null ? 0 : this._item6.GetHashCode() ) ^ ( this._item7 == null ? 0 : this._item7.GetHashCode() ) ^ ( this._item8 == null ? 0 : this._item8.GetHashCode() ) ^ ( this._item9 == null ? 0 : this._item9.GetHashCode() ) ^ ( this._item10 == null ? 0 : this._item10.GetHashCode() ) ^ ( this._item11 == null ? 0 : this._item11.GetHashCode() ) ^ ( this._item12 == null ? 0 : this._item12.GetHashCode() ) ^ ( this._item13 == null ? 0 : this._item13.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13&gt;"/> to compare.
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
			
			if( !( obj is Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> ) )
			{
				return false;
			}
			
			return this.Equals( ( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> other )
		{
			return ( this._item1 == null ? other._item1 == null : this._item1.Equals( other._item1 ) ) && ( this._item2 == null ? other._item2 == null : this._item2.Equals( other._item2 ) ) && ( this._item3 == null ? other._item3 == null : this._item3.Equals( other._item3 ) ) && ( this._item4 == null ? other._item4 == null : this._item4.Equals( other._item4 ) ) && ( this._item5 == null ? other._item5 == null : this._item5.Equals( other._item5 ) ) && ( this._item6 == null ? other._item6 == null : this._item6.Equals( other._item6 ) ) && ( this._item7 == null ? other._item7 == null : this._item7.Equals( other._item7 ) ) && ( this._item8 == null ? other._item8 == null : this._item8.Equals( other._item8 ) ) && ( this._item9 == null ? other._item9 == null : this._item9.Equals( other._item9 ) ) && ( this._item10 == null ? other._item10 == null : this._item10.Equals( other._item10 ) ) && ( this._item11 == null ? other._item11 == null : this._item11.Equals( other._item11 ) ) && ( this._item12 == null ? other._item12 == null : this._item12.Equals( other._item12 ) ) && ( this._item13 == null ? other._item13 == null : this._item13.Equals( other._item13 ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> right )
		{
			return !left.Equals( right );
		}		
	}
	/// <summary>
	/// 	Represents pair of values.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> : global::System.IEquatable<Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>>
				where T1 : IEquatable<T1>
				where T2 : IEquatable<T2>
				where T3 : IEquatable<T3>
				where T4 : IEquatable<T4>
				where T5 : IEquatable<T5>
				where T6 : IEquatable<T6>
				where T7 : IEquatable<T7>
				where T8 : IEquatable<T8>
				where T9 : IEquatable<T9>
				where T10 : IEquatable<T10>
				where T11 : IEquatable<T11>
				where T12 : IEquatable<T12>
				where T13 : IEquatable<T13>
				where T14 : IEquatable<T14>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> Null { get { return default( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ); } }
		
		/// <summary>
		/// 	The item of #1.
		/// </summary>
		private readonly T1 _item1;
		
		/// <summary>
		/// 	Get the item of #1.
		/// </summary>
		/// <value>
		/// 	The item of #1.
		/// </value>
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
				
		/// <summary>
		/// 	The item of #2.
		/// </summary>
		private readonly T2 _item2;
		
		/// <summary>
		/// 	Get the item of #2.
		/// </summary>
		/// <value>
		/// 	The item of #2.
		/// </value>
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
				
		/// <summary>
		/// 	The item of #3.
		/// </summary>
		private readonly T3 _item3;
		
		/// <summary>
		/// 	Get the item of #3.
		/// </summary>
		/// <value>
		/// 	The item of #3.
		/// </value>
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
				
		/// <summary>
		/// 	The item of #4.
		/// </summary>
		private readonly T4 _item4;
		
		/// <summary>
		/// 	Get the item of #4.
		/// </summary>
		/// <value>
		/// 	The item of #4.
		/// </value>
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
				
		/// <summary>
		/// 	The item of #5.
		/// </summary>
		private readonly T5 _item5;
		
		/// <summary>
		/// 	Get the item of #5.
		/// </summary>
		/// <value>
		/// 	The item of #5.
		/// </value>
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
				
		/// <summary>
		/// 	The item of #6.
		/// </summary>
		private readonly T6 _item6;
		
		/// <summary>
		/// 	Get the item of #6.
		/// </summary>
		/// <value>
		/// 	The item of #6.
		/// </value>
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
				
		/// <summary>
		/// 	The item of #7.
		/// </summary>
		private readonly T7 _item7;
		
		/// <summary>
		/// 	Get the item of #7.
		/// </summary>
		/// <value>
		/// 	The item of #7.
		/// </value>
		public T7 Item7
		{
			get
			{
				return this._item7;
			}
		}
				
		/// <summary>
		/// 	The item of #8.
		/// </summary>
		private readonly T8 _item8;
		
		/// <summary>
		/// 	Get the item of #8.
		/// </summary>
		/// <value>
		/// 	The item of #8.
		/// </value>
		public T8 Item8
		{
			get
			{
				return this._item8;
			}
		}
				
		/// <summary>
		/// 	The item of #9.
		/// </summary>
		private readonly T9 _item9;
		
		/// <summary>
		/// 	Get the item of #9.
		/// </summary>
		/// <value>
		/// 	The item of #9.
		/// </value>
		public T9 Item9
		{
			get
			{
				return this._item9;
			}
		}
				
		/// <summary>
		/// 	The item of #10.
		/// </summary>
		private readonly T10 _item10;
		
		/// <summary>
		/// 	Get the item of #10.
		/// </summary>
		/// <value>
		/// 	The item of #10.
		/// </value>
		public T10 Item10
		{
			get
			{
				return this._item10;
			}
		}
				
		/// <summary>
		/// 	The item of #11.
		/// </summary>
		private readonly T11 _item11;
		
		/// <summary>
		/// 	Get the item of #11.
		/// </summary>
		/// <value>
		/// 	The item of #11.
		/// </value>
		public T11 Item11
		{
			get
			{
				return this._item11;
			}
		}
				
		/// <summary>
		/// 	The item of #12.
		/// </summary>
		private readonly T12 _item12;
		
		/// <summary>
		/// 	Get the item of #12.
		/// </summary>
		/// <value>
		/// 	The item of #12.
		/// </value>
		public T12 Item12
		{
			get
			{
				return this._item12;
			}
		}
				
		/// <summary>
		/// 	The item of #13.
		/// </summary>
		private readonly T13 _item13;
		
		/// <summary>
		/// 	Get the item of #13.
		/// </summary>
		/// <value>
		/// 	The item of #13.
		/// </value>
		public T13 Item13
		{
			get
			{
				return this._item13;
			}
		}
				
		/// <summary>
		/// 	The item of #14.
		/// </summary>
		private readonly T14 _item14;
		
		/// <summary>
		/// 	Get the item of #14.
		/// </summary>
		/// <value>
		/// 	The item of #14.
		/// </value>
		public T14 Item14
		{
			get
			{
				return this._item14;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="item1">
		/// 	The item of #1.
		/// </param>
		/// <param name="item2">
		/// 	The item of #2.
		/// </param>
		/// <param name="item3">
		/// 	The item of #3.
		/// </param>
		/// <param name="item4">
		/// 	The item of #4.
		/// </param>
		/// <param name="item5">
		/// 	The item of #5.
		/// </param>
		/// <param name="item6">
		/// 	The item of #6.
		/// </param>
		/// <param name="item7">
		/// 	The item of #7.
		/// </param>
		/// <param name="item8">
		/// 	The item of #8.
		/// </param>
		/// <param name="item9">
		/// 	The item of #9.
		/// </param>
		/// <param name="item10">
		/// 	The item of #10.
		/// </param>
		/// <param name="item11">
		/// 	The item of #11.
		/// </param>
		/// <param name="item12">
		/// 	The item of #12.
		/// </param>
		/// <param name="item13">
		/// 	The item of #13.
		/// </param>
		/// <param name="item14">
		/// 	The item of #14.
		/// </param>
		public Pair(
			T1 item1,
			T2 item2,
			T3 item3,
			T4 item4,
			T5 item5,
			T6 item6,
			T7 item7,
			T8 item8,
			T9 item9,
			T10 item10,
			T11 item11,
			T12 item12,
			T13 item13,
			T14 item14
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
			this._item7 = item7;
			this._item8 = item8;
			this._item9 = item9;
			this._item10 = item10;
			this._item11 = item11;
			this._item12 = item12;
			this._item13 = item13;
			this._item14 = item14;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( FormatProviders.InvariantCulture, "{0}", new object[] { Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, Item9, Item10, Item11, Item12, Item13, Item14 } as object );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._item1 == null ? 0 : this._item1.GetHashCode() ) ^ ( this._item2 == null ? 0 : this._item2.GetHashCode() ) ^ ( this._item3 == null ? 0 : this._item3.GetHashCode() ) ^ ( this._item4 == null ? 0 : this._item4.GetHashCode() ) ^ ( this._item5 == null ? 0 : this._item5.GetHashCode() ) ^ ( this._item6 == null ? 0 : this._item6.GetHashCode() ) ^ ( this._item7 == null ? 0 : this._item7.GetHashCode() ) ^ ( this._item8 == null ? 0 : this._item8.GetHashCode() ) ^ ( this._item9 == null ? 0 : this._item9.GetHashCode() ) ^ ( this._item10 == null ? 0 : this._item10.GetHashCode() ) ^ ( this._item11 == null ? 0 : this._item11.GetHashCode() ) ^ ( this._item12 == null ? 0 : this._item12.GetHashCode() ) ^ ( this._item13 == null ? 0 : this._item13.GetHashCode() ) ^ ( this._item14 == null ? 0 : this._item14.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14&gt;"/> to compare.
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
			
			if( !( obj is Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> ) )
			{
				return false;
			}
			
			return this.Equals( ( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> other )
		{
			return ( this._item1 == null ? other._item1 == null : this._item1.Equals( other._item1 ) ) && ( this._item2 == null ? other._item2 == null : this._item2.Equals( other._item2 ) ) && ( this._item3 == null ? other._item3 == null : this._item3.Equals( other._item3 ) ) && ( this._item4 == null ? other._item4 == null : this._item4.Equals( other._item4 ) ) && ( this._item5 == null ? other._item5 == null : this._item5.Equals( other._item5 ) ) && ( this._item6 == null ? other._item6 == null : this._item6.Equals( other._item6 ) ) && ( this._item7 == null ? other._item7 == null : this._item7.Equals( other._item7 ) ) && ( this._item8 == null ? other._item8 == null : this._item8.Equals( other._item8 ) ) && ( this._item9 == null ? other._item9 == null : this._item9.Equals( other._item9 ) ) && ( this._item10 == null ? other._item10 == null : this._item10.Equals( other._item10 ) ) && ( this._item11 == null ? other._item11 == null : this._item11.Equals( other._item11 ) ) && ( this._item12 == null ? other._item12 == null : this._item12.Equals( other._item12 ) ) && ( this._item13 == null ? other._item13 == null : this._item13.Equals( other._item13 ) ) && ( this._item14 == null ? other._item14 == null : this._item14.Equals( other._item14 ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> right )
		{
			return !left.Equals( right );
		}		
	}
	/// <summary>
	/// 	Represents pair of values.
	/// </summary>
	[global::System.Runtime.InteropServices.StructLayout( global::System.Runtime.InteropServices.LayoutKind.Sequential )]
	[global::System.Serializable]
	public partial struct Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> : global::System.IEquatable<Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>>
				where T1 : IEquatable<T1>
				where T2 : IEquatable<T2>
				where T3 : IEquatable<T3>
				where T4 : IEquatable<T4>
				where T5 : IEquatable<T5>
				where T6 : IEquatable<T6>
				where T7 : IEquatable<T7>
				where T8 : IEquatable<T8>
				where T9 : IEquatable<T9>
				where T10 : IEquatable<T10>
				where T11 : IEquatable<T11>
				where T12 : IEquatable<T12>
				where T13 : IEquatable<T13>
				where T14 : IEquatable<T14>
				where T15 : IEquatable<T15>
	{
		/// <summary>
		/// 	Get empty instance.
		/// </summary>
		public static Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> Null { get { return default( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ); } }
		
		/// <summary>
		/// 	The item of #1.
		/// </summary>
		private readonly T1 _item1;
		
		/// <summary>
		/// 	Get the item of #1.
		/// </summary>
		/// <value>
		/// 	The item of #1.
		/// </value>
		public T1 Item1
		{
			get
			{
				return this._item1;
			}
		}
				
		/// <summary>
		/// 	The item of #2.
		/// </summary>
		private readonly T2 _item2;
		
		/// <summary>
		/// 	Get the item of #2.
		/// </summary>
		/// <value>
		/// 	The item of #2.
		/// </value>
		public T2 Item2
		{
			get
			{
				return this._item2;
			}
		}
				
		/// <summary>
		/// 	The item of #3.
		/// </summary>
		private readonly T3 _item3;
		
		/// <summary>
		/// 	Get the item of #3.
		/// </summary>
		/// <value>
		/// 	The item of #3.
		/// </value>
		public T3 Item3
		{
			get
			{
				return this._item3;
			}
		}
				
		/// <summary>
		/// 	The item of #4.
		/// </summary>
		private readonly T4 _item4;
		
		/// <summary>
		/// 	Get the item of #4.
		/// </summary>
		/// <value>
		/// 	The item of #4.
		/// </value>
		public T4 Item4
		{
			get
			{
				return this._item4;
			}
		}
				
		/// <summary>
		/// 	The item of #5.
		/// </summary>
		private readonly T5 _item5;
		
		/// <summary>
		/// 	Get the item of #5.
		/// </summary>
		/// <value>
		/// 	The item of #5.
		/// </value>
		public T5 Item5
		{
			get
			{
				return this._item5;
			}
		}
				
		/// <summary>
		/// 	The item of #6.
		/// </summary>
		private readonly T6 _item6;
		
		/// <summary>
		/// 	Get the item of #6.
		/// </summary>
		/// <value>
		/// 	The item of #6.
		/// </value>
		public T6 Item6
		{
			get
			{
				return this._item6;
			}
		}
				
		/// <summary>
		/// 	The item of #7.
		/// </summary>
		private readonly T7 _item7;
		
		/// <summary>
		/// 	Get the item of #7.
		/// </summary>
		/// <value>
		/// 	The item of #7.
		/// </value>
		public T7 Item7
		{
			get
			{
				return this._item7;
			}
		}
				
		/// <summary>
		/// 	The item of #8.
		/// </summary>
		private readonly T8 _item8;
		
		/// <summary>
		/// 	Get the item of #8.
		/// </summary>
		/// <value>
		/// 	The item of #8.
		/// </value>
		public T8 Item8
		{
			get
			{
				return this._item8;
			}
		}
				
		/// <summary>
		/// 	The item of #9.
		/// </summary>
		private readonly T9 _item9;
		
		/// <summary>
		/// 	Get the item of #9.
		/// </summary>
		/// <value>
		/// 	The item of #9.
		/// </value>
		public T9 Item9
		{
			get
			{
				return this._item9;
			}
		}
				
		/// <summary>
		/// 	The item of #10.
		/// </summary>
		private readonly T10 _item10;
		
		/// <summary>
		/// 	Get the item of #10.
		/// </summary>
		/// <value>
		/// 	The item of #10.
		/// </value>
		public T10 Item10
		{
			get
			{
				return this._item10;
			}
		}
				
		/// <summary>
		/// 	The item of #11.
		/// </summary>
		private readonly T11 _item11;
		
		/// <summary>
		/// 	Get the item of #11.
		/// </summary>
		/// <value>
		/// 	The item of #11.
		/// </value>
		public T11 Item11
		{
			get
			{
				return this._item11;
			}
		}
				
		/// <summary>
		/// 	The item of #12.
		/// </summary>
		private readonly T12 _item12;
		
		/// <summary>
		/// 	Get the item of #12.
		/// </summary>
		/// <value>
		/// 	The item of #12.
		/// </value>
		public T12 Item12
		{
			get
			{
				return this._item12;
			}
		}
				
		/// <summary>
		/// 	The item of #13.
		/// </summary>
		private readonly T13 _item13;
		
		/// <summary>
		/// 	Get the item of #13.
		/// </summary>
		/// <value>
		/// 	The item of #13.
		/// </value>
		public T13 Item13
		{
			get
			{
				return this._item13;
			}
		}
				
		/// <summary>
		/// 	The item of #14.
		/// </summary>
		private readonly T14 _item14;
		
		/// <summary>
		/// 	Get the item of #14.
		/// </summary>
		/// <value>
		/// 	The item of #14.
		/// </value>
		public T14 Item14
		{
			get
			{
				return this._item14;
			}
		}
				
		/// <summary>
		/// 	The item of #15.
		/// </summary>
		private readonly T15 _item15;
		
		/// <summary>
		/// 	Get the item of #15.
		/// </summary>
		/// <value>
		/// 	The item of #15.
		/// </value>
		public T15 Item15
		{
			get
			{
				return this._item15;
			}
		}
				
		/// <summary>
		/// 	Initialize new instance.
		/// </summary>
		/// <param name="item1">
		/// 	The item of #1.
		/// </param>
		/// <param name="item2">
		/// 	The item of #2.
		/// </param>
		/// <param name="item3">
		/// 	The item of #3.
		/// </param>
		/// <param name="item4">
		/// 	The item of #4.
		/// </param>
		/// <param name="item5">
		/// 	The item of #5.
		/// </param>
		/// <param name="item6">
		/// 	The item of #6.
		/// </param>
		/// <param name="item7">
		/// 	The item of #7.
		/// </param>
		/// <param name="item8">
		/// 	The item of #8.
		/// </param>
		/// <param name="item9">
		/// 	The item of #9.
		/// </param>
		/// <param name="item10">
		/// 	The item of #10.
		/// </param>
		/// <param name="item11">
		/// 	The item of #11.
		/// </param>
		/// <param name="item12">
		/// 	The item of #12.
		/// </param>
		/// <param name="item13">
		/// 	The item of #13.
		/// </param>
		/// <param name="item14">
		/// 	The item of #14.
		/// </param>
		/// <param name="item15">
		/// 	The item of #15.
		/// </param>
		public Pair(
			T1 item1,
			T2 item2,
			T3 item3,
			T4 item4,
			T5 item5,
			T6 item6,
			T7 item7,
			T8 item8,
			T9 item9,
			T10 item10,
			T11 item11,
			T12 item12,
			T13 item13,
			T14 item14,
			T15 item15
		)
		{
			this._item1 = item1;
			this._item2 = item2;
			this._item3 = item3;
			this._item4 = item4;
			this._item5 = item5;
			this._item6 = item6;
			this._item7 = item7;
			this._item8 = item8;
			this._item9 = item9;
			this._item10 = item10;
			this._item11 = item11;
			this._item12 = item12;
			this._item13 = item13;
			this._item14 = item14;
			this._item15 = item15;
		}
				
		/// <summary>
		/// 	Returns string representation of this instnace.
		/// </summary>
		/// <returns>
		/// 	String representation of this instance.
		/// </returns>
		public override string ToString()
		{
			return String.Format( FormatProviders.InvariantCulture, "{0}", new object[] { Item1, Item2, Item3, Item4, Item5, Item6, Item7, Item8, Item9, Item10, Item11, Item12, Item13, Item14, Item15 } as object );
		}
				
		/// <summary>
		/// 	Returns hash code of this instnace.
		/// </summary>
		/// <returns>
		/// 	Hash code of this instance.
		/// </returns>
		public override int GetHashCode()
		{
			return ( this._item1 == null ? 0 : this._item1.GetHashCode() ) ^ ( this._item2 == null ? 0 : this._item2.GetHashCode() ) ^ ( this._item3 == null ? 0 : this._item3.GetHashCode() ) ^ ( this._item4 == null ? 0 : this._item4.GetHashCode() ) ^ ( this._item5 == null ? 0 : this._item5.GetHashCode() ) ^ ( this._item6 == null ? 0 : this._item6.GetHashCode() ) ^ ( this._item7 == null ? 0 : this._item7.GetHashCode() ) ^ ( this._item8 == null ? 0 : this._item8.GetHashCode() ) ^ ( this._item9 == null ? 0 : this._item9.GetHashCode() ) ^ ( this._item10 == null ? 0 : this._item10.GetHashCode() ) ^ ( this._item11 == null ? 0 : this._item11.GetHashCode() ) ^ ( this._item12 == null ? 0 : this._item12.GetHashCode() ) ^ ( this._item13 == null ? 0 : this._item13.GetHashCode() ) ^ ( this._item14 == null ? 0 : this._item14.GetHashCode() ) ^ ( this._item15 == null ? 0 : this._item15.GetHashCode() );
		}
				
		/// <summary>
		/// 	Compare specified object is <see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15&gt;"/> and equal to this instnace.
		/// </summary>
		/// <param name="obj">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15&gt;"/> to compare.
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
			
			if( !( obj is Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> ) )
			{
				return false;
			}
			
			return this.Equals( ( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> )obj );
		}
				
		/// <summary>
		/// 	Compare specified object is equal to this instnace.
		/// </summary>
		/// <param name="other">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified instance equals to this instance then true.
		/// </returns>
		public bool Equals( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> other )
		{
			return ( this._item1 == null ? other._item1 == null : this._item1.Equals( other._item1 ) ) && ( this._item2 == null ? other._item2 == null : this._item2.Equals( other._item2 ) ) && ( this._item3 == null ? other._item3 == null : this._item3.Equals( other._item3 ) ) && ( this._item4 == null ? other._item4 == null : this._item4.Equals( other._item4 ) ) && ( this._item5 == null ? other._item5 == null : this._item5.Equals( other._item5 ) ) && ( this._item6 == null ? other._item6 == null : this._item6.Equals( other._item6 ) ) && ( this._item7 == null ? other._item7 == null : this._item7.Equals( other._item7 ) ) && ( this._item8 == null ? other._item8 == null : this._item8.Equals( other._item8 ) ) && ( this._item9 == null ? other._item9 == null : this._item9.Equals( other._item9 ) ) && ( this._item10 == null ? other._item10 == null : this._item10.Equals( other._item10 ) ) && ( this._item11 == null ? other._item11 == null : this._item11.Equals( other._item11 ) ) && ( this._item12 == null ? other._item12 == null : this._item12.Equals( other._item12 ) ) && ( this._item13 == null ? other._item13 == null : this._item13.Equals( other._item13 ) ) && ( this._item14 == null ? other._item14 == null : this._item14.Equals( other._item14 ) ) && ( this._item15 == null ? other._item15 == null : this._item15.Equals( other._item15 ) );
		}
		
		/// <summary>
		/// 	Compare specified two objects are equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are equal then true.
		/// </returns>
		public static bool operator ==( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> right )
		{
			return left.Equals( right );
		}
		
		/// <summary>
		/// 	Compare specified two objects are not equal.
		/// </summary>
		/// <param name="left">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15&gt;"/> to compare.
		/// </param>
		/// <param name="right">
		/// 	<see cref="Pair&lt;T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15&gt;"/> to compare.
		/// </param>
		/// <returns>
		/// 	If specified objects are not equal then true.
		/// </returns>
		public static bool operator !=( Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> left, Pair<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> right )
		{
			return !left.Equals( right );
		}		
	}
}