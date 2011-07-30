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

// This code is generated from T4Template TupleFormatter`n.tt.
// Do not modify this source code directly.

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;

using NLiblet.Properties;
using NLiblet.Reflection;

namespace NLiblet.Text
{
	// FIXME: should not nested
	partial class CommonCustomFormatter
	{
		/// <summary>
		///		Non-generic entry point for tuple formatter.
		/// </summary>
		internal static class TupleFormatter
		{
			public static ItemFormatter<T> Get<T>()
			{
				Contract.Assert(
					typeof( T ).IsClosedTypeOf( typeof( Tuple<> ) )
					|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,> ) )
					|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,> ) )
					|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,> ) )
					|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,,> ) )
					|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,,,> ) )
					|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,,,,> ) )
					|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,,,,,> ) )
				);
				
				var genericArguments = typeof( T ).GetGenericArguments();
				
				switch( genericArguments.Length )
				{
					case 1:
					{
						return Activator.CreateInstance( typeof( TupleFormatter<> ).MakeGenericType( genericArguments ) ) as ItemFormatter<T>;
					}
					case 2:
					{
						return Activator.CreateInstance( typeof( TupleFormatter<,> ).MakeGenericType( genericArguments ) ) as ItemFormatter<T>;
					}
					case 3:
					{
						return Activator.CreateInstance( typeof( TupleFormatter<,,> ).MakeGenericType( genericArguments ) ) as ItemFormatter<T>;
					}
					case 4:
					{
						return Activator.CreateInstance( typeof( TupleFormatter<,,,> ).MakeGenericType( genericArguments ) ) as ItemFormatter<T>;
					}
					case 5:
					{
						return Activator.CreateInstance( typeof( TupleFormatter<,,,,> ).MakeGenericType( genericArguments ) ) as ItemFormatter<T>;
					}
					case 6:
					{
						return Activator.CreateInstance( typeof( TupleFormatter<,,,,,> ).MakeGenericType( genericArguments ) ) as ItemFormatter<T>;
					}
					case 7:
					{
						return Activator.CreateInstance( typeof( TupleFormatter<,,,,,,> ).MakeGenericType( genericArguments ) ) as ItemFormatter<T>;
					}
					case 8:
					{
						return Activator.CreateInstance( typeof( TupleFormatter<,,,,,,,> ).MakeGenericType( genericArguments ) ) as ItemFormatter<T>;
					}
					default:
					{
						throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, Resources.Formatter_UexpectedType, typeof( T ).AssemblyQualifiedName ) );
					}
				}
			}
		}
		// FIXME: Change to instance base.
		partial class GenericItemFormatter<T>
		{
			private static void FormatTuple1To( T item, FormattingContext context )
			{
				TupleFormatter.Get<T>().FormatTo( item, context );
			}
		}
		// FIXME: Change to instance base.
		partial class GenericItemFormatter<T>
		{
			private static void FormatTuple2To( T item, FormattingContext context )
			{
				TupleFormatter.Get<T>().FormatTo( item, context );
			}
		}
		// FIXME: Change to instance base.
		partial class GenericItemFormatter<T>
		{
			private static void FormatTuple3To( T item, FormattingContext context )
			{
				TupleFormatter.Get<T>().FormatTo( item, context );
			}
		}
		// FIXME: Change to instance base.
		partial class GenericItemFormatter<T>
		{
			private static void FormatTuple4To( T item, FormattingContext context )
			{
				TupleFormatter.Get<T>().FormatTo( item, context );
			}
		}
		// FIXME: Change to instance base.
		partial class GenericItemFormatter<T>
		{
			private static void FormatTuple5To( T item, FormattingContext context )
			{
				TupleFormatter.Get<T>().FormatTo( item, context );
			}
		}
		// FIXME: Change to instance base.
		partial class GenericItemFormatter<T>
		{
			private static void FormatTuple6To( T item, FormattingContext context )
			{
				TupleFormatter.Get<T>().FormatTo( item, context );
			}
		}
		// FIXME: Change to instance base.
		partial class GenericItemFormatter<T>
		{
			private static void FormatTuple7To( T item, FormattingContext context )
			{
				TupleFormatter.Get<T>().FormatTo( item, context );
			}
		}
		// FIXME: Change to instance base.
		partial class GenericItemFormatter<T>
		{
			private static void FormatTuple8To( T item, FormattingContext context )
			{
				TupleFormatter.Get<T>().FormatTo( item, context );
			}
		}

		internal sealed class TupleFormatter<T1> :
			ItemFormatter<Tuple<T1>>
		{
			private readonly Action<T1, FormattingContext> _item1Formatter;
			
			// Combination of tuple is too many to cache.
			public TupleFormatter()	
			{
				Debug.WriteLine( "TupleFormatter<T1>::.ctor() init item1Formatter with {0}", GenericItemFormatter<T1>.Action.Method );
				this._item1Formatter = GenericItemFormatter<T1>.Action;
			
			}
			
			public sealed override void FormatTo(  Tuple<T1> tuple, FormattingContext context )
			{
				Debug.WriteLine( "TupleFormatter<T1>::FormatTo( {0}, {1} )", tuple, context );
				
				context.Buffer.Append( "[ " );
				context.EnterCollection();
				this._item1Formatter( tuple.Item1, context );
		
				context.LeaveCollection();
				context.Buffer.Append( " ]" );				
			}
		}

		internal sealed class TupleFormatter<T1, T2> :
			ItemFormatter<Tuple<T1, T2>>
		{
			private readonly Action<T1, FormattingContext> _item1Formatter;
			private readonly Action<T2, FormattingContext> _item2Formatter;
			
			// Combination of tuple is too many to cache.
			public TupleFormatter()	
			{
				Debug.WriteLine( "TupleFormatter<T1, T2>::.ctor() init item1Formatter with {0}", GenericItemFormatter<T1>.Action.Method );
				this._item1Formatter = GenericItemFormatter<T1>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2>::.ctor() init item2Formatter with {0}", GenericItemFormatter<T2>.Action.Method );
				this._item2Formatter = GenericItemFormatter<T2>.Action;
			
			}
			
			public sealed override void FormatTo(  Tuple<T1, T2> tuple, FormattingContext context )
			{
				Debug.WriteLine( "TupleFormatter<T1, T2>::FormatTo( {0}, {1} )", tuple, context );
				
				context.Buffer.Append( "[ " );
				context.EnterCollection();
				this._item1Formatter( tuple.Item1, context );
				context.Buffer.Append( ", " );
				this._item2Formatter( tuple.Item2, context );
		
				context.LeaveCollection();
				context.Buffer.Append( " ]" );				
			}
		}

		internal sealed class TupleFormatter<T1, T2, T3> :
			ItemFormatter<Tuple<T1, T2, T3>>
		{
			private readonly Action<T1, FormattingContext> _item1Formatter;
			private readonly Action<T2, FormattingContext> _item2Formatter;
			private readonly Action<T3, FormattingContext> _item3Formatter;
			
			// Combination of tuple is too many to cache.
			public TupleFormatter()	
			{
				Debug.WriteLine( "TupleFormatter<T1, T2, T3>::.ctor() init item1Formatter with {0}", GenericItemFormatter<T1>.Action.Method );
				this._item1Formatter = GenericItemFormatter<T1>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3>::.ctor() init item2Formatter with {0}", GenericItemFormatter<T2>.Action.Method );
				this._item2Formatter = GenericItemFormatter<T2>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3>::.ctor() init item3Formatter with {0}", GenericItemFormatter<T3>.Action.Method );
				this._item3Formatter = GenericItemFormatter<T3>.Action;
			
			}
			
			public sealed override void FormatTo(  Tuple<T1, T2, T3> tuple, FormattingContext context )
			{
				Debug.WriteLine( "TupleFormatter<T1, T2, T3>::FormatTo( {0}, {1} )", tuple, context );
				
				context.Buffer.Append( "[ " );
				context.EnterCollection();
				this._item1Formatter( tuple.Item1, context );
				context.Buffer.Append( ", " );
				this._item2Formatter( tuple.Item2, context );
				context.Buffer.Append( ", " );
				this._item3Formatter( tuple.Item3, context );
		
				context.LeaveCollection();
				context.Buffer.Append( " ]" );				
			}
		}

		internal sealed class TupleFormatter<T1, T2, T3, T4> :
			ItemFormatter<Tuple<T1, T2, T3, T4>>
		{
			private readonly Action<T1, FormattingContext> _item1Formatter;
			private readonly Action<T2, FormattingContext> _item2Formatter;
			private readonly Action<T3, FormattingContext> _item3Formatter;
			private readonly Action<T4, FormattingContext> _item4Formatter;
			
			// Combination of tuple is too many to cache.
			public TupleFormatter()	
			{
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4>::.ctor() init item1Formatter with {0}", GenericItemFormatter<T1>.Action.Method );
				this._item1Formatter = GenericItemFormatter<T1>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4>::.ctor() init item2Formatter with {0}", GenericItemFormatter<T2>.Action.Method );
				this._item2Formatter = GenericItemFormatter<T2>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4>::.ctor() init item3Formatter with {0}", GenericItemFormatter<T3>.Action.Method );
				this._item3Formatter = GenericItemFormatter<T3>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4>::.ctor() init item4Formatter with {0}", GenericItemFormatter<T4>.Action.Method );
				this._item4Formatter = GenericItemFormatter<T4>.Action;
			
			}
			
			public sealed override void FormatTo(  Tuple<T1, T2, T3, T4> tuple, FormattingContext context )
			{
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4>::FormatTo( {0}, {1} )", tuple, context );
				
				context.Buffer.Append( "[ " );
				context.EnterCollection();
				this._item1Formatter( tuple.Item1, context );
				context.Buffer.Append( ", " );
				this._item2Formatter( tuple.Item2, context );
				context.Buffer.Append( ", " );
				this._item3Formatter( tuple.Item3, context );
				context.Buffer.Append( ", " );
				this._item4Formatter( tuple.Item4, context );
		
				context.LeaveCollection();
				context.Buffer.Append( " ]" );				
			}
		}

		internal sealed class TupleFormatter<T1, T2, T3, T4, T5> :
			ItemFormatter<Tuple<T1, T2, T3, T4, T5>>
		{
			private readonly Action<T1, FormattingContext> _item1Formatter;
			private readonly Action<T2, FormattingContext> _item2Formatter;
			private readonly Action<T3, FormattingContext> _item3Formatter;
			private readonly Action<T4, FormattingContext> _item4Formatter;
			private readonly Action<T5, FormattingContext> _item5Formatter;
			
			// Combination of tuple is too many to cache.
			public TupleFormatter()	
			{
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5>::.ctor() init item1Formatter with {0}", GenericItemFormatter<T1>.Action.Method );
				this._item1Formatter = GenericItemFormatter<T1>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5>::.ctor() init item2Formatter with {0}", GenericItemFormatter<T2>.Action.Method );
				this._item2Formatter = GenericItemFormatter<T2>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5>::.ctor() init item3Formatter with {0}", GenericItemFormatter<T3>.Action.Method );
				this._item3Formatter = GenericItemFormatter<T3>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5>::.ctor() init item4Formatter with {0}", GenericItemFormatter<T4>.Action.Method );
				this._item4Formatter = GenericItemFormatter<T4>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5>::.ctor() init item5Formatter with {0}", GenericItemFormatter<T5>.Action.Method );
				this._item5Formatter = GenericItemFormatter<T5>.Action;
			
			}
			
			public sealed override void FormatTo(  Tuple<T1, T2, T3, T4, T5> tuple, FormattingContext context )
			{
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5>::FormatTo( {0}, {1} )", tuple, context );
				
				context.Buffer.Append( "[ " );
				context.EnterCollection();
				this._item1Formatter( tuple.Item1, context );
				context.Buffer.Append( ", " );
				this._item2Formatter( tuple.Item2, context );
				context.Buffer.Append( ", " );
				this._item3Formatter( tuple.Item3, context );
				context.Buffer.Append( ", " );
				this._item4Formatter( tuple.Item4, context );
				context.Buffer.Append( ", " );
				this._item5Formatter( tuple.Item5, context );
		
				context.LeaveCollection();
				context.Buffer.Append( " ]" );				
			}
		}

		internal sealed class TupleFormatter<T1, T2, T3, T4, T5, T6> :
			ItemFormatter<Tuple<T1, T2, T3, T4, T5, T6>>
		{
			private readonly Action<T1, FormattingContext> _item1Formatter;
			private readonly Action<T2, FormattingContext> _item2Formatter;
			private readonly Action<T3, FormattingContext> _item3Formatter;
			private readonly Action<T4, FormattingContext> _item4Formatter;
			private readonly Action<T5, FormattingContext> _item5Formatter;
			private readonly Action<T6, FormattingContext> _item6Formatter;
			
			// Combination of tuple is too many to cache.
			public TupleFormatter()	
			{
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6>::.ctor() init item1Formatter with {0}", GenericItemFormatter<T1>.Action.Method );
				this._item1Formatter = GenericItemFormatter<T1>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6>::.ctor() init item2Formatter with {0}", GenericItemFormatter<T2>.Action.Method );
				this._item2Formatter = GenericItemFormatter<T2>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6>::.ctor() init item3Formatter with {0}", GenericItemFormatter<T3>.Action.Method );
				this._item3Formatter = GenericItemFormatter<T3>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6>::.ctor() init item4Formatter with {0}", GenericItemFormatter<T4>.Action.Method );
				this._item4Formatter = GenericItemFormatter<T4>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6>::.ctor() init item5Formatter with {0}", GenericItemFormatter<T5>.Action.Method );
				this._item5Formatter = GenericItemFormatter<T5>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6>::.ctor() init item6Formatter with {0}", GenericItemFormatter<T6>.Action.Method );
				this._item6Formatter = GenericItemFormatter<T6>.Action;
			
			}
			
			public sealed override void FormatTo(  Tuple<T1, T2, T3, T4, T5, T6> tuple, FormattingContext context )
			{
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6>::FormatTo( {0}, {1} )", tuple, context );
				
				context.Buffer.Append( "[ " );
				context.EnterCollection();
				this._item1Formatter( tuple.Item1, context );
				context.Buffer.Append( ", " );
				this._item2Formatter( tuple.Item2, context );
				context.Buffer.Append( ", " );
				this._item3Formatter( tuple.Item3, context );
				context.Buffer.Append( ", " );
				this._item4Formatter( tuple.Item4, context );
				context.Buffer.Append( ", " );
				this._item5Formatter( tuple.Item5, context );
				context.Buffer.Append( ", " );
				this._item6Formatter( tuple.Item6, context );
		
				context.LeaveCollection();
				context.Buffer.Append( " ]" );				
			}
		}

		internal sealed class TupleFormatter<T1, T2, T3, T4, T5, T6, T7> :
			ItemFormatter<Tuple<T1, T2, T3, T4, T5, T6, T7>>
		{
			private readonly Action<T1, FormattingContext> _item1Formatter;
			private readonly Action<T2, FormattingContext> _item2Formatter;
			private readonly Action<T3, FormattingContext> _item3Formatter;
			private readonly Action<T4, FormattingContext> _item4Formatter;
			private readonly Action<T5, FormattingContext> _item5Formatter;
			private readonly Action<T6, FormattingContext> _item6Formatter;
			private readonly Action<T7, FormattingContext> _item7Formatter;
			
			// Combination of tuple is too many to cache.
			public TupleFormatter()	
			{
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7>::.ctor() init item1Formatter with {0}", GenericItemFormatter<T1>.Action.Method );
				this._item1Formatter = GenericItemFormatter<T1>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7>::.ctor() init item2Formatter with {0}", GenericItemFormatter<T2>.Action.Method );
				this._item2Formatter = GenericItemFormatter<T2>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7>::.ctor() init item3Formatter with {0}", GenericItemFormatter<T3>.Action.Method );
				this._item3Formatter = GenericItemFormatter<T3>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7>::.ctor() init item4Formatter with {0}", GenericItemFormatter<T4>.Action.Method );
				this._item4Formatter = GenericItemFormatter<T4>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7>::.ctor() init item5Formatter with {0}", GenericItemFormatter<T5>.Action.Method );
				this._item5Formatter = GenericItemFormatter<T5>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7>::.ctor() init item6Formatter with {0}", GenericItemFormatter<T6>.Action.Method );
				this._item6Formatter = GenericItemFormatter<T6>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7>::.ctor() init item7Formatter with {0}", GenericItemFormatter<T7>.Action.Method );
				this._item7Formatter = GenericItemFormatter<T7>.Action;
			
			}
			
			public sealed override void FormatTo(  Tuple<T1, T2, T3, T4, T5, T6, T7> tuple, FormattingContext context )
			{
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7>::FormatTo( {0}, {1} )", tuple, context );
				
				context.Buffer.Append( "[ " );
				context.EnterCollection();
				this._item1Formatter( tuple.Item1, context );
				context.Buffer.Append( ", " );
				this._item2Formatter( tuple.Item2, context );
				context.Buffer.Append( ", " );
				this._item3Formatter( tuple.Item3, context );
				context.Buffer.Append( ", " );
				this._item4Formatter( tuple.Item4, context );
				context.Buffer.Append( ", " );
				this._item5Formatter( tuple.Item5, context );
				context.Buffer.Append( ", " );
				this._item6Formatter( tuple.Item6, context );
				context.Buffer.Append( ", " );
				this._item7Formatter( tuple.Item7, context );
		
				context.LeaveCollection();
				context.Buffer.Append( " ]" );				
			}
		}

		internal sealed class TupleFormatter<T1, T2, T3, T4, T5, T6, T7, T8> :
			ItemFormatter<Tuple<T1, T2, T3, T4, T5, T6, T7, T8>>
		{
			private readonly Action<T1, FormattingContext> _item1Formatter;
			private readonly Action<T2, FormattingContext> _item2Formatter;
			private readonly Action<T3, FormattingContext> _item3Formatter;
			private readonly Action<T4, FormattingContext> _item4Formatter;
			private readonly Action<T5, FormattingContext> _item5Formatter;
			private readonly Action<T6, FormattingContext> _item6Formatter;
			private readonly Action<T7, FormattingContext> _item7Formatter;
			private readonly Action<T8, FormattingContext> _item8Formatter;
			
			// Combination of tuple is too many to cache.
			public TupleFormatter()	
			{
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7, T8>::.ctor() init item1Formatter with {0}", GenericItemFormatter<T1>.Action.Method );
				this._item1Formatter = GenericItemFormatter<T1>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7, T8>::.ctor() init item2Formatter with {0}", GenericItemFormatter<T2>.Action.Method );
				this._item2Formatter = GenericItemFormatter<T2>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7, T8>::.ctor() init item3Formatter with {0}", GenericItemFormatter<T3>.Action.Method );
				this._item3Formatter = GenericItemFormatter<T3>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7, T8>::.ctor() init item4Formatter with {0}", GenericItemFormatter<T4>.Action.Method );
				this._item4Formatter = GenericItemFormatter<T4>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7, T8>::.ctor() init item5Formatter with {0}", GenericItemFormatter<T5>.Action.Method );
				this._item5Formatter = GenericItemFormatter<T5>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7, T8>::.ctor() init item6Formatter with {0}", GenericItemFormatter<T6>.Action.Method );
				this._item6Formatter = GenericItemFormatter<T6>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7, T8>::.ctor() init item7Formatter with {0}", GenericItemFormatter<T7>.Action.Method );
				this._item7Formatter = GenericItemFormatter<T7>.Action;
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7, T8>::.ctor() init item8Formatter with {0}", GenericItemFormatter<T8>.Action.Method );
				this._item8Formatter = GenericItemFormatter<T8>.Action;
			
			}
			
			public sealed override void FormatTo(  Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple, FormattingContext context )
			{
				Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7, T8>::FormatTo( {0}, {1} )", tuple, context );
				
				context.Buffer.Append( "[ " );
				context.EnterCollection();
				this._item1Formatter( tuple.Item1, context );
				context.Buffer.Append( ", " );
				this._item2Formatter( tuple.Item2, context );
				context.Buffer.Append( ", " );
				this._item3Formatter( tuple.Item3, context );
				context.Buffer.Append( ", " );
				this._item4Formatter( tuple.Item4, context );
				context.Buffer.Append( ", " );
				this._item5Formatter( tuple.Item5, context );
				context.Buffer.Append( ", " );
				this._item6Formatter( tuple.Item6, context );
				context.Buffer.Append( ", " );
				this._item7Formatter( tuple.Item7, context );
				context.Buffer.Append( ", " );
				this._item8Formatter( tuple.Rest, context );
		
				context.LeaveCollection();
				context.Buffer.Append( " ]" );				
			}
		}
		
	}
}