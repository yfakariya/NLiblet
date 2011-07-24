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
using System.Reflection;
using NLiblet.Properties;

namespace NLiblet.Reflection
{
	/// <summary>
	///		Provides weak typed method invocation features without <see cref="TargetInvocationException"/>.
	/// </summary>
	public static class Invoker
	{
		#region -- CreateActionInvoker --

		/// <summary>
		///		Create <see cref="Action{T1,T2}"/> as invoker without host type nor skipping visibility.
		/// </summary>
		/// <param name="target">Target <see cref="MethodInfo"/> which does not have return value.</param>
		/// <returns>
		///		<see cref="Action{T1,T2}"/> as invoker without host type.
		///		Its 1st argument is instance as instance method target (for static method, this should be null).
		///		2nd argument is arguments for the target.
		/// </returns>
		public static Action<object, object[]> CreateActionInvoker( MethodInfo target )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<ArgumentException>( target.IsPublic );
			Contract.Requires<ArgumentException>( target.ReturnType == typeof( void ) );

			return CreateActionInvoker( target, false );
		}

		/// <summary>
		///		Create <see cref="Action{T1,T2}"/> as invoker without host type.
		/// </summary>
		/// <param name="target">Target <see cref="MethodInfo"/> which does not have return value.</param>
		/// <param name="restrictedSkipVisibility">To skip visibility check specify <c>true</c>; otherwise <c>false</c>.</param>
		/// <returns>
		///		<see cref="Action{T1,T2}"/> as invoker without host type.
		///		Its 1st argument is instance as instance method target (for static method, this should be null).
		///		2nd argument is arguments for the target.
		/// </returns>
		public static Action<object, object[]> CreateActionInvoker( MethodInfo target, bool restrictedSkipVisibility )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<ArgumentException>( target.IsPublic );
			Contract.Requires<ArgumentException>( target.ReturnType == typeof( void ) );

			return ShimCodeGenerator.CreateLooseProcedureInvocationShimMethod.Invoke( null, new object[] { target, null, restrictedSkipVisibility } ) as Action<object, object[]>;
		}

		/// <summary>
		///		Create <see cref="Action{T1,T2}"/> as invoker with specified host type nor skipping visibility.
		/// </summary>
		/// <param name="target">Target <see cref="MethodInfo"/> which does not have return value.</param>
		/// <param name="hostType">Host type of generated shim code. <paramref name="target"/> can access non-public member of this.</param>
		/// <returns>
		///		<see cref="Action{T1,T2}"/> as invoker without host type.
		///		Its 1st argument is instance as instance method target (for static method, this should be null).
		///		2nd argument is arguments for the target.
		/// </returns>
		public static Action<object, object[]> CreateActionInvoker( MethodInfo target, Type hostType )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<ArgumentNullException>( hostType != null );
			Contract.Requires<ArgumentException>( target.ReturnType == typeof( void ) );

			return CreateActionInvoker( target, hostType, false );
		}

		/// <summary>
		///		Create <see cref="Action{T1,T2}"/> as invoker with specified host type.
		/// </summary>
		/// <param name="target">Target <see cref="MethodInfo"/> which does not have return value.</param>
		/// <param name="hostType">Host type of generated shim code. <paramref name="target"/> can access non-public member of this.</param>
		/// <param name="skipVisibility">To skip visibility check specify <c>true</c>; otherwise <c>false</c>.</param>
		/// <returns>
		///		<see cref="Action{T1,T2}"/> as invoker without host type.
		///		Its 1st argument is instance as instance method target (for static method, this should be null).
		///		2nd argument is arguments for the target.
		/// </returns>
		public static Action<object, object[]> CreateActionInvoker( MethodInfo target, Type hostType, bool skipVisibility )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<ArgumentNullException>( hostType != null );
			Contract.Requires<ArgumentException>( target.ReturnType.TypeHandle.Equals( typeof( void ).TypeHandle ), "target.ReturnType == typeof( void )" );

			return ShimCodeGenerator.CreateLooseProcedureInvocationShimMethod.Invoke( null, new object[] { target, hostType, skipVisibility } ) as Action<object, object[]>;
		}

		#endregion

		#region -- CreateFuncInvoker --

		/// <summary>
		///		Create <see cref="Func{T1,T2,TResult}"/> as invoker without host type nor skipping visibility.
		/// </summary>
		/// <param name="target">Target <see cref="MethodBase"/> which is method having return value or instance constructor.</param>
		/// <returns>
		///		<see cref="Func{T1,T2,TResult}"/> as invoker without host type.
		///		Its 1st argument is instance as instance method target (for static method or constructor, this should be null).
		///		2nd argument is arguments for the target.
		/// </returns>
		public static Func<object, object[], object> CreateFuncInvoker( MethodBase target )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<NotSupportedException>( !( target.IsSpecialName && target.Name == ".cctor" ), "!target.IsTypeInitializer" );
			Contract.Requires<ArgumentException>( target.IsPublic );
			Contract.Requires<ArgumentException>( target as MethodInfo == null || !( target as MethodInfo ).ReturnType.TypeHandle.Equals( typeof( void ).TypeHandle ), "target.ReturnType != typeof( void )" );

			return CreateFuncInvoker( target, false );
		}

		/// <summary>
		///		Create <see cref="Func{T1,T2,TResult}"/> as invoker without host type.
		/// </summary>
		/// <param name="target">Target <see cref="MethodBase"/> which is method having return value or instance constructor.</param>
		/// <param name="restrictedSkipVisibility">To skip visibility check specify <c>true</c>; otherwise <c>false</c>.</param>
		/// <returns>
		///		<see cref="Func{T1,T2,TResult}"/> as invoker without host type.
		///		Its 1st argument is instance as instance method target (for static method or constructor, this should be null).
		///		2nd argument is arguments for the target.
		/// </returns>
		public static Func<object, object[], object> CreateFuncInvoker( MethodBase target, bool restrictedSkipVisibility )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<NotSupportedException>( !( target.IsSpecialName && target.Name == ".cctor" ), "!target.IsTypeInitializer" );
			Contract.Requires<ArgumentException>( target.IsPublic );
			Contract.Requires<ArgumentException>( target as MethodInfo == null || !( target as MethodInfo ).ReturnType.TypeHandle.Equals( typeof( void ).TypeHandle ), "target.ReturnType != typeof( void )" );

			return ShimCodeGenerator.CreateLooseFunctionInvocationShimMethod.Invoke( null, new object[] { target, null, restrictedSkipVisibility } ) as Func<object, object[], object>;
		}

		/// <summary>
		///		Create <see cref="Func{T1,T2,TResult}"/> as invoker with specified host type nor skipping visibility.
		/// </summary>
		/// <param name="target">Target <see cref="MethodBase"/> which is method having return value or instance constructor.</param>
		/// <param name="hostType">Host type of generated shim code. <paramref name="target"/> can access non-public member of this.</param>
		/// <returns>
		///		<see cref="Func{T1,T2,TResult}"/> as invoker without host type.
		///		Its 1st argument is instance as instance method target (for static method or constructor, this should be null).
		///		2nd argument is arguments for the target.
		/// </returns>
		public static Func<object, object[], object> CreateFuncInvoker( MethodBase target, Type hostType )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<ArgumentNullException>( hostType != null );
			Contract.Requires<NotSupportedException>( !( target.IsSpecialName && target.Name == ".cctor" ) );
			Contract.Requires<ArgumentException>( target as MethodInfo == null || !( target as MethodInfo ).ReturnType.TypeHandle.Equals( typeof( void ).TypeHandle ), "target.ReturnType != typeof( void )" );

			return CreateFuncInvoker( target, hostType, false );
		}

		/// <summary>
		///		Create <see cref="Func{T1,T2,TResult}"/> as invoker with specified host type nor skipping visibility.
		/// </summary>
		/// <param name="target">Target <see cref="MethodBase"/> which is method having return value or instance constructor.</param>
		/// <param name="hostType">Host type of generated shim code. <paramref name="target"/> can access non-public member of this.</param>
		/// <param name="skipVisibility">To skip visibility check specify <c>true</c>; otherwise <c>false</c>.</param>
		/// <returns>
		///		<see cref="Func{T1,T2,TResult}"/> as invoker without host type.
		///		Its 1st argument is instance as instance method target (for static method or constructor, this should be null).
		///		2nd argument is arguments for the target.
		/// </returns>
		public static Func<object, object[], object> CreateFuncInvoker( MethodBase target, Type hostType, bool skipVisibility )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<ArgumentNullException>( hostType != null );
			Contract.Requires<NotSupportedException>( !( target.IsSpecialName && target.Name == ".cctor" ) );
			Contract.Requires<ArgumentException>( target as MethodInfo == null || !( target as MethodInfo ).ReturnType.TypeHandle.Equals( typeof( void ).TypeHandle ), "target.ReturnType != typeof( void )" );

			return ShimCodeGenerator.CreateLooseFunctionInvocationShimMethod.Invoke( null, new object[] { target, hostType, skipVisibility } ) as Func<object, object[], object>;
		}

		#endregion

		#region -- Invoke --

		/// <summary>
		///		Invoke target instance method without host type nor skipping visiblity.
		/// </summary>
		/// <param name="target">Target <see cref="MethodInfo"/>.</param>
		/// <param name="instance">Instance as instance method target (for static method or constructor, this should be null).</param>
		/// <param name="arguments">Arguments for the <paramref name="target"/>.</param>
		/// <returns>
		///		Return value of invoked target.
		///		If return type of <paramref name="target"/> is void, then null.
		/// </returns>
		public static object Invoke( MethodInfo target, object instance, params object[] arguments )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<ArgumentException>( !target.IsStatic );
			Contract.Requires<ArgumentNullException>( instance != null );

			return Invoke( target, false, instance, arguments );
		}

		/// <summary>
		///		Invoke target instance method without host type.
		/// </summary>
		/// <param name="target">Target <see cref="MethodInfo"/>.</param>
		/// <param name="restrictedSkipVisibility">To skip visibility check specify <c>true</c>; otherwise <c>false</c>.</param>
		/// <param name="instance">Instance as instance method target (for static method or constructor, this should be null).</param>
		/// <param name="arguments">Arguments for the <paramref name="target"/>.</param>
		/// <returns>
		///		Return value of invoked target.
		///		If return type of <paramref name="target"/> is void, then null.
		/// </returns>
		public static object Invoke( MethodInfo target, bool restrictedSkipVisibility, object instance, params object[] arguments )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<ArgumentException>( !target.IsStatic );
			Contract.Requires<ArgumentNullException>( instance != null );

			return InvokeCore( target, instance, null, restrictedSkipVisibility, arguments ?? Empty.Array<object>() );
		}

		/// <summary>
		///		Invoke target instance method with specified host type nor skipping visiblity.
		/// </summary>
		/// <param name="target">Target <see cref="MethodInfo"/>.</param>
		/// <param name="hostType">Host type of generated shim code. <paramref name="target"/> can access non-public member of this. Normally, type of <paramref name="instance"/>.</param>
		/// <param name="instance">Instance as instance method target (for static method or constructor, this should be null).</param>
		/// <param name="arguments">Arguments for the <paramref name="target"/>.</param>
		/// <returns>
		///		Return value of invoked target.
		///		If return type of <paramref name="target"/> is void, then null.
		/// </returns>
		public static object Invoke( MethodInfo target, Type hostType, object instance, params object[] arguments )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<ArgumentException>( !target.IsStatic );
			Contract.Requires<ArgumentNullException>( hostType != null );
			Contract.Requires<ArgumentNullException>( instance != null );

			return Invoke( target, hostType, false, instance, arguments );
		}

		/// <summary>
		///		Invoke target instance method with specified host type.
		/// </summary>
		/// <param name="target">Target <see cref="MethodInfo"/>.</param>
		/// <param name="hostType">Host type of generated shim code. <paramref name="target"/> can access non-public member of this. Normally, type of <paramref name="instance"/>.</param>
		/// <param name="skipVisibility">To skip visibility check specify <c>true</c>; otherwise <c>false</c>.</param>
		/// <param name="instance">Instance as instance method target (for static method or constructor, this should be null).</param>
		/// <param name="arguments">Arguments for the <paramref name="target"/>.</param>
		/// <returns>
		///		Return value of invoked target.
		///		If return type of <paramref name="target"/> is void, then null.
		/// </returns>
		public static object Invoke( MethodInfo target, Type hostType, bool skipVisibility, object instance, params object[] arguments )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<ArgumentException>( !target.IsStatic );
			Contract.Requires<ArgumentNullException>( hostType != null );
			Contract.Requires<ArgumentNullException>( instance != null );

			return InvokeCore( target, instance, hostType, skipVisibility, arguments ?? Empty.Array<object>() );
		}

		#endregion

		#region -- InvokeStatic --

		/// <summary>
		///		Invoke target static method without host type nor skipping visiblity.
		/// </summary>
		/// <param name="target">Target <see cref="MethodInfo"/>.</param>
		/// <param name="arguments">Arguments for the <paramref name="target"/>.</param>
		/// <returns>
		///		Return value of invoked target.
		///		If return type of <paramref name="target"/> is void, then null.
		/// </returns>
		public static object InvokeStatic( MethodInfo target, params object[] arguments )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<ArgumentException>( target.IsStatic );
			Contract.Requires<ArgumentException>( !( target.IsSpecialName && target.Name == ".cctor" ) );

			return InvokeStatic( target, false, arguments );
		}

		/// <summary>
		///		Invoke target static method without host type.
		/// </summary>
		/// <param name="target">Target <see cref="MethodInfo"/>.</param>
		/// <param name="restrictedSkipVisibility">To skip visibility check specify <c>true</c>; otherwise <c>false</c>.</param>
		/// <param name="arguments">Arguments for the <paramref name="target"/>.</param>
		/// <returns>
		///		Return value of invoked target.
		///		If return type of <paramref name="target"/> is void, then null.
		/// </returns>
		public static object InvokeStatic( MethodInfo target, bool restrictedSkipVisibility, params object[] arguments )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<ArgumentException>( target.IsStatic );
			Contract.Requires<ArgumentException>( !( target.IsSpecialName && target.Name == ".cctor" ) );

			return InvokeCore( target, null, typeof( GeneratedCodeHelper ), restrictedSkipVisibility, arguments ?? Empty.Array<object>() );
		}

		/// <summary>
		///		Invoke target static method with specified host type nor skipping visiblity.
		/// </summary>
		/// <param name="target">Target <see cref="MethodInfo"/>.</param>
		/// <param name="hostType">Host type of generated shim code. <paramref name="target"/> can access non-public member of this. Normally, declaring type of <paramref name="target"/>.</param>
		/// <param name="arguments">Arguments for the <paramref name="target"/>.</param>
		/// <returns>
		///		Return value of invoked target.
		///		If return type of <paramref name="target"/> is void, then null.
		/// </returns>
		public static object InvokeStatic( MethodInfo target, Type hostType, params object[] arguments )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<ArgumentException>( target.IsStatic );
			Contract.Requires<ArgumentNullException>( hostType != null );
			Contract.Requires<NotSupportedException>( !( target.IsSpecialName && target.Name == ".cctor" ) );

			return InvokeStatic( target, hostType, false, arguments );
		}

		/// <summary>
		///		Invoke target static method with specified host type.
		/// </summary>
		/// <param name="target">Target <see cref="MethodInfo"/>.</param>
		/// <param name="hostType">Host type of generated shim code. <paramref name="target"/> can access non-public member of this. Normally, declaring type of <paramref name="target"/>.</param>
		/// <param name="skipVisibility">To skip visibility check specify <c>true</c>; otherwise <c>false</c>.</param>
		/// <param name="arguments">Arguments for the <paramref name="target"/>.</param>
		/// <returns>
		///		Return value of invoked target.
		///		If return type of <paramref name="target"/> is void, then null.
		/// </returns>
		public static object InvokeStatic( MethodInfo target, Type hostType, bool skipVisibility, params object[] arguments )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<ArgumentException>( target.IsStatic );
			Contract.Requires<ArgumentNullException>( hostType != null );
			Contract.Requires<NotSupportedException>( !( target.IsSpecialName && target.Name == ".cctor" ) );

			return InvokeCore( target, null, hostType, skipVisibility, arguments ?? Empty.Array<object>() );
		}

		#endregion

		#region -- InvokeConstructor --

		/// <summary>
		///		Invoke target constructor without host type.
		/// </summary>
		/// <param name="target">Target <see cref="ConstructorInfo"/>.</param>
		/// <param name="arguments">Arguments for the <paramref name="target"/>.</param>
		/// <returns>
		///		New instance created by <paramref name="target"/>.
		/// </returns>
		public static object InvokeConstructor( ConstructorInfo target, params object[] arguments )
		{
			Contract.Requires<ArgumentNullException>( target != null );

			return InvokeConstructor( target, false, arguments ?? Empty.Array<object>() );
		}

		/// <summary>
		///		Invoke target constructor without host type.
		/// </summary>
		/// <param name="target">Target <see cref="ConstructorInfo"/>.</param>
		/// <param name="restrictedSkipVisibility">To skip visibility check specify <c>true</c>; otherwise <c>false</c>.</param>
		/// <param name="arguments">Arguments for the <paramref name="target"/>.</param>
		/// <returns>
		///		New instance created by <paramref name="target"/>.
		/// </returns>
		public static object InvokeConstructor( ConstructorInfo target, bool restrictedSkipVisibility, params object[] arguments )
		{
			Contract.Requires<ArgumentNullException>( target != null );

			return InvokeCore( target, null, null, restrictedSkipVisibility, arguments ?? Empty.Array<object>() );
		}

		/// <summary>
		///		Invoke target static method with specified host type.
		/// </summary>
		/// <param name="target">Target <see cref="ConstructorInfo"/>.</param>
		/// <param name="hostType">Host type of generated shim code. <paramref name="target"/> can access non-public member of this. Normally, declaring type of <paramref name="target"/>.</param>
		/// <param name="arguments">Arguments for the <paramref name="target"/>.</param>
		/// <returns>
		///		New instance created by <paramref name="target"/>.
		/// </returns>
		public static object InvokeConstructor( ConstructorInfo target, Type hostType, params object[] arguments )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<NotSupportedException>( !target.IsStatic );
			Contract.Requires<ArgumentNullException>( hostType != null );

			return InvokeConstructor( target, hostType, false, arguments );
		}

		/// <summary>
		///		Invoke target static method with specified host type.
		/// </summary>
		/// <param name="target">Target <see cref="ConstructorInfo"/>.</param>
		/// <param name="hostType">Host type of generated shim code. <paramref name="target"/> can access non-public member of this. Normally, declaring type of <paramref name="target"/>.</param>
		/// <param name="skipVisibility">To skip visibility check specify <c>true</c>; otherwise <c>false</c>.</param>
		/// <param name="arguments">Arguments for the <paramref name="target"/>.</param>
		/// <returns>
		///		New instance created by <paramref name="target"/>.
		/// </returns>
		public static object InvokeConstructor( ConstructorInfo target, Type hostType, bool skipVisibility, params object[] arguments )
		{
			Contract.Requires<ArgumentNullException>( target != null );
			Contract.Requires<NotSupportedException>( !target.IsStatic );
			Contract.Requires<ArgumentNullException>( hostType != null );

			return InvokeCore( target, null, hostType, skipVisibility, arguments ?? Empty.Array<object>() );
		}

		#endregion


		private static object InvokeCore( MethodBase target, object instance, Type hostType, bool skipVisibility, object[] arguments )
		{
			var asMethodInfo = target as MethodInfo;
			var asConstructorInfo = target as ConstructorInfo;
			if ( asConstructorInfo != null )
			{
				if ( hostType == null )
				{
					return CreateFuncInvoker( target, skipVisibility )( null, arguments );
				}
				else
				{
					return CreateFuncInvoker( target, hostType, skipVisibility )( null, arguments );
				}
			}
			else
			{
				if ( asMethodInfo == null )
				{
					throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, Resources.Reflection_CustomMethodBaseIsNotSupported, target.GetType() ) );
				}

				if ( typeof( void ).TypeHandle.Equals( asMethodInfo.ReturnType.TypeHandle ) )
				{
					if ( hostType == null )
					{
						CreateActionInvoker( asMethodInfo, skipVisibility )( instance, arguments );
					}
					else
					{
						CreateActionInvoker( asMethodInfo, hostType, skipVisibility )( instance, arguments );
					}
					return null;
				}
				else
				{
					if ( hostType == null )
					{
						return CreateFuncInvoker( target, skipVisibility )( instance, arguments );
					}
					else
					{
						return CreateFuncInvoker( target, hostType, skipVisibility )( instance, arguments );
					}
				}
			}
		}
	}
}
