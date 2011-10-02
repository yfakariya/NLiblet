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
using System.Collections;

namespace NLiblet.Collections
{
	[TestFixture]
	[Timeout( 1000 )]
	public class LinkedDictionaryTest
	{
		[Test]
		public void TestAddForEach()
		{
			var target = new LinkedDictionary<string, int>();

			// empty
			Assert.That( target.Any(), Is.False );

			var first = target.Add( "A", 1 );
			Assert.That( first.Dictionary, Is.SameAs( target ) );
			Assert.That( first.IsHead, Is.True );
			Assert.That( first.IsTail, Is.True );
			Assert.That( first.Key, Is.EqualTo( "A" ) );
			Assert.That( first.Next, Is.Null );
			Assert.That( first.Previous, Is.Null );
			Assert.That( first.Value, Is.EqualTo( 1 ) );
			Assert.That( target.Count, Is.EqualTo( 1 ) );

			var second = target.Add( "B", 2 );
			Assert.That( first.Dictionary, Is.SameAs( target ) );
			Assert.That( first.IsHead, Is.True );
			Assert.That( first.IsTail, Is.False );
			Assert.That( first.Key, Is.EqualTo( "A" ) );
			Assert.That( first.Next, Is.SameAs( second ) );
			Assert.That( first.Previous, Is.Null );
			Assert.That( first.Value, Is.EqualTo( 1 ) );
			Assert.That( second.Dictionary, Is.SameAs( target ) );
			Assert.That( second.IsHead, Is.False );
			Assert.That( second.IsTail, Is.True );
			Assert.That( second.Key, Is.EqualTo( "B" ) );
			Assert.That( second.Next, Is.Null );
			Assert.That( second.Previous, Is.SameAs( first ) );
			Assert.That( second.Value, Is.EqualTo( 2 ) );
			Assert.That( target.Count, Is.EqualTo( 2 ) );

			var iterated = new List<KeyValuePair<string, int>>();
			foreach ( var item in target )
			{
				iterated.Add( item );
			}

			Assert.That( iterated[ 0 ].Key, Is.EqualTo( first.Key ) );
			Assert.That( iterated[ 0 ].Value, Is.EqualTo( first.Value ) );
			Assert.That( iterated[ 1 ].Key, Is.EqualTo( second.Key ) );
			Assert.That( iterated[ 1 ].Value, Is.EqualTo( second.Value ) );
		}

		[Test]
		public void TestAdd_Duplicate()
		{
			var target = new LinkedDictionary<string, int>();
			var first = target.Add( "A", 1 );
			Assert.That( first, Is.Not.Null );
			var second = target.Add( "A", 2 );
			Assert.That( second, Is.Null );
		}

		[Test]
		public void TestIndexer()
		{
			var target = new LinkedDictionary<string, int>();
			target[ "A" ] = 1;
			Assert.That( target.Count, Is.EqualTo( 1 ) );
			target[ "B" ] = 2;
			Assert.That( target.Count, Is.EqualTo( 2 ) );

			var iterated = new List<KeyValuePair<string, int>>();
			foreach ( var item in target )
			{
				iterated.Add( item );
			}

			Assert.That( iterated[ 0 ].Key, Is.EqualTo( "A" ) );
			Assert.That( iterated[ 0 ].Value, Is.EqualTo( 1 ) );
			Assert.That( iterated[ 1 ].Key, Is.EqualTo( "B" ) );
			Assert.That( iterated[ 1 ].Value, Is.EqualTo( 2 ) );
			Assert.That( target[ "A" ], Is.EqualTo( 1 ) );
			Assert.That( target[ "B" ], Is.EqualTo( 2 ) );
		}

		[Test]
		public void TestIndexer_Duplicate()
		{
			var target = new LinkedDictionary<string, int>();
			var node = target.Add( "A", 1 );
			Assert.That( target.Count, Is.EqualTo( 1 ) );
			target[ "A" ] = 2;
			Assert.That( target.Count, Is.EqualTo( 1 ) );
			Assert.That( node.Value, Is.EqualTo( 2 ) );

			var iterated = new List<KeyValuePair<string, int>>();
			foreach ( var item in target )
			{
				iterated.Add( item );
			}

			Assert.That( iterated[ 0 ].Key, Is.EqualTo( "A" ) );
			Assert.That( iterated[ 0 ].Value, Is.EqualTo( 2 ) );
			Assert.That( target[ "A" ], Is.EqualTo( 2 ) );
		}

		[Test]
		public void TestAddRemove()
		{
			var target = new LinkedDictionary<string, int>();
			var removed = target.Remove( "A" );
			Assert.That( removed, Is.Null );
			var node0 = target.Add( "A", 1 );
			Assert.That( target.ContainsKey( "A" ), Is.True );
			Assert.That( target.ContainsValue( 1 ), Is.True );
			Assert.That( target.Head, Is.SameAs( node0 ) );
			Assert.That( target.Tail, Is.SameAs( node0 ) );
			var node1 = target.Add( "B", 2 );
			Assert.That( target.ContainsKey( "B" ), Is.True );
			Assert.That( target.ContainsValue( 2 ), Is.True );
			Assert.That( target.Head, Is.SameAs( node0 ) );
			Assert.That( target.Tail, Is.SameAs( node1 ) );
			var node2 = target.Add( "C", 3 );
			Assert.That( target.ContainsKey( "C" ), Is.True );
			Assert.That( target.ContainsValue( 3 ), Is.True );
			Assert.That( target.Head, Is.SameAs( node0 ) );
			Assert.That( target.Tail, Is.SameAs( node2 ) );
			var node3 = target.Add( "D", 4 );
			Assert.That( target.ContainsKey( "D" ), Is.True );
			Assert.That( target.ContainsValue( 4 ), Is.True );
			Assert.That( target.Head, Is.SameAs( node0 ) );
			Assert.That( target.Tail, Is.SameAs( node3 ) );

			// Intermediate
			removed = target.Remove( "B" );
			Assert.That( target.ContainsKey( "B" ), Is.False );
			Assert.That( target.ContainsValue( 2 ), Is.False );
			Assert.That( target.Head, Is.SameAs( node0 ) );
			Assert.That( target.Tail, Is.SameAs( node3 ) );
			Assert.That( removed, Is.Not.Null );
			VerifyRemovedNode( removed, node1, "B", 2 );
			Assert.That( node0.Next, Is.SameAs( node2 ) );
			Assert.That( node2.Previous, Is.SameAs( node0 ) );
			Assert.That( target.Count, Is.EqualTo( 3 ) );
			Assert.That( target.Any(), Is.True );

			// Head
			removed = target.Remove( "A" );
			Assert.That( target.ContainsKey( "A" ), Is.False );
			Assert.That( target.ContainsValue( 1 ), Is.False );
			Assert.That( target.Head, Is.SameAs( node2 ) );
			Assert.That( target.Tail, Is.SameAs( node3 ) );
			Assert.That( removed, Is.Not.Null );
			VerifyRemovedNode( removed, node0, "A", 1 );
			Assert.That( node2.Previous, Is.Null );
			Assert.That( target.Count, Is.EqualTo( 2 ) );
			Assert.That( target.Any(), Is.True );

			// Tail
			removed = target.Remove( "D" );
			Assert.That( target.ContainsKey( "D" ), Is.False );
			Assert.That( target.ContainsValue( 4 ), Is.False );
			Assert.That( target.Head, Is.SameAs( node2 ) );
			Assert.That( target.Tail, Is.SameAs( node2 ) );
			Assert.That( removed, Is.Not.Null );
			VerifyRemovedNode( removed, node3, "D", 4 );
			Assert.That( node2.Previous, Is.Null );
			Assert.That( node2.Next, Is.Null );
			Assert.That( target.Count, Is.EqualTo( 1 ) );
			Assert.That( target.Any(), Is.True );

			// To be empty
			Assert.That( target.Remove( "A" ), Is.Null );
			Assert.That( target.Remove( "D" ), Is.Null );
			removed = target.Remove( "C" );
			Assert.That( target.ContainsKey( "C" ), Is.False );
			Assert.That( target.ContainsValue( 3 ), Is.False );
			Assert.That( target.Head, Is.Null );
			Assert.That( target.Tail, Is.Null );
			Assert.That( removed, Is.Not.Null );
			VerifyRemovedNode( removed, node2, "C", 3 );
			Assert.That( target.Count, Is.EqualTo( 0 ) );
			Assert.That( target.Any(), Is.False );
		}

		private static void VerifyRemovedNode<TKey, TValue>( LinkedDictionaryNode<TKey, TValue> removed, LinkedDictionaryNode<TKey, TValue> expectedNode, TKey expectedKey, TValue expectedValue )
		{
			Assert.That( removed, Is.SameAs( expectedNode ) );
			Assert.That( removed.Dictionary, Is.Null );
			Assert.That( removed.Next, Is.Null );
			Assert.That( removed.Previous, Is.Null );
			Assert.That( removed.IsHead, Is.True );
			Assert.That( removed.IsTail, Is.True );
			Assert.That( removed.Key, Is.EqualTo( expectedKey ) );
			Assert.That( removed.Value, Is.EqualTo( expectedValue ) );
		}

		[Test]
		public void TestRemove_Node()
		{
			var target = new LinkedDictionary<string, int>();
			var node0 = target.Add( "A", 1 );
			var node1 = target.Add( "B", 2 );
			var node2 = target.Add( "C", 3 );
			var node3 = target.Add( "D", 4 );

			// Intermediate
			target.Remove( node1 );
			Assert.That( target.ContainsKey( node1.Key ), Is.False );
			Assert.That( target.ContainsValue( node1.Value ), Is.False );
			Assert.That( target.Head, Is.SameAs( node0 ) );
			Assert.That( target.Tail, Is.SameAs( node3 ) );
			VerifyRemovedNode( node1, node1, "B", 2 );
			Assert.That( node0.Next, Is.SameAs( node2 ) );
			Assert.That( node2.Previous, Is.SameAs( node0 ) );
			Assert.That( target.Count, Is.EqualTo( 3 ) );
			Assert.That( target.Any(), Is.True );

			// Head
			target.Remove( node0 );
			Assert.That( target.ContainsKey( node0.Key ), Is.False );
			Assert.That( target.ContainsValue( node0.Value ), Is.False );
			Assert.That( target.Head, Is.SameAs( node2 ) );
			Assert.That( target.Tail, Is.SameAs( node3 ) );
			VerifyRemovedNode( node0, node0, "A", 1 );
			Assert.That( node2.Previous, Is.Null );
			Assert.That( target.Count, Is.EqualTo( 2 ) );
			Assert.That( target.Any(), Is.True );

			// Tail
			target.Remove( node3 );
			Assert.That( target.ContainsKey( node3.Key ), Is.False );
			Assert.That( target.ContainsValue( node3.Value ), Is.False );
			Assert.That( target.Head, Is.SameAs( node2 ) );
			Assert.That( target.Tail, Is.SameAs( node2 ) );
			VerifyRemovedNode( node3, node3, "D", 4 );
			Assert.That( node2.Previous, Is.Null );
			Assert.That( node2.Next, Is.Null );
			Assert.That( target.Count, Is.EqualTo( 1 ) );
			Assert.That( target.Any(), Is.True );

			// To be empty
			target.Remove( node2 );
			Assert.That( target.ContainsKey( node2.Key ), Is.False );
			Assert.That( target.ContainsValue( node2.Value ), Is.False );
			Assert.That( target.Head, Is.Null );
			Assert.That( target.Tail, Is.Null );
			VerifyRemovedNode( node2, node2, "C", 3 );
			Assert.That( target.Count, Is.EqualTo( 0 ) );
			Assert.That( target.Any(), Is.False );
		}

		[Test]
		public void TestClear()
		{
			var target = new LinkedDictionary<string, int>();
			var nodes = new List<LinkedDictionaryNode<string, int>>();
			nodes.Add( target.Add( "A", 1 ) );
			nodes.Add( target.Add( "B", 2 ) );
			nodes.Add( target.Add( "C", 3 ) );
			nodes.Add( target.Add( "D", 4 ) );

			target.Clear();
			Assert.That( target.Count, Is.EqualTo( 0 ) );
			Assert.That( target.Any(), Is.False );
			Assert.That( target.Head, Is.Null );
			Assert.That( target.Tail, Is.Null );
			foreach ( var node in nodes )
			{
				Assert.That( target.ContainsKey( node.Key ), Is.False );
				Assert.That( target.ContainsValue( node.Value ), Is.False );
				Assert.That( node.Dictionary, Is.Null );
				Assert.That( node.IsHead, Is.True );
				Assert.That( node.IsTail, Is.True );
				Assert.That( node.Previous, Is.Null );
				Assert.That( node.Next, Is.Null );
			}
		}

		[Test]
		public void TestCopyTo()
		{
			var target = new LinkedDictionary<string, int>();
			var array = new KeyValuePair<string, int>[ 5 ];

			// empty
			target.CopyTo( array );
			VerifyArray( target, array, 0, 0 );
			target.CopyTo( array, 1 );
			VerifyArray( target, array, 0, 0 );
			target.CopyTo( 1, array, 1, 2 );
			VerifyArray( target, array, 0, 0 );

			target.Add( "A", 1 );
			target.Add( "B", 2 );
			target.Add( "C", 3 );

			target.CopyTo( array );
			VerifyArray( target, array, 0, 3 );
			Array.Clear( array, 0, array.Length );

			target.CopyTo( array, 1 );
			VerifyArray( target, array, 1, 3 );
			Array.Clear( array, 0, array.Length );

			target.CopyTo( 1, array, 1, 2 );
			Assert.That( array[ 0 ], Is.EqualTo( default( KeyValuePair<string, int> ) ) );
			Assert.That( array[ 1 ], Is.EqualTo( new KeyValuePair<string, int>( "B", 2 ) ) );
			Assert.That( array[ 2 ], Is.EqualTo( new KeyValuePair<string, int>( "C", 3 ) ) );
			Assert.That( array[ 3 ], Is.EqualTo( default( KeyValuePair<string, int> ) ) );
			Assert.That( array[ 4 ], Is.EqualTo( default( KeyValuePair<string, int> ) ) );
		}

		private static void VerifyArray<TKey, TValue>( LinkedDictionary<TKey, TValue> target, KeyValuePair<TKey, TValue>[] array, int startAt, int count )
		{
			var enumerator = target.GetEnumerator();
			try
			{
				for ( int i = 0; i < array.Length; i++ )
				{
					if ( i < startAt || startAt + count <= i )
					{
						Assert.That( array[ i ], Is.EqualTo( default( KeyValuePair<TKey, TValue> ) ) );
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
			var target = new LinkedDictionary<string, int>();

			var first = target.Add( "A", 1 );
			var second = target.Add( "B", 2 );

			var iterated = new List<KeyValuePair<string, int>>();
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

			Assert.That( iterated[ 1 ].Key, Is.EqualTo( first.Key ) );
			Assert.That( iterated[ 1 ].Value, Is.EqualTo( first.Value ) );
			Assert.That( iterated[ 0 ].Key, Is.EqualTo( second.Key ) );
			Assert.That( iterated[ 0 ].Value, Is.EqualTo( second.Value ) );
		}

		[Test]
		public void TestReverse()
		{
			var target = new LinkedDictionary<string, int>();

			// empty
			Assert.That( target.Reverse().Any(), Is.False );

			var first = target.Add( "A", 1 );
			var second = target.Add( "B", 2 );

			var iterated = new List<KeyValuePair<string, int>>();
			foreach ( var item in target.Reverse() )
			{
				iterated.Add( item );
			}

			Assert.That( iterated[ 1 ].Key, Is.EqualTo( first.Key ) );
			Assert.That( iterated[ 1 ].Value, Is.EqualTo( first.Value ) );
			Assert.That( iterated[ 0 ].Key, Is.EqualTo( second.Key ) );
			Assert.That( iterated[ 0 ].Value, Is.EqualTo( second.Value ) );
		}

		[Test]
		public void TestFirst()
		{
			var target = new LinkedDictionary<int, int>();
			// empty
			VerifyFirstLast( target.First, default( KeyValuePair<int, int> ), false, false );

			// 1
			target.Add( 1, 1 );
			VerifyFirstLast( target.First, new KeyValuePair<int, int>( 1, 1 ), true, false );

			// 2
			target.Add( 2, 2 );
			// true first
			VerifyFirstLast( () => target.First( item => item.Key == 1 ), new KeyValuePair<int, int>( 1, 1 ), true, false );
			// true second
			VerifyFirstLast( () => target.First( item => item.Key == 2 ), new KeyValuePair<int, int>( 2, 2 ), true, false );
			// true both
			VerifyFirstLast( () => target.First( item => 0 < item.Key ), new KeyValuePair<int, int>( 1, 1 ), true, false );
			// false
			VerifyFirstLast( () => target.First( item => item.Key < 0 ), new KeyValuePair<int, int>( 0, 0 ), false, false );

			// Default
			target.Clear();
			// empty
			VerifyFirstLast( target.FirstOrDefault, default( KeyValuePair<int, int> ), false, true );

			// 1
			target.Add( 1, 1 );
			VerifyFirstLast( target.FirstOrDefault, new KeyValuePair<int, int>( 1, 1 ), true, true );

			// 2
			target.Add( 2, 2 );
			// true first
			VerifyFirstLast( () => target.FirstOrDefault( item => item.Key == 1 ), new KeyValuePair<int, int>( 1, 1 ), true, true );
			// true second
			VerifyFirstLast( () => target.FirstOrDefault( item => item.Key == 2 ), new KeyValuePair<int, int>( 2, 2 ), true, true );
			// true both
			VerifyFirstLast( () => target.FirstOrDefault( item => 0 < item.Key ), new KeyValuePair<int, int>( 1, 1 ), true, true );
			// false
			VerifyFirstLast( () => target.FirstOrDefault( item => item.Key < 0 ), new KeyValuePair<int, int>( 0, 0 ), false, true );
		}

		[Test]
		public void TestLast()
		{
			var target = new LinkedDictionary<int, int>();
			// empty
			VerifyFirstLast( target.Last, default( KeyValuePair<int, int> ), false, false );

			// 1
			target.Add( 1, 1 );
			VerifyFirstLast( target.Last, new KeyValuePair<int, int>( 1, 1 ), true, false );

			// 2
			target.Add( 2, 2 );
			// true first
			VerifyFirstLast( () => target.Last( item => item.Key == 1 ), new KeyValuePair<int, int>( 1, 1 ), true, false );
			// true second
			VerifyFirstLast( () => target.Last( item => item.Key == 2 ), new KeyValuePair<int, int>( 2, 2 ), true, false );
			// true both
			VerifyFirstLast( () => target.Last( item => 0 < item.Key ), new KeyValuePair<int, int>( 2, 2 ), true, false );
			// false
			VerifyFirstLast( () => target.Last( item => item.Key < 0 ), new KeyValuePair<int, int>( 0, 0 ), false, false );

			// Default
			target.Clear();
			// empty
			VerifyFirstLast( target.LastOrDefault, default( KeyValuePair<int, int> ), false, true );

			// 1
			target.Add( 1, 1 );
			VerifyFirstLast( target.LastOrDefault, new KeyValuePair<int, int>( 1, 1 ), true, true );

			// 2
			target.Add( 2, 2 );
			// true first
			VerifyFirstLast( () => target.LastOrDefault( item => item.Key == 1 ), new KeyValuePair<int, int>( 1, 1 ), true, true );
			// true second
			VerifyFirstLast( () => target.LastOrDefault( item => item.Key == 2 ), new KeyValuePair<int, int>( 2, 2 ), true, true );
			// true both
			VerifyFirstLast( () => target.LastOrDefault( item => 0 < item.Key ), new KeyValuePair<int, int>( 2, 2 ), true, true );
			// false
			VerifyFirstLast( () => target.LastOrDefault( item => item.Key < 0 ), new KeyValuePair<int, int>( 0, 0 ), false, true );
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

		[Test]
		public void TestGetNode()
		{
			var target = new LinkedDictionary<string, int>();
			Assert.That( target.GetNode( "A" ), Is.Null );
			var added = target.Add( "A", 1 );
			Assert.That( added, Is.Not.Null );
			Assert.That( target.GetNode( "A" ), Is.SameAs( added ) );
		}


		#region -- Ported from Mono's Dictionary`2 unit tests --

		[Test]
		public void RemoveTest()
		{
			// Tests explicit interface implementation.
			IDictionary<string, string> _dictionary = new LinkedDictionary<string, string>();
			_dictionary.Add( "key1", "value1" );
			_dictionary.Add( "key2", "value2" );
			_dictionary.Add( "key3", "value3" );
			_dictionary.Add( "key4", "value4" );
			Assert.IsTrue( _dictionary.Remove( "key3" ) );
			Assert.IsFalse( _dictionary.Remove( "foo" ) );
			Assert.AreEqual( 3, _dictionary.Count );
			Assert.IsFalse( _dictionary.ContainsKey( "key3" ) );
		}

		[Test] // bug 432441
		public void Clear_Iterators()
		{
			var d = new LinkedDictionary<object, object>();

			d[ new object() ] = new object();
			d.Clear();
			int hash = 0;
			foreach ( object o in d )
			{
				hash += o.GetHashCode();
			}
		}

		[Test]
		public void TryGetValueTest()
		{
			// Tests explicit interface implementation.
			IDictionary<string, object> _dictionary = new LinkedDictionary<string, object>();
			_dictionary.Add( "key1", "value1" );
			_dictionary.Add( "key2", "value2" );
			_dictionary.Add( "key3", "value3" );
			_dictionary.Add( "key4", "value4" );
			object value = "";
			bool retrieved = _dictionary.TryGetValue( "key4", out value );
			Assert.IsTrue( retrieved );
			Assert.AreEqual( "value4", ( string )value, "TryGetValue does not return value!" );

			retrieved = _dictionary.TryGetValue( "key7", out value );
			Assert.IsFalse( retrieved );
			Assert.IsNull( value, "value for non existant value should be null!" );
		}

		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void IDictionaryAddTest()
		{
			IDictionary iDict = new LinkedDictionary<string, object>();
			iDict.Add( "key1", "value1" );
			iDict.Add( "key2", "value3" );
			Assert.AreEqual( 2, iDict.Count, "IDictioanry interface add is not working!" );

			//Negative test case
			iDict.Add( 12, "value" );
			iDict.Add( "key", 34 );
		}

		[Test]
		public void IEnumeratorTest()
		{
			var _dictionary = new LinkedDictionary<string, object>();
			_dictionary.Add( "key1", "value1" );
			_dictionary.Add( "key2", "value2" );
			_dictionary.Add( "key3", "value3" );
			_dictionary.Add( "key4", "value4" );
			IEnumerator itr = ( ( IEnumerable )_dictionary ).GetEnumerator();
			while ( itr.MoveNext() )
			{
				object o = itr.Current;
				Assert.AreEqual( typeof( KeyValuePair<string, object> ), o.GetType(), "Current should return a type of KeyValuePair" );
				KeyValuePair<string, object> entry = ( KeyValuePair<string, object> )itr.Current;
			}
			Assert.AreEqual( "value4", _dictionary[ "key4" ].ToString(), "" );
		}


		[Test]
		public void IEnumeratorGenericTest()
		{
			var _dictionary = new LinkedDictionary<string, object>();
			_dictionary.Add( "key1", "value1" );
			_dictionary.Add( "key2", "value2" );
			_dictionary.Add( "key3", "value3" );
			_dictionary.Add( "key4", "value4" );
			IEnumerator<KeyValuePair<string, object>> itr = ( ( IEnumerable<KeyValuePair<string, object>> )_dictionary ).GetEnumerator();
			while ( itr.MoveNext() )
			{
				object o = itr.Current;
				Assert.AreEqual( typeof( KeyValuePair<string, object> ), o.GetType(), "Current should return a type of KeyValuePair<object,string>" );
				KeyValuePair<string, object> entry = ( KeyValuePair<string, object> )itr.Current;
			}
			Assert.AreEqual( "value4", _dictionary[ "key4" ].ToString(), "" );
		}

		[Test]
		public void IDictionaryEnumeratorTest()
		{
			var _dictionary = new LinkedDictionary<string, object>();
			_dictionary.Add( "key1", "value1" );
			_dictionary.Add( "key2", "value2" );
			_dictionary.Add( "key3", "value3" );
			_dictionary.Add( "key4", "value4" );
			IDictionaryEnumerator itr = ( ( IDictionary )_dictionary ).GetEnumerator();
			while ( itr.MoveNext() )
			{
				object o = itr.Current;
				Assert.AreEqual( typeof( DictionaryEntry ), o.GetType(), "Current should return a type of DictionaryEntry" );
				DictionaryEntry entry = ( DictionaryEntry )itr.Current;
			}
			Assert.AreEqual( "value4", _dictionary[ "key4" ].ToString(), "" );
		}

		[Test]
		public void ForEachTest()
		{
			var _dictionary = new LinkedDictionary<string, object>();
			_dictionary.Add( "key1", "value1" );
			_dictionary.Add( "key2", "value2" );
			_dictionary.Add( "key3", "value3" );
			_dictionary.Add( "key4", "value4" );

			int i = 0;
			foreach ( KeyValuePair<string, object> entry in _dictionary )
				i++;
			Assert.AreEqual( 4, i, "fail1: foreach entry failed!" );

			i = 0;
			foreach ( KeyValuePair<string, object> entry in ( ( IEnumerable )_dictionary ) )
				i++;
			Assert.AreEqual( 4, i, "fail2: foreach entry failed!" );

			i = 0;
			foreach ( DictionaryEntry entry in ( ( IDictionary )_dictionary ) )
				i++;
			Assert.AreEqual( 4, i, "fail3: foreach entry failed!" );
		}

		[Test] // bug 75073
		public void SliceCollectionsEnumeratorTest()
		{
			var values = new LinkedDictionary<string, int>();

			IEnumerator<string> ke = values.Keys.GetEnumerator();
			IEnumerator<int> ve = values.Values.GetEnumerator();

			Assert.IsTrue( ke is LinkedDictionary<string, int>.KeySet.Enumerator );
			Assert.IsTrue( ve is LinkedDictionary<string, int>.ValueCollection.Enumerator );
		}

		[Test]
		public void PlainEnumeratorReturnTest()
		{
			var _dictionary = new LinkedDictionary<string, object>();
			// Test that we return a KeyValuePair even for non-generic dictionary iteration
			_dictionary[ "foo" ] = "bar";
			IEnumerator<KeyValuePair<string, object>> enumerator = _dictionary.GetEnumerator();
			Assert.IsTrue( enumerator.MoveNext(), "#1" );
			Assert.AreEqual( typeof( KeyValuePair<string, object> ), ( ( IEnumerator )enumerator ).Current.GetType(), "#2" );
			Assert.AreEqual( typeof( DictionaryEntry ), ( ( IDictionaryEnumerator )enumerator ).Entry.GetType(), "#3" );
			Assert.AreEqual( typeof( KeyValuePair<string, object> ), ( ( IDictionaryEnumerator )enumerator ).Current.GetType(), "#4" );
			Assert.AreEqual( typeof( KeyValuePair<string, object> ), ( ( object )enumerator.Current ).GetType(), "#5" );
		}

		[Test, ExpectedException( typeof( InvalidOperationException ) )]
		public void FailFastTest1()
		{
			var d = new LinkedDictionary<int, int>();
			d[ 1 ] = 1;
			int count = 0;
			foreach ( KeyValuePair<int, int> kv in d )
			{
				d[ kv.Key + 1 ] = kv.Value + 1;
				if ( count++ != 0 )
					Assert.Fail( "Should not be reached" );
			}
			Assert.Fail( "Should not be reached" );
		}

		[Test, ExpectedException( typeof( InvalidOperationException ) )]
		public void FailFastTest2()
		{
			var d = new LinkedDictionary<int, int>();
			d[ 1 ] = 1;
			int count = 0;
			foreach ( int i in d.Keys )
			{
				d[ i + 1 ] = i + 1;
				if ( count++ != 0 )
					Assert.Fail( "Should not be reached" );
			}
			Assert.Fail( "Should not be reached" );
		}

		[Test, ExpectedException( typeof( InvalidOperationException ) )]
		public void FailFastTest3()
		{
			var d = new LinkedDictionary<int, int>();
			d[ 1 ] = 1;
			int count = 0;
			foreach ( int i in d.Keys )
			{
				d[ i ] = i;
				if ( count++ != 0 )
					Assert.Fail( "Should not be reached" );
			}
			Assert.Fail( "Should not be reached" );
		}

		[Test]
		public void Empty_KeysValues_CopyTo()
		{
			var d = new LinkedDictionary<int, int>();
			int[] array = new int[ 1 ];
			d.Keys.CopyTo( array, array.Length );
			d.Values.CopyTo( array, array.Length );
		}

		[Test]
		public void IDictionary_Contains()
		{
			IDictionary d = new LinkedDictionary<int, int>();
			d.Add( 1, 2 );
			Assert.IsTrue( d.Contains( 1 ) );
			Assert.IsFalse( d.Contains( 2 ) );
			Assert.IsFalse( d.Contains( "x" ) );
		}

		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void IDictionary_Add2()
		{
			IDictionary d = new LinkedDictionary<int, int>();
			d.Add( "bar", 1 );
		}

		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void IDictionary_Add3()
		{
			IDictionary d = new LinkedDictionary<int, int>();
			d.Add( 1, "bar" );
		}

		[Test]
		public void IDictionary_Add_Null()
		{
			IDictionary d = new LinkedDictionary<int, string>();
			d.Add( 1, null );
			d[ 2 ] = null;

			Assert.IsNull( d[ 1 ] );
			Assert.IsNull( d[ 2 ] );
		}

		[Test]
		[ExpectedException( typeof( ArgumentException ) )]
		public void IDictionary_Add_Null_2()
		{
			IDictionary d = new LinkedDictionary<int, int>();
			d.Add( 1, null );
		}

		[Test]
		public void IDictionary_Remove1()
		{
			IDictionary d = new LinkedDictionary<int, int>();
			d.Add( 1, 2 );
			d.Remove( 1 );
			d.Remove( 5 );
			d.Remove( "foo" );
		}

		[Test]
		public void IDictionary_IndexerGetNonExistingTest()
		{
			IDictionary d = new LinkedDictionary<int, int>();
			d.Add( 1, 2 );
			Assert.IsNull( d[ 2 ] );
			Assert.IsNull( d[ "foo" ] );
		}

		[Test] // bug #332534
		public void Dictionary_MoveNext()
		{
			var a = new LinkedDictionary<int, int>();
			a.Add( 3, 1 );
			a.Add( 4, 1 );

			IEnumerator en = a.GetEnumerator();
			for ( int i = 1; i < 10; i++ )
				en.MoveNext();
		}

		[Test]
		public void KeyObjectMustNotGetChangedIfKeyAlreadyExists()
		{
			var d = new LinkedDictionary<string, int>();
			string s1 = "Test";
			string s2 = "Tes" + "T".ToLowerInvariant();
			d[ s1 ] = 1;
			d[ s2 ] = 2;
			string comp = String.Empty;
			foreach ( String s in d.Keys )
				comp = s;
			Assert.IsTrue( Object.ReferenceEquals( s1, comp ) );
		}

		[Test]
		public void ResetKeysEnumerator()
		{
			var test = new LinkedDictionary<string, string>();
			test.Add( "monkey", "singe" );
			test.Add( "singe", "mono" );
			test.Add( "mono", "monkey" );

			IEnumerator enumerator = test.Keys.GetEnumerator();

			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );

			enumerator.Reset();

			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsFalse( enumerator.MoveNext() );
		}

		[Test]
		public void ResetValuesEnumerator()
		{
			var test = new LinkedDictionary<string, string>();
			test.Add( "monkey", "singe" );
			test.Add( "singe", "mono" );
			test.Add( "mono", "monkey" );

			IEnumerator enumerator = test.Values.GetEnumerator();

			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );

			enumerator.Reset();

			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsTrue( enumerator.MoveNext() );
			Assert.IsFalse( enumerator.MoveNext() );
		}

		[Test]
		public void ResetShimEnumerator()
		{
			IDictionary test = new LinkedDictionary<string, string>();
			test.Add( "monkey", "singe" );
			test.Add( "singe", "mono" );
			test.Add( "mono", "monkey" );

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
		public void ICollectionOfKeyValuePairContains()
		{
			var dictionary = new LinkedDictionary<string, int>();
			dictionary.Add( "foo", 42 );
			dictionary.Add( "bar", 12 );

			var collection = dictionary as ICollection<KeyValuePair<string, int>>;

			Assert.AreEqual( 2, collection.Count );

			Assert.IsFalse( collection.Contains( new KeyValuePair<string, int>( "baz", 13 ) ) );
			Assert.IsFalse( collection.Contains( new KeyValuePair<string, int>( "foo", 13 ) ) );
			Assert.IsTrue( collection.Contains( new KeyValuePair<string, int>( "foo", 42 ) ) );
		}

		[Test]
		public void ICollectionOfKeyValuePairRemove()
		{
			var dictionary = new LinkedDictionary<string, int>();
			dictionary.Add( "foo", 42 );
			dictionary.Add( "bar", 12 );

			var collection = dictionary as ICollection<KeyValuePair<string, int>>;

			Assert.AreEqual( 2, collection.Count );

			Assert.IsFalse( collection.Remove( new KeyValuePair<string, int>( "baz", 13 ) ) );
			Assert.IsFalse( collection.Remove( new KeyValuePair<string, int>( "foo", 13 ) ) );
			Assert.IsTrue( collection.Remove( new KeyValuePair<string, int>( "foo", 42 ) ) );

			Assert.AreEqual( 12, dictionary[ "bar" ] );
			Assert.IsFalse( dictionary.ContainsKey( "foo" ) );
		}

		[Test]
		public void ICollectionCopyToKeyValuePairArray()
		{
			var dictionary = new LinkedDictionary<string, int>();
			dictionary.Add( "foo", 42 );

			var collection = dictionary as ICollection;

			Assert.AreEqual( 1, collection.Count );

			var pairs = new KeyValuePair<string, int>[ 1 ];

			collection.CopyTo( pairs, 0 );

			Assert.AreEqual( "foo", pairs[ 0 ].Key );
			Assert.AreEqual( 42, pairs[ 0 ].Value );
		}

		[Test]
		public void ICollectionCopyToDictionaryEntryArray()
		{
			var dictionary = new LinkedDictionary<string, int>();
			dictionary.Add( "foo", 42 );

			var collection = dictionary as ICollection;

			Assert.AreEqual( 1, collection.Count );

			var entries = new DictionaryEntry[ 1 ];

			collection.CopyTo( entries, 0 );

			Assert.AreEqual( "foo", ( string )entries[ 0 ].Key );
			Assert.AreEqual( 42, ( int )entries[ 0 ].Value );
		}

		[Test]
		public void ICollectionCopyToObjectArray()
		{
			var dictionary = new LinkedDictionary<string, int>();
			dictionary.Add( "foo", 42 );

			var collection = dictionary as ICollection;

			Assert.AreEqual( 1, collection.Count );

			var array = new object[ 1 ];

			collection.CopyTo( array, 0 );

			var pair = ( KeyValuePair<string, int> )array[ 0 ];

			Assert.AreEqual( "foo", pair.Key );
			Assert.AreEqual( 42, pair.Value );
		}

		[Test]
		[ExpectedException( typeof( ArgumentException ) )]
		public void ICollectionCopyToInvalidArray()
		{
			var dictionary = new LinkedDictionary<string, int>();
			dictionary.Add( "foo", 42 );

			var collection = dictionary as ICollection;

			Assert.AreEqual( 1, collection.Count );

			var array = new int[ 1 ];

			collection.CopyTo( array, 0 );
		}

		[Test]
		public void ValuesCopyToObjectArray()
		{
			var dictionary = new LinkedDictionary<string, string> { { "foo", "bar" } };

			var values = dictionary.Values as ICollection;

			var array = new object[ values.Count ];

			values.CopyTo( array, 0 );

			Assert.AreEqual( "bar", array[ 0 ] );
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
			var e1 = new LinkedDictionary<int, int>.Enumerator();
			Assert.IsFalse( Throws( delegate { var x = e1.Current; } ) );

			var d = new LinkedDictionary<int, int>();
			var e2 = d.GetEnumerator();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );
			e2.MoveNext();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );
			e2.Dispose();
			Assert.IsFalse( Throws( delegate { var x = e2.Current; } ) );

			var e3 = ( ( IEnumerable<KeyValuePair<int, int>> )d ).GetEnumerator();
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

		[Test]
		// based on #491858, #517415
		public void KeyEnumerator_Current()
		{
			var e1 = new LinkedDictionary<int, int>.KeySet.Enumerator();
			Assert.IsFalse( Throws( delegate { var x = e1.Current; } ) );

			var d = new LinkedDictionary<int, int>().Keys;
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

		[Test]
		// based on #491858, #517415
		public void ValueEnumerator_Current()
		{
			var e1 = new LinkedDictionary<int, int>.ValueCollection.Enumerator();
			Assert.IsFalse( Throws( delegate { var x = e1.Current; } ) );

			var d = new LinkedDictionary<int, int>().Values;
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

		[Test]
		public void ICollectionCopyTo()
		{
			var d = new LinkedDictionary<int, string>();

			ICollection c = d;
			c.CopyTo( new object[ 0 ], 0 );
			c.CopyTo( new string[ 0 ], 0 );
			c.CopyTo( new MyClass[ 0 ], 0 );

			c = d.Keys;
			c.CopyTo( new object[ 0 ], 0 );
			c.CopyTo( new ValueType[ 0 ], 0 );

			c = d.Values;
			c.CopyTo( new object[ 0 ], 0 );
			c.CopyTo( new MyClass[ 0 ], 0 );

			d[ 3 ] = null;

			c = d.Keys;
			c.CopyTo( new object[ 1 ], 0 );
			c.CopyTo( new ValueType[ 1 ], 0 );

			c = d.Values;
			c.CopyTo( new object[ 1 ], 0 );
			c.CopyTo( new MyClass[ 1 ], 0 );
		}

		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void ICollectionCopyTo_ex3()
		{
			var d = new LinkedDictionary<int, string>();
			d[ 3 ] = "5";

			ICollection c = d.Keys;
			c.CopyTo( new MyClass[ 1 ], 0 );
		}

		[Test, ExpectedException( typeof( ArgumentException ) )]
		public void ICollectionCopyTo_ex4()
		{
			var d = new LinkedDictionary<int, string>();
			d[ 3 ] = "5";

			ICollection c = d.Values;
			c.CopyTo( new MyClass[ 1 ], 0 );
		}

		#endregion -- Ported from Mono's Dictionary`2 unit tests --

		[Test]
		public void TestKeys()
		{
			var dictionary = new LinkedDictionary<string, int>() { { "A", 1 }, { "B", 2 } };
			var target = dictionary.Keys;
			var result = new List<string>();
			foreach ( var item in target )
			{
				result.Add( item );
			}
			Assert.That( result.Count, Is.EqualTo( 2 ) );
			Assert.That( result[ 0 ], Is.EqualTo( "A" ) );
			Assert.That( result[ 1 ], Is.EqualTo( "B" ) );
		}

		[Test]
		public void TestKeysCopyTo()
		{
			var dictionary = new LinkedDictionary<string, int>();
			var target = dictionary.Keys;
			var array = new string[ 5 ];

			// empty
			target.CopyTo( array );
			VerifyArray( target, array, 0, 0 );
			target.CopyTo( array, 1 );
			VerifyArray( target, array, 0, 0 );
			target.CopyTo( 1, array, 1, 2 );
			VerifyArray( target, array, 0, 0 );

			dictionary.Add( "A", 1 );
			dictionary.Add( "B", 2 );
			dictionary.Add( "C", 3 );

			target.CopyTo( array );
			VerifyArray( target, array, 0, 3 );
			Array.Clear( array, 0, array.Length );

			target.CopyTo( array, 1 );
			VerifyArray( target, array, 1, 3 );
			Array.Clear( array, 0, array.Length );

			target.CopyTo( 1, array, 1, 2 );
			Assert.That( array[ 0 ], Is.EqualTo( default( string ) ) );
			Assert.That( array[ 1 ], Is.EqualTo( "B" ) );
			Assert.That( array[ 2 ], Is.EqualTo( "C" ) );
			Assert.That( array[ 3 ], Is.EqualTo( default( string ) ) );
			Assert.That( array[ 4 ], Is.EqualTo( default( string ) ) );
		}

		[Test]
		public void TestKeySetIsProperSupersetOf()
		{
			var dictionary = new LinkedDictionary<int, int>();
			var target = dictionary.Keys;

			// empty
			Assert.That( target.IsProperSupersetOf( new int[ 0 ] ), Is.False );
			Assert.That( target.IsProperSupersetOf( new int[] { 1, 2, 3 } ), Is.False );

			dictionary.Add( 1, 1 );
			dictionary.Add( 2, 2 );
			dictionary.Add( 3, 3 );

			Assert.That( target.IsProperSupersetOf( new int[] { 1, 2, 3 } ), Is.False );
			Assert.That( target.IsProperSupersetOf( new int[] { 1, 2 } ), Is.True );
			Assert.That( target.IsProperSupersetOf( new int[] { 1, 2, 3, 4 } ), Is.False );
			Assert.That( target.IsProperSupersetOf( new int[] { 1, 2, 3, 1 } ), Is.False );
			Assert.That( target.IsProperSupersetOf( new int[] { 1, 2, 1 } ), Is.True );
			Assert.That( target.IsProperSupersetOf( new int[] { 1, 2, 3, 4, 1 } ), Is.False );
			Assert.That( target.IsProperSupersetOf( new int[ 0 ] ), Is.True );
		}

		[Test]
		public void TestKeySetIsSupersetOf()
		{
			var dictionary = new LinkedDictionary<int, int>();
			var target = dictionary.Keys;

			// empty
			Assert.That( target.IsSupersetOf( new int[ 0 ] ), Is.True );
			Assert.That( target.IsSupersetOf( new int[] { 1, 2, 3 } ), Is.False );

			dictionary.Add( 1, 1 );
			dictionary.Add( 2, 2 );
			dictionary.Add( 3, 3 );

			Assert.That( target.IsSupersetOf( new int[] { 1, 2, 3 } ), Is.True );
			Assert.That( target.IsSupersetOf( new int[] { 1, 2 } ), Is.True );
			Assert.That( target.IsSupersetOf( new int[] { 1, 2, 3, 4 } ), Is.False );
			Assert.That( target.IsSupersetOf( new int[] { 1, 2, 3, 1 } ), Is.True );
			Assert.That( target.IsSupersetOf( new int[] { 1, 2, 1 } ), Is.True );
			Assert.That( target.IsSupersetOf( new int[] { 1, 2, 3, 4, 1 } ), Is.False );
			Assert.That( target.IsSupersetOf( new int[ 0 ] ), Is.True );
		}

		[Test]
		public void TestKeySetIsProperSubsetOf()
		{
			var dictionary = new LinkedDictionary<int, int>();
			var target = dictionary.Keys;

			// empty
			Assert.That( target.IsProperSubsetOf( new int[ 0 ] ), Is.False );
			Assert.That( target.IsProperSubsetOf( new int[] { 1, 2, 3 } ), Is.True );

			dictionary.Add( 1, 1 );
			dictionary.Add( 2, 2 );
			dictionary.Add( 3, 3 );

			Assert.That( target.IsProperSubsetOf( new int[] { 1, 2, 3 } ), Is.False );
			Assert.That( target.IsProperSubsetOf( new int[] { 1, 2 } ), Is.False );
			Assert.That( target.IsProperSubsetOf( new int[] { 1, 2, 3, 4 } ), Is.True );
			Assert.That( target.IsProperSubsetOf( new int[] { 1, 2, 3, 1 } ), Is.False );
			Assert.That( target.IsProperSubsetOf( new int[] { 1, 2, 1 } ), Is.False );
			Assert.That( target.IsProperSubsetOf( new int[] { 1, 2, 3, 4, 1 } ), Is.True );
			Assert.That( target.IsProperSubsetOf( new int[ 0 ] ), Is.False );
		}

		[Test]
		public void TestKeySetIsSubsetOf()
		{
			var dictionary = new LinkedDictionary<int, int>();
			var target = dictionary.Keys;

			// empty
			Assert.That( target.IsSubsetOf( new int[ 0 ] ), Is.True );
			Assert.That( target.IsSubsetOf( new int[] { 1, 2, 3 } ), Is.True );

			dictionary.Add( 1, 1 );
			dictionary.Add( 2, 2 );
			dictionary.Add( 3, 3 );

			Assert.That( target.IsSubsetOf( new int[] { 1, 2, 3 } ), Is.True );
			Assert.That( target.IsSubsetOf( new int[] { 1, 2 } ), Is.False );
			Assert.That( target.IsSubsetOf( new int[] { 1, 2, 3, 4 } ), Is.True );
			Assert.That( target.IsSubsetOf( new int[] { 1, 2, 3, 1 } ), Is.True );
			Assert.That( target.IsSubsetOf( new int[] { 1, 2, 1 } ), Is.False );
			Assert.That( target.IsSubsetOf( new int[] { 1, 2, 3, 4, 1 } ), Is.True );
			Assert.That( target.IsSubsetOf( new int[ 0 ] ), Is.False );
		}

		[Test]
		public void TestKeySetOverlaps()
		{
			var dictionary = new LinkedDictionary<int, int>();
			var target = dictionary.Keys;

			// empty
			Assert.That( target.Overlaps( new int[ 0 ] ), Is.False );
			Assert.That( target.Overlaps( new int[] { 1, 2, 3 } ), Is.False );

			dictionary.Add( 1, 1 );
			dictionary.Add( 2, 2 );
			dictionary.Add( 3, 3 );

			Assert.That( target.Overlaps( new int[] { 1, 2, 3 } ), Is.True );
			Assert.That( target.Overlaps( new int[] { 1, 2 } ), Is.True );
			Assert.That( target.Overlaps( new int[] { 1, 2, 3, 4 } ), Is.True );
			Assert.That( target.Overlaps( new int[] { 1, 2, 3, 1 } ), Is.True );
			Assert.That( target.Overlaps( new int[] { 1, 2, 1 } ), Is.True );
			Assert.That( target.Overlaps( new int[] { 1, 2, 3, 4, 1 } ), Is.True );
			Assert.That( target.Overlaps( new int[ 0 ] ), Is.False );
		}

		[Test]
		public void TestKeySetSetEquals()
		{
			var dictionary = new LinkedDictionary<int, int>();
			var target = dictionary.Keys;

			// empty
			Assert.That( target.SetEquals( new int[ 0 ] ), Is.True );
			Assert.That( target.SetEquals( new int[] { 1, 2, 3 } ), Is.False );

			dictionary.Add( 1, 1 );
			dictionary.Add( 2, 2 );
			dictionary.Add( 3, 3 );

			Assert.That( target.SetEquals( new int[] { 1, 2, 3 } ), Is.True );
			Assert.That( target.SetEquals( new int[] { 1, 2 } ), Is.False );
			Assert.That( target.SetEquals( new int[] { 1, 2, 3, 4 } ), Is.False );
			Assert.That( target.SetEquals( new int[] { 1, 2, 3, 1 } ), Is.True );
			Assert.That( target.SetEquals( new int[] { 1, 2, 1 } ), Is.False );
			Assert.That( target.SetEquals( new int[] { 1, 2, 3, 4, 1 } ), Is.False );
			Assert.That( target.SetEquals( new int[ 0 ] ), Is.False );
		}

		[Test]
		public void TestValues()
		{
			var dictionary = new LinkedDictionary<string, int>() { { "A", 1 }, { "B", 2 } };
			var target = dictionary.Values;
			var result = new List<int>();
			foreach ( var item in target )
			{
				result.Add( item );
			}
			Assert.That( result.Count, Is.EqualTo( 2 ) );
			Assert.That( result[ 0 ], Is.EqualTo( 1 ) );
			Assert.That( result[ 1 ], Is.EqualTo( 2 ) );
		}

		[Test]
		public void TestValuesCopyTo()
		{
			var dictionary = new LinkedDictionary<string, int>();
			var target = dictionary.Values;
			var array = new int[ 5 ];

			// empty
			target.CopyTo( array );
			VerifyArray( target, array, 0, 0 );
			target.CopyTo( array, 1 );
			VerifyArray( target, array, 0, 0 );
			target.CopyTo( 1, array, 1, 2 );
			VerifyArray( target, array, 0, 0 );

			dictionary.Add( "A", 1 );
			dictionary.Add( "B", 2 );
			dictionary.Add( "C", 3 );

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

		private sealed class MyClass { }
	}
}
