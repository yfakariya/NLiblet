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
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

using NLiblet.Properties;
using NLiblet.Text;

namespace NLiblet.Reflection
{
#pragma warning disable 1572, 1587, 1591

	// These methods called from generated shim code.
	[EditorBrowsable( EditorBrowsableState.Never )]
	public sealed partial class GeneratedCodeHelper
	{
		private static GeneratedCodeHelper _instance = new GeneratedCodeHelper();

		[EditorBrowsable( EditorBrowsableState.Never )]
		public static GeneratedCodeHelper Instance
		{
			get { return _instance; }
			internal set { _instance = value; }
		}

		[EditorBrowsable( EditorBrowsableState.Never )]
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Just like as Resources." )]
		public static string Resource_CastCode_CannotCastArrayItemAt
		{
			get { return Resources.CastCode_CannotCastArrayItemAt; }
		}

		[EditorBrowsable( EditorBrowsableState.Never )]
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Just like as Resources." )]
		public static string Resource_CastCode_CannotCastArrayItemWithTypeConverterAt
		{
			get { return Resources.CastCode_CannotCastArrayItemWithTypeConverterAt; }
		}

		[EditorBrowsable( EditorBrowsableState.Never )]
		[SuppressMessage( "Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Just like as Resources." )]
		public static string Resource_CastCode_CannotCastWithTypeConverter
		{
			get { return Resources.CastCode_CannotCastWithTypeConverter; }
		}

		private bool _isTracingEnabled;

		[EditorBrowsable( EditorBrowsableState.Never )]
		public bool IsTracingEnabled
		{
			get { return this._isTracingEnabled; }
			set { this._isTracingEnabled = value; }
		}

		[EditorBrowsable( EditorBrowsableState.Never )]
		public TTarget CastArrayItem<TTarget>( object[] array, int index )
		{
			Contract.Requires<ArgumentNullException>( array != null );
			Contract.Requires<ArgumentOutOfRangeException>( 0 <= index );
			Contract.Requires<ArgumentOutOfRangeException>( index < array.Length );

			var item = array[ index ];
			if ( item == null )
			{
				if ( typeof( TTarget ).IsValueType )
				{
					throw new ArgumentException(
						String.Format(
							CultureInfo.CurrentCulture,
							NLiblet.Properties.Resources.GeneratedCodeHelper_ValueTypeArgumentCannotBeNull,
							index,
							typeof( TTarget ).FullName
						),
						"arguments[" + index + "]"
					);
				}
			}

			if ( item is TTarget )
			{
				return ( TTarget )item;
			}

			return this.CastCore<object, TTarget>( item, index );
		}

		[EditorBrowsable( EditorBrowsableState.Never )]
		public TTarget Cast<TSource, TTarget>( TSource source )
		{
			return this.CastCore<TSource, TTarget>( source );
		}

		private readonly Dictionary<CastingTypePair, Delegate> _cast =
			new Dictionary<CastingTypePair, Delegate>();

		private TTarget CastCore<TSource, TTarget>( TSource source )
		{
			var key = new CastingTypePair( typeof( TSource ).TypeHandle, typeof( TTarget ).TypeHandle );
			Func<TSource, int, TTarget> cast = null;
			lock ( this._cast )
			{
				Delegate result;
				if ( this._cast.TryGetValue( key, out result ) )
				{
					cast = result as Func<TSource, int, TTarget>;
				}
			}

			if ( cast == null )
			{
				cast = CreateCastMethod<TSource, TTarget>( false, this._isTracingEnabled );

				lock ( this._cast )
				{
					this._cast[ key ] = cast;
				}
			}

			return cast( source, -1 );
		}

		private readonly Dictionary<CastingTypePair, Delegate> _arrayElementCast =
			new Dictionary<CastingTypePair, Delegate>();

		private TTarget CastCore<TSource, TTarget>( TSource source, int arrayIndex )
		{
			var key = new CastingTypePair( typeof( TSource ).TypeHandle, typeof( TTarget ).TypeHandle );
			Func<TSource, int, TTarget> cast = null;
			lock ( this._arrayElementCast )
			{
				Delegate result;
				if ( this._arrayElementCast.TryGetValue( key, out result ) )
				{
					cast = result as Func<TSource, int, TTarget>;
				}
			}

			if ( cast == null )
			{
				cast = CreateCastMethod<TSource, TTarget>( true, this._isTracingEnabled );

				lock ( this._cast )
				{
					this._arrayElementCast[ key ] = cast;
				}
			}

			return cast( source, arrayIndex );
		}

		private static Func<TSource, int, TTarget> CreateCastMethod<TSource, TTarget>( bool forArrayItem, bool isTracingEnabled )
		{
			if ( typeof( TTarget ).IsAssignableFrom( typeof( TSource ) ) )
			{
				return forArrayItem
					? CreateUnboxAnyCastMethodOfArrayItem( typeof( Func<TSource, int, TTarget> ), typeof( TSource ), typeof( TTarget ), isTracingEnabled ) as Func<TSource, int, TTarget>
					: CreateUnboxAnyCastMethod( typeof( Func<TSource, int, TTarget> ), typeof( TSource ), typeof( TTarget ), isTracingEnabled ) as Func<TSource, int, TTarget>;
			}

			if ( typeof( TSource ).TypeHandle.Equals( typeof( object ).TypeHandle ) )
			{
				if ( typeof( TTarget ).IsPrimitive )
				{
					return forArrayItem
						? CreateUnboxAndConvCastMethodOfArrayItem( typeof( Func<TSource, int, TTarget> ), typeof( TSource ), typeof( TTarget ), isTracingEnabled ) as Func<TSource, int, TTarget>
						: CreateUnboxAndConvCastMethod( typeof( Func<TSource, int, TTarget> ), typeof( TSource ), typeof( TTarget ), isTracingEnabled ) as Func<TSource, int, TTarget>;
				}
				else
				{
					return forArrayItem
						? CreateUnboxAnyCastMethodOfArrayItem( typeof( Func<TSource, int, TTarget> ), typeof( TSource ), typeof( TTarget ), isTracingEnabled ) as Func<TSource, int, TTarget>
						: CreateUnboxAnyCastMethod( typeof( Func<TSource, int, TTarget> ), typeof( TSource ), typeof( TTarget ), isTracingEnabled ) as Func<TSource, int, TTarget>;
				}
			}

			// Search conversion operators...
			var conversionOperator = ResolveOverload( FindBestConversionOperator<TSource, TTarget>() );
			if ( conversionOperator != null )
			{
				var conversionMethod = conversionOperator as MethodInfo;
				return forArrayItem
					? CreateOperatorCastMethodOfArrayItem( typeof( Func<TSource, int, TTarget> ), typeof( TSource ), typeof( TTarget ), isTracingEnabled, conversionMethod ) as Func<TSource, int, TTarget>
					: CreateOperatorCastMethod( typeof( Func<TSource, int, TTarget> ), typeof( TSource ), typeof( TTarget ), isTracingEnabled, conversionMethod ) as Func<TSource, int, TTarget>;
			}

			// Search conversion constructors...
			var conversionConstructor =
				ResolveOverload(
					typeof( TTarget ).GetConstructors( BindingFlags.Public | BindingFlags.Instance )
					.Where( ctor =>
						{
							var paramters = ctor.GetParameters();
							return paramters.Length == 1 && paramters[ 0 ].ParameterType.IsAssignableFrom( typeof( TSource ) );
						}
					).ToArray()
				);

			if ( conversionConstructor != null )
			{
				var conversionConstructorInfo = conversionConstructor as ConstructorInfo;
				return forArrayItem
					? CreateConstructorCastMethodOfArrayItem( typeof( Func<TSource, int, TTarget> ), typeof( TSource ), typeof( TTarget ), isTracingEnabled, conversionConstructorInfo ) as Func<TSource, int, TTarget>
					: CreateConstructorCastMethod( typeof( Func<TSource, int, TTarget> ), typeof( TSource ), typeof( TTarget ), isTracingEnabled, conversionConstructorInfo ) as Func<TSource, int, TTarget>;
			}

			// Delegates to TypeConverter which causes boxing...

			return forArrayItem
					? CreateTypeConverterCastMethodOfArrayItem( typeof( Func<TSource, int, TTarget> ), typeof( TSource ), typeof( TTarget ), isTracingEnabled ) as Func<TSource, int, TTarget>
					: CreateTypeConverterCastMethod( typeof( Func<TSource, int, TTarget> ), typeof( TSource ), typeof( TTarget ), isTracingEnabled ) as Func<TSource, int, TTarget>;
		}

		private static MethodInfo[] FindBestConversionOperator<TSource, TTarget>()
		{
			var typeTuple = Tuple.Create( typeof( TSource ), typeof( TTarget ) );
			// Implicit conversion is better
			var conversionOperators = typeof( TTarget ).FindMembers( MemberTypes.Method, BindingFlags.Static | BindingFlags.Public, FilterImplictOperator, typeTuple );
			if ( conversionOperators.Length == 0 )
			{
				conversionOperators = typeof( TSource ).FindMembers( MemberTypes.Method, BindingFlags.Static | BindingFlags.Public, FilterImplictOperator, typeTuple );
			}

			if ( conversionOperators.Length == 0 )
			{
				conversionOperators = typeof( TTarget ).FindMembers( MemberTypes.Method, BindingFlags.Static | BindingFlags.Public, FilterExplictOperator, typeTuple );
			}

			if ( conversionOperators.Length == 0 )
			{
				conversionOperators = typeof( TSource ).FindMembers( MemberTypes.Method, BindingFlags.Static | BindingFlags.Public, FilterExplictOperator, typeTuple );
			}

			return conversionOperators as MethodInfo[];
		}

		private static bool FilterImplictOperator( MemberInfo currentMember, object typesAsCriteria )
		{
			var tuple = typesAsCriteria as Tuple<Type, Type>;
			return FilterConversionOperator( currentMember, tuple.Item1, tuple.Item2, "op_Implicit" );
		}

		private static bool FilterExplictOperator( MemberInfo currentMember, object typesAsCriteria )
		{
			var tuple = typesAsCriteria as Tuple<Type, Type>;
			return FilterConversionOperator( currentMember, tuple.Item1, tuple.Item2, "op_Explicit" );
		}

		private static bool FilterConversionOperator( MemberInfo currentMember, Type sourceType, Type destinationType, string operatorName )
		{
			var method = currentMember as MethodInfo;
			if ( method == null || ( method.Attributes | MethodAttributes.SpecialName ) == 0 || method.Name != operatorName )
			{
				return false;
			}

			var parameters = method.GetParameters();
			return parameters.Length == 1 && parameters[ 0 ].ParameterType.IsAssignableFrom( ( sourceType ) ) && destinationType.IsAssignableFrom( method.ReturnType );
		}

		private static MethodBase ResolveOverload( MethodBase[] overloads )
		{
			switch ( overloads.Length )
			{
				case 0:
				{
					return null;
				}
				case 1:
				{
					return overloads[ 0 ];
				}
			}

			// HACK: naive bubling method...
			var candidates = overloads.ToDictionary( method => method.GetParameters()[ 0 ].ParameterType );
			var candidateTypes = candidates.Keys.ToArray();
			for ( int i = 0; 1 < candidates.Count && i < candidateTypes.Length; i++ )
			{
				var removals = candidates.Keys.Where( key => key != candidateTypes[ i ] && key.IsAssignableFrom( candidateTypes[ i ] ) ).ToArray();
				foreach ( var removal in removals )
				{
					candidates.Remove( removal );
				}
			}

			if ( candidates.Count != 1 )
			{
				throw new AmbiguousMatchException( String.Format( FormatProviders.CurrentCulture, NLiblet.Properties.Resources.GeneratedCodeHelper_CannotResolveOverload, overloads as object ) );
			}

			return candidates.First().Value;
		}

		private static readonly Type[] _standardConversionErrors =
			new[] { typeof( InvalidCastException ), typeof( ArgumentException ), typeof( OverflowException ), typeof( NotSupportedException ) };


		private static Delegate CreateUnboxAndConvCastMethod( Type delegateType, Type sourceType, Type targetType, bool isTracingEnabled )
		{
			/**
			 *	target method:
			 *	public static TPrimitive Cast( object source )
			 *	{
			 *		return ( TPrimitive )UnboxPrimitive( source ); // throws OverflowException
			 *	}
			 */

			var dynamicMethod = new DynamicMethod( CreateCastMethodName( sourceType, targetType, false ), targetType, new Type[] { sourceType, typeof( int ) }, typeof( GeneratedCodeHelper ), false );

			var buffer = isTracingEnabled ? new StringBuilder() : null;
			using ( var trace = isTracingEnabled ? new StringWriter( buffer, CultureInfo.InvariantCulture ) : null )
			using ( var il = new TracingILGenerator( dynamicMethod, trace ) )
			{
				EmitUnboxAndConvCastMethodBody( targetType, il );
				il.EmitRet();

				if ( trace != null )
				{
					trace.Flush();
				}
			}

			ILEmittion.TraceIL( ILEmittion.EmitUnboxAndConvCastEventId, dynamicMethod, buffer );

			return dynamicMethod.CreateDelegate( delegateType );

		}

		private static void EmitUnboxAndConvCastMethodBody( Type targetType, TracingILGenerator il )
		{
			il.EmitGetProperty( InstanceProperty );
			il.EmitLdarg_0();
			switch ( Type.GetTypeCode( targetType ) )
			{
				case TypeCode.SByte:
				{
					il.EmitAnyCall( UnboxPrimitiveIntegerMethod );
					il.EmitConv_I1();
					break;
				}
				case TypeCode.Int16:
				{
					il.EmitAnyCall( UnboxPrimitiveIntegerMethod );
					il.EmitConv_I2();
					break;
				}
				case TypeCode.Int32:
				{
					il.EmitAnyCall( UnboxPrimitiveIntegerMethod );
					il.EmitConv_I4();
					break;
				}
				case TypeCode.Int64:
				{
					il.EmitAnyCall( UnboxPrimitiveIntegerMethod );
					il.EmitConv_I8();
					break;
				}
				case TypeCode.Byte:
				{
					il.EmitAnyCall( UnboxPrimitiveIntegerMethod );
					il.EmitConv_U1();
					break;
				}
				case TypeCode.UInt16:
				{
					il.EmitAnyCall( UnboxPrimitiveIntegerMethod );
					il.EmitConv_U2();
					break;
				}
				case TypeCode.UInt32:
				{
					il.EmitAnyCall( UnboxPrimitiveIntegerMethod );
					il.EmitConv_U4();
					break;
				}
				case TypeCode.UInt64:
				{
					il.EmitAnyCall( UnboxPrimitiveIntegerMethod );
					break;
				}
				case TypeCode.Single:
				{
					il.EmitAnyCall( UnboxPrimitiveRealMethod );
					il.EmitConv_R4();
					break;
				}
				case TypeCode.Double:
				{
					il.EmitAnyCall( UnboxPrimitiveRealMethod );
					break;
				}
			}
		}


		private static Delegate CreateUnboxAndConvCastMethodOfArrayItem( Type delegateType, Type sourceType, Type targetType, bool isTracingEnabled )
		{
			/**
			 *	target method:
			 *	public static TDestination Cast( TSource source, int arrayIndex )
			 *	{
			 *		try
			 *		{
			 *			return ( TDestination ) source; // throws InvalidCastException
			 *		}
			 *		catch( ...)
			 *		{
			 *			...
			 *		}
			 *	}
			 */

			return CreateCastMethodOfArrayItem(
				delegateType,
				sourceType,
				targetType,
				isTracingEnabled,
				il =>
				{
					EmitUnboxAndConvCastMethodBody( targetType, il );
				},
				_standardConversionErrors
			);
		}

		private static Delegate CreateUnboxAnyCastMethod( Type delegateType, Type sourceType, Type targetType, bool isTracingEnabled )
		{
			/**
			 *	target method:
			 *	public static TDestination Cast( TSource source )
			 *	{
			 *		return ( TDestination ) source; // throws InvalidCastException
			 *	}
			 */

			var dynamicMethod = new DynamicMethod( CreateCastMethodName( sourceType, targetType, false ), targetType, new Type[] { sourceType, typeof( int ) }, typeof( GeneratedCodeHelper ), false );

			var buffer = isTracingEnabled ? new StringBuilder() : null;
			using ( var trace = isTracingEnabled ? new StringWriter( buffer, CultureInfo.InvariantCulture ) : null )
			using ( var il = new TracingILGenerator( dynamicMethod, trace ) )
			{
				il.EmitLdarg_0();
				il.EmitUnbox_Any( targetType );
				il.EmitRet();

				if ( trace != null )
				{
					trace.Flush();
				}
			}

			ILEmittion.TraceIL( ILEmittion.EmitUnboxAnyCastEventId, dynamicMethod, buffer );

			return dynamicMethod.CreateDelegate( delegateType );
		}

		private static Delegate CreateUnboxAnyCastMethodOfArrayItem( Type delegateType, Type sourceType, Type targetType, bool isTracingEnabled )
		{
			/**
			 *	target method:
			 *	public static TDestination Cast( TSource source, int arrayIndex )
			 *	{
			 *		try
			 *		{
			 *			return ( TDestination ) source; // throws InvalidCastException
			 *		}
			 *		catch( ...)
			 *		{
			 *			...
			 *		}
			 *	}
			 */

			return CreateCastMethodOfArrayItem(
				delegateType,
				sourceType,
				targetType,
				isTracingEnabled,
				il =>
				{
					il.EmitLdarg_0();
					il.EmitUnbox_Any( targetType );
				},
				_standardConversionErrors
			);
		}

		private static Delegate CreateOperatorCastMethod( Type delegateType, Type sourceType, Type targetType, bool isTracingEnabled, MethodInfo conversionOperator )
		{
			/**
			 *	target method:
			 *	public static TDestination Cast( TSource source )
			 *	{
			 *		return ( TDestination ) source; // throws any Exception
			 *	}
			 */

			var dynamicMethod = new DynamicMethod( CreateCastMethodName( sourceType, targetType, false ), targetType, new Type[] { sourceType, typeof( int ) }, typeof( GeneratedCodeHelper ), false );
			var buffer = isTracingEnabled ? new StringBuilder() : null;
			using ( var trace = isTracingEnabled ? new StringWriter( buffer, CultureInfo.InvariantCulture ) : null )
			using ( var il = new TracingILGenerator( dynamicMethod, trace ) )
			{
				il.EmitLdarg_0();
				il.EmitAnyCall( conversionOperator );
				il.EmitRet();

				trace.Flush();
			}

			ILEmittion.TraceIL( ILEmittion.EmitOperatorCastEventId, dynamicMethod, buffer );

			return dynamicMethod.CreateDelegate( delegateType );
		}

		private static Delegate CreateOperatorCastMethodOfArrayItem( Type delegateType, Type sourceType, Type targetType, bool isTracingEnabled, MethodInfo conversionOperator )
		{
			/**
			 *	target method:
			 *	public static TDestination Cast( TSource source, int arrayIndex )
			 *	{
			 *		try
			 *		{
			 *			return ( TDestination ) source; // throws InvalidCastException
			 *		}
			 *		catch(...)
			 *		{
			 *			...
			 *		}
			 *	}
			 */

			// op_Explicit may throw OverflowException or ArgumentException ( or its sub exception like FormatException, ArgumentNullException, etc.)
			return CreateCastMethodOfArrayItem(
				delegateType,
				sourceType,
				targetType,
				isTracingEnabled,
				il =>
				{
					il.EmitLdarg_0();
					il.EmitAnyCall( conversionOperator );
				},
				_standardConversionErrors
			);
		}

		private static Delegate CreateConstructorCastMethod( Type delegateType, Type sourceType, Type targetType, bool isTracingEnabled, ConstructorInfo conversionConstructor )
		{
			/**
			 *	target method:
			 *	public static TDestination Cast( TSource source )
			 *	{
			 *		return new TDestination( source ); // throws any Exception
			 *	}
			 */

			var dynamicMethod = new DynamicMethod( CreateCastMethodName( sourceType, targetType, false ), targetType, new Type[] { sourceType, typeof( int ) }, typeof( GeneratedCodeHelper ), false );
			var buffer = isTracingEnabled ? new StringBuilder() : null;
			using ( var trace = isTracingEnabled ? new StringWriter( buffer, CultureInfo.InvariantCulture ) : null )
			using ( var il = new TracingILGenerator( dynamicMethod, trace ) )
			{
				il.EmitLdarg_0();
				il.EmitNewobj( conversionConstructor );
				il.EmitRet();

				if ( trace != null )
				{
					trace.Flush();
				}
			}

			ILEmittion.TraceIL( ILEmittion.EmitConstructorCastEventId, dynamicMethod, buffer );

			return dynamicMethod.CreateDelegate( delegateType );
		}

		private static Delegate CreateConstructorCastMethodOfArrayItem( Type delegateType, Type sourceType, Type targetType, bool isTracingEnabled, ConstructorInfo conversionConstructor )
		{
			/**
			 *	target method:
			 *	public static TDestination Cast( TSource source, int arrayIndex )
			 *	{
			 *		try
			 *		{
			 *			return new TDestination( source ); // throws any Exception
			 *		}
			 *		catch(...)
			 *		{
			 *			...
			 *		}
			 *	}
			 */

			// Conversion constructor may throw OverflowException or ArgumentException ( or its sub exception like FormatException, ArgumentNullException, etc.)
			return CreateCastMethodOfArrayItem(
				delegateType,
				sourceType,
				targetType,
				isTracingEnabled,
				il =>
				{
					il.EmitLdarg_0();
					il.EmitNewobj( conversionConstructor );
				},
				_standardConversionErrors
			);
		}


		private static Delegate CreateCastMethodOfArrayItem( Type delegateType, Type sourceType, Type targetType, bool isTracingEnabled, Action<TracingILGenerator> bodyEmittion, params Type[] catchingExceptionTypes )
		{
			/**
			 *	target method:
			 *	public static TDestination Cast( TSource source, int arrayIndex )
			 *	{
			 *		try
			 *		{
			 *			BODY
			 *		}
			 *		catch( InvalidCastException e )
			 *		{
			 *			throw new ArgumentException( 
			 *				String.Format(
			 *					CultureInfo.CurrentCulture,
			 *					Resources.CannotCastArrayElementAt, 
			 *					arrayIndex, 
			 *					typeof( TSource ),
			 *					typeof( TTarget )
			 *				),
			 *				"array",
			 *				e
			 *			);
			 *		}
			 *		
			 *		return L_0;
			 *	}
			 */

			var dynamicMethod = new DynamicMethod( CreateCastMethodName( sourceType, targetType, false ), targetType, new Type[] { sourceType, typeof( int ) }, typeof( GeneratedCodeHelper ), false );
			var buffer = isTracingEnabled ? new StringBuilder() : null;
			using ( var trace = isTracingEnabled ? new StringWriter( buffer, CultureInfo.InvariantCulture ) : null )
			using ( var il = new TracingILGenerator( dynamicMethod, trace ) )
			{
				var resultLocal = il.DeclareLocal( targetType, "result" );
				var formatArgsLocal = il.DeclareLocal( typeof( object[] ) );
				var exLocals = catchingExceptionTypes.Select( item => il.DeclareLocal( item, "ex" ) ).ToArray();
				var endOfMethod = il.DefineLabel( "END_OF_METHOD" );
				il.BeginExceptionBlock();
				bodyEmittion( il );
				il.EmitAnyStloc( resultLocal.LocalIndex );

				for ( int i = 0; i < catchingExceptionTypes.Length; i++ )
				{
					il.BeginCatchBlock( catchingExceptionTypes[ i ] );
					il.EmitAnyStloc( exLocals[ i ].LocalIndex );
					il.EmitStringFormat(
						formatArgsLocal.LocalIndex,
						typeof( GeneratedCodeHelper ),
						"Resource_CastCode_CannotCastArrayItemAt",
						il0 =>
						{
							il0.EmitLdarg_1();
							il0.EmitBox( typeof( int ) );
						},
						il0 =>
						{
							/*
							 * arg0 == null ? typeof( T0 ) : arg0.GetType()
							 */
							var elseConditional = il0.DefineLabel( "ELSE_CONDITIONAL" );
							var endConditional = il0.DefineLabel( "END_CONDITIONAL" );
							il0.EmitLdarg_0();
							il0.EmitBrfalse_S( elseConditional );
							il0.EmitLdarg_0();
							il0.EmitGetType();
							il0.EmitBr_S( endConditional );
							il0.MarkLabel( elseConditional );
							il0.EmitTypeOf( sourceType );
							il0.MarkLabel( endConditional );
						},
						il0 => il0.EmitTypeOf( targetType )
					);
					il.EmitLdstr( "array" );
					il.EmitAnyLdloc( exLocals[ i ].LocalIndex );
					il.EmitThrowNewArgumentExceptionWithInnerException();
				}

				il.EndExceptionBlock();
				il.MarkLabel( endOfMethod );
				il.EmitAnyLdloc( resultLocal.LocalIndex );
				il.EmitRet();

				if ( trace != null )
				{
					trace.Flush();
				}
			}

			ILEmittion.TraceIL( ILEmittion.EmitCastMethodOfArrayItemEventId, dynamicMethod, buffer );

			return dynamicMethod.CreateDelegate( delegateType );

		}

		private static readonly MethodInfo _TypeDescriptor_GetConverter =
			typeof( TypeDescriptor ).GetMethod( "GetConverter", new[] { typeof( Type ) } );

		private static readonly MethodInfo _TypeConverter_ConvertFrom =
			typeof( TypeConverter ).GetMethod( "ConvertFrom", new[] { typeof( object ) } );

		private static readonly MethodInfo _TypeConverter_IsValid =
			typeof( TypeConverter ).GetMethod( "IsValid", new[] { typeof( object ) } );

		private static Delegate CreateTypeConverterCastMethod( Type delegateType, Type sourceType, Type targetType, bool isTracingEnabled )
		{
			/**
			 *	target method:
			 *	public static TDestination Cast( TSource source )
			 *	{
			 *		var converter = TypeDescriptor.GetConverter( typeof( TSource ) );
			 * 		object converted = null;
			 * 		try
			 * 		{
			 * 			// Some type converter cannot treat null to null pass through.
			 * 			if ( source == null && !converter.IsValid( default( TSource ) ) )
			 * 			{
			 * 				return default( TDestination );
			 * 			}
			 * 
			 * 			converted = converter.ConvertFrom( source );
			 * 		}
			 * 		catch ( NotEsupprotedException ex )
			 * 		{
			 *			throw new ArgumentException( ..., ex );
			 *		}
			 * 
			 *		return ( TTarget )converted;
			 *	}
			 */

			return
				CreateTypeConverterCastMethodCore(
					delegateType,
					sourceType,
					targetType,
					isTracingEnabled,
					( il, exLocal, formatArgsLocal, converterLocal ) =>
					{
						il.EmitStringFormat(
							formatArgsLocal.LocalIndex,
							typeof( GeneratedCodeHelper ),
							"Resource_CastCode_CannotCastWithTypeConverter",
							( il0 ) => il0.EmitTypeOf( sourceType ),
							( il0 ) => il0.EmitTypeOf( targetType ),
							( il0 ) =>
							{
								il0.EmitAnyLdloc( converterLocal.LocalIndex );
								il0.EmitGetType();
							}
						);
						il.EmitAnyLdloc( exLocal.LocalIndex );
						il.EmitThrowNewExceptionWithInnerException( typeof( InvalidCastException ) );
					}
				);
		}

		private static Delegate CreateTypeConverterCastMethodOfArrayItem( Type delegateType, Type sourceType, Type targetType, bool isTracingEnabled )
		{
			/**
			 *	target method:
			 *	public static TDestination Cast( TSource source, int index )
			 *	{
			 *		var converter = TypeDescriptor.GetConverter( typeof( TSource ) );
			 * 		object converted = null;
			 * 		try
			 * 		{
			 * 			// Some type converter cannot treat null to null pass through.
			 * 			if ( source == null && !converter.IsValid( default( TSource ) ) )
			 * 			{
			 * 				return default( TDestination );
			 * 			}
			 * 
			 * 			converted = converter.ConvertFrom( source );
			 * 		}
			 * 		catch ( NotEsupprotedException ex )
			 * 		{
			 *			throw new ArgumentException( ..., ex );
			 *		}
			 * 
			 *		return ( TTarget )converted;
			 *	}
			 */

			return
				CreateTypeConverterCastMethodCore(
					delegateType,
					sourceType,
					targetType,
					isTracingEnabled,
					( il, exLocal, formatArgsLocal, converterLocal ) =>
					{
						il.EmitStringFormat(
							formatArgsLocal.LocalIndex,
							typeof( GeneratedCodeHelper ),
							"Resource_CastCode_CannotCastArrayItemWithTypeConverterAt",
							il0 =>
							{
								il0.EmitLdarg_1();
								il0.EmitBox( typeof( int ) );
							},
							il0 =>
							{
								/*
								 * arg0 == null ? typeof( T0 ) : arg0.GetType()
								 */
								var elseConditional = il0.DefineLabel( "ELSE_CONDITIONAL" );
								var endConditional = il0.DefineLabel( "END_CONDITIONAL" );
								il0.EmitLdarg_0();
								il0.EmitBrfalse_S( elseConditional );
								il0.EmitLdarg_0();
								il0.EmitGetType();
								il0.EmitBr_S( endConditional );
								il0.MarkLabel( elseConditional );
								il0.EmitTypeOf( sourceType );
								il0.MarkLabel( endConditional );
							},
							il0 => il0.EmitTypeOf( targetType ),
							il0 =>
							{
								il0.EmitAnyLdloc( converterLocal.LocalIndex );
								il0.EmitGetType();
							}
						);
						il.EmitLdstr( "array" );
						il.EmitAnyLdloc( exLocal.LocalIndex );
						il.EmitThrowNewArgumentExceptionWithInnerException();
					}
				);
		}

		private static Delegate CreateTypeConverterCastMethodCore( Type delegateType, Type sourceType, Type targetType, bool isTracingEnabled, Action<TracingILGenerator, LocalBuilder, LocalBuilder, LocalBuilder> catchBodyEmittion )
		{
			/**
			 *	target method:
			 *	public static TDestination Cast( TSource source )
			 *	{
			 *		var converter = TypeDescriptor.GetConverter( typeof( TSource ) );
			 * 		object converted = null;
			 * 		try
			 * 		{
			 * 			// Some type converter cannot treat null to null pass through.
			 * 			if ( source == null && !converter.IsValid( default( TSource ) ) )
			 * 			{
			 * 				return default( TDestination );
			 * 			}
			 * 
			 * 			converted = converter.ConvertFrom( source );
			 * 		}
			 * 		catch ( NotEsupprotedException ex )
			 * 		{
			 *			CATCH_BODY
			 *		}
			 * 
			 *		return ( TTarget )converted;
			 *	}
			 */

			var dynamicMethod = new DynamicMethod( CreateCastMethodName( sourceType, targetType, false ), targetType, new Type[] { sourceType, typeof( int ) }, typeof( GeneratedCodeHelper ), false );
			var buffer = isTracingEnabled ? new StringBuilder() : null;
			using ( var trace = isTracingEnabled ? new StringWriter( buffer, CultureInfo.InvariantCulture ) : null )
			using ( var il = new TracingILGenerator( dynamicMethod, trace ) )
			{
				var converterLocal = il.DeclareLocal( typeof( TypeConverter ), "typeConverter" );
				var convertedLocal = il.DeclareLocal( typeof( object ), "converted" );
				var defaultLocal = default( LocalBuilder );
				if ( targetType.IsValueType )
				{
					defaultLocal = il.DeclareLocal( targetType );
				}
				var formatArgsLocal = il.DeclareLocal( typeof( object[] ) );
				var exLocals = _standardConversionErrors.Select( item => il.DeclareLocal( item, "ex" ) ).ToArray();
				il.EmitTypeOf( targetType );
				il.EmitAnyCall( _TypeDescriptor_GetConverter );
				il.EmitAnyStloc( converterLocal.LocalIndex );

				var endOfMethod = il.DefineLabel( "END_OF_METHOD" );
				var endIf = il.DefineLabel( "END_IF" );

				il.BeginExceptionBlock();

				// if( source == null ..
				il.EmitLdarg_0();
				il.EmitLdnull();
				il.EmitCeq();
				il.EmitBrfalse_S( endIf );
				// && !converter.IsValid( default( TSource ) )
				il.EmitLdloc_0();
				il.EmitLdnull();
				il.EmitAnyCall( _TypeConverter_IsValid );
				il.EmitBrtrue_S( endIf );
				// return default(T)
				if ( targetType.IsValueType )
				{
					il.EmitLdloca_S( ( byte )defaultLocal.LocalIndex );
					il.EmitInitobj( targetType );
					il.EmitAnyLdloc( defaultLocal.LocalIndex );
					il.EmitBox( targetType );
				}
				else
				{
					il.EmitLdnull();
				}
				il.EmitAnyStloc( convertedLocal.LocalIndex );
				il.EmitLeave( endOfMethod );

				il.MarkLabel( endIf );
				// converted = converter.ConvertFrom( source )
				il.EmitAnyLdloc( converterLocal.LocalIndex );
				il.EmitLdarg_0();
				il.EmitAnyCall( _TypeConverter_ConvertFrom );
				il.EmitAnyStloc( convertedLocal.LocalIndex );

				for ( int i = 0; i < _standardConversionErrors.Length; i++ )
				{
					il.BeginCatchBlock( _standardConversionErrors[ i ] );
					il.EmitAnyStloc( exLocals[ i ].LocalIndex );
					catchBodyEmittion( il, exLocals[ i ], formatArgsLocal, converterLocal );
				}

				il.EndExceptionBlock();
				il.MarkLabel( endOfMethod );
				il.EmitAnyLdloc( convertedLocal.LocalIndex );
				il.EmitUnbox_Any( targetType );
				il.EmitRet();

				if ( trace != null )
				{
					trace.Flush();
				}
			}

			ILEmittion.TraceIL( ILEmittion.EmitTypeConverterCastEventId, dynamicMethod, buffer );

			return dynamicMethod.CreateDelegate( delegateType );
		}

		private static string CreateCastMethodName( Type sourceType, Type targetType, bool isArrayElementCast )
		{
			var sourceAssemblyName = sourceType.Assembly.GetName();
			var targetAssemblyName = targetType.Assembly.GetName();

			var name = new StringBuilder( 256 );
			name.Append( isArrayElementCast ? "CastArrayElement" : "Cast" ).Append( '_' )
				.Append( sourceType.Name ).Append( '_' )
				.Append( sourceAssemblyName.Name ).Append( '_' )
				.Append( sourceAssemblyName.Version ).Append( '_' )
				.Append( HexFormat.ToHexString( sourceAssemblyName.GetPublicKeyToken() ) ).Append( '_' )
				.AppendFormat( "{0:x}", sourceType.TypeHandle.Value ).Append( '_' )
				.Append( targetType.Name ).Append( '_' )
				.Append( targetAssemblyName.Name ).Append( '_' )
				.Append( targetAssemblyName.Version ).Append( '_' )
				.Append( HexFormat.ToHexString( targetAssemblyName.GetPublicKeyToken() ) ).Append( '_' )
				.AppendFormat( "{0:x}", sourceType.TypeHandle.Value );
			return name.ToString();
		}

		internal static readonly PropertyInfo InstanceProperty = typeof( GeneratedCodeHelper ).GetProperty( "Instance" );
		internal static readonly MethodInfo CastMethod = typeof( GeneratedCodeHelper ).GetMethod( "Cast" );
		internal static readonly MethodInfo CastArrayItemMethod = typeof( GeneratedCodeHelper ).GetMethod( "CastArrayItem" );
	}

#pragma warning restore 1572, 1587, 1591
}
