# Packaging scripts
# Path to Nuget.exe is required.

param( $baseDirectory, $config = "Release", $outDirectory = ".\dist" )

if( ![IO.Directory]::Exists( $outDirectory ) )
{
	[void]( mkdir $outDirectory )
}

# & .\build.ps1 Release Build 

foreach( $projectDirectory in ( dir $baseDirectory ) )
{
	foreach( $projectFile in ( dir $projectDirectory.FullName "*.*proj" ) )
	{
		if( ![IO.File]::Exists( [IO.Path]::ChangeExtension( $projectFile.FullName, ".nuspec" ) ) )
		{
			break
		}
		
		Nuget pack $projectFile.FullName -Build -Properties "Configuration=$Config" -OutputDirectory $outDirectory -Verbose
		if( $LastExitCode -ne 0 )
		{
			exit $LastExitCode
		}
		break
	}
}
