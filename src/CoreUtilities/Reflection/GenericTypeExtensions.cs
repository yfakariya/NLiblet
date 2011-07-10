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

namespace NLiblet.Reflection
{
	/// <summary>
	///		Define utility extension method for generic type.
	/// </summary>
	public static class GenericTypeExtensions
	{
		/// <summary>
		///		Determine whether source type inherits directly or indirectly from specified generic type or its built type.
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <param name="genericTypeDefinition">Generic type definition.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="source"/>, directly or indirectly, inherits from <paramref name="genericTypeDefinition"/>,
		///		or built closed generic type;
		///		otherwise <c>false</c>.
		/// </returns>
		[Pure]
		public static bool Inherits( this Type source, Type genericTypeDefinition )
		{
			Contract.Requires<ArgumentNullException>( source != null );
			Contract.Requires<ArgumentNullException>( genericTypeDefinition != null );
			Contract.Requires<ArgumentException>( genericTypeDefinition.IsGenericTypeDefinition );

			for ( Type current = source; current != null; current = current.BaseType )
			{
				if ( current.IsGenericType )
				{
					var definition = current.IsGenericTypeDefinition ? current : current.GetGenericTypeDefinition();
					if ( definition.TypeHandle.Equals( genericTypeDefinition.TypeHandle ) )
					{
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		///		Determine whether source type implements specified generic type or its built type.
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <param name="genericTypeDefinition">Generic interface type definition.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="source"/> implements <paramref name="genericTypeDefinition"/>,
		///		or built closed generic interface type;
		///		otherwise <c>false</c>.
		/// </returns>
		[Pure]
		public static bool Implements( this Type source, Type genericTypeDefinition )
		{
			Contract.Requires<ArgumentNullException>( source != null );
			Contract.Requires<ArgumentNullException>( genericTypeDefinition != null );
			Contract.Requires<ArgumentException>( genericTypeDefinition.IsInterface );
			Contract.Requires<ArgumentException>( genericTypeDefinition.IsGenericTypeDefinition );

			return
				source.GetInterfaces()
				.Where( @interface => @interface.IsGenericType )
				.Select( @interface => @interface.GetGenericTypeDefinition() )
				.Any( definition => definition.TypeHandle.Equals( genericTypeDefinition.TypeHandle ) );
		}

		/// <summary>
		///		Get name of type without namespace and assembly name of itself and its generic arguments.
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <returns>Simple name of type.</returns>
		[Pure]
		public static string GetName( this Type source )
		{
			Contract.Requires<ArgumentNullException>( source != null );
			if ( !source.IsGenericType )
			{
				return source.Name;
			}

			return
				String.Join(
					String.Empty,
					source.Name,
					'[',
					String.Join( ", ", source.GetGenericArguments().Select( t => t.GetName() ) ),
					']'
				);
		}

		/// <summary>
		///		Get full name of type including namespace and excluding assembly name of itself and its generic arguments.
		/// </summary>
		/// <param name="source">Target type.</param>
		/// <returns>Full name of type.</returns>
		[Pure]
		public static string GetFullName( this Type source )
		{
			Contract.Requires<ArgumentNullException>( source != null );
			if ( !source.IsGenericType )
			{
				return source.FullName;
			}

			return 
				String.Join( 
					String.Empty,
					source.Namespace,
					Type.Delimiter,
					source.Name,
					'[',
					String.Join( ", ", source.GetGenericArguments().Select( t => t.GetFullName() ) ),
					']'
				);
		}

		/// <summary>
		///		Determines whether the specified same type other <see cref="Type"/> is equal to the instance.
		/// </summary>
		/// <param name="source">The <see cref="Type"/> to compare with <paramref name="other"/> instance.</param>
		/// <param name="other">The <see cref="Type"/> to compare with <paramref name="source"/> instance.</param>
		/// <returns>
		///		<c>true</c> if <paramref name="other"/> is equal to <paramref name="source"/> instance; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>
		///		This method compares <see cref="Type.TypeHandle"/> property.
		/// </remarks>
		[Pure]
		public static bool Equals( this Type source, Type other )
		{
			Contract.Requires<ArgumentNullException>( source != null );

			if ( Object.ReferenceEquals( other, null ) )
			{
				return false;
			}

			if ( Object.ReferenceEquals( source, other ) )
			{
				return true;
			}

			try
			{
				return source.TypeHandle.Equals( other.TypeHandle );
			}
			catch ( NotSupportedException )
			{
				return false;
			}
		}
	}
}
