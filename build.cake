///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildNumber = Argument("buildNumber", 0);

var csproj = @".\AlienArc.PCanIntegration\AlienArc.PCanIntegration.csproj";
var nuspec = @".\AlienArc.PCanIntegration\AlienArc.PCanIntegration.nuspec";
var pcanVersionFile = @".\PCanBasicVersion.txt";

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(ctx =>
{
   // Executed BEFORE the first task.
   Information("Running tasks...");
});

Teardown(ctx =>
{
   // Executed AFTER the last task.
   Information("Finished running tasks.");
});

///////////////////////////////////////////////////////////////////////////////
// TASKS
///////////////////////////////////////////////////////////////////////////////

Task("Default")
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
    nupackSettings.OutputDirectory = @".\nuget";
    NuGetPack(nuspec, nupackSettings);
});

RunTarget(target);