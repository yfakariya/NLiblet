using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Mono.Options;

namespace UpdateVersion
{
	internal sealed class Program
	{
		private static int Main( string[] args )
		{
			try
			{
				ushort? major = null;
				ushort? minor = null;
				ushort? revision = null;
				ushort? build = null;
				bool incrementsMajor = false;
				bool incrementsMinor = false;
				bool incrementsBuild = false;
				bool show = false;
				bool help = false;
				string suffix = null;
				bool noSuffix = false;
				string baseDirectory = "src";

				var options =
					new Mono.Options.OptionSet()
				{
					{ 
						"show",
						"Show each project versions. This options is exclusive.",
						arg => show = arg != null
					},
					{ 
						"d=|base-directory=",
						"Set base directory of source tree, e.g. \"src\".",
						( string value ) => baseDirectory = value
					},
					{ 
						"j|increments-major",
						"Increments Major version. If --major is specified, -j is ignored.",
						arg => incrementsMajor = arg != null 
					},
					{ 
						"n|increments-minor", 
						"Increments Minor version. If --minor is specified, -n is ignored.",
						arg => incrementsMinor = arg != null 
					},
					{ 
						"b|increments-build", 
						"Increments Build number. If --build is specified, -b is ignored.",
						arg => incrementsBuild = arg != null 
					},
					{ 
						"major=", 
						"Set Major version as spcified value. Valid value is [0-65535]. If -j is specified, -j is ignored.",
						( ushort value ) => major = value 
					},
					{ 
						"minor=", 
						"Set Major version as spcified value. Valid value is [0-65535]. If -j is specified, -j is ignored.",
						( ushort value ) => minor = value 
					},
					{ 
						"build=", 
						"Set Build as spcified value.  If -f or -n is specified, these are ignored.",
						( ushort value ) => build = value
					},
					{ 
						"revision=",
						"Set Revision as spcified value.",
						( ushort value ) => revision = value 
					},
					{ 
						"suffix=", 
						"Set Suffix of informational version. Default is empty (i.e. no suffix)",
						( string value )=> suffix = value
					},
					{ 
						"no-suffix", 
						"Clear Suffix of informational version.",
						arg => noSuffix = arg != null
					},
					{ 
						"?|h|help", 
						"Show this help message.",
						arg => help = arg != null
					},
				};

				List<string> targets;
				try
				{
					targets = options.Parse( args );
				}
				catch ( OptionException ex )
				{
					Console.Error.WriteLine( ex );
					ShowHelp( options, Console.Error );
					return Marshal.GetHRForException( new ArgumentException() );
				}

				if ( help )
				{
					ShowHelp( options, Console.Out );
					return 1;
				}

				if ( !Directory.Exists( baseDirectory ) )
				{
					Console.Error.WriteLine( "Base directory \"{0}\" does not exist.", Path.GetFullPath( baseDirectory ) );
					ShowHelp( options, Console.Error );
					return Marshal.GetHRForException( new FileNotFoundException() );
				}

				CheckArgumentConfliction( major, minor, build, incrementsMajor, incrementsMinor, incrementsBuild, noSuffix, suffix );

				if ( targets.Any() )
				{
					VerifyTargets( baseDirectory, targets );
				}
				else
				{
					FillTargets( baseDirectory, targets );
				}


				if ( show )
				{
					ShowProjectVersions( baseDirectory, targets );
					return 0;
				}

				new Program( major, minor, revision, build, incrementsMajor, incrementsMinor, incrementsBuild, noSuffix ? null : suffix )
					.UpdateVersions( baseDirectory, targets );
				return 0;
			}
			catch ( Exception ex )
			{
				Console.Error.WriteLine( ex );
				return Marshal.GetHRForException( ex );
			}
		}

		private static void ShowHelp( OptionSet options, TextWriter output )
		{
			output.WriteLine( "Usage: {0} <Options>", typeof( Program ).Assembly.ManifestModule.Name );
			options.WriteOptionDescriptions( output );
		}

		private static void CheckArgumentConfliction( ushort? major, ushort? minor, ushort? build, bool incrementsMajor, bool incrementsMinor, bool incrementsBuild, bool noSuffix, string suffix )
		{
			if ( major != null && incrementsMajor )
			{
				Console.Error.WriteLine( "-j (increments-major) option is ignored due to explicitly specified via --major." );
			}

			if ( minor != null && incrementsMinor )
			{
				Console.Error.WriteLine( "-n (increments-minor) option is ignored due to explicitly specified via --minor." );
			}

			if ( build != null && incrementsBuild )
			{
				Console.Error.WriteLine( "-b (increments-build) option is ignored due to explicitly specified via --build." );
			}

			if ( incrementsMajor )
			{
				if ( incrementsMinor )
				{
					Console.Error.WriteLine( "-i (increments-minor) option is ignored because -j (increments-major) is specified." );
				}

				if ( incrementsBuild )
				{
					Console.Error.WriteLine( "-b (increments-build) option is ignored because -j (increments-major) is specified." );
				}
			}
			else if ( incrementsMinor )
			{
				if ( incrementsBuild )
				{
					Console.Error.WriteLine( "-b (increments-build) option is ignored because -i (increments-minor) is specified." );
				}
			}

			if ( !String.IsNullOrWhiteSpace( suffix ) && noSuffix )
			{
				Console.Error.WriteLine( "--suffix option is ignored due to --no-suffix." );
			}
		}

		private static bool HasMainAndTest( string baseDirectory )
		{
			var subDirectories = Directory.GetDirectories( baseDirectory );
			return subDirectories.Contains( "main", StringComparer.Ordinal ) && ( subDirectories.Contains( "test", StringComparer.Ordinal ) || subDirectories.Contains( "tests", StringComparer.Ordinal ) );
		}

		private static void VerifyTargets( string baseDirectory, List<string> targets )
		{
			string targetDirectory = HasMainAndTest( baseDirectory ) ? baseDirectory + Path.DirectorySeparatorChar + "main" : baseDirectory;
			var missings = targets.Except( Directory.GetDirectories( targetDirectory ), StringComparer.OrdinalIgnoreCase );
			if ( missings.Any() )
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentCulture,
						"Some projects do not exist: [{0}]",
						String.Join( CultureInfo.CurrentCulture.TextInfo.ListSeparator, missings )
					),
					"targets"
				);
			}
		}

		private static void FillTargets( string baseDirectory, List<string> targets )
		{
			targets.AddRange(
				new DirectoryInfo( baseDirectory )
				.EnumerateDirectories()
				.Where( dir =>
					!dir.Name.StartsWith( "." )
					&& ( dir.Attributes & FileAttributes.Hidden ) == 0
					&& ( dir.Attributes & FileAttributes.System ) == 0
				).Select( dir => dir.FullName )
			);
		}

		private static void ShowProjectVersions( string baseDirectory, List<string> targets )
		{
			var commonAssemblyInfos = FindCommonAssemblyInfo( baseDirectory );
			var commonAsesmblyVersion = commonAssemblyInfos.Select( item => item.AssemblyVersion ).FirstOrDefault( item => item != null );
			var commonAsesmblyFileVersion = commonAssemblyInfos.Select( item => item.AssemblyFileVersion ).FirstOrDefault( item => item != null );
			var commonAsesmblyInformationalVersion = commonAssemblyInfos.Select( item => item.FullAssemblyInformationalVersion ).FirstOrDefault( item => item != null );

			foreach ( var target in targets )
			{
				ShowVersionInformation( target, commonAsesmblyVersion, commonAsesmblyFileVersion, commonAsesmblyInformationalVersion );
			}
		}

		private static void ShowVersionInformation( string target, Version commonAsesmblyVersion, Version commonAsesmblyFileVersion, string commonAsesmblyInformationalVersion )
		{
			// Get *.*proj
			var projectFile = Directory.EnumerateFiles( target, "*.*proj" ).FirstOrDefault();
			// Get extension
			var extension = projectFile == null ? ".cs" : _sourceExtensionExtractor.Match( projectFile ).Groups[ "SourceExtension" ].Value;
			// Get files and calc newest
			var updateTimes =
				Directory.EnumerateFileSystemEntries( target, "*." + extension, SearchOption.AllDirectories )
				.OfType<FileInfo>().ToArray();
			var newestUpdateTime = updateTimes.Length == 0 ? File.GetLastWriteTimeUtc( projectFile ) : updateTimes.Max( file => file.LastWriteTimeUtc );
			// Find AssemblyInfo and update
			Version assemblyVersion = commonAsesmblyVersion;
			Version assemblyFileVersion = commonAsesmblyFileVersion;
			string assemblyInformationalVersion = commonAsesmblyInformationalVersion;

			foreach ( var assemblyInfo in
				Directory.EnumerateFiles( target, "AssemblyInfo*" + extension, SearchOption.AllDirectories )
				.Select( file => VersionInformation.Create( new FileInfo( file ), Console.Out ) ) )
			{
				if ( assemblyVersion == null && assemblyInfo.AssemblyVersion != null )
				{
					assemblyVersion = assemblyInfo.AssemblyVersion;
				}

				if ( assemblyFileVersion == null && assemblyInfo.AssemblyFileVersion != null )
				{
					assemblyFileVersion = assemblyInfo.AssemblyFileVersion;
				}

				if ( assemblyInformationalVersion == null && assemblyInfo.AssemblyInformationalVersion != null )
				{
					assemblyInformationalVersion = assemblyInfo.FullAssemblyInformationalVersion;
				}
			}

			Console.WriteLine( "{0} [{1}]", target, projectFile == null ? "(project file not found)" : Path.GetFullPath( projectFile ) );

			if ( assemblyVersion != null )
			{
				Console.WriteLine( "\tAssemblyVersion:              {0}", assemblyVersion );
			}

			if ( assemblyFileVersion != null )
			{
				Console.WriteLine( "\tAssemblyFileVersion:          {0}", assemblyFileVersion );
			}

			if ( assemblyInformationalVersion != null )
			{
				Console.WriteLine( "\tAssemblyInformationalVersion: {0}", assemblyInformationalVersion );
			}

			Console.WriteLine();
		}

		private readonly ushort? _major;
		private readonly ushort? _minor;
		private readonly ushort? _revision;
		private readonly ushort? _build;
		private readonly bool _incrementsMajor;
		private readonly bool _incrementsMinor;
		private readonly bool _incrementsBuild;
		private readonly string _suffix;

		private Program( ushort? major, ushort? minor, ushort? revision, ushort? build, bool incrementsMajor, bool incrementsMinor, bool incrementsBuild, string suffix )
		{
			this._major = major;
			this._minor = minor;
			this._revision = revision;
			this._build = build;
			this._incrementsMajor = incrementsMajor;
			this._incrementsMinor = incrementsMinor;
			this._incrementsBuild = incrementsBuild;
			this._suffix = suffix;
		}

		private void UpdateVersions( string baseDirectory, List<string> targets )
		{
			DateTime newestUpdateTime = DateTime.MinValue.ToUniversalTime();
			foreach ( var target in targets )
			{
				var projectNewestUpdateTime = UpdateVersion( target ).ToUniversalTime();
				if ( newestUpdateTime < projectNewestUpdateTime )
				{
					newestUpdateTime = projectNewestUpdateTime;
				}
			}

			foreach ( var commonAssemblyInfo in FindCommonAssemblyInfo( baseDirectory ) )
			{
				Update( commonAssemblyInfo, newestUpdateTime );
			}
		}

		private void Update( VersionInformation versionInfomation, DateTime newestUpdateTime )
		{
			versionInfomation.SetVersion( this._major, this._minor, this._build, this._revision, this._incrementsMajor, this._incrementsMinor, this._incrementsBuild, this._suffix, newestUpdateTime );
			versionInfomation.Overwrite( Console.Out );
		}

		private static IEnumerable<VersionInformation> FindCommonAssemblyInfo( string baseDirectory )
		{
			for ( var dir = new DirectoryInfo( baseDirectory ); dir != null; dir = dir.Parent )
			{
				bool found = false;
				foreach ( var info in dir.EnumerateFiles( "CommonAssemblyInfo*" ) )
				{
					found = true;
					yield return VersionInformation.Create( info, Console.Out );
				}

				if ( found )
				{
					yield break;
				}
			}
		}

		private static readonly Regex _sourceExtensionExtractor =
			new Regex( "(?<SourceExtension>\\.[a-z]+)proj$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Singleline );

		private DateTime UpdateVersion( string target )
		{
			// Get *.*proj
			var projectFile = Directory.EnumerateFiles( target, "*.*proj" ).FirstOrDefault() ?? ".csproj";
			// Get extension
			var extension = _sourceExtensionExtractor.Match( projectFile ).Groups[ "SourceExtension" ].Value;
			// Get files and calc newest
			var updateTimes =
				Directory.EnumerateFileSystemEntries( target, "*." + extension, SearchOption.AllDirectories )
				.OfType<FileInfo>()
				.ToArray();

			if ( updateTimes.Length == 0 )
			{
				return File.GetLastWriteTimeUtc( projectFile );
			}

			var newestUpdateTime = updateTimes.Max( file => file.LastWriteTimeUtc );

			// Find AssemblyInfo and update
			foreach ( var assemblyInfo in
				Directory.EnumerateFileSystemEntries( target, "AssemblyInfo*" + extension, SearchOption.AllDirectories )
				.Select( file => VersionInformation.Create( new FileInfo( file ), Console.Out ) ) )
			{
				Update( assemblyInfo, newestUpdateTime );
			}

			return newestUpdateTime;
		}
	}
}