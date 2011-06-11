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

// This code is generated from T4Template ServiceLocator.TypedFuncs.tt.
// Do not modify this source code directly.

using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace NLiblet.ServiceLocators
{
	partial class ServiceLocator
	{
		/// <summary>
		/// 	Register strongly typed factory method for specified service type.
		/// </summary>
		/// <param name="serviceType"><see cref="Type"/> of service to be registered.</param>
		/// <param name="factory">Delegate to strongly typed factory method to be registered.</param>
		/// <returns>
		/// 	If specified factory method for <paramref name="serviceType"/> is registered successfully then true. 
		/// 	Else, when a factory method for specified type is already registered then false.
		/// 	To unregister a factory method for specific type, invoke <see cref="RemoveFactory"/>.
		/// </returns>
		/// <remarks>
		/// 	Registered factory method will be invoked every <see cref="Get&lt;T&gt;"/> calls.
		/// 	When specified arguments are not compatible for parameters of <paramref name="factory"/>,
		/// 	then <see cref="ArgumentException"/> will be thrown from <see cref="Get&lt;T&gt;"/> method.
		/// </remarks>
		[GeneratedCode( "Microsoft.VisualStudio.TextTemplating.10.0", "10.0.40219.1" )]
		public bool RegisterFactory< TResult >( Type serviceType, Func< TResult > factory )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentException>( serviceType.IsAssignableFrom( typeof( TResult ) ) );
			Contract.Requires<ArgumentNullException>( factory != null );
			
			return
				this.RegisterFactory( 
					serviceType,
					arguments =>
						factory(
						)
				);
		}
		
		/// <summary>
		/// 	Register strongly typed factory method for specified service type.
		/// </summary>
		/// <param name="serviceType"><see cref="Type"/> of service to be registered.</param>
		/// <param name="factory">Delegate to strongly typed factory method to be registered.</param>
		/// <returns>
		/// 	If specified factory method for <paramref name="serviceType"/> is registered successfully then true. 
		/// 	Else, when a factory method for specified type is already registered then false.
		/// 	To unregister a factory method for specific type, invoke <see cref="RemoveFactory"/>.
		/// </returns>
		/// <remarks>
		/// 	Registered factory method will be invoked every <see cref="Get&lt;T&gt;"/> calls.
		/// 	When specified arguments are not compatible for parameters of <paramref name="factory"/>,
		/// 	then <see cref="ArgumentException"/> will be thrown from <see cref="Get&lt;T&gt;"/> method.
		/// </remarks>
		[GeneratedCode( "Microsoft.VisualStudio.TextTemplating.10.0", "10.0.40219.1" )]
		public bool RegisterFactory< T, TResult >( Type serviceType, Func< T, TResult > factory )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentException>( serviceType.IsAssignableFrom( typeof( TResult ) ) );
			Contract.Requires<ArgumentNullException>( factory != null );
			
			return
				this.RegisterFactory( 
					serviceType,
					arguments =>
						factory(
							Cast< T >( arguments, 0 ) 
						)
				);
		}
		
		/// <summary>
		/// 	Register strongly typed factory method for specified service type.
		/// </summary>
		/// <param name="serviceType"><see cref="Type"/> of service to be registered.</param>
		/// <param name="factory">Delegate to strongly typed factory method to be registered.</param>
		/// <returns>
		/// 	If specified factory method for <paramref name="serviceType"/> is registered successfully then true. 
		/// 	Else, when a factory method for specified type is already registered then false.
		/// 	To unregister a factory method for specific type, invoke <see cref="RemoveFactory"/>.
		/// </returns>
		/// <remarks>
		/// 	Registered factory method will be invoked every <see cref="Get&lt;T&gt;"/> calls.
		/// 	When specified arguments are not compatible for parameters of <paramref name="factory"/>,
		/// 	then <see cref="ArgumentException"/> will be thrown from <see cref="Get&lt;T&gt;"/> method.
		/// </remarks>
		[GeneratedCode( "Microsoft.VisualStudio.TextTemplating.10.0", "10.0.40219.1" )]
		public bool RegisterFactory< T1, T2, TResult >( Type serviceType, Func< T1, T2, TResult > factory )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentException>( serviceType.IsAssignableFrom( typeof( TResult ) ) );
			Contract.Requires<ArgumentNullException>( factory != null );
			
			return
				this.RegisterFactory( 
					serviceType,
					arguments =>
						factory(
							Cast< T1 >( arguments, 0 ) ,
							Cast< T2 >( arguments, 1 ) 
						)
				);
		}
		
		/// <summary>
		/// 	Register strongly typed factory method for specified service type.
		/// </summary>
		/// <param name="serviceType"><see cref="Type"/> of service to be registered.</param>
		/// <param name="factory">Delegate to strongly typed factory method to be registered.</param>
		/// <returns>
		/// 	If specified factory method for <paramref name="serviceType"/> is registered successfully then true. 
		/// 	Else, when a factory method for specified type is already registered then false.
		/// 	To unregister a factory method for specific type, invoke <see cref="RemoveFactory"/>.
		/// </returns>
		/// <remarks>
		/// 	Registered factory method will be invoked every <see cref="Get&lt;T&gt;"/> calls.
		/// 	When specified arguments are not compatible for parameters of <paramref name="factory"/>,
		/// 	then <see cref="ArgumentException"/> will be thrown from <see cref="Get&lt;T&gt;"/> method.
		/// </remarks>
		[GeneratedCode( "Microsoft.VisualStudio.TextTemplating.10.0", "10.0.40219.1" )]
		public bool RegisterFactory< T1, T2, T3, TResult >( Type serviceType, Func< T1, T2, T3, TResult > factory )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentException>( serviceType.IsAssignableFrom( typeof( TResult ) ) );
			Contract.Requires<ArgumentNullException>( factory != null );
			
			return
				this.RegisterFactory( 
					serviceType,
					arguments =>
						factory(
							Cast< T1 >( arguments, 0 ) ,
							Cast< T2 >( arguments, 1 ) ,
							Cast< T3 >( arguments, 2 ) 
						)
				);
		}
		
		/// <summary>
		/// 	Register strongly typed factory method for specified service type.
		/// </summary>
		/// <param name="serviceType"><see cref="Type"/> of service to be registered.</param>
		/// <param name="factory">Delegate to strongly typed factory method to be registered.</param>
		/// <returns>
		/// 	If specified factory method for <paramref name="serviceType"/> is registered successfully then true. 
		/// 	Else, when a factory method for specified type is already registered then false.
		/// 	To unregister a factory method for specific type, invoke <see cref="RemoveFactory"/>.
		/// </returns>
		/// <remarks>
		/// 	Registered factory method will be invoked every <see cref="Get&lt;T&gt;"/> calls.
		/// 	When specified arguments are not compatible for parameters of <paramref name="factory"/>,
		/// 	then <see cref="ArgumentException"/> will be thrown from <see cref="Get&lt;T&gt;"/> method.
		/// </remarks>
		[GeneratedCode( "Microsoft.VisualStudio.TextTemplating.10.0", "10.0.40219.1" )]
		public bool RegisterFactory< T1, T2, T3, T4, TResult >( Type serviceType, Func< T1, T2, T3, T4, TResult > factory )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentException>( serviceType.IsAssignableFrom( typeof( TResult ) ) );
			Contract.Requires<ArgumentNullException>( factory != null );
			
			return
				this.RegisterFactory( 
					serviceType,
					arguments =>
						factory(
							Cast< T1 >( arguments, 0 ) ,
							Cast< T2 >( arguments, 1 ) ,
							Cast< T3 >( arguments, 2 ) ,
							Cast< T4 >( arguments, 3 ) 
						)
				);
		}
		
		/// <summary>
		/// 	Register strongly typed factory method for specified service type.
		/// </summary>
		/// <param name="serviceType"><see cref="Type"/> of service to be registered.</param>
		/// <param name="factory">Delegate to strongly typed factory method to be registered.</param>
		/// <returns>
		/// 	If specified factory method for <paramref name="serviceType"/> is registered successfully then true. 
		/// 	Else, when a factory method for specified type is already registered then false.
		/// 	To unregister a factory method for specific type, invoke <see cref="RemoveFactory"/>.
		/// </returns>
		/// <remarks>
		/// 	Registered factory method will be invoked every <see cref="Get&lt;T&gt;"/> calls.
		/// 	When specified arguments are not compatible for parameters of <paramref name="factory"/>,
		/// 	then <see cref="ArgumentException"/> will be thrown from <see cref="Get&lt;T&gt;"/> method.
		/// </remarks>
		[GeneratedCode( "Microsoft.VisualStudio.TextTemplating.10.0", "10.0.40219.1" )]
		public bool RegisterFactory< T1, T2, T3, T4, T5, TResult >( Type serviceType, Func< T1, T2, T3, T4, T5, TResult > factory )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentException>( serviceType.IsAssignableFrom( typeof( TResult ) ) );
			Contract.Requires<ArgumentNullException>( factory != null );
			
			return
				this.RegisterFactory( 
					serviceType,
					arguments =>
						factory(
							Cast< T1 >( arguments, 0 ) ,
							Cast< T2 >( arguments, 1 ) ,
							Cast< T3 >( arguments, 2 ) ,
							Cast< T4 >( arguments, 3 ) ,
							Cast< T5 >( arguments, 4 ) 
						)
				);
		}
		
		/// <summary>
		/// 	Register strongly typed factory method for specified service type.
		/// </summary>
		/// <param name="serviceType"><see cref="Type"/> of service to be registered.</param>
		/// <param name="factory">Delegate to strongly typed factory method to be registered.</param>
		/// <returns>
		/// 	If specified factory method for <paramref name="serviceType"/> is registered successfully then true. 
		/// 	Else, when a factory method for specified type is already registered then false.
		/// 	To unregister a factory method for specific type, invoke <see cref="RemoveFactory"/>.
		/// </returns>
		/// <remarks>
		/// 	Registered factory method will be invoked every <see cref="Get&lt;T&gt;"/> calls.
		/// 	When specified arguments are not compatible for parameters of <paramref name="factory"/>,
		/// 	then <see cref="ArgumentException"/> will be thrown from <see cref="Get&lt;T&gt;"/> method.
		/// </remarks>
		[GeneratedCode( "Microsoft.VisualStudio.TextTemplating.10.0", "10.0.40219.1" )]
		public bool RegisterFactory< T1, T2, T3, T4, T5, T6, TResult >( Type serviceType, Func< T1, T2, T3, T4, T5, T6, TResult > factory )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentException>( serviceType.IsAssignableFrom( typeof( TResult ) ) );
			Contract.Requires<ArgumentNullException>( factory != null );
			
			return
				this.RegisterFactory( 
					serviceType,
					arguments =>
						factory(
							Cast< T1 >( arguments, 0 ) ,
							Cast< T2 >( arguments, 1 ) ,
							Cast< T3 >( arguments, 2 ) ,
							Cast< T4 >( arguments, 3 ) ,
							Cast< T5 >( arguments, 4 ) ,
							Cast< T6 >( arguments, 5 ) 
						)
				);
		}
		
		/// <summary>
		/// 	Register strongly typed factory method for specified service type.
		/// </summary>
		/// <param name="serviceType"><see cref="Type"/> of service to be registered.</param>
		/// <param name="factory">Delegate to strongly typed factory method to be registered.</param>
		/// <returns>
		/// 	If specified factory method for <paramref name="serviceType"/> is registered successfully then true. 
		/// 	Else, when a factory method for specified type is already registered then false.
		/// 	To unregister a factory method for specific type, invoke <see cref="RemoveFactory"/>.
		/// </returns>
		/// <remarks>
		/// 	Registered factory method will be invoked every <see cref="Get&lt;T&gt;"/> calls.
		/// 	When specified arguments are not compatible for parameters of <paramref name="factory"/>,
		/// 	then <see cref="ArgumentException"/> will be thrown from <see cref="Get&lt;T&gt;"/> method.
		/// </remarks>
		[GeneratedCode( "Microsoft.VisualStudio.TextTemplating.10.0", "10.0.40219.1" )]
		public bool RegisterFactory< T1, T2, T3, T4, T5, T6, T7, TResult >( Type serviceType, Func< T1, T2, T3, T4, T5, T6, T7, TResult > factory )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentException>( serviceType.IsAssignableFrom( typeof( TResult ) ) );
			Contract.Requires<ArgumentNullException>( factory != null );
			
			return
				this.RegisterFactory( 
					serviceType,
					arguments =>
						factory(
							Cast< T1 >( arguments, 0 ) ,
							Cast< T2 >( arguments, 1 ) ,
							Cast< T3 >( arguments, 2 ) ,
							Cast< T4 >( arguments, 3 ) ,
							Cast< T5 >( arguments, 4 ) ,
							Cast< T6 >( arguments, 5 ) ,
							Cast< T7 >( arguments, 6 ) 
						)
				);
		}
		
		/// <summary>
		/// 	Register strongly typed factory method for specified service type.
		/// </summary>
		/// <param name="serviceType"><see cref="Type"/> of service to be registered.</param>
		/// <param name="factory">Delegate to strongly typed factory method to be registered.</param>
		/// <returns>
		/// 	If specified factory method for <paramref name="serviceType"/> is registered successfully then true. 
		/// 	Else, when a factory method for specified type is already registered then false.
		/// 	To unregister a factory method for specific type, invoke <see cref="RemoveFactory"/>.
		/// </returns>
		/// <remarks>
		/// 	Registered factory method will be invoked every <see cref="Get&lt;T&gt;"/> calls.
		/// 	When specified arguments are not compatible for parameters of <paramref name="factory"/>,
		/// 	then <see cref="ArgumentException"/> will be thrown from <see cref="Get&lt;T&gt;"/> method.
		/// </remarks>
		[GeneratedCode( "Microsoft.VisualStudio.TextTemplating.10.0", "10.0.40219.1" )]
		public bool RegisterFactory< T1, T2, T3, T4, T5, T6, T7, T8, TResult >( Type serviceType, Func< T1, T2, T3, T4, T5, T6, T7, T8, TResult > factory )
		{
			Contract.Requires<ArgumentNullException>( serviceType != null );
			Contract.Requires<ArgumentException>( serviceType.IsAssignableFrom( typeof( TResult ) ) );
			Contract.Requires<ArgumentNullException>( factory != null );
			
			return
				this.RegisterFactory( 
					serviceType,
					arguments =>
						factory(
							Cast< T1 >( arguments, 0 ) ,
							Cast< T2 >( arguments, 1 ) ,
							Cast< T3 >( arguments, 2 ) ,
							Cast< T4 >( arguments, 3 ) ,
							Cast< T5 >( arguments, 4 ) ,
							Cast< T6 >( arguments, 5 ) ,
							Cast< T7 >( arguments, 6 ) ,
							Cast< T8 >( arguments, 7 ) 
						)
				);
		}
		
		private static T Cast<T>( object[] arguments, int index )
		{
			if( arguments.Length <= index )
			{
				throw new ArgumentException( 
					String.Format( 
						CultureInfo.CurrentCulture, 
						"Cannot take argument from arguments array at index {0}. The array's length must be at least {1} but actual is {2}.",
						index, 
						index + 1,
						arguments.Length 
					),
					"arguments" 
				);
			}
			
			var item = arguments[ index ];
			if( item == null )
			{
				if( typeof( T ).IsValueType )
				{
					throw new ArgumentException( 
						String.Format( 
							CultureInfo.CurrentCulture, 
							"Type of argument at {0} is value type ({1}), so it cannot be null.",
							index, 
							typeof( T ).FullName
						),
						"arguments[" + index + "]"
					);
				}
			}
			
			if( item is T )
			{
				return ( T ) item;
			}
			
			var converter = TypeDescriptor.GetConverter( typeof( T ) );
			object converted = null;
			try
			{
				converted = converter.ConvertFrom( item );
			}
			catch( Exception ex )
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Cannot convert arguments[{0}](type '{1}') to target type '{2}'. {3}",
						index,
						arguments[ index ] == null ? "(null)" : arguments[ index ].GetType().FullName,
						typeof( T ),
						ex.Message
					),
					"arguments[" + index + "]",
					ex
				);
			}

			try
			{
				return ( T )converted;
			}
			catch( InvalidCastException ex )
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Cannot convert arguments[{0}](type '{1}') to target type '{2}' since TypeConverter '{3}'(type '{4}') returns invalid object '{5}'(type '{6}').",
						index,
						arguments[ index ] == null ? "(null)" : arguments[ index ].GetType().FullName,
						typeof( T ),
						converter,
						converter.GetType().FullName,
						converted,
						converted == null ? "(null)" : converted.GetType().FullName
					),
					"arguments[" + index + "]",
					ex
				);
			}
		}
	}
}
