---
title: What NuGet Packages Should I Use?
title-version: xUnit.net v2
---

xUnit.net v2 ships with many NuGet packages. This guide helps you understand which NuGet packages you should use in your projects.

If you don't know where to start, try starting with a reference to [`xunit`](#xunit).

{ #radio-selector }
**Version selector:**
<input type="radio" name="version" class="version-picker" data-selector=".version-stable" checked="checked" /> Stable (NuGet)
<input type="radio" name="version" class="version-picker" data-selector=".version-ci" /> Latest CI (feedz.io)

## Packages for writing tests{ #testers }

_Packages used by developers who are writing unit tests._

### `xunit`{ #xunit }

This is the package that will most typically be used by unit test authors. It brings in references to [`xunit.core`](#xunit.core) (which contains the unit testing framework), [`xunit.analyzers`](#xunit.analyzers) (which contains source code analyzers), and [`xunit.assert`](#xunit.assert) (which contains the class you use to write assertions).

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.svg?style=flat)](https://www.nuget.org/packages/xunit){ .version-stable }[![](https://img.shields.io/feedz/vpre/xunit/xunit/xunit?logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit){ .version-ci .hidden }
> | Introduced | 2.0
> | Depends On | [`xunit.assert`](#xunit.assert), [`xunit.analyzers`](#xunit.analyzers), [`xunit.core`](#xunit.core)

### `xunit.assert`{ #xunit.assert }

This package contains the xUnit.net assertion library (i.e., the `Assert` class).

This is a separate NuGet package, because some developers wish to use the xUnit.net framework and test runners, but with a different assertion library.

> [!NOTE]
> If you want to extend the `Assert` class, you should consider using the [`xunit.assert.source`](#xunit.assert.source) package instead.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.assert.svg?style=flat)](https://www.nuget.org/packages/xunit.assert){ .version-stable }[![](https://img.shields.io/feedz/vpre/xunit/xunit/xunit.assert?logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.assert){ .version-ci .hidden }
> | Introduced | 2.0
> | Targets    | `netstandard1.1`

### `xunit.analyzers`{ #xunit.analyzers }

This package contains the xUnit.net source code analyzers.

This library provides code analysis and code fixers for common issues that are encountered both by test authors and extensibility authors. It's based on the [.NET Compiler Platform ("Roslyn") analyzers](https://docs.microsoft.com/visualstudio/code-quality/roslyn-analyzers-overview?view=vs-2019) which can provide real-time source code analysis inside IDEs (including Visual Studio and Visual Studio Code), as well as compile-time source code analysis.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.analyzers.svg?style=flat)](https://www.nuget.org/packages/xunit.analyzers){ .version-stable }[![](https://img.shields.io/feedz/vpre/xunit/xunit/xunit.analyzers?logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.analyzers){ .version-ci .hidden }
> | Introduced | 2.3

### `xunit.core`{ #xunit.core }

This package contains the core types for the test framework (i.e., `FactAttribute`).

Referencing this package includes `xunit.core.dll` and also copies (but does not reference) the appropriate execution libraries (`xunit.execution.*.dll`) which are required to support discovering and executing tests.

> [!NOTE]
> If you wish to use `xunit.core.dll` for extensibility purposes (for example, to write your own reusable theory data providers), you should reference [`xunit.extensibility.core`](#xunit.extensibility.core) instead.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.core.svg?style=flat)](https://www.nuget.org/packages/xunit.core){ .version-stable }[![](https://img.shields.io/feedz/vpre/xunit/xunit/xunit.core?logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.core){ .version-ci .hidden }
> | Introduced | 2.0
> | Targets    | `netstandard1.1`
> | Depends on | [`xunit.extensibility.core`](#xunit.extensibility.core)

## Packages for running tests{ #runners }

_Packages used by developers who are running unit tests._

### `xunit.runner.console`{ #xunit.runner.console }

This package contains the console test runner. This runner is capable of running .NET Framework projects from xUnit.net v1 and v2.

> [!NOTE]
> To run .NET Core projects from the command line (with `dotnet test`), please reference [`xunit.runner.visualstudio`](#xunit.runner.visualstudio) instead.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.runner.console.svg?style=flat)](https://www.nuget.org/packages/xunit.runner.console){ .version-stable }[![](https://img.shields.io/feedz/vpre/xunit/xunit/xunit.runner.console?logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.runner.console){ .version-ci .hidden }
> | Introduced | 2.0
> | Targets    | `net452`

### `xunit.runner.msbuild`{ #xunit.runner.msbuild }

This package contains the MSBuild test runner. This runner is capable of running .NET Framework projects from xUnit.net v1 and v2. When added, it automatically references the `xunit` MSBuild target, which can be used in your project file.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.runner.msbuild.svg?style=flat)](https://www.nuget.org/packages/xunit.runner.msbuild){ .version-stable }[![](https://img.shields.io/feedz/vpre/xunit/xunit/xunit.runner.msbuild?logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.runner.msbuild){ .version-ci .hidden }
> | Introduced | 2.0
> | Targets    | `net452`

### `xunit.runner.visualstudio`{ #xunit.runner.visualstudio }

This package contains the adapter to run tests via VSTest. This runner is capable of running .NET Framework projects from xUnit.net v1/v2/v3, and .NET/.NET Core projects from xUnit.net v2/v3. (Support for v3 requires using runner version 3.0 or later.)

The VSTest framework is used by several 3rd party runners, including:

* Visual Studio (via Test Explorer)
* Visual Studio Code (via the Testing panel)
* `dotnet test` and `dotnet vstest`
* `vstest.console.exe`

> [!NOTE]
> The latest versions of this package impose a much newer minimum target framework version than Core Framework v2. To test projects that target older framework versions, please use an older version of this package which aligns with your needs.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.runner.visualstudio.svg?style=flat)](https://www.nuget.org/packages/xunit.runner.visualstudio){ .version-stable }[![](https://img.shields.io/feedz/vpre/xunit/xunit/xunit.runner.visualstudio?logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.runner.visualstudio){ .version-ci .hidden }
> | Introduced | 2.0
> | Targets    | `net472`, `net8.0`

## Packages for extending xUnit.net{ #extenders }

_Packages used by developers who are extending xUnit.net and/or creating unit test runners._

### `xunit.abstractions`{ #xunit.abstractions }

This package is an internal dependency of the [test framework](#xunit.core), the platform specific execution DLLs (`xunit.execution.*.dll`) and the [runner utility libraries](#xunit.runner.utility). It defines a stable set of interfaces that these components use to communicate with each other. As long as the interfaces do not change, each component can be modified without requiring any changes or rebuilds of the other components. This allows for a version independent runner API.

You usually won't need to directly reference this package.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.abstractions.svg?style=flat)](https://www.nuget.org/packages/xunit.abstractions){ .version-stable }[![](https://img.shields.io/feedz/vpre/xunit/xunit/xunit.abstractions?logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.abstractions){ .version-ci .hidden }
> | Introduced | 2.0
> | Targets    | `net35`, `netstandard1.0`

### `xunit.assert.source`{ #xunit.assert.source }

This package contains the xUnit.net assertion library (i.e., the `Assert` class) in source form. The `Assert` class is a partial class, which allows developers to write additional methods available directly from `Assert`.

When you have multiple unit test libraries in your project, it is common practice to import this package into a "test utility" library where you write all your custom assertions, and then reference that "test utility" library from your unit test projects.

For more information, see the [`xunit/assert.xunit` README](https://github.com/xunit/assert.xunit/blob/main/README.md)

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.assert.source.svg?style=flat)](https://www.nuget.org/packages/xunit.assert.source){ .version-stable }[![](https://img.shields.io/feedz/vpre/xunit/xunit/xunit.assert.source?logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.assert.source){ .version-ci .hidden }
> | Targets    | `netstandard1.1` with C# 6 or later
> | Introduced | 2.0

### `xunit.extensibility.core`{ #xunit.extensibility.core }

This package contains `xunit.core.dll`. It is intended to be used by developers who wish to reference this DLL for extensibility purposes, such as writing your own theory data provider.

This package is referenced by [`xunit.core`](#xunit.core). It differs in that it does not include or copy the platform specific execution DLLs (`xunit.execution.*.dll`), and does not include the custom MSBuild activities required by unit test projects.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.extensibility.core.svg?style=flat)](https://www.nuget.org/packages/xunit.extensibility.core){ .version-stable }[![](https://img.shields.io/feedz/vpre/xunit/xunit/xunit.extensibility.core?logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.extensibility.core){ .version-ci .hidden }
> | Introduced | 2.0
> | Targets    | `net452`, `netstandard1.1`
> | Depends on | [`xunit.abstractions`](#xunit.abstractions)

### `xunit.extensibility.execution`{ #xunit.extensibility.execution }

This package contains the execution libraries for all supported target platforms (`xunit.execution.*.dll`). It is intended to be used by developers who wish to reference this DLL for extensibility purposes, such as creating custom test discovery and execution.

It differs from the [`xunit.core`](#xunit.core) package in that it adds a reference to the platform specific execution DLL, rather than simply copying to the project's output folder.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.extensibility.execution.svg?style=flat)](https://www.nuget.org/packages/xunit.extensibility.execution){ .version-stable }[![](https://img.shields.io/feedz/vpre/xunit/xunit/xunit.extensibility.execution?logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.extensibility.execution){ .version-ci .hidden }
> | Introduced | 2.0
> | Targets    | `net452`, `netstandard1.1`
> | Depends on | [`xunit.extensibility.core`](#xunit.extensibility.core)

### `xunit.runner.utility`{ #xunit.runner.utility }

This package contains the runner utility libraries for all supported target platforms (`xunit.runner.utility.*.dll`). It is intended to be used by developers who are writing their own test runners, and provides a version independent API for discovering and executing tests.

The libraries contained here are both backward and forward compatible for all xUnit.net v1 and v2 test projects.

> { .table-compact }
> |            |
> | ---------- | -----
> | Latest     | [![](https://img.shields.io/nuget/v/xunit.runner.utility.svg?style=flat)](https://www.nuget.org/packages/xunit.runner.utility){ .version-stable }[![](https://img.shields.io/feedz/vpre/xunit/xunit/xunit.runner.utility?logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.runner.utility){ .version-ci .hidden }
> | Introduced | 2.0
> | Targets    | `net35`, `net452`, `netstandard1.1`, `netstandard1.5`, `netcoreapp1.0`
> | Depends on | [`xunit.abstractions`](#xunit.abstractions)

## Retired packages{ #retired }

### `dotnet-xunit`{ #dotnet-xunit }

This package was retired as of 2.4 Beta 2. .NET/.NET Core users should use `xunit.runner.visualstudio` and the `dotnet test` command line.

### `xunit.execution`{ #xunit.execution }

This package was renamed to [`xunit.extensibility.execution`](#xunit.extensibility.execution) just before 2.0 shipped.

### `xunit.extensions`{ #xunit.extensions }

This package was part of xUnit.net v1, and is no longer shipped as part of xUnit.net. Some of the features of this package were integrated into the core framework, and others were moved to the [xUnit.net samples project](https://github.com/xunit/samples).

### `xunit.runner.aspnet`{ #xunit.runner.aspnet }

This package was retired as of 2.1 Beta 2.

### `xunit.runner.devices`{ #xunit.runner.devices }

This package was retired as of 2.5.25.

### `xunit.runner.dnx`{ #xunit.runner.dnx }

This package was retired as of 2.3.

### `xunit.runner.visualstudio.win8`{ #xunit.runner.visualstudio.win8 }

This functionality was rolled into [`xunit.runner.visualstudio`](#xunit.runner.visualstudio).

### `xunit.runner.visualstudio.wpa81`{ #xunit.runner.visualstudio.wpa81 }

This functionality was rolled into [`xunit.runner.visualstudio`](#xunit.runner.visualstudio).

### `xunit.runners`{ #xunit.runners }

This package was part of xUnit.net v1, and is no longer shipped as part of xUnit.net. It has been replaced with two new packages: [`xunit.runner.console`](#xunit.runner.console) and [`xunit.runner.msbuild`](#xunit.runner.msbuild).

<script defer src="nuget-packages.js"></script>
