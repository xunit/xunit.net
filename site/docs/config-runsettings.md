---
title: Config with RunSettings (VSTest)
---

# Config with RunSettings (VSTest)

RunSettings describes the configuration system provided by VSTest, the mechanism used by Visual Studio and `dotnet test` to run unit tests since roughly 2013. VSTest is currently being sunset in favor of Microsoft Testing Platform.

RunSettings can either by provided by an XML file (named `.runsettings`) which can be consumed by Test Explorer, `dotnet test`, `dotnet vstest`, and `vstest.console.exe`; or, it can be provided via command line options to `dotnet test`. The resulting RunSettings values are passed to `xunit.runner.visualstudio`, which parses the XML.

> [!WARNING]
> Since RunSettings are expressed as XML, it's important to remember that XML element names are case-sensitive. Please carefully verify that you are using the correct casing for the XML element names. This also applies to the command line switches for `dotnet test`, as they are transparently translated into XML elements behind the scenes.

> [!NOTE]
> RunSettings are only supported when running tests with VSTest. Running tests any other way (including using our first party runners, non-VSTest third party runners, or running tests in Microsoft.Testing.Platform mode) does not support RunSettings, and you should rely on [`xunit.runner.json`](config-xunit-runner-json) instead.

## Format of the `.runsettings` file{ #runsettings }

The `.runsettings` file is simply an XML file with a specific format. You will place your values inside an `xUnit` section in the configuration file. For example, to disable app domains and parallelization:

```xml
<RunSettings>
  <xUnit>
    <AppDomain>denied</AppDomain>
    <ParallelizeTestCollections>false</ParallelizeTestCollections>
  </xUnit>
</RunSettings>
```

> [!NOTE]
> For more information on using `.runsettings` files, please see the [Visual Studio documentation](https://learn.microsoft.com/visualstudio/test/configure-unit-tests-by-using-a-dot-runsettings-file).

## Format of the `dotnet test` command line switches{ #switches }

When passing RunSettings via the `dotnet test` command line, you will format them as `xUnit.key=value` pairs. For example, to disable app domains and parallelization:

```text
dotnet test path/to/myproject -- xUnit.AppDomain=denied xUnit.ParallelizeTestCollections=false
```

> [!NOTE]
> For more information on using command line switches for RunSettings with `dotnet test`, please see the [dotnet test documentation](https://learn.microsoft.com/dotnet/core/tools/dotnet-test).

## Supported configuration items{ #items }

All values list the minimum version of `xunit.runner.visualstudio` that is required to support the configuration value (previous versions will ignore unknown configuration values). Features that require support from the test framework itself will also list the minimum version of the test framework required for the feature; if you don't see a minimum test framework version, then it means the feature is implemented purely in the runner and works with all test framework versions.

### `AppDomain`{ #AppDomain }

Set this value to determine whether App Domains are used. By default, they will be used when available (the `ifAvailable` value). If you set this to `required`, it will require that app domains are available; if you set this to `denied`, it will not use app domains. _Note that App Domains are only supported with .NET Framework tests, and only with tests linked against xUnit.net framework v1 or v2._

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.5.1+
> | Test framework | v1, v2
> | Valid values   | `required`, `ifAvailable`, `denied`
> | Default value  | `ifAvailable`

### `AssertEquivalentMaxDepth`{ #AssertEquivalentMaxDepth }

Set this value to limit the recursive depth `Assert.Equivalent` will use when comparing objects for equivalence.

_This can also be set by environment variable `XUNIT_ASSERT_EQUIVALENT_MAX_DEPTH`. A value in the configuration file will take precedence over the environment variable._

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 3.0.2+
> | Test framework | v3 1.1+
> | Minimum value  | `1`
> | Default value  | `50`

### `Culture`{ #Culture }

Set this value to override the default culture used to run all unit tests in the assembly. You can pass `default` to use the default behavior (use the default culture of the operating system); you can pass `invariant` to use the invariant culture; or you can pass any string which describes a valid culture on the target operating system (see [BCP 47](https://www.rfc-editor.org/info/bcp47) for a description of how culture names are formatted; note that the list of supported cultures will vary by target operating system).

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 3.0.0+
> | Test framework | v3
> | Default value  | `default`

### `DiagnosticMessages`{ #DiagnosticMessages }

Set this value to `true` to include diagnostic information during test discovery and execution. Each runner has a unique method of presenting diagnostic messages.

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.5.1+
> | Test framework | v2, v3
> | Default value  | `false`

### `Explicit`{ #Explicit }

Change the way explicit tests are handled:

* `on` Run both explicit and non-explicit tests
* `off` Run only non-explicit tests
* `only` Run only explicit tests

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 3.0.0+
> | Test framework | v3
> | Default value  | `off`

### `FailSkips`{ #FailSkips }

Set this to `true` to enable skipped tests to be treated as errors.

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.5.1+
> | Default value  | `false`

### `FailWarns`{ #FailWarns }

Set this to `true` to enable warned tests to be treated as errors. By default, warnings will be reported with a passing test result.

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 3.0.0+
> | Test framework | v3
> | Default value  | `false`

### `InternalDiagnosticMessages`{ #InternalDiagnosticMessages }

Set this value to `true` to include internals diagnostic information during test discovery and execution. Each runner has a unique method of presenting diagnostic messages. Internal diagnostics often include information that is only useful when debugging the test framework itself.

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.5.1+
> | Test framework | v2, v3
> | Default value  | `false`

### `LongRunningTestSeconds`{ #LongRunningTestSeconds }

Set this value to enable long-running (hung) test detection. When the runner is idle waiting for tests to finished, it will report that fact once the timeout has passed. Use a value of `0` to disable the feature, or a positive integer value to enable the feature (time in seconds).

**NOTE:** Long running test messages are diagnostic messages. You must enable diagnostic messages in order to see the long running test warnings.

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.5.1+
> | Minimum value  | `0`
> | Default value  | `0` _(disabled)_

### `MaxParallelThreads`{ #MaxParallelThreads }

Set this to override the maximum number of threads to be used when parallelizing tests within this assembly. Use a value of `0` (or `default`^1^) to indicate that you would like the default behavior; use a value of `-1` (or `unlimited`^1^) to indicate that you do not wish to limit the number of threads used for parallelization; use a value with the multiplier syntax^1^ (i.e., values like `0.5x` or `2x`) to indicate that you wish you have a multiple of the number of CPU threads (so `2x` indicates you want the number of maximum parallel threads to be 2 * the number of CPU threads).

**^1^ NOTE:** While this configuration value has been present since runner version 2.5.1, the newer values (`default`, `unlimited`, and the multiplier syntax) are only supported with runner version 2.8 and later (and all Runners v3).

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runner version   | 2.5.1+
> | Test framework   | v2, v3
> | Default value    | The number of CPU threads

### `MethodDisplay`{ #MethodDisplay }

Set this to override the default display name for test cases. If you set this to `method`, the display name will be just the method (without the class name); if this set this value to `classAndMethod`, the default display name will be the fully qualified class name and method name.

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.5.1+
> | Test framework | v2, v3
> | Valid values   | `method`, `classAndMethod`
> | Default value  | `classAndMethod`

### `MethodDisplayOptions`{ #MethodDisplayOptions }

Set this to automatically perform transforms on default test names. This value can either be `all`, `none`, or a comma-separated combination of one or more of the following values:

* `replaceUnderscoreWithSpace` replaces all underscores with spaces.
* `useOperatorMonikers` replaces operator names with matching symbols. Replacements:
  * `eq` becomes `=`
  * `ne` becomes `!=`
  * `lt` becomes `<`
  * `le` becomes `<=`
  * `gt` becomes `>`
  * `ge` becomes `>=`
* `useEscapeSequences` replaces escape sequences in the format `Xnn` or `Unnnn` with their ASCII or Unicode character equivalents. Examples:
  * `X2C` becomes `,`
  * `U1D13` becomes `ᴓ`
* `replacePeriodWithComma` replaces periods with a comma and a space. This option is typically only useful if `methodDisplay` is `classAndMethod`.

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.5.1+
> | Test framework | v2, v3
> | Valid values   | `all`, `none`, or comma separated flags
> | Default value  | `none`

### `NoAutoReporters`{ #NoAutoReporters }

Set this to `true` to disable automatically enabled reporters (for example, reporters that automatically detect and enable support for AppVeyor, TeamCity, or Azure Pipelines). This is typically only used in debugging scenarios when trying to determine why tests aren't properly reporting into your CI environment.

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.5.1+
> | Test framework | v2, v3
> | Default value  | `false`

### `ParallelAlgorithm`{ #ParallelAlgorithm }

Set this to change the way tests are scheduled when they're running in parallel. For more information, see [Running Tests in Parallel](running-tests-in-parallel#algorithms). Note that the algorithm only applies when you have [enabled test collection parallelism](#parallelizeTestCollections), and are using a limited [number of threads](#maxParallelThreads) (i.e., not `unlimited` or `-1`).

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.8+
> | Test framework | v2 2.8+, v3
> | Valid values   | `conservative`, `aggressive`
> | Default value  | `conservative`

### `ParallelizeAssembly`{ #ParallelizeAssembly }

Set this to `true` if this assembly is willing to participate in parallelization with other assemblies. Test runners can use this information to automatically enable parallelization across assemblies if all the assemblies agree to it.

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.5.1+
> | Test framework | v2, v3
> | Default value  | `false`

### `ParallelizeTestCollections`{ #ParallelizeTestCollections }

Set this to `true` if the assembly is willing to run tests inside this assembly in parallel against each other. Tests in the same test collection will be run sequentially against each other, but tests in different test collections will be run in parallel against each other. Set this to `false` to disable all parallelization within this test assembly.

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.5.1+
> | Test framework | v2, v3
> | Default value  | `true`

### `PreEnumerateTheories`{ #PreEnumerateTheories }

Set this to `true` to pre-enumerate theories so that there is an individual test case for each theory data row. Set this to `false` to return a single test case for each theory without pre-enumerating the data ahead of time (this is how xUnit.net v1 used to behave). This is most useful for developers running tests inside Visual Studio, who wish to have the Code Lens test runner icons on their theory methods, since Code Lens does not support multiple tests from a single method.

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.5.1+
> | Test framework | v2, v3
> | Default value  | `true`

### `PrintMaxEnumerableLength`{ #PrintMaxEnumerableLength }

Set this value to limit the number of items to print in a collection (followed by an ellipsis when the collection is longer). This is also used when printing into the middle of a collection with a mismatch index, which means the printing may also start with an ellipsis.

Set this to `0` to always print the full collection.

_This can also be set by environment variable `XUNIT_PRINT_MAX_ENUMERABLE_LENGTH`. A value in the configuration file will take precedence over the environment variable._

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 3.0.2+
> | Test framework | v3 1.1+
> | Minimum value  | `0`
> | Default value  | `5`

### `PrintMaxObjectDepth`{ #PrintMaxObjectDepth }

Set this value to limit the recursive depth when printing objects (followed by an ellipsis when the object depth is too deep).

Set this to `0` to always print objects at all depths.

> [!WARNING]
> Disabling this when printing objects with circular references could result in an infinite loop that will cause an `OutOfMemoryException` and crash the test runner process.

_This can also be set by environment variable `XUNIT_PRINT_MAX_OBJECT_DEPTH`. A value in the configuration file will take precedence over the environment variable._

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 3.0.2+
> | Test framework | v3 1.1+
> | Minimum value  | `0`
> | Default value  | `3`

### `PrintMaxObjectMemberCount`{ #PrintMaxObjectMemberCount }

Set this value to limit the the number of members (fields and properties) to include when printing objects (followed by an ellipsis when there are more members).

Set this to `0` to always print all members.

_This can also be set by environment variable `XUNIT_PRINT_MAX_OBJECT_MEMBER_COUNT`. A value in the configuration file will take precedence over the environment variable._

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 3.0.2+
> | Test framework | v3 1.1+
> | Minimum value  | `0`
> | Default value  | `5`

### `PrintMaxStringLength`{ #PrintMaxStringLength }

Set this value to limit the number of characters to print in a string (followed by an ellipsis when the collection is longer). This is also used when printing into the middle of a string with a mismatch index, which means the printing may also start with an ellipsis.

Set this to `0` to always print full strings.

_This can also be set by environment variable `XUNIT_PRINT_MAX_STRING_LENGTH`. A value in the configuration file will take precedence over the environment variable._

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 3.0.2+
> | Test framework | v3 1.1+
> | Minimum value  | `0`
> | Default value  | `50`

### `ReporterSwitch`{ #ReporterSwitch }

Set this value to use a different reporter than the default. There are five reporters that ship with `xunit.runner.visualstudio` with behavior that deviates from the default:

* `quiet` will only print failure information
* `verbose` will print messages when tests start abd finish
* `json` will print messages in a JSON format
* `teamcity` will print TeamCity-formatted messages
* `silent` turns off all messages (xunit.runner.visualstudio 2.5.4+)

> [!NOTE]
> In order to see the reporter output, you will need to change the verbosity level of the console reporter. In Visual Studio, you can do this by opening the Options settings, navigating to `Test` > `General`, and the setting the drop down under "Logging Level" to "Diagnostic" (the logs in Visual Studio will appear in the Output window, under "Tests"). From the command line, you can pass `--logger "console;verbosity=normal"` on the `dotnet test` command line.

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.5.1+
> | Test framework | v1, v2, v3
> | Valid values   | `verbose`, `quiet`, `silent`, `json`, `teamcity`
> | Default value  | unset

### `Seed`{ #Seed }

Set this to set the seed used for randomization (affects how the test cases are randomized). This is only valid for v3.0+ test assemblies; it will be ignored for v1 or v2 assemblies. If the seed value isn't set, then the system will determine a reasonable seed (and print that seed when running the test assembly, to assist you in reproducing order-dependent failures).

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 3.0+
> | Test framework | v3
> | Minimum value  | `0`
> | Default value  | unset

### `ShadowCopy`{ #ShadowCopy }

Set this to `true` to use shadow copying when running tests in separate app domains; set to `false` to run tests without shadow   copying. When running tests without app domains, this value is ignored.

_This value is only valid for .NET Framework test projects targeting xUnit.net v1 and v2._

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.5.1+
> | Test framework | v1, v2
> | Default value  | `true`

### `ShowLiveOutput`{ #ShowLiveOutput }

Set this to `true` to show captured output messages live during the test run, in addition to showing them after the test has completed; set to `false` to only show test output messages after the test has completed.

> [!NOTE]
> When using `dotnet test` you may need to pass `--logger "console;verbosity=normal"` to be able to see messages from xUnit.net, as they may be hidden by default.

> { .table-compact }
> |                |
> | -------------- | -----
> | Runner version | 2.8.1+
> | Test framework | v2, v3
> | Default value  | `false`

### `StopOnFail`{ #StopOnFail }

Set this to `true` to stop running further tests once a test has failed. (Because of the asynchronous nature of test execution, this will not necessarily happen immediately; any test that is already in flight may complete, which may result in multiple test failures reported.)

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runner version   | 2.5.1+
> | Test framework   | v2, v3
> | Default value    | `false`
