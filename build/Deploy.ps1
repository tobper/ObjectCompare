Param (
    $variables = @{},   
    $artifacts = @{},
    $scriptPath,
    $buildFolder,
    $srcFolder,
    $outFolder,
    $tempFolder,
    $projectName,
    $projectVersion,
    $projectBuildNumber
)

& $srcFolder\tools\NuGet.exe pack $srcFolder\src\ObjectCompare.nuspec -BasePath $tempFolder\$projectName -Version $projectVersion -OutputDirectory $outFolder -Symbols
& $srcFolder\tools\NuGet.exe push $outFolder\$projectName.$projectVersion.nupkg -ApiKey $variables["SecureNuGetApiKey"] -NonInteractive
