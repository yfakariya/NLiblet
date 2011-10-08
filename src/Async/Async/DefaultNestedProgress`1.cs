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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NLiblet.Async
{
	/// <summary>
	///		Automatic generated delegate based <see cref="IProgress{T}"/> implementation.
	/// </summary>
	/// <typeparam name="T">Type of value of progress reporting.</typeparam>
	internal sealed class DefaultNestedProgress<T> : IProgress<T>
	{
		private static readonly Func<T, T, T> _adjuster = InitializeAdjuster();

		private static Func<T, T, T> InitializeAdjuster()
		{
			var value = Expression.Parameter( typeof( T ), "value" );
			var factor = Expression.Parameter( typeof( T ), "factor" );
			try
			{
				return Expression.Lambda<Func<T, T, T>>( Expression.Multiply( value, factor ), value, factor ).Compile();
			}
			catch ( InvalidOperationException ) { }

			bool isNullable =
				typeof( T ).IsGenericType && typeof( Nullable<> ).TypeHandle.Equals( typeof( T ).GetGenericTypeDefinition().TypeHandle );
			Type targetType = typeof( T );
			if ( isNullable )
			{
				targetType = typeof( T ).GetGenericArguments()[ 0 ];
				Contract.Assert( targetType != null );
			}

			var multiplyMethod =
				targetType.FindMembers(
					MemberTypes.Method,
					BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance,
					( member, criteria ) =>
					{
						var criteriaType = criteria as Type;
						var method = member as MethodInfo;
						if ( method == null || method.Name != "Multiply" || method.ReturnType == typeof( void ) )
						{
							return false;
						}

						if ( !criteriaType.IsAssignableFrom( method.ReturnType ) )
						{
							return false;
						}

						var paramaters = method.GetParameters();
						switch ( paramaters.Length )
						{
							case 1:
							{
								if ( method.IsStatic )
								{
									return false;
								}

								break;
							}
							case 2:
							{
								if ( !method.IsStatic )
								{
									return false;
								}

								break;
							}
							default:
							{
								return false;
							}
						}

						return paramaters.All( item => item.ParameterType.IsAssignableFrom( criteriaType ) );
					},
					targetType
				).OfType<MethodInfo>()
				.OrderByDescending( item => item.GetParameters().Length )
				.FirstOrDefault();

			if ( multiplyMethod == null )
			{
				return null;
			}

			try
			{
				if ( multiplyMethod.IsStatic )
				{
					return
						Expression.Lambda<Func<T, T, T>>(
							Expression.Multiply( value, factor, multiplyMethod ),
							value,
							factor
						).Compile();
				}
				else
				{
					var multiply =
						Expression.Call(
							isNullable ? ( Expression )Expression.Coalesce( value, Expression.Default( targetType ) ) : value,
							multiplyMethod,
							isNullable ? ( Expression )Expression.Coalesce( factor, Expression.Default( targetType ) ) : factor
						);
					return
						Expression.Lambda<Func<T, T, T>>(
							isNullable ? ( Expression )Expression.Convert( multiply, typeof( T ) ) : multiply,
							value,
							factor
						).Compile();
				}
			}
			catch ( InvalidOperationException )
			{
				return null;
			}
		}

		public static bool CanMultily
		{
			get { return _adjuster != null; }
		}

		private readonly T _factor;
		private readonly IProgress<T> _parent;

		public DefaultNestedProgress( IProgress<T> parent, T factor )
		{
			Contract.Requires( parent != null );

			this._parent = parent;
			this._factor = factor;
		}

		public void Report( T value )
		{
			this._parent.Report( _adjuster( value, this._factor ) );
		}
	}
}
