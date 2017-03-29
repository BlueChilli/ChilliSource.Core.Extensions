using System.Threading;
using System.Text.RegularExpressions;

//////////////////////////////////////////////////////////////////////
// ADDINS
//////////////////////////////////////////////////////////////////////

#addin "Cake.FileHelpers"
#addin "Cake.Incubator"
//////////////////////////////////////////////////////////////////////
// TOOLS
//////////////////////////////////////////////////////////////////////

#tool "GitReleaseManager"
#tool "GitVersion.CommandLine"
#tool "GitLink"
using Cake.Common.Build.TeamCity;


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
// Build configuration
var local = BuildSystem.IsLocalBuild;
var isTeamCity = BuildSystem.TeamCity.IsRunningOnTeamCity;
var isRunningOnUnix = IsRunningOnUnix();
var isRunningOnWindows = IsRunningOnWindows();
var teamCity = BuildSystem.TeamCity;
var branch = EnvironmentVariable("Git_Branch");
var isPullRequest = !String.IsNullOrEmpty(branch) && branch.ToUpper().Contains("PULL-REQUEST"); //teamCity.Environment.PullRequest.IsPullRequest;
var projectName =  EnvironmentVariable("TEAMCITY_PROJECT_NAME"); //  teamCity.Environment.Project.Name;
var isRepository = StringComparer.OrdinalIgnoreCase.Equals("chillisource core", projectName);
var isTagged = !String.IsNullOrEmpty(branch) && branch.ToUpper().Contains("TAGS");
var buildConfName = EnvironmentVariable("TEAMCITY_BUILDCONF_NAME"); //teamCity.Environment.Build.BuildConfName
var buildNumber = GetEnvironmentInteger("BUILD_NUMBER");
var isReleaseBranch = StringComparer.OrdinalIgnoreCase.Equals("master", buildConfName);

var githubOwner = "BlueChilli";
var githubRepository = "ChilliSource.Core";
var githubUrl = string.Format("https://github.com/{0}/{1}", githubOwner, githubRepository);
var licenceUrl = string.Format("{0}/blob/master/LICENSE", githubUrl);

// Version
var gitVersion = GitVersion();
var majorMinorPatch = gitVersion.MajorMinorPatch;
var semVersion = gitVersion.SemVer;
var informationalVersion = gitVersion.InformationalVersion;
var nugetVersion = gitVersion.NuGetVersion;
var buildVersion = gitVersion.FullBuildMetaData;

// Artifacts
var artifactDirectory = "./artifacts/";
var packageWhitelist = new[] { "ChilliSource.Core" };

var buildSolution = "./ChilliSource.Core.sln";

var productName = "ChilliSource Core";
var configuration = "Release";
// Macros
Func<string> GetMSBuildLoggerArguments = () => {
    return BuildSystem.TeamCity.IsRunningOnTeamCity ? EnvironmentVariable("MsBuildLogger"): null;
};

Action Abort = () => { throw new Exception("A non-recoverable fatal error occurred."); };
Action<string> TestFailuresAbort = testResult => { throw new Exception(testResult); };
Action NonMacOSAbort = () => { throw new Exception("Running on platforms other macOS is not supported."); };

Action<string> RestorePackages = (solution) =>
{
    DotNetCoreRestore(solution);
};


Action<string, string> Package = (nuspec, basePath) =>
{
    CreateDirectory(artifactDirectory);

    Information("Packaging {0} using {1} as the BasePath.", nuspec, basePath);

    NuGetPack(nuspec, new NuGetPackSettings {
        Authors                  = new [] { "Blue Chilli Technology Pty Ltd" },
        Owners                   = new [] { "Blue Chilli Technology Pty Ltd" },

        ProjectUrl               = new Uri(githubUrl),
        IconUrl                  = new Uri("https://avatars0.githubusercontent.com/u/5924219?v=3&s=200"),
        LicenseUrl               = new Uri(licenceUrl),
        Copyright                = "Copyright Blue Chilli Technology Pty Ltd 2017",
        RequireLicenseAcceptance = false,

        Version                  = nugetVersion,
        Tags                     = new [] {  "ChilliSource", "ChilliSource Core"},
        ReleaseNotes             = new [] { string.Format("{0}/releases", githubUrl) },

        Symbols                  = false,
        Verbosity                = NuGetVerbosity.Detailed,
        OutputDirectory          = artifactDirectory,
        BasePath                 = basePath
    });
};

Action<string, string, Exception> WriteErrorLog = (message, identity, ex) => 
{
	if(isTeamCity) 
	{
		teamCity.BuildProblem(message, identity);
		teamCity.WriteStatus(String.Format("{0}", identity), "ERROR", ex.ToString());
		throw ex;
	}
	else {
		throw new Exception(String.Format("task {0} - {1}", identity, message), ex);
	}
};


Func<string, IDisposable> BuildBlock = message => {

	if(BuildSystem.TeamCity.IsRunningOnTeamCity) 
	{
		return BuildSystem.TeamCity.BuildBlock(message);
	}
	
	return null;
	
};

Func<string, IDisposable> Block = message => {

	if(BuildSystem.TeamCity.IsRunningOnTeamCity) 
	{
		BuildSystem.TeamCity.Block(message);
	}

	return null;
};


Action<string> build = (solution) =>
{
    Information("Building {0}", solution);
	using(BuildBlock("Build")) 
	{			
		var settings = new DotNetCoreBuildSettings
		{
			Configuration = configuration
		};

		if(isTeamCity) {

			var msBuildLogger = GetMSBuildLoggerArguments();
	
			settings.ArgumentCustomization = arguments => {
				 arguments.Clear();
				
				 arguments.Append("build");

				 // Specific path?
	            if (solution != null)
	            {
	                arguments.AppendQuoted(solution);
	            }

				 arguments.Append(string.Format("/p:ci={0}", true));

				// Configuration
	            if (!string.IsNullOrEmpty(settings.Configuration))
	            {
	                arguments.Append(string.Format("/p:Configuration={0}", settings.Configuration));
	            }


				 if(!string.IsNullOrEmpty(msBuildLogger)) {
					arguments.Append(string.Format("/logger:{0}", msBuildLogger));
				 }
				
				return arguments;
			};
		}
		
		DotNetCoreBuild(solution, settings);
    };		

};

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////
Setup((context) =>
{
    Information("Building version {0} of ChilliSource.Core. (isTagged: {1})", informationalVersion, isTagged);

		if (isTeamCity)
		{
			Information(
					@"Environment:
					 PullRequest: {0}
					 Build Configuration Name: {1}
					 TeamCity Project Name: {2}
					 Branch: {3}",
					 isPullRequest,
					 buildConfName,
					 projectName,
					 branch
					);
        }
        else
        {
             Information("Not running on TeamCity");
        }

         CleanDirectories(artifactDirectory);
});

Teardown((context) =>
{
    // Executed AFTER the last task.
});

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Build")
    .IsDependentOn("RestorePackages")
    .IsDependentOn("UpdateAssemblyInfo")
    .Does (() =>
{
    build(buildSolution);
})
.OnError(exception => {
	WriteErrorLog("Build failed", "Build", exception);
});


Task("UpdateAssemblyInfo")
    .Does (() =>
{
    var file = "./CommonAssemblyInfo.cs";

	using(BuildBlock("UpdateAssemblyInfo")) 
	{
		CreateAssemblyInfo(file, new AssemblyInfoSettings {
			Product = productName,
			Version = majorMinorPatch,
			FileVersion = majorMinorPatch,
			InformationalVersion = informationalVersion,
			Copyright = "Copyright (c) BlueChilli Technology PTY LTD"
		});
	};
   
})
.OnError(exception => {
	WriteErrorLog("updating assembly info failed", "UpdateAssemblyInfo", exception);
});

Task("RestorePackages")
.Does (() =>
{
    Information("Restoring Packages for {0}", buildSolution);
	using(BuildBlock("RestorePackages")) 
	{
	    RestorePackages(buildSolution);
	};
})
.OnError(exception => {
	WriteErrorLog("restoring packages failed", "RestorePackages", exception);
});



Task("RunUnitTests")
    .IsDependentOn("RestorePackages")
    .IsDependentOn("Build")
    .Does(() =>
{
	Information("Running Unit Tests for {0}", buildSolution);
	using(BuildBlock("RunUnitTests")) 
	{
		var settings = new DotNetCoreTestSettings
		{
			Configuration = configuration
		};

		DotNetCoreTest(settings, "./src/ChilliSource.Core.Tests/ChilliSource.Core.Tests.csproj",  new XUnit2Settings {
			OutputDirectory = artifactDirectory,
            XmlReportV1 = false
		});
	};
});


Task("Package")
    .IsDependentOn("Build")
    .IsDependentOn("RunUnitTests")
    .Does (() =>
{
	using(BuildBlock("Package")) 
	{
		foreach(var package in packageWhitelist)
		{
			// only push the package which was created during this build run.
			var packagePath = string.Format("./{0}.nuspec", package);

			// Push the package.
			Package(packagePath, "./");
		}
	};

    
})
.OnError(exception => {
	WriteErrorLog("Generating packages failed", "Package", exception);
});


Task("PublishPackages")
	.IsDependentOn("Build")
    .IsDependentOn("RunUnitTests")
    .IsDependentOn("Package")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isRepository)
    .Does (() =>
{
	using(BuildBlock("Package"))
	{
		string apiKey;
		string source;

		if (isReleaseBranch && !isTagged)
		{
			// Resolve the API key.
			apiKey = EnvironmentVariable("MYGET_APIKEY");
			if (string.IsNullOrEmpty(apiKey))
			{
				throw new Exception("The MYGET_APIKEY environment variable is not defined.");
			}

			source = EnvironmentVariable("MYGET_SOURCE");
			if (string.IsNullOrEmpty(source))
			{
				throw new Exception("The MYGET_SOURCE environment variable is not defined.");
			}
		}
		else 
		{
			// Resolve the API key.
			apiKey = EnvironmentVariable("NUGET_APIKEY");
			if (string.IsNullOrEmpty(apiKey))
			{
				throw new Exception("The NUGET_APIKEY environment variable is not defined.");
			}

			source = EnvironmentVariable("NUGET_SOURCE");
			if (string.IsNullOrEmpty(source))
			{
				throw new Exception("The NUGET_SOURCE environment variable is not defined.");
			}
		}



		// only push whitelisted packages.
		foreach(var package in packageWhitelist)
		{
			// only push the package which was created during this build run.
			var packagePath = artifactDirectory + File(string.Concat(package, ".", nugetVersion, ".nupkg"));

			// Push the package.
			NuGetPush(packagePath, new NuGetPushSettings {
				Source = source,
				ApiKey = apiKey
			});
		}

	};

  
})
.OnError(exception => {
	WriteErrorLog("publishing packages failed", "PublishPackages", exception);
});

Task("CreateRelease")
    .IsDependentOn("Build")
    .IsDependentOn("RunUnitTests")
    .IsDependentOn("Package")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isRepository)
    .WithCriteria(() => isReleaseBranch)
    .WithCriteria(() => !isTagged)
    .WithCriteria(() => isRunningOnWindows)
    .Does (() =>
{
	using(BuildBlock("CreateRelease"))
	{
		var username = EnvironmentVariable("GITHUB_USERNAME");
		if (string.IsNullOrEmpty(username))
		{
			throw new Exception("The GITHUB_USERNAME environment variable is not defined.");
		}

		var token = EnvironmentVariable("GITHUB_TOKEN");
		if (string.IsNullOrEmpty(token))
		{
			throw new Exception("The GITHUB_TOKEN environment variable is not defined.");
		}

		GitReleaseManagerCreate(username, token, githubOwner, githubRepository, new GitReleaseManagerCreateSettings {
			Milestone         = majorMinorPatch,
			Name              = majorMinorPatch,
			Prerelease        = true,
			TargetCommitish   = "master"
		});
	};

})
.OnError(exception => {
	WriteErrorLog("creating release failed", "CreateRelease", exception);
});

Task("PublishRelease")
   .IsDependentOn("Build")
    .IsDependentOn("RunUnitTests")
    .IsDependentOn("Package")
    .WithCriteria(() => !local)
    .WithCriteria(() => !isPullRequest)
    .WithCriteria(() => isRepository)
    .WithCriteria(() => isReleaseBranch)
    .WithCriteria(() => isTagged)
    .WithCriteria(() => isRunningOnWindows)
    .Does (() =>
{
	using(BuildBlock("PublishRelease"))
	{
		var username = EnvironmentVariable("GITHUB_USERNAME");
		if (string.IsNullOrEmpty(username))
		{
			throw new Exception("The GITHUB_USERNAME environment variable is not defined.");
		}

		var token = EnvironmentVariable("GITHUB_TOKEN");
		if (string.IsNullOrEmpty(token))
		{
			throw new Exception("The GITHUB_TOKEN environment variable is not defined.");
		}

		// only push whitelisted packages.
		foreach(var package in packageWhitelist)
		{
			// only push the package which was created during this build run.
			var packagePath = artifactDirectory + File(string.Concat(package, ".", nugetVersion, ".nupkg"));

			GitReleaseManagerAddAssets(username, token, githubOwner, githubRepository, majorMinorPatch, packagePath);
		}

		GitReleaseManagerClose(username, token, githubOwner, githubRepository, majorMinorPatch);
	}; 
})
.OnError(exception => {
	WriteErrorLog("updating release assets failed", "PublishRelease", exception);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("CreateRelease")
    .IsDependentOn("PublishPackages")
    .IsDependentOn("PublishRelease")
    .Does (() =>
{

});


//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
