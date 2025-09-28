---
title: Home
---

![.NET Foundation logo](https://raw.githubusercontent.com/xunit/media/main/dotnet-foundation.svg){.dnflogo}

# About xUnit.net

xUnit.net is a free, open source, community-focused unit testing tool for C#, F#, and Visual Basic. xUnit.net v3 supports .NET 8.0 or later, and .NET Framework 4.7.2 or later.

xUnit.net works with the [.NET SDK](https://dotnet.microsoft.com/download) command line tools, [Visual Studio](https://visualstudio.microsoft.com/), [Visual Studio Code](https://code.visualstudio.com/), [JetBrains Rider](https://www.jetbrains.com/rider/), [NCrunch](https://www.ncrunch.net/), and any development environment compatible with [Microsoft Testing Platform](https://learn.microsoft.com/dotnet/core/testing/microsoft-testing-platform-intro) or [VSTest](https://github.com/microsoft/vstest).

xUnit.net is part of the [.NET Foundation](https://www.dotnetfoundation.org/) and operates under their [code of conduct](https://www.dotnetfoundation.org/code-of-conduct). It is licensed under [Apache 2](https://opensource.org/licenses/Apache-2.0) (an OSI approved license). The project is [governed](/governance) by a Project Lead.

{ .about-links }
> Follow: [xUnit.net on Mastodon](https://dotnet.social/@xunit), [xUnit.net on Bluesky](https://bsky.app/profile/xunit.net), [Brad Wilson](https://bradwilson.dev/), [James Newkirk](https://www.jamesnewkirk.com/)<br />
> JetBrains Rider support is provided and supported by [JetBrains](https://www.jetbrains.com/).<br />
> NCrunch support is provided and supported by [Remco Software](https://www.ncrunch.net/).<br />
> The xUnit.net logo was designed by [Nathan Young](https://mas.to/@nathanyoung).

## Quick Start with .NET SDK{ #quick-start }

### [C#](#tab/cs)

Install the xUnit.net v3 project templates:

```
dotnet new install xunit.v3.templates
```

```text
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

```text
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

```text
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

```text
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

```text
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

```text
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

```text
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

```text
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

```text
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

## Latest Release Notes{ #release-notes }

{: .latest }
|                       | Stable                                             | Prerelease
| --------------------- | -------------------------------------------------- | ----------
| Core Framework v3     | [3.1.0](/releases/v3/3.1.0){: .release }           | _None_
| Core Framework v2     | [2.9.3](/releases/v2/2.9.3){: .release }           | _None_
| Analyzers             | [1.24.0](/releases/analyzers/1.24.0){: .release }  | _None_
| Visual Studio adapter | [3.1.5](/releases/visualstudio/3.1.5){: .release } | _None_

_For older release notes, see the [full release notes list](/releases/)._

## Latest NuGet Packages{ #packages }

{: .table .latest }
|                             | Latest stable                                                                                                                            | Latest CI ([how to use](/docs/using-ci-builds))                                                                                                                                                                          | Build status
| --------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------ | ------------
| `xunit.v3`                  | [![](https://img.shields.io/nuget/v/xunit.v3.svg?logo=nuget)](https://www.nuget.org/packages/xunit.v3)                                   | [![](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.v3/latest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3)                                   | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/xunit/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/xunit/goto?ref=main)
| `xunit`                     | [![](https://img.shields.io/nuget/v/xunit.svg?logo=nuget)](https://www.nuget.org/packages/xunit)                                         | [![](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit/latest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit)                                         | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/xunit/badge%3Fref%3Dv2&amp;label=build)](https://actions-badge.atrox.dev/xunit/xunit/goto?ref=v2)
| `xunit.analyzers`           | [![](https://img.shields.io/nuget/v/xunit.analyzers.svg?logo=nuget)](https://www.nuget.org/packages/xunit.analyzers)                     | [![](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.analyzers/latest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.analyzers)                     | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/xunit.analyzers/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/xunit.analyzers/goto?ref=main)
| `xunit.runner.visualstudio` | [![](https://img.shields.io/nuget/v/xunit.runner.visualstudio.svg?logo=nuget)](https://www.nuget.org/packages/xunit.runner.visualstudio) | [![](https://img.shields.io/badge/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.runner.visualstudio/latest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.runner.visualstudio) | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/visualstudio.xunit/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/visualstudio.xunit/goto?ref=main)

## Issues and Discussions{ #issues }

* Issues in the core framework and analyzers should be reported in the [primary issue tracker](https://github.com/xunit/xunit/issues?q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc)
* Issues in `xunit.runner.visualstudio` should be reported in the [project issue tracker](https://github.com/xunit/visualstudio.xunit/issues?q=is%3Aissue+is%3Aopen+sort%3Aupdated-desc)
* Discussions are hosted in the [primary discussion forums](https://github.com/xunit/xunit/discussions?discussions_q=is%3Aunanswered+is%3Aopen)

## Github Projects{ #github }

* [xUnit.net](https://github.com/xunit/xunit) (core framework, built-in runners)
* [Assertion library](https://github.com/xunit/assert.xunit)
* [Analyzers](https://github.com/xunit/xunit.analyzers)
* [Visual Studio adapter](https://github.com/xunit/visualstudio.xunit)
* [Media files](https://github.com/xunit/media)
* [This site](https://github.com/xunit/xunit.net)

## Links to Resources{ #links }

* [Visual Studio Community](https://visualstudio.microsoft.com/vs/community/)
* [MSBuild Reference](https://docs.microsoft.com/visualstudio/msbuild/msbuild-reference)

[![Powered by NDepend](https://raw.githubusercontent.com/xunit/media/refs/heads/main/powered-by-ndepend-transparent.svg){: width="175" }](https://www.ndepend.com)

## Sponsors{ #sponsors }

Help support this project by becoming a sponsor through [GitHub Sponsors](https://github.com/sponsors/xunit).

## Additional copyrights{ #copyrights }

Portions copyright The Legion Of The Bouncy Castle
