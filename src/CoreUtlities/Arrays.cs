using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NLiblet
{
	public static class Arrays
	{
		public static T[] Empty<T>()
		{
			return TypedArrays<T>.Empty;
		}

		private static class TypedArrays<T>
		{
			public static readonly T[] Empty = new T[ 0 ];
		}
	}
}
