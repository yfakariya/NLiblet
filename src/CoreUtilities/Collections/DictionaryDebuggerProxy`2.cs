using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NLiblet.Collections
{
	// Thanks to Mono implementation.

	/// <summary>
	///		Debugger type proxy for <see cref="IDictionary{TKey,TValue}"/>.
	/// </summary>
	/// <typeparam name="TKey">The key type of the dictionary.</typeparam>
	/// <typeparam name="TValue">The value type of the dictionary.</typeparam>
	internal sealed class DictionaryDebuggerProxy<TKey,TValue>
	{
		private readonly IDictionary<TKey,TValue> _dictionary;

		public DictionaryDebuggerProxy( IDictionary<TKey, TValue> target )
		{
			this._dictionary = target;
		}

		[DebuggerBrowsable( DebuggerBrowsableState.RootHidden )]
		public KeyValuePair<TKey,TValue>[] Items
		{
			get
			{
				if ( this._dictionary == null )
				{
					return null;
				}

				var result = new KeyValuePair<TKey, TValue>[ this._dictionary.Count ];
				this._dictionary.CopyTo( result, 0 );
				return result;
			}
		}
	}
}
