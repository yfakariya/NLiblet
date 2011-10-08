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
using NUnit.Framework;
using System.Threading;

namespace NLiblet.Async
{
	[TestFixture]
	[Timeout( 1000 )]
	public class NestedProgressTest
	{
		[Test]
		public void TestNumericProgress()
		{
			using ( var waitHandle = new ManualResetEventSlim() )
			{
				double reported = 0.0;
				var parent =
					new Progress<double>( item =>
						{
							reported += item;
							waitHandle.Set();
						}
					);
				var target = NestedProgress.Numeric( parent, 0.5 );
				target.Report( 0.1 );
				waitHandle.Wait();
				Assert.That( reported, Is.EqualTo( 0.05 ) );
				waitHandle.Reset();
				target.Report( 0.1 );
				waitHandle.Wait();
				Assert.That( reported, Is.EqualTo( 0.1 ) );
			}
		}

		[Test]
		public void TestCustomProgress()
		{
			using ( var waitHandle = new ManualResetEventSlim() )
			{
				float reported = 0.0f;
				var parent =
					new Progress<float>( item =>
						{
							reported += item;
							waitHandle.Set();
						}
					);
				var target = NestedProgress.Of( parent, 0.5f, ( arg0, arg1 ) => arg0 * arg1 );
				target.Report( 0.1f );
				waitHandle.Wait();
				Assert.That( reported, Is.EqualTo( 0.05f ) );
				waitHandle.Reset();
				target.Report( 0.1f );
				waitHandle.Wait();
				Assert.That( reported, Is.EqualTo( 0.1f ) );
			}
		}

		[Test]
		public void TestDefaultProgressNumeric()
		{
			TestDefaultProgressCore( 1, 2, ( left, right ) => left + right, 2, 4 );
		}

		[Test]
		public void TestDefaultProgressNullableNumeric()
		{
			TestDefaultProgressCoreNullable<int>( 1, 2, ( left, right ) => left + right, 2, 4 );
		}

		[Test]
		public void TestDefaultProgressOperator()
		{
			TestDefaultProgressCore<HasOperator>( 1, 2, ( left, right ) => left + right, 2, 4 );
		}

		[Test]
		public void TestDefaultProgressNullableOperator()
		{
			TestDefaultProgressCoreNullable<HasOperator>( 1, 2, ( left, right ) => left + right, 2, 4 );
		}

		[Test]
		public void TestDefaultProgressStaticMultiply()
		{
			TestDefaultProgressCore<HasStaticMultply>( 1, 2, ( left, right ) => left + right, 2, 4 );
		}

		[Test]
		public void TestDefaultProgressNullableStaticMultiply()
		{
			TestDefaultProgressCoreNullable<HasStaticMultply>( 1, 2, ( left, right ) => left + right, 2, 4 );
		}

		[Test]
		public void TestDefaultProgressMultiply()
		{
			TestDefaultProgressCore<HasInstanceMultply>( 1, 2, ( left, right ) => left + right, 2, 4 );
		}

		[Test]
		public void TestDefaultProgressNullableMultiply()
		{
			TestDefaultProgressCoreNullable<HasInstanceMultply>( 1, 2, ( left, right ) => left + right, 2, 4 );
		}

		[Test]
		public void TestDefaultProgressStaticMultiplyPolymorphic()
		{
			TestDefaultProgressCore<HasStaticMultplyPolymorphic>( 1, 2, ( left, right ) => left + right, 2, 4 );
		}

		[Test]
		public void TestDefaultProgressMultiplyPolymorphic()
		{
			TestDefaultProgressCore<HasInstanceMultplyPolymorphic>( 1, 2, ( left, right ) => left + right, 2, 4 );
		}

		private static void TestDefaultProgressCore<T>( T value, T factor, Func<T, T, T> addition, params T[] expecteds )
		{
			using ( var waitHandle = new ManualResetEventSlim() )
			{
				T reported = default( T );
				var parent =
					new Progress<T>( item =>
						{
							reported = addition( reported, item );
							waitHandle.Set();
						}
					);
				var target = NestedProgress.Of( parent, factor );

				foreach ( var expected in expecteds )
				{
					target.Report( value );
					waitHandle.Wait();
					Assert.That( reported, Is.EqualTo( expected ) );
					waitHandle.Reset();
				}
			}
		}

		private static void TestDefaultProgressCoreNullable<T>( T? value, T? factor, Func<T?, T?, T?> addition, params T?[] expecteds )
			where T : struct
		{
			using ( var waitHandle = new ManualResetEventSlim() )
			{
				T? reported = default( T );
				var parent =
					new Progress<T?>( item =>
						{
							reported = addition( reported, item );
							waitHandle.Set();
						}
					);
				var target = NestedProgress.Of( parent, factor );

				foreach ( var expected in expecteds )
				{
					target.Report( value );
					waitHandle.Wait();
					Assert.That( reported, Is.EqualTo( expected ) );
					waitHandle.Reset();
				}
			}
		}

		private struct HasOperator
		{
			readonly int _value;

			public HasOperator( int value )
			{
				this._value = value;
			}

			public override string ToString()
			{
				return this._value.ToString();
			}

			public override bool Equals( object obj )
			{
				if ( !( obj is HasOperator ) )
				{
					return false;
				}

				return this._value.Equals( ( ( HasOperator )obj )._value );
			}

			public override int GetHashCode()
			{
				return this._value.GetHashCode();
			}

			public static HasOperator operator +( HasOperator left, HasOperator right )
			{
				return new HasOperator( left._value + right._value );
			}

			public static HasOperator operator *( HasOperator left, HasOperator right )
			{
				return new HasOperator( left._value * right._value );
			}

			public static implicit operator HasOperator( int value )
			{
				return new HasOperator( value );
			}
		}

		private struct HasStaticMultply
		{
			readonly int _value;

			public HasStaticMultply( int value )
			{
				this._value = value;
			}

			public override string ToString()
			{
				return this._value.ToString();
			}

			public override bool Equals( object obj )
			{
				if ( !( obj is HasStaticMultply ) )
				{
					return false;
				}

				return this._value.Equals( ( ( HasStaticMultply )obj )._value );
			}

			public override int GetHashCode()
			{
				return this._value.GetHashCode();
			}
			public static HasStaticMultply Multiply( HasStaticMultply left, HasStaticMultply right )
			{
				return new HasStaticMultply( left._value * right._value );
			}

			public static HasStaticMultply operator +( HasStaticMultply left, HasStaticMultply right )
			{
				return new HasStaticMultply( left._value + right._value );
			}

			public static implicit operator HasStaticMultply( int value )
			{
				return new HasStaticMultply( value );
			}
		}

		private struct HasInstanceMultply
		{
			readonly int _value;

			public HasInstanceMultply( int value )
			{
				this._value = value;
			}

			public override string ToString()
			{
				return this._value.ToString();
			}

			public override bool Equals( object obj )
			{
				if ( !( obj is HasInstanceMultply ) )
				{
					return false;
				}

				return this._value.Equals( ( ( HasInstanceMultply )obj )._value );
			}

			public override int GetHashCode()
			{
				return this._value.GetHashCode();
			}

			public HasInstanceMultply Multiply( HasInstanceMultply other )
			{
				return new HasInstanceMultply( this._value * other._value );
			}

			public static HasInstanceMultply operator +( HasInstanceMultply left, HasInstanceMultply right )
			{
				return new HasInstanceMultply( left._value + right._value );
			}

			public static implicit operator HasInstanceMultply( int value )
			{
				return new HasInstanceMultply( value );
			}
		}

		private abstract class PolymorphicBase
		{
			protected internal readonly int Value;

			protected PolymorphicBase( int value )
			{
				this.Value = value;
			}

			public override string ToString()
			{
				return this.Value.ToString();
			}

			public override bool Equals( object obj )
			{
				var other = obj as PolymorphicBase;
				if ( Object.ReferenceEquals( other, null ) )
				{
					return false;
				}

				return this.Value.Equals( other.Value );
			}

			public override int GetHashCode()
			{
				return this.Value.GetHashCode();
			}
		}

		private class HasStaticMultplyPolymorphic : PolymorphicBase
		{
			public HasStaticMultplyPolymorphic( int value ) : base( value ) { }

			public static HasStaticMultplyPolymorphicDerived Multiply( PolymorphicBase left, PolymorphicBase right )
			{
				return new HasStaticMultplyPolymorphicDerived( ( left == null ? 0 : left.Value ) * ( right == null ? 0 : right.Value ) );
			}

			public static HasStaticMultplyPolymorphic operator +( HasStaticMultplyPolymorphic left, HasStaticMultplyPolymorphic right )
			{
				return new HasStaticMultplyPolymorphic( ( left == null ? 0 : left.Value ) + ( right == null ? 0 : right.Value ) );
			}

			public static implicit operator HasStaticMultplyPolymorphic( int value )
			{
				return new HasStaticMultplyPolymorphic( value );
			}
		}

		private sealed class HasStaticMultplyPolymorphicDerived : HasStaticMultplyPolymorphic
		{
			public HasStaticMultplyPolymorphicDerived( int value ) : base( value ) { }
		}

		private class HasInstanceMultplyPolymorphic : PolymorphicBase
		{
			public HasInstanceMultplyPolymorphic( int value ) : base( value ) { }

			public HasInstanceMultplyPolymorphicDerived Multiply( PolymorphicBase other )
			{
				return new HasInstanceMultplyPolymorphicDerived( this.Value * ( other == null ? 0 : other.Value  ));
			}

			public static HasInstanceMultplyPolymorphic operator +( HasInstanceMultplyPolymorphic left, HasInstanceMultplyPolymorphic right )
			{
				return new HasInstanceMultplyPolymorphic( ( left == null ? 0 : left.Value ) + ( right == null ? 0 : right.Value ) );
			}

			public static implicit operator HasInstanceMultplyPolymorphic( int value )
			{
				return new HasInstanceMultplyPolymorphic( value );
			}
		}

		private sealed class HasInstanceMultplyPolymorphicDerived : HasInstanceMultplyPolymorphic
		{
			public HasInstanceMultplyPolymorphicDerived( int value ) : base( value ) { }
		}
	}
}
