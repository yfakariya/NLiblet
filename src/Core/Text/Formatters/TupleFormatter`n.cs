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

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Non-generic entry point for tuple formatter.
	/// </summary>
	internal static class TupleFormatter
	{
		public static ItemFormatter Get( Type tupleType )
		{
			Contract.Assert(
				tupleType.IsClosedTypeOf( typeof( Tuple<> ) )
				|| tupleType.IsClosedTypeOf( typeof( Tuple<,> ) )
				|| tupleType.IsClosedTypeOf( typeof( Tuple<,,> ) )
				|| tupleType.IsClosedTypeOf( typeof( Tuple<,,,> ) )
				|| tupleType.IsClosedTypeOf( typeof( Tuple<,,,,> ) )
				|| tupleType.IsClosedTypeOf( typeof( Tuple<,,,,,> ) )
				|| tupleType.IsClosedTypeOf( typeof( Tuple<,,,,,,> ) )
				|| tupleType.IsClosedTypeOf( typeof( Tuple<,,,,,,,> ) )
			);
			
			var genericArguments = tupleType.GetGenericArguments();
			
			switch( genericArguments.Length )
			{
				case 1:
				{
					return Activator.CreateInstance( typeof( TupleFormatter<> ).MakeGenericType( genericArguments ) ) as ItemFormatter;
				}
				case 2:
				{
					return Activator.CreateInstance( typeof( TupleFormatter<,> ).MakeGenericType( genericArguments ) ) as ItemFormatter;
				}
				case 3:
				{
					return Activator.CreateInstance( typeof( TupleFormatter<,,> ).MakeGenericType( genericArguments ) ) as ItemFormatter;
				}
				case 4:
				{
					return Activator.CreateInstance( typeof( TupleFormatter<,,,> ).MakeGenericType( genericArguments ) ) as ItemFormatter;
				}
				case 5:
				{
					return Activator.CreateInstance( typeof( TupleFormatter<,,,,> ).MakeGenericType( genericArguments ) ) as ItemFormatter;
				}
				case 6:
				{
					return Activator.CreateInstance( typeof( TupleFormatter<,,,,,> ).MakeGenericType( genericArguments ) ) as ItemFormatter;
				}
				case 7:
				{
					return Activator.CreateInstance( typeof( TupleFormatter<,,,,,,> ).MakeGenericType( genericArguments ) ) as ItemFormatter;
				}
				case 8:
				{
					return Activator.CreateInstance( typeof( TupleFormatter<,,,,,,,> ).MakeGenericType( genericArguments ) ) as ItemFormatter;
				}
				default:
				{
					throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, Resources.Formatter_UnexpectedType, tupleType.AssemblyQualifiedName ) );
				}
			}
		}
	// FIXME : DELETE
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
					throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, Resources.Formatter_UnexpectedType, typeof( T ).AssemblyQualifiedName ) );
				}
			}
		}
	}

	internal sealed class TupleFormatter<T1> :
		ItemFormatter<Tuple<T1>>
	{
		private readonly IItemFormatter<T1> _item1Formatter;
			
			// Combination of tuple is too many to cache.
		public TupleFormatter()	
		{
			this._item1Formatter = ItemFormatter.Get<T1>();
			
		}
			
		public sealed override void FormatTo(  Tuple<T1> tuple, FormattingContext context )
		{
			Debug.WriteLine( "TupleFormatter<T1>::FormatTo( {0}, {1} )", tuple, context );
			
			context.Buffer.Append( "[ " );
			context.EnterCollection();
			this._item1Formatter.FormatTo( tuple.Item1, context );
		
			context.LeaveCollection();
			context.Buffer.Append( " ]" );				
		}
	}

	internal sealed class TupleFormatter<T1, T2> :
		ItemFormatter<Tuple<T1, T2>>
	{
		private readonly IItemFormatter<T1> _item1Formatter;
		private readonly IItemFormatter<T2> _item2Formatter;
			
			// Combination of tuple is too many to cache.
		public TupleFormatter()	
		{
			this._item1Formatter = ItemFormatter.Get<T1>();
			this._item2Formatter = ItemFormatter.Get<T2>();
			
		}
			
		public sealed override void FormatTo(  Tuple<T1, T2> tuple, FormattingContext context )
		{
			Debug.WriteLine( "TupleFormatter<T1, T2>::FormatTo( {0}, {1} )", tuple, context );
			
			context.Buffer.Append( "[ " );
			context.EnterCollection();
			this._item1Formatter.FormatTo( tuple.Item1, context );
			context.Buffer.Append( ", " );
			this._item2Formatter.FormatTo( tuple.Item2, context );
		
			context.LeaveCollection();
			context.Buffer.Append( " ]" );				
		}
	}

	internal sealed class TupleFormatter<T1, T2, T3> :
		ItemFormatter<Tuple<T1, T2, T3>>
	{
		private readonly IItemFormatter<T1> _item1Formatter;
		private readonly IItemFormatter<T2> _item2Formatter;
		private readonly IItemFormatter<T3> _item3Formatter;
			
			// Combination of tuple is too many to cache.
		public TupleFormatter()	
		{
			this._item1Formatter = ItemFormatter.Get<T1>();
			this._item2Formatter = ItemFormatter.Get<T2>();
			this._item3Formatter = ItemFormatter.Get<T3>();
			
		}
			
		public sealed override void FormatTo(  Tuple<T1, T2, T3> tuple, FormattingContext context )
		{
			Debug.WriteLine( "TupleFormatter<T1, T2, T3>::FormatTo( {0}, {1} )", tuple, context );
			
			context.Buffer.Append( "[ " );
			context.EnterCollection();
			this._item1Formatter.FormatTo( tuple.Item1, context );
			context.Buffer.Append( ", " );
			this._item2Formatter.FormatTo( tuple.Item2, context );
			context.Buffer.Append( ", " );
			this._item3Formatter.FormatTo( tuple.Item3, context );
		
			context.LeaveCollection();
			context.Buffer.Append( " ]" );				
		}
	}

	internal sealed class TupleFormatter<T1, T2, T3, T4> :
		ItemFormatter<Tuple<T1, T2, T3, T4>>
	{
		private readonly IItemFormatter<T1> _item1Formatter;
		private readonly IItemFormatter<T2> _item2Formatter;
		private readonly IItemFormatter<T3> _item3Formatter;
		private readonly IItemFormatter<T4> _item4Formatter;
			
			// Combination of tuple is too many to cache.
		public TupleFormatter()	
		{
			this._item1Formatter = ItemFormatter.Get<T1>();
			this._item2Formatter = ItemFormatter.Get<T2>();
			this._item3Formatter = ItemFormatter.Get<T3>();
			this._item4Formatter = ItemFormatter.Get<T4>();
			
		}
			
		public sealed override void FormatTo(  Tuple<T1, T2, T3, T4> tuple, FormattingContext context )
		{
			Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4>::FormatTo( {0}, {1} )", tuple, context );
			
			context.Buffer.Append( "[ " );
			context.EnterCollection();
			this._item1Formatter.FormatTo( tuple.Item1, context );
			context.Buffer.Append( ", " );
			this._item2Formatter.FormatTo( tuple.Item2, context );
			context.Buffer.Append( ", " );
			this._item3Formatter.FormatTo( tuple.Item3, context );
			context.Buffer.Append( ", " );
			this._item4Formatter.FormatTo( tuple.Item4, context );
		
			context.LeaveCollection();
			context.Buffer.Append( " ]" );				
		}
	}

	internal sealed class TupleFormatter<T1, T2, T3, T4, T5> :
		ItemFormatter<Tuple<T1, T2, T3, T4, T5>>
	{
		private readonly IItemFormatter<T1> _item1Formatter;
		private readonly IItemFormatter<T2> _item2Formatter;
		private readonly IItemFormatter<T3> _item3Formatter;
		private readonly IItemFormatter<T4> _item4Formatter;
		private readonly IItemFormatter<T5> _item5Formatter;
			
			// Combination of tuple is too many to cache.
		public TupleFormatter()	
		{
			this._item1Formatter = ItemFormatter.Get<T1>();
			this._item2Formatter = ItemFormatter.Get<T2>();
			this._item3Formatter = ItemFormatter.Get<T3>();
			this._item4Formatter = ItemFormatter.Get<T4>();
			this._item5Formatter = ItemFormatter.Get<T5>();
			
		}
			
		public sealed override void FormatTo(  Tuple<T1, T2, T3, T4, T5> tuple, FormattingContext context )
		{
			Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5>::FormatTo( {0}, {1} )", tuple, context );
			
			context.Buffer.Append( "[ " );
			context.EnterCollection();
			this._item1Formatter.FormatTo( tuple.Item1, context );
			context.Buffer.Append( ", " );
			this._item2Formatter.FormatTo( tuple.Item2, context );
			context.Buffer.Append( ", " );
			this._item3Formatter.FormatTo( tuple.Item3, context );
			context.Buffer.Append( ", " );
			this._item4Formatter.FormatTo( tuple.Item4, context );
			context.Buffer.Append( ", " );
			this._item5Formatter.FormatTo( tuple.Item5, context );
		
			context.LeaveCollection();
			context.Buffer.Append( " ]" );				
		}
	}

	internal sealed class TupleFormatter<T1, T2, T3, T4, T5, T6> :
		ItemFormatter<Tuple<T1, T2, T3, T4, T5, T6>>
	{
		private readonly IItemFormatter<T1> _item1Formatter;
		private readonly IItemFormatter<T2> _item2Formatter;
		private readonly IItemFormatter<T3> _item3Formatter;
		private readonly IItemFormatter<T4> _item4Formatter;
		private readonly IItemFormatter<T5> _item5Formatter;
		private readonly IItemFormatter<T6> _item6Formatter;
			
			// Combination of tuple is too many to cache.
		public TupleFormatter()	
		{
			this._item1Formatter = ItemFormatter.Get<T1>();
			this._item2Formatter = ItemFormatter.Get<T2>();
			this._item3Formatter = ItemFormatter.Get<T3>();
			this._item4Formatter = ItemFormatter.Get<T4>();
			this._item5Formatter = ItemFormatter.Get<T5>();
			this._item6Formatter = ItemFormatter.Get<T6>();
			
		}
			
		public sealed override void FormatTo(  Tuple<T1, T2, T3, T4, T5, T6> tuple, FormattingContext context )
		{
			Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6>::FormatTo( {0}, {1} )", tuple, context );
			
			context.Buffer.Append( "[ " );
			context.EnterCollection();
			this._item1Formatter.FormatTo( tuple.Item1, context );
			context.Buffer.Append( ", " );
			this._item2Formatter.FormatTo( tuple.Item2, context );
			context.Buffer.Append( ", " );
			this._item3Formatter.FormatTo( tuple.Item3, context );
			context.Buffer.Append( ", " );
			this._item4Formatter.FormatTo( tuple.Item4, context );
			context.Buffer.Append( ", " );
			this._item5Formatter.FormatTo( tuple.Item5, context );
			context.Buffer.Append( ", " );
			this._item6Formatter.FormatTo( tuple.Item6, context );
		
			context.LeaveCollection();
			context.Buffer.Append( " ]" );				
		}
	}

	internal sealed class TupleFormatter<T1, T2, T3, T4, T5, T6, T7> :
		ItemFormatter<Tuple<T1, T2, T3, T4, T5, T6, T7>>
	{
		private readonly IItemFormatter<T1> _item1Formatter;
		private readonly IItemFormatter<T2> _item2Formatter;
		private readonly IItemFormatter<T3> _item3Formatter;
		private readonly IItemFormatter<T4> _item4Formatter;
		private readonly IItemFormatter<T5> _item5Formatter;
		private readonly IItemFormatter<T6> _item6Formatter;
		private readonly IItemFormatter<T7> _item7Formatter;
			
			// Combination of tuple is too many to cache.
		public TupleFormatter()	
		{
			this._item1Formatter = ItemFormatter.Get<T1>();
			this._item2Formatter = ItemFormatter.Get<T2>();
			this._item3Formatter = ItemFormatter.Get<T3>();
			this._item4Formatter = ItemFormatter.Get<T4>();
			this._item5Formatter = ItemFormatter.Get<T5>();
			this._item6Formatter = ItemFormatter.Get<T6>();
			this._item7Formatter = ItemFormatter.Get<T7>();
			
		}
			
		public sealed override void FormatTo(  Tuple<T1, T2, T3, T4, T5, T6, T7> tuple, FormattingContext context )
		{
			Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7>::FormatTo( {0}, {1} )", tuple, context );
			
			context.Buffer.Append( "[ " );
			context.EnterCollection();
			this._item1Formatter.FormatTo( tuple.Item1, context );
			context.Buffer.Append( ", " );
			this._item2Formatter.FormatTo( tuple.Item2, context );
			context.Buffer.Append( ", " );
			this._item3Formatter.FormatTo( tuple.Item3, context );
			context.Buffer.Append( ", " );
			this._item4Formatter.FormatTo( tuple.Item4, context );
			context.Buffer.Append( ", " );
			this._item5Formatter.FormatTo( tuple.Item5, context );
			context.Buffer.Append( ", " );
			this._item6Formatter.FormatTo( tuple.Item6, context );
			context.Buffer.Append( ", " );
			this._item7Formatter.FormatTo( tuple.Item7, context );
		
			context.LeaveCollection();
			context.Buffer.Append( " ]" );				
		}
	}

	internal sealed class TupleFormatter<T1, T2, T3, T4, T5, T6, T7, T8> :
		ItemFormatter<Tuple<T1, T2, T3, T4, T5, T6, T7, T8>>
	{
		private readonly IItemFormatter<T1> _item1Formatter;
		private readonly IItemFormatter<T2> _item2Formatter;
		private readonly IItemFormatter<T3> _item3Formatter;
		private readonly IItemFormatter<T4> _item4Formatter;
		private readonly IItemFormatter<T5> _item5Formatter;
		private readonly IItemFormatter<T6> _item6Formatter;
		private readonly IItemFormatter<T7> _item7Formatter;
		private readonly IItemFormatter<T8> _item8Formatter;
			
			// Combination of tuple is too many to cache.
		public TupleFormatter()	
		{
			this._item1Formatter = ItemFormatter.Get<T1>();
			this._item2Formatter = ItemFormatter.Get<T2>();
			this._item3Formatter = ItemFormatter.Get<T3>();
			this._item4Formatter = ItemFormatter.Get<T4>();
			this._item5Formatter = ItemFormatter.Get<T5>();
			this._item6Formatter = ItemFormatter.Get<T6>();
			this._item7Formatter = ItemFormatter.Get<T7>();
			this._item8Formatter = ItemFormatter.Get<T8>();
			
		}
			
		public sealed override void FormatTo(  Tuple<T1, T2, T3, T4, T5, T6, T7, T8> tuple, FormattingContext context )
		{
			Debug.WriteLine( "TupleFormatter<T1, T2, T3, T4, T5, T6, T7, T8>::FormatTo( {0}, {1} )", tuple, context );
			
			context.Buffer.Append( "[ " );
			context.EnterCollection();
			this._item1Formatter.FormatTo( tuple.Item1, context );
			context.Buffer.Append( ", " );
			this._item2Formatter.FormatTo( tuple.Item2, context );
			context.Buffer.Append( ", " );
			this._item3Formatter.FormatTo( tuple.Item3, context );
			context.Buffer.Append( ", " );
			this._item4Formatter.FormatTo( tuple.Item4, context );
			context.Buffer.Append( ", " );
			this._item5Formatter.FormatTo( tuple.Item5, context );
			context.Buffer.Append( ", " );
			this._item6Formatter.FormatTo( tuple.Item6, context );
			context.Buffer.Append( ", " );
			this._item7Formatter.FormatTo( tuple.Item7, context );
			context.Buffer.Append( ", " );
			this._item8Formatter.FormatTo( tuple.Rest, context );
		
			context.LeaveCollection();
			context.Buffer.Append( " ]" );				
		}
	}
		
}