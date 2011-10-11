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
using NLiblet;

namespace NLiblet.ServiceLocators
{
	[TestFixture]
	public sealed class ServiceLocatorTest
	{
#if DEBUG
		private static readonly ConsoleTraceListener _listener = new ConsoleTraceListener();
		private static SourceLevels _previousLevel;
#endif

		[TestFixtureSetUp]
		public static void FixtureSetup()
		{
#if DEBUG
			var source = ServiceLocator.DebugTrace;
			_previousLevel = source.Switch.Level;
			source.Switch.Level = SourceLevels.Error;
			source.Listeners.Add( _listener );
#endif
		}

		[TestFixtureTearDown]
		public static void FixtureTearDown()
		{
#if DEBUG
			var source = ServiceLocator.DebugTrace;
			source.Listeners.Remove( _listener );
			source.Switch.Level = _previousLevel;
#endif
		}

		[Test]
		public void TestStatics()
		{
			Assert.IsNotNull( ServiceLocator.Default );
			Assert.AreSame( ServiceLocator.Default, ServiceLocator.Instance );
			var token = Guid.NewGuid().ToString();
			var replacement = new ServiceLocator( token );
			ServiceLocator.SetInstance( replacement );
			Assert.AreNotSame( ServiceLocator.Default, ServiceLocator.Instance );
			Assert.AreEqual( token, ServiceLocator.Instance.ToString() );
			ServiceLocator.ResetToDefault();
			Assert.AreSame( ServiceLocator.Default, ServiceLocator.Instance );
		}

		[Test]
		public void TestSingleton()
		{
			var target = new ServiceLocator();
			Assert.IsTrue( target.RegisterSingleton( typeof( MarshalByRefObject ), new ConsoleTraceListener() ) );
			Assert.IsNotNull( target.GetSingleton<MarshalByRefObject>() );
			Assert.AreSame(
				target.GetSingleton<MarshalByRefObject>(),
				target.GetSingleton<MarshalByRefObject>()
			);

			Assert.IsFalse( target.RegisterSingleton( typeof( MarshalByRefObject ), new EventLogTraceListener() ) );

			Assert.IsTrue( target.RegisterSingleton( typeof( TraceListener ), () => new ConsoleTraceListener( true ) ) );
			Assert.IsNotNull( target.GetSingleton<TraceListener>() );
			Assert.AreSame(
				target.GetSingleton<TraceListener>(),
				target.GetSingleton<TraceListener>()
			);
			Assert.AreSame( Console.Error, ( ( ConsoleTraceListener )target.GetSingleton<TraceListener>() ).Writer );
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestGet_NotRegistered_WithoutDefault()
		{
			new ServiceLocator().Get<TraceListener>();
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestGetSingleton_NotRegistered_WithoutDefault()
		{
			new ServiceLocator().GetSingleton<TraceListener>();
		}

		[Test]
		public void TestGet_NotRegistered_WithDefault()
		{
			var target = new ServiceLocator();
			var result1 = target.Get<HasDefaultImplementationOnlyDefaultConstructorContract>();
			Assert.That( result1, Is.Not.Null );
			Assert.That( result1 is HasDefaultImplementationOnlyDefaultConstructor );
			var result2 = target.Get<HasDefaultImplementationNoDefaultConstructorContract>( 0 );
			Assert.That( result2, Is.Not.Null );
			Assert.That( result2 is HasDefaultImplementationNoDefaultConstructor );
			var result3 = target.Get<HasDefaultImplementationNoDefaultConstructorContract>( 0, 1 );
			Assert.That( result3, Is.Not.Null );
			Assert.That( result3 is HasDefaultImplementationNoDefaultConstructor );
			var result4 = target.Get<GenericServiceContract<DateTime>>();
			Assert.That( result4, Is.Not.Null );
			Assert.That( result4 is GenericService<DateTime> );
			var result5 = target.Get<GenericServiceContract<int, DateTime>>();
			Assert.That( result5, Is.Not.Null );
			Assert.That( result5 is GenericService<int, DateTime> );
			var result6 = target.Get<GenericServiceClosedContract<object>>();
			Assert.That( result6, Is.Not.Null );
			Assert.That( result6 is GenericServiceClosed );
		}

		[Test]
		public void TestGetSingleton_NotRegistered_WithDefault()
		{
			var target = new ServiceLocator();
			var result1 = target.GetSingleton<HasDefaultImplementationOnlyDefaultConstructorContract>();
			Assert.That( result1, Is.Not.Null );
			Assert.That( result1 is HasDefaultImplementationOnlyDefaultConstructor );
			var result2 = target.GetSingleton<GenericServiceContract<DateTime>>();
			Assert.That( result2, Is.Not.Null );
			Assert.That( result2 is GenericService<DateTime> );
			var result3 = target.GetSingleton<GenericServiceContract<int, DateTime>>();
			Assert.That( result3, Is.Not.Null );
			Assert.That( result3 is GenericService<int, DateTime> );
			var result4 = target.GetSingleton<GenericServiceClosedContract<object>>();
			Assert.That( result4, Is.Not.Null );
			Assert.That( result4 is GenericServiceClosed );
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ), ExpectedMessage = "((?i)constructor)", MatchType = MessageMatch.Regex )]
		[SetUICulture( "en-us" )]
		public void TestGet_WithDefault_DoesNotHaveAppropriateConstructor_Default()
		{
			var target = new ServiceLocator();
			target.Get<HasDefaultImplementationNoDefaultConstructorContract>();
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ), ExpectedMessage = "((?i)constructor)", MatchType = MessageMatch.Regex )]
		[SetUICulture( "en-us" )]
		public void TestGet_WithDefault_DoesNotHaveAppropriateConstructor_NotPublic()
		{
			var target = new ServiceLocator();
			target.Get<HasDefaultImplementationOnlyNonPublicDefaultConstructorContract>();
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ), ExpectedMessage = "((?i)constructor)", MatchType = MessageMatch.Regex )]
		[SetUICulture( "en-us" )]
		public void TestGet_WithDefault_DoesNotHaveAppropriateConstructor_WithParameters()
		{
			var target = new ServiceLocator();
			target.Get<HasDefaultImplementationOnlyDefaultConstructorContract>( 0 );
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ), ExpectedMessage = "((?i)is not assignable)", MatchType = MessageMatch.Regex )]
		[SetUICulture( "en-us" )]
		public void TestGet_WithDefault_Generic_DefaultIsClosedButIncompatible()
		{
			var target = new ServiceLocator();
			target.Get<GenericServiceClosedContract<string>>();
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ), ExpectedMessage = "((?i)partially constructed)", MatchType = MessageMatch.Regex )]
		[SetUICulture( "en-us" )]
		public void TestGet_WithDefault_Generic_DefaultIsPartialClosed()
		{
			var target = new ServiceLocator();
			target.Get<GenericServiceClosedContract<object, string>>();
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ), ExpectedMessage = "((?i)cannot be open constructed generic)", MatchType = MessageMatch.Regex )]
		[SetUICulture( "en-us" )]
		public void TestGet_WithDefault_Generic_DefaultIsGenericButContractIsNot()
		{
			var target = new ServiceLocator();
			target.Get<GenericServiceOpeningContract>();
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ), ExpectedMessage = "((?i)constructor)", MatchType = MessageMatch.Regex )]
		[SetUICulture( "en-us" )]
		public void TestGetSingleton_WithDefault_DoesNotHaveAppropriateConstructor_Default()
		{
			var target = new ServiceLocator();
			target.GetSingleton<HasDefaultImplementationNoDefaultConstructorContract>();
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ), ExpectedMessage = "((?i)constructor)", MatchType = MessageMatch.Regex )]
		[SetUICulture( "en-us" )]
		public void TestGetSingleton_WithDefault_DoesNotHaveAppropriateConstructor_NonPublic()
		{
			var target = new ServiceLocator();
			target.GetSingleton<HasDefaultImplementationOnlyNonPublicDefaultConstructorContract>();
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ), ExpectedMessage = "((?i)partially constructed)", MatchType = MessageMatch.Regex )]
		[SetUICulture( "en-us" )]
		public void TestGetSingleton_WithDefault_Generic_DefaultIsPartialClosed()
		{
			var target = new ServiceLocator();
			target.GetSingleton<GenericServiceClosedContract<object, string>>();
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ), ExpectedMessage = "((?i)is not assignable)", MatchType = MessageMatch.Regex )]
		[SetUICulture( "en-us" )]
		public void TestGetSingleton_WithDefault_Generic_DefaultIsClosedButIncompatible()
		{
			var target = new ServiceLocator();
			target.GetSingleton<GenericServiceClosedContract<string>>();
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ), ExpectedMessage = "((?i)cannot be open constructed generic)", MatchType = MessageMatch.Regex )]
		[SetUICulture( "en-us" )]
		public void TestGetSingleton_WithDefault_Generic_DefaultIsGenericButContractIsNot()
		{
			var target = new ServiceLocator();
			target.GetSingleton<GenericServiceOpeningContract>();
		}


		// TODO: [DefaultImplementationFactory(factoryType)] ?
		[Test]
		public void TestGetRegisteredServices()
		{
			var target = new ServiceLocator();
			Assert.That( target.GetRegisteredServices().Length, Is.EqualTo( 0 ) );
			Assert.That( target.GetRegisteredSingletonServices().Length, Is.EqualTo( 0 ) );
			target.RegisterFactory( typeof( TraceListener ), () => new ConsoleTraceListener() );
			Assert.That( target.GetRegisteredServices().Length, Is.EqualTo( 1 ) );
			Assert.That( target.GetRegisteredServices()[ 0 ], Is.EqualTo( typeof( TraceListener ) ) );
			Assert.That( target.GetRegisteredSingletonServices().Length, Is.EqualTo( 0 ) );
			var dummy1 = target.Get<HasDefaultImplementationOnlyDefaultConstructorContract>();
			Assert.That( target.GetRegisteredServices().Length, Is.EqualTo( 2 ) );
			Assert.That( target.GetRegisteredServices(), Contains.Item( typeof( TraceListener ) ) );
			Assert.That( target.GetRegisteredServices(), Contains.Item( typeof( HasDefaultImplementationOnlyDefaultConstructorContract ) ) );
			Assert.That( target.GetRegisteredSingletonServices().Length, Is.EqualTo( 0 ) );
			var dummy2 = target.Get<HasDefaultImplementationNoDefaultConstructorContract>( 0 );
			Assert.That( target.GetRegisteredServices().Length, Is.EqualTo( 3 ) );
			Assert.That( target.GetRegisteredServices(), Contains.Item( typeof( TraceListener ) ) );
			Assert.That( target.GetRegisteredServices(), Contains.Item( typeof( HasDefaultImplementationOnlyDefaultConstructorContract ) ) );
			Assert.That( target.GetRegisteredServices(), Contains.Item( typeof( HasDefaultImplementationNoDefaultConstructorContract ) ) );
			Assert.That( target.GetRegisteredSingletonServices().Length, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestGetRegisteredSingletonServices()
		{
			var target = new ServiceLocator();
			Assert.That( target.GetRegisteredSingletonServices().Length, Is.EqualTo( 0 ) );
			Assert.That( target.GetRegisteredServices().Length, Is.EqualTo( 0 ) );
			target.RegisterSingleton( typeof( TraceListener ), () => new ConsoleTraceListener() );
			Assert.That( target.GetRegisteredSingletonServices().Length, Is.EqualTo( 1 ) );
			Assert.That( target.GetRegisteredSingletonServices()[ 0 ], Is.EqualTo( typeof( TraceListener ) ) );
			Assert.That( target.GetRegisteredServices().Length, Is.EqualTo( 0 ) );
			var dummy1 = target.GetSingleton<HasDefaultImplementationOnlyDefaultConstructorContract>();
			Assert.That( target.GetRegisteredSingletonServices().Length, Is.EqualTo( 2 ) );
			Assert.That( target.GetRegisteredSingletonServices(), Contains.Item( typeof( TraceListener ) ) );
			Assert.That( target.GetRegisteredSingletonServices(), Contains.Item( typeof( HasDefaultImplementationOnlyDefaultConstructorContract ) ) );
			Assert.That( target.GetRegisteredServices().Length, Is.EqualTo( 0 ) );
		}

		[Test]
		public void TestFactory()
		{
			var target = new ServiceLocator();
			object expected = new object();
			object result = null;
			Assert.IsTrue(
				target.RegisterFactory(
					typeof( TraceListener ),
					args =>
					{
						Assert.AreSame( expected, args[ 0 ] );
						result = new ConsoleTraceListener( ( bool )args[ 1 ] );
						return result;
					}
				)
			);
			var returned = target.Get<TraceListener>( expected, true );
			Assert.AreSame( result, returned );
		}

		[Test]
		[ExpectedException( typeof( IntendedException ), ExpectedMessage = "DUMMY", MatchType = MessageMatch.Exact )] // thanks to IL shim, NOT TargetInvocationException
		public void TestFactoryThrow()
		{
			var target = new ServiceLocator();
			Assert.IsTrue(
				target.RegisterFactory(
					typeof( TraceListener ),
					args =>
					{
						throw new IntendedException( "DUMMY" );
					}
				)
			);
			target.Get<TraceListener>();
		}

		[Test]
		public void TestTypedFactory()
		{
			var target = new ServiceLocator();
			Func<bool, TraceListener> func = useErrorStream => new ConsoleTraceListener( useErrorStream );
			Assert.IsTrue( target.RegisterFactory( typeof( TraceListener ), func ) );

			Assert.AreSame( Console.Error, ( ( ConsoleTraceListener )target.Get<TraceListener>( true ) ).Writer );
			Assert.AreSame( Console.Out, ( ( ConsoleTraceListener )target.Get<TraceListener>( false ) ).Writer );
		}

		[Test]
		[ExpectedException( typeof( ArgumentException ), ExpectedMessage = @"arguments\[\s*[0-9]+\s*\]", MatchType = MessageMatch.Regex )]
		public void TestTypedFactory_ArgumentTypeMismatch()
		{
			var target = new ServiceLocator();
			Func<bool, TraceListener> func = useErrorStream => new ConsoleTraceListener( useErrorStream );
			Assert.IsTrue( target.RegisterFactory( typeof( TraceListener ), func ) );
			target.Get<TraceListener>( "foo" );
		}

		[Test]
		public void TestTypedFactory_ArgumentTypeMismatch_Convertible()
		{
			var target = new ServiceLocator();
			Func<bool, TraceListener> func = useErrorStream => new ConsoleTraceListener( useErrorStream );
			Assert.IsTrue( target.RegisterFactory( typeof( TraceListener ), func ) );
			Assert.AreSame( Console.Error, ( ( ConsoleTraceListener )target.Get<TraceListener>( "true" ) ).Writer );
			Assert.AreSame( Console.Out, ( ( ConsoleTraceListener )target.Get<TraceListener>( "false" ) ).Writer );
		}

		[Test]
		[ExpectedException( typeof( ArgumentException ), ExpectedMessage = @"arguments", MatchType = MessageMatch.Regex )]
		public void TestTypedFactory_ArgumentCountMismatch()
		{
			var target = new ServiceLocator();
			Func<bool, TraceListener> func = useErrorStream => new ConsoleTraceListener( useErrorStream );
			Assert.IsTrue( target.RegisterFactory( typeof( TraceListener ), func ) );
			target.Get<TraceListener>();
		}

		[Test]
		public void TestConsctructor()
		{
			var target = new ServiceLocator();
			Assert.IsTrue(
				target.RegisterFactory(
					typeof( TraceListener ),
					typeof( ConsoleTraceListener ).GetConstructor( new Type[] { typeof( bool ) } )
				)
			);
			var result = target.Get<TraceListener>( true );
			Assert.IsNotNull( result as ConsoleTraceListener );
			Assert.AreNotSame( result, target.Get<TraceListener>( true ) );
		}

		[Test]
		[ExpectedException( typeof( IntendedException ), ExpectedMessage = "DUMMY", MatchType = MessageMatch.Exact )] // thanks to IL shim, NOT TargetInvocationException
		public void TestConsctructorThrow()
		{
			var target = new ServiceLocator();
			Assert.IsTrue(
				target.RegisterFactory(
					typeof( IFoo ),
					typeof( FooImplFail ).GetConstructor( new Type[] { typeof( string ) } )
				)
			);
			target.Get<IFoo>( default( object ) );
		}

		[Test]
		public void TestProperty()
		{
			var target = new ServiceLocator();
			var start = DateTime.Now;
			Assert.IsTrue(
				target.RegisterFactory(
					typeof( DateTime ),
					typeof( DateTime ).GetProperty( "Now" )
				)
			);
			var result = target.Get<DateTime>();
			Assert.GreaterOrEqual( result, start );
			Assert.AreNotEqual( result, target.Get<DateTime>() );
		}

		[Test]
		[ExpectedException( typeof( IntendedException ), ExpectedMessage = "DUMMY", MatchType = MessageMatch.Exact )] // thanks to IL shim, NOT TargetInvocationException
		public void TestPropertyThrow()
		{
			var target = new ServiceLocator();
			var start = DateTime.Now;
			Assert.IsTrue(
				target.RegisterFactory(
					typeof( IFoo ),
					typeof( Foo ).GetProperty( "AlwaysFail" )
				)
			);
			target.Get<IFoo>();
		}

		[Test]
		public void TestMethod()
		{
			var target = new ServiceLocator();
			Assert.IsTrue(
				target.RegisterFactory(
					typeof( Guid ),
					typeof( Guid ).GetMethod( "NewGuid" )
				)
			);
			var result = target.Get<Guid>();
			Assert.AreNotEqual( result, target.Get<Guid>() );
		}

		[Test]
		[ExpectedException( typeof( IntendedException ), ExpectedMessage = "DUMMY", MatchType = MessageMatch.Exact )] // thanks to IL shim, NOT TargetInvocationException
		public void TestMethodThrow()
		{
			var target = new ServiceLocator();
			Assert.IsTrue(
				target.RegisterFactory(
					typeof( IFoo ),
					typeof( Foo ).GetMethod( "ThrowException" )
				)
			);
			target.Get<IFoo>( new IntendedException( "DUMMY" ) );
		}

		[Test]
		public void TestAutoConstructor()
		{
			var target = new ServiceLocator();
			Assert.IsTrue( target.RegisterFactory<IFoo, FooImpl>() );
			var token = Guid.NewGuid().ToString();
			var result = target.Get<IFoo>( token );
			Assert.AreEqual( token, result.Token );
		}

		[Test]
		[ExpectedException( typeof( IntendedException ), ExpectedMessage = "DUMMY", MatchType = MessageMatch.Exact )] // thanks to IL shim, NOT TargetInvocationException
		public void TestAutoConstructorThrow()
		{
			var target = new ServiceLocator();
			Assert.IsTrue( target.RegisterFactory<IFoo, FooImplFail>() );
			target.Get<IFoo>( default( object ) );
		}

		interface IFoo
		{
			string Token { get; }
		}

		abstract class Foo : IFoo
		{
			public abstract string Token { get; }

			public static IFoo AlwaysFail { get { throw new IntendedException( "DUMMY" ); } }

			public static IFoo ThrowException( Exception exception )
			{
				throw exception;
			}
		}

		class FooImpl : Foo
		{
			private readonly string _arg;

			public override string Token { get { return this._arg; } }

			public FooImpl( string arg )
			{
				this._arg = arg;
			}

		}

		class FooImplFail : Foo
		{
			private readonly string _arg;

			public override string Token { get { return this._arg; } }

			public FooImplFail( object maybeNull )
			{
				if ( maybeNull == null )
				{
					throw new IntendedException( "DUMMY" );
				}

				this._arg = maybeNull.ToString();
			}
		}

		sealed class IntendedException : Exception
		{
			public IntendedException( string message ) : base( message ) { }
		}


		[DefaultImplementation( typeof( HasDefaultImplementationNoDefaultConstructor ) )]
		public abstract class HasDefaultImplementationNoDefaultConstructorContract
		{

		}

		public class HasDefaultImplementationNoDefaultConstructor : HasDefaultImplementationNoDefaultConstructorContract
		{
			public HasDefaultImplementationNoDefaultConstructor( int arg1 ) { }
			public HasDefaultImplementationNoDefaultConstructor( int arg1, int arg2 ) { }
		}

		[DefaultImplementation( typeof( HasDefaultImplementationOnlyDefaultConstructor ) )]
		public abstract class HasDefaultImplementationOnlyDefaultConstructorContract
		{

		}

		public class HasDefaultImplementationOnlyDefaultConstructor : HasDefaultImplementationOnlyDefaultConstructorContract
		{
			public HasDefaultImplementationOnlyDefaultConstructor() { }
		}

		[DefaultImplementation( typeof( HaDefaultImplementationOnlyNonPublicDefaultConstructor ) )]
		public abstract class HasDefaultImplementationOnlyNonPublicDefaultConstructorContract
		{

		}

		public class HaDefaultImplementationOnlyNonPublicDefaultConstructor : HasDefaultImplementationOnlyNonPublicDefaultConstructorContract
		{
			internal HaDefaultImplementationOnlyNonPublicDefaultConstructor() { }
		}

		[DefaultImplementation( typeof( GenericService<> ) )]
		public abstract class GenericServiceContract<T> { }

		public class GenericService<T> : GenericServiceContract<T>
		{
			public GenericService() { }
		}

		[DefaultImplementation( typeof( GenericService<,> ) )]
		public abstract class GenericServiceContract<T1, T2> { }

		public class GenericService<T1, T2> : GenericServiceContract<T1, T2>
		{
			public GenericService() { }
		}

		[DefaultImplementation( typeof( GenericServiceClosed ) )]
		public abstract class GenericServiceClosedContract<T> { }

		public class GenericServiceClosed : GenericServiceClosedContract<object>
		{
			public GenericServiceClosed() { }
		}

		[DefaultImplementation( typeof( GenericServiceClosed<> ) )]
		public abstract class GenericServiceClosedContract<T1, T2> { }

		public class GenericServiceClosed<T> : GenericServiceClosedContract<object, T>
		{
			public GenericServiceClosed() { }
		}

		[DefaultImplementation( typeof( GenericServiceOpening<> ) )]
		public abstract class GenericServiceOpeningContract { }

		public class GenericServiceOpening<T> : GenericServiceOpeningContract
		{
			public GenericServiceOpening() { }
		}
	}
}
