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
	[TestFixture]
	public class InvokerTest
	{
		private static readonly ConsoleTraceListener _traceListener = new ConsoleTraceListener();
		private static SourceLevels _originalSourceSwitchLevel;

		[TestFixtureSetUp]
		public static void ClassSetUp()
		{
			//ShimCodeGenerator.IsTracing = true;
			//GeneratedCodeHelper.Instance.IsTracingEnabled = true;

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
		public void TestInvokeStatic()
		{
			Invoker.InvokeStatic( TestClass.MayBeAction0Method );
			Invoker.CreateActionInvoker( TestClass.MayBeAction0Method )( null, null );

			Invoker.InvokeStatic( TestClass.MayBeAction16Method, 0, 0, 0, 0, 0, 0, 0, 0, false, '0', null, null, IntPtr.Zero, 0m, null, null );
			Invoker.CreateActionInvoker( TestClass.MayBeAction16Method )( null, new object[] { 0, 0, 0, 0, 0, 0, 0, 0, false, '0', null, null, IntPtr.Zero, 0m, null, null } );

			Assert.That( Invoker.InvokeStatic( TestClass.MayBeFunc1Method ), Is.EqualTo( "OK" ) );
			Assert.That( Invoker.CreateFuncInvoker( TestClass.MayBeFunc1Method )( null, null ), Is.EqualTo( "OK" ) );

			var str = Guid.NewGuid().ToString();
			Assert.That( Invoker.InvokeStatic( TestClass.MayBeFunc17Method, 0, 0, 0, 0, 0, 0, 0, 0, false, '0', null, str, IntPtr.Zero, 0m, null, null ), Is.EqualTo( "OK:" + str ) );
			Assert.That( Invoker.CreateFuncInvoker( TestClass.MayBeFunc17Method )( null, new object[] { 0, 0, 0, 0, 0, 0, 0, 0, false, '0', null, str, IntPtr.Zero, 0m, null, null } ), Is.EqualTo( "OK:" + str ) );
		}

		[Test]
		public void TestInvoke()
		{
			var str = Guid.NewGuid().ToString();
			Assert.That( Invoker.Invoke( TestClass.InstanceMethod, new TestClass(), str ), Is.EqualTo( "OK:" + str ) );
			Assert.That( Invoker.CreateFuncInvoker( TestClass.InstanceMethod )( new TestClass(), new object[] { str } ), Is.EqualTo( "OK:" + str ) );
		}

		[Test]
		public void TestInvokeStatic_ValueType()
		{
			Assert.That( Invoker.InvokeStatic( TestClass.GetMaxMethod ), Is.EqualTo( DateTime.MaxValue ) );
			Assert.That( Invoker.CreateFuncInvoker( TestClass.GetMaxMethod )( null, null ), Is.EqualTo( DateTime.MaxValue ) );
		}

		[Test]
		public void TestInvokeStatic_Private()
		{
			Assert.That( Invoker.InvokeStatic( TestClass.PrivateMethod, typeof( TestClass ) ), Is.EqualTo( "OK" ) );
			Assert.That( Invoker.CreateFuncInvoker( TestClass.PrivateMethod, typeof( TestClass ) )( null, null ), Is.EqualTo( "OK" ) );
		}

		[Test]
		public void TestInvoke_Interface()
		{
			Assert.That( Invoker.Invoke( TestClass.InterfaceMethod, typeof( TestClass ), new TestClass() ), Is.EqualTo( "OK" ) );
			Assert.That( Invoker.CreateFuncInvoker( TestClass.InterfaceMethod, typeof( TestClass ) )( new TestClass(), null ), Is.EqualTo( "OK" ) );
		}

		[Test]
		public void TestInvoke_Override()
		{
			Assert.That( Invoker.Invoke( TestClass.OverrideMethod, new TestClass() ), Is.EqualTo( "OK" ) );
			Assert.That( Invoker.CreateFuncInvoker( TestClass.OverrideMethod )( new TestClass(), null ), Is.EqualTo( "OK" ) );
		}

		[Test]
		public void TestInvoke_Inherit()
		{
			Assert.That( Invoker.Invoke( TestClass.InheritMethod, new TestClass() ), Is.EqualTo( "OK" ) );
			Assert.That( Invoker.CreateFuncInvoker( TestClass.InheritMethod )( new TestClass(), null ), Is.EqualTo( "OK" ) );
		}

		[Test]
		public void TestInvokeConstructor()
		{
			var result = Invoker.InvokeConstructor( TestClass.Ctor_String, "OK" ) as TestClass;
			Assert.That( result.ConstructorArgument, Is.EqualTo( "OK" ) );
			result = Invoker.CreateFuncInvoker( TestClass.Ctor_String )( null, new object[] { "OK" } ) as TestClass;
			Assert.That( result.ConstructorArgument, Is.EqualTo( "OK" ) );
		}

		[Test]
		public void TestInvokeStatic_Exception()
		{
			var str = Guid.NewGuid().ToString();
			try
			{
				Invoker.InvokeStatic( TestClass.ThrowExceptionMethod, new ApplicationException( str ) );
				Assert.Fail();
			}
			catch ( ApplicationException ex )
			{
				Assert.That( ex.Message, Is.EqualTo( str ) );
			}

			try
			{
				Invoker.CreateActionInvoker( TestClass.ThrowExceptionMethod )( null, new object[] { new ApplicationException( str ) } );
				Assert.Fail();
			}
			catch ( ApplicationException ex )
			{
				Assert.That( ex.Message, Is.EqualTo( str ) );
			}
		}
	}
}
