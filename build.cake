#addin "nuget:?package=NuGet.Core"

///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildNumber = Argument("buildNumber", 0);
var targetPath = Argument("targetPath", @".\nuget");

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

var solution = @".\AlienArc.PCanIntegration.sln";
var csproj = @".\AlienArc.PCanIntegration\AlienArc.PCanIntegration.csproj";
var nuspec = @".\AlienArc.PCanIntegration\AlienArc.PCanIntegration.nuspec";
var pcanVersionFile = @".\PCanBasicVersion.txt";

Setup(ctx =>
{
   // Executed BEFORE the first task.
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("RestorePackages")
.Does(() => {
    NuGetRestore(solution);
});

Task("Build")
    .IsDependentOn("RestorePackages")
.Does(() => {

    var pcanVersion = System.IO.File.ReadAllText(pcanVersionFile);
    var fullVersion = $"{pcanVersion}.{buildNumber}";
    var nugetVersion = $"{pcanVersion.Substring(0, pcanVersion.LastIndexOf("."))}.{buildNumber}";

    MSBuild(csproj, c => 
        c.WithProperty("Platform", "x86")
        .WithProperty("Configuration", configuration)
        .WithProperty("VersionAssembly", fullVersion)
    );
    MSBuild(csproj, c => 
        c.WithProperty("Platform", "x64")
        .WithProperty("Configuration", configuration)
        .WithProperty("VersionAssembly", fullVersion)
    );

    var nupackSettings = new NuGetPackSettings();
    nupackSettings.Version = nugetVersion;
    nupackSettings.OutputDirectory = targetPath;
    NuGetPack(nuspec, nupackSettings);
});

Task("Default")
    .IsDependentOn("RestorePackages")
    .IsDependentOn("Build");

RunTarget(target);