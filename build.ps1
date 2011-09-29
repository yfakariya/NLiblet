# Build with MSBiuld/XBuild

param([string]$targetFile, [string]$config = "Release", [string]$target = "Build", [string]$outDir = "" )

[string]$msbuild
switch( [Environment]::OSVersion.Platform )
{
	{ $_ -eq [PlatformID]::Win32Windows -or $_ -eq [PlatformID]::Win32NT }
	{
		$framworkRoot
		if( [IntPtr]::Size -eq 8 )
		{
			$framworkRoot = "$env:SystemRoot\Microsoft.NET\Framework64"
		}
		else
		{
			$framworkRoot = "$env:SystemRoot\Microsoft.NET\Framework"
		}
		
		$frameworkDirectories = dir $framworkRoot "v*"
		$msbuild = [IO.Path]::Combine( $frameworkDirectories[ $frameworkDirectories.Length - 1 ].FullName, "MSBuild.exe" )
	}
	{ $_ -eq [PlatformID]::Unix -or $_ -eq [PlatformID]::MacOSX }
	{
		$msbuild = "$env:MONO_ROOT/bin/xbuild"
	}
	default
	{
		throw ( "Unknown platform {0}." -f [Environment]::OSVersion.Platform )
	}
}


if( ![String]::IsNullOrEmpty( $outdir ) )
{
	$outDir = [IO.Path]::GetFullPath( $outdir )
	& $msbuild $targetFile -t:$target -p:Configuration=$config -p:OutDir=$outdir
}
else
{
	& $msbuild $targetFile -t:$target -p:Configuration=$config
}
	
exit $LastExitCode