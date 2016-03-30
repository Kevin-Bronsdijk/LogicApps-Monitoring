// --------------------------------------------------------------------------------------
// FAKE build script 
// --------------------------------------------------------------------------------------

#r @"tools\FAKE\tools\FakeLib.dll"
open Fake
open Fake.AssemblyInfoFile

// --------------------------------------------------------------------------------------
// Information about the project to be used at NuGet and in AssemblyInfo files
// --------------------------------------------------------------------------------------
let project = "Azure Logic App Monitoring"
let authors = ["Kevin Bronsdijk"]
let summary = "Azure Logic App Monitoring"
let version = "1.0.0.1"
let description = """
Solution which will track failed Logic Apps workflow runs within Azure Applications Insight.
"""
let notes = ""
let tags = "LogicApps C# API Monitoring Alerts"
let gitHome = "https://github.com/Kevin-Bronsdijk"
let gitName = "LogicApps-Monitoring"

// --------------------------------------------------------------------------------------
// Build script 
// --------------------------------------------------------------------------------------

let buildDir = "./output/"

// --------------------------------------------------------------------------------------

Target "Clean" (fun _ ->
 CleanDir buildDir
)

// --------------------------------------------------------------------------------------

Target "AssemblyInfo" (fun _ ->
    let attributes =
        [ 
            Attribute.Title project
            Attribute.Product project
            Attribute.Description summary
            Attribute.Version version
            Attribute.FileVersion version
        ]

    CreateCSharpAssemblyInfo "src/LogicAppsMonitoring.Logic/Properties/AssemblyInfo.cs" attributes
    CreateCSharpAssemblyInfo "src/LogicAppsMonitoring.WebJob/Properties/AssemblyInfo.cs" attributes
)

// --------------------------------------------------------------------------------------

Target "Build" (fun _ ->
 !! "src/*.sln"
 |> MSBuildRelease buildDir "Build"
 |> Log "AppBuild-Output: "
)

// --------------------------------------------------------------------------------------

Target "All" DoNothing

"Clean"
  ==> "AssemblyInfo"
  ==> "Build"
  ==> "All"

RunTargetOrDefault "All"