using System;
using System.Diagnostics.Contracts;

namespace NLiblet.Text.Formatters
{
	/// <summary>
	///		Defines common base class of item formatter.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal abstract class ItemFormatter<T> : ItemFormatter
	{
		public abstract void FormatTo( T item, FormattingContext context );

		public sealed override void FormatObjectTo( object item, FormattingContext context )
		{
			Contract.Assert( item is T, item == null ? "(null)" : item.GetType().FullName );

			this.FormatTo( ( T )item, context );
		}
	}
}
