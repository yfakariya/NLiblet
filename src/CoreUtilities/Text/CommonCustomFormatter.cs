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
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Collections;
using System.Numerics;
using System.Reflection;

namespace NLiblet.Text
{

	/*
	 * format
	 *	a : ascii; non-ascii chars are escaped
	 *	b : Unicode block name
	 *	c : Unicode Category
	 *	d : utf-32 Decimal
	 *	e : Escaping non printable chars with U+FFFD
	 *  g : General; same as 'm'
	 *	l : Literal style
	 *	m : Multi line escaped char with \uxxxx notation
	 *	r : Raw-char without any escaping
	 *	s : Single line escaped char with \uxxxx notation
	 *	u : Utf-32 hex, a-f will be lowercase(reserved)
	 *	U : Utf-32 hex, a-f will be uppercase(reserved)
	 *	x : utf-16 heX, a-f will be lowercase
	 *	X : utf-16 heX, a-f will be uppercase
	 *	
	 * Collection items are always escaped with 'L'
	 */

	internal sealed class CommonCustomFormatter : ICustomFormatter, IFormatProvider
	{
		private const string _nullRepresentation = "null";

		private readonly IFormatProvider _defaultFormatProvider;

		public CommonCustomFormatter( IFormatProvider defaultFormatProvider )
		{
			Contract.Requires<ArgumentNullException>( defaultFormatProvider != null );

			this._defaultFormatProvider = defaultFormatProvider;
		}

		object IFormatProvider.GetFormat( Type formatType )
		{
			if ( typeof( ICustomFormatter ) == formatType )
			{
				return this;
			}
			else
			{
				return this._defaultFormatProvider.GetFormat( formatType );
			}
		}

		private static CharEscapingFilter GetCharEscapingFilter( string format )
		{
			if ( String.IsNullOrWhiteSpace( format ) )
			{
				return CharEscapingFilter.DefaultCSharpStyle;
			}

			switch ( format )
			{
				case "a":
				case "A":
				{
					return CharEscapingFilter.NonAsciiCSharpStyle;
				}
				case "e":
				case "E":
				{
					return CharEscapingFilter.UnicodeStandard;
				}
				case "g":
				case "G":
				{
					goto case "m";
				}
				case "l":
				case "L":
				{
					return CharEscapingFilter.DefaultCSharpLiteralStyle;
				}
				case "m":
				case "M":
				{
					return CharEscapingFilter.DefaultCSharpStyle;
				}
				case "r":
				case "R":
				{
					return CharEscapingFilter.Null;
				}
				case "s":
				case "S":
				{
					return CharEscapingFilter.DefaultCSharpStyleSingleLine;
				}
				default:
				{
					throw new FormatException( String.Format( CultureInfo.CurrentCulture, "Unknown format '{0}'.", format ) );
				}
			}
		}

		public string Format( string format, object arg, IFormatProvider formatProvider )
		{
			if ( arg == null )
			{
				return _nullRepresentation;
			}

			if ( arg is char )
			{
				return FormatChar( format, ( char )arg );
			}

			if ( ( format ?? String.Empty ).Length > 1 && ( format[ 0 ] == 'u' || format[ 1 ] == 'U' ) && ( arg is Int32 ) )
			{
				var asInt32 = ( int )arg;
				if ( 0 <= asInt32 && asInt32 <= 0x10FFFF )
				{
					return FormatUtf32Char( format.Substring( 1 ), asInt32 );
				}
				else
				{
					throw new FormatException( String.Format( CultureInfo.CurrentCulture, "Invalid code point : 0x{0}", asInt32 ) );
				}
			}

			var buffer = new StringBuilder();
			ItemFormatter.Get( arg.GetType() ).FormatTo( arg, format, formatProvider, buffer );
			return buffer.ToString();
		}

		private string FormatUtf32Char( string format, int c )
		{
			switch ( format ?? String.Empty )
			{
				case "b":
				case "B":
				{
					throw new NotImplementedException();
				}
				case "c":
				case "C":
				{
					return UnicodeUtility.GetUnicodeCategory( c ).ToString();
				}
				case "d":
				case "D":
				{
					return ( c ).ToString( "d" );
				}
				case "u":
				case "U":
				{
					throw new FormatException( "UTF-16 sequence cannot be UTF-32 sequence." );
				}
				case "x":
				{
					return ( c ).ToString( "x" );
				}
				case "X":
				{
					return ( c ).ToString( "X" );
				}
			}

			return String.Join( String.Empty, GetCharEscapingFilter( format ).Escape( Enumerable.Repeat( c, 1 ) ) );
		}

		private static string FormatChar( string format, char c )
		{
			switch ( format ?? String.Empty )
			{
				case "b":
				case "B":
				{
					throw new NotImplementedException();
				}
				case "c":
				case "C":
				{
					return CharUnicodeInfo.GetUnicodeCategory( c ).ToString();
				}
				case "d":
				case "D":
				{
					return ( ( int )c ).ToString( "d" );
				}
				case "u":
				case "x":
				{
					return ( ( int )c ).ToString( "x" );
				}
				case "U":
				case "X":
				{
					return ( ( int )c ).ToString( "X" );
				}
				default:
				{
					return String.Join( String.Empty, GetCharEscapingFilter( format ).Escape( Enumerable.Repeat( c, 1 ) ) );
				}
			}
		}

		private static IEnumerable<char> EnumerateStringBuilder( StringBuilder stringBuilder )
		{
			for ( int i = 0; i < stringBuilder.Length; i++ )
			{
				yield return stringBuilder[ i ];
			}
		}

		private static readonly HashSet<RuntimeTypeHandle> _numericTypes =
			new HashSet<RuntimeTypeHandle>()
			{
				typeof( byte ).TypeHandle, 
				typeof( sbyte ).TypeHandle,
				typeof( short ).TypeHandle,
				typeof( ushort ).TypeHandle,
				typeof( int ).TypeHandle,
				typeof( uint ).TypeHandle,
				typeof( long ).TypeHandle,
				typeof( ulong ).TypeHandle,
				typeof( float  ).TypeHandle,
				typeof( double ).TypeHandle,
				typeof( decimal ).TypeHandle,
				typeof( IntPtr ).TypeHandle,
				typeof( UIntPtr ).TypeHandle,
				typeof( BigInteger ).TypeHandle,
				typeof( Complex ).TypeHandle
			};

		private static bool IsNumerics( RuntimeTypeHandle typeHandle )
		{
			return _numericTypes.Contains( typeHandle );
		}

		private abstract class ItemFormatter
		{
			public static ItemFormatter Get( Type itemType )
			{
				// TODO: caching
				return Activator.CreateInstance( typeof( ItemFormatter<> ).MakeGenericType( itemType ) ) as ItemFormatter;
			}

			public abstract void FormatTo( object item, string format, IFormatProvider formatProvider, StringBuilder buffer );
		}

		private sealed class ItemFormatter<T> : ItemFormatter
		{
			public static readonly Action<T, String, IFormatProvider, StringBuilder> Action;

			static ItemFormatter()
			{
				const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.NonPublic;

				if ( typeof( T ).TypeHandle.Equals( typeof( bool ).TypeHandle ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, String, IFormatProvider, StringBuilder> ),
							typeof( ItemFormatter<T> ).GetMethod( "FormatBoleanTo", bindingFlags )
						) as Action<T, String, IFormatProvider, StringBuilder>;
					return;
				}

				if ( IsNumerics( typeof( T ).TypeHandle ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, String, IFormatProvider, StringBuilder> ),
							typeof( ItemFormatter<T> ).GetMethod( "FormatNumericTo", bindingFlags )
						) as Action<T, String, IFormatProvider, StringBuilder>;
					return;
				}

				if ( typeof( IFormattable ).IsAssignableFrom( typeof( T ) ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, String, IFormatProvider, StringBuilder> ),
							typeof( ItemFormatter<T> ).GetMethod( "FormatFormattableTo", bindingFlags )
						) as Action<T, String, IFormatProvider, StringBuilder>;
					return;
				}

				if ( typeof( IDictionary ).IsAssignableFrom( typeof( T ) ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, String, IFormatProvider, StringBuilder> ),
							typeof( ItemFormatter<T> ).GetMethod( "FormatNonGenericDictionaryTo", bindingFlags )
						) as Action<T, String, IFormatProvider, StringBuilder>;
					return;
				}

				if ( typeof( T ).IsGenericType )
				{
					if ( typeof( T ).GetGenericTypeDefinition().TypeHandle.Equals( typeof( IEnumerable<> ).TypeHandle ) )
					{
						Action =
							Delegate.CreateDelegate(
								typeof( Action<T, String, IFormatProvider, StringBuilder> ),
								typeof( ItemFormatter<T> ).GetMethod( "FormatGenericEnumerableTo", bindingFlags )
							) as Action<T, String, IFormatProvider, StringBuilder>;
						return;
					}

					if ( typeof( T ).GetGenericTypeDefinition().TypeHandle.Equals( typeof( IDictionary<,> ).TypeHandle ) )
					{
						Action =
							Delegate.CreateDelegate(
								typeof( Action<T, String, IFormatProvider, StringBuilder> ),
								typeof( ItemFormatter<T> ).GetMethod( "FormatGenericDictionaryTo", bindingFlags )
							) as Action<T, String, IFormatProvider, StringBuilder>;
						return;
					}
				}

				if ( typeof( IEnumerable ).IsAssignableFrom( typeof( T ) ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, String, IFormatProvider, StringBuilder> ),
							typeof( ItemFormatter<T> ).GetMethod( "FormatNonGenericEnumerableTo", bindingFlags )
						) as Action<T, String, IFormatProvider, StringBuilder>;
					return;
				}

				Action =
					Delegate.CreateDelegate(
						typeof( Action<T, String, IFormatProvider, StringBuilder> ),
						typeof( ItemFormatter<T> ).GetMethod( "FormatTo", bindingFlags )
					) as Action<T, String, IFormatProvider, StringBuilder>;
				return;
			}

			private static void FormatBoleanTo( bool item, string format, IFormatProvider formatProvider, StringBuilder buffer )
			{
				buffer.Append( item ? "true" : "false" );
			}

			private static void FormatNumericTo( T item, string format, IFormatProvider formatProvider, StringBuilder buffer )
			{
				buffer.Append( item );
			}

			private static void FormatFormattableTo( T item, string format, IFormatProvider formatProvider, StringBuilder buffer )
			{
				if ( item == null )
				{
					buffer.Append( "null" );
				}
				else
				{
					buffer.Append( '"' );
					foreach ( var c in CharEscapingFilter.DefaultCSharpLiteralStyle.Escape( ( item as IFormattable ).ToString( format, formatProvider ) ) )
					{
						buffer.Append( c );
					}
					buffer.Append( '"' );
				}
			}

			private static void FormatTo( T item, string format, IFormatProvider formatProvider, StringBuilder buffer )
			{
				if ( item == null )
				{
					buffer.Append( "null" );
				}
				else
				{
					buffer.Append( '"' );
					foreach ( var c in CharEscapingFilter.DefaultCSharpLiteralStyle.Escape( item.ToString() ) )
					{
						buffer.Append( c );
					}
					buffer.Append( '"' );
				}
			}

			private static void FormatNonGenericEnumerableTo( T item, string format, IFormatProvider formatProvider, StringBuilder buffer )
			{
				Contract.Assert( item is IEnumerable );

				buffer.Append( "[ " );

				bool isFirstEntry = true;
				foreach ( var entry in ( item as IEnumerable ) )
				{
					if ( !isFirstEntry )
					{
						buffer.Append( ", " );
					}

					if ( Object.ReferenceEquals( entry, null ) )
					{
						buffer.Append( _nullRepresentation );
					}
					else
					{
						ItemFormatter.Get( entry.GetType() ).FormatTo( entry, format, formatProvider, buffer );
					}

					isFirstEntry = false;
				}

				buffer.Append( " ]" );
			}

			private static void FormatGenericEnumerableTo( T item, string format, IFormatProvider formatProvider, StringBuilder buffer )
			{
				Contract.Assert( typeof( T ).IsGenericType );
				Contract.Assert( typeof( IEnumerable<> ).TypeHandle.Equals( typeof( T ).GetGenericTypeDefinition().TypeHandle ) );
				var genericArguments = typeof( T ).GetGenericTypeDefinition().GetGenericArguments();
				Contract.Assert( genericArguments.Length == 1 );

				SequenceFormatter.Get( genericArguments[ 0 ] ).FormatTo( item, format, formatProvider, buffer );
			}

			private static void FormatNonGenericDictionaryTo( T item, string format, IFormatProvider formatProvider, StringBuilder buffer )
			{
				Contract.Assert( item is IEnumerable );

				buffer.Append( "{ " );

				bool isFirstEntry = true;
				foreach ( DictionaryEntry entry in ( item as IEnumerable ) )
				{
					if ( !isFirstEntry )
					{
						buffer.Append( ", " );
					}

					ItemFormatter.Get( entry.Key.GetType() ).FormatTo( entry.Key, format, formatProvider, buffer );

					buffer.Append( " : " );

					ItemFormatter.Get( entry.Value.GetType() ).FormatTo( entry.Value, format, formatProvider, buffer );

					isFirstEntry = false;
				}

				buffer.Append( " }" );
			}

			private static void FormatGenericDictionaryTo( T item, string format, IFormatProvider formatProvider, StringBuilder buffer )
			{
				Contract.Assert( typeof( T ).IsGenericType );
				Contract.Assert( typeof( IDictionary<,> ).TypeHandle.Equals( typeof( T ).GetGenericTypeDefinition().TypeHandle ) );
				var genericArguments = typeof( T ).GetGenericTypeDefinition().GetGenericArguments();
				Contract.Assert( genericArguments.Length == 2 );

				DictionaryFormatter.Get( genericArguments[ 0 ], genericArguments[ 1 ] ).FormatTo( item, format, formatProvider, buffer );
			}

			public ItemFormatter() { }

			public override void FormatTo( object item, string format, IFormatProvider formatProvider, StringBuilder buffer )
			{
				Action( ( T )item, format, formatProvider, buffer );
			}

			private abstract class SequenceFormatter
			{
				public static SequenceFormatter Get( Type itemType )
				{
					// TODO: caching
					return Activator.CreateInstance( typeof( SequenceFormatter<> ).MakeGenericType( itemType ) ) as SequenceFormatter;
				}

				public abstract void FormatTo( object sequence, string format, IFormatProvider formatProvider, StringBuilder buffer );
			}

			private sealed class SequenceFormatter<TItem> : SequenceFormatter
			{
				private readonly Action<TItem, String, IFormatProvider, StringBuilder> _itemFormatter = ItemFormatter<TItem>.Action;

				public SequenceFormatter() { }

				public override void FormatTo( object sequence, string format, IFormatProvider formatProvider, StringBuilder buffer )
				{
					Contract.Assert( sequence is IEnumerable<TItem> );
					buffer.Append( "[ " );

					var asEnumerable = sequence as IEnumerable<TItem>;
					if ( asEnumerable != null )
					{
						bool isFirstEntry = true;
						foreach ( var entry in asEnumerable )
						{
							if ( !isFirstEntry )
							{
								buffer.Append( ", " );
							}

							this._itemFormatter( entry, format, formatProvider, buffer );

							isFirstEntry = false;
						}
					}

					buffer.Append( " ]" );
				}
			}


			private abstract class DictionaryFormatter
			{
				public static DictionaryFormatter Get( Type keyType, Type valueType )
				{
					// TODO: caching
					return Activator.CreateInstance( typeof( DictionaryFormatter<,> ).MakeGenericType( keyType, valueType ) ) as DictionaryFormatter;
				}

				public abstract void FormatTo( object dictionary, string format, IFormatProvider formatProvider, StringBuilder buffer );
			}

			private sealed class DictionaryFormatter<TKey, TValue> : DictionaryFormatter
			{
				private readonly Action<TKey, String, IFormatProvider, StringBuilder> _keyFormatter = ItemFormatter<TKey>.Action;
				private readonly Action<TValue, String, IFormatProvider, StringBuilder> _valueFormatter = ItemFormatter<TValue>.Action;

				public DictionaryFormatter() { }

				public override void FormatTo( object dictionary, string format, IFormatProvider formatProvider, StringBuilder buffer )
				{
					buffer.Append( "{ " );

					var asDictionary = dictionary as IDictionary<TKey, TValue>;
					if ( asDictionary != null )
					{
						bool isFirstEntry = true;
						foreach ( var entry in asDictionary )
						{
							if ( !isFirstEntry )
							{
								buffer.Append( ", " );
							}

							this._keyFormatter( entry.Key, format, formatProvider, buffer );
							buffer.Append( " : " );
							this._valueFormatter( entry.Value, format, formatProvider, buffer );
							isFirstEntry = false;
						}
					}

					buffer.Append( " }" );
				}
			}
		}
	}
}
