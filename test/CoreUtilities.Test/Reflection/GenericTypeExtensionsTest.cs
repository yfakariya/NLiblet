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
using System.Collections;
using System.Linq;
using NLiblet.Collections;
using System.Runtime.CompilerServices;

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
			Assert.IsTrue( typeof( SomeClassDirect ).Inherits( typeof( EqualityComparer<string> ) ) );
			Assert.IsTrue( typeof( SomeClassIndirect ).Inherits( typeof( EqualityComparer<string> ) ) );
			Assert.IsFalse( typeof( SomeClassDirect ).Inherits( typeof( EqualityComparer<int> ) ) );
			Assert.IsFalse( typeof( SomeClassIndirect ).Inherits( typeof( EqualityComparer<int> ) ) );
			Assert.IsFalse( typeof( EqualityComparer<> ).Inherits( typeof( EqualityComparer<> ) ) );

			Assert.IsTrue( typeof( StringDictionary<> ).Inherits( typeof( StringDictionaryBase<> ) ) );
			Assert.IsTrue( typeof( StringDictionary<> ).Inherits( typeof( Dictionary<,> ) ) );
			Assert.IsFalse( typeof( StringDictionary<> ).Inherits( typeof( StringDictionary<> ) ) );
		}

		[Test]
		public void TestElementInherits()
		{
			Assert.IsTrue( typeof( SomeClassDirect[] ).ElementInherits( typeof( EqualityComparer<> ) ) );
			Assert.IsTrue( typeof( SomeClassIndirect[] ).ElementInherits( typeof( EqualityComparer<> ) ) );
			TestElementInheritsHelper<string>();
		}

		private static void TestElementInheritsHelper<T>()
		{
			Assert.IsFalse( typeof( EqualityComparer<T>[] ).ElementInherits( typeof( EqualityComparer<T> ) ) );
			Assert.IsFalse( typeof( EqualityComparer<T>[] ).ElementInherits( typeof( EqualityComparer<> ) ) );

			Assert.IsTrue( typeof( StringDictionary<T>[] ).ElementInherits( typeof( StringDictionaryBase<T> ) ) );
			Assert.IsTrue( typeof( StringDictionary<T>[] ).ElementInherits( typeof( Dictionary<string, T> ) ) );
			Assert.IsFalse( typeof( StringDictionary<T>[] ).ElementInherits( typeof( StringDictionary<T> ) ) );
			Assert.IsTrue( typeof( StringDictionary<T>[] ).ElementInherits( typeof( StringDictionaryBase<> ) ) );
			Assert.IsFalse( typeof( StringDictionary<T>[] ).ElementInherits( typeof( StringDictionary<> ) ) );
		}

		[Test]
		public void TestImplements()
		{
			Assert.IsTrue( typeof( SomeClassDirect ).Implements( typeof( IEqualityComparer<string> ) ) );
			Assert.IsTrue( typeof( SomeClassIndirect ).Implements( typeof( IEqualityComparer<string> ) ) );
			Assert.IsTrue( typeof( ISomeInterfaceDirect ).Implements( typeof( IEqualityComparer<string> ) ) );
			Assert.IsTrue( typeof( ISomeInterfaceIndirect ).Implements( typeof( IEqualityComparer<string> ) ) );
			Assert.IsFalse( typeof( SomeClassDirect ).Implements( typeof( IEqualityComparer<int> ) ) );
			Assert.IsFalse( typeof( SomeClassIndirect ).Implements( typeof( IEqualityComparer<int> ) ) );
			Assert.IsFalse( typeof( ISomeInterfaceDirect ).Implements( typeof( IEqualityComparer<int> ) ) );
			Assert.IsFalse( typeof( ISomeInterfaceIndirect ).Implements( typeof( IEqualityComparer<int> ) ) );
			Assert.IsTrue( typeof( SomeClassDirect ).Implements( typeof( IEqualityComparer<> ) ) );
			Assert.IsTrue( typeof( SomeClassIndirect ).Implements( typeof( IEqualityComparer<> ) ) );
			Assert.IsTrue( typeof( ISomeInterfaceDirect ).Implements( typeof( IEqualityComparer<> ) ) );
			Assert.IsTrue( typeof( ISomeInterfaceIndirect ).Implements( typeof( IEqualityComparer<> ) ) );
			Assert.IsFalse( typeof( IEqualityComparer<> ).Implements( typeof( IEqualityComparer<> ) ) );
			Assert.IsTrue( typeof( EqualityComparer<> ).Implements( typeof( IEqualityComparer<> ) ) );
			Assert.IsTrue( typeof( string[] ).Implements( typeof( IList<> ) ) );
			Assert.IsTrue( typeof( string[] ).Implements( typeof( IEnumerable<> ) ) );

			Assert.IsTrue( typeof( StringDictionary<> ).Implements( typeof( IStringDictionary<> ) ) );
			Assert.IsTrue( typeof( StringDictionary<> ).Implements( typeof( IDictionary<,> ) ) );
			Assert.IsTrue( typeof( IStringDictionary<> ).Implements( typeof( IDictionary<,> ) ) );
		}

		[Test]
		public void TestElementImplements()
		{
			Assert.IsTrue( typeof( SomeClassDirect[] ).ElementImplements( typeof( IEqualityComparer<> ) ) );
			Assert.IsTrue( typeof( SomeClassIndirect[] ).ElementImplements( typeof( IEqualityComparer<> ) ) );
			Assert.IsTrue( typeof( ISomeInterfaceDirect[] ).ElementImplements( typeof( IEqualityComparer<> ) ) );
			Assert.IsTrue( typeof( ISomeInterfaceIndirect[] ).ElementImplements( typeof( IEqualityComparer<> ) ) );
			Assert.IsTrue( typeof( SomeClassDirect[] ).ElementImplements( typeof( IEqualityComparer<string> ) ) );
			Assert.IsTrue( typeof( SomeClassIndirect[] ).ElementImplements( typeof( IEqualityComparer<string> ) ) );
			Assert.IsTrue( typeof( ISomeInterfaceDirect[] ).ElementImplements( typeof( IEqualityComparer<string> ) ) );
			Assert.IsTrue( typeof( ISomeInterfaceIndirect[] ).ElementImplements( typeof( IEqualityComparer<string> ) ) );
			Assert.IsFalse( typeof( SomeClassDirect[] ).ElementImplements( typeof( IEqualityComparer<int> ) ) );
			Assert.IsFalse( typeof( SomeClassIndirect[] ).ElementImplements( typeof( IEqualityComparer<int> ) ) );
			Assert.IsFalse( typeof( ISomeInterfaceDirect[] ).ElementImplements( typeof( IEqualityComparer<int> ) ) );
			Assert.IsFalse( typeof( ISomeInterfaceIndirect[] ).ElementImplements( typeof( IEqualityComparer<int> ) ) );
			TestElementImplementsHelper<string>();
		}

		private static void TestElementImplementsHelper<T>()
		{
			Assert.IsFalse( typeof( IEqualityComparer<T>[] ).ElementImplements( typeof( IEqualityComparer<T> ) ) );
			Assert.IsTrue( typeof( EqualityComparer<T>[] ).ElementImplements( typeof( IEqualityComparer<T> ) ) );
			Assert.IsFalse( typeof( IEqualityComparer<T>[] ).ElementImplements( typeof( IEqualityComparer<> ) ) );
			Assert.IsTrue( typeof( EqualityComparer<T>[] ).ElementImplements( typeof( IEqualityComparer<> ) ) );

			Assert.IsTrue( typeof( StringDictionary<T>[] ).ElementImplements( typeof( IStringDictionary<T> ) ) );
			Assert.IsTrue( typeof( StringDictionary<T>[] ).ElementImplements( typeof( IDictionary<string, T> ) ) );
			Assert.IsTrue( typeof( IStringDictionary<T>[] ).ElementImplements( typeof( IDictionary<string, T> ) ) );
			Assert.IsTrue( typeof( StringDictionary<T>[] ).ElementImplements( typeof( IStringDictionary<> ) ) );
		}

		[Test]
		public void TestFindGenericTypes()
		{
			Assert.That( typeof( SomeClassDirect ).FindGenericTypes( typeof( EqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( EqualityComparer<string> ) } ) );
			Assert.That( typeof( SomeClassDirect ).FindGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<string> ) } ) );
			Assert.That( typeof( SomeClassIndirect ).FindGenericTypes( typeof( EqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( EqualityComparer<string> ) } ) );
			Assert.That( typeof( SomeClassIndirect ).FindGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<string> ) } ) );
			Assert.That( typeof( ISomeInterfaceDirect ).FindGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<string> ) } ) );
			Assert.That( typeof( ISomeInterfaceIndirect ).FindGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<string> ) } ) );
			Assert.That( typeof( EqualityComparer<> ).FindGenericTypes( typeof( EqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( EqualityComparer<> ) } ) );
			Assert.That( typeof( IEqualityComparer<> ).FindGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<> ) } ) );
			Assert.That( typeof( EqualityComparer<> ).FindGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<> ) } ) );
			Assert.That( typeof( EqualityComparer<string> ).FindGenericTypes( typeof( EqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( EqualityComparer<string> ) } ) );
			Assert.That( typeof( IEqualityComparer<string> ).FindGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<string> ) } ) );
			Assert.That( typeof( EqualityComparer<string> ).FindGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<string> ) } ) );
			Assert.That( typeof( string[] ).FindGenericTypes( typeof( IList<> ) ), Is.EquivalentTo( new[] { typeof( IList<string> ) } ) );
			Assert.That( typeof( string[] ).FindGenericTypes( typeof( IList<string> ) ), Is.EquivalentTo( new[] { typeof( IList<string> ) } ) );
			Assert.That( typeof( string[] ).FindGenericTypes( typeof( IList<int> ) ), Is.Empty );
			Assert.That( typeof( string ).FindGenericTypes( typeof( IEqualityComparer<string> ) ), Is.Empty );
			Assert.That( typeof( string ).FindGenericTypes( typeof( EqualityComparer<string> ) ), Is.Empty );
			Assert.That( typeof( string ).FindGenericTypes( typeof( IEqualityComparer<> ) ), Is.Empty );
			Assert.That( typeof( string ).FindGenericTypes( typeof( EqualityComparer<> ) ), Is.Empty );

			Assert.That( typeof( StringDictionary<> ).FindGenericTypes( typeof( StringDictionaryBase<> ) ), Is.EquivalentTo( new[] { typeof( StringDictionaryBase<> ) } ) );
			Assert.That( typeof( StringDictionary<> ).FindGenericTypes( typeof( Dictionary<,> ) ), Is.EquivalentTo( new[] { typeof( Dictionary<,> ) } ) );
			Assert.That( typeof( StringDictionary<> ).FindGenericTypes( typeof( Dictionary<,> ) ), Is.EquivalentTo( new[] { typeof( Dictionary<,> ) } ) );
			Assert.That( typeof( StringDictionary<> ).FindGenericTypes( typeof( StringDictionary<> ) ), Is.EquivalentTo( new[] { typeof( StringDictionary<> ) } ) );
			Assert.That( typeof( StringDictionary<> ).FindGenericTypes( typeof( IStringDictionary<> ) ), Is.EquivalentTo( new[] { typeof( IStringDictionary<> ) } ) );
			Assert.That( typeof( StringDictionary<> ).FindGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<,> ) } ) );
			Assert.That( typeof( StringDictionary<> ).FindGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<,> ) } ) );
			Assert.That( typeof( IStringDictionary<> ).FindGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<,> ) } ) );
			Assert.That( typeof( IStringDictionary<> ).FindGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<,> ) } ) );
			Assert.That( typeof( StringDictionary<string> ).FindGenericTypes( typeof( StringDictionaryBase<> ) ), Is.EquivalentTo( new[] { typeof( StringDictionaryBase<string> ) } ) );
			Assert.That( typeof( StringDictionary<string> ).FindGenericTypes( typeof( Dictionary<,> ) ), Is.EquivalentTo( new[] { typeof( Dictionary<string, string> ) } ) );
			Assert.That( typeof( StringDictionary<string> ).FindGenericTypes( typeof( Dictionary<,> ) ), Is.EquivalentTo( new[] { typeof( Dictionary<string, string> ) } ) );
			Assert.That( typeof( StringDictionary<string> ).FindGenericTypes( typeof( StringDictionary<> ) ), Is.EquivalentTo( new[] { typeof( StringDictionary<string> ) } ) );
			Assert.That( typeof( StringDictionary<string> ).FindGenericTypes( typeof( IStringDictionary<> ) ), Is.EquivalentTo( new[] { typeof( IStringDictionary<string> ) } ) );
			Assert.That( typeof( StringDictionary<string> ).FindGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<string, string> ) } ) );
			Assert.That( typeof( StringDictionary<string> ).FindGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<string, string> ) } ) );
			Assert.That( typeof( IStringDictionary<string> ).FindGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<string, string> ) } ) );
			Assert.That( typeof( IStringDictionary<string> ).FindGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<string, string> ) } ) );

			Assert.That( typeof( MultiType ).FindGenericTypes( typeof( IEnumerable<int> ) ), Is.EquivalentTo( new[] { typeof( IEnumerable<int> ) } ) );
			Assert.That( typeof( MultiType ).FindGenericTypes( typeof( IEnumerable<string> ) ), Is.EquivalentTo( new[] { typeof( IEnumerable<string> ) } ) );
			Assert.That( typeof( MultiType ).FindGenericTypes( typeof( IEnumerable<> ) ), Is.EquivalentTo( new[] { typeof( IEnumerable<int> ), typeof( IEnumerable<string> ) } ) );
			Assert.That( typeof( IMultiType ).FindGenericTypes( typeof( IEnumerable<int> ) ), Is.EquivalentTo( new[] { typeof( IEnumerable<int> ) } ) );
			Assert.That( typeof( IMultiType ).FindGenericTypes( typeof( IEnumerable<string> ) ), Is.EquivalentTo( new[] { typeof( IEnumerable<string> ) } ) );
			Assert.That( typeof( IMultiType ).FindGenericTypes( typeof( IEnumerable<> ) ), Is.EquivalentTo( new[] { typeof( IEnumerable<int> ), typeof( IEnumerable<string> ) } ) );
		}

		[Test]
		public void TestFindElementGenericTypes()
		{
			Assert.That( typeof( SomeClassDirect[] ).FindElementGenericTypes( typeof( EqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( EqualityComparer<string> ) } ) );
			Assert.That( typeof( SomeClassDirect[] ).FindElementGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<string> ) } ) );
			Assert.That( typeof( SomeClassIndirect[] ).FindElementGenericTypes( typeof( EqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( EqualityComparer<string> ) } ) );
			Assert.That( typeof( SomeClassIndirect[] ).FindElementGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<string> ) } ) );
			Assert.That( typeof( ISomeInterfaceDirect[] ).FindElementGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<string> ) } ) );
			Assert.That( typeof( ISomeInterfaceIndirect[] ).FindElementGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<string> ) } ) );
			Assert.That( typeof( EqualityComparer<string>[] ).FindElementGenericTypes( typeof( EqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( EqualityComparer<string> ) } ) );
			Assert.That( typeof( IEqualityComparer<string>[] ).FindElementGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<string> ) } ) );
			Assert.That( typeof( EqualityComparer<string>[] ).FindElementGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<string> ) } ) );
			Assert.That( typeof( string[] ).FindElementGenericTypes( typeof( EqualityComparer<string> ) ), Is.Empty );
			Assert.That( typeof( string[] ).FindElementGenericTypes( typeof( IEqualityComparer<> ) ), Is.Empty );
			Assert.That( typeof( string[] ).FindElementGenericTypes( typeof( EqualityComparer<> ) ), Is.Empty );

			Assert.That( typeof( StringDictionary<string>[] ).FindElementGenericTypes( typeof( StringDictionaryBase<> ) ), Is.EquivalentTo( new[] { typeof( StringDictionaryBase<string> ) } ) );
			Assert.That( typeof( StringDictionary<string>[] ).FindElementGenericTypes( typeof( Dictionary<,> ) ), Is.EquivalentTo( new[] { typeof( Dictionary<string, string> ) } ) );
			Assert.That( typeof( StringDictionary<string>[] ).FindElementGenericTypes( typeof( Dictionary<,> ) ), Is.EquivalentTo( new[] { typeof( Dictionary<string, string> ) } ) );
			Assert.That( typeof( StringDictionary<string>[] ).FindElementGenericTypes( typeof( StringDictionary<> ) ), Is.EquivalentTo( new[] { typeof( StringDictionary<string> ) } ) );
			Assert.That( typeof( StringDictionary<string>[] ).FindElementGenericTypes( typeof( IStringDictionary<> ) ), Is.EquivalentTo( new[] { typeof( IStringDictionary<string> ) } ) );
			Assert.That( typeof( StringDictionary<string>[] ).FindElementGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<string, string> ) } ) );
			Assert.That( typeof( StringDictionary<string>[] ).FindElementGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<string, string> ) } ) );
			Assert.That( typeof( IStringDictionary<string>[] ).FindElementGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<string, string> ) } ) );
			Assert.That( typeof( IStringDictionary<string>[] ).FindElementGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<string, string> ) } ) );

			Assert.That( typeof( MultiType[] ).FindElementGenericTypes( typeof( IEnumerable<int> ) ), Is.EquivalentTo( new[] { typeof( IEnumerable<int> ) } ) );
			Assert.That( typeof( MultiType[] ).FindElementGenericTypes( typeof( IEnumerable<string> ) ), Is.EquivalentTo( new[] { typeof( IEnumerable<string> ) } ) );
			Assert.That( typeof( MultiType[] ).FindElementGenericTypes( typeof( IEnumerable<> ) ), Is.EquivalentTo( new[] { typeof( IEnumerable<int> ), typeof( IEnumerable<string> ) } ) );
			Assert.That( typeof( IMultiType[] ).FindElementGenericTypes( typeof( IEnumerable<int> ) ), Is.EquivalentTo( new[] { typeof( IEnumerable<int> ) } ) );
			Assert.That( typeof( IMultiType[] ).FindElementGenericTypes( typeof( IEnumerable<string> ) ), Is.EquivalentTo( new[] { typeof( IEnumerable<string> ) } ) );
			Assert.That( typeof( IMultiType[] ).FindElementGenericTypes( typeof( IEnumerable<> ) ), Is.EquivalentTo( new[] { typeof( IEnumerable<int> ), typeof( IEnumerable<string> ) } ) );
		}

		private static void TestFindElementGenericTypes<T>()
		{
			Assert.That( typeof( EqualityComparer<T>[] ).FindElementGenericTypes( typeof( EqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( EqualityComparer<> ) } ) );
			Assert.That( typeof( IEqualityComparer<T>[] ).FindElementGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<> ) } ) );
			Assert.That( typeof( EqualityComparer<T>[] ).FindElementGenericTypes( typeof( IEqualityComparer<> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<> ) } ) );
			Assert.That( typeof( EqualityComparer<T>[] ).FindElementGenericTypes( typeof( EqualityComparer<T> ) ), Is.EquivalentTo( new[] { typeof( EqualityComparer<T> ) } ) );
			Assert.That( typeof( IEqualityComparer<T>[] ).FindElementGenericTypes( typeof( IEqualityComparer<T> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<T> ) } ) );
			Assert.That( typeof( EqualityComparer<T>[] ).FindElementGenericTypes( typeof( IEqualityComparer<T> ) ), Is.EquivalentTo( new[] { typeof( IEqualityComparer<T> ) } ) );
	
			Assert.That( typeof( StringDictionary<T>[] ).FindElementGenericTypes( typeof( StringDictionaryBase<> ) ), Is.EquivalentTo( new[] { typeof( StringDictionaryBase<> ) } ) );
			Assert.That( typeof( StringDictionary<T>[] ).FindElementGenericTypes( typeof( Dictionary<,> ) ), Is.EquivalentTo( new[] { typeof( Dictionary<,> ) } ) );
			Assert.That( typeof( StringDictionary<T>[] ).FindElementGenericTypes( typeof( Dictionary<,> ) ), Is.EquivalentTo( new[] { typeof( Dictionary<,> ) } ) );
			Assert.That( typeof( StringDictionary<T>[] ).FindElementGenericTypes( typeof( StringDictionary<> ) ), Is.EquivalentTo( new[] { typeof( StringDictionary<> ) } ) );
			Assert.That( typeof( StringDictionary<T>[] ).FindElementGenericTypes( typeof( IStringDictionary<> ) ), Is.EquivalentTo( new[] { typeof( IStringDictionary<> ) } ) );
			Assert.That( typeof( StringDictionary<T>[] ).FindElementGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<,> ) } ) );
			Assert.That( typeof( StringDictionary<T>[] ).FindElementGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<,> ) } ) );
			Assert.That( typeof( IStringDictionary<T>[] ).FindElementGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<,> ) } ) );
			Assert.That( typeof( IStringDictionary<T>[] ).FindElementGenericTypes( typeof( IDictionary<,> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<,> ) } ) );

			Assert.That( typeof( StringDictionary<T>[] ).FindElementGenericTypes( typeof( StringDictionaryBase<T> ) ), Is.EquivalentTo( new[] { typeof( StringDictionaryBase<T> ) } ) );
			Assert.That( typeof( StringDictionary<T>[] ).FindElementGenericTypes( typeof( Dictionary<string,T> ) ), Is.EquivalentTo( new[] { typeof( Dictionary<string,T> ) } ) );
			Assert.That( typeof( StringDictionary<T>[] ).FindElementGenericTypes( typeof( Dictionary<string,T> ) ), Is.EquivalentTo( new[] { typeof( Dictionary<string,T> ) } ) );
			Assert.That( typeof( StringDictionary<T>[] ).FindElementGenericTypes( typeof( StringDictionary<T> ) ), Is.EquivalentTo( new[] { typeof( StringDictionary<T> ) } ) );
			Assert.That( typeof( StringDictionary<T>[] ).FindElementGenericTypes( typeof( IStringDictionary<T> ) ), Is.EquivalentTo( new[] { typeof( IStringDictionary<T> ) } ) );
			Assert.That( typeof( StringDictionary<T>[] ).FindElementGenericTypes( typeof( IDictionary<string,T> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<string,T> ) } ) );
			Assert.That( typeof( StringDictionary<T>[] ).FindElementGenericTypes( typeof( IDictionary<string,T> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<string,T> ) } ) );
			Assert.That( typeof( IStringDictionary<T>[] ).FindElementGenericTypes( typeof( IDictionary<string,T> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<string,T> ) } ) );
			Assert.That( typeof( IStringDictionary<T>[] ).FindElementGenericTypes( typeof( IDictionary<string,T> ) ), Is.EquivalentTo( new[] { typeof( IDictionary<string,T> ) } ) );
		}

		[Test]
		public void TestAssignableTo()
		{
			// Omit some test depending on current implementation... it is bad, but enough.
			Assert.That( typeof( Object ).IsAssignableTo( typeof( string ) ), Is.False );
			Assert.That( typeof( string ).IsAssignableTo( typeof( string ) ), Is.True );
			Assert.That( typeof( string ).IsAssignableTo( typeof( IConvertible ) ), Is.True );
			Assert.That( typeof( Object ).IsAssignableTo( typeof( Object ) ), Is.True );
			Assert.That( typeof( Object ).IsAssignableTo( null ), Is.False );
		}

		[Test]
		public void TestIsClosedTypeOf()
		{
			Assert.IsFalse( typeof( IEnumerable<> ).IsClosedTypeOf( typeof( IEnumerable<> ) ) );
			Assert.IsTrue( typeof( IEnumerable<int> ).IsClosedTypeOf( typeof( IEnumerable<> ) ) );
			Assert.IsFalse( typeof( ICollection<int> ).IsClosedTypeOf( typeof( IEnumerable<> ) ) );
			Assert.IsTrue( typeof( List<int> ).IsClosedTypeOf( typeof( List<> ) ) );
			Assert.IsFalse( typeof( List<int> ).IsClosedTypeOf( typeof( IList<> ) ) );
			Assert.IsTrue( typeof( ArraySegment<int> ).IsClosedTypeOf( typeof( ArraySegment<> ) ) );
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

		private interface IStringDictionary<T> : IDictionary<string, T> { }
		private class StringDictionaryBase<T> : Dictionary<string, T>, IStringDictionary<T> { }
		private class StringDictionary<T> : StringDictionaryBase<T> { }

		private interface IMultiType : IEnumerable<string>, IEnumerable<int> { }
		private class MultiType : IMultiType
		{
			public IEnumerator<string> GetEnumerator()
			{
				throw new NotImplementedException();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				throw new NotImplementedException();
			}

			IEnumerator<int> IEnumerable<int>.GetEnumerator()
			{
				throw new NotImplementedException();
			}
		}
	}
}
