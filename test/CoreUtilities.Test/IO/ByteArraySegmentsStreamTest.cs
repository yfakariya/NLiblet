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

using NLiblet.Collections;
using System.IO;

namespace NLiblet.IO
{
	[TestFixture]
	public class ByteArraySegmentsStreamTest
	{
		[Test]
		public void WriteTest_LessThanSegmentSize()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1 }, 0, 1 );
			Assert.AreEqual( 1, target.Length );
			Assert.AreEqual( 1, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 1, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 1 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
		}

		[Test]
		public void WriteTest_EqualsToSegmentSize()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2 }, 0, 2 );
			Assert.AreEqual( 2, target.Length );
			Assert.AreEqual( 2, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 1, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
		}

		[Test]
		public void WriteTest_ExceedsSegmentSize_LessThanCapacity()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3 }, 0, 3 );
			Assert.AreEqual( 3, target.Length );
			Assert.AreEqual( 3, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 2, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 3, 1 ), list[ 1 ].AsEnumerable(), String.Join( ", ", list[ 1 ].AsEnumerable() ) );
		}

		[Test]
		public void WriteTest_ExceedsSegmentSize_EqualsToCapacity()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4 }, 0, 4 );
			Assert.AreEqual( 4, target.Length );
			Assert.AreEqual( 4, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 2, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 3, 2 ), list[ 1 ].AsEnumerable(), String.Join( ", ", list[ 1 ].AsEnumerable() ) );
		}

		[Test]
		public void WriteTest_ExceedsSegmentSize_ExceedsCapacity()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			Assert.AreEqual( 5, target.Length );
			Assert.AreEqual( 5, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 3, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 3, 2 ), list[ 1 ].AsEnumerable(), String.Join( ", ", list[ 1 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 5, 1 ), list[ 2 ].AsEnumerable(), String.Join( ", ", list[ 2 ].AsEnumerable() ) );
		}

		[Test]
		public void WriteTest_Twice()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3 }, 0, 3 );
			target.Write( new byte[] { 4, 5, 6 }, 0, 3 );
			Assert.AreEqual( 6, target.Length );
			Assert.AreEqual( 6, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 3, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 3, 2 ), list[ 1 ].AsEnumerable(), String.Join( ", ", list[ 1 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 5, 2 ), list[ 2 ].AsEnumerable(), String.Join( ", ", list[ 2 ].AsEnumerable() ) );
		}

		[Test]
		public void WriteByteTest_LessThanSegmentSize()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.WriteByte( 1 );
			Assert.AreEqual( 1, target.Length );
			Assert.AreEqual( 1, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 1, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 1 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
		}

		[Test]
		public void WriteByteTest_EqualsToSegmentSize()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.WriteByte( 1 );
			target.WriteByte( 2 );
			Assert.AreEqual( 2, target.Length );
			Assert.AreEqual( 2, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 1, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
		}

		[Test]
		public void WriteByteTest_ExceedsSegmentSize_LessThanCapacity()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.WriteByte( 1 );
			target.WriteByte( 2 );
			target.WriteByte( 3 );
			Assert.AreEqual( 3, target.Length );
			Assert.AreEqual( 3, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 2, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 3, 1 ), list[ 1 ].AsEnumerable(), String.Join( ", ", list[ 1 ].AsEnumerable() ) );
		}

		[Test]
		public void WriteByteTest_ExceedsSegmentSize_EqualsToCapacity()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.WriteByte( 1 );
			target.WriteByte( 2 );
			target.WriteByte( 3 );
			target.WriteByte( 4 );
			Assert.AreEqual( 4, target.Length );
			Assert.AreEqual( 4, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 2, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 3, 2 ), list[ 1 ].AsEnumerable(), String.Join( ", ", list[ 1 ].AsEnumerable() ) );
		}

		[Test]
		public void WriteByteTest_ExceedsSegmentSize_ExceedsCapacity()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.WriteByte( 1 );
			target.WriteByte( 2 );
			target.WriteByte( 3 );
			target.WriteByte( 4 );
			target.WriteByte( 5 );
			Assert.AreEqual( 5, target.Length );
			Assert.AreEqual( 5, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 3, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 3, 2 ), list[ 1 ].AsEnumerable(), String.Join( ", ", list[ 1 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 5, 1 ), list[ 2 ].AsEnumerable(), String.Join( ", ", list[ 2 ].AsEnumerable() ) );
		}

		[Test]
		public void PositionTest_Same()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = target.Position;
		}

		[Test]
		public void PositionTest_Head()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
		}

		[Test]
		public void PositionTest_Tail()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			target.Position = target.Length;
		}

		[Test]
		public void PositionTest_Plus()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			target.Position = 1;
		}

		[Test]
		public void PositionTest_PlusToSegmentBoundary()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			target.Position = 2;
		}

		[Test]
		public void PositionTest_Plus1Segment()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			target.Position = 3;
		}

		[Test]
		public void PositionTest_Plus2Segments()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			target.Position = 5;
		}

		[Test]
		public void PositionTest_ToCapacity()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			target.Position = 6;
			Assert.AreEqual( 6, target.Length );
		}

		[Test]
		public void PositionTest_ExceedsCapacity()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			target.Position = 7;
			Assert.AreEqual( 7, target.Length );
		}

		[Test]
		public void PositionTest_Minus()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6 );
			target.Position = 5;
		}

		[Test]
		public void PositionTest_MinusToSegmentBoundary()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6 );
			target.Position = 4;
		}

		[Test]
		public void PositionTest_Minus1Segment()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6 );
			target.Position = 3;
		}

		[Test]
		public void PositionTest_Minus2Segments()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6 );
			target.Position = 1;
		}

		[Test]
		public void Seek_Begin_0()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6 );
			target.Seek( 0, SeekOrigin.Begin );
			Assert.AreEqual( 6, target.Length );
			Assert.AreEqual( 0, target.Position );
		}

		[Test]
		public void Seek_Begin_Plus()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6 );
			target.Seek( 1, SeekOrigin.Begin );
			Assert.AreEqual( 6, target.Length );
			Assert.AreEqual( 1, target.Position );
		}

		[Test]
		public void Seek_Current_0()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6 );
			target.Position = 3;
			target.Seek( 0, SeekOrigin.Current );
			Assert.AreEqual( 6, target.Length );
			Assert.AreEqual( 3, target.Position );
		}

		[Test]
		public void Seek_Current_Plus()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6 );
			target.Position = 3;
			target.Seek( 1, SeekOrigin.Current );
			Assert.AreEqual( 6, target.Length );
			Assert.AreEqual( 4, target.Position );
		}

		[Test]
		public void Seek_Current_Minus()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6 );
			target.Position = 3;
			target.Seek( -1, SeekOrigin.Current );
			Assert.AreEqual( 6, target.Length );
			Assert.AreEqual( 2, target.Position );
		}

		[Test]
		public void Seek_End_0()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6 );
			target.Position = 3;
			target.Seek( 0, SeekOrigin.End );
			Assert.AreEqual( 6, target.Length );
			Assert.AreEqual( 6, target.Position );
		}

		[Test]
		public void Seek_End_Plus()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6 );
			target.Position = 3;
			target.Seek( 1, SeekOrigin.End );
			Assert.AreEqual( 7, target.Length );
			Assert.AreEqual( 7, target.Position );
		}

		[Test]
		public void Seek_End_Minus()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6 );
			target.Position = 3;
			target.Seek( -1, SeekOrigin.End );
			Assert.AreEqual( 6, target.Length );
			Assert.AreEqual( 5, target.Position );
		}

		[Test]
		public void ReadSegmentTest_Empty()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			var result = target.Read( 1 );
			Assert.AreEqual( 0, result.Count );
			Assert.AreEqual( 0, target.Position );
			Assert.AreEqual( 0, target.Length );
		}

		[Test]
		public void ReadSegmentTest_InTail()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.WriteByte( 1 );
			var result = target.Read( 1 );
			Assert.AreEqual( 0, result.Count );
			Assert.AreEqual( 1, target.Position );
			Assert.AreEqual( 1, target.Length );
		}

		[Test]
		public void ReadSegmentTest_FromHead()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			var result = target.Read( 1 );
			Assert.AreEqual( 1, result.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 1 ).Select( item => ( byte )item ), result[ 0 ].AsEnumerable(), String.Join( ", ", result[ 0 ].AsEnumerable() ) );
			Assert.AreEqual( 1, target.Position );
			Assert.AreEqual( 5, target.Length );
		}

		[Test]
		public void ReadSegmentTest_FromMid()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 1;
			var result = target.Read( 1 );
			Assert.AreEqual( 1, result.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 2, 1 ).Select( item => ( byte )item ), result[ 0 ].AsEnumerable(), String.Join( ", ", result[ 0 ].AsEnumerable() ) );
			Assert.AreEqual( 2, target.Position );
			Assert.AreEqual( 5, target.Length );
		}

		[Test]
		public void ReadSegmentTest_JustSegment()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			var result = target.Read( 2 );
			Assert.AreEqual( 1, result.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ).Select( item => ( byte )item ), result[ 0 ].AsEnumerable(), String.Join( ", ", result[ 0 ].AsEnumerable() ) );
			Assert.AreEqual( 2, target.Position );
			Assert.AreEqual( 5, target.Length );
		}

		[Test]
		public void ReadSegmentTest_Spanning1()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			var result = target.Read( 3 );
			Assert.AreEqual( 2, result.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ).Select( item => ( byte )item ), result[ 0 ].AsEnumerable(), String.Join( ", ", result[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 3, 1 ).Select( item => ( byte )item ), result[ 1 ].AsEnumerable(), String.Join( ", ", result[ 0 ].AsEnumerable() ) );
			Assert.AreEqual( 3, target.Position );
			Assert.AreEqual( 5, target.Length );
		}

		[Test]
		public void ReadSegmentTest_Spanning2()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			var result = target.Read( 5 );
			Assert.AreEqual( 3, result.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ).Select( item => ( byte )item ), result[ 0 ].AsEnumerable(), String.Join( ", ", result[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 3, 2 ).Select( item => ( byte )item ), result[ 1 ].AsEnumerable(), String.Join( ", ", result[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 5, 1 ).Select( item => ( byte )item ), result[ 2 ].AsEnumerable(), String.Join( ", ", result[ 0 ].AsEnumerable() ) );
			Assert.AreEqual( 5, target.Position );
			Assert.AreEqual( 5, target.Length );
		}

		[Test]
		public void ReadSegmentTest_Repeat()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			var result = target.Read( 2 );
			Assert.AreEqual( 1, result.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ).Select( item => ( byte )item ), result[ 0 ].AsEnumerable(), String.Join( ", ", result[ 0 ].AsEnumerable() ) );
			Assert.AreEqual( 2, target.Position );
			Assert.AreEqual( 5, target.Length );

			result = target.Read( 2 );
			Assert.AreEqual( 1, result.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 3, 2 ).Select( item => ( byte )item ), result[ 0 ].AsEnumerable(), String.Join( ", ", result[ 0 ].AsEnumerable() ) );
			Assert.AreEqual( 4, target.Position );
			Assert.AreEqual( 5, target.Length );

			result = target.Read( 2 );
			Assert.AreEqual( 1, result.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 5, 1 ).Select( item => ( byte )item ), result[ 0 ].AsEnumerable(), String.Join( ", ", result[ 0 ].AsEnumerable() ) );
			Assert.AreEqual( 5, target.Position );
			Assert.AreEqual( 5, target.Length );
		}

		[Test]
		public void ReadTest_Empty()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			byte[] buffer = new byte[] { 0xff, 0xff };
			Assert.AreEqual( 0, target.Read( buffer, 0, buffer.Length ) );
			Assert.IsTrue( buffer.All( item => item == 0xff ) );
			Assert.AreEqual( 0, target.Position );
			Assert.AreEqual( 0, target.Length );
		}

		[Test]
		public void ReadTest_InTail()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			byte[] buffer = new byte[] { 0xff, 0xff };
			target.WriteByte( 1 );
			Assert.AreEqual( 0, target.Read( buffer, 0, buffer.Length ) );
			Assert.IsTrue( buffer.All( item => item == 0xff ) );
			Assert.AreEqual( 1, target.Position );
			Assert.AreEqual( 1, target.Length );
		}

		[Test]
		public void ReadTest_FromHead()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			byte[] buffer = new byte[] { 0xff, 0xff };
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			Assert.AreEqual( 1, target.Read( buffer, 0, 1 ) );
			CollectionAssert.AreEqual( new byte[] { 1, 0xff }, buffer );
			Assert.AreEqual( 1, target.Position );
			Assert.AreEqual( 5, target.Length );
		}

		[Test]
		public void ReadTest_FromMid()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			byte[] buffer = new byte[] { 0xff, 0xff };
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 1;
			Assert.AreEqual( 1, target.Read( buffer, 0, 1 ) );
			CollectionAssert.AreEqual( new byte[] { 2, 0xff }, buffer );
			Assert.AreEqual( 2, target.Position );
			Assert.AreEqual( 5, target.Length );
		}

		[Test]
		public void ReadTest_JustSegment()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			byte[] buffer = new byte[] { 0xff, 0xff };
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			Assert.AreEqual( 2, target.Read( buffer, 0, buffer.Length ) );
			CollectionAssert.AreEqual( new byte[] { 1, 2 }, buffer );
			Assert.AreEqual( 2, target.Position );
			Assert.AreEqual( 5, target.Length );
		}

		[Test]
		public void ReadTest_Spanning1()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			byte[] buffer = new byte[] { 0xff, 0xff, 0xff };
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			Assert.AreEqual( 3, target.Read( buffer, 0, buffer.Length ) );
			CollectionAssert.AreEqual( new byte[] { 1, 2, 3 }, buffer );
			Assert.AreEqual( 3, target.Position );
			Assert.AreEqual( 5, target.Length );
		}

		[Test]
		public void ReadTest_Spanning2()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			byte[] buffer = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff };
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			Assert.AreEqual( 5, target.Read( buffer, 0, buffer.Length ) );
			CollectionAssert.AreEqual( new byte[] { 1, 2, 3, 4, 5 }, buffer );
			Assert.AreEqual( 5, target.Position );
			Assert.AreEqual( 5, target.Length );
		}

		[Test]
		public void ReadTest_Repeat()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			byte[] buffer = new byte[] { 0xff, 0xff };
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			Assert.AreEqual( 2, target.Read( buffer, 0, buffer.Length ) );
			CollectionAssert.AreEqual( new byte[] { 1, 2 }, buffer );
			Assert.AreEqual( 2, target.Position );
			Assert.AreEqual( 5, target.Length );

			Assert.AreEqual( 2, target.Read( buffer, 0, buffer.Length ) );
			CollectionAssert.AreEqual( new byte[] { 3, 4 }, buffer );
			Assert.AreEqual( 4, target.Position );
			Assert.AreEqual( 5, target.Length );

			Assert.AreEqual( 1, target.Read( buffer, 0, buffer.Length ) );
			CollectionAssert.AreEqual( new byte[] { 5, 4 }, buffer );
			Assert.AreEqual( 5, target.Position );
			Assert.AreEqual( 5, target.Length );
		}

		[Test]
		public void ReadByteTest_Empty()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			Assert.AreEqual( -1, target.ReadByte() );
			Assert.AreEqual( 0, target.Position );
			Assert.AreEqual( 0, target.Length );
		}

		[Test]
		public void ReadByteTest_InTail()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.WriteByte( 1 );
			Assert.AreEqual( -1, target.ReadByte() );
			Assert.AreEqual( 1, target.Position );
			Assert.AreEqual( 1, target.Length );
		}

		[Test]
		public void ReadByteTest_FromHead()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			Assert.AreEqual( 1, target.ReadByte() );
			Assert.AreEqual( 1, target.Position );
			Assert.AreEqual( 5, target.Length );
		}

		[Test]
		public void ReadByteTest_FromMid()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 1;
			Assert.AreEqual( 2, target.ReadByte() );
			Assert.AreEqual( 2, target.Position );
			Assert.AreEqual( 5, target.Length );
		}

		[Test]
		public void ReadByteTest_Repeat()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5 }, 0, 5 );
			target.Position = 0;
			for ( int i = 1; i <= 5; i++ )
			{
				Assert.AreEqual( i, target.ReadByte() );
				Assert.AreEqual( i, target.Position );
				Assert.AreEqual( 5, target.Length );
			}
		}

		[Test]
		public void InsertTest_Empty()
		{
			foreach ( int count in new[] { 1, 2, 3, 5 } )
			{
				var target = new ByteArraySegmentsStream( 2, 2 );
				target.Insert( new ArraySegment<byte>( Enumerable.Range( 1, count ).Select( item => ( byte )item ).ToArray() ) );
				Assert.AreEqual( count, target.Length );
				Assert.AreEqual( count, target.Position );
				var list = target.ToList();
				Assert.AreEqual( 1, list.Count );
				CollectionAssert.AreEqual( Enumerable.Range( 1, count ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
			}
		}

		[Test]
		public void InsertTest_Head()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2 }, 0, 2 );
			target.Position = 0;
			target.Insert( new ArraySegment<byte>( Enumerable.Range( 1, 3 ).Select( item => ( byte )item ).ToArray() ) );
			Assert.AreEqual( 5, target.Length );
			Assert.AreEqual( 3, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 2, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 3 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ), list[ 1 ].AsEnumerable(), String.Join( ", ", list[ 1 ].AsEnumerable() ) );
		}

		[Test]
		public void InsertTest_MidOfSegment()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2 }, 0, 2 );
			target.Position = 1;
			target.Insert( new ArraySegment<byte>( Enumerable.Range( 1, 3 ).Select( item => ( byte )item ).ToArray() ) );
			Assert.AreEqual( 5, target.Length );
			Assert.AreEqual( 4, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 3, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 1 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 3 ), list[ 1 ].AsEnumerable(), String.Join( ", ", list[ 1 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 2, 1 ), list[ 2 ].AsEnumerable(), String.Join( ", ", list[ 2 ].AsEnumerable() ) );
		}

		[Test]
		public void InsertTest_BetweenSegments()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4 }, 0, 4 );
			target.Position = 2;
			target.Insert( new ArraySegment<byte>( Enumerable.Range( 1, 3 ).Select( item => ( byte )item ).ToArray() ) );
			Assert.AreEqual( 7, target.Length );
			Assert.AreEqual( 5, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 3, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 3 ), list[ 1 ].AsEnumerable(), String.Join( ", ", list[ 1 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 3, 2 ), list[ 2 ].AsEnumerable(), String.Join( ", ", list[ 2 ].AsEnumerable() ) );
		}

		[Test]
		public void InsertTest_MidOfLastSegment()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2, 3, 4, 5, 6 }, 0, 6 );
			Assert.AreEqual( 6, target.Length );
			Assert.AreEqual( 6, target.Position );
			target.Position = 5;
			target.Insert( new ArraySegment<byte>( Enumerable.Range( 1, 3 ).Select( item => ( byte )item ).ToArray() ) );
			Assert.AreEqual( 9, target.Length );
			Assert.AreEqual( 8, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 5, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 3, 2 ), list[ 1 ].AsEnumerable(), String.Join( ", ", list[ 2 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 5, 1 ), list[ 2 ].AsEnumerable(), String.Join( ", ", list[ 1 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 3 ), list[ 3 ].AsEnumerable(), String.Join( ", ", list[ 1 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 6, 1 ), list[ 4 ].AsEnumerable(), String.Join( ", ", list[ 1 ].AsEnumerable() ) );
		}

		[Test]
		public void InsertTest_Tail()
		{
			var target = new ByteArraySegmentsStream( 2, 2 );
			target.Write( new byte[] { 1, 2 }, 0, 2 );
			target.Insert( new ArraySegment<byte>( Enumerable.Range( 1, 3 ).Select( item => ( byte )item ).ToArray() ) );
			Assert.AreEqual( 5, target.Length );
			Assert.AreEqual( 5, target.Position );
			var list = target.ToList();
			Assert.AreEqual( 2, list.Count );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 2 ), list[ 0 ].AsEnumerable(), String.Join( ", ", list[ 0 ].AsEnumerable() ) );
			CollectionAssert.AreEqual( Enumerable.Range( 1, 3 ), list[ 1 ].AsEnumerable(), String.Join( ", ", list[ 1 ].AsEnumerable() ) );
		}
	}
}
