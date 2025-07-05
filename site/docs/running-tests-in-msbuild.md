---
title: Running xUnit.net tests in MSBuild
---

# Running xUnit.net tests in MSBuild

xUnit.net includes a runner which can be used from your MSBuild scripts to run unit tests. The runner is contained in the NuGet package `xunit.runner.msbuild`. When including this NuGet package into a project, the project file (for example, the `.csproj` file) will automatically gain access to the `<xunit>` task. The examples below show custom MSBuild files; when using `<xunit>` from within a project file, you can skip the `<UsingTask>` line.

## Running a single assembly with the <xunit> task{ #single }

Here is an example MSBuild file using the `<xunit>` task to run a single assembly:

**xUnit.net v2**

```xml
<Project DefaultTargets="Test">

  <UsingTask
    AssemblyFile="path\to\xunit.runner.msbuild.net452.dll"
    TaskName="Xunit.Runner.MSBuild.xunit"/>

  <Target Name="Test">
    <xunit Assemblies="path\to\MyTests.dll" />
  </Target>

</Project>
```

**xUnit.net v3**

```xml
<Project DefaultTargets="Test">

  <UsingTask
    AssemblyFile="path\to\xunit.v3.runner.msbuild.dll"
    TaskName="Xunit.Runner.MSBuild.xunit"/>

  <Target Name="Test">
    <xunit Assemblies="path\to\MyTests.dll" />
  </Target>

</Project>
```

## <a id="multiple"></a>Running multiple assemblies with the <xunit> task

Often, you will want to run multiple assemblies. You can use an [`<ItemGroup>`](https://learn.microsoft.com/visualstudio/msbuild/itemgroup-element-msbuild) in MSBuild to specify which test DLLs to run (including using the wildcard syntax to automatically find test DLLs).

Here is an example MSBuild file using the `<xunit>` task to run several assemblies:

**xUnit.net v2**

```xml
<Project DefaultTargets="Test">

  <UsingTask
    AssemblyFile="path\to\xunit.runner.msbuild.net452.dll"
    TaskName="Xunit.Runner.MSBuild.xunit" />

  <ItemGroup>
    <TestAssemblies Include="**\bin\Release\*.tests.dll" />
  </ItemGroup>

  <Target Name="Test">
    <xunit Assemblies="@(TestAssemblies)" />
  </Target>

</Project>
```

**xUnit.net v3**

```xml
<Project DefaultTargets="Test">

  <UsingTask
    AssemblyFile="path\to\xunit.v3.runner.msbuild.dll"
    TaskName="Xunit.Runner.MSBuild.xunit" />

  <ItemGroup>
    <TestAssemblies Include="**\bin\Release\*.tests.dll" />
  </ItemGroup>

  <Target Name="Test">
    <xunit Assemblies="@(TestAssemblies)" />
  </Target>

</Project>
```

## Options for the MSBuild runner{ #options }

The `<xunit>` MSBuild task has several options to help you configure your test execution. The version column indicates the minimum version of the MSBuild runner you must be using to take advantage of the feature.

| Property                     | Min&nbsp;Version    | Usage
| ---------------------------- | ------------------- | -----
| `AppDomains`                 | v2 2.1              | **[Optional][Boolean]** Controls whether app domains are used for test discovery and execution. Defaults to `true`. (With the v3 runner, string values `"IfAvailable"`, `"Required"`, and `"Denied"` are also accepted.)
| `Assemblies`                 | v2 2.0              | **[Required][ItemGroup]** The item group of the assemblies (DLLs) run tests for.
| `Ctrf`                       | v3 1.0              | **[Optional][String]** Filename where a [Common Test Report Format](https://ctrf.io) report will be generated after run.
| `Culture`                    | v3 1.0              | **[Optional][String]** Controls the default culture for tests. Can be set to `default` (default OS culture), `invariant` (invariant culture), or any legal culture string (legal values are operating system dependent).<br />_**NOTE:** Only supported by v3 or later test projects._
| `DiagnosticMessages`         | v2 2.1              | **[Optional][Boolean]** Set to true to include diagnostic messages when running tests.
| `ExcludeTraits`              | v2 2.0              | **[Optional][String]** When set, excludes tests that match the given traits (in `Name=Value;Name=Value` format).
| `ExitCode`                   | v2 2.0              | **[Output][Integer]** Returns the exit code from the execution.
| `Explicit`                   | v3 1.0              | **[Optional][String]** Controls how explicit tests are handled. Acceptable values include `"off"`, `"on"`, and `"only"`. Defaults to `"off"`.<br />_**NOTE:** Only supported by v3 or later test projects._
| `FailSkips`                  | v2 2.1              | **[Optional][Boolean]** Set to true to cause skipped tests to become failures.
| `FailWarns`                  | v3 1.0              | **[Optional][Boolean]** Set to true to cause otherwise passing tests with warnings to become failures.<br />_**NOTE:** Only supported by v3 or later test projects._
| `Html`                       | v2 2.0              | **[Optional][String]** Filename where an HTML report will be generated after run.
| `IgnoreFailures`             | v2 2.2              | **[Optional][Boolean]** Set to true to allow the build to continue after tests have failed.
| `IncludeTraits`              | v2 2.0              | **[Optional][String]** When set, only runs tests that match the given traits (in `Name=Value;Name=Value` format)
| `InternalDiagnosticMessages` | v2 2.3              | **[Optional][Boolean]** Set to try to include internal diagnostic messages when running tests.
| `JUnit`                      | v2 2.4              | **[Optional][String]** FIlename where a JUnit report will be generated after run.
| `MaxParallelThreads`         | v2 2.0              | **[Optional][String]** Controls the number of threads to use when running tests in parallel. Can be set to `"default"` (one thread per CPU), `"unlimited"`, or any positive number.
| `MethodDisplay`              | v3 1.0              | **[Optional][String]** Controls how default test method display names are generated. Acceptable values include `"ClassAndMethod"` and `"Method"`. Defaults to `"ClassAndMethod"`.
| `MethodDisplayOptions`       | v3 1.0              | **[Optional][String]** Controls how default test method display names are transformed. Acceptable values include `"all"`", `"none"`, or a comma-separated list of one or more of `"ReplaceUnderscoreWithSpace"`, `"UseOperatorMonikers"`, `"UseEscapeSequences"`, and `"ReplacePeriodWithComma"`. For more information on how these operate, see the [`methodDisplayOptions` configuration file setting](/docs/config-xunit-runner-json#methodDisplayOptions).
| `NoAutoReporters`            | v2 2.2              | **[Optional][Boolean]** Stops reporters (like TeamCity or AppVeyor) which automatically enable themselves based on the runtime environment.
| `NoLogo`                     | v2 2.1              | **[Optional][Boolean]** When set, disables the display of the MSBuild runner welcome banner.
| `NUnit`                      | v2 2.1              | **[Optional][String]** Filename where an XML report (in NUnit format) will be generated after run.
| `ParallelAlgorithm`          | v2 2.8              | **[Optional][String]** Controls how parallelism performs. Acceptable values include `"conservative"` and `"aggressive"`. Defaults to `"conservative"`. See the [parallel algorithm documentation](/docs/running-tests-in-parallel#algorithms) for more information.
| `ParallelizeAssemblies`      | v2 2.0              | **[Optional][Boolean]** When set to `true`, runs multiple assemblies in parallel. Defaults to `false`.
| `ParallelizeTestCollections` | v2 2.0              | **[Optional][Boolean]** When set to `true`, runs multiple test collections in parallel. Defaults to `true`.
| `PreEnumerateTheories`       | v3 1.0              | **[Optional][Boolean]** When set to `true`, theory data will be pre-enumerated. Defaults to `false`.
| `Reporter`                   | v2 2.1              | **[Optional][String]** Selects a reporter to use for printing test results. Default reporters include `teamcity`, `appveyor`, `verbose`, and `quiet`; v3 also adds `silent` and `json`. Third parties can add additional reporter options.
| `ShadowCopy`                 | v2 2.0              | **[Optional][Boolean]** Determines whether the tests are run as a shadow copy. Defaults to `true`. Ignored when `AppDomains` is set to `false`.
| `ShowLiveOutput`             | v2 2.8.1            | **[Optional][Boolean]** When set to `true`, captured output is shown live when it's captured. Defaults to `false`.
| `StopOnFail`                 | v2 2.3              | **[Optional][Boolean]** When set to `true`, attempts to cancel running tests after the first test failure is encountered. Defaults to `false`.
| `Trx`                        | v3 1.0              | **[Optional][String]** Filename where a TRX report will be generated after run.
| `WorkingFolder`              | v2 2.0              | **[Optional][String]** The working folder where the tests are executed from. Defaults to the folder containing the assembly DLL.
| `Xml`                        | v2 2.0              | **[Optional][String]** Filename where an XML report (in [xUnit.net Format v2](/docs/format-xml-v2)) will be generated after run.
| `XmlV1`                      | v2 2.0              | **[Optional][String]** Filename where an XML report (in [xUnit.net Format v1](/docs/format-xml-v1)) will be generated after run.

> [!NOTE]
> When running multiple assemblies, you can specify the configuration file for each assembly using [ItemGroup metadata](https://learn.microsoft.com/visualstudio/msbuild/itemgroup-element-msbuild). The <xunit> task looks for metadata named `ConfigFile` on each item in your item group. Configuration files are ignored when `AppDomains` is set to `false`.

The following configuration elements were deprecated in 2.1, and removed from 2.2.

| Property   | Replacement
| ---------- | -----------
| `TeamCity` | Set `Reporter` to `teamcity`
| `Verbose`  | Set `Reporter` to `verbose`

## <a id="choosing"></a>Choosing MSBuild runner vs. Console runner

You can use an [`<exec>` task](https://learn.microsoft.com/visualstudio/msbuild/exec-task) in MSBuild to run the console runner. You may choose to use the console runner if you need more control over running tests in 32- vs. 64-bit environments. When using the MSBuild runner, you are restricted to the bitness choice of the MSBuild executable that you used to run your build.
