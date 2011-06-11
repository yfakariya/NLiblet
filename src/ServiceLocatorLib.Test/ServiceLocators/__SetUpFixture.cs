#region -- License Terms --
//
// MessagePack for CLI
//
// Copyright (C) 2010 FUJIWARA, Yusuke
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
using NUnit.Framework;

namespace NLiblet.ServiceLocators
{
	[SetUpFixture]
	public sealed class __SetUpFixture
	{
		[SetUp]
		public void SetupCurrentNamespaceTests()
		{
			Contract.ContractFailed += ( sender, e ) =>
			{
				e.SetHandled();
				e.SetUnwind();
				Assert.Fail( "Contract failed.{0}{3}{0}Type:{1}{0}Condition:{2}{0}Exception:{4}", Environment.NewLine, e.FailureKind, e.Condition, e.Message, e.OriginalException );
			};
		}
	}
}
