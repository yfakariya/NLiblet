using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics.Contracts;

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
