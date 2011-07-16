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
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using NLiblet.Collections;
using NUnit.Framework;

namespace NLiblet.NUnitExtensions
{
	/// <summary>
	///		Define LINQ to XML constracts verification.
	/// </summary>
	public static partial class XmlAssert
	{
		/// <summary>
		/// 	Verify two <see cref="IEnumerable{XNode}"/>s are equal.
		/// </summary>
		/// <param name="expected">Expected <see cref="XAttribute"/> value.</param>
		/// <param name="actual">Actual <see cref="XAttribute"/> value.</param>
		/// <exception cref=""NUnit.Framework.AssertionException\"">Verification failed.</exception>
		public static void AreEqual( IEnumerable<XNode> expected, IEnumerable<XNode> actual )
		{
			AreEqual( expected, actual, default( Func<string> ) );
		}

		/// <summary>
		/// 	Verify two <see cref="IEnumerable{XNode}"/>s are equal with specified error message.
		/// </summary>
		/// <param name="expected">Expected <see cref="XAttribute"/> value.</param>
		/// <param name="actual">Actual <see cref="XAttribute"/> value.</param>
		/// <param name="message">Custom error message or its format string. This value can be null.</param>
		/// <param name="args">Format argument of custom error messgae.</param>
		/// <exception cref=""NUnit.Framework.AssertionException\"">Verification failed.</exception>
		/// <exception cref="FormatException"><paramref name="message"/>, <paramref name="args"/> or both of them are invalid.</exception>
		public static void AreEqual( IEnumerable<XNode> expected, IEnumerable<XNode> actual, string message, params object[] args )
		{
			AreEqual( expected, actual, () => String.Format( CultureInfo.CurrentCulture, message, args ) );
		}

		/// <summary>
		/// 	Verify two <see cref="IEnumerable{XNode}"/>s are equal with specified error message provider.
		/// </summary>
		/// <param name="expected">Expected <see cref="XAttribute"/> value.</param>
		/// <param name="actual">Actual <see cref="XAttribute"/> value.</param>
		/// <param name="messageProvider">Delegate which provides custom assertion error message. This value can be null.</param>
		/// <exception cref=""NUnit.Framework.AssertionException\"">Verification failed.</exception>
		public static void AreEqual( IEnumerable<XNode> expected, IEnumerable<XNode> actual, Func<string> messageProvider )
		{
			if ( AssertWhetherNull( expected, actual, messageProvider, _ => "<collection>" ) )
			{
				return;
			}

			AssertNodesAreEqual( null, expected, actual, messageProvider );
		}

		private static void AreEqualCore( XAttribute expected, XAttribute actual, Func<string> messageProvider )
		{
			if ( expected.Name != actual.Name )
			{
				Fail(
					messageProvider,
					Properties.Resources.XmlAssert_AttributeNamesAreNotEqual,
					Environment.NewLine,
					Difference.Compare( expected.Name.ToString(), actual.Name.ToString() ),
					BuildXPath( expected )
				);
			}

			if ( expected.Value != actual.Value )
			{
				Fail(
					messageProvider,
					Properties.Resources.XmlAssert_AttributeValuesAreNotEqual,
					Environment.NewLine,
					Difference.Compare( expected.Value, actual.Value ),
					BuildXPath( expected )
				);
			}
		}

		private static void AreEqualCore( XComment expected, XComment actual, Func<string> messageProvider )
		{
			if ( !String.Equals( expected.Value, actual.Value, StringComparison.Ordinal ) )
			{
				Fail(
					messageProvider,
					Properties.Resources.XmlAssert_CommentAreNotEqual,
					Difference.Compare( expected.Value, actual.Value )
				);
			}
		}

		private static void AreEqualCore( XText expected, XText actual, Func<string> messageProvider )
		{
			if ( !String.Equals( expected.Value, actual.Value, StringComparison.Ordinal ) )
			{
				Fail(
					messageProvider,
					Properties.Resources.XmlAssert_TextAreNotEqual,
					Difference.Compare( expected.Value, actual.Value )
				);
			}
		}

		private static void AreEqualCore( XDeclaration expected, XDeclaration actual, Func<string> messageProvider )
		{
			if ( !String.Equals( expected.Encoding, actual.Encoding, StringComparison.Ordinal ) )
			{
				Fail( messageProvider, Properties.Resources.XmlAssert_EncodingOfDeclarionAreNotEqual, expected.Encoding, actual.Encoding );
			}

			if ( !String.Equals( expected.Standalone, actual.Standalone, StringComparison.Ordinal ) )
			{
				Fail( messageProvider, Properties.Resources.XmlAssert_StandaloneOfDeclarionAreNotEqual, expected.Standalone, actual.Standalone );
			}

			if ( !String.Equals( expected.Version, actual.Version, StringComparison.Ordinal ) )
			{
				Fail( messageProvider, Properties.Resources.XmlAssert_VersionOfDeclarionAreNotEqual, expected.Version, actual.Version );
			}
		}

		private static void AreEqualCore( XProcessingInstruction expected, XProcessingInstruction actual, Func<string> messageProvider )
		{
			if ( !String.Equals( expected.Target, actual.Target, StringComparison.Ordinal ) )
			{
				Fail( messageProvider, Properties.Resources.XmlAssert_TargetOfInstructionAreNotEqual, expected.Target, actual.Target );
			}

			if ( !String.Equals( expected.Data, actual.Data, StringComparison.Ordinal ) )
			{
				Fail( messageProvider, Properties.Resources.XmlAssert_DataOfInstructionAreNotEqual, expected.Data, actual.Data );
			}
		}

		private static void AreEqualCore( XDocumentType expected, XDocumentType actual, Func<string> messageProvider )
		{
			if ( !String.Equals( expected.Name, actual.Name, StringComparison.Ordinal ) )
			{
				Fail( messageProvider, Properties.Resources.XmlAssert_NameOfDtdAreNotEqual, expected.Name, actual.Name );
			}

			if ( !String.Equals( expected.PublicId, actual.PublicId, StringComparison.Ordinal ) )
			{
				Fail( messageProvider, Properties.Resources.XmlAssert_PublicIdOfDtdAreNotEqual, expected.PublicId, actual.PublicId );
			}

			if ( !String.Equals( expected.SystemId, actual.SystemId, StringComparison.Ordinal ) )
			{
				Fail( messageProvider, Properties.Resources.XmlAssert_SystemIdOfDtdAreNotEqual, expected.SystemId, actual.SystemId );
			}

			if ( !String.Equals( expected.InternalSubset, actual.InternalSubset, StringComparison.Ordinal ) )
			{
				Fail( messageProvider, Properties.Resources.XmlAssert_InternalSubsetOfDtdAreNotEqual, expected.InternalSubset, actual.InternalSubset );
			}
		}

		private static void AreEqualCore( XDocument expected, XDocument actual, Func<string> messageProvider )
		{
			AreEqual( expected.Declaration, actual.Declaration, messageProvider );
			AreEqual( expected.DocumentType, actual.DocumentType, messageProvider );
			AreEqualCore( expected.Root, actual.Root, messageProvider );
		}

		private static void AreEqualCore( XElement expected, XElement actual, Func<string> messageProvider )
		{
			if ( expected.Name != actual.Name )
			{
				Fail(
					messageProvider,
					Properties.Resources.XmlAssert_ElementNamesAreNotEqual,
					Environment.NewLine,
					Difference.Compare( expected.Name.ToString(), actual.Name.ToString() ),
					BuildXPath( expected )
				);
			}

			AssertAttributesAreEquivelant( expected, actual, messageProvider );
			AssertChildrenAreEqual( expected, actual, messageProvider );
		}

		private static void AssertAttributesAreEquivelant( XElement expected, XElement actual, Func<string> messageProvider )
		{
			// attributes are UNordred by spec.

			var expectedAttributes = expected.Attributes().ToDictionary( attribute => attribute.Name );
			var actualAttributes = actual.Attributes().ToDictionary( attribute => attribute.Name );

			foreach ( var expectedAttribute in expectedAttributes )
			{
				XAttribute actualAttribute;
				if ( actualAttributes.TryGetValue( expectedAttribute.Key, out actualAttribute ) )
				{
					AreEqual( expectedAttribute.Value, actualAttribute, messageProvider );
				}
				else
				{
					Fail(
						messageProvider,
						Properties.Resources.XmlAssert_AttributeDoesNotExist,
						Environment.NewLine,
						expectedAttribute.Value.Name,
						BuildXPath( expected )
					);
				}
			}

			var extraAttributes = actualAttributes.Except( expectedAttributes, _xAttributeNameComparer );
			if ( extraAttributes.Any() )
			{
				Fail(
					messageProvider,
					Properties.Resources.XmlAssert_SomeExtraAttributesExist,
					Environment.NewLine,
					String.Join( ", ", extraAttributes.Select( kv => "'" + kv.Value + "'" ) ),
					BuildXPath( expected )
				);
			}
		}

		private static void AssertChildrenAreEqual( XElement expected, XElement actual, Func<string> messageProvider )
		{
			// children are ORDERED by spec.
			AssertNodesAreEqual( expected, expected.Nodes(), actual.Nodes(), messageProvider );
		}

		private static void AssertNodesAreEqual( XNode parent, IEnumerable<XNode> expected, IEnumerable<XNode> actual, Func<string> messageProvider )
		{
			var expectedChildren = expected.ToArray();
			var actualChildren = actual.ToArray();

			int position = 0;
			for ( ; position < expectedChildren.Length && position < actualChildren.Length; position++ )
			{
				if ( expectedChildren[ position ] == null )
				{
					if ( actualChildren[ position ] == null )
					{
						continue;
					}

					Fail(
						messageProvider,
						Properties.Resources.XmlAssert_ActualNodeIsNotNull,
						Environment.NewLine,
						position,
						BuildXPath( parent ),
						actualChildren[ position ],
						actualChildren[ position ].GetType().Name
					);
					return;
				}
				else if ( actualChildren[ position ] == null )
				{
					Fail(
						messageProvider,
						Properties.Resources.XmlAssert_ExpectedNodeIsNotNull,
						Environment.NewLine,
						position,
						BuildXPath( parent ),
						expectedChildren[ position ],
						expectedChildren[ position ].GetType().Name
					);
					return;
				}

				if ( expectedChildren[ position ].GetType() != actualChildren[ position ].GetType() )
				{
					Fail(
						messageProvider,
						Properties.Resources.XmlAssert_NodesAreNotEqual,
						Environment.NewLine,
						position,
						BuildXPath( parent ),
						expectedChildren[ position ],
						expectedChildren[ position ].GetType().Name,
						actualChildren[ position ],
						actualChildren[ position ].GetType().Name
					);
					return;
				}

				Func<string> format =
					() =>
						String.Format(
							CultureInfo.CurrentCulture,
							Properties.Resources.XmlAssert_ChildNodesAssertionTemplate,
							Environment.NewLine,
							messageProvider == null ? String.Empty : messageProvider(),
							position
						);
				var asElement = expectedChildren[ position ] as XElement;
				if ( asElement != null )
				{
					AreEqualCore( asElement, actualChildren[ position ] as XElement, format );
					continue;
				}

				var asComment = expectedChildren[ position ] as XComment;
				if ( asComment != null )
				{
					AreEqualCore( asComment, actualChildren[ position ] as XComment, format );
					continue;
				}

				var asText = expectedChildren[ position ] as XText;
				if ( asText != null )
				{
					AreEqualCore( asText, actualChildren[ position ] as XText, format );
					continue;
				}

				var asPI = expectedChildren[ position ] as XProcessingInstruction;
				if ( asPI != null )
				{
					AreEqualCore( asPI, actualChildren[ position ] as XProcessingInstruction, format );
					continue;
				}

				var asDtd = expectedChildren[ position ] as XDocumentType;
				if ( asDtd != null )
				{
					AreEqualCore( asDtd, actualChildren[ position ] as XDocumentType, format );
					continue;
				}
			} // for

			if ( position < expectedChildren.Length )
			{
				Fail(
					messageProvider,
					Properties.Resources.XmlAssert_ChildNodeDoesNotExist,
					Environment.NewLine,
					GetNodeName( expectedChildren[ position ] ),
					expectedChildren[ position ].GetType().Name,
					position,
					BuildXPath( parent )
				);
			}
			else if ( position < actualChildren.Length )
			{
				Fail(
					messageProvider,
					Properties.Resources.XmlAssert_SomeExtraChildNodesExist,
					Environment.NewLine,
					position,
					String.Join( ", ", actualChildren.Skip( position ).Select( node => "'" + GetNodeName( node ) + "'(" + node.GetType().Name + ")" ) ),
					BuildXPath( parent )
				);
			}
		}

		private static string GetNodeName( XNode node )
		{
			var asElement = node as XElement;
			if ( asElement != null )
			{
				return asElement.Name.ToString();
			}

			var asCData = node as XCData;
			if ( asCData != null )
			{
				return "<![CDATA[" + asCData.Value + "]]>";
			}

			var asText = node as XText;
			if ( asText != null )
			{
				return "\"" + asText.Value + "\"";
			}

			var asComment = node as XComment;
			if ( asComment != null )
			{
				return "<!--" + asComment.Value + "-->";
			}

			var asDocument = node as XDocument;
			if ( asDocument != null )
			{
				return asDocument.Root.Name.ToString();
			}

			var asDocumentType = node as XDocumentType;
			if ( asDocumentType != null )
			{
				return asDocumentType.ToString();
			}

			var asPI = node as XProcessingInstruction;
			if ( asPI != null )
			{
				return asPI.ToString();
			}

			return String.Format( CultureInfo.InvariantCulture, "{0}({1})", node.GetType().FullName, node.ToString( SaveOptions.DisableFormatting ) );
		}

		private static readonly DelegateEqualityComparer<KeyValuePair<XName, XAttribute>> _xAttributeNameComparer =
			new DelegateEqualityComparer<KeyValuePair<XName, XAttribute>>( ( left, right ) => left.Key == right.Key, item => item.Key.GetHashCode() );

		private static bool AssertWhetherNull<T>( T expected, T actual, Func<string> userMessageProvider, Func<T, string> toString )
		{
			if ( Object.ReferenceEquals( expected, null ) )
			{
				if ( Object.ReferenceEquals( actual, null ) )
				{
					return true;
				}
				else
				{
					Fail( userMessageProvider, Properties.Resources.XmlAssert_ActualIsNotNull, toString( actual ), actual.GetType().Name );
				}
			}
			else if ( object.ReferenceEquals( actual, null ) )
			{
				Fail( userMessageProvider, Properties.Resources.XmlAssert_ActualIsNull, toString( expected ), expected.GetType().Name );
			}

			return false;
		}

		private static void Fail( Func<string> userMessageProvider, string format, params object[] args )
		{
			if ( userMessageProvider == null )
			{
				Assert.Fail( format, args );
			}
			else
			{
				Assert.Fail( "{1}{0}{2}", Environment.NewLine, userMessageProvider(), String.Format( CultureInfo.CurrentCulture, format, args ) );
			}
		}

		internal static string BuildXPath( XAttribute attribute )
		{
			var stack = new Stack<string>();
			stack.Push( "@" + attribute.Name );

			for ( var element = attribute.Parent; element != null; element = element.Parent )
			{
				stack.Push( element.Name.ToString() );
			}

			stack.Push( String.Empty );

			return String.Join( "/", stack );
		}

		internal static string BuildXPath( XNode node )
		{
			if ( node == null )
			{
				return Properties.Resources.XmlAssert_XPathLabelUnknown;
			}

			var asDocument = node as XDocument;
			if ( asDocument != null )
			{
				return "/" + asDocument.Root.Name;
			}

			var stack = new Stack<string>();

			if ( node is XCData )
			{
				stack.Push( Properties.Resources.XmlAssert_XPathLabelCData );
			}
			else if ( node is XText )
			{
				stack.Push( Properties.Resources.XmlAssert_XPathLabelTextContent );
			}
			else if ( node is XComment )
			{
				stack.Push( Properties.Resources.XmlAssert_XPathLabelComment );
			}
			else
			{
				var asElement = node as XElement;
				if ( asElement != null )
				{
					stack.Push( asElement.Name.ToString() );
				}
				else
				{
					stack.Push( String.Format( CultureInfo.InvariantCulture, "{0}({1})", node.GetType().FullName, node.ToString( SaveOptions.DisableFormatting ) ) );
				}
			}

			for ( var element = node.Parent; element != null; element = element.Parent )
			{
				stack.Push( element.Name.ToString() );
			}

			stack.Push( String.Empty );

			return String.Join( "/", stack );
		}

	}
}

