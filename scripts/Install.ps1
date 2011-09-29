# Removes extra *.Contract Assembly references
param($installPath, $toolsPath, $package, $project)
$project.Object.References | Where-Object { $_.Name -match "NLiblet\..+\.Contracts" } | ForEach-Object { $_.Remove() }