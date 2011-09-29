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
using System.Linq;
using System.Reflection;
using NLiblet.Properties;
using System.Diagnostics;
using NLiblet.Text;

namespace NLiblet.Reflection
{
	/// <summary>
	///		Define extension method of <see cref="MethodInfo"/>.
	/// </summary>
	public static class MemberInfoExtensions
	{
		private static readonly Type[] _actions =
			new[]
			{
				typeof( Action ), 
				typeof( Action<> ), 
				typeof( Action<,> ), 
				typeof( Action<,,> ), 
				typeof( Action<,,,> ),
				typeof( Action<,,,,> ),
				typeof( Action<,,,,,> ),
				typeof( Action<,,,,,,> ),
				typeof( Action<,,,,,,,> ),
				typeof( Action<,,,,,,,,> ),
				typeof( Action<,,,,,,,,,> ),
				typeof( Action<,,,,,,,,,,> ),
				typeof( Action<,,,,,,,,,,,> ),
				typeof( Action<,,,,,,,,,,,,> ),
				typeof( Action<,,,,,,,,,,,,,> ),
				typeof( Action<,,,,,,,,,,,,,,> ),
				typeof( Action<,,,,,,,,,,,,,,,> )
			};

		private static readonly Type[] _funcs =
			new[]
			{
				typeof( Func<> ), 
				typeof( Func<,> ), 
				typeof( Func<,,> ), 
				typeof( Func<,,,> ),
				typeof( Func<,,,,> ),
				typeof( Func<,,,,,> ),
				typeof( Func<,,,,,,> ),
				typeof( Func<,,,,,,,> ),
				typeof( Func<,,,,,,,,> ),
				typeof( Func<,,,,,,,,,> ),
				typeof( Func<,,,,,,,,,,> ),
				typeof( Func<,,,,,,,,,,,> ),
				typeof( Func<,,,,,,,,,,,,> ),
				typeof( Func<,,,,,,,,,,,,,> ),
				typeof( Func<,,,,,,,,,,,,,,> ),
				typeof( Func<,,,,,,,,,,,,,,,> ),
				typeof( Func<,,,,,,,,,,,,,,,,> )
			};

		/// <summary>
		///		Create delegate for invoke specified public method.
		/// </summary>
		/// <param name="source"><see cref="MethodInfo"/>.</param>
		/// <returns>
		///		Delegate to invoke <paramref name="source"/>.
		///		Its type is any of Action (when return value is void) or Func (otherwise).
		///		If <paramref name="source"/> is instance method, then 1st parameter is target instance itself.
		///	</returns>
		///	<exception cref="NotSupportedException">
		///		<paramref name="source"/> has too many parameters.
		///		Or <paramref name="source"/> is not <see cref="MethodInfo"/> nor <see cref="ConstructorInfo"/>.
		///		Or <paramref name="source"/> is type initializer (static constructor/class constructor).
		///	</exception>
		/// <remarks>
		///		By invoking via delegate, you can avoid <see cref="TargetInvocationException"/> problem.
		///		You can catch the exception thrown by target or deeper methods instead of traversing <see cref="Exception.InnerException"/> of <see cref="TargetInvocationException"/>.
		///		<note>
		///			This method supports up to 16 parameters for static method and up to 15 parameters for instance method.
		///		</note>
		/// </remarks>
		public static Delegate CreateDelegate( this MethodBase source )
		{
			Contract.Requires<ArgumentNullException>( source != null );
			Contract.Requires<ArgumentException>( !source.ContainsGenericParameters );

			return CreateDelegate( source, typeof( GeneratedCodeHelper ) );
		}

		// TODO: Describe CAS restriction.
		/// <summary>
		///		Create delegate for invoke specified method in scope of host type.
		/// </summary>
		/// <param name="source"><see cref="MethodInfo"/>.</param>
		/// <param name="hostType"><see cref="Type"/> will be associated with generated shim code.</param>
		/// <returns>
		///		Delegate to invoke <paramref name="source"/>.
		///		Its type is any of Action (when return value is void) or Func (otherwise).
		///		If <paramref name="source"/> is instance method, then 1st parameter is target instance itself.
		///	</returns>
		///	<exception cref="NotSupportedException">
		///		<paramref name="source"/> has too many parameters.
		///		Or <paramref name="source"/> is not <see cref="MethodInfo"/> nor <see cref="ConstructorInfo"/>.
		///		Or <paramref name="source"/> is type initializer (static constructor/class constructor).
		///	</exception>
		/// <remarks>
		///		By invoking via delegate, you can avoid <see cref="TargetInvocationException"/> problem.
		///		You can catch the exception thrown by target or deeper methods instead of traversing <see cref="Exception.InnerException"/> of <see cref="TargetInvocationException"/>.
		///		<note>
		///			This method supports up to 16 parameters for static method and up to 15 parameters for instance method.
		///		</note>
		/// </remarks>
		public static Delegate CreateDelegate( this MethodBase source, Type hostType )
		{
			Contract.Requires<ArgumentNullException>( source != null );
			Contract.Requires<ArgumentException>( !source.ContainsGenericParameters );
			Contract.Requires<ArgumentNullException>( hostType != null );

			var parameters = source.GetParameters();
			var shimParameterCount = parameters.Length + ( ( source.IsStatic || source.IsConstructor ) ? 0 : 1 );

			if ( 16 < shimParameterCount )
			{
				throw new NotSupportedException(
					String.Format(
						CultureInfo.CurrentCulture,
						Resources.MethodInfoExtensions_TooManyParameters,
						source.DeclaringType.Module.Name,
						source.DeclaringType.FullName,
						source.Name + "(" + String.Join( ", ", parameters.Select( item => item.ParameterType.FullName ) ) + ")"
					)
				);
			}

			var asMethodInfo = source as MethodInfo;
			Type returnType;
			if ( source.IsConstructor )
			{
				if ( source.IsStatic )
				{
					throw new NotSupportedException( Resources.Reflection_TypeInitializerIsNotSupported );
				}

				returnType = source.DeclaringType;
			}
			else
			{
				if ( asMethodInfo == null )
				{
					throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, Resources.Reflection_CustomMethodBaseIsNotSupported, source.GetType() ) );
				}

				returnType = typeof( void ).TypeHandle.Equals( asMethodInfo.ReturnType.TypeHandle ) ? null : asMethodInfo.ReturnType;
			}

			var delegateTypeDefinition =
				returnType == null
				? _actions[ shimParameterCount ]
				: _funcs[ shimParameterCount ];

			Contract.Assert( delegateTypeDefinition != null, source.ToString() );

			var shimParameterTypes =
				( source.IsStatic || source.IsConstructor )
				? parameters.Select( item => item.ParameterType ).ToArray()
				: Enumerable.Repeat( source.DeclaringType, 1 ).Concat( parameters.Select( item => item.ParameterType ) ).ToArray();

			var delegateType =
				returnType == null
				? ( delegateTypeDefinition.ContainsGenericParameters ? delegateTypeDefinition.MakeGenericType( shimParameterTypes ) : delegateTypeDefinition )
				: delegateTypeDefinition.MakeGenericType( shimParameterTypes.Concat( new[] { returnType } ).ToArray() );

			return ShimCodeGenerator.CreateShimMethod.MakeGenericMethod( delegateType ).Invoke( null, new object[] { source, shimParameterTypes, returnType, hostType, false } ) as Delegate;
		}
	}
}
