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
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using NLiblet.Reflection;
using System.Runtime.Serialization;

namespace NLiblet.Text
{
	// TODO: refactor

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
	 *	u : Treat integer as utf-32 hex, a-f will be lowercase, or utf-16 hex with 0 padding and a-f will be lowercase.
	 *	u : Treat integer as utf-32 hex, a-f will be lowercase, or utf-16 hex with 0 padding and a-f will be uppercase.
	 *	x : utf-16 heX, a-f will be lowercase
	 *	X : utf-16 heX, a-f will be uppercase
	 *	
	 * Collection items are always escaped with 'L'
	 */

	// FIXME: should not be partial
	/// <summary>
	///		Implementing <see cref="ICustomFormatter"/> and <see cref="IFormatProvider"/>.
	/// </summary>
	internal sealed partial class CommonCustomFormatter : ICustomFormatter, IFormatProvider
	{
		private const string _nullRepresentation = "null";
		private static readonly CharEscapingFilter _collectionItemFilter = CharEscapingFilter.UpperCaseDefaultCSharpLiteralStyle;

		private readonly IFormatProvider _defaultFormatProvider;

		internal IFormatProvider DefaultFormatProvider
		{
			get { return this._defaultFormatProvider; }
		}

		/// <summary>
		///		Initializes a new instance of the <see cref="CommonCustomFormatter"/> class.
		/// </summary>
		/// <param name="defaultFormatProvider">Format provider to format <see cref="IFormattable"/> items.</param>
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
				// Avoid normal placeholders are escaped.
				return CharEscapingFilter.Null;
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

			var asString = arg as string;
			if ( asString != null )
			{
				return String.Join( String.Empty, GetCharEscapingFilter( format ).Escape( asString ) );
			}

			var asStringBuilder = arg as StringBuilder;
			if ( asStringBuilder != null )
			{
				return String.Join( String.Empty, GetCharEscapingFilter( format ).Escape( asStringBuilder.AsEnumerable() ) );
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
					throw new FormatException( String.Format( CultureInfo.CurrentCulture, Properties.Resources.Error_InvalidCodePoint, asInt32 ) );
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
				{
					return ( c ).ToString( "x4" );
				}
				case "U":
				{
					return ( c ).ToString( "X4" );
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
				{
					return ( ( int )c ).ToString( "x4" );
				}
				case "x":
				{
					return ( ( int )c ).ToString( "x" );
				}
				case "U":
				{
					return ( ( int )c ).ToString( "X4" );
				}
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

		// BigInteger, Decimal and Complex is not considered as numerics because ECMA Script spec specifies that numeric is Double,
		// so BigInteger will be overflowed, decimal will lose its precision, and complex will not be able to express.
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
				{ typeof( IntPtr ).TypeHandle, typeof( IFormattable ).IsAssignableFrom( typeof( IntPtr ) ) },
				{ typeof( UIntPtr ).TypeHandle,	typeof( IFormattable ).IsAssignableFrom( typeof( UIntPtr ) ) },
			};

		/// <summary>
		///		Determine whether specified type is numeric.
		/// </summary>
		/// <param name="typeHandle">Type handle.</param>
		/// <param name="isFormattable">Set true if <paramref name="typeHandle"/> is formattable.</param>
		/// <returns><c>true</c> if sepcified type is numerics.</returns>
		private static bool IsNumerics( RuntimeTypeHandle typeHandle, out bool isFormattable )
		{
			return _numericTypes.TryGetValue( typeHandle, out isFormattable );
		}

		/// <summary>
		///		Consolidates context information.
		/// </summary>
		internal sealed class FormattingContext
		{
			private readonly string _format;

			/// <summary>
			///		Get format string specified to Format().
			/// </summary>
			public string Format
			{
				get { return this._format; }
			}

			/// <summary>
			///		Get fallback provider which was passed on constructor. This value may be CultureInfo.
			/// </summary>
			public IFormatProvider FallbackProvider
			{
				get { return this._formatter._defaultFormatProvider; }
			}

			private readonly StringBuilder _buffer;

			/// <summary>
			///		Get buffer to append formatting result.
			/// </summary>
			public StringBuilder Buffer
			{
				get { return this._buffer; }
			}

			private readonly CommonCustomFormatter _formatter;

			/// <summary>
			///		Get the reference to current <see cref="CommonCustomFormatter"/>.
			/// </summary>
			public CommonCustomFormatter Formatter
			{
				get { return this._formatter; }
			}

			private int _isInCollection;

			public bool IsInCollection
			{
				get { return this._isInCollection > 0; }
			}

			public void EnterCollection()
			{
				this._isInCollection++;
			}

			public void LeaveCollection()
			{
				this._isInCollection--;
			}

			public FormattingContext( CommonCustomFormatter formatter, string format, StringBuilder buffer )
			{
				Contract.Requires( formatter != null );

				this._formatter = formatter;
				this._format = format;
				this._buffer = buffer;
			}

			public override string ToString()
			{
				return String.Format( CultureInfo.InvariantCulture, "{{ Format:'{0}', Formatter:@{1:x8}, Buffer.Length:{2:#,##0}, IsInCollection:{3} }}", this._format, RuntimeHelpers.GetHashCode( this._formatter ), this._buffer.Length, this._isInCollection );
			}
		}

		/// <summary>
		///		Define non-geneneric entry points for item formatting.
		/// </summary>
		private abstract class ItemFormatter
		{
			/// <summary>
			///		Get appropriate formatter.
			/// </summary>
			/// <param name="itemType">Type of item.</param>
			/// <returns>Appropriate formatter.</returns>
			public static ItemFormatter Get( Type itemType )
			{
				Contract.Requires( itemType != null );
				Contract.Ensures( Contract.Result<ItemFormatter>() != null );

				if ( itemType.TypeHandle.Equals( typeof( object ).TypeHandle ) )
				{
					// Avoid infinite recursion.
					Debug.WriteLine( "ItemFormatter::Get( {0} ) -> {1}", itemType, typeof( ObjectFormatter ) );
					return ObjectFormatter.Instance;
				}

				// TODO: caching
				var result = Activator.CreateInstance( typeof( GenericItemFormatter<> ).MakeGenericType( itemType ) ) as ItemFormatter;
				Debug.WriteLine( "ItemFormatter::Get( {0} ) -> {1}(Action:{2})", itemType.GetFullName(), result.GetType().GetName(), ( result.GetType().GetField( "Action" ).GetValue( null ) as Delegate ).Method );
				return result;
			}

			/// <summary>
			///		Format specified item using context.
			/// </summary>
			/// <param name="item">Item to be formatted.</param>
			/// <param name="context">Context information.</param>
			public abstract void FormatTo( object item, FormattingContext context );

			public static bool IsRuntimeCheckNeeded( Type type )
			{
				return typeof( object ).TypeHandle.Equals( type.TypeHandle ) || typeof( ValueType ).TypeHandle.Equals( type.TypeHandle );
			}

			// FIXME: Should be other class
			internal static IEnumerable<char> EscapeChars( object item )
			{
				// TODO: Consider custom escaping... ?

				Contract.Assert(
					item is string
					|| item is StringBuilder
					|| item is IEnumerable<char>
				);

				var asEnumerable = item as IEnumerable<char>;
				if ( asEnumerable != null )
				{
					return _collectionItemFilter.Escape( asEnumerable );
				}

				var asStringBuilder = item as StringBuilder;
				if ( asStringBuilder != null )
				{
					return asStringBuilder.AsEnumerable();
				}

				return Empty.Array<char>();
			}

		}

		/// <summary>
		///		<see cref="ItemFormatter"/> specialized for <see cref="Object"/>.
		/// </summary>
		private sealed class ObjectFormatter : ItemFormatter
		{
			public static readonly ObjectFormatter Instance = new ObjectFormatter();

			private ObjectFormatter() { }

			public override void FormatTo( object item, FormattingContext context )
			{
				Debug.WriteLine( "ObjectFormatter::FormatTo( {0} : {1}, {2} )", item, item == null ? "(unknown)" : item.GetType().FullName, context );

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

		/// <summary>
		///		Type specific <see cref="ItemFormatter"/> implementation.
		/// </summary>
		/// <typeparam name="T">Type of item.</typeparam>
		private sealed partial class GenericItemFormatter<T> : ItemFormatter
		{
			public static readonly Action<T, FormattingContext> Action;

			/// <summary>
			///		Initialize closed type specialized for <typeparamref name="T"/> type.
			/// </summary>
			static GenericItemFormatter()
			{
				const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.NonPublic;

				if ( typeof( T ).TypeHandle.Equals( typeof( string ).TypeHandle )
					|| typeof( T ).TypeHandle.Equals( typeof( StringBuilder ).TypeHandle ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatStringTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( T ).TypeHandle.Equals( typeof( bool ).TypeHandle ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatBoleanTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( byte[] ).TypeHandle.Equals( typeof( T ).TypeHandle ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatBytesTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( char[] ).TypeHandle.Equals( typeof( T ).TypeHandle ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatStringTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( T ).IsArray )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatGenericEnumerableTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( DateTimeOffset ).TypeHandle.Equals( typeof( T ).TypeHandle )
					|| typeof( DateTime ).TypeHandle.Equals( typeof( T ).TypeHandle ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatDateTimeTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( TimeSpan ).TypeHandle.Equals( typeof( T ).TypeHandle ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatTimeSpanTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				bool isFormattable;
				if ( IsNumerics( typeof( T ).TypeHandle, out isFormattable ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							isFormattable
							? typeof( GenericItemFormatter<T> ).GetMethod( "FormatFormattableNumericTo", bindingFlags ).MakeGenericMethod( typeof( T ) )
							: typeof( GenericItemFormatter<T> ).GetMethod( "FormatNumericTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( IFormattable ).IsAssignableFrom( typeof( T ) ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatFormattableTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( SerializationInfo ).TypeHandle.Equals( typeof( T ).TypeHandle ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatSerializationInfoTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( T ).IsClosedTypeOf( typeof( ArraySegment<> ) ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatArraySegmentTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( T ).IsClosedTypeOf( typeof( Tuple<> ) )
					|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,> ) )
					|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,> ) )
					|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,> ) )
					|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,,> ) )
					|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,,,> ) )
					|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,,,,> ) )
					|| typeof( T ).IsClosedTypeOf( typeof( Tuple<,,,,,,,> ) )
				)
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatTuple" + typeof( T ).GetGenericArguments().Length + "To", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				var toString = typeof( T ).GetMethod( "ToString", BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null );
				if ( toString != null 
					&& !typeof( object ).TypeHandle.Equals( toString.DeclaringType.TypeHandle ) 
					&& !typeof( ValueType ).TypeHandle.Equals( toString.DeclaringType.TypeHandle ) 
				)
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatObjectTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( T ).Implements( typeof( IDictionary<,> ) ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatGenericDictionaryTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				if ( typeof( T ).Implements( typeof( IEnumerable<> ) ) )
				{
					if ( typeof( ICollection ).IsAssignableFrom( typeof( T ) ) )
					{
						var genericArgument = typeof( T ).GetGenericArguments()[ 0 ];

						if ( typeof( byte ).TypeHandle.Equals( genericArgument.TypeHandle ) )
						{
							Action =
								Delegate.CreateDelegate(
									typeof( Action<T, FormattingContext> ),
									typeof( GenericItemFormatter<T> ).GetMethod( "FormatBytesTo", bindingFlags )
								) as Action<T, FormattingContext>;
						}
						else if ( typeof( char ).TypeHandle.Equals( genericArgument.TypeHandle ) )
						{
							Action =
								Delegate.CreateDelegate(
									typeof( Action<T, FormattingContext> ),
									typeof( GenericItemFormatter<T> ).GetMethod( "FormatStringTo", bindingFlags )
								) as Action<T, FormattingContext>;
						}
						else
						{
							Action =
								Delegate.CreateDelegate(
									typeof( Action<T, FormattingContext> ),
									typeof( GenericItemFormatter<T> ).GetMethod( "FormatGenericEnumerableTo", bindingFlags )
								) as Action<T, FormattingContext>;
						}

						return;
					}
				}
				else if ( typeof( IDictionary ).IsAssignableFrom( typeof( T ) ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatNonGenericDictionaryTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}
				else if ( typeof( IEnumerable ).IsAssignableFrom( typeof( T ) ) )
				{
					Action =
						Delegate.CreateDelegate(
							typeof( Action<T, FormattingContext> ),
							typeof( GenericItemFormatter<T> ).GetMethod( "FormatNonGenericEnumerableTo", bindingFlags )
						) as Action<T, FormattingContext>;
					return;
				}

				Action =
					Delegate.CreateDelegate(
						typeof( Action<T, FormattingContext> ),
						typeof( GenericItemFormatter<T> ).GetMethod( "FormatObjectTo", bindingFlags )
					) as Action<T, FormattingContext>;
				return;
			}

			// Note: These methods are invoked via delegate.

			private static void FormatBoleanTo( bool item, FormattingContext context )
			{
				Debug.WriteLine( "ItemFormatter<{0}>::FormatBoleanTo( {1}, {2} )", typeof( T ).FullName, item, context );
				context.Buffer.Append( item ? "true" : "false" );
			}

			private static void FormatNumericTo( T item, FormattingContext context )
			{
				Debug.WriteLine( "ItemFormatter<{0}>::FormatNumericTo( {1}, {2} )", typeof( T ).FullName, item, context );
				context.Buffer.Append( item );
			}

			private static void FormatFormattableNumericTo<TItem>( TItem item, FormattingContext context )
				where TItem : IFormattable
			{
				Contract.Assert( typeof( TItem ).TypeHandle.Equals( typeof( T ).TypeHandle ) );

				Debug.WriteLine( "ItemFormatter<{0}>::FormatFormattableNumericTo<{3}>( {1}, {2} )", typeof( T ).FullName, item, context, typeof( TItem ).FullName );

				if ( context.IsInCollection )
				{
					context.Buffer.Append( item );
				}
				else
				{
					context.Buffer.Append( item.ToString( context.Format, context.FallbackProvider ) );
				}
			}

			private static void FormatBoxedFormattableNumericTo( IFormattable item, FormattingContext context )
			{
				Debug.WriteLine( "ItemFormatter<{0}>::FormatBoxedFormattableNumericTo( {1}, {2} )", typeof( T ).FullName, item, context );

				if ( context.IsInCollection )
				{
					context.Buffer.Append( item );
				}
				else
				{
					context.Buffer.Append( item.ToString( context.Format, context.FallbackProvider ) );
				}
			}

			private static void FormatBytesTo( T item, FormattingContext context )
			{
				Debug.WriteLine( "ItemFormatter<{0}>::FormatBytesTo( {1}, {2} )", typeof( T ).FullName, item, context );

				if ( context.IsInCollection )
				{
					context.Buffer.Append( '\"' );
				}

				context.Buffer.AppendHex( item as IEnumerable<byte> );

				if ( context.IsInCollection )
				{
					context.Buffer.Append( '\"' );
				}
			}

			private static void FormatStringTo( T item, FormattingContext context )
			{
				Debug.WriteLine( "ItemFormatter<{0}>::FormatStringTo( {1}, {2} )", typeof( T ).FullName, item, context );

				if ( context.IsInCollection )
				{
					context.Buffer.Append( '\"' );
				}

				foreach ( var c in context.IsInCollection ? EscapeChars( item ) : item as IEnumerable<char> )
				{
					context.Buffer.Append( c );
				}

				if ( context.IsInCollection )
				{
					context.Buffer.Append( '\"' );
				}
			}

			private static void FormatFormattableTo( T item, FormattingContext context )
			{
				Debug.WriteLine( "ItemFormatter<{0}>::FormatFormattableTo( {1}, {2} )", typeof( T ).FullName, item, context );

				if ( item == null )
				{
					context.Buffer.Append( _nullRepresentation );
				}
				else
				{
					if ( context.IsInCollection )
					{
						context.Buffer.Append( '"' );

						// always tend to Json compat
						foreach ( var c in _collectionItemFilter.Escape( ( ( IFormattable )item ).ToString( context.Format, CultureInfo.InvariantCulture ) ) )
						{
							context.Buffer.Append( c );
						}

						context.Buffer.Append( '"' );
					}
					else
					{
						foreach ( var c in _collectionItemFilter.Escape( ( ( IFormattable )item ).ToString( context.Format, context.FallbackProvider ) ) )
						{
							context.Buffer.Append( c );
						}
					}
				}
			}

			private static void FormatTo( T item, FormattingContext context )
			{
				Debug.WriteLine( "ItemFormatter<{0}>::FormatTo( {1}, {2} )", typeof( T ).FullName, item, context );

				if ( Object.ReferenceEquals( item, null ) )
				{
					context.Buffer.Append( _nullRepresentation );
				}
				else
				{
					ItemFormatter.Get( item.GetType() ).FormatTo( item, context );
				}
			}

			private static void FormatDateTimeTo( T item, FormattingContext context )
			{
				Debug.WriteLine( "ItemFormatter<{0}>::FormatDateTimeTo( {1}, {2} )", typeof( T ).FullName, item, context );

				if ( Object.ReferenceEquals( item, null ) )
				{
					context.Buffer.Append( _nullRepresentation );
				}
				else
				{
					if ( context.IsInCollection )
					{
						context.Buffer.Append( '"' );
						// always JSON compatible
						context.Buffer.Append( ( item as IFormattable ).ToString( "o", CultureInfo.InvariantCulture ) );
						context.Buffer.Append( '"' );
					}
					else
					{
						context.Buffer.Append( ( item as IFormattable ).ToString( context.Format, context.FallbackProvider ) );
					}
				}
			}

			private static void FormatTimeSpanTo( T item, FormattingContext context )
			{
				Debug.WriteLine( "ItemFormatter<{0}>::FormatTimeSpanTo( {1}, {2} )", typeof( T ).FullName, item, context );

				if ( Object.ReferenceEquals( item, null ) )
				{
					context.Buffer.Append( _nullRepresentation );
				}
				else
				{
					if ( context.IsInCollection )
					{
						context.Buffer.Append( '"' );
					}

					context.Buffer.Append( ( item as IFormattable ).ToString( "c", context.FallbackProvider ) );

					if ( context.IsInCollection )
					{
						context.Buffer.Append( '"' );
					}
				}
			}

			private static void FormatArraySegmentTo( T item, FormattingContext context )
			{
				Debug.WriteLine( "ItemFormatter<{0}>::FormatArraySegmentTo( {1}, {2} )", typeof( T ).FullName, item, context );

				ArraySegmentFormatter.Get<T>().FormatTo( item, context );

			}

			private static void FormatSerializationInfoTo( T item, FormattingContext context )
			{
				Debug.WriteLine( "ItemFormatter<{0}>::FormatSerializationInfoTo( {1}, {2} )", typeof( T ).FullName, item, context );

				context.Buffer.Append( "{ " );
				context.EnterCollection();

				bool isFirstEntry = true;
				foreach ( SerializationEntry entry in ( item as SerializationInfo ) )
				{
					if ( !isFirstEntry )
					{
						context.Buffer.Append( ", " );
					}

					context.Buffer.Append( '"' ).Append( entry.Name ).Append( "\" : " );

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

				context.LeaveCollection();
				context.Buffer.Append( " }" );
			}

			private static void FormatObjectTo( T item, FormattingContext context )
			{
				Debug.WriteLine( "ItemFormatter<{0}>::FormatObjectTo( {1}, {2} )", typeof( T ).FullName, item, context );

				if ( Object.ReferenceEquals( item, null ) )
				{
					context.Buffer.Append( _nullRepresentation );
					return;
				}

				if ( IsRuntimeCheckNeeded( typeof( T ) ) )
				{
					ItemFormatter.Get( item.GetType() ).FormatTo( item, context );
					return;
				}

				if ( context.IsInCollection )
				{
					context.Buffer.Append( '"' );
				}

				context.Buffer.Append( item );

				if ( context.IsInCollection )
				{
					context.Buffer.Append( '"' );
				}
			}

			private static void FormatNonGenericEnumerableTo( T item, FormattingContext context )
			{
				Contract.Assert( item is IEnumerable );

				Debug.WriteLine( "ItemFormatter<{0}>::FormatNonGenericEnumerableTo( {1}, {2} )", typeof( T ).FullName, item, context );

				context.Buffer.Append( "[ " );

				context.EnterCollection();

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

				context.LeaveCollection();
				context.Buffer.Append( " ]" );
			}

			private static void FormatGenericEnumerableTo( T item, FormattingContext context )
			{
				Contract.Assert( typeof( T ).Implements( typeof( IEnumerable<> ) ) );

				Debug.WriteLine( "ItemFormatter<{0}>::FormatGenericEnumerableTo( {1}, {2} )", typeof( T ).FullName, item, context );

				var ienumerable =
					typeof( T ).GetInterfaces()
					.First( @interface => @interface.IsGenericType
						&& !@interface.IsGenericTypeDefinition
						&& @interface.Name == typeof( IEnumerable<> ).Name
					);

				var genericArguments = ienumerable.GetGenericArguments();
				Contract.Assert( genericArguments.Length == 1 );

				if ( ItemFormatter.IsRuntimeCheckNeeded( genericArguments[ 0 ] ) )
				{
					FormatNonGenericEnumerableTo( item, context );
				}
				else
				{
					SequenceFormatter.Get( genericArguments[ 0 ] ).FormatTo( item, context );
				}
			}

			private static void FormatNonGenericDictionaryTo( T item, FormattingContext context )
			{
				Contract.Assert( item is IEnumerable );

				Debug.WriteLine( "ItemFormatter<{0}>::FormatNonGenericDictionaryTo( {1}, {2} )", typeof( T ).FullName, item, context );

				context.Buffer.Append( "{ " );
				context.EnterCollection();

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

				context.LeaveCollection();
				context.Buffer.Append( " }" );
			}

			private static void FormatGenericDictionaryTo( T item, FormattingContext context )
			{
				Contract.Assert( typeof( T ).Implements( typeof( IDictionary<,> ) ) );

				Debug.WriteLine( "ItemFormatter<{0}>::FormatGenericDictionaryTo( {1}, {2} )", typeof( T ).FullName, item, context );

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

			public GenericItemFormatter() { }

			public override void FormatTo( object item, FormattingContext context )
			{
				Action( ( T )item, context );
			}
		}

		/// <summary>
		///		Non-generic entrypoint for sequence formatter.
		/// </summary>
		private abstract class SequenceFormatter
		{
			public static SequenceFormatter Get( Type itemType )
			{
				// TODO: caching
				return Activator.CreateInstance( typeof( SequenceFormatter<> ).MakeGenericType( itemType ) ) as SequenceFormatter;
			}

			public abstract void FormatTo( object sequence, FormattingContext context );
		}

		/// <summary>
		///		Formatter for generic sequence.
		/// </summary>
		private sealed class SequenceFormatter<TItem> : SequenceFormatter
		{
			private readonly Action<TItem, FormattingContext> _itemFormatter = GenericItemFormatter<TItem>.Action;

			public SequenceFormatter() { }

			public override void FormatTo( object sequence, FormattingContext context )
			{
				Contract.Assert( sequence is IEnumerable<TItem>, sequence == null ? "(null)" : sequence.GetType().FullName );

				Debug.WriteLine( "SequenceFormatter<{0}>::FormatTo( {1}, {2} )", typeof( TItem ).FullName, sequence, context );

				context.Buffer.Append( '[' );
				context.EnterCollection();

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
						else
						{
							context.Buffer.Append( ' ' );
						}

						this._itemFormatter( entry, context );

						isFirstEntry = false;
					}

					if ( !isFirstEntry )
					{
						context.Buffer.Append( ' ' );
					}
				}

				context.LeaveCollection();
				context.Buffer.Append( ']' );
			}
		}

		/// <summary>
		///		Non-generic entrypoint for dictionary formatter.
		/// </summary>
		private abstract class DictionaryFormatter
		{
			public static DictionaryFormatter Get( Type keyType, Type valueType )
			{
				// TODO: caching
				return Activator.CreateInstance( typeof( DictionaryFormatter<,> ).MakeGenericType( keyType, valueType ) ) as DictionaryFormatter;
			}

			public abstract void FormatTo( object dictionary, FormattingContext context );
		}

		/// <summary>
		///		Generic formatter for dictionary/map.
		/// </summary>
		private sealed class DictionaryFormatter<TKey, TValue> : DictionaryFormatter
		{
			private readonly Action<TKey, FormattingContext> _keyFormatter = GenericItemFormatter<TKey>.Action;
			private readonly Action<TValue, FormattingContext> _valueFormatter = GenericItemFormatter<TValue>.Action;

			public DictionaryFormatter() { }

			public override void FormatTo( object dictionary, FormattingContext context )
			{
				Debug.WriteLine( "DictionaryFormatter<{0}, {1}>::FormatTo( {2}, {3} )", typeof( TKey ).FullName, typeof( TValue ).FullName, dictionary, context );

				context.Buffer.Append( "{ " );
				context.EnterCollection();

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

				context.LeaveCollection();
				context.Buffer.Append( " }" );
			}
		}
	}
}
