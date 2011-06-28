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
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace NLiblet.Reflection
{
	[TestFixture]
	public class GenericTypeExtensionsTest
	{
		[Test]
		public void TestGetName()
		{
			Assert.AreEqual( "Object", typeof( object ).GetName() );
			Assert.AreEqual( "Object[]", typeof( object[] ).GetName() );
			Assert.AreEqual(
				"Dictionary`2[String, Object]",
				typeof( Dictionary<string, object> ).GetName()
			);
			Assert.AreEqual(
				"Func`2[IEnumerable`1[FileInfo], IDictionary`2[String, Int64]]",
				typeof( Func<IEnumerable<FileInfo>, IDictionary<string, long>> ).GetName()
			);
		}
		
		[Test]
		public void TestGetFullName()
		{
			Assert.AreEqual( "System.Object", typeof( object ).GetFullName() );
			Assert.AreEqual( "System.Object[]", typeof( object[] ).GetFullName() );
			Assert.AreEqual( 
				"System.Collections.Generic.Dictionary`2[System.String, System.Object]", 
				typeof( Dictionary<string, object> ).GetFullName() 
			);
			Assert.AreEqual(
				"System.Func`2[System.Collections.Generic.IEnumerable`1[System.IO.FileInfo], System.Collections.Generic.IDictionary`2[System.String, System.Int64]]",
				typeof( Func<IEnumerable<FileInfo>, IDictionary<string, long>> ).GetFullName()
			);
		}

		[Test]
		public void TestInherits()
		{
			Assert.IsTrue( typeof( SomeClassDirect ).Inherits( typeof( EqualityComparer<> ) ) );
			Assert.IsTrue( typeof( SomeClassIndirect ).Inherits( typeof( EqualityComparer<> ) ) );
			Assert.IsFalse( typeof( SomeClassDirect ).Inherits( typeof( IEqualityComparer<> ) ) );
			Assert.IsFalse( typeof( SomeClassDirect ).Inherits( typeof( IEqualityComparer<> ) ) );
			Assert.IsFalse( typeof( ISomeInterfaceDirect ).Inherits( typeof( IEqualityComparer<> ) ) );
			Assert.IsFalse( typeof( ISomeInterfaceIndirect ).Inherits( typeof( IEqualityComparer<> ) ) );
		}

		[Test]
		public void TestImplements()
		{
			Assert.IsTrue( typeof( SomeClassDirect ).Implements( typeof( IEqualityComparer<> ) ) );
			Assert.IsTrue( typeof( SomeClassDirect ).Implements( typeof( IEqualityComparer<> ) ) );
			Assert.IsTrue( typeof( ISomeInterfaceDirect ).Implements( typeof( IEqualityComparer<> ) ) );
			Assert.IsTrue( typeof( ISomeInterfaceIndirect ).Implements( typeof( IEqualityComparer<> ) ) );
		}

		private class SomeClassDirect : EqualityComparer<string>
		{
			public override bool Equals( string x, string y )
			{
				throw new NotImplementedException();
			}

			public override int GetHashCode( string obj )
			{
				throw new NotImplementedException();
			}
		}

		private abstract class SomeClassBase<T> : EqualityComparer<T> { }

		private class SomeClassIndirect : SomeClassBase<string>
		{
			public override bool Equals( string x, string y )
			{
				throw new NotImplementedException();
			}

			public override int GetHashCode( string obj )
			{
				throw new NotImplementedException();
			}
		}

		private interface ISomeInterfaceDirect : IEqualityComparer<string> { }
		private interface ISomeInterfaceBase<T> : IEqualityComparer<T> { }
		private interface ISomeInterfaceIndirect : ISomeInterfaceBase<string> { }

	}
}
