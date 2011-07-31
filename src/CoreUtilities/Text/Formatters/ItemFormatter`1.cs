using System;
using System.Diagnostics;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Defines common base class of item formatter.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal abstract class ItemFormatter<T> : ItemFormatter, NLiblet.Text.Formatters.IItemFormatter<T>
	{
		public abstract void FormatTo( T item, FormattingContext context );

		public sealed override void FormatObjectTo( object item, FormattingContext context )
		{
			// Due to rewriter bug, use Debug instead of Contract
			Debug.Assert( item is T, item == null ? "(null)" : item.GetType().FullName );

			this.FormatTo( ( T )item, context );
		}
	}
}
