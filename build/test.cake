using System.Threading;
using System.Text.RegularExpressions;

//////////////////////////////////////////////////////////////////////
// ADDINS
//////////////////////////////////////////////////////////////////////

#addin "Cake.FileHelpers"
#addin "Cake.Incubator"
#addin "Cake.Watch"
#addin nuget:?package=Newtonsoft.Json
//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////

using Cake.Common.Build.TeamCity;
using Cake.Core.IO;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");

if (string.IsNullOrWhiteSpace(target))
{
    target = "Default";
}


//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Should MSBuild & GitLink treat any errors as warnings?
var treatWarningsAsErrors = false;
Func<string, int> GetEnvironmentInteger = name => {
	
	var data = EnvironmentVariable(name);
	int d = 0;
	if(!String.IsNullOrEmpty(data) && int.TryParse(data, out d)) 
	{
		return d;
	} 
	
	return 0;

};

// Load json Configuartion
var configFilePath = "./config.json";
JObject config;

if(!FileExists(configFilePath)) {
	
	throw new Exception(string.Format("config.json can not be found at {0}", configFilePath));
}

var configFile = File(configFilePath);

using(var stream = new StreamReader(System.IO.File.OpenRead(configFile.Path.FullPath))) {
	var json = stream.ReadToEnd();
	config = JObject.Parse(json);
};

if(config == null) {
	throw new Exception(string.Format("config.json can not be found at {0}", configFilePath));
}


// Build configuration
var local = BuildSystem.IsLocalBuild;
var isTeamCity = BuildSystem.TeamCity.IsRunningOnTeamCity;
var isRunningOnUnix = IsRunningOnUnix();
var isRunningOnWindows = IsRunningOnWindows();

// Artifacts
var artifactDirectory = "./artifacts/";
var buildSolution = config.Value<string>("solutionFile");
var configuration = "Release";
// Macros
Func<string> GetMSBuildLoggerArguments = () => {
    return BuildSystem.TeamCity.IsRunningOnTeamCity ? EnvironmentVariable("MsBuildLogger"): null;
};

Action Abort = () => { throw new Exception("A non-recoverable fatal error occurred."); };
Action<string> TestFailuresAbort = testResult => { throw new Exception(testResult); };

Action<string> RestorePackages = (solution) =>
{
    DotNetCoreRestore(solution);
	
};

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("RestorePackages")
.Does (() =>
{
	RestorePackages(buildSolution);
});


var testProject = config.Value<string>("testProjectPath");
Task("RunUnitTests")
    .Does(() =>
{
		var settings = new DotNetCoreTestSettings
		{
			Configuration = configuration
		};

		DotNetCoreTest(settings, testProject,  new XUnit2Settings {
			OutputDirectory = artifactDirectory,
            XmlReportV1 = false
		});
});

Task("WatchFiles")
    .IsDependentOn("RestorePackages")
    .Does(() =>
{
	var settings = new WatchSettings { Recursive = true, Path = "../src", Pattern = "*.cs" };
	Watch(settings , (changes) => {
	    var list = changes.ToList();
	    if(list.Count() > 0) {
	    	RunTarget("RunUnitTests");
	    }
	});
});


//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("WatchFiles")
    .Does (() =>
{

});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
