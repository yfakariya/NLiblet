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

using System.Globalization;
using System.Threading;
using NUnit.Framework;

namespace NLiblet.Text
{
	[TestFixture]
	public class FormatProvidersTest
	{
#if DEBUG
		[Test]
		public void TestInvariantCulture()
		{
			Assert.IsNotNull( FormatProviders.InvariantCulture );
			var asCustom = FormatProviders.InvariantCulture as CommonCustomFormatter;
			Assert.IsNotNull( asCustom );
			Assert.IsTrue( CultureInfo.InvariantCulture.Equals( asCustom.DefaultFormatProvider ) );
			Assert.AreSame( FormatProviders.InvariantCulture,FormatProviders.InvariantCulture);
		}

		[Test]
		public void TestCurrentCulture()
		{
			var original = Thread.CurrentThread.CurrentCulture;
			try
			{
				foreach ( var culture in new[] { CultureInfo.GetCultureInfo( "en-US" ), CultureInfo.GetCultureInfo( "ja-JP" ), CultureInfo.GetCultureInfo( "fr-FR" ) } )
				{
					Thread.CurrentThread.CurrentCulture = culture;
					Assert.IsNotNull( FormatProviders.CurrentCulture );
					var asCustom = FormatProviders.CurrentCulture as CommonCustomFormatter;
					Assert.IsNotNull( asCustom );
					Assert.IsTrue( culture.Equals( asCustom.DefaultFormatProvider ) );
					Assert.AreSame( FormatProviders.CurrentCulture, FormatProviders.CurrentCulture );
				}
			}
			finally
			{
				Thread.CurrentThread.CurrentCulture = original;
			}
		}
#endif
	}
}
