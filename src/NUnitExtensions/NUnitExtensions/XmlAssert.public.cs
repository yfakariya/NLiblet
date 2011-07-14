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

// This code is generated from T4Template XmlAssert.public.tt.
// Do not modify this source code directly.

using System;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace NLiblet.NUnitExtensions
{
	partial class XmlAssert
	{

		/// <summary>
		/// 	Verify two <see cref="XAttribute"/>s are equal.
		/// </summary>
		/// <param name="expected">Expected <see cref="XAttribute"/> value.</param>
		/// <param name="actual">Actual <see cref="XAttribute"/> value.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XAttribute expected, XAttribute actual )
		{
			AreEqual( expected, actual, default( Func<string> ) );
		}
		
		/// <summary>
		/// 	Verify two <see cref="XAttribute"/>s are equal with specified error message.
		/// </summary>
		/// <param name="expected">Expected <see cref="XAttribute"/> value.</param>
		/// <param name="actual">Actual <see cref="XAttribute"/> value.</param>
		/// <param name="message">Custom error message or its format string. This value can be null.</param>
		/// <param name="args">Format argument of custom error messgae.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		/// <exception cref="FormatException"><paramref name="message"/>, <paramref name="args"/> or both of them are invalid.</exception>
		public static void AreEqual( XAttribute expected, XAttribute actual, string message, params object[] args )
		{
			AreEqual( expected, actual, () => String.Format( CultureInfo.CurrentCulture, message, args ) );
		}

		/// <summary>
		/// 	Verify two <see cref="XAttribute"/>s are equal with specified error message provider.
		/// </summary>
		/// <param name="expected">Expected <see cref="XAttribute"/> value.</param>
		/// <param name="actual">Actual <see cref="XAttribute"/> value.</param>
		/// <param name="messageProvider">Delegate which provides custom assertion error message. This value can be null.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XAttribute expected, XAttribute actual, Func<string> messageProvider )
		{
			if ( AssertWhetherNull( expected, actual, messageProvider, target => target.ToString(  ) ) )
			{
				return;
			}

			AreEqualCore( expected, actual, messageProvider );
		}

		/// <summary>
		/// 	Verify two <see cref="XElement"/>s are equal.
		/// </summary>
		/// <param name="expected">Expected <see cref="XElement"/> value.</param>
		/// <param name="actual">Actual <see cref="XElement"/> value.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XElement expected, XElement actual )
		{
			AreEqual( expected, actual, default( Func<string> ) );
		}
		
		/// <summary>
		/// 	Verify two <see cref="XElement"/>s are equal with specified error message.
		/// </summary>
		/// <param name="expected">Expected <see cref="XElement"/> value.</param>
		/// <param name="actual">Actual <see cref="XElement"/> value.</param>
		/// <param name="message">Custom error message or its format string. This value can be null.</param>
		/// <param name="args">Format argument of custom error messgae.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		/// <exception cref="FormatException"><paramref name="message"/>, <paramref name="args"/> or both of them are invalid.</exception>
		public static void AreEqual( XElement expected, XElement actual, string message, params object[] args )
		{
			AreEqual( expected, actual, () => String.Format( CultureInfo.CurrentCulture, message, args ) );
		}

		/// <summary>
		/// 	Verify two <see cref="XElement"/>s are equal with specified error message provider.
		/// </summary>
		/// <param name="expected">Expected <see cref="XElement"/> value.</param>
		/// <param name="actual">Actual <see cref="XElement"/> value.</param>
		/// <param name="messageProvider">Delegate which provides custom assertion error message. This value can be null.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XElement expected, XElement actual, Func<string> messageProvider )
		{
			if ( AssertWhetherNull( expected, actual, messageProvider, target => target.ToString( SaveOptions.DisableFormatting ) ) )
			{
				return;
			}

			AreEqualCore( expected, actual, messageProvider );
		}

		/// <summary>
		/// 	Verify two <see cref="XDocument"/>s are equal.
		/// </summary>
		/// <param name="expected">Expected <see cref="XDocument"/> value.</param>
		/// <param name="actual">Actual <see cref="XDocument"/> value.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XDocument expected, XDocument actual )
		{
			AreEqual( expected, actual, default( Func<string> ) );
		}
		
		/// <summary>
		/// 	Verify two <see cref="XDocument"/>s are equal with specified error message.
		/// </summary>
		/// <param name="expected">Expected <see cref="XDocument"/> value.</param>
		/// <param name="actual">Actual <see cref="XDocument"/> value.</param>
		/// <param name="message">Custom error message or its format string. This value can be null.</param>
		/// <param name="args">Format argument of custom error messgae.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		/// <exception cref="FormatException"><paramref name="message"/>, <paramref name="args"/> or both of them are invalid.</exception>
		public static void AreEqual( XDocument expected, XDocument actual, string message, params object[] args )
		{
			AreEqual( expected, actual, () => String.Format( CultureInfo.CurrentCulture, message, args ) );
		}

		/// <summary>
		/// 	Verify two <see cref="XDocument"/>s are equal with specified error message provider.
		/// </summary>
		/// <param name="expected">Expected <see cref="XDocument"/> value.</param>
		/// <param name="actual">Actual <see cref="XDocument"/> value.</param>
		/// <param name="messageProvider">Delegate which provides custom assertion error message. This value can be null.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XDocument expected, XDocument actual, Func<string> messageProvider )
		{
			if ( AssertWhetherNull( expected, actual, messageProvider, target => target.ToString( SaveOptions.DisableFormatting ) ) )
			{
				return;
			}

			AreEqualCore( expected, actual, messageProvider );
		}

		/// <summary>
		/// 	Verify two <see cref="XDeclaration"/>s are equal.
		/// </summary>
		/// <param name="expected">Expected <see cref="XDeclaration"/> value.</param>
		/// <param name="actual">Actual <see cref="XDeclaration"/> value.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XDeclaration expected, XDeclaration actual )
		{
			AreEqual( expected, actual, default( Func<string> ) );
		}
		
		/// <summary>
		/// 	Verify two <see cref="XDeclaration"/>s are equal with specified error message.
		/// </summary>
		/// <param name="expected">Expected <see cref="XDeclaration"/> value.</param>
		/// <param name="actual">Actual <see cref="XDeclaration"/> value.</param>
		/// <param name="message">Custom error message or its format string. This value can be null.</param>
		/// <param name="args">Format argument of custom error messgae.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		/// <exception cref="FormatException"><paramref name="message"/>, <paramref name="args"/> or both of them are invalid.</exception>
		public static void AreEqual( XDeclaration expected, XDeclaration actual, string message, params object[] args )
		{
			AreEqual( expected, actual, () => String.Format( CultureInfo.CurrentCulture, message, args ) );
		}

		/// <summary>
		/// 	Verify two <see cref="XDeclaration"/>s are equal with specified error message provider.
		/// </summary>
		/// <param name="expected">Expected <see cref="XDeclaration"/> value.</param>
		/// <param name="actual">Actual <see cref="XDeclaration"/> value.</param>
		/// <param name="messageProvider">Delegate which provides custom assertion error message. This value can be null.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XDeclaration expected, XDeclaration actual, Func<string> messageProvider )
		{
			if ( AssertWhetherNull( expected, actual, messageProvider, target => target.ToString(  ) ) )
			{
				return;
			}

			AreEqualCore( expected, actual, messageProvider );
		}

		/// <summary>
		/// 	Verify two <see cref="XDocumentType"/>s are equal.
		/// </summary>
		/// <param name="expected">Expected <see cref="XDocumentType"/> value.</param>
		/// <param name="actual">Actual <see cref="XDocumentType"/> value.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XDocumentType expected, XDocumentType actual )
		{
			AreEqual( expected, actual, default( Func<string> ) );
		}
		
		/// <summary>
		/// 	Verify two <see cref="XDocumentType"/>s are equal with specified error message.
		/// </summary>
		/// <param name="expected">Expected <see cref="XDocumentType"/> value.</param>
		/// <param name="actual">Actual <see cref="XDocumentType"/> value.</param>
		/// <param name="message">Custom error message or its format string. This value can be null.</param>
		/// <param name="args">Format argument of custom error messgae.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		/// <exception cref="FormatException"><paramref name="message"/>, <paramref name="args"/> or both of them are invalid.</exception>
		public static void AreEqual( XDocumentType expected, XDocumentType actual, string message, params object[] args )
		{
			AreEqual( expected, actual, () => String.Format( CultureInfo.CurrentCulture, message, args ) );
		}

		/// <summary>
		/// 	Verify two <see cref="XDocumentType"/>s are equal with specified error message provider.
		/// </summary>
		/// <param name="expected">Expected <see cref="XDocumentType"/> value.</param>
		/// <param name="actual">Actual <see cref="XDocumentType"/> value.</param>
		/// <param name="messageProvider">Delegate which provides custom assertion error message. This value can be null.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XDocumentType expected, XDocumentType actual, Func<string> messageProvider )
		{
			if ( AssertWhetherNull( expected, actual, messageProvider, target => target.ToString( SaveOptions.DisableFormatting ) ) )
			{
				return;
			}

			AreEqualCore( expected, actual, messageProvider );
		}

		/// <summary>
		/// 	Verify two <see cref="XProcessingInstruction"/>s are equal.
		/// </summary>
		/// <param name="expected">Expected <see cref="XProcessingInstruction"/> value.</param>
		/// <param name="actual">Actual <see cref="XProcessingInstruction"/> value.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XProcessingInstruction expected, XProcessingInstruction actual )
		{
			AreEqual( expected, actual, default( Func<string> ) );
		}
		
		/// <summary>
		/// 	Verify two <see cref="XProcessingInstruction"/>s are equal with specified error message.
		/// </summary>
		/// <param name="expected">Expected <see cref="XProcessingInstruction"/> value.</param>
		/// <param name="actual">Actual <see cref="XProcessingInstruction"/> value.</param>
		/// <param name="message">Custom error message or its format string. This value can be null.</param>
		/// <param name="args">Format argument of custom error messgae.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		/// <exception cref="FormatException"><paramref name="message"/>, <paramref name="args"/> or both of them are invalid.</exception>
		public static void AreEqual( XProcessingInstruction expected, XProcessingInstruction actual, string message, params object[] args )
		{
			AreEqual( expected, actual, () => String.Format( CultureInfo.CurrentCulture, message, args ) );
		}

		/// <summary>
		/// 	Verify two <see cref="XProcessingInstruction"/>s are equal with specified error message provider.
		/// </summary>
		/// <param name="expected">Expected <see cref="XProcessingInstruction"/> value.</param>
		/// <param name="actual">Actual <see cref="XProcessingInstruction"/> value.</param>
		/// <param name="messageProvider">Delegate which provides custom assertion error message. This value can be null.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XProcessingInstruction expected, XProcessingInstruction actual, Func<string> messageProvider )
		{
			if ( AssertWhetherNull( expected, actual, messageProvider, target => target.ToString( SaveOptions.DisableFormatting ) ) )
			{
				return;
			}

			AreEqualCore( expected, actual, messageProvider );
		}

		/// <summary>
		/// 	Verify two <see cref="XComment"/>s are equal.
		/// </summary>
		/// <param name="expected">Expected <see cref="XComment"/> value.</param>
		/// <param name="actual">Actual <see cref="XComment"/> value.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XComment expected, XComment actual )
		{
			AreEqual( expected, actual, default( Func<string> ) );
		}
		
		/// <summary>
		/// 	Verify two <see cref="XComment"/>s are equal with specified error message.
		/// </summary>
		/// <param name="expected">Expected <see cref="XComment"/> value.</param>
		/// <param name="actual">Actual <see cref="XComment"/> value.</param>
		/// <param name="message">Custom error message or its format string. This value can be null.</param>
		/// <param name="args">Format argument of custom error messgae.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		/// <exception cref="FormatException"><paramref name="message"/>, <paramref name="args"/> or both of them are invalid.</exception>
		public static void AreEqual( XComment expected, XComment actual, string message, params object[] args )
		{
			AreEqual( expected, actual, () => String.Format( CultureInfo.CurrentCulture, message, args ) );
		}

		/// <summary>
		/// 	Verify two <see cref="XComment"/>s are equal with specified error message provider.
		/// </summary>
		/// <param name="expected">Expected <see cref="XComment"/> value.</param>
		/// <param name="actual">Actual <see cref="XComment"/> value.</param>
		/// <param name="messageProvider">Delegate which provides custom assertion error message. This value can be null.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XComment expected, XComment actual, Func<string> messageProvider )
		{
			if ( AssertWhetherNull( expected, actual, messageProvider, target => target.ToString( SaveOptions.DisableFormatting ) ) )
			{
				return;
			}

			AreEqualCore( expected, actual, messageProvider );
		}

		/// <summary>
		/// 	Verify two <see cref="XText"/>s are equal.
		/// </summary>
		/// <param name="expected">Expected <see cref="XText"/> value.</param>
		/// <param name="actual">Actual <see cref="XText"/> value.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XText expected, XText actual )
		{
			AreEqual( expected, actual, default( Func<string> ) );
		}
		
		/// <summary>
		/// 	Verify two <see cref="XText"/>s are equal with specified error message.
		/// </summary>
		/// <param name="expected">Expected <see cref="XText"/> value.</param>
		/// <param name="actual">Actual <see cref="XText"/> value.</param>
		/// <param name="message">Custom error message or its format string. This value can be null.</param>
		/// <param name="args">Format argument of custom error messgae.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		/// <exception cref="FormatException"><paramref name="message"/>, <paramref name="args"/> or both of them are invalid.</exception>
		public static void AreEqual( XText expected, XText actual, string message, params object[] args )
		{
			AreEqual( expected, actual, () => String.Format( CultureInfo.CurrentCulture, message, args ) );
		}

		/// <summary>
		/// 	Verify two <see cref="XText"/>s are equal with specified error message provider.
		/// </summary>
		/// <param name="expected">Expected <see cref="XText"/> value.</param>
		/// <param name="actual">Actual <see cref="XText"/> value.</param>
		/// <param name="messageProvider">Delegate which provides custom assertion error message. This value can be null.</param>
		/// <exception cref="NUnit.Framework.AssertionException\">Verification failed.</exception>
		public static void AreEqual( XText expected, XText actual, Func<string> messageProvider )
		{
			if ( AssertWhetherNull( expected, actual, messageProvider, target => target.ToString( SaveOptions.DisableFormatting ) ) )
			{
				return;
			}

			AreEqualCore( expected, actual, messageProvider );
		}
	}
}