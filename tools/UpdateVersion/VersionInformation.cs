using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace UpdateVersion
{
	internal class VersionInformation
	{
		private static readonly DateTime _epoc = new DateTime( 2000, 1, 1, 0, 0, 0, DateTimeKind.Utc );

		private readonly Regex _regex;
		public readonly FileInfo TargetFile;
		public Version AssemblyVersion;
		public readonly MatchLine AssemblyVersionPosition;
		public Version AssemblyFileVersion;
		public readonly MatchLine AssemblyFileVersionPosition;
		public string AssemblyInformationalVersion;
		public string AssemblyInformationalVersionSuffix;
		public readonly MatchLine AssemblyInformationalVersionPosition;

		private VersionInformation(
			Regex regex,
			FileInfo targetFile,
			Version assemblyVersion,
			MatchLine assemblyVersionPosition,
			Version assemblyFileVersion,
			MatchLine assemblyFileVersionPosition,
			string assemblyInformationalVersion,
			string assemblyInformationalVersionSuffix,
			MatchLine assemblyInformationalVersionPosition
		)
		{
			this._regex = regex;
			this.TargetFile = targetFile;
			this.AssemblyVersion = assemblyVersion;
			this.AssemblyVersionPosition = assemblyVersionPosition;
			this.AssemblyFileVersion = assemblyFileVersion;
			this.AssemblyFileVersionPosition = assemblyFileVersionPosition;
			this.AssemblyInformationalVersion = assemblyInformationalVersion;
			this.AssemblyInformationalVersionSuffix = assemblyInformationalVersionSuffix;
			this.AssemblyInformationalVersionPosition = assemblyInformationalVersionPosition;
		}

		public void SetVersion( ushort? major, ushort? minor, ushort? build, ushort? revision, bool incrementsMajor, bool incrementsMinor, bool incrementsBuild, string suffix, DateTime newestUpdateTime )
		{
			ushort currentMajor =
				( ushort )( this.AssemblyVersion ?? this.AssemblyFileVersion ?? new Version() ).Major;
			ushort currentMinor =
				( ushort )( this.AssemblyVersion ?? this.AssemblyFileVersion ?? new Version() ).Minor;
			ushort currentBuild =
				( ushort )( this.AssemblyVersion ?? this.AssemblyFileVersion ?? new Version() ).Build;
			ushort currentRevision =
				( ushort )( this.AssemblyVersion ?? this.AssemblyFileVersion ?? new Version() ).Revision;

			ushort epocRevision = ( ushort )newestUpdateTime.ToUniversalTime().Subtract( _epoc ).Days;


			if ( incrementsBuild )
			{
				currentBuild++;
			}

			if ( incrementsMinor )
			{
				currentMinor++;
				currentBuild = 0;
			}

			if ( incrementsMajor )
			{
				currentMajor++;
				currentBuild = 0;
				currentMinor = 0;
			}

			ushort finalMajor = major ?? currentMajor;
			ushort finalMinor = minor ?? currentMinor;
			ushort finalBuild = build ?? currentBuild;
			ushort finalRevision = revision ?? currentRevision;

			this.AssemblyVersion = new Version( finalMajor, finalMinor, finalBuild, 0 );
			this.AssemblyFileVersion = new Version( finalMajor, finalMinor, finalBuild, finalRevision );

			var informationalVersion = new StringBuilder();
			informationalVersion.Append( finalMajor ).Append( "." ).Append( finalMinor );
			if ( 0 < finalBuild )
			{
				informationalVersion.Append( "." ).Append( finalBuild );

				if ( revision != null )
				{
					informationalVersion.Append( "." ).Append( finalRevision );
				}
			}

			this.AssemblyInformationalVersion = informationalVersion.ToString();

			if ( suffix != null )
			{
				this.AssemblyInformationalVersionSuffix = suffix.Trim();
			}
		}

		public void Overwrite( TextWriter traceWriter )
		{
			File.WriteAllLines(
				this.TargetFile.FullName,
				File.ReadLines( this.TargetFile.FullName, Encoding.UTF8 )
				.Zip( EnumerableEx.Generate( 0, _ => true, i => i + 1, i => i ),
					( line, i ) => new { SourceLine = i, Line = line, Match = this._regex.Match( line ) }
				).Select( item =>
					{
						if ( !item.Match.Success )
						{
							return item.Line;
						}

						if ( this.AssemblyVersion != null && item.SourceLine == this.AssemblyVersionPosition.SourceLine )
						{
							traceWriter.WriteLine( "Set {0} to {1} ({2}).", typeof( AssemblyVersionAttribute ).Name, this.AssemblyVersion, this.TargetFile.FullName );
							return Replace( item.Match, this.AssemblyVersion );
						}
						else if ( this.AssemblyFileVersionPosition != null && item.SourceLine == this.AssemblyFileVersionPosition.SourceLine )
						{
							traceWriter.WriteLine( "Set {0} to {1} ({2}).", typeof( AssemblyFileVersionAttribute ).Name, this.AssemblyFileVersion, this.TargetFile.FullName );
							return Replace( item.Match, this.AssemblyFileVersion );
						}
						else if ( this.AssemblyInformationalVersionPosition != null && item.SourceLine == this.AssemblyInformationalVersionPosition.SourceLine )
						{
							traceWriter.WriteLine( 
								"Set {0} to {1}{2} ({3}).", 
								typeof( AssemblyInformationalVersionAttribute ).Name, 
								this.AssemblyInformationalVersion, 
								String.IsNullOrWhiteSpace( this.AssemblyInformationalVersionSuffix ) ? String.Empty : "-" + this.AssemblyInformationalVersionSuffix, this.TargetFile.FullName 
							);
							return Replace( item.Match, this.AssemblyInformationalVersion, this.AssemblyInformationalVersionSuffix );
						}

						return item.Line;
					}
				)
			);
		}

		private static string Replace( Match match, Version version )
		{
			return match.Result( "${Leading}${Type}${Delimiter}" + version + "${Trailing}" );
		}

		private string Replace( Match match, string version, string suffix )
		{
			return match.Result( "${Leading}${Type}${Delimiter}" + version + ( !String.IsNullOrWhiteSpace( suffix ) ? "-" + suffix : String.Empty ) + "${Trailing}" );
		}

		public static VersionInformation Create( FileInfo targetFile, TextWriter traceWriter )
		{
			var attributeTypeExpressions = new Dictionary<Type, string>();
			var regex = CreateAttributeReplacementRegex( targetFile, attributeTypeExpressions );
			var matchLines =
				File.ReadLines( targetFile.FullName, Encoding.UTF8 )
				.Select( line => regex.Match( line ) )
				.Where( match => match.Success )
				.Zip( EnumerableEx.Generate( 0, _ => true, i => i + 1, i => i ),
					( match, i ) => new MatchLine() { SourceLine = i, Match = match }
				).ToArray();

			Version assemblyVersion, assemblyFileVersion;
			MatchLine assemblyVersionPosition, assemblyFileVersionPosition, assemblyInformationalVersionPosition;
			string assemblyInformationalVersion, assemblyInformationalVersionSuffix;
			ExtractVersion( matchLines, typeof( AssemblyVersionAttribute ), attributeTypeExpressions[ typeof( AssemblyVersionAttribute ) ], traceWriter, out assemblyVersionPosition, out assemblyVersion );
			ExtractVersion( matchLines, typeof( AssemblyFileVersionAttribute ), attributeTypeExpressions[ typeof( AssemblyFileVersionAttribute ) ], traceWriter, out assemblyFileVersionPosition, out assemblyFileVersion );
			ExtractVersion( matchLines, typeof( AssemblyInformationalVersionAttribute ), attributeTypeExpressions[ typeof( AssemblyInformationalVersionAttribute ) ], traceWriter, out assemblyInformationalVersionPosition, out assemblyInformationalVersion, out assemblyInformationalVersionSuffix );

			return
				new VersionInformation(
					regex,
					targetFile,
					assemblyVersion,
					assemblyVersionPosition,
					assemblyFileVersion,
					assemblyFileVersionPosition,
					assemblyInformationalVersion,
					assemblyInformationalVersionSuffix,
					assemblyInformationalVersionPosition
				);
		}

		private static void ExtractVersion( MatchLine[] matchLines, Type attributeType, string qualifiedTypeName, TextWriter traceWriter, out MatchLine line, out Version version )
		{
			line = ExtractVersion( matchLines, attributeType, qualifiedTypeName );
			if ( line == null )
			{
				version = null;
				return;
			}
			else
			{
				var value = line.Match.Groups[ "Value" ];
				version = value.Success ? new Version( value.Value ) : null;
			}
		}

		private static void ExtractVersion( MatchLine[] matchLines, Type attributeType, string qualifiedTypeName, TextWriter traceWriter, out MatchLine line, out string version, out String suffix )
		{
			line = ExtractVersion( matchLines, attributeType, qualifiedTypeName );
			if ( line == null )
			{
				version = null;
				suffix = null;
				return;
			}
			else
			{
				version = line.Match.Groups[ "Value" ].Value;
				suffix = line.Match.Groups[ "Suffix" ].Value;
			}
		}

		private static MatchLine ExtractVersion( MatchLine[] matchLines, Type attributeType, string qualifiedTypeName )
		{
			return
				matchLines
				.FirstOrDefault( item =>
					String.Equals( item.Match.Groups[ "Type" ].Value, attributeType.Name, StringComparison.OrdinalIgnoreCase )
					|| String.Equals( item.Match.Groups[ "Type" ].Value, attributeType.FullName, StringComparison.OrdinalIgnoreCase )
					|| String.Equals( item.Match.Groups[ "Type" ].Value, qualifiedTypeName, StringComparison.OrdinalIgnoreCase )
					|| String.Equals( item.Match.Groups[ "Type" ].Value + "Attribute", attributeType.Name, StringComparison.OrdinalIgnoreCase )
					|| String.Equals( item.Match.Groups[ "Type" ].Value + "Attribute", attributeType.FullName, StringComparison.OrdinalIgnoreCase )
					|| String.Equals( item.Match.Groups[ "Type" ].Value + "Attribute", qualifiedTypeName, StringComparison.OrdinalIgnoreCase )
				);
		}

		private static Regex CreateAttributeReplacementRegex( FileInfo targetFile, Dictionary<Type, string> attributeTypeExpressions )
		{
			if ( !CodeDomProvider.IsDefinedExtension( targetFile.Extension ) )
			{
				throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, "Unknon extension \"{0}\" of attribute  file \"{1}\".", targetFile.Extension, targetFile.FullName ) );
			}

			var language = CodeDomProvider.GetLanguageFromExtension( targetFile.Extension );
			if ( !CodeDomProvider.IsDefinedLanguage( language ) )
			{
				throw new NotSupportedException( String.Format( CultureInfo.CurrentCulture, "Lanuage \"{0}\" (*{1}) is not supported.", language, targetFile.Extension ) );
			}

			const string dummyType = "TYPE";
			const string dummyArg = "ARG";
			var provider = CodeDomProvider.CreateProvider( language );
			var compileUnit = new CodeCompileUnit();
			compileUnit.AssemblyCustomAttributes.Add(
				new CodeAttributeDeclaration( dummyType, new CodeAttributeArgument( new CodePrimitiveExpression( dummyArg ) ) )
			);
			var writer = new StringWriter();
			provider.GenerateCodeFromCompileUnit( compileUnit, writer, new CodeGeneratorOptions() { IndentString = String.Empty } );
			var template = Regex.Escape( writer.ToString() );
			template = template.Replace( "TYPE", "[A-Za-z_][A-Za-z0-9_]*" );
			var regex =
				new Regex(
					Regex.Replace(
						template,
						@"^(.*)(TYPE)(.*)(\p{P})(ARG)(\p{P})(.*)$",
						match =>
							"^(?<Leading>\\s*" +
							Regex.Escape( match.Groups[ 0 ].Value ) +
							")" +
							"(?<Type>[A-Za-z_][A-Za-z0-9_]*)" + // 1
							"(?<Delimiter>" +
							Regex.Escape( match.Groups[ 2 ].Value ) +
							"\\s*" +
							Regex.Escape( match.Groups[ 3 ].Value ) +
							")" +
							"(?<Value>[0-9]+(\\.[0-9]+(\\.[0-9]+(\\.[0-9]+)?)?)?)" +
							"-" +
							"(?<Suffix>[^" + Regex.Escape( match.Groups[ 5 ].Value ) + "]*)" +
							"(?<Trailing>" +
							Regex.Escape( match.Groups[ 5 ].Value ) +
							"\\s*" +
							Regex.Escape( match.Groups[ 6 ].Value ) +
							"\\s*)$"
					),
					RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Singleline
				);

			attributeTypeExpressions[ typeof( AssemblyVersionAttribute ) ] = GetAttributeTypeExpression( provider, typeof( AssemblyVersionAttribute ) );
			attributeTypeExpressions[ typeof( AssemblyFileVersionAttribute ) ] = GetAttributeTypeExpression( provider, typeof( AssemblyFileVersionAttribute ) );
			attributeTypeExpressions[ typeof( AssemblyInformationalVersionAttribute ) ] = GetAttributeTypeExpression( provider, typeof( AssemblyInformationalVersionAttribute ) );
			return regex;
		}

		private static string GetAttributeTypeExpression( CodeDomProvider provider, Type attributeType )
		{
			var writer = new StringWriter();
			provider.GenerateCodeFromExpression(
				new CodeTypeReferenceExpression( attributeType ),
				writer,
				new CodeGeneratorOptions() { IndentString = String.Empty }
			);
			return writer.ToString();
		}
	}
}
