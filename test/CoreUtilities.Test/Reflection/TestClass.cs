using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NLiblet.Reflection
{
	public class TestClass : TestAbstractClass, ITestInterface
	{
		private readonly string _arg;

		public string ConstructorArgument
		{
			get { return this._arg; }
		}

		public static readonly ConstructorInfo TypeInitializer =
			typeof( TestClass ).GetConstructor( BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static, null, Type.EmptyTypes, null );

		static TestClass() { }

		public static readonly ConstructorInfo Ctor =
			typeof( TestClass ).GetConstructor( Type.EmptyTypes );

		public TestClass() : this( null ) { }

		public static readonly ConstructorInfo Ctor_String =
			typeof( TestClass ).GetConstructor( new[] { typeof( string ) } );

		public TestClass( string arg )
		{
			this._arg = arg;
		}

		public static readonly MethodInfo PrivateMethod =
			typeof( TestClass ).GetMethod( "Private", BindingFlags.NonPublic | BindingFlags.Static );

		private static string Private()
		{
			return "OK";
		}

		public static readonly MethodInfo GetMaxMethod =
			typeof( TestClass ).GetMethod( "GetMax" );

		public static DateTime GetMax()
		{
			return DateTime.MaxValue;
		}

		public static readonly MethodInfo InstanceMethod =
			typeof( TestClass ).GetMethod( "Instance" );

		public string Instance( string arg )
		{
			return "OK:" + arg;
		}

		public static readonly MethodInfo ThrowExceptionMethod =
			typeof( TestClass ).GetMethod( "ThrowException" );

		public static void ThrowException( Exception exception )
		{
			throw exception;
		}

		public static readonly MethodInfo MayBeAction0Method =
			typeof( TestClass ).GetMethod( "MayBeAction0" );

		public static void MayBeAction0() { }

		public static readonly MethodInfo MayBeAction1Method =
			typeof( TestClass ).GetMethod( "MayBeAction1" );

		public static void MayBeAction1( int arg0 ) { }

		public static readonly MethodInfo MayBeAction16Method =
			typeof( TestClass ).GetMethod( "MayBeAction16" );

		public static void MayBeAction16(
			byte arg0, short arg1, int arg2, long arg3,
			sbyte arg4, ushort arg5, uint arg6, ulong arg7,
			bool arg8, char arg9, object arg10, string arg11,
			IntPtr arg12, decimal arg13, Uri arg14, object[] arg15
		) { }

		public static readonly MethodInfo MayBeFunc1Method =
			typeof( TestClass ).GetMethod( "MayBeFunc1" );

		public static string MayBeFunc1()
		{
			return "OK";
		}

		public static readonly MethodInfo MayBeFunc17Method =
			typeof( TestClass ).GetMethod( "MayBeFunc17" );

		public static string MayBeFunc17(
			byte arg0, short arg1, int arg2, long arg3,
			sbyte arg4, ushort arg5, uint arg6, ulong arg7,
			bool arg8, char arg9, object arg10, string arg11,
			IntPtr arg12, decimal arg13, Uri arg14, object[] arg15
		)
		{
			return "OK:" + arg11;
		}

		public static readonly MethodInfo OverrideMethod =
			typeof( TestClass ).GetMethod( "Override" );

		public override string Override()
		{
			return "OK";
		}

		public static readonly MethodInfo InheritMethod =
			typeof( TestClass ).GetMethod( "Inherit" );

		public static readonly MethodInfo InterfaceMethod =
			typeof( TestClass ).GetMethod( "NLiblet.Reflection.ITestInterface.Interface", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );

		string ITestInterface.Interface()
		{
			return "OK";
		}
	}

	public interface ITestInterface
	{
		string Interface();
	}

	public abstract class TestAbstractClass
	{
		public abstract string Override();
		public string Inherit()
		{
			return "OK";
		}
	}

}
