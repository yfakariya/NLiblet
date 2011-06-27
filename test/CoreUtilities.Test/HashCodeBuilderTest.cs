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
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace NLiblet
{
	[TestFixture]
	public class HashCodeBuilderTest
	{
		[Test]
		public void TestDefault()
		{
			var target = default( HashCodeBuilder );

			// Check just as that any exception is not thrown.
			Assert.AreEqual( target.GetHashCode(), target.BuildHashCode() );
			target.ToString();
		}

		[Test]
		public void TestAppendHashCode()
		{
			var target = default( HashCodeBuilder );
			Assert.AreEqual( target.BuildHashCode(), target.Append( default( string ) ).BuildHashCode() );
			Assert.AreEqual( target.BuildHashCode() ^ ( 1 ).GetHashCode(), target.Append( 1 ).BuildHashCode() );
			Assert.AreEqual(
				target.BuildHashCode() ^ ( 13 ).GetHashCode() ^ "abc".GetHashCode() ^ "あいう".GetHashCode(),
				target.Append( 13 ).Append( "abc" ).Append( "あいう" ).BuildHashCode() 
			);
		}

		[Test]
		public void TestEquals()
		{
			var target = default( HashCodeBuilder );
			Assert.IsTrue( target.Equals( default( HashCodeBuilder ) ) );
			Assert.IsTrue( target.Equals( ( object )default( HashCodeBuilder ) ) );
			Assert.IsTrue( target == default( HashCodeBuilder ) );
			Assert.IsFalse( target != default( HashCodeBuilder ) );
			Assert.IsFalse( target.Equals( default( HashCodeBuilder ).Append( 1 ) ) );
			Assert.IsFalse( target.Equals( ( object )default( HashCodeBuilder ).Append( 1 ) ) );
			Assert.IsFalse( target == default( HashCodeBuilder ).Append( 1 ) );
			Assert.IsTrue( target != default( HashCodeBuilder ).Append( 1 ) );
			Assert.IsFalse( target.Equals( null ) );
		}
	}
}
