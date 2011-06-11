using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.Globalization;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Security;
using System.IO;

namespace NLiblet.ServiceLocators
{
	/// <summary>
	///		Simple service locator.
	/// </summary>
	/// <remarks>
	///		<para>
	///			You can do 'light weight' dependency injection (DI) with service locator.
	///		</para>
	///		<para>
	///			In real world, it is rare to use DI on production code, but it is useful to inject any test doubles on testing process.
	///			Therefore, 'heavy weight' full stack DI mechanism like DI container is overkill.
	///			In addition, more flexibily often brings more weakness in terms of structure as mechanics with many joints tend to be broken.
	///			It might cause undesirable unpredictivity to the system in the future.
	///			With service locator, you can get enough flexibility to use test doubles without bringing weakness nor unpredictivity.
	///		</para>
	///		<example>
	///			In test initialization code, you can register test doubles as following:
	///			<code>
	///			ServiceLocator.Instance.RegisterService( typeof( SomeDataAccessor ), args => CreateMockSomeDataAccessor( ( string )args[ 0 ] ) );
	///			</code>
	///			If you want to isolate service locator from other tests, you can swap it to dedicated locator:
	///			<code>
	///			private readonly ServiceLocator serviceLocatorForTest = new ServiceLocator( "Service Locator for xxx test." );
	///			
	///			[...] // Test initialization code depends on testing framework.
	///			public void SetupTest( ... )
	///			{
	///				ServiceLocator.SetInstance( this.serviceLocatorForTest );
	///			}
	///			
	///			[...] // Test clean up code depends on testing framework.
	///			public void CleanUpTest( ... )
	///			{
	///				ServiceLocator.ResetToDefault();
	///			}
	///			</code>
	///		</example>
	/// </remarks>
	public sealed class ServiceLocator
	{
		private static class TraceEventId
		{
			public const int ChangeInstance = 101;
			public const int ResetInstance = 102;
			public const int RegisterSingletonServiceFactory = 201;
			public const int InitializeSingletonService = 202;
			public const int UnregisterSingletonServiceFactory = 211;
			public const int RemoveSingletonService = 212;
			public const int RegisterAdHocServiceFactory = 301;
			public const int UnregisterAdHocServiceFactory = 311;
			public const int RetrievingNonPublicConstructorIsNotAllowed = 401;
			public const int FoundIdealConstructor = 402;
			public const int DumpDynamicFactory = 411;
		}

		private static readonly TraceSource _trace = new TraceSource( typeof( ServiceLocator ).FullName );

#if DEBUG
		internal static TraceSource DebugTrace
		{
			get { return _trace; }
		}
#endif

		#region -- Static Locator --

		private static readonly ServiceLocator _default = new ServiceLocator( "Default ServiceLocator" );

		public static ServiceLocator Default
		{
			get { return _default; }
		}

		public static void ResetToDefault()
		{
			_trace.TraceEvent( TraceEventType.Information, TraceEventId.ResetInstance, "Reset instance from {0} to default.", _appDomainSingletonInstance );
			_appDomainSingletonInstance = Default;
		}

		private static ServiceLocator _appDomainSingletonInstance = Default;

		public static ServiceLocator Instance
		{
			get { return _appDomainSingletonInstance; }
		}

		public static void SetInstance( ServiceLocator appDomainSingletonInstance )
		{
			Contract.Requires<ArgumentNullException>( appDomainSingletonInstance != null );

			_trace.TraceEvent( TraceEventType.Information, TraceEventId.ChangeInstance, "{0}->{1}", _appDomainSingletonInstance, appDomainSingletonInstance );
			_appDomainSingletonInstance = appDomainSingletonInstance;
		}

		#endregion

		#region -- Initialization --

		/// <summary>
		///		Initialize new instance with default display name.
		/// </summary>
		public ServiceLocator() : this( null ) { }

		/// <summary>
		///		Initialize new instance with specified display name.
		/// </summary>
		/// <param name="displayName">Display name to be shown in a debugger etc. To use default name, specify null or blank.</param>
		public ServiceLocator( string displayName )
		{
			this._displayName = String.IsNullOrWhiteSpace( displayName ) ? null : displayName;
		}

		#endregion

		#region -- Display name --

		private readonly string _displayName;

		public sealed override string ToString()
		{
			if ( String.IsNullOrEmpty( this._displayName ) )
			{
				return typeof( ServiceLocator ).FullName + "#" + this.GetHashCode();
			}
			else
			{
				return this._displayName;
			}
		}

		#endregion

		#region -- Singleton Services --

		private readonly Dictionary<RuntimeTypeHandle, object> _singletonServices = new Dictionary<RuntimeTypeHandle, object>();
		private readonly Dictionary<RuntimeTypeHandle, Func<object>> _singletonServiceFactories = new Dictionary<RuntimeTypeHandle, Func<object>>();

		// It is OK to use simple monitor since singleton factory will rarely be referred.
		private readonly object _singletonServiceFactoriesLock = new object();
#if !Silverlight
		private readonly ReaderWriterLockSlim _singletonServicesLock = new ReaderWriterLockSlim( LockRecursionPolicy.NoRecursion );
#else
		private readonly object _singletonServicesLock = new object();
#endif

		public T GetSingleton<T>()
			where T : class
		{
			object service;
			bool lockTaken = false;

			try
			{
				AcquireReadLock( this._singletonServicesLock, ref lockTaken );
				if ( !this._singletonServices.TryGetValue( typeof( T ).TypeHandle, out service ) || service == null )
				{
					SwitchToWriteLock( this._singletonServicesLock, ref lockTaken );
					lock ( this._singletonServiceFactoriesLock )
					{
						Func<object> factory;
						if ( !this._singletonServiceFactories.TryGetValue( typeof( T ).TypeHandle, out factory ) )
						{
							throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Service '{0}' is not registered.", typeof( T ) ) );
						}

						T newService = null;
						try
						{
							newService = ( T )factory();
						}
						catch ( InvalidCastException ex )
						{
							throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Service facory '{0}' did not create '{1}' type.", factory, typeof( T ) ), ex );
						}

						if ( newService == null )
						{
							throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Service facory '{0}' did not create non-null '{1}'.", factory, typeof( T ) ) );
						}

						this._singletonServices[ typeof( T ).TypeHandle ] = newService;
						_trace.TraceEvent( TraceEventType.Verbose, TraceEventId.InitializeSingletonService, "Initialize singleton service '{0}'(0x{1:x}) with '{2}'(type:{3}(0x{4:x}))", typeof( T ), typeof( T ).TypeHandle.Value, newService, newService.GetType(), newService.GetType().TypeHandle.Value );
						service = newService;
						this._singletonServiceFactories.Remove( typeof( T ).TypeHandle );
					}
				}
			}
			finally
			{
				ReleaseLock( this._singletonServicesLock, lockTaken );
			}

			return ( T )service;
		}

		public bool RegisterSingleton( Type serviceType, object singletonServiceInstance )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentNullException>( singletonServiceInstance != null );
			Contract.Requires<ArgumentException>( !serviceType.IsValueType );
			Contract.Requires<ArgumentException>( serviceType.IsAssignableFrom( singletonServiceInstance.GetType() ) );

			return this.RegisterSingleton( serviceType, () => singletonServiceInstance );
		}

		public bool RegisterSingleton( Type serviceType, Func<object> singletonServiceInstanceProvider )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentException>( !serviceType.IsValueType );
			Contract.Requires<ArgumentNullException>( singletonServiceInstanceProvider != null );

			lock ( this._singletonServiceFactoriesLock )
			{
				if ( this._singletonServiceFactories.ContainsKey( serviceType.TypeHandle ) )
				{
					return false;
				}
				else
				{
					this._singletonServiceFactories.Add( serviceType.TypeHandle, singletonServiceInstanceProvider );
					_trace.TraceEvent( TraceEventType.Verbose, TraceEventId.RegisterSingletonServiceFactory, "Register singleton service factory '{0}' for service type '{1}'(0x{2:x})", singletonServiceInstanceProvider, serviceType, serviceType.TypeHandle.Value );
					return true;
				}
			}
		}

		public bool RemoveSingleton( Type serviceType )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );

			bool result = false;

			bool lockTaken = false;

			lock ( this._singletonServiceFactoriesLock )
			{
				var removed = this._singletonServiceFactories.Remove( serviceType.TypeHandle );
				result |= removed;
				if ( removed )
				{
					_trace.TraceEvent( TraceEventType.Verbose, TraceEventId.UnregisterSingletonServiceFactory, "Unregister singleton service factory for service type '{0}'(0x{1:x})", serviceType, serviceType.TypeHandle.Value );
				}
			}

			try
			{
				AcquireWriteLock( this._singletonServicesLock, ref lockTaken );
				var removed = this._singletonServices.Remove( serviceType.TypeHandle );
				result |= removed;
				if ( removed )
				{
					_trace.TraceEvent( TraceEventType.Verbose, TraceEventId.RemoveSingletonService, "Remove singleton service instance for service type '{0}'(0x{1:x})", serviceType, serviceType.TypeHandle.Value );
				}
			}
			finally
			{
				ReleaseLock( this._singletonServicesLock, lockTaken );
			}

			return result;
		}

		#endregion

		#region -- Ad-Hoc Services --

		private readonly Dictionary<RuntimeTypeHandle, Func<object[], object>> _adhocServiceFactories = new Dictionary<RuntimeTypeHandle, Func<object[], object>>();

#if !Silverlight
		private readonly ReaderWriterLockSlim _adhocServiceFactoriesLock = new ReaderWriterLockSlim( LockRecursionPolicy.NoRecursion );
#else
		private readonly object _adhocServiceFactoriesLock = new object();
#endif

		public T Get<T>( params object[] args )
		{
			var handle = typeof( T ).TypeHandle;
			bool lockTaken = false;
			Func<object[], object> factory;

			try
			{
				AcquireReadLock( this._adhocServiceFactoriesLock, ref lockTaken );
				this._adhocServiceFactories.TryGetValue( handle, out factory );
			}
			finally
			{
				ReleaseLock( this._adhocServiceFactoriesLock, lockTaken );
			}

			if ( factory == null )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Factory for service '{0}' is not registered.", typeof( T ) ) );
			}

			object service = factory( args ?? Arrays.Empty<object>() );
			try
			{
				return ( T )service;
			}
			catch ( InvalidCastException ex )
			{
				throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Factory '{0}' is not create '{1}' but '{2}'(type:'{3}').", factory, typeof( T ), service, ( service == null ? "(null)" : service.GetType().FullName ) ) );
			}
		}

		public bool RegisterFactory<TService, TInstance>()
			where TInstance : TService
		{
			return this.RegisterFactory( typeof( TService ), GetIdealContructor( typeof( TInstance ) ) );
		}

		#region ---- GetIdealContructor ----

		private static ConstructorInfo GetIdealContructor( Type type )
		{
			var candidates = GetAvailableConstructors( type );
			switch ( candidates.Length )
			{
				case 0:
				{
					throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' does not have any available constructors.", type ) );
				}
				case 1:
				{
					_trace.TraceEvent( TraceEventType.Verbose, TraceEventId.FoundIdealConstructor, "Found ideal constructor for service type '{0}'(0x{1:x}) is '{2}'(0x{3:x})", type, type.TypeHandle.Value, candidates[ 0 ], candidates[ 0 ].MethodHandle.Value );
					return candidates[ 0 ];
				}
				default:
				{
					throw new InvalidOperationException( String.Format( CultureInfo.CurrentCulture, "Type '{0}' has more than one available constructors.", type ) );
				}
			}
		}

		// Prevent inlining for catch.
		[MethodImpl( MethodImplOptions.NoInlining )]
		private static ConstructorInfo[] GetAvailableConstructors( Type type )
		{
			ConstructorInfo[] result = null;
			try
			{
				result = GetAllConstructors( type );
			}
			catch ( SecurityException )
			{
				// Do not have ReflectionPermission on old CAS or not run on fully trusted AppDomain.
				_trace.TraceEvent( TraceEventType.Verbose, TraceEventId.RetrievingNonPublicConstructorIsNotAllowed, "Retrieving non-public constructor for type '{0}'(0x:{1:x}) is not allowed.", type, type.TypeHandle.Value );
				result = type.GetConstructors();
			}

			return result;
		}

		// Prevent inlining to catch SecurityException due to LinkDemand.
		[MethodImpl( MethodImplOptions.NoInlining )]
		private static ConstructorInfo[] GetAllConstructors( Type type )
		{
			return type.GetConstructors( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
		}

		#endregion

		public bool RegisterFactory( Type serviceType, PropertyInfo property )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentNullException>( property != null );
			Contract.Requires<ArgumentException>( property.CanRead );
			Contract.Requires<ArgumentException>( serviceType.IsAssignableFrom( property.PropertyType ) );

			return this.RegisterFactory( serviceType, CreateFactory( serviceType, property.GetGetMethod() ) );
		}

		public bool RegisterFactory( Type serviceType, ConstructorInfo constructor )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentNullException>( constructor != null );
			Contract.Requires<ArgumentException>( !constructor.IsStatic );
			Contract.Requires<ArgumentException>( serviceType.IsAssignableFrom( constructor.DeclaringType ) );

			return this.RegisterFactory( serviceType, this.CreateFactory( serviceType, constructor ) );
		}

		public bool RegisterFactory( Type serviceType, MethodInfo factoryMethod )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentNullException>( factoryMethod != null );
			Contract.Requires<ArgumentException>( factoryMethod.IsStatic );
			Contract.Requires<ArgumentException>( serviceType.IsAssignableFrom( factoryMethod.ReturnType ) );

			return this.RegisterFactory( serviceType, this.CreateFactory( serviceType, factoryMethod ) );
		}

		#region ---- CreateFactory ----

		private static readonly Type[] _factoryMethodParameterTypes = new Type[] { typeof( object[] ) };

		private Func<object[], object> CreateFactory( Type serviceType, MethodBase target )
		{
			// Target code is following.
			/*
			 * public static object $ServiceLoctor$FactoryMethod$<FullName>$Token(object[] args)
			 * {
			 *		return 
			 *			( object ) target(
			 *				(T0)args[0],
			 *				:
			 *				(Tn)args[n]
			 *			);
			 * }
			 * 
			 */

			// We don't want to have double code base, so we cannot use Expression<T>.

			var methodName = new StringBuilder( typeof( ServiceLocator ).FullName.Length + serviceType.FullName.Length + 32 );
			methodName.Append( typeof( ServiceLocator ).FullName ).Append( "_FactoryMethod_" ).Append( serviceType.FullName ).Append( '_' ).Append( serviceType.TypeHandle.Value.ToString( "x" ) ).Replace( Type.Delimiter, '_' );
			var method = new DynamicMethod( methodName.ToString(), typeof( object ), _factoryMethodParameterTypes, true );
			var asConstructor = target as ConstructorInfo;
			Type instanceType = asConstructor == null ? ( ( MethodInfo )target ).ReturnType : asConstructor.DeclaringType;
			var parameters = target.GetParameters();

			var il = method.GetILGenerator();

			// debuginfo
			var ilTraceBuffer = _trace.Switch.ShouldTrace( TraceEventType.Verbose ) ? new StringBuilder() : null;
			var ilTrace = _trace.Switch.ShouldTrace( TraceEventType.Verbose ) ? new StringWriter( ilTraceBuffer ) : TextWriter.Null;
			const string traceDelimiter = ";\\r\\n";
			int line = 0;

			// Push arguments from param array with casting.
			for ( int i = 0; i < parameters.Length; i++ )
			{
				il.Emit( OpCodes.Ldarg_0 );
				ilTrace.Write( line++ );
				ilTrace.Write( " ldarg.0" );
				ilTrace.Write( traceDelimiter );

				// Make effort to generate efficient ILs
				ilTrace.Write( line++ );
				switch ( i )
				{
					case 0: { il.Emit( OpCodes.Ldc_I4_0 );ilTrace.Write(" ldc.i4.0" ); break; }
					case 1: { il.Emit( OpCodes.Ldc_I4_1 ); ilTrace.Write( " ldc.i4.1" ); break; }
					case 2: { il.Emit( OpCodes.Ldc_I4_2 ); ilTrace.Write( " ldc.i4.2" ); break; }
					case 3: { il.Emit( OpCodes.Ldc_I4_3 ); ilTrace.Write( " ldc.i4.3" ); break; }
					case 4: { il.Emit( OpCodes.Ldc_I4_4 ); ilTrace.Write( " ldc.i4.4" ); break; }
					case 5: { il.Emit( OpCodes.Ldc_I4_5 ); ilTrace.Write( " ldc.i4.5" ); break; }
					case 6: { il.Emit( OpCodes.Ldc_I4_6 ); ilTrace.Write( " ldc.i4.6" ); break; }
					case 7: { il.Emit( OpCodes.Ldc_I4_7 ); ilTrace.Write( " ldc.i4.7" ); break; }
					case 8: { il.Emit( OpCodes.Ldc_I4_8 ); ilTrace.Write( " ldc.i4.8" ); break; }
					default:
					{
						if ( i <= Byte.MaxValue )
						{
							il.Emit( OpCodes.Ldc_I4_S, ( byte )i );
							ilTrace.Write( " ldc.i4.s " );
						}
						else
						{
							il.Emit( OpCodes.Ldc_I4, i );
							ilTrace.Write( " ldc.i4 " );
						}
						ilTrace.Write( i );

						break;
					}
				}
				ilTrace.Write( traceDelimiter );

				// CLI states that index of vector is not int32 but 'native int'.
				il.Emit( OpCodes.Conv_I );
				ilTrace.Write( line++ );
				ilTrace.Write( " conv.i" );
				ilTrace.Write( traceDelimiter );

				il.Emit( OpCodes.Ldelem_Ref );
				ilTrace.Write( line++ );
				ilTrace.Write( " ldelem.ref" );
				ilTrace.Write( traceDelimiter );

				il.Emit( OpCodes.Unbox_Any, parameters[ i ].ParameterType );
				ilTrace.Write( line++ );
				ilTrace.Write( " unbox.any [" );
				ilTrace.Write( parameters[ i ].ParameterType.Assembly.FullName );
				ilTrace.Write( "]" );
				ilTrace.Write( parameters[ i ].ParameterType.FullName );
				ilTrace.Write( traceDelimiter );
			}

			// Invoke target
			if ( asConstructor != null )
			{
				// It is OK to call .ctor of value type.
				il.Emit( OpCodes.Newobj, asConstructor );
				ilTrace.Write( line++ );
				ilTrace.Write( " newobj instance void [" );
				ilTrace.Write( asConstructor.DeclaringType.Assembly.FullName );
				ilTrace.Write( "]" );
				ilTrace.Write( asConstructor );
				ilTrace.Write( traceDelimiter );
			}
			else
			{
				// Invoke factory method.
				il.Emit( OpCodes.Call, ( MethodInfo )target );
				ilTrace.Write( line++ );
				ilTrace.Write( " call " );
				ilTrace.Write( target.IsStatic ? "class" : "instance" );
				ilTrace.Write( " " );
				ilTrace.Write( ( (MethodInfo)target).ReturnType.FullName);
				ilTrace.Write( " [" );
				ilTrace.Write( target.DeclaringType.Assembly.FullName );
				ilTrace.Write( "]" );
				ilTrace.Write( target );
				ilTrace.Write( traceDelimiter );

			}

			// Box return value if necessary
			if ( instanceType.IsValueType )
			{
				il.Emit( OpCodes.Box );
				ilTrace.Write( line++ );
				ilTrace.Write( " box" );
				ilTrace.Write( traceDelimiter );
			}

			// Ret
			il.Emit( OpCodes.Ret );
			ilTrace.Write( line++ );
			ilTrace.Write( " ret" );
			ilTrace.Write( traceDelimiter );

			_trace.TraceEvent( TraceEventType.Verbose, TraceEventId.DumpDynamicFactory, "Dynamic factory method is created. Name:'{0}', Target:'{1}'(0x{2:x}), IL:{3}", methodName, target, target.MethodHandle.Value, ilTraceBuffer );


			return ( Func<object[], object> )method.CreateDelegate( typeof( Func<object[], object> ) );
		}

		#endregion

		public bool RegisterFactory( Type serviceType, Func<object[], object> factory )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentNullException>( factory != null );

			return this.RegisterFactory( serviceType, () => factory );
		}

		private bool RegisterFactory( Type serviceType, Func<Func<object[], object>> factoryFactory )
		{
			bool lockTaken = false;
			try
			{
				AcquireReadLock( this._adhocServiceFactoriesLock, ref lockTaken );
				if ( this._adhocServiceFactories.ContainsKey( serviceType.TypeHandle ) )
				{
					return false;
				}

				SwitchToWriteLock( this._adhocServiceFactoriesLock, ref lockTaken );

				// Check again.
				if ( this._adhocServiceFactories.ContainsKey( serviceType.TypeHandle ) )
				{
					return false;
				}

				var factory = factoryFactory();
				this._adhocServiceFactories.Add( serviceType.TypeHandle, factory );
				_trace.TraceEvent( TraceEventType.Verbose, TraceEventId.RegisterAdHocServiceFactory, "Register ad-hoc service factory '{0}' for service type '{1}'(0x{2:x})", factory, serviceType, serviceType.TypeHandle.Value );
				return true;
			}
			finally
			{
				ReleaseLock( this._adhocServiceFactoriesLock, lockTaken );
			}
		}

		public bool RemoveFactory( Type serviceType )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );

			bool lockTaken = false;
			try
			{
				AcquireWriteLock( this._adhocServiceFactoriesLock, ref lockTaken );
				var result = this._adhocServiceFactories.Remove( serviceType.TypeHandle );
				if ( result )
				{
					_trace.TraceEvent( TraceEventType.Verbose, TraceEventId.UnregisterAdHocServiceFactory, "Unregister ad-hoc service factory for service type '{0}'(0x{1:x})", serviceType, serviceType.TypeHandle.Value );
				}
				return result;
			}
			finally
			{
				ReleaseLock( this._adhocServiceFactoriesLock, lockTaken );
			}
		}

		#endregion

		#region -- Synchronization --

#if !Silverlight
		private static void AcquireReadLock( ReaderWriterLockSlim syncRoot, ref bool lockTaken )
		{
			Contract.Assert( syncRoot != null );
			// Do not use UpgradeableLock since it is not scalable.
			try { }
			finally
			{
				syncRoot.EnterReadLock();
				lockTaken = true;
			}
		}

		private static void AcquireWriteLock( ReaderWriterLockSlim syncRoot, ref bool lockTaken )
		{
			Contract.Assert( syncRoot != null );
			try { }
			finally
			{
				syncRoot.EnterWriteLock();
				lockTaken = true;
			}
		}

		private static void SwitchToWriteLock( ReaderWriterLockSlim syncRoot, ref bool lockTaken )
		{
			Contract.Assert( syncRoot != null );
			Contract.Assert( syncRoot.IsReadLockHeld );

			// Do not use UpgradeableLock since it is not scalable.
			try { }
			finally
			{
				syncRoot.ExitReadLock();
				lockTaken = false;
			}

			try { }
			finally
			{
				// It is possible that other thread(s) change previous state here.
				syncRoot.EnterWriteLock();
				lockTaken = true;
			}
			// So, you must check the state after exit from this method.
		}

		private static void ReleaseLock( ReaderWriterLockSlim syncRoot, bool lockHeld )
		{
			Contract.Assert( syncRoot != null );
			Contract.Assert( !syncRoot.IsUpgradeableReadLockHeld );

			if ( !lockHeld )
			{
				return;
			}

			if ( syncRoot.IsReadLockHeld )
			{
				syncRoot.ExitReadLock();
			}
			else
			{
				Contract.Assert( syncRoot.IsWriteLockHeld );
				syncRoot.ExitWriteLock();
			}
		}

#else
		private static void AcquireReadLock( object syncRoot, ref bool lockTaken )
		{
			Contract.Assert( syncRoot != null );
			Contract.Assert( !syncRoot.GetType().IsValueType );
			Contract.Assert( !( syncRoot is MemberInfo ) );

			Monitor.Enter( syncRoot, ref lockTaken );
		}

		private static void AcquireWriteLock( object syncRoot, ref bool lockTaken )
		{
			Contract.Assert( syncRoot != null );
			Contract.Assert( !syncRoot.GetType().IsValueType );
			Contract.Assert( !( syncRoot is MemberInfo ) );

			Monitor.Enter( syncRoot, ref lockTaken );
		}

		private static void SwitchToWriteLock( object syncRoot )
		{
			Contract.Assert( syncRoot != null );
			Contract.Assert( !syncRoot.GetType().IsValueType );
			Contract.Assert( !( syncRoot is MemberInfo ) );

			// Nothing to do.
		}

		private static void ReleaseLock( object syncRoot, bool lockHeld )
		{
			Contract.Assert( syncRoot != null );
			Contract.Assert( !syncRoot.GetType().IsValueType );
			Contract.Assert( !( syncRoot is MemberInfo ) );

			if ( !lockHeld )
			{
				return;
			}

			Monitor.Exit( syncRoot );
		}
#endif
		#endregion
	}
}
