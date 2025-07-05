---
title: What NuGet Packages Should I Use?
title-version: xUnit.net v3
---


xUnit.net v3 ships with many NuGet packages. This guide helps you understand which NuGet packages you should use in your projects.

If you don't know where to start, try starting with a reference to [`xunit.v3`](#xunit.v3).

> [!NOTE]
> Because xUnit.net v3 injects code into your test projects to make them executable, non-unit test projects must not reference `xunit.v3` (or `xunit.v3.core`) and should instead reference `xunit.v3.extensibility.core` to be able to provide extensions to xUnit.net v3.

{ #radio-selector }
**Version selector:**
<input type="radio" name="version" class="version-picker" data-selector=".version-stable" checked="checked" /> Stable (NuGet)
<input type="radio" name="version" class="version-picker" data-selector=".version-ci" /> Latest CI (feedz.io)

## Packages for writing tests{ #testers }

_Packages used by developers who are writing unit tests._

### `xunit.v3`{ #xunit.v3 }

This is the package that will most typically be used by unit test authors. It brings in references to [`xunit.v3.core`](#xunit.v3.core) (which contains the unit testing framework), [`xunit.analyzers`](#xunit.analyzers) (which contains source code analyzers), and [`xunit.v3.assert`](#xunit.v3.assert) (which contains the class you use to write assertions).

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.v3.svg?style=flat)](https://www.nuget.org/packages/xunit.v3){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.v3%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | `net472`, `net8.0`
> | Depends on | [`xunit.v3.assert`](#xunit.v3.assert), [`xunit.v3.core`](#xunit.v3.core), [`xunit.analyzers`](#xunit.analyzers)

### `xunit.v3.assert`{ #xunit.v3.assert }

This package contains the xUnit.net assertion library (i.e., the `Assert` class).

This is a separate NuGet package, because some developers wish to use the xUnit.net framework and test runners, but with a different assertion library.

> [!NOTE]
> If you want to extend the `Assert` class, you should consider using the [`xunit.v3.assert.source`](#xunit.v3.assert.source) package instead.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.v3.assert.svg?style=flat)](https://www.nuget.org/packages/xunit.v3.assert){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.v3.assert%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3.assert){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | `netstandard2.0`, `net8.0`

### `xunit.v3.core`{ #xunit.v3.core }

This package contains the core types for the test framework (f.e., `FactAttribute`).

Referencing this package provides the required infrastructure to be able to write unit tests, including the injection of the entry point that is required to make all v3 test projects stand-alone executables.

> [!NOTE]
> This package should only be used by unit test projects. If you wish to extend xUnit.net or create a base library consumed by unit test projects, then you should reference [`xunit.v3.extensibility.core`](#xunit.v3.extensibility.core) instead.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.v3.core.svg?style=flat)](https://www.nuget.org/packages/xunit.v3.core){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.v3.core%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3.core){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | `net472`, `net8.0`
> | Depends on | [`xunit.v3.extensibility.core`](#xunit.v3.extensibility.core), [`xunit.v3.runner.inproc.console`](#xunit.v3.runner.inproc.console)

### `xunit.v3.templates`{ #xunit.v3.templates }

This package provides templates for `dotnet new` to support xUnit.net v3 test projects and xUnit.net v3 extensibility projects.

To install this package, run: `dotnet new install xunit.v3.templates`. This will add two new templates (`xunit3` and `xunit3-extension`) for C#, F#, and VB.NET.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.v3.templates.svg?style=flat)](https://www.nuget.org/packages/xunit.v3.templates){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.v3.templates%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3.templates){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | **xunit3**: **`net8.0`**, `net9.0`, `net472`, `net48`, `net481`<br />**xunit3-extension**: **`netstandard2.0`**, `net8.0`, `net9.0`, `net472`, `net48`, `net481`

### `xunit.analyzers`{ #xunit.analyzers }

This package contains the xUnit.net source code analyzers.

This library provides code analysis and code fixers for common issues that are encountered both by test authors and extensibility authors. It's based on the [.NET Compiler Platform ("Roslyn") analyzers](https://docs.microsoft.com/visualstudio/code-quality/roslyn-analyzers-overview) which can provide real-time source code analysis inside IDEs (including Visual Studio and Visual Studio Code), as well as compile-time source code analysis.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.analyzers.svg?style=flat)](https://www.nuget.org/packages/xunit.analyzers){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.analyzers%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.analyzers){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | Roslyn 3.11+ (Visual Studio 2019 16.11+)

## Packages for running tests{ #runners }

_Packages used by developers who are running unit tests._

### `xunit.v3.runner.console`{ #xunit.v3.runner.console }

This package contains the console test runner. This runner is capable of running .NET Framework and .NET projects from xUnit.net v3, as well as .NET Framework projects from xUnit.net v1 and v2. It can run multiple test projects in parallel.

> [!NOTE]
> Note: we ship runners targeting multiple versions of .NET Framework, because xUnit.net v1 and v2 projects are loaded into runner process, so the version of .NET Framework that the runner is built against determines the version of .NET Framework that the tests will run against (and more importantly, the features available). We also ship AnyCPU versions as well as versions which are built for 32-bit x86. Since xUnit.net v3 projects are stand-alone executables, they will run with whatever version (and bitness) of .NET or .NET Framework that they were built against regardless of the runner's version.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.v3.runner.console.svg?style=flat)](https://www.nuget.org/packages/xunit.v3.runner.console){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.v3.runner.console%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.runner.console){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | `net472` (v1/v2/v3), `net8.0` (v3)

### `xunit.v3.runner.msbuild`{ #xunit.v3.runner.msbuild }

This package contains the MSBuild test runner. This runner is capable of running .NET Framework and .NET projects from xUnit.net v3, as well as .NET Framework projects from xUnit.net v1 and v2. It can run multiple test projects in parallel.

> [!NOTE]
> For xUnit.net v3 projects, this runner can be used from both `msbuild.exe` as well as `dotnet msbuild`. For xUnit.net v1/v2 projects, this runner only supports being run from `msbuild.exe`.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.v3.runner.msbuild.svg?style=flat)](https://www.nuget.org/packages/xunit.v3.runner.msbuild){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.v3.runner.msbuild%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3.runner.msbuild){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | `net472` (v1/v2/v3), `net8.0` (v3)

### `xunit.runner.visualstudio`{ #xunit.runner.visualstudio }

This package contains the adapter to run tests via VSTest. This runner is capable of running .NET Framework projects from xUnit.net v1/v2/v3, and .NET projects from xUnit.net v2/v3. (Support for v3 requires using runner version 3.0 or later.)

The VSTest framework is used by several 3rd party runners, including:

* Visual Studio (via Test Explorer)
* Visual Studio Code (via the Testing panel)
* `dotnet test` and `dotnet vstest`
* `vstest.console.exe`

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.runner.visualstudio.svg?style=flat)](https://www.nuget.org/packages/xunit.runner.visualstudio){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.runner.visualstudio%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.runner.visualstudio){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | `net472` (v1/v2/v3), `net8.0` (v2/v3)

## Packages for extending xUnit.net{ #extenders }

_Packages used by developers who are extending xUnit.net and/or creating unit test runners._

### `xunit.v3.assert.source`{ #xunit.v3.assert.source }

This package contains the xUnit.net assertion library (i.e., the `Assert` class) in source form. The `Assert` class is a partial class, which allows developers to write additional methods available directly from `Assert`.

When you have multiple unit test libraries in your project, it is common practice to import this package into a "test utility" library where you write all your custom assertions, and then reference that "test utility" library from your unit test projects.

For more information, see the [`xunit/assert.xunit` README](https://github.com/xunit/assert.xunit/blob/main/README.md)

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.v3.assert.source.svg?style=flat)](https://www.nuget.org/packages/xunit.v3.assert.source){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.v3.assert.source%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3.assert.source){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | `netstandard1.1` with C# 6 or later

### `xunit.v3.extensibility.core`{ #xunit.v3.extensibility.core }

This package contains `xunit.v3.core.dll`. It is intended to be used by developers who wish to reference this DLL for extensibility purposes, such as writing your own theory data provider.

This package is referenced by [`xunit.v3.core`](#xunit.v3.core). It differs in that it does not include the MSBuild steps required to make the project an executable test project.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.v3.extensibility.core.svg?style=flat)](https://www.nuget.org/packages/xunit.v3.extensibility.core){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.v3.extensibility.core%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3.extensibility.core){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | `netstandard2.0`
> | Depends on | [`xunit.v3.common`](#xunit.v3.common)

### `xunit.v3.runner.inproc.console`{ #xunit.v3.runner.inproc.console }

This package contains the code that provides the command line UI for running xUnit.net v3 projects as stand-alone executables. This includes both the native xUnit.net command line UX as well as the Microsoft Testing Platform command line UX.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.v3.runner.inproc.console.svg?style=flat)](https://www.nuget.org/packages/xunit.v3.runner.inproc.console){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.v3.runner.inproc.console%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3.runner.inproc.console){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | `net472`, `net8.0`
> | Depends on | [`xunit.v3.extensibility.core`](#xunit.v3.extensibility.core), [`xunit.v3.runner.common`](#xunit.v3.runner.common)

### `xunit.v3.runner.utility`{ #xunit.v3.runner.utility }

This package contains `xunit.v3.runner.utility.*.dll`. It is intended to be used by developers who are writing their own test runners.

The libraries contained here are both backward and forward compatible for all v1, v2, and v3 xUnit.net tests.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.v3.runner.utility.svg?style=flat)](https://www.nuget.org/packages/xunit.v3.runner.utility){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.v3.runner.utility%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3.runner.utility){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | `net472`, `net8.0`
> | Depends on | [`xunit.abstractions`](#xunit.abstractions), [`xunit.v3.runner.common`](#xunit.v3.runner.common)

## Packages for shared code

_Packages that contain types that are shared among packages in other categories. Not typically directly referenced._

### `xunit.v3.common`{ #xunit.v3.common }

This package contains types that are shared between [`xunit.v3.core`](#xunit.v3.core) and [`xunit.v3.runner.common`](#xunit.v3.runner.common).

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.v3.common.svg?style=flat)](https://www.nuget.org/packages/xunit.v3.common){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.v3.common%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3.common){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | `netstandard2.0`

### `xunit.v3.runner.common`{ #xunit.v3.runner.common }

This package contains types that are shared between [`xunit.v3.runner.inproc.console`](#xunit.v3.runner.inproc.console) and [`xunit.v3.runner.utility`](#xunit.v3.runner.utility).

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.v3.runner.common.svg?style=flat)](https://www.nuget.org/packages/xunit.v3.runner.common){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.v3.runner.common%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3.runner.common){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | `netstandard2.0`
> | Depends on | [`xunit.v3.common`](#xunit.v3.common)

### `xunit.abstractions`{ #xunit.abstractions }

This package contains common interfaces used by various parts of xUnit.net v2. It is used by [`xunit.v3.runner.utility`](#xunit.v3.runner.utility) to be able to run xUnit.net v2 test projects.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.abstractions.svg?style=flat)](https://www.nuget.org/packages/xunit.abstractions){ .version-stable }[![](https://img.shields.io/badge/endpoint.svg?url=https%3A%2F%2Ff.feedz.io%2Fxunit%2Fxunit%2Fshield%2Fxunit.abstractions%2Flatest&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.abstractions){ .version-ci .hidden }
> | Introduced | 1.0
> | Targets    | `net35`, `netstandard1.0`

<script defer src="nuget-packages.js"></script>
