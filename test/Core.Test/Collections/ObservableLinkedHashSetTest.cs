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
using System.Collections.Specialized;
using System.ComponentModel;
using NUnit.Framework;

namespace NLiblet.Collections
{
	[TestFixture]
	[Timeout( 1000 )]
	public class ObservableLinkedHashSetTest
	{
		[Test]
		public void TestNotification()
		{
			var propertyChangedRecord = new Dictionary<string, int>();
			var target = new ObservableLinkedHashSet<string>();

			// No handler
			target.Add( "A" );
			target.Add( "B" );
			target.Remove( "A" );
			target.Clear();

			( target as INotifyPropertyChanged ).PropertyChanged += ( sender, e ) => InrementRecord( propertyChangedRecord, e.PropertyName );
			string value = "A";
			bool raised = false;
			NotifyCollectionChangedEventHandler handler =
				( sender, e ) =>
				{
					Assert.That( e.Action, Is.EqualTo( NotifyCollectionChangedAction.Add ) );
					Assert.That( e.NewItems.Count, Is.EqualTo( 1 ) );
					Assert.That( e.NewItems[ 0 ], Is.EqualTo( "A" ) );
					raised = true;
				};
			target.CollectionChanged += handler;
			target.Add( value );
			target.CollectionChanged -= handler;
			Assert.That( raised, Is.True );
			Assert.That( propertyChangedRecord.Count, Is.EqualTo( 4 ) );
			Assert.That( propertyChangedRecord[ "Count" ], Is.EqualTo( 1 ) );
			Assert.That( propertyChangedRecord[ "Head" ], Is.EqualTo( 1 ) );
			Assert.That( propertyChangedRecord[ "Tail" ], Is.EqualTo( 1 ) );
			Assert.That( propertyChangedRecord[ "Item[]" ], Is.EqualTo( 1 ) );

			value = "B";
			raised = false;
			handler =
				( sender, e ) =>
				{
					Assert.That( e.Action, Is.EqualTo( NotifyCollectionChangedAction.Add ) );
					Assert.That( e.NewItems.Count, Is.EqualTo( 1 ) );
					Assert.That( e.NewItems[ 0 ], Is.EqualTo( "B" ) );
					Assert.That( e.OldItems, Is.Null );
					raised = true;
				};
			target.CollectionChanged += handler;
			target.Add( value );
			target.CollectionChanged -= handler;
			Assert.That( raised, Is.True );
			Assert.That( propertyChangedRecord.Count, Is.EqualTo( 4 ) );
			Assert.That( propertyChangedRecord[ "Count" ], Is.EqualTo( 2 ) );
			Assert.That( propertyChangedRecord[ "Head" ], Is.EqualTo( 1 ) );
			Assert.That( propertyChangedRecord[ "Tail" ], Is.EqualTo( 2 ) );
			Assert.That( propertyChangedRecord[ "Item[]" ], Is.EqualTo( 2 ) );

			value = "A";
			raised = false;
			handler =
				( sender, e ) =>
				{
					Assert.That( e.Action, Is.EqualTo( NotifyCollectionChangedAction.Remove ) );
					Assert.That( e.OldItems.Count, Is.EqualTo( 1 ) );
					Assert.That( e.OldItems[ 0 ], Is.EqualTo( "A" ) );
					Assert.That( e.NewItems, Is.Null );
					raised = true;
				};
			target.CollectionChanged += handler;
			target.Remove( value );
			target.CollectionChanged -= handler;
			Assert.That( raised, Is.True );
			Assert.That( propertyChangedRecord.Count, Is.EqualTo( 4 ) );
			Assert.That( propertyChangedRecord[ "Count" ], Is.EqualTo( 3 ) );
			Assert.That( propertyChangedRecord[ "Head" ], Is.EqualTo( 2 ) );
			Assert.That( propertyChangedRecord[ "Tail" ], Is.EqualTo( 2 ) );
			Assert.That( propertyChangedRecord[ "Item[]" ], Is.EqualTo( 3 ) );

			raised = false;
			handler =
				( sender, e ) =>
				{
					Assert.That( e.Action, Is.EqualTo( NotifyCollectionChangedAction.Reset ) );
					Assert.That( e.OldItems, Is.Null );
					Assert.That( e.NewItems, Is.Null );
					raised = true;
				};
			target.CollectionChanged += handler;
			target.Clear();
			target.CollectionChanged -= handler;
			Assert.That( raised, Is.True );
			Assert.That( propertyChangedRecord.Count, Is.EqualTo( 4 ) );
			Assert.That( propertyChangedRecord[ "Count" ], Is.EqualTo( 4 ) );
			Assert.That( propertyChangedRecord[ "Head" ], Is.EqualTo( 3 ) );
			Assert.That( propertyChangedRecord[ "Tail" ], Is.EqualTo( 3 ) );
			Assert.That( propertyChangedRecord[ "Item[]" ], Is.EqualTo( 4 ) );
		}

		[Test]
		public void TestReentrantGuard_EventHandlerCountIs1()
		{
			var target = new ObservableLinkedHashSet<int>();
			target.CollectionChanged +=
				( sender, e ) =>
				{
					var source = sender as ObservableLinkedHashSet<int>;
					// Avoding infinite recursion
					if ( ( int )e.NewItems[ 0 ] == 1 )
					{
						source.Add( 2 );
					}
				};
			target.Add( 1 );

			Assert.That( target.Count, Is.EqualTo( 2 ) );
		}

		[Test]
		[ExpectedException( typeof( InvalidOperationException ) )]
		public void TestReentrantGuard_EventHandlerCountIs2()
		{
			var target = new ObservableLinkedHashSet<int>();
			target.CollectionChanged +=
				( sender, e ) => { return; };
			target.CollectionChanged +=
				( sender, e ) =>
				{
					var source = sender as ObservableLinkedHashSet<int>;
					// Avoding infinite recursion
					if ( ( int )e.NewItems[ 0 ] == 1 )
					{
						source.Add( 2 );
					}
				};
			target.Add( 1 );
		}

		private void InrementRecord( IDictionary<string, int> record, string key )
		{
			int oldCount;
			record.TryGetValue( key, out oldCount );
			record[ key ] = oldCount + 1;
		}
	}
}
