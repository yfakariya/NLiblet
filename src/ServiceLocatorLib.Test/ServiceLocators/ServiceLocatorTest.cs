using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;

namespace NLiblet.ServiceLocators
{
	[TestFixture]
	public sealed class ServiceLocatorTest
	{
		private static readonly ConsoleTraceListener _listener = new ConsoleTraceListener();
		private static SourceLevels _previousLevel;

		[TestFixtureSetUp]
		public static void FixtureSetup()
		{
			var source = ServiceLocator.DebugTrace;
			_previousLevel = source.Switch.Level;
			source.Switch.Level = SourceLevels.All;
			source.Listeners.Add( _listener );
		}

		[TestFixtureTearDown]
		public static void FixtureTearDown()
		{
			var source = ServiceLocator.DebugTrace;
			source.Listeners.Remove( _listener );
			source.Switch.Level = _previousLevel;
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
		public void TestGetSingleton_NotRegistered()
		{
			new ServiceLocator().GetSingleton<TraceListener>();
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestGet_NotRegistered()
		{
			new ServiceLocator().Get<TraceListener>();
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
			Assert.AreSame( result, target.Get<TraceListener>( expected, true ) );
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

		// FIXME: Strongly typed actory method
		// Func<T1,T2,TResult> 

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
			target.Get<IFoo>( default( string ) );
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
					typeof( Foo ).GetProperty( "AlwaysThrow" )
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
			target.Get<IFoo>( default( string ) );
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

			public FooImplFail( string maybeNull )
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
	}
}
