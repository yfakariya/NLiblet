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

using NLiblet.Reflection;

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
		private static readonly CharEscapingFilter _collectionItemFilter = CharEscapingFilter.UpperCaseDefaultCSharpLiteralStyle;

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
				return CharEscapingFilter.UpperCaseDefaultCSharpStyle;
			}

			switch ( format )
			{
				case "a":
				{
					return CharEscapingFilter.LowerCaseNonAsciiCSharpStyle;
				}
				case "A":
				{
					return CharEscapingFilter.UpperCaseNonAsciiCSharpStyle;
				}
				case "e":
				case "E":
				{
					return CharEscapingFilter.UnicodeStandard;
				}
				case "g":
				{
					goto case "m";
				}
				case "G":
				{
					goto case "M";
				}
				case "l":
				{
					return CharEscapingFilter.LowerCaseDefaultCSharpLiteralStyle;
				}
				case "L":
				{
					return CharEscapingFilter.UpperCaseDefaultCSharpLiteralStyle;
				}
				case "m":
				{
					return CharEscapingFilter.LowerCaseDefaultCSharpStyle;
				}
				case "M":
				{
					return CharEscapingFilter.UpperCaseDefaultCSharpStyle;
				}
				case "r":
				case "R":
				{
					return CharEscapingFilter.Null;
				}
				case "s":
				{
					return CharEscapingFilter.LowerCaseDefaultCSharpStyleSingleLine;
				}
				case "S":
				{
					return CharEscapingFilter.UpperCaseDefaultCSharpStyleSingleLine;
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

			if ( ( format ?? String.Empty ).Length > 1 && ( format[ 0 ] == 'u' || format[ 0 ] == 'U' ) && ( arg is Int32 ) )
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
			ItemFormatter.Get( arg.GetType() ).FormatTo( arg, new FormattingContext( this, format, buffer ) );
			return buffer.ToString();
		}

		private string FormatUtf32Char( string format, int c )
		{
			switch ( format ?? String.Empty )
			{
				case "b":
				case "B":
				{
					return UnicodeUtility.GetUnicodeBlockName( c );
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
					return UnicodeUtility.GetUnicodeBlockName( c );
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

		private static readonly Dictionary<RuntimeTypeHandle, bool> _numericTypes =
			new Dictionary<RuntimeTypeHandle, bool>()
			{
				{ typeof( byte ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( byte ) ) },
				{ typeof( sbyte ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( sbyte ) ) },
				{ typeof( short ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( short ) ) },
				{ typeof( ushort ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( ushort ) ) },
				{ typeof( int ).TypeHandle,	typeof( IFormattable ).IsAssignableFrom( typeof( int ) ) },
				{ typeof( uint ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( uint ) ) },
				{ typeof( long ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( long ) ) },
				{ typeof( ulong ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( ulong ) ) },
				{ typeof( float  ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( float  ) ) },
				{ typeof( double ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( double ) ) },
				{ typeof( decimal ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( decimal ) ) },
				{ typeof( IntPtr ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( IntPtr ) ) },
				{ typeof( UIntPtr ).TypeHandle,	typeof( IFormattable ).IsAssignableFrom( typeof( UIntPtr ) ) },
				{ typeof( BigInteger ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( BigInteger ) ) },
				{ typeof( Complex ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( Complex ) ) },
			};

		private static bool IsNumerics( RuntimeTypeHandle typeHandle )
		{
			return _numericTypes.ContainsKey( typeHandle );
		}

		private sealed class FormattingContext
		{
			private readonly string _format;

			/// <summary>
			///		Get format string specified to Format().
			/// </summary>
			public string Format
			{
				get { return this._format; }
			}

			private readonly IFormatProvider _fallbackProvider;

			/// <summary>
			///		Get fallback provider which was passed on constructor. This value may be CultureInfo.
			/// </summary>
			public IFormatProvider FallbackProvider
			{
				get { return this._formatter._defaultFormatProvider; }
			}

			private readonly StringBuilder _buffer;

			public StringBuilder Buffer
			{
				get { return this._buffer; }
			}

			private readonly CommonCustomFormatter _formatter;
			public CommonCustomFormatter Formatter
			{
				get { return this._formatter; }
			}

			public FormattingContext( CommonCustomFormatter formatter, string format, StringBuilder buffer )
			{
				Contract.Requires( formatter != null );

				this._formatter = formatter;
				this._format = format;
				this._buffer = buffer;
			}
		}

		private abstract class ItemFormatter
		{
			public static ItemFormatter Get( Type itemType )
			{
				if ( itemType.TypeHandle.Equals( typeof( object ).TypeHandle ) )
				{
					return ObjectFormatter.Instance;
				}

				// TODO: caching
				return Activator.CreateInstance( typeof( ItemFormatter<> ).MakeGenericType( itemType ) ) as ItemFormatter;
			}

			public abstract void FormatTo( object item, FormattingContext context );
		}

		private sealed class ObjectFormatter : ItemFormatter
		{
			public static readonly ObjectFormatter Instance = new ObjectFormatter();

			private ObjectFormatter() { }

			public override void FormatTo( object item, FormattingContext context )
			{
				if ( Object.ReferenceEquals( item, null ) )
				{
					context.Buffer.Append( _nullRepresentation );
				}
				else
				{
					context.Buffer.Append( '\"' ).Append( item.ToString() ).Append( "\"" );
				}
			}
		}

		private sealed class ItemFormatter<T> : ItemFormatter
		{
			public static readonly Action<T, FormattingContext> Action;

			static ItemFormatter()
			{
				const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.NonPublic;

				if ( typeof( T ).TypeHandle.Equals( typeof( string ).TypeHandle )
					|| typeof( T ).TypeHandle.Equals( typeof( StringBuilder ).TypeHandle ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( ItemFormatter<T> ).GetMethod( "FormatStringTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( T ).TypeHandle.Equals( typeof( bool ).TypeHandle ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( ItemFormatter<T> ).GetMethod( "FormatBoleanTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( IsNumerics( typeof( T ).TypeHandle ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( ItemFormatter<T> ).GetMethod( "FormatNumericTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( IFormattable ).IsAssignableFrom( typeof( T ) ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( ItemFormatter<T> ).GetMethod( "FormatFormattableTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( T ).Implements( typeof( IDictionary<,> ) ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( ItemFormatter<T> ).GetMethod( "FormatGenericDictionaryTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( T ).Implements( typeof( IEnumerable<> ) ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( ItemFormatter<T> ).GetMethod( "FormatGenericEnumerableTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( IDictionary ).IsAssignableFrom( typeof( T ) ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( ItemFormatter<T> ).GetMethod( "FormatNonGenericDictionaryTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( IEnumerable ).IsAssignableFrom( typeof( T ) ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( ItemFormatter<T> ).GetMethod( "FormatNonGenericEnumerableTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				Action =
					Delegate.CreateDelegate(
						typeof( Action<T, FormattingContext> ),
						typeof( ItemFormatter<T> ).GetMethod( "FormatTo", bindingFlags )
					) as Action<T, FormattingContext>;
				return;
			}

			private static void FormatBoleanTo( bool item, FormattingContext context )
			{
				context.Buffer.Append( item ? "true" : "false" );
			}

			private static void FormatNumericTo( T item, FormattingContext context )
			{
				context.Buffer.Append( item );
			}

			private static void FormatStringTo( T item, FormattingContext context )
			{
				context.Buffer.Append( '\"' );
				foreach ( var c in EscapeChars( item ) )
				{
					context.Buffer.Append( c );
				}
				context.Buffer.Append( '\"' );
			}

			private static IEnumerable<char> EscapeChars( T item )
			{
				// TODO: Consider custom escaping... ?

				Contract.Assert( typeof( T ).TypeHandle.Equals( typeof( string ).TypeHandle ) || typeof( T ).TypeHandle.Equals( typeof( StringBuilder ).TypeHandle ) );

				var asString = item as string;
				if ( asString != null )
				{
					return _collectionItemFilter.Escape( asString );
				}

				var asStringBuilder = item as StringBuilder;
				if ( asStringBuilder != null )
				{
					return asStringBuilder.EnumerateChars();
				}

				return Arrays.Empty<char>();
			}

			private static void FormatFormattableTo( T item, FormattingContext context )
			{
				if ( item == null )
				{
					context.Buffer.Append( _nullRepresentation );
				}
				else
				{
					context.Buffer.Append( '"' );
					foreach ( var c in _collectionItemFilter.Escape( ( item as IFormattable ).ToString( context.Format, context.FallbackProvider ) ) )
					{
						context.Buffer.Append( c );
					}
					context.Buffer.Append( '"' );
				}
			}

			private static void FormatTo( T item, FormattingContext context )
			{
				if ( Object.ReferenceEquals( item, null ) )
				{
					context.Buffer.Append( _nullRepresentation );
				}
				else
				{
					ItemFormatter.Get( item.GetType() ).FormatTo( item, context );
				}
			}

			private static void FormatNonGenericEnumerableTo( T item, FormattingContext context )
			{
				Contract.Assert( item is IEnumerable );

				context.Buffer.Append( "[ " );

				bool isFirstEntry = true;
				foreach ( var entry in ( item as IEnumerable ) )
				{
					if ( !isFirstEntry )
					{
						context.Buffer.Append( ", " );
					}

					if ( Object.ReferenceEquals( entry, null ) )
					{
						context.Buffer.Append( _nullRepresentation );
					}
					else
					{
						ItemFormatter.Get( entry.GetType() ).FormatTo( entry, context );
					}

					isFirstEntry = false;
				}

				context.Buffer.Append( " ]" );
			}

			private static void FormatGenericEnumerableTo( T item, FormattingContext context )
			{
				Contract.Assert( typeof( T ).Implements( typeof( IEnumerable<> ) ) );

				var ienumerable =
					typeof( T ).GetInterfaces()
					.First( @interface => @interface.IsGenericType
						&& !@interface.IsGenericTypeDefinition
						&& @interface.Name == typeof( IEnumerable<> ).Name
					);

				var genericArguments = ienumerable.GetGenericArguments();
				Contract.Assert( genericArguments.Length == 1 );

				SequenceFormatter.Get( genericArguments[ 0 ] ).FormatTo( item, context );
			}

			private static void FormatNonGenericDictionaryTo( T item, FormattingContext context )
			{
				Contract.Assert( item is IEnumerable );

				context.Buffer.Append( "{ " );

				bool isFirstEntry = true;
				foreach ( DictionaryEntry entry in ( item as IEnumerable ) )
				{
					if ( !isFirstEntry )
					{
						context.Buffer.Append( ", " );
					}

					if ( Object.ReferenceEquals( entry.Key, null ) )
					{
						context.Buffer.Append( _nullRepresentation );
					}
					else
					{
						ItemFormatter.Get( entry.Key.GetType() ).FormatTo( entry.Key, context );
					}

					context.Buffer.Append( " : " );

					if ( Object.ReferenceEquals( entry.Value, null ) )
					{
						context.Buffer.Append( _nullRepresentation );
					}
					else
					{
						ItemFormatter.Get( entry.Value.GetType() ).FormatTo( entry.Value, context );
					}

					isFirstEntry = false;
				}

				context.Buffer.Append( " }" );
			}

			private static void FormatGenericDictionaryTo( T item, FormattingContext context )
			{
				Contract.Assert( typeof( T ).Implements( typeof( IDictionary<,> ) ) );

				var idictionary =
					typeof( T ).GetInterfaces()
					.First( @interface => @interface.IsGenericType
						&& !@interface.IsGenericTypeDefinition
						&& @interface.Name == typeof( IDictionary<,> ).Name
					);

				var genericArguments = idictionary.GetGenericArguments();
				Contract.Assert( genericArguments.Length == 2 );

				DictionaryFormatter.Get( genericArguments[ 0 ], genericArguments[ 1 ] ).FormatTo( item, context );
			}

			public ItemFormatter() { }

			public override void FormatTo( object item, FormattingContext context )
			{
				Action( ( T )item, context );
			}
		}
		private abstract class SequenceFormatter
		{
			public static SequenceFormatter Get( Type itemType )
			{
				// TODO: caching
				return Activator.CreateInstance( typeof( SequenceFormatter<> ).MakeGenericType( itemType ) ) as SequenceFormatter;
			}

			public abstract void FormatTo( object sequence, FormattingContext context );
		}

		private sealed class SequenceFormatter<TItem> : SequenceFormatter
		{
			private readonly Action<TItem, FormattingContext> _itemFormatter = ItemFormatter<TItem>.Action;

			public SequenceFormatter() { }

			public override void FormatTo( object sequence, FormattingContext context )
			{
				Contract.Assert( sequence is IEnumerable<TItem> );

				context.Buffer.Append( "[ " );

				var asEnumerable = sequence as IEnumerable<TItem>;
				if ( asEnumerable != null )
				{
					bool isFirstEntry = true;
					foreach ( var entry in asEnumerable )
					{
						if ( !isFirstEntry )
						{
							context.Buffer.Append( ", " );
						}

						this._itemFormatter( entry, context );

						isFirstEntry = false;
					}
				}

				context.Buffer.Append( " ]" );
			}
		}


		private abstract class DictionaryFormatter
		{
			public static DictionaryFormatter Get( Type keyType, Type valueType )
			{
				// TODO: caching
				return Activator.CreateInstance( typeof( DictionaryFormatter<,> ).MakeGenericType( keyType, valueType ) ) as DictionaryFormatter;
			}

			public abstract void FormatTo( object dictionary, FormattingContext context );
		}

		private sealed class DictionaryFormatter<TKey, TValue> : DictionaryFormatter
		{
			private readonly Action<TKey, FormattingContext> _keyFormatter = ItemFormatter<TKey>.Action;
			private readonly Action<TValue, FormattingContext> _valueFormatter = ItemFormatter<TValue>.Action;

			public DictionaryFormatter() { }

			public override void FormatTo( object dictionary, FormattingContext context )
			{
				context.Buffer.Append( "{ " );

				var asDictionary = dictionary as IDictionary<TKey, TValue>;
				if ( asDictionary != null )
				{
					bool isFirstEntry = true;
					foreach ( var entry in asDictionary )
					{
						if ( !isFirstEntry )
						{
							context.Buffer.Append( ", " );
						}

						this._keyFormatter( entry.Key, context );
						context.Buffer.Append( " : " );
						this._valueFormatter( entry.Value, context );
						isFirstEntry = false;
					}
				}

				context.Buffer.Append( " }" );
			}
		}
	}
}
