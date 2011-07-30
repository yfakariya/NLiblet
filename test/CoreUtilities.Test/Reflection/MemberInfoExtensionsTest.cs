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
using System.Diagnostics;
using NUnit.Framework;

namespace NLiblet.Reflection
{
#if DEBUG
	[TestFixture]
	public class MemberInfoExtensionsTest
	{
		private static readonly ConsoleTraceListener _traceListener = new ConsoleTraceListener();
		private static SourceLevels _originalSourceSwitchLevel;

		[TestFixtureSetUp]
		public static void ClassSetUp()
		{
			//ShimCodeGenerator.IsTracing = true;
			//GeneratedCodeHelper.Instance.IsTracingEnabled = true

			_originalSourceSwitchLevel = ILEmittion.TraceSource.Switch.Level;
			//ILEmittion.TraceSource.Switch.Level = SourceLevels.Verbose;
			ILEmittion.TraceSource.Listeners.Add( _traceListener );
		}

		[TestFixtureTearDown]
		public static void ClassCleanUp()
		{
			ShimCodeGenerator.IsTracing = false;
			ILEmittion.TraceSource.Listeners.Remove( _traceListener );
			ILEmittion.TraceSource.Switch.Level = _originalSourceSwitchLevel;
		}

		[Test]
		public void TestCreateDelegate_Static()
		{
			var action0 = TestClass.MayBeAction0Method.CreateDelegate() as Action;
			Assert.IsNotNull( action0 );
			action0();

			var action16 = TestClass.MayBeAction16Method.CreateDelegate()
				as Action<byte, short, int, long, sbyte, ushort, uint, ulong, bool, char, object, string, IntPtr, decimal, Uri, object[]>;
			Assert.IsNotNull( action16 );
			action16( 0, 0, 0, 0, 0, 0, 0, 0, false, '0', null, null, IntPtr.Zero, 0, null, null );

			var func1 = TestClass.MayBeFunc1Method.CreateDelegate() as Func<string>;
			Assert.IsNotNull( func1 );
			Assert.That( func1(), Is.EqualTo( "OK" ) );

			var func17 = TestClass.MayBeFunc17Method.CreateDelegate()
				as Func<byte, short, int, long, sbyte, ushort, uint, ulong, bool, char, object, string, IntPtr, decimal, Uri, object[], string>;
			Assert.IsNotNull( action16 );
			var str = Guid.NewGuid().ToString();
			Assert.That( func17( 0, 0, 0, 0, 0, 0, 0, 0, false, '0', null, str, IntPtr.Zero, 0, null, null ), Is.EqualTo( "OK:" + str ) );
		}

		[Test]
		public void TestCreateDelegate_Instance()
		{
			var str = Guid.NewGuid().ToString();
			var target = TestClass.InstanceMethod.CreateDelegate() as Func<TestClass, string, string>;
			Assert.IsNotNull( target );
			Assert.That( target( new TestClass(), str ), Is.EqualTo( "OK:" + str ) );
		}

		[Test]
		public void TestCreateDelegate_ValueType()
		{
			var target = TestClass.GetMaxMethod.CreateDelegate() as Func<DateTime>;
			Assert.IsNotNull( target );
			Assert.That( target(), Is.EqualTo( DateTime.MaxValue ) );
		}

		[Test]
		public void TestCreateDelegate_Private()
		{
			var target = TestClass.PrivateMethod.CreateDelegate( typeof( TestClass ) ) as Func<string>;
			Assert.IsNotNull( target );
			Assert.That( target(), Is.EqualTo( "OK" ) );
		}

		[Test]
		public void TestCreateDelegate_Interface()
		{
			var target = TestClass.InterfaceMethod.CreateDelegate( typeof( TestClass ) );
			Assert.IsNotNull( target );
			Assert.That( target, Is.TypeOf<Func<TestClass, string>>() );
			Assert.That( ( target as Func<TestClass, string> )( new TestClass() ), Is.EqualTo( "OK" ) );
		}

		[Test]
		public void TestCreateDelegate_Override()
		{
			var target = TestClass.OverrideMethod.CreateDelegate();
			Assert.IsNotNull( target );
			Assert.That( target, Is.TypeOf<Func<TestClass, string>>() );
			Assert.That( ( target as Func<TestClass, string> )( new TestClass() ), Is.EqualTo( "OK" ) );
		}

		[Test]
		public void TestCreateDelegate_Inherit()
		{
			var target = TestClass.InheritMethod.CreateDelegate();
			Assert.IsNotNull( target );
			Assert.That( target, Is.TypeOf<Func<TestAbstractClass, string>>() );
			Assert.That( ( target as Func<TestClass, string> )( new TestClass() ), Is.EqualTo( "OK" ) );
		}

		[Test]
		[ExpectedException( typeof( NotSupportedException ) )]
		public void TestCreateDelegate_TypeInitializer()
		{
			TestClass.TypeInitializer.CreateDelegate();
		}

		[Test]
		public void TestCreateDelegate_Ctor()
		{
			var target = TestClass.Ctor_String.CreateDelegate();
			Assert.IsNotNull( target );
			Assert.That( target, Is.TypeOf<Func<string, TestClass>>() );
			var result = ( target as Func<string, TestClass> )( "OK" );
			Assert.That( result.ConstructorArgument, Is.EqualTo( "OK" ) );
		}

		[Test]
		public void TestCreateDelegate_Exception()
		{
			var target = TestClass.ThrowExceptionMethod.CreateDelegate() as Action<Exception>;
			Assert.IsNotNull( target );
			var str = Guid.NewGuid().ToString();
			try
			{
				target( new ApplicationException( str ) );
				Assert.Fail();
			}
			catch ( ApplicationException ex )
			{
				Assert.That( ex.Message, Is.EqualTo( str ) );
			}
		}
	}
#endif
}
