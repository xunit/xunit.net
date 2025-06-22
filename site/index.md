---
title: Home
---

![.NET Foundation logo](https://raw.githubusercontent.com/xunit/media/main/dotnet-foundation.svg){.dnflogo}

# About xUnit.net

xUnit.net is a free, open source, community-focused unit testing tool for C#, F#, and Visual Basic. xUnit.net v3 support .NET 8.0 or later, and .NET Framework 4.7.2 or later.

xUnit.net works with the [.NET SDK](https://dotnet.microsoft.com/download) command line tools, [Visual Studio](https://visualstudio.microsoft.com/), [Visual Studio Code](https://code.visualstudio.com/), [JetBrains Rider](https://www.jetbrains.com/rider/), [NCrunch](https://www.ncrunch.net/), and any development environment compatible with [Microsoft Testing Platform](https://learn.microsoft.com/dotnet/core/testing/microsoft-testing-platform-intro) or [VSTest](https://github.com/microsoft/vstest).

xUnit.net is part of the [.NET Foundation](https://www.dotnetfoundation.org/) and operates under their [code of conduct](https://www.dotnetfoundation.org/code-of-conduct). It is licensed under [Apache 2](https://opensource.org/licenses/Apache-2.0) (an OSI approved license).

{ .about-links }
> Follow: <a href="https://dotnet.social/@xunit" rel="me">xUnit.net on Mastodon</a>, [xUnit.net on BlueSky](https://bsky.app/@xunit.net), [Brad Wilson](https://bradwilson.dev/), [James Newkirk](https://www.jamesnewkirk.com/), [Claire Novotny](https://github.com/clairernovotny)<br />
> JetBrains Rider support is provided and supported by [JetBrains](https://www.jetbrains.com/).<br />
> NCrunch support is provided and supported by [Remco Software](https://www.ncrunch.net/).<br />
> The xUnit.net logo was designed by [Nathan Young](https://mas.to/@nathanyoung).


- [Quick Start with .NET SDK](#quick-start-with-net-sdk)
- [Documentation](#documentation)
- [Issues and Discussions](#issues-and-discussions)
- [Release Notes](#release-notes)
- [Latest Builds](#latest-builds)

## Quick Start with .NET SDK

### [C#](#tab/cs)

Install the xUnit.net v3 project templates:

```
dotnet new install xunit.v3.templates
```

```shell
The following template packages will be installed:
    xunit.v3.templates

Success: xunit.v3.templates::2.0.3 installed the following templates:
Template Name                   Short Name        Language    Tags
------------------------------  ----------------  ----------  ----------
xUnit.net v3 Extension Project  xunit3-extension  [C#],F#,VB  Test/xUnit
xUnit.net v3 Test Project       xunit3            [C#],F#,VB  Test/xUnit
```

Create a unit test project:

```
dotnet new xunit3 --language C#
```

```shell
The template "xUnit.net v3 Test Project" was created successfully.

Processing post-creation actions...
Restoring C:\Dev\SampleProject\SampleProject.csproj:
Restore succeeded.
```

Edit `UnitTest1.cs`:

```csharp
namespace SampleProject;

public class UnitTest1
{
    public static int Add(int x, int y) =>
        x + y;

    [Fact]
    public void Good() =>
        Assert.Equal(4, Add(2, 2));

    [Fact]
    public void Bad() =>
        Assert.Equal(5, Add(2, 2));
}
```

Execute the tests:

```
dotnet run
```

```shell
xUnit.net v3 In-Process Runner v2.0.3+216a74a292 (64-bit .NET 8.0.17)
  Discovering: SampleProject
  Discovered:  SampleProject
  Starting:    SampleProject
    SampleProject.UnitTest1.Bad [FAIL]
      Assert.Equal() Failure: Values differ
      Expected: 5
      Actual:   4
      Stack Trace:
        UnitTest1.cs(14,0): at SampleProject.UnitTest1.Bad()
  Finished:    SampleProject
=== TEST EXECUTION SUMMARY ===
    SampleProject  Total: 2, Errors: 0, Failed: 1, Skipped: 0, Not Run: 0, Time: 0.054s
```

### [F#](#tab/fs)

Install the xUnit.net v3 project templates:

```
dotnet new install xunit.v3.templates
```

```shell
The following template packages will be installed:
    xunit.v3.templates

Success: xunit.v3.templates::2.0.3 installed the following templates:
Template Name                   Short Name        Language    Tags
------------------------------  ----------------  ----------  ----------
xUnit.net v3 Extension Project  xunit3-extension  [C#],F#,VB  Test/xUnit
xUnit.net v3 Test Project       xunit3            [C#],F#,VB  Test/xUnit
```

Create a unit test project:

```
dotnet new xunit3 --language F#
```

```shell
The template "xUnit.net v3 Test Project" was created successfully.

Processing post-creation actions...
Restoring C:\Dev\SampleProject\SampleProject.fsproj:
Restore succeeded.
```

Edit `UnitTest1.fs`:

```fsharp
module UnitTest1

open Xunit

let Add(x: int, y: int) =
    x + y

[<Fact>]
let Good() =
    Assert.Equal(4, Add(2, 2))

[<Fact>]
let Bad() =
    Assert.Equal(5, Add(2, 2))
```

Execute the tests: `dotnet run`

```
dotnet run
```

```shell
xUnit.net v3 In-Process Runner v2.0.3+216a74a292 (64-bit .NET 8.0.17)
  Discovering: SampleProject
  Discovered:  SampleProject
  Starting:    SampleProject
    UnitTest1.Bad [FAIL]
      Assert.Equal() Failure: Values differ
      Expected: 5
      Actual:   4
      Stack Trace:
        UnitTest1.fs(14,0): at UnitTest1.Bad()
  Finished:    SampleProject
=== TEST EXECUTION SUMMARY ===
    SampleProject  Total: 2, Errors: 0, Failed: 1, Skipped: 0, Not Run: 0, Time: 0.052s
```

### [Visual Basic](#tab/vb)

Install the xUnit.net v3 project templates:

```
dotnet new install xunit.v3.templates
```

```shell
The following template packages will be installed:
    xunit.v3.templates

Success: xunit.v3.templates::2.0.3 installed the following templates:
Template Name                   Short Name        Language    Tags
------------------------------  ----------------  ----------  ----------
xUnit.net v3 Extension Project  xunit3-extension  [C#],F#,VB  Test/xUnit
xUnit.net v3 Test Project       xunit3            [C#],F#,VB  Test/xUnit
```

Create a unit test project:

```
dotnet new xunit3 --language VB
```

```shell
The template "xUnit.net v3 Test Project" was created successfully.

Processing post-creation actions...
Restoring C:\Dev\SampleProject\SampleProject.vbproj:
Restore succeeded.
```

Edit `UnitTest1.vb`:

```vb
Imports Xunit

Public Class UnitTest1

    Public Function Add(x As Integer, y As Integer) As Integer
        Return x + y
    End Function

    <Fact>
    Public Sub Good()
        Assert.Equal(4, Add(2, 2))
    End Sub

    <Fact>
    Public Sub Bad()
        Assert.Equal(5, Add(2, 2))
    End Sub

End Class
```

Execute the tests: `dotnet run`

```
dotnet run
```

```shell
xUnit.net v3 In-Process Runner v2.0.3+216a74a292 (64-bit .NET 8.0.17)
  Discovering: SampleProject
  Discovered:  SampleProject
  Starting:    SampleProject
    SampleProject.UnitTest1.Bad [FAIL]
      Assert.Equal() Failure: Values differ
      Expected: 5
      Actual:   4
      Stack Trace:
        UnitTest1.vb(16,0): at SampleProject.UnitTest1.Bad()
  Finished:    SampleProject
=== TEST EXECUTION SUMMARY ===
    SampleProject  Total: 2, Errors: 0, Failed: 1, Skipped: 0, Not Run: 0, Time: 0.052s
```

---

## Documentation

### Getting Started

* [Getting started with the .NET SDK command line](/docs/v3/getting-started)<br />
  _Includes running tests from the command line, Visual Studio, Visual Studio Code, and JetBrains Rider_
* [API documentation](https://api.xunit.net/)
* [xUnit.net analyzer documentation](/xunit.analyzers/rules/)
* [Query filter language](/docs/v3/query-filter-language)
* [NuGet packages](/docs/v3/nuget-packages)
* [Configuration with `xunit.runner.json`](/docs/configuration-files)
* [Sample projects (including testing and extensibility)](https://github.com/xunit/samples.xunit)

### Migrating from xUnit.net v2

* [What's new in xUnit.net v3?](/docs/v3/whats-new)
* Migrating from xUnit.net v2 [for unit test authors](/docs/v3/migration) and [extensibility authors](/docs/v3/migration-extensibility)

### Unit Test Parallelism

* [Running tests in parallel](/docs/running-tests-in-parallel)
* [Sharing context between tests](/docs/shared-context) (class, collection, and assembly fixtures)

### Microsoft Testing Platform

* [Microsoft Testing Platform support](/docs/v3/microsoft-testing-platform)
* [Code Coverage with Microsoft Testing Platform](/docs/v3/code-coverage-with-mtp)
* [Configuration with `testconfig.json` with Microsoft Testing Platform](/docs/v3/testconfig-json)

### VSTest and `xunit.runner.visualstudio`

* [Configuration with RunSettings](/docs/runsettings)

### Test Results in CI Systems

* [Getting Test Results in Azure DevOps](/docs/getting-test-results-in-azure-devops)
* [Getting Test Results in TeamCity](/docs/getting-test-results-in-teamcity)
* [Getting Test Results in CruiseControl.NET](/docs/getting-test-results-in-ccnet)

### Frequently Asked Questions

* [How do I use a CI build NuGet package?](/docs/using-ci-builds)
* [What is the JSON schema for `xunit.runner.json`?](/schema/)
* [What is the format of the XML generated by the test runners?](/docs/format-xml-v2)
* [How do I build xUnit.net?](https://github.com/xunit/xunit/blob/main/BUILDING.md)
* [Why doesn't xUnit.net support netstandard?](/docs/why-no-netstandard)
* [What is "theory data stability"?](/docs/theory-data-stability-in-vs)

### Advanced topics

* [Capturing output](/docs/capturing-output)
* [Serialization with `IXunitSerializer` and `IXunitSerializable`](/docs/custom-serialization)
* [Writing a custom Runner Reporter](/docs/custom-runner-reporter)
* [Multi-targeting on non-Windows OSes](/docs/getting-started/multi-target/non-windows)
* [Equality with hash sets vs. linear containers](/docs/hash-sets-vs-linear-containers)

## Issues and Discussions

* Issues in the core framework and analyzers should be reported in the [primary issue tracker](https://github.com/xunit/xunit/issues?q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc)
* Issues in `xunit.runner.visualstudio` should be reported in the [project issue tracker](https://github.com/xunit/visualstudio.xunit/issues?q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc)
* Discussions are hosted in the [primary discussion forums](https://github.com/xunit/xunit/discussions?discussions_q=is%3Aunanswered+is%3Aopen)

## Release Notes

{: .latest }
|                           | Stable                                             | Prerelease                                                       |
| ------------------------- | -------------------------------------------------- | ---------------------------------------------------------------- | -------------------------------
| xunit.v3                  | [2.0.3](/releases/v3/2.0.3){: .release }           | [3.0.0-pre.25](/releases/v3/3.0.0-pre.25){: .prerelease }        | ([all releases](/releases/v3/))
| xunit.analyzers           | [1.22.0](/releases/analyzers/1.22.0){: .release }  | [1.23.0-pre.3](/releases/analyzers/1.23.0-pre.3){: .prerelease } | ([all releases](/releases/analyzers/))
| xunit.runner.visualstudio | [3.1.1](/releases/visualstudio/3.1.1){: .release } |                                                                  | ([all releases](/releases/visualstudio/))

## Latest Builds

{: .table .latest }
|                           | Stable                                                                                                                                   | Latest CI ([how to use](/docs/using-ci-builds))                                                                                                                                                                          | Build status
| ------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------
| xunit.v3                  | [![](https://img.shields.io/nuget/v/xunit.v3.svg?logo=nuget)](https://www.nuget.org/packages/xunit.v3)                                   | [![](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.v3/latest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3)                                   | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/xunit/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/xunit/goto?ref=main)
| xunit.analyzers           | [![](https://img.shields.io/nuget/v/xunit.analyzers.svg?logo=nuget)](https://www.nuget.org/packages/xunit.analyzers)                     | [![](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.analyzers/latest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.analyzers)                     | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/xunit.analyzers/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/xunit.analyzers/goto?ref=main)
| xunit.runner.visualstudio | [![](https://img.shields.io/nuget/v/xunit.runner.visualstudio.svg?logo=nuget)](https://www.nuget.org/packages/xunit.runner.visualstudio) | [![](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.runner.visualstudio/latest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.runner.visualstudio) | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/visualstudio.xunit/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/visualstudio.xunit/goto?ref=main)
