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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace NLiblet.Collections
{
	[TestFixture]
	[Timeout( 1000 )]
	public class LinkedHashSetTest
	{
		[Test]
		public void TestAddForEach()
		{
			var target = new LinkedHashSet<int>();

			// empty
			Assert.That( target.Any(), Is.False );

			var first = target.Add( 1 );
			Assert.That( first.Set, Is.SameAs( target ) );
			Assert.That( first.IsHead, Is.True );
			Assert.That( first.IsTail, Is.True );
			Assert.That( first.Next, Is.Null );
			Assert.That( first.Previous, Is.Null );
			Assert.That( first.Value, Is.EqualTo( 1 ) );
			Assert.That( target.Count, Is.EqualTo( 1 ) );

			var second = target.Add( 2 );
			Assert.That( first.Set, Is.SameAs( target ) );
			Assert.That( first.IsHead, Is.True );
			Assert.That( first.IsTail, Is.False );
			Assert.That( first.Next, Is.SameAs( second ) );
			Assert.That( first.Previous, Is.Null );
			Assert.That( first.Value, Is.EqualTo( 1 ) );
			Assert.That( second.Set, Is.SameAs( target ) );
			Assert.That( second.IsHead, Is.False );
			Assert.That( second.IsTail, Is.True );
			Assert.That( second.Next, Is.Null );
			Assert.That( second.Previous, Is.SameAs( first ) );
			Assert.That( second.Value, Is.EqualTo( 2 ) );
			Assert.That( target.Count, Is.EqualTo( 2 ) );

			var iterated = new List<int>();
			foreach ( var item in target )
			{
				iterated.Add( item );
			}

			Assert.That( iterated[ 0 ], Is.EqualTo( first.Value ) );
			Assert.That( iterated[ 1 ], Is.EqualTo( second.Value ) );
		}

		[Test]
		public void TestAdd_Duplicate()
		{
			var target = new LinkedHashSet<int>();
			var first = target.Add( 1 );
			Assert.That( first, Is.Not.Null );
			var second = target.Add( 1 );
			Assert.That( second, Is.Null );
		}

		[Test]
		public void TestAddRemove()
		{
			var target = new LinkedHashSet<int>();
			var removed = target.Remove( 1 );
			Assert.That( removed, Is.Null );
			var node0 = target.Add( 1 );
			Assert.That( target.Contains( 1 ), Is.True );
			Assert.That( target.Head, Is.SameAs( node0 ) );
			Assert.That( target.Tail, Is.SameAs( node0 ) );
			var node1 = target.Add( 2 );
			Assert.That( target.Contains( 2 ), Is.True );
			Assert.That( target.Head, Is.SameAs( node0 ) );
			Assert.That( target.Tail, Is.SameAs( node1 ) );
			var node2 = target.Add( 3 );
			Assert.That( target.Contains( 3 ), Is.True );
			Assert.That( target.Head, Is.SameAs( node0 ) );
			Assert.That( target.Tail, Is.SameAs( node2 ) );
			var node3 = target.Add( 4 );
			Assert.That( target.Contains( 4 ), Is.True );
			Assert.That( target.Head, Is.SameAs( node0 ) );
			Assert.That( target.Tail, Is.SameAs( node3 ) );

			// Intermediate
			removed = target.Remove( 2 );
			Assert.That( target.Contains( 2 ), Is.False );
			Assert.That( target.Head, Is.SameAs( node0 ) );
			Assert.That( target.Tail, Is.SameAs( node3 ) );
			Assert.That( removed, Is.Not.Null );
			VerifyRemovedNode( removed, node1, 2 );
			Assert.That( node0.Next, Is.SameAs( node2 ) );
			Assert.That( node2.Previous, Is.SameAs( node0 ) );
			Assert.That( target.Count, Is.EqualTo( 3 ) );
			Assert.That( target.Any(), Is.True );

			// Head
			removed = target.Remove( 1 );
			Assert.That( target.Contains( 1 ), Is.False );
			Assert.That( target.Head, Is.SameAs( node2 ) );
			Assert.That( target.Tail, Is.SameAs( node3 ) );
			Assert.That( removed, Is.Not.Null );
			VerifyRemovedNode( removed, node0, 1 );
			Assert.That( node2.Previous, Is.Null );
			Assert.That( target.Count, Is.EqualTo( 2 ) );
			Assert.That( target.Any(), Is.True );

			// Tail
			removed = target.Remove( 4 );
			Assert.That( target.Contains( 4 ), Is.False );
			Assert.That( target.Head, Is.SameAs( node2 ) );
			Assert.That( target.Tail, Is.SameAs( node2 ) );
			Assert.That( removed, Is.Not.Null );
			VerifyRemovedNode( removed, node3, 4 );
			Assert.That( node2.Previous, Is.Null );
			Assert.That( node2.Next, Is.Null );
			Assert.That( target.Count, Is.EqualTo( 1 ) );
			Assert.That( target.Any(), Is.True );

			// To be empty
			Assert.That( target.Remove( 1 ), Is.Null );
			Assert.That( target.Remove( 4 ), Is.Null );
			removed = target.Remove( 3 );
			Assert.That( target.Contains( 3 ), Is.False );
			Assert.That( target.Head, Is.Null );
			Assert.That( target.Tail, Is.Null );
			Assert.That( removed, Is.Not.Null );
			VerifyRemovedNode( removed, node2, 3 );
			Assert.That( target.Count, Is.EqualTo( 0 ) );
			Assert.That( target.Any(), Is.False );
		}

		private static void VerifyRemovedNode<T>( LinkedSetNode<T> removed, LinkedSetNode<T> expectedNode, T expectedValue )
		{
			Assert.That( removed, Is.SameAs( expectedNode ) );
			Assert.That( removed.Set, Is.Null );
			Assert.That( removed.Next, Is.Null );
			Assert.That( removed.Previous, Is.Null );
			Assert.That( removed.IsHead, Is.True );
			Assert.That( removed.IsTail, Is.True );
			Assert.That( removed.Value, Is.EqualTo( expectedValue ) );
		}

		[Test]
		public void TestRemove_Node()
		{
			var target = new LinkedHashSet<int>();
			var node0 = target.Add( 1 );
			var node1 = target.Add( 2 );
			var node2 = target.Add( 3 );
			var node3 = target.Add( 4 );

			// Intermediate
			target.Remove( node1 );
			Assert.That( target.Contains( node1.Value ), Is.False );
			Assert.That( target.Head, Is.SameAs( node0 ) );
			Assert.That( target.Tail, Is.SameAs( node3 ) );
			VerifyRemovedNode( node1, node1, 2 );
			Assert.That( node0.Next, Is.SameAs( node2 ) );
			Assert.That( node2.Previous, Is.SameAs( node0 ) );
			Assert.That( target.Count, Is.EqualTo( 3 ) );
			Assert.That( target.Any(), Is.True );

			// Head
			target.Remove( node0 );
			Assert.That( target.Contains( node0.Value ), Is.False );
			Assert.That( target.Head, Is.SameAs( node2 ) );
			Assert.That( target.Tail, Is.SameAs( node3 ) );
			VerifyRemovedNode( node0, node0, 1 );
			Assert.That( node2.Previous, Is.Null );
			Assert.That( target.Count, Is.EqualTo( 2 ) );
			Assert.That( target.Any(), Is.True );

			// Tail
			target.Remove( node3 );
			Assert.That( target.Contains( node3.Value ), Is.False );
			Assert.That( target.Head, Is.SameAs( node2 ) );
			Assert.That( target.Tail, Is.SameAs( node2 ) );
			VerifyRemovedNode( node3, node3, 4 );
			Assert.That( node2.Previous, Is.Null );
			Assert.That( node2.Next, Is.Null );
			Assert.That( target.Count, Is.EqualTo( 1 ) );
			Assert.That( target.Any(), Is.True );

			// To be empty
			target.Remove( node2 );
			Assert.That( target.Contains( node2.Value ), Is.False );
			Assert.That( target.Head, Is.Null );
			Assert.That( target.Tail, Is.Null );
			VerifyRemovedNode( node2, node2, 3 );
			Assert.That( target.Count, Is.EqualTo( 0 ) );
			Assert.That( target.Any(), Is.False );
		}

		[Test]
		public void TestClear()
		{
			var target = new LinkedHashSet<int>();
			var nodes = new List<LinkedSetNode<int>>();
			nodes.Add( target.Add( 1 ) );
			nodes.Add( target.Add( 2 ) );
			nodes.Add( target.Add( 3 ) );
			nodes.Add( target.Add( 4 ) );

			target.Clear();
			Assert.That( target.Count, Is.EqualTo( 0 ) );
			Assert.That( target.Any(), Is.False );
			Assert.That( target.Head, Is.Null );
			Assert.That( target.Tail, Is.Null );
			foreach ( var node in nodes )
			{
				Assert.That( target.Contains( node.Value ), Is.False );
				Assert.That( node.Set, Is.Null );
				Assert.That( node.IsHead, Is.True );
				Assert.That( node.IsTail, Is.True );
				Assert.That( node.Previous, Is.Null );
				Assert.That( node.Next, Is.Null );
			}
		}

		[Test]
		public void TestCopyTo()
		{
			var target = new LinkedHashSet<int>();
			var array = new int[ 5 ];

			// empty
			target.CopyTo( array );
			VerifyArray( target, array, 0, 0 );
			target.CopyTo( array, 1 );
			VerifyArray( target, array, 0, 0 );
			target.CopyTo( 1, array, 1, 2 );
			VerifyArray( target, array, 0, 0 );

			target.Add( 1 );
			target.Add( 2 );
			target.Add( 3 );

			target.CopyTo( array );
			VerifyArray( target, array, 0, 3 );
			Array.Clear( array, 0, array.Length );

			target.CopyTo( array, 1 );
			VerifyArray( target, array, 1, 3 );
			Array.Clear( array, 0, array.Length );

			target.CopyTo( 1, array, 1, 2 );
			Assert.That( array[ 0 ], Is.EqualTo( default( int ) ) );
			Assert.That( array[ 1 ], Is.EqualTo( 2 ) );
			Assert.That( array[ 2 ], Is.EqualTo( 3 ) );
			Assert.That( array[ 3 ], Is.EqualTo( default( int ) ) );
			Assert.That( array[ 4 ], Is.EqualTo( default( int ) ) );
		}

		private static void VerifyArray<T>( LinkedHashSet<T> target, T[] array, int startAt, int count )
		{
			var enumerator = target.GetEnumerator();
			try
			{
				for ( int i = 0; i < array.Length; i++ )
				{
					if ( i < startAt || startAt + count <= i )
					{
						Assert.That( array[ i ], Is.EqualTo( default( T ) ) );
					}
					else
					{
						var hasNext = enumerator.MoveNext();
						Assert.That( hasNext, Is.True );
						Assert.That( enumerator.Current, Is.EqualTo( array[ i ] ) );
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		private static void VerifyArray<T>( ICollection<T> target, T[] array, int startAt, int count )
		{
			var enumerator = target.GetEnumerator();
			try
			{
				for ( int i = 0; i < array.Length; i++ )
				{
					if ( i < startAt || startAt + count <= i )
					{
						Assert.That( array[ i ], Is.EqualTo( default( T ) ) );
					}
					else
					{
						var hasNext = enumerator.MoveNext();
						Assert.That( hasNext, Is.True );
						Assert.That( enumerator.Current, Is.EqualTo( array[ i ] ) );
					}
				}
			}
			finally
			{
				enumerator.Dispose();
			}
		}

		[Test]
		public void TestGetReverseEnumerator()
		{
			var target = new LinkedHashSet<int>();

			var first = target.Add( 1 );
			var second = target.Add( 2 );

			var iterated = new List<int>();
			var enumerator = target.GetReverseEnumerator();
			try
			{
				while ( enumerator.MoveNext() )
				{
					iterated.Add( enumerator.Current );
				}
			}
			finally
			{
				enumerator.Dispose();
			}

			Assert.That( iterated[ 1 ], Is.EqualTo( first.Value ) );
			Assert.That( iterated[ 0 ], Is.EqualTo( second.Value ) );
		}

		[Test]
		public void TestReverse()
		{
			var target = new LinkedHashSet<int>();

			// empty
			Assert.That( target.Reverse().Any(), Is.False );

			var first = target.Add( 1 );
			var second = target.Add( 2 );

			var iterated = new List<int>();
			foreach ( var item in target.Reverse() )
			{
				iterated.Add( item );
			}

			Assert.That( iterated[ 1 ], Is.EqualTo( first.Value ) );
			Assert.That( iterated[ 0 ], Is.EqualTo( second.Value ) );
		}

		[Test]
		public void TestFirst()
		{
			var target = new LinkedHashSet<int>();
			// empty
			VerifyFirstLast( target.First, default( int ), false, false );

			// 1
			target.Add( 1 );
			VerifyFirstLast( target.First, 1, true, false );

			// 2
			target.Add( 2 );
			// true first
			VerifyFirstLast( () => target.First( item => item == 1 ), 1, true, false );
			// true second
			VerifyFirstLast( () => target.First( item => item == 2 ), 2, true, false );
			// true both
			VerifyFirstLast( () => target.First( item => 0 < item ), 1, true, false );
			// false
			VerifyFirstLast( () => target.First( item => item < 0 ), 0, false, false );

			// Default
			target.Clear();
			// empty
			VerifyFirstLast( target.FirstOrDefault, default( int ), false, true );

			// 1
			target.Add( 1 );
			VerifyFirstLast( target.FirstOrDefault, 1, true, true );

			// 2
			target.Add( 2 );
			// true first
			VerifyFirstLast( () => target.FirstOrDefault( item => item == 1 ), 1, true, true );
			// true second
			VerifyFirstLast( () => target.FirstOrDefault( item => item == 2 ), 2, true, true );
			// true both
			VerifyFirstLast( () => target.FirstOrDefault( item => 0 < item ), 1, true, true );
			// false
			VerifyFirstLast( () => target.FirstOrDefault( item => item < 0 ), 0, false, true );
		}

		[Test]
		public void TestLast()
		{
			var target = new LinkedHashSet<int>();
			// empty
			VerifyFirstLast( target.Last, default( int ), false, false );

			// 1
			target.Add( 1 );
			VerifyFirstLast( target.Last, 1, true, false );

			// 2
			target.Add( 2 );
			// true first
			VerifyFirstLast( () => target.Last( item => item == 1 ), 1, true, false );
			// true second
			VerifyFirstLast( () => target.Last( item => item == 2 ), 2, true, false );
			// true both
			VerifyFirstLast( () => target.Last( item => 0 < item ), 2, true, false );
			// false
			VerifyFirstLast( () => target.Last( item => item < 0 ), 0, false, false );

			// Default
			target.Clear();
			// empty
			VerifyFirstLast( target.LastOrDefault, default( int ), false, true );

			// 1
			target.Add( 1 );
			VerifyFirstLast( target.LastOrDefault, 1, true, true );

			// 2
			target.Add( 2 );
			// true first
			VerifyFirstLast( () => target.LastOrDefault( item => item == 1 ), 1, true, true );
			// true second
			VerifyFirstLast( () => target.LastOrDefault( item => item == 2 ), 2, true, true );
			// true both
			VerifyFirstLast( () => target.LastOrDefault( item => 0 < item ), 2, true, true );
			// false
			VerifyFirstLast( () => target.LastOrDefault( item => item < 0 ), 0, false, true );
		}

		private static void VerifyFirstLast<T>( Func<T> target, T expected, bool success, bool isDefault )
		{
			try
			{
				T actual = target();
				if ( success )
				{
					Assert.That( EqualityComparer<T>.Default.Equals( expected, actual ), Is.True );
				}
				else if ( isDefault )
				{
					Assert.That( EqualityComparer<T>.Default.Equals( default( T ), actual ), Is.True );
				}
			}
			catch ( InvalidOperationException )
			{
				Assert.IsFalse( success );
				Assert.IsFalse( isDefault );
			}
		}

		#region -- Ported from Mono's Dictionary`2 unit tests --

		[Test] // bug 432441
		public void Clear_Iterators()
		{
			var d = new LinkedHashSet<object>();

			d.Add( new object() );
			d.Clear();
			int hash = 0;
			foreach ( object o in d )
			{
				hash += o.GetHashCode();
			}
		}

		[Test]
		public void IEnumeratorTest()
		{
			var _dictionary = new LinkedHashSet<string>();
			_dictionary.Add( "value1" );
			_dictionary.Add( "value2" );
			_dictionary.Add( "value3" );
			_dictionary.Add( "value4" );
			IEnumerator itr = ( ( IEnumerable )_dictionary ).GetEnumerator();
			while ( itr.MoveNext() )
			{
				object o = itr.Current;
				Assert.AreEqual( typeof( string ), o.GetType(), "Current should return a type of KeyValuePair" );
				string entry = ( string )itr.Current;
			}
		}

		[Test]
		public void ForEachTest()
		{
			var _dictionary = new LinkedHashSet<string>();
			_dictionary.Add( "value1" );
			_dictionary.Add( "value2" );
			_dictionary.Add( "value3" );
			_dictionary.Add( "value4" );

			int i = 0;
			foreach ( string entry in _dictionary )
				i++;
			Assert.AreEqual( 4, i, "fail1: foreach entry failed!" );

			i = 0;
			foreach ( string entry in ( ( IEnumerable )_dictionary ) )
				i++;
			Assert.AreEqual( 4, i, "fail2: foreach entry failed!" );
		}

		[Test, ExpectedException( typeof( InvalidOperationException ) )]
		public void FailFastTest1()
		{
			var d = new LinkedHashSet<int>();
			d.Add( 1 );
			int count = 0;
			foreach ( int kv in d )
			{
				d.Add( 2 );
				if ( count++ != 0 )
					Assert.Fail( "Should not be reached" );
			}
			Assert.Fail( "Should not be reached" );
		}

		[Test]
		public void ResetShimEnumerator()
		{
			var test = new LinkedHashSet<string>();
			test.Add( "singe" );
			test.Add( "mono" );
			test.Add( "monkey" );

			IEnumerator enumerator = test.GetEnumerator();

			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );

			enumerator.Reset();

			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsFalse( enumerator.MoveNext() );
		}

		[Test]
		public void ICollectionContains()
		{
			var dictionary = new LinkedHashSet<int>();
			dictionary.Add( 42 );
			dictionary.Add( 12 );

			var collection = dictionary as ICollection<int>;

			Assert.AreEqual( 2, collection.Count );

			Assert.IsFalse( collection.Contains( 13 ) );
			Assert.IsTrue( collection.Contains( 42 ) );
		}

		[Test]
		public void ICollectionRemove()
		{
			var dictionary = new LinkedHashSet<int>();
			dictionary.Add( 42 );
			dictionary.Add( 12 );

			var collection = dictionary as ICollection<int>;

			Assert.AreEqual( 2, collection.Count );

			Assert.IsFalse( collection.Remove( 13 ) );
			Assert.IsFalse( collection.Remove( 13 ) );
			Assert.IsTrue( collection.Remove( 42 ) );

			Assert.AreEqual( 12, collection.First() );
			Assert.IsFalse( dictionary.Contains( 42 ) );
		}

		delegate void D();
		bool Throws( D d )
		{
			try
			{
				d();
				return false;
			}
			catch
			{
				return true;
			}
		}

		[Test]
		// based on #491858, #517415
		public void Enumerator_Current()
		{
			var e1 = new LinkedHashSet<int>.Enumerator();
			Assert.IsFalse( Throws( delegate { var x = e1.Current; } ) );

			var d = new LinkedHashSet<int>();
			var e2 = d.GetEnumerator();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );
			e2.MoveNext();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );
			e2.Dispose();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );

			var e3 = ( ( IEnumerable<int> )d ).GetEnumerator();
			Assert.IsFalse( Throws( delegate { var x = e3.Current; } ) );
			e3.MoveNext();
			Assert.IsFalse( Throws( delegate { var x = e3.Current; } ) );
			e3.Dispose();
			Assert.IsFalse( Throws( delegate { var x = e3.Current; } ) );

			var e4 = ( ( IEnumerable )d ).GetEnumerator();
			Assert.IsTrue( Throws( delegate { var x = e4.Current; } ) );
			e4.MoveNext();
			Assert.IsTrue( Throws( delegate { var x = e4.Current; } ) );
			( ( IDisposable )e4 ).Dispose();
			Assert.IsTrue( Throws( delegate { var x = e4.Current; } ) );
		}

		#endregion -- Ported from Mono's Dictionary`2 unit tests --

		[Test]
		public void TestIsProperSupersetOf()
		{
			var target = new LinkedHashSet<int>();

			// empty
			Assert.That( target.IsProperSupersetOf( new int[ 0 ] ), Is.False );
			Assert.That( target.IsProperSupersetOf( new int[] { 1, 2, 3 } ), Is.False );

			target.Add( 1 );
			target.Add( 2 );
			target.Add( 3 );

			Assert.That( target.IsProperSupersetOf( new int[] { 1, 2, 3 } ), Is.False );
			Assert.That( target.IsProperSupersetOf( new int[] { 1, 2 } ), Is.True );
			Assert.That( target.IsProperSupersetOf( new int[] { 1, 2, 3, 4 } ), Is.False );
			Assert.That( target.IsProperSupersetOf( new int[] { 1, 2, 3, 1 } ), Is.False );
			Assert.That( target.IsProperSupersetOf( new int[] { 1, 2, 1 } ), Is.True );
			Assert.That( target.IsProperSupersetOf( new int[] { 1, 2, 3, 4, 1 } ), Is.False );
			Assert.That( target.IsProperSupersetOf( new int[ 0 ] ), Is.True );
		}

		[Test]
		public void TestIsSupersetOf()
		{
			var target = new LinkedHashSet<int>();

			// empty
			Assert.That( target.IsSupersetOf( new int[ 0 ] ), Is.True );
			Assert.That( target.IsSupersetOf( new int[] { 1, 2, 3 } ), Is.False );

			target.Add( 1 );
			target.Add( 2 );
			target.Add( 3 );

			Assert.That( target.IsSupersetOf( new int[] { 1, 2, 3 } ), Is.True );
			Assert.That( target.IsSupersetOf( new int[] { 1, 2 } ), Is.True );
			Assert.That( target.IsSupersetOf( new int[] { 1, 2, 3, 4 } ), Is.False );
			Assert.That( target.IsSupersetOf( new int[] { 1, 2, 3, 1 } ), Is.True );
			Assert.That( target.IsSupersetOf( new int[] { 1, 2, 1 } ), Is.True );
			Assert.That( target.IsSupersetOf( new int[] { 1, 2, 3, 4, 1 } ), Is.False );
			Assert.That( target.IsSupersetOf( new int[ 0 ] ), Is.True );
		}

		[Test]
		public void TestIsProperSubsetOf()
		{
			var target = new LinkedHashSet<int>();

			// empty
			Assert.That( target.IsProperSubsetOf( new int[ 0 ] ), Is.False );
			Assert.That( target.IsProperSubsetOf( new int[] { 1, 2, 3 } ), Is.True );

			target.Add( 1 );
			target.Add( 2 );
			target.Add( 3 );

			Assert.That( target.IsProperSubsetOf( new int[] { 1, 2, 3 } ), Is.False );
			Assert.That( target.IsProperSubsetOf( new int[] { 1, 2 } ), Is.False );
			Assert.That( target.IsProperSubsetOf( new int[] { 1, 2, 3, 4 } ), Is.True );
			Assert.That( target.IsProperSubsetOf( new int[] { 1, 2, 3, 1 } ), Is.False );
			Assert.That( target.IsProperSubsetOf( new int[] { 1, 2, 1 } ), Is.False );
			Assert.That( target.IsProperSubsetOf( new int[] { 1, 2, 3, 4, 1 } ), Is.True );
			Assert.That( target.IsProperSubsetOf( new int[ 0 ] ), Is.False );
		}

		[Test]
		public void TestIsSubsetOf()
		{
			var target = new LinkedHashSet<int>();

			// empty
			Assert.That( target.IsSubsetOf( new int[ 0 ] ), Is.True );
			Assert.That( target.IsSubsetOf( new int[] { 1, 2, 3 } ), Is.True );

			target.Add( 1 );
			target.Add( 2 );
			target.Add( 3 );

			Assert.That( target.IsSubsetOf( new int[] { 1, 2, 3 } ), Is.True );
			Assert.That( target.IsSubsetOf( new int[] { 1, 2 } ), Is.False );
			Assert.That( target.IsSubsetOf( new int[] { 1, 2, 3, 4 } ), Is.True );
			Assert.That( target.IsSubsetOf( new int[] { 1, 2, 3, 1 } ), Is.True );
			Assert.That( target.IsSubsetOf( new int[] { 1, 2, 1 } ), Is.False );
			Assert.That( target.IsSubsetOf( new int[] { 1, 2, 3, 4, 1 } ), Is.True );
			Assert.That( target.IsSubsetOf( new int[ 0 ] ), Is.False );
		}

		[Test]
		public void TestOverlaps()
		{
			var target = new LinkedHashSet<int>();

			// empty
			Assert.That( target.Overlaps( new int[ 0 ] ), Is.False );
			Assert.That( target.Overlaps( new int[] { 1, 2, 3 } ), Is.False );

			target.Add( 1 );
			target.Add( 2 );
			target.Add( 3 );

			Assert.That( target.Overlaps( new int[] { 1, 2, 3 } ), Is.True );
			Assert.That( target.Overlaps( new int[] { 1, 2 } ), Is.True );
			Assert.That( target.Overlaps( new int[] { 1, 2, 3, 4 } ), Is.True );
			Assert.That( target.Overlaps( new int[] { 1, 2, 3, 1 } ), Is.True );
			Assert.That( target.Overlaps( new int[] { 1, 2, 1 } ), Is.True );
			Assert.That( target.Overlaps( new int[] { 1, 2, 3, 4, 1 } ), Is.True );
			Assert.That( target.Overlaps( new int[ 0 ] ), Is.False );
		}

		[Test]
		public void TestSetEquals()
		{
			var target = new LinkedHashSet<int>();

			// empty
			Assert.That( target.SetEquals( new int[ 0 ] ), Is.True );
			Assert.That( target.SetEquals( new int[] { 1, 2, 3 } ), Is.False );

			target.Add( 1 );
			target.Add( 2 );
			target.Add( 3 );

			Assert.That( target.SetEquals( new int[] { 1, 2, 3 } ), Is.True );
			Assert.That( target.SetEquals( new int[] { 1, 2 } ), Is.False );
			Assert.That( target.SetEquals( new int[] { 1, 2, 3, 4 } ), Is.False );
			Assert.That( target.SetEquals( new int[] { 1, 2, 3, 1 } ), Is.True );
			Assert.That( target.SetEquals( new int[] { 1, 2, 1 } ), Is.False );
			Assert.That( target.SetEquals( new int[] { 1, 2, 3, 4, 1 } ), Is.False );
			Assert.That( target.SetEquals( new int[ 0 ] ), Is.False );
		}

		[Test]
		public void TestExceptWith()
		{
			// empty
			AssertContains( new LinkedHashSet<int>(), target => target.ExceptWith( new int[ 0 ] ) );
			AssertContains( new LinkedHashSet<int>(), target => target.ExceptWith( new int[] { 1, 2, 3 } ) );

			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.ExceptWith( new int[] { 1, 2, 3 } ) );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.ExceptWith( new int[] { 1, 2 } ), 3 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.ExceptWith( new int[] { 1, 2, 3, 4 } ) );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.ExceptWith( new int[] { 1, 2, 3, 1 } ) );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.ExceptWith( new int[] { 1, 2, 1 } ), 3 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.ExceptWith( new int[] { 1, 2, 3, 4, 1 } ) );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.ExceptWith( new int[ 0 ] ), 1, 2, 3 );
		}

		[Test]
		public void TestIntersectWith()
		{
			// empty
			AssertContains( new LinkedHashSet<int>(), target => target.IntersectWith( new int[ 0 ] ) );
			AssertContains( new LinkedHashSet<int>(), target => target.IntersectWith( new int[] { 1, 2, 3 } ) );

			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.IntersectWith( new int[] { 1, 2, 3 } ), 1, 2, 3 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.IntersectWith( new int[] { 1, 2 } ), 1, 2 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.IntersectWith( new int[] { 1, 2, 3, 4 } ), 1, 2, 3 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.IntersectWith( new int[] { 1, 2, 3, 1 } ), 1, 2, 3 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.IntersectWith( new int[] { 1, 2, 1 } ), 1, 2 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.IntersectWith( new int[] { 1, 2, 3, 4, 1 } ), 1, 2, 3 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.IntersectWith( new int[ 0 ] ) );
		}

		[Test]
		public void TestUnionWith()
		{
			// empty
			AssertContains( new LinkedHashSet<int>(), target => target.UnionWith( new int[ 0 ] ) );
			AssertContains( new LinkedHashSet<int>(), target => target.UnionWith( new int[] { 1, 2, 3 } ), 1, 2, 3 );

			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.UnionWith( new int[] { 1, 2, 3 } ), 1, 2, 3 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.UnionWith( new int[] { 1, 2 } ), 1, 2, 3 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.UnionWith( new int[] { 1, 2, 3, 4 } ), 1, 2, 3, 4 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.UnionWith( new int[] { 1, 2, 3, 1 } ), 1, 2, 3 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.UnionWith( new int[] { 1, 2, 1 } ), 1, 2, 3 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.UnionWith( new int[] { 1, 2, 3, 4, 1 } ), 1, 2, 3, 4 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.UnionWith( new int[ 0 ] ), 1, 2, 3 );
		}

		[Test]
		public void TestSymmetricExceptWith()
		{
			// empty
			AssertContains( new LinkedHashSet<int>(), target => target.SymmetricExceptWith( new int[ 0 ] ) );
			AssertContains( new LinkedHashSet<int>(), target => target.SymmetricExceptWith( new int[] { 1, 2, 3 } ), 1, 2, 3 );

			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.SymmetricExceptWith( new int[] { 1, 2, 3 } ) );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.SymmetricExceptWith( new int[] { 1, 2 } ), 3 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.SymmetricExceptWith( new int[] { 1, 2, 3, 4 } ), 4 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.SymmetricExceptWith( new int[] { 1, 2, 3, 1 } ) );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.SymmetricExceptWith( new int[] { 1, 2, 1 } ), 3 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.SymmetricExceptWith( new int[] { 1, 2, 3, 4, 1 } ), 4 );
			AssertContains( new LinkedHashSet<int>() { 1, 2, 3 }, target => target.SymmetricExceptWith( new int[ 0 ] ), 1, 2, 3 );
		}

		private static void AssertContains<T>( ISet<T> target, Action<ISet<T>> action, params T[] values )
		{
			action( target );
			Assert.That( target.Count, Is.EqualTo( values.Length ) );
			Assert.That( target.SetEquals( values ), Is.True );
		}

		[Test]
		public void TestMoveNode()
		{
			// 2 nodes
			var mock = new MovableLinkedHashSet<string>() { "A", "B" };
			var moving = mock.GetNode( "B" );
			// Tail to Head
			mock.MoveNode( moving, null, mock.Head );
			AssertOrder( mock, "B", "A" );
			// Head to Tail
			mock.MoveNode( moving, mock.Tail, null );
			AssertOrder( mock, "A", "B" );

			// 4 nodes
			mock.Add( "C" );
			mock.Add( "D" );
			AssertOrder( mock, "A", "B", "C", "D" );
			// Mid to Head
			moving = mock.GetNode( "B" );
			mock.MoveNode( moving, null, mock.Head );
			AssertOrder( mock, "B", "A", "C", "D" );
			// Mid to Tail
			moving = mock.GetNode( "C" );
			mock.MoveNode( moving, mock.Tail, null );
			AssertOrder( mock, "B", "A", "D", "C" );
			// Head to Tail
			mock.MoveNode( mock.Head, mock.Tail, null );
			AssertOrder( mock, "A", "D", "C", "B" );
			// Tail to Head
			mock.MoveNode( mock.Tail, null, mock.Head );
			AssertOrder( mock, "B", "A", "D", "C" );
			// Head to Mid
			mock.MoveNode( mock.Head, mock.Head.Next, mock.Head.Next.Next );
			AssertOrder( mock, "A", "B", "D", "C" );
			// Tail to Mid
			mock.MoveNode( mock.Tail, mock.Tail.Previous.Previous, mock.Tail.Previous );
			AssertOrder( mock, "A", "B", "C", "D" );
			// Mid to Mid
			moving = mock.GetNode( "B" );
			mock.MoveNode( moving, moving.Next, moving.Next.Next );
			AssertOrder( mock, "A", "C", "B", "D" );
			// Same
			mock.MoveNode( moving, moving.Previous, moving.Next );
			AssertOrder( mock, "A", "C", "B", "D" );
		}

		[Test]
		public void TestMoveNodeToHead()
		{
			LinkedSetNode<string> current = null;
			var mock =
				new MovableLinkedHashSet<string>(
						( moving, newPrevious, newNext ) =>
						{
							Assert.That( moving, Is.SameAs( current ) );
							Assert.That( newPrevious, Is.Null );
							Assert.That( newNext, Is.SameAs( moving.Set.Head ) );
							return;
						}
					) { "A", "B", "C", "D" };
			current = mock.GetNode( "B" );
			mock.MoveNodeToHead( current );
			AssertOrder( mock, "B", "A", "C", "D" );
			current = mock.GetNode( "D" );
			mock.MoveNodeToHead( current );
			AssertOrder( mock, "D", "B", "A", "C" );
			mock.MoveNodeToHead( mock.Head );
			AssertOrder( mock, "D", "B", "A", "C" );
		}

		[Test]
		public void TestMoveNodeToTail()
		{
			LinkedSetNode<string> current = null;
			var mock =
				new MovableLinkedHashSet<string>(
						( moving, newPrevious, newNext ) =>
						{
							Assert.That( moving, Is.SameAs( current ) );
							Assert.That( newPrevious, Is.SameAs( moving.Set.Tail ) );
							Assert.That( newNext, Is.Null );
							return;
						}
					) { "A", "B", "C", "D" };
			current = mock.GetNode( "B" );
			mock.MoveNodeToTail( current );
			AssertOrder( mock, "A", "C", "D", "B" );
			current = mock.GetNode( "A" );
			mock.MoveNodeToTail( current );
			AssertOrder( mock, "C", "D", "B", "A" );
			mock.MoveNodeToTail( mock.Tail );
			AssertOrder( mock, "C", "D", "B", "A" );
		}

		[Test]
		public void TestMoveNodeToAfter()
		{
			LinkedSetNode<string> current = null;
			LinkedSetNode<string> destination = null;
			var mock =
				new MovableLinkedHashSet<string>(
						( moving, newPrevious, newNext ) =>
						{
							Assert.That( moving, Is.SameAs( current ) );
							Assert.That( newPrevious, Is.Not.Null.And.SameAs( destination ) );
							Assert.That( newNext, Is.SameAs( destination.Next ) );
							return;
						}
					) { "A", "B", "C", "D" };
			current = mock.GetNode( "B" );

			destination = mock.GetNode( "C" );
			mock.MoveNodeToAfter( destination, current );
			AssertOrder( mock, "A", "C", "B", "D" );
			destination = mock.GetNode( "A" );
			mock.MoveNodeToAfter( destination, current );
			AssertOrder( mock, "A", "B", "C", "D" );
			destination = mock.GetNode( "D" );
			mock.MoveNodeToAfter( destination, current );
			AssertOrder( mock, "A", "C", "D", "B" );
			mock.MoveNodeToAfter( current.Previous, current );
			AssertOrder( mock, "A", "C", "D", "B" );
		}

		[Test]
		public void TestMoveNodeToBefore()
		{
			LinkedSetNode<string> current = null;
			LinkedSetNode<string> destination = null;
			var mock =
				new MovableLinkedHashSet<string>(
						( moving, newPrevious, newNext ) =>
						{
							Assert.That( moving, Is.SameAs( current ) );
							Assert.That( newNext, Is.Not.Null.And.SameAs( destination ) );
							Assert.That( newPrevious, Is.SameAs( destination.Previous ) );
							return;
						}
					) { "A", "B", "C", "D" };
			current = mock.GetNode( "C" );

			destination = mock.GetNode( "B" );
			mock.MoveNodeToBefore( destination, current );
			AssertOrder( mock, "A", "C", "B", "D" );
			destination = mock.GetNode( "D" );
			mock.MoveNodeToBefore( destination, current );
			AssertOrder( mock, "A", "B", "C", "D" );
			destination = mock.GetNode( "A" );
			mock.MoveNodeToBefore( destination, current );
			AssertOrder( mock, "C", "A", "B", "D" );
			mock.MoveNodeToBefore( current.Next, current );
			AssertOrder( mock, "C", "A", "B", "D" );
		}
		
		private static void AssertOrder<T>( LinkedHashSet<T> target, params T[] expected )
		{
			Assert.That( target.Count, Is.EqualTo( expected.Length ) );
			Assert.That( target.ToArray(), Is.EqualTo( expected ) );
			Assert.That( target.Head.Value, Is.EqualTo( expected.First() ) );
			Assert.That( target.Head.Previous, Is.Null );
			Assert.That( target.Tail.Value, Is.EqualTo( expected.Last() ) );
			Assert.That( target.Tail.Next, Is.Null );

			var nodes = new List<LinkedSetNode<T>>( expected.Length );
			for ( var node = target.Head; node != null; node = node.Next )
			{
				nodes.Add( node );
			}

			Assert.That( nodes.Count, Is.EqualTo( expected.Length ) );
			for ( int i = 0; i < nodes.Count; i++ )
			{
				Assert.That( nodes[ i ].Set, Is.SameAs( target ), "[{0}]", i );
				Assert.That( nodes[ i ].Value, Is.EqualTo( expected[ i ] ), "[{0}]", i );

				if ( 0 < i )
				{
					Assert.That( nodes[ i ].Previous, Is.SameAs( nodes[ i - 1 ] ), "[{0}]", i );
				}
				else
				{
					Assert.That( nodes[ i ].Previous, Is.Null, "[{0}]", i );
					Assert.That( nodes[ i ], Is.SameAs( target.Head ), "[{0}]", i );
				}

				if ( i < nodes.Count - 1 )
				{
					Assert.That( nodes[ i ].Next, Is.SameAs( nodes[ i + 1 ] ), "[{0}]", i );
				}
				else
				{
					Assert.That( nodes[ i ].Next, Is.Null, "[{0}]", i );
					Assert.That( nodes[ i ], Is.SameAs( target.Tail ), "[{0}]", i );
				}
			}
		}

		private sealed class MovableLinkedHashSet<T> : MovingAssertableLinkedHashSet<T>
		{
			public MovableLinkedHashSet() : base( null ) { }

			public MovableLinkedHashSet( Action<LinkedSetNode<T>, LinkedSetNode<T>, LinkedSetNode<T>> assertion ) : base( assertion ) { }

			public LinkedSetNode<T> GetNode( T item )
			{
				return base.GetNodeDirect( item );
			}

			public new void MoveNode( LinkedSetNode<T> moving, LinkedSetNode<T> newPrevious, LinkedSetNode<T> newNext )
			{
				base.MoveNode( moving, newPrevious, newNext );
			}

			public new void MoveNodeToHead( LinkedSetNode<T> moving )
			{
				base.MoveNodeToHead( moving );
			}

			public new void MoveNodeToTail( LinkedSetNode<T> moving )
			{
				base.MoveNodeToTail( moving );
			}

			public new void MoveNodeToAfter( LinkedSetNode<T> destination, LinkedSetNode<T> moving )
			{
				base.MoveNodeToAfter( destination, moving );
			}

			public new void MoveNodeToBefore( LinkedSetNode<T> destination, LinkedSetNode<T> moving )
			{
				base.MoveNodeToBefore( destination, moving );
			}
		}

		private class MovingAssertableLinkedHashSet<T> : LinkedHashSet<T>
		{
			private readonly Action<LinkedSetNode<T>, LinkedSetNode<T>, LinkedSetNode<T>> _assertion;

			public MovingAssertableLinkedHashSet( Action<LinkedSetNode<T>, LinkedSetNode<T>, LinkedSetNode<T>> assertion )
			{
				this._assertion = assertion ?? ( ( _0, _1, _2 ) => { return; } );
			}

			protected override void MoveNode( LinkedSetNode<T> moving, LinkedSetNode<T> newPrevious, LinkedSetNode<T> newNext )
			{
				this._assertion( moving, newPrevious, newNext );
				base.MoveNode( moving, newPrevious, newNext );
			}
		}
	}
}
