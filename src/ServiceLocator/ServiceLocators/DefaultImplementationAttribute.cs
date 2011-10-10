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
using System.Reflection;

namespace NLiblet.ServiceLocators
{
	/// <summary>
	///		Defines default implementation class of the service.
	/// </summary>
	[AttributeUsage( AttributeTargets.Class | AttributeTargets.Interface, Inherited = true, AllowMultiple = false )]
	public sealed class DefaultImplementationAttribute : Attribute
	{
		private readonly Type _defaultImplementationType;
		private readonly ConstructorInfo[] _constructors;

		/// <summary>
		///		Gets the public constructors of implementation type.
		/// </summary>
		/// <value>
		///		The public constructors of implementation type.
		/// </value>
		internal ConstructorInfo[] InternalConstructors
		{
			get
			{
				Contract.Ensures( Contract.Result<ConstructorInfo[]>() != null );
				return this._constructors;
			}
		}


		/// <summary>
		///		Gets the default implementation type of the sevice.
		/// </summary>
		/// <value>
		///		The default implementation type of the sevice.
		/// </value>
		public Type DefaultImplementationType
		{
			get
			{
				Contract.Ensures( Contract.Result<Type>() != null );
				return this._defaultImplementationType;
			}
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="DefaultImplementationAttribute"/> class.
		/// </summary>
		/// <param name="defaultImplementationType">Default type of the implementation.</param>
		public DefaultImplementationAttribute( Type defaultImplementationType )
		{
			Contract.Requires<ArgumentNullException>( defaultImplementationType != null, "defaultImplementationType" );
			Contract.Requires<ArgumentException>( !defaultImplementationType.IsAbstract );
			Contract.Requires<ArgumentException>( !defaultImplementationType.IsInterface );

			this._defaultImplementationType = defaultImplementationType;
			this._constructors = defaultImplementationType.GetConstructors();
		}
	}
}
