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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace NLiblet.Reflection
{
	internal static class ShimCodeGenerator
	{
		private static bool _isTracing;

		public static bool IsTracing
		{
			get { return _isTracing; }
			set { _isTracing = value; }
		}

		/// <summary>
		///		CreateShim&lt;TDelegate&gt;(MethodBase,Type[],Type,Type,bool)
		/// </summary>
		internal static readonly MethodInfo CreateShimMethod =
			typeof( ShimCodeGenerator ).GetMethod( "CreateShim", BindingFlags.Public | BindingFlags.Static );

		public static TDelegate CreateShim<TDelegate>(
			MethodBase destination,
			Type[] parameterTypes,
			Type returnType,
			Type shimHostType,
			bool skipVisibility
		)
			where TDelegate : class
		{
			// Target code is following.
			/*
			 * public static TReturn <FullName>$InvocationShim$Token(T0 arg0,...,Tn argn)
			 * {
			 *		return target(arg0, ..., argn );
			 * }
			 * 
			 */

			// We don't want to have double code base, so we cannot use Expression<T>.

			var method = DefineDynamicMehtod( destination, parameterTypes, returnType, shimHostType, skipVisibility );
			var buffer = IsTracing ? new StringBuilder() : null;
			using ( var tracer = IsTracing ? new StringWriter( buffer, CultureInfo.InvariantCulture ) : null )
			using ( var il = new TracingILGenerator( method, tracer ) )
			{
				// Push arguments from param array with casting.
				for ( int i = 0; i < parameterTypes.Length; i++ )
				{
					il.EmitAnyLdarg( i );
				}

				// Invoke target
				Type returningType = null;
				var asMethod = destination as MethodInfo;
				if ( asMethod != null )
				{
					returningType = asMethod.ReturnType;
					il.EmitAnyCall( asMethod );
				}
				else
				{
					var asConstructor = destination as ConstructorInfo;
					Contract.Assert( asConstructor != null );
					returningType = asConstructor.DeclaringType;
					// It is OK to call .ctor of value type.
					il.EmitNewobj( asConstructor );
				}

				Contract.Assert( returningType != null );

				// Ret
				il.EmitRet();

				if ( tracer != null )
				{
					tracer.Flush();
				}
			}

			ILEmittion.TraceIL( ILEmittion.EmitInvocationShimEventId, method, buffer );
			return method.CreateDelegate( typeof( TDelegate ) ) as TDelegate;

		}

		private static readonly Type[] _generalShimPamrameterTypes = new[] { typeof( object ), typeof( object[] ) };

		/// <summary>
		///		CreateLooseProcedureInvocationShim(MethodBase,Type,bool)
		/// </summary>
		internal static readonly MethodInfo CreateLooseProcedureInvocationShimMethod =
			typeof( ShimCodeGenerator ).GetMethod( "CreateLooseProcedureInvocationShim", BindingFlags.Public | BindingFlags.Static );

		public static Action<object, object[]> CreateLooseProcedureInvocationShim(
			MethodBase destination,
			Type shimHostType,
			bool skipVisibility
		)
		{
			Contract.Assert( destination is MethodInfo );
			Contract.Assert( ( destination as MethodInfo ).ReturnType == typeof( void ) );

			return CreateLooseInvocationShim<Action<object, object[]>>( destination, shimHostType, skipVisibility );
		}

		/// <summary>
		///		CreateLooseFunctionInvocationShim(MethodBase,Type,bool)
		/// </summary>
		internal static readonly MethodInfo CreateLooseFunctionInvocationShimMethod =
			typeof( ShimCodeGenerator ).GetMethod( "CreateLooseFunctionInvocationShim", BindingFlags.Public | BindingFlags.Static );

		public static Func<object, object[], object> CreateLooseFunctionInvocationShim(
			MethodBase destination,
			Type shimHostType,
			bool skipVisibility
		)
		{
			return CreateLooseInvocationShim<Func<object, object[], object>>( destination, shimHostType, skipVisibility );
		}

		private static TDelegate CreateLooseInvocationShim<TDelegate>(
			MethodBase destination,
			Type shimHostType,
			bool skipVisibility
		)
			where TDelegate : class
		{
			// Target code is following.
			/*
			 * public static object <FullName>$InvocationShim$Token(object[] args)
			 * {
			 *		return 
			 *			( object ) target(
			 *				Cast<T0>( args, 0 ),
			 *				:
			 *				Cast<Tn>( args, n )
			 *			);
			 * }
			 * 
			 */

			// We don't want to have double code base, so we cannot use Expression<T>.
			Type returningType = null;
			var asMethod = destination as MethodInfo;
			var asConstructor = destination as ConstructorInfo;
			if ( asMethod != null )
			{
				returningType = asMethod.ReturnType;
			}
			else
			{
				Contract.Assert( asConstructor != null );
				returningType = asConstructor.DeclaringType;
				// It is OK to call .ctor of value type.
			}
			var method = DefineDynamicMehtod( destination, _generalShimPamrameterTypes, typeof( void ).TypeHandle.Equals( returningType.TypeHandle ) ? null : typeof( object ), shimHostType, skipVisibility );
			var buffer = IsTracing ? new StringBuilder() : null;
			using ( var tracer = IsTracing ? new StringWriter( buffer, CultureInfo.InvariantCulture ) : null )
			using ( var il = new TracingILGenerator( method, tracer ) )
			{
				var destinationParameters = destination.GetParameters();

				if ( !destination.IsStatic && !destination.IsConstructor )
				{
					il.EmitGetProperty( GeneratedCodeHelper.InstanceProperty );
					il.EmitLdarg_0();
					il.EmitAnyCall( GeneratedCodeHelper.CastMethod.MakeGenericMethod( typeof( object ), destination.DeclaringType ) );
				}

				// Push arguments from param array with casting.
				for ( int i = 0; i < destinationParameters.Length; i++ )
				{
					il.EmitGetProperty( GeneratedCodeHelper.InstanceProperty );
					il.EmitLdarg_1();
					il.EmitLiteralInteger( i );
					il.EmitAnyCall( GeneratedCodeHelper.CastArrayItemMethod.MakeGenericMethod( destinationParameters[ i ].ParameterType ) );
				}

				// Invoke target
				if ( asMethod != null )
				{
					il.EmitAnyCall( asMethod );
				}
				else
				{
					il.EmitNewobj( asConstructor );
				}

				// Box return value if necessary
				if ( !typeof( void ).TypeHandle.Equals( returningType.TypeHandle ) && returningType.IsValueType )
				{
					il.EmitBox( returningType );
				}

				// Ret
				il.EmitRet();

				if ( tracer != null )
				{
					tracer.Flush();
				}
			}

			ILEmittion.TraceIL( ILEmittion.EmitInvocationShimEventId, method, buffer );

			return method.CreateDelegate( typeof( TDelegate ) ) as TDelegate;
		}

		private static DynamicMethod DefineDynamicMehtod(
			MemberInfo destination,
			Type[] shimParameterTypes,
			Type shimReturnType,
			Type shimHostType,
			bool skipVisibility
		)
		{
			var shimMethodName = new StringBuilder( destination.DeclaringType.FullName.Length + destination.Name.Length + 32 );
			shimMethodName.Append( destination.DeclaringType.FullName ).Append( destination.Name ).Append( "$InvocationShim$" ).Append( GetToken( destination ).ToString( "x" ) ).Replace( Type.Delimiter, '$' );
			if ( shimHostType == null )
			{
				return new DynamicMethod( shimMethodName.ToString(), shimReturnType, shimParameterTypes, skipVisibility );
			}
			else
			{
				return new DynamicMethod( shimMethodName.ToString(), shimReturnType, shimParameterTypes, shimHostType, skipVisibility );
			}
		}

		private static IntPtr GetToken( MemberInfo memberInfo )
		{
			var asMethodbase = memberInfo as MethodBase;
			if ( asMethodbase != null )
			{
				return asMethodbase.MethodHandle.Value;
			}

			var asFieldInfo = memberInfo as FieldInfo;
			if ( asFieldInfo != null )
			{
				return asFieldInfo.FieldHandle.Value;
			}

			Contract.Assert( false, memberInfo.GetType().FullName );
			return IntPtr.Zero;
		}
	}
}
