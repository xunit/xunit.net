---
title: Config with xunit.runner.json
---

# Config with `xunit.runner.json`

Configuration files can be used to configure xUnit.net on a per test-assembly basis.

> [!NOTE]
> In these examples, we tell you to use the file name `xunit.runner.json`. You can also use `<AssemblyName>.xunit.runner.json` (where `<AssemblyName>` is the name of your unit test assembly, without the file extension like `.dll` or `.exe`). You should only need to use this longer name format if your unit tests DLLs will all be placed into the same output folder, and you need to disambiguate the various configuration files.
>
> The assembly-specific filename takes precedence over the non-specific filename; there is **no merging** of values between files.

## Adding the configuration file{ #file }


1. Add a new JSON file to the root of your test project. Name the file `xunit.runner.json`. Start with a schema reference so that text editors (like Visual Studio & Visual Studio Code) can provide auto-complete behavior while editing the file:

   ```json
   {
     "$schema": "https://xunit.net/schema/current/xunit.runner.schema.json"
   }
   ```

1. Tell the build system to copy the file into the build output directory. Edit your `.csproj` file and add the following:

   ```xml
   <ItemGroup>
     <Content Include="xunit.runner.json" CopyToOutputDirectory="PreserveNewest" />
   </ItemGroup>
   ```

## Supported configuration items{ #items }

The current schema is online at [https://xunit.net/schema/current/xunit.runner.schema.json](https://xunit.net/schema/current/xunit.runner.schema.json), which can be set in the JSON file to aid with intellisense from development IDEs like [Visual Studio](https://visualstudio.microsoft.com/) and [Visual Studio Code](https://code.visualstudio.com/).

Configuration elements are placed inside a top-level object:

```json
{
  "$schema": "https://xunit.net/schema/current/xunit.runner.schema.json",
  "enum-or-string-key": "value1",
  "boolean-key": true,
  "integer-key": 42
}
```

All values list the minimum runner version that is required to support the configuration value (previous versions will ignore unknown configuration values). Features that require support from the test framework itself will also list the minimum version of the test framework required for the feature; if you don't see a minimum test framework version, then it means the feature is implemented purely in the runners and works with all test framework versions.

### `appDomain`{ #appDomain }

Set this value to determine whether App Domains are used. By default, they will be used when available (the `"ifAvailable"` value). If you set this to `"required"`, it will require that app domains are available; if you set this to `"denied"`, it will not use app domains. _Note that App Domains are only supported with .NET Framework tests, and only with tests linked against xUnit.net framework v1 or v2._

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.1+, v3
> | Test framework   | v1, v2
> | JSON schema type | String
> | Valid values     | `"required"`, `"ifAvailable"`, `"denied"`
> | Default value    | `"ifAvailable"`

### `assertEquivalentMaxDepth`{ #assertEquivalentMaxDepth }

Set this value to limit the recursive depth `Assert.Equivalent` will use when comparing objects for equivalence.

_This can also be set by environment variable `XUNIT_ASSERT_EQUIVALENT_MAX_DEPTH`. A value in the configuration file will take precedence over the environment variable._

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v3 1.1+
> | Test framework   | v3 1.1+
> | JSON schema type | 32-bit integer
> | Minimum value    | `1`
> | Default value    | `50`

### `culture`{ #culture }

Set this value to override the default culture used to run all unit tests in the assembly. You can pass `"default"` to use the default behavior (use the default culture of the operating system); you can pass `"invariant"` to use the invariant culture; or you can pass any string which describes a valid culture on the target operating system (see [BCP 47](https://www.rfc-editor.org/info/bcp47) for a description of how culture names are formatted; note that the list of supported cultures will vary by target operating system).

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v3
> | Test framework   | v3
> | JSON schema type | String
> | Default value    | `"default"`

### `diagnosticMessages`{ #diagnosticMessages }

Set this value to `true` to include diagnostic information during test discovery and execution. Each runner has a unique method of presenting diagnostic messages.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.1+, v3
> | Test framework   | v2, v3
> | JSON schema type | Boolean
> | Default value    | `false`

### `failSkips`{ #failSkips }

Set this to `true` to enable skipped tests to be treated as errors.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.5+, v3
> | JSON schema type | Boolean
> | Default value    | `false`

### `failWarns`{ #failWarns }

Set this to `true` to enable warned tests to be treated as errors. By default, warnings will be reported with a passing test result.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v3
> | Test framework   | v3
> | JSON schema type | Boolean
> | Default value    | `false`

### `internalDiagnosticMessages`{ #internalDiagnosticMessages }

Set this value to `true` to include internals diagnostic information during test discovery and execution. Each runner has a unique method of presenting diagnostic messages. Internal diagnostics often include information that is only useful when debugging the test framework itself.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.1+, v3
> | Test framework   | v2, v3
> | JSON schema type | Boolean
> | Default value    | `false`

### `longRunningTestSeconds`{ #longRunningTestSeconds }

Set this value to enable long-running (hung) test detection. When the runner is idle waiting for tests to finished, it will report that fact once the timeout has passed. Use a value of `0` to disable the feature, or a positive integer value to enable the feature (time in seconds).

**NOTE:** Long running test messages are diagnostic messages. You must enable diagnostic messages in order to see the long running test warnings.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.2+, v3
> | JSON schema type | 32-bit integer
> | Minimum value    | `0`
> | Default value    | `0` _(disabled)_

### `maxParallelThreads`{ #maxParallelThreads }

Set this to override the maximum number of threads to be used when parallelizing tests within this assembly. Use a value of `0` (or `"default"`^1^) to indicate that you would like the default behavior; use a value of `-1` (or `"unlimited"`^1^) to indicate that you do not wish to limit the number of threads used for parallelization; use a value with the multiplier syntax^1^ (i.e., values like `"0.5x"` or `"2x"`) to indicate that you wish you have a multiple of the number of CPU threads (so `2x` indicates you want the number of maximum parallel threads to be 2 * the number of CPU threads).

**^1^ NOTE:** While this configuration value has been present since v2 2.1, the newer values (`"default"`, `"unlimited"`, and the multiplier syntax) are only supported with Runners v2 2.8 and later (and all Runners v3).

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.1+, v3
> | Test framework   | v2, v3
> | JSON schema type | 32-bit integer (schema v2.1+), 32-bit integer or string (schema v2.8+)
> | Default value    | The number of CPU threads

### `methodDisplay`{ #methodDisplay }

Set this to override the default display name for test cases. If you set this to `"method"`, the display name will be just the method (without the class name); if this set this value to `"classAndMethod"`, the default display name will be the fully qualified class name and method name.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.1+, v3
> | Test framework   | v2, v3
> | JSON schema type | String
> | Valid values     | `"method"`, `"classAndMethod"`
> | Default value    | `"classAndMethod"`

### `methodDisplayOptions`{ #methodDisplayOptions }

Set this to automatically perform transforms on default test names. This value can either be `all`, `none`, or a comma-separated combination of one or more of the following values:

* `"replaceUnderscoreWithSpace"` replaces all underscores with spaces.
* `"useOperatorMonikers"` replaces operator names with matching symbols. Replacements:
  * `eq` becomes `=`
  * `ne` becomes `!=`
  * `lt` becomes `&lt;`
  * `le` becomes `&lt;=`
  * `gt` becomes `&gt;`
  * `ge` becomes `&gt;=`
* `"useEscapeSequences"` replaces escape sequences in the format `Xnn` or `Unnnn` with their ASCII or Unicode character equivalents. Examples:
  * `X2C` becomes `,`
  * `U1D13` becomes `ᴓ`
* `"replacePeriodWithComma"` replaces periods with a comma and a space. This option is typically only useful if `methodDisplay` is `classAndMethod`.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.4+, v3
> | Test framework   | v2, v3
> | JSON schema type | String flags
> | Valid values     | `"all"`, `"none"`, or comma separated flags
> | Default value    | `"none"`

### `parallelAlgorithm`{ #parallelAlgorithm }

Set this to change the way tests are scheduled when they're running in parallel. For more information, see [Running Tests in Parallel](/docs/running-tests-in-parallel#algorithms). Note that the algorithm only applies when you have [enabled test collection parallelism](#parallelizeTestCollections), and are using a limited [number of threads](#maxParallelThreads) (i.e., not `unlimited` or `-1`).

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.8+, v3
> | Test framework   | v2 2.8+, v3
> | JSON schema type | String
> | Valid values     | `"conservative"`, `"aggressive"`
> | Default value    | `"conservative"`

### `parallelizeAssembly`{ #parallelizeAssembly }

Set this to `true` if this assembly is willing to participate in parallelization with other assemblies. Test runners can use this information to automatically enable parallelization across assemblies if all the assemblies agree to it.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.1+, v3
> | Test framework   | v2, v3
> | JSON schema type | Boolean
> | Default value    | `false`

### `parallelizeTestCollections`{ #parallelizeTestCollections }

Set this to `true` if the assembly is willing to run tests inside this assembly in parallel against each other. Tests in the same test collection will be run sequentially against each other, but tests in different test collections will be run in parallel against each other. Set this to `false` to disable all parallelization within this test assembly.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.1+, v3
> | Test framework   | v2, v3
> | JSON schema type | Boolean
> | Default value    | `true`

### `preEnumerateTheories`{ #preEnumerateTheories }

Set this to `true` to pre-enumerate theories so that there is an individual test case for each theory data row. Set this to `false` to return a single test case for each theory without pre-enumerating the data ahead of time (this is how xUnit.net v1 used to behave). This is most useful for developers running tests inside Visual Studio, who wish to have the Code Lens test runner icons on their theory methods, since Code Lens does not support multiple tests from a single method.

This value does not have a default, because it's up to each individual test runner to decide what the best default behavior is. The Visual Studio adapter, for example, will default to `true`, while the console and MSBuild runners will default to `false`.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.1+, v3
> | Test framework   | v2, v3
> | JSON schema type | Boolean

### `printMaxEnumerableLength`{ #printMaxEnumerableLength }

Set this value to limit the number of items to print in a collection (followed by an ellipsis when the collection is longer). This is also used when printing into the middle of a collection with a mismatch index, which means the printing may also start with an ellipsis.

Set this to `0` to always print the full collection.

_This can also be set by environment variable `XUNIT_PRINT_MAX_ENUMERABLE_LENGTH`. A value in the configuration file will take precedence over the environment variable._

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v3 1.1+
> | Test framework   | v3 1.1+
> | JSON schema type | 32-bit integer
> | Minimum value    | `0`
> | Default value    | `5`

### `printMaxObjectDepth`{ #printMaxObjectDepth }

Set this value to limit the recursive depth when printing objects (followed by an ellipsis when the object depth is too deep).

Set this to `0` to always print objects at all depths.

> [!WARNING]
> Disabling this when printing objects with circular references could result in an infinite loop that will cause an `OutOfMemoryException` and crash the test runner process.

_This can also be set by environment variable `XUNIT_PRINT_MAX_OBJECT_DEPTH`. A value in the configuration file will take precedence over the environment variable._

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v3 1.1+
> | Test framework   | v3 1.1+
> | JSON schema type | 32-bit integer
> | Minimum value    | `0`
> | Default value    | `3`

### `printMaxObjectMemberCount`{ #printMaxObjectMemberCount }

Set this value to limit the the number of members (fields and properties) to include when printing objects (followed by an ellipsis when there are more members).

Set this to `0` to always print all members.

_This can also be set by environment variable `XUNIT_PRINT_MAX_OBJECT_MEMBER_COUNT`. A value in the configuration file will take precedence over the environment variable._

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v3 1.1+
> | Test framework   | v3 1.1+
> | JSON schema type | 32-bit integer
> | Minimum value    | `0`
> | Default value    | `5`

### `printMaxStringLength`{ #printMaxStringLength }

Set this value to limit the number of characters to print in a string (followed by an ellipsis when the collection is longer). This is also used when printing into the middle of a string with a mismatch index, which means the printing may also start with an ellipsis.

Set this to `0` to always print full strings.

_This can also be set by environment variable `XUNIT_PRINT_MAX_STRING_LENGTH`. A value in the configuration file will take precedence over the environment variable._

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v3 1.1+
> | Test framework   | v3 1.1+
> | JSON schema type | 32-bit integer
> | Minimum value    | `0`
> | Default value    | `50`

### `seed`{ #seed }

Set this to set the seed used for randomization (affects how the test cases are randomized). This is only valid for v3.0+ test assemblies; it will be ignored for v1 or v2 assemblies. If the seed value isn't set, then the system will determine a reasonable seed (and print that seed when running the test assembly, to assist you in reproducing order-dependent failures).

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v3
> | Test framework   | v3
> | JSON schema type | 32-bit integer
> | Minimum value    | `0`
> | Default value    | unset

### `shadowCopy`{ #shadowCopy }

Set this to `true` to use shadow copying when running tests in separate app domains; set to `false` to run tests without shadow   copying. When running tests without app domains, this value is ignored.

_This value is only valid for .NET Framework test projects targeting xUnit.net v1 and v2._

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.1+, v3
> | Test framework   | v1, v2
> | JSON schema type | Boolean
> | Default value    | `true`

### `showLiveOutput`{ #showLiveOutput }

Set this to `true` to show messages from `ITestOutputHelper` live during the test run, in addition to showing them after the test has completed; set to `false` to only show test output messages after the test has completed.

> [!NOTE]
> When using `dotnet test` you may need to pass `--logger "console;verbosity=normal"` to be able to see messages from xUnit.net, as they may be hidden by default.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.8.1+, v3
> | Test framework   | v2, v3
> | JSON schema type | Boolean
> | Default value    | `false`

### `stopOnFail`{ #stopOnFail }

Set this to `true` to stop running further tests once a test has failed. (Because of the asynchronous nature of test execution, this will not necessarily happen immediately; any test that is already in flight may complete, which may result in multiple test failures reported.)

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Runners          | v2 2.5+, v3
> | Test framework   | v2, v3
> | JSON schema type | Boolean
> | Default value    | `false`

## List of schema versions{ #schemas }

| Version            | Url
| ------------------ | ---
| 3.1<br />(current) | [https://xunit.net/schema/v3.1/xunit.runner.schema.json](/schema/v3.1/xunit.runner.schema.json)<br />[https://xunit.net/schema/current/xunit.runner.schema.json](/schema/current/xunit.runner.schema.json)
| 3.0                | [https://xunit.net/schema/v3.0/xunit.runner.schema.json](/schema/v3.0/xunit.runner.schema.json)
| 2.8.1              | [https://xunit.net/schema/v2.8.1/xunit.runner.schema.json](/schema/v2.8.1/xunit.runner.schema.json)
| 2.8                | [https://xunit.net/schema/v2.8/xunit.runner.schema.json](/schema/v2.8/xunit.runner.schema.json)
| 2.5                | [https://xunit.net/schema/v2.5/xunit.runner.schema.json](/schema/v2.5/xunit.runner.schema.json)
| 2.4                | [https://xunit.net/schema/v2.4/xunit.runner.schema.json](/schema/v2.4/xunit.runner.schema.json)
| 2.3                | [https://xunit.net/schema/v2.3/xunit.runner.schema.json](/schema/v2.3/xunit.runner.schema.json)
| 2.2                | [https://xunit.net/schema/v2.2/xunit.runner.schema.json](/schema/v2.2/xunit.runner.schema.json)
| 2.1                | [https://xunit.net/schema/v2.1/xunit.runner.schema.json](/schema/v2.1/xunit.runner.schema.json)
| 1.0                | [https://xunit.net/schema/v1/xunit.runner.schema.json](/schema/v1/xunit.runner.schema.json)
