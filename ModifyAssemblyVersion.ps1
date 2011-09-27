# Update versions.
param( [string]$mode = "fix", [string]$suffix = $null, [bool]$forceRebuild = $false )

$baseDirectory = ".\src"
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
		
		$frameworkDirectories = dir $framworkRoot
		$msbuild = $frameworkDirectories[ $frameworkDirectories.Length ] + "MSBuild.exe"
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

if( $forceRebuild -or ![IO.File]::Exists( ".\tools\UpdateVersion\bin\UpdateVersion.exe" ) )
{
	& "$msbuild" ".\tools\UpdateVersion\UpdateVersion.csproj" "-Config=Release"
}

[Version]$version = $null
if( $versionString -ne $null )
{
	try
	{
		$version = new-object Version( $versionString )
	}
	catch { }
}
switch ( $mode )
{
	"show"
	{
		& ".\tools\UpdateVersion\bin\UpdateVersion.exe" -d "$baseDirectory" --show
	}
	"fix"
	{
		& ".\tools\UpdateVersion\bin\UpdateVersion.exe" -d "$baseDirectory" -b
	}
	"minor"
	{
		& ".\tools\UpdateVersion\bin\UpdateVersion.exe" -d "$baseDirectory" -n
	}
	"major"
	{
		& ".\tools\UpdateVersion\bin\UpdateVersion.exe" -d "$baseDirectory" -j
	}
	"beta"
	{
		& ".\tools\UpdateVersion\bin\UpdateVersion.exe" -d "$baseDirectory" -n --suffix Beta
	}
	"rc"
	{
		& ".\tools\UpdateVersion\bin\UpdateVersion.exe" -d "$baseDirectory" --suffix RC
	}
	"release"
	{
		& ".\tools\UpdateVersion\bin\UpdateVersion.exe" -d "$baseDirectory" --no-suffix
	}
	default
	{
		throw ( "Unknown mode `"{0}`"" -f $mode )
	}
}