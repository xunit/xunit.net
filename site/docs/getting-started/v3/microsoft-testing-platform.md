---
title: "Microsoft Testing Platform"
title-version: 2025 November 2
---

xUnit.net v3 includes built-in support for the new [Microsoft Testing Platform](https://learn.microsoft.com/dotnet/core/testing/unit-testing-platform-intro) (MTP).

Samples below are shown using:

* .NET 9 SDK (version `9.0.305`)
* `xunit.v3` (version `3.1.0`)
* `xunit.runner.visualstudio` (version `3.1.5`)

## What is Microsoft Testing Platform?

VSTest has been the underlying driver behind `dotnet test` and Test Explorer (and `vstest.console` and Test View before them) since it first launched in Visual Studio 2010. The new Microsoft Testing Platform aims to replace those with a new engine that is modernized, streamlined, performs better, and offers much greater extensibility for test framework authors.

Like xUnit.net v3—and for many of the same reasons—test projects for Microsoft Testing Platform are standalone executables. When a Microsoft Testing Platform test project is compiled, it can then be run directly (typically by invoking the already built executable, or using `dotnet run` to both build and run). This allows for a streamlined experience where the produced executable is all that's needed to run the tests.

The xUnit.net integration with Microsoft Testing Platform comes at three levels:

1. You can replace the default xUnit.net command line experience with the Microsoft Testing Platform command line experience

2. You can run tests with the new Microsoft Testing Platform integrated `dotnet test`

3. You can run tests with the new Microsoft Testing Platform integrated Test Explorer

Unlike our support for VSTest, our support for Microsoft Testing Platform is built natively into xUnit.net v3. If you want to rely solely on Microsoft Testing Platform support, you can remove the package references to `xunit.runner.visualstudio` and `Microsoft.NET.Test.Sdk`. However, for backward compatibility reasons, we recommend you leave these in place until you can be certain that all your supported versions of your development environments are using MTP instead of VSTest. Once all runners can support Microsoft Testing Platform, then we'll be able to deprecate `xunit.runner.visualstudio`. Supporting VSTest is separate from (and does not interfere with) our support for Microsoft Testing Platform.

### Choosing the Microsoft Testing Platform version

Starting with v3 build `3.2.0`, we now support the ability to choose which major version of Microsoft Testing Platform you wish to support. This includes:

* Microsoft Testing Platform v1 support **(default)**
* Microsoft Testing Platform v2 support
* Microsoft Testing Platform support disabled

If you are currently including the `xunit.v3` NuGet package, you may choose among these alternatives:

* `xunit.v3` (includes whatever the default version is, which is currently MTP v1)
* `xunit.v3.mtp-v1` (explicitly chooses MTP v1)
* `xunit.v3.mtp-v2` (explicitly chooses MTP v2)
* `xunit.v3.mtp-off` (explicitly disables MTP support)

If you are currently including the `xunit.v3.core` NuGet package, you may choose among these alternatives:

* `xunit.v3.core` (includes whatever the default version is, which is currently MTP v1)
* `xunit.v3.core.mtp-v1` (explicitly chooses MTP v1)
* `xunit.v3.core.mtp-v2` (explicitly chooses MTP v2)
* `xunit.v3.core.mtp-off` (explicitly disables MTP support)

At this point in time, we do not have a timeframe for when support for MTP v1 will be discontinued, as Microsoft has not announced any timeframe for the retirement of support for MTP v1. We also do not have a timeframe for if or when we might choose to move to MTP v2 by default (though when we do, that will be considered a breaking change, and we will bump the major version of the xUnit.net NuGet packages).

### Configuration with `testconfig.json`

Starting with v3 build `3.0.0`, you can use `testconfig.json` to provide xUnit.net configuration options for your test project. Note that this configuration is only applied when running in Microsoft Testing Platform mode.

For more information, see [Using `testconfig.json` with Microsoft Testing Platform](/docs/testconfig-json-mtp).

### Microsoft Testing Platform Telemetry

Starting with v3 build `3.2.0`, our NuGet packages now include package references to `Microsoft.Testing.Extensions.Telemetry`. This allows the Microsoft Testing Platform team to collect usage metrics.

You can disable metrics collection by setting an environment variable named `TESTINGPLATFORM_TELEMETRY_OPTOUT` to `1`.

For more information, see [Microsoft.Testing.Platform telemetry](https://learn.microsoft.com/en-us/dotnet/core/testing/microsoft-testing-platform-telemetry).

## Enabling the command line experience

By default, xUnit.net v3 projects have a native command line experience that is similar to our console runner command line experience.

If you `dotnet run` your test project, you should see something like this (examples using our test project from the [Getting Started](/docs/getting-started/v3/cmdline) documentation):

```shell
$ dotnet run
xUnit.net v3 In-Process Runner v3.1.0+03a071627b (64-bit .NET 8.0.20)
  Discovering: MyFirstUnitTests
  Discovered:  MyFirstUnitTests
  Starting:    MyFirstUnitTests
    MyFirstUnitTests.UnitTest1.FailingTest [FAIL]
      Assert.Equal() Failure: Values differ
      Expected: 5
      Actual:   4
      Stack Trace:
        UnitTest1.cs(14,0): at MyFirstUnitTests.UnitTest1.FailingTest()
    MyFirstUnitTests.UnitTest1.MyFirstTheory(value: 6) [FAIL]
      Assert.True() Failure
      Expected: True
      Actual:   False
      Stack Trace:
        UnitTest1.cs(28,0): at MyFirstUnitTests.UnitTest1.MyFirstTheory(Int32 value)
  Finished:    MyFirstUnitTests (ID = '80800e0af62d4c98efccd4ccc97bc5732c1c4655418cf54e297426d4f586eeff')
=== TEST EXECUTION SUMMARY ===
   MyFirstUnitTests  Total: 5, Errors: 0, Failed: 2, Skipped: 0, Not Run: 0, Time: 0.064s
```

If you want to replace the xUnit.net native command line experience with the Microsoft Testing Platform command line experience, add the following property to your project file (.csproj/.fsproj/.vbproj):

```xml
<PropertyGroup>
  <UseMicrosoftTestingPlatformRunner>true</UseMicrosoftTestingPlatformRunner>
</PropertyGroup>
```

Now, using `dotnet run`, you should see:

```shell
$ dotnet run
xUnit.net v3 Microsoft.Testing.Platform Runner v3.1.0+03a071627b (64-bit .NET 8.0.20)

failed MyFirstUnitTests.UnitTest1.FailingTest (7ms)
  Assert.Equal() Failure: Values differ
  Expected: 5
  Actual:   4
    at MyFirstUnitTests.UnitTest1.FailingTest() in UnitTest1.cs:14
failed MyFirstUnitTests.UnitTest1.MyFirstTheory(value: 6) (0ms)
  Assert.True() Failure
  Expected: True
  Actual:   False
    at MyFirstUnitTests.UnitTest1.MyFirstTheory(Int32 value) in UnitTest1.cs:28

Test run summary: Failed! - bin\Debug\net8.0\MyFirstUnitTests.dll (net8.0|x64)
  total: 5
  failed: 2
  succeeded: 3
  skipped: 0
  duration: 121ms
```

Using the Microsoft Testing Platform command line experience will give you a familiar UX if you frequently use other Microsoft Testing Platform integrated test frameworks, like [MSTest 3.6](https://github.com/microsoft/testfx/blob/main/docs/Changelog.md#3.6.0) or [TUnit](https://thomhurst.github.io/TUnit/).

The command line switches are different between the two platforms; type `dotnet run -- -?` to see them. The table below offers a rough mapping between the xUnit.net native command line option and the equivalent Microsoft Testing Platform command line option:

{ .table-smaller }
| xUnit.net native command line vs.<br />Microsoft Testing Platform command line            | Activity
| ----------------------------------------------------------------------------------------- | --------
| `:<seed>`<br />`--seed <seed>`                                                            | Set the randomization seed
| `path/to/configFile.json`<br />`--xunit-config-filename path/to/configFile.json`          | Set the configuration file
| `-assertEquivalentMaxDepth <option>`<br />`--assert-equivalent-max-depth <option>`        | Set the maximum depth for `Assert.Equivalent` comparisons
| `-class "name"`<br />`--filter-class "name"`^1^                                           | Run all tests in a given test class
| `-class- "name"`<br />`--filter-not-class "name"`^1^                                      | Do not run any tests in the given test class
| `-ctrf <file>`<br />`--report-ctrf --report-ctrf-filename <file>`^2^                      | Enable generating CTRF (JSON) report
| `-culture <option>`<br />`--culture <option>`                                             | Set the execution culture
| `-diagnostics`<br />`--xunit-diagnostics on`^3^                                           | Display diagnostic messages
| `-explicit <option>`<br />`--explicit <option>`                                           | Change the way explicit tests are handled
| `-failSkips`<br />`--fail-skips on`                                                       | Treat skipped tests as failed
| `-failSkips-`<br />`--fail-skips off`                                                     | Treat skipped tests as skipped
| `-failWarns`<br />`--fail-warns on`                                                       | Treat passing tests with warnings as failed
| `-failWarns-`<br />`--fail-warns off`                                                     | Treat passing tests with warnings as passed
| `-internalDiagnostics`<br />`--xunit-internal-diagnostics on`^3^                          | Display internal diagnostic messages
| `-html <file>`<br />`--report-xunit-html --report-xunit-html-filename <file>`^2^          | Enable generating xUnit.net HTML report
| `-jUnit <file>`<br />`--report-junit --report-junit-filename <file>`^2^                   | Enable generating JUnit (XML) report
| `-longRunning <seconds>`<br />`--long-running <seconds>`                                  | Enable long running (hung) test detection
| `-maxThreads <option>`<br />`--max-threads <option>`                                      | Set maximum thread count for collection parallelization
| `-method "name"`<br />`--filter-method "name"`^1^                                         | Run a given test method
| `-method- "name"`<br />`--filter-not-method "name"`^1^                                    | Do not run a given test method
| `-methodDisplay <option>`<br />`--method-display <option>`                                | Set default test display name
| `-methodDisplayOptions <option>`<br />`--method-display-options <option>`^4^              | Alters the default test display name
| `-namespace "name"`<br />`--filter-namespace "name"`^1^                                   | Run all tests in the given namespace
| `-namespace- "name"`<br />`--filter-not-namespace "name"`^1^                              | Do not run any tests in the given namespace
| `-noAutoReporters`<br />`--auto-reporters off`^3^                                         | Do not allow reporters to be auto-enabled by environment
| `-nUnit <file>`<br />`--report-nunit --report-nunit-filename <file>`^2^                   | Enable generating NUnit (v2.5 XML) report
| `-parallel <option>`<br />`--parallel <option>`                                           | Change test parallelization
| `-parallelAlgorithm <option>`<br />`--parallel-algorithm <option>`                        | Change the parallelization algorithm
| `-preEnumerateTheories`<br />`--pre-enumerate-theories on`^3^                             | Turns on theory pre-enumeration
| `-printMaxEnumerableLength <option>`<br />`--print-max-enumerable-length <option>`        | Set the maximum length when printing a collection
| `-printMaxObjectDepth <option>`<br />`--print-max-object-depth <option>`                  | Set the maximum child depth when printing an object
| `-printMaxObjectMemberCount <option>`<br />`--print-max-object-member-count <option>`     | Set the maximum member count when printing an object
| `-printMaxStringLength <option>`<br />`--print-max-string-length <option>`                | Set the maximum length when printing a string
| `-showLiveOutput`<br />`--show-live-output on`^3^                                         | Turns on live reporting of test output (from ITestOutputHelper)
| `-stopOnFail`<br />`--stop-on-fail on`^3^                                                 | Stop running tests after the first test failure
| `-trait "name=value"`<br />`--filter-trait "name=value"`^1^                               | Run all tests with a given trait value
| `-trait- "name=value"`<br />`--filter-not-trait "name=value"`^1^                          | Do not run any test with a given trait value
| `-trx <file>`<br />`--report-xunit-trx --report-xunit-trx-filename <file>`^2^             | Enable generating xUnit.net TRX report
| `-xml <file>`<br />`--report-xunit --report-xunit-filename <file>`^2^                     | Enable generating xUnit.net (v2+ XML) report

> [!NOTE]
> 1. Filter options in the xUnit.net command line interface must be specified one at a time, repeating the filter switch each time. With the Microsoft Testing Platform command line interface, multiple filters of the same kind can be specified with just a single switch. For example, `-class Foo -class Bar` in the xUnit.net command line interface can be expressed as `--filter-class Foo Bar` in the Microsoft Testing Platform command line interface.
>
> 2. Unlike in the xUnit.net command line experience, providing a filename with Microsoft Testing Platform for reports is optional, and will default to a name that includes the current username, computer name, and the current date & time when the report was run. If you specify the report filename in Microsoft Testing Platform, it must be a filename only without path components. All reports are output to the results folder, which defaults to `TestResults` under the output folder (and can be overridden with `--results-directory <directory>`).
>
> 3. These options in the xUnit.net command line interface can only be specified in one direction (either on or off, depending on the switch). The Microsoft Testing Platform command line interface allows both `on` and `off` command line options, which offers more flexibility (and in the `-?` help, describes which value is the default).
>
> 4. The method display options switch on the xUnit.net command line interface must be expressed as multiple values pushed together with commas. The Microsoft Testing Platform command line interface allows the individual values to be specified without the comma separators. For example, `-methodDisplayOptions replacePeriodWithComma,useEscapeSequences` in the xUnit.net command line interface is expressed as `--method-display-options replacePeriodWithComma useEscapeSequences` in the Microsoft Testing Platform command line interface.

There are several switches that are native to Microsoft Testing Platform, and they are discussed here: [https://learn.microsoft.com/dotnet/core/testing/unit-testing-platform-intro#options](https://learn.microsoft.com/dotnet/core/testing/unit-testing-platform-intro#options). Any xUnit.net command line option that isn't listed here, is either covered by one of the Microsoft Testing Platform switches, or is not available in Microsoft Testing Platform command line mode.

We have added one new switch (`--xunit-info`) which allows you to see the output that you'd normally see from the native xUnit.net command line experience, combined with the output from Microsoft Testing Platform:

```shell
$ dotnet run -- --xunit-info
xUnit.net v3 Microsoft.Testing.Platform Runner v3.1.0+03a071627b (64-bit .NET 8.0.20)

xUnit.net v3 In-Process Runner v3.1.0+03a071627b (64-bit .NET 8.0.20)
  Discovering: MyFirstUnitTests
  Discovered:  MyFirstUnitTests
  Starting:    MyFirstUnitTests
    MyFirstUnitTests.UnitTest1.FailingTest [FAIL]
      Assert.Equal() Failure: Values differ
      Expected: 5
      Actual:   4
      Stack Trace:
        UnitTest1.cs(14,0): at MyFirstUnitTests.UnitTest1.FailingTest()
    MyFirstUnitTests.UnitTest1.MyFirstTheory(value: 6) [FAIL]
      Assert.True() Failure
      Expected: True
      Actual:   False
      Stack Trace:
        UnitTest1.cs(28,0): at MyFirstUnitTests.UnitTest1.MyFirstTheory(Int32 value)
failed MyFirstUnitTests.UnitTest1.FailingTest (7ms)
  Assert.Equal() Failure: Values differ
  Expected: 5
  Actual:   4
    at MyFirstUnitTests.UnitTest1.FailingTest() in UnitTest1.cs:14
failed MyFirstUnitTests.UnitTest1.MyFirstTheory(value: 6) (0ms)
  Assert.True() Failure
  Expected: True
  Actual:   False
    at MyFirstUnitTests.UnitTest1.MyFirstTheory(Int32 value) in UnitTest1.cs:28
  Finished:    MyFirstUnitTests (ID = '80800e0af62d4c98efccd4ccc97bc5732c1c4655418cf54e297426d4f586eeff')
=== TEST EXECUTION SUMMARY ===
   MyFirstUnitTests  Total: 5, Errors: 0, Failed: 2, Skipped: 0, Not Run: 0, Time: 0.060s

Test run summary: Failed! - bin\Debug\net8.0\MyFirstUnitTests.dll (net8.0|x64)
  total: 5
  failed: 2
  succeeded: 3
  skipped: 0
  duration: 126ms
```

### Additional Microsoft Testing Platform features

If you enable the Microsoft Testing Platform command line experience, you will also be able to take advantage of their extension system to add new features to your test project. These include reporting extensions, code coverage extensions, and more. For more information, visit [Microsoft.Testing.Platform extensions](https://learn.microsoft.com/dotnet/core/testing/unit-testing-platform-extensions).

We have created documentation describing how to get [code coverage with Microsoft Testing Platform](/docs/getting-started/v3/code-coverage-with-mtp), since the standard Coverlet experience is not supported.

## Enabling the `dotnet test` experience

By default, xUnit.net v3 projects use VSTest when run via `dotnet test`, support for which comes from the `xunit.runner.visualstudio` package reference.

This is what the VSTest `dotnet test` output looks like (examples using our test project from the [Getting Started](/docs/getting-started/v3/getting-started) documentation):

```shell
$ dotnet test
Restore complete (0.2s)
  MyFirstUnitTests succeeded (0.1s) → bin\Debug\net8.0\MyFirstUnitTests.dll
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.1.5+1b188a7b0a (64-bit .NET 8.0.20)
[xUnit.net 00:00:00.09]   Discovering: MyFirstUnitTests
[xUnit.net 00:00:00.19]   Discovered:  MyFirstUnitTests
[xUnit.net 00:00:00.29]   Starting:    MyFirstUnitTests
[xUnit.net 00:00:00.34]     MyFirstUnitTests.UnitTest1.MyFirstTheory(value: 6) [FAIL]
[xUnit.net 00:00:00.34]       Assert.True() Failure
[xUnit.net 00:00:00.34]       Expected: True
[xUnit.net 00:00:00.34]       Actual:   False
[xUnit.net 00:00:00.34]       Stack Trace:
[xUnit.net 00:00:00.34]         UnitTest1.cs(28,0): at MyFirstUnitTests.UnitTest1.MyFirstTheory(Int32 value)
copyOfArgs, BindingFlags invokeAttr)
[xUnit.net 00:00:00.35]     MyFirstUnitTests.UnitTest1.FailingTest [FAIL]
[xUnit.net 00:00:00.35]       Assert.Equal() Failure: Values differ
[xUnit.net 00:00:00.35]       Expected: 5
[xUnit.net 00:00:00.35]       Actual:   4
[xUnit.net 00:00:00.35]       Stack Trace:
[xUnit.net 00:00:00.35]         UnitTest1.cs(14,0): at MyFirstUnitTests.UnitTest1.FailingTest()
[xUnit.net 00:00:00.35]   Finished:    MyFirstUnitTests (ID = '80800e0af62d4c98efccd4ccc97bc5732c1c4655418cf54e297426d4f586eeff')
  MyFirstUnitTests test failed with 2 error(s) (0.7s)
    UnitTest1.cs(28): error TESTERROR:
      MyFirstUnitTests.UnitTest1.MyFirstTheory(value: 6) (5ms): Error Message: Assert.True() Failure
      Expected: True
      Actual:   False
      Stack Trace:
         at MyFirstUnitTests.UnitTest1.MyFirstTheory(Int32 value) in UnitTest1.cs:line 28
    UnitTest1.cs(14): error TESTERROR:
      MyFirstUnitTests.UnitTest1.FailingTest (2ms): Error Message: Assert.Equal() Failure: Values differ
      Expected: 5
      Actual:   4
      Stack Trace:
         at MyFirstUnitTests.UnitTest1.FailingTest() in UnitTest1.cs:line 14

Test summary: total: 5, failed: 2, succeeded: 3, skipped: 0, duration: 0.7s
Build failed with 2 error(s) in 1.2s
```

To enable MTP compatibility with the VSTest mode of `dotnet test`, add the following property to your project file (.csproj/.fsproj/.vbproj):

```xml
<PropertyGroup>
  <TestingPlatformDotnetTestSupport>true</TestingPlatformDotnetTestSupport>
</PropertyGroup>
```

Now when running `dotnet test`, your output should looks something like this:

```shell
$ dotnet test
Restore complete (0.2s)
  MyFirstUnitTests succeeded (0.2s) → bin\Debug\net8.0\MyFirstUnitTests.dll
  MyFirstUnitTests test failed with 1 error(s) (0.3s)
    bin\Debug\net8.0\MyFirstUnitTests.dll : error run failed: Tests failed: 'bin\Debug\net8.0\TestResults\MyFirstUnitTests_net8.0_x64.log' [net8.0|x64]

Test summary: total: 5, failed: 2, succeeded: 3, skipped: 0, duration: 0.2s
Build failed with 1 error(s) in 0.9s
```

A log file is always generated from `dotnet test` runs, but is usually only shown when the test run failed. The failure log in this case looks like:

```text
xUnit.net v3 Microsoft.Testing.Platform Runner v3.1.0+03a071627b (64-bit .NET 8.0.20)

failed MyFirstUnitTests.UnitTest1.FailingTest (7ms)
  Assert.Equal() Failure: Values differ
  Expected: 5
  Actual:   4
    at MyFirstUnitTests.UnitTest1.FailingTest() in UnitTest1.cs:14
failed MyFirstUnitTests.UnitTest1.MyFirstTheory(value: 6) (0ms)
  Assert.True() Failure
  Expected: True
  Actual:   False
    at MyFirstUnitTests.UnitTest1.MyFirstTheory(Int32 value) in UnitTest1.cs:28

Test run summary: Failed! - bin\Debug\net8.0\MyFirstUnitTests.dll (net8.0|x64)
  total: 5
  failed: 2
  succeeded: 3
  skipped: 0
  duration: 147ms

=== COMMAND LINE ===
bin\Debug\net8.0\MyFirstUnitTests.exe --internal-msbuild-node testingplatform.pipe.b1e011b9ecd145c696246de44d7af67c
```

The same command line options available in the Microsoft Testing Platform command line experience (described in the table above) are also available for `dotnet test`. The command line options are passed after `--`. For example, to filter tests to a single class with the Microsoft Testing Platform `dotnet test` experience, you could run: `dotnet test -- --filter-class ClassName`. This includes command line options from any [Microsoft Testing Platform features](#additional-microsoft-testing-platform-features) you may add. _**Note:** These command line options are available for `dotnet test` regardless of whether you enable the Microsoft Testing Platform command line experience._

You can find additional configuration options for the Microsoft Testing Platform `dotnet test` integration here: [Microsoft.Testing.Platform mode of `dotnet test`](https://learn.microsoft.com/dotnet/core/testing/unit-testing-with-dotnet-test#microsofttestingplatform-mtp-mode-of-dotnet-test)

## Enabling the Test Explorer experience

> [!NOTE]
> As of the updating of this document, the current version of Visual Studio 2022 (17.14.16) has the Microsoft Testing Platform Test Explorer experience enabled by default.

Like all Microsoft Testing Platform test framework projects, xUnit.net v3 projects are automatically enabled for the new Microsoft Testing Platform Test Explorer experience.

While we don't anticipate any issues, if you think you are experiencing an issues with Test Explorer in Microsoft Testing Platform mode, you can temporarily disable it by adding the following property to your project file (.csproj/.fsproj/.vbproj) and then restarting Visual Studio:

```xml
<PropertyGroup>
  <DisableTestingPlatformServerCapability>true</DisableTestingPlatformServerCapability>
</PropertyGroup>
```

If you do find such an issue, please be sure to let us and/or the Visual Studio team know about it, so we can determine where the issue resides.

Just a reminder that VSTest mode requires the package references to `xunit.runner.visualstudio` and `Microsoft.NET.Test.Sdk`. We recommend you keep these package references if you are using any test runners which have not been updated for Microsoft Testing Platform (for example, the need to use older versions of Visual Studio).
