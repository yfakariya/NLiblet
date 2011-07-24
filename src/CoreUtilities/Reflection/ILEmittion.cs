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
using System.Diagnostics;
using System.Text;
using System.Reflection;

namespace NLiblet.Reflection
{
	internal static class ILEmittion
	{
		private static readonly TraceSource _trace = new TraceSource( typeof( ILEmittion ).FullName );

		internal static TraceSource TraceSource
		{
			get { return _trace; }
		}

		private static readonly Dictionary<int, string> _eventIdDictionary =
			new Dictionary<int, string>()
				{
					{ EmitUnboxAnyCastEventId, "EmitUnboxAnyCast" },
					{ EmitConstructorCastEventId, "EmitConstructorCast" },
					{ EmitOperatorCastEventId, "EmitOperatorCast" },
					{ EmitTypeConverterCastEventId, "EmitTypeConverterCast" },
					{ EmitUnboxAndConvCastEventId, "EmitUnboxAndConvCast" },
					{ EmitCastMethodOfArrayItemEventId, "EmitCastMethodOfArrayItem" },
					{ EmitInvocationShimEventId, "EmitInvocationShim" },
				};

		public const int EmitUnboxAnyCastEventId = 101;
		public const int EmitConstructorCastEventId = 102;
		public const int EmitOperatorCastEventId = 103;
		public const int EmitTypeConverterCastEventId = 104;
		public const int EmitUnboxAndConvCastEventId = 105;
		public const int EmitCastMethodOfArrayItemEventId = 201;
		public const int EmitInvocationShimEventId = 301;

		private const int _defaultEventId = 0;

		//[Conditional( "TRACE_IL_EMITTION" )]
		public static void TraceIL( int eventId, MethodInfo method, StringBuilder tracingContent )
		{
			if ( tracingContent == null )
			{
				return;
			}

			if ( _trace.Switch.ShouldTrace( TraceEventType.Verbose ) )
			{
				_trace.TraceEvent( TraceEventType.Verbose, eventId, "IL Emittion Trace [{1}]:{0}{2}{0}{3}", Environment.NewLine, _eventIdDictionary[ eventId ], method, tracingContent );
			}
		}
	}
}
