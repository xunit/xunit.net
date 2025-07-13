---
title: Config with testconfig.json (Microsoft Testing Platform)
---

# Config with `testconfig.json`

Beginning with xUnit.net v3 version `3.0.0-pre.15`, when running tests in Microsoft Testing Platform mode, you can utilize [`testconfig.json`](https://learn.microsoft.com/dotnet/core/testing/microsoft-testing-platform-config#testconfigjson) to provide test project configuration.

> [!NOTE]
> Using `testconfig.json` is only supported when running tests in Microsoft Testing Platform mode. Running tests any other way (including using our first party runners or any non-Microsoft Testing Plateform third party runner) does not support `testconfig.json`, and you should rely on [xUnit.net's native JSON configuration files](/docs/config-xunit-runner-json) instead. For more information about v3 and Microsoft Testing Platform, see [our documentation page](/docs/getting-started/v3/microsoft-testing-platform).

## Format of the `testconfig.json` file

The `testconfig.json` is a standard JSON file that lives in the root of your test project. xUnit.net configuration items are placed into a top-level object named `xUnit`. For example, to set the runtime culture and disable parallelization:

```json
{
  "xUnit": {
    "culture": "en-GB",
    "parallelizeTestCollections": false
  }
}
```

## Supported configuration items

### `assertEquivalentMaxDepth`{ #assertEquivalentMaxDepth }

Set this value to limit the recursive depth `Assert.Equivalent` will use when comparing objects for equivalence.

_This can also be set by environment variable `XUNIT_ASSERT_EQUIVALENT_MAX_DEPTH`. A value in the configuration file will take precedence over the environment variable._

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
> | JSON schema type | 32-bit integer
> | Minimum value    | `1`
> | Default value    | `50`

### `culture`{ #culture }

Set this value to override the default culture used to run all unit tests in the assembly. You can pass `"default"` to use the default behavior (use the default culture of the operating system); you can pass `"invariant"` to use the invariant culture; or you can pass any string which describes a valid culture on the target operating system (see [BCP 47](https://www.rfc-editor.org/info/bcp47) for a description of how culture names are formatted; note that the list of supported cultures will vary by target operating system).

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
> | JSON schema type | String
> | Default value    | `"default"`

### `diagnosticMessages`{ #diagnosticMessages }

Set this value to `true` to include diagnostic information during test discovery and execution. Each runner has a unique method of presenting diagnostic messages.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
> | JSON schema type | Boolean
> | Default value    | `false`

### `explicit`{ #explicit }

Change the way explicit tests are handled:

* `"on"` Run both explicit and non-explicit tests
* `"off"` Run only non-explicit tests
* `"only"` Run only explicit tests

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
> | JSON schema type | String
> | Valid values     | `"on"`, `"off"`, `"only"`
> | Default value    | `"off"`

### `failSkips`{ #failSkips }

Set this to `true` to enable skipped tests to be treated as errors.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
> | JSON schema type | Boolean
> | Default value    | `false`

### `failWarns`{ #failWarns }

Set this to `true` to enable warned tests to be treated as errors. By default, warnings will be reported with a passing test result.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
> | JSON schema type | Boolean
> | Default value    | `false`

### `internalDiagnosticMessages`{ #internalDiagnosticMessages }

Set this value to `true` to include internals diagnostic information during test discovery and execution. Each runner has a unique method of presenting diagnostic messages. Internal diagnostics often include information that is only useful when debugging the test framework itself.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
> | JSON schema type | Boolean
> | Default value    | `false`

### `longRunningTestSeconds`{ #longRunningTestSeconds }

Set this value to enable long-running (hung) test detection. When the runner is idle waiting for tests to finished, it will report that fact once the timeout has passed. Use a value of `0` to disable the feature, or a positive integer value to enable the feature (time in seconds).

**NOTE:** Long running test messages are diagnostic messages. You must enable diagnostic messages in order to see the long running test warnings.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
> | JSON schema type | 32-bit integer
> | Minimum value    | `0`
> | Default value    | `0` _(disabled)_

### `maxParallelThreads`{ #maxParallelThreads }

Set this to override the maximum number of threads to be used when parallelizing tests within this assembly. Use a value of `0` (or `"default"`) to indicate that you would like the default behavior; use a value of `-1` (or `"unlimited"`) to indicate that you do not wish to limit the number of threads used for parallelization; use a value with the multiplier syntax (i.e., values like `"0.5x"` or `"2x"`) to indicate that you wish you have a multiple of the number of CPU threads (so `2x` indicates you want the number of maximum parallel threads to be 2 * the number of CPU threads).

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
> | JSON schema type | 32-bit integer or string
> | Default value    | `"default"` (the number of CPU threads)

### `methodDisplay`{ #methodDisplay }

Set this to override the default display name for test cases. If you set this to `"method"`, the display name will be just the method (without the class name); if this set this value to `"classAndMethod"`, the default display name will be the fully qualified class name and method name.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
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
> | Test framework   | v3 3.0+
> | JSON schema type | String flags
> | Valid values     | `"all"`, `"none"`, or comma separated flags
> | Default value    | `"none"`

### `parallelAlgorithm`{ #parallelAlgorithm }

Set this to change the way tests are scheduled when they're running in parallel. For more information, see [Running Tests in Parallel](/docs/running-tests-in-parallel#algorithms). Note that the algorithm only applies when you have [enabled test collection parallelism](#parallelizeTestCollections), and are using a limited [number of threads](#maxParallelThreads) (i.e., not `unlimited` or `-1`).

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
> | JSON schema type | String
> | Valid values     | `"conservative"`, `"aggressive"`
> | Default value    | `"conservative"`

### `parallelizeTestCollections`{ #parallelizeTestCollections }

Set this to `true` if the assembly is willing to run tests inside this assembly in parallel against each other. Tests in the same test collection will be run sequentially against each other, but tests in different test collections will be run in parallel against each other. Set this to `false` to disable all parallelization within this test assembly.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
> | JSON schema type | Boolean
> | Default value    | `true`

### `preEnumerateTheories`{ #preEnumerateTheories }

Set this to `true` to pre-enumerate theories so that there is an individual test case for each theory data row. Set this to `false` to return a single test case for each theory without pre-enumerating the data ahead of time (this is how xUnit.net v1 used to behave). This is most useful for developers running tests inside Visual Studio, who wish to have the Code Lens test runner icons on their theory methods, since Code Lens does not support multiple tests from a single method.

This value does not have a default, because it's up to each individual test runner to decide what the best default behavior is. The Visual Studio adapter, for example, will default to `true`, while the console and MSBuild runners will default to `false`.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
> | JSON schema type | Boolean
> | Default value    | `true`

### `printMaxEnumerableLength`{ #printMaxEnumerableLength }

Set this value to limit the number of items to print in a collection (followed by an ellipsis when the collection is longer). This is also used when printing into the middle of a collection with a mismatch index, which means the printing may also start with an ellipsis.

Set this to `0` to always print the full collection.

_This can also be set by environment variable `XUNIT_PRINT_MAX_ENUMERABLE_LENGTH`. A value in the configuration file will take precedence over the environment variable._

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
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
> | Test framework   | v3 3.0+
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
> | Test framework   | v3 3.0+
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
> | Test framework   | v3 3.0+
> | JSON schema type | 32-bit integer
> | Minimum value    | `0`
> | Default value    | `50`

### `seed`{ #seed }

Set this to set the seed used for randomization (affects how the test cases are randomized). If the seed value isn't set, then the system will determine a reasonable seed (and print that seed when diagnostic messages are enabled, to assist you in reproducing order-dependent failures).

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
> | JSON schema type | 32-bit integer
> | Minimum value    | `0`
> | Default value    | unset

### `showLiveOutput`{ #showLiveOutput }

Set this to `true` to show messages from `ITestOutputHelper` live during the test run, in addition to showing them after the test has completed; set to `false` to only show test output messages after the test has completed.

> [!NOTE]
> When using `dotnet test` you may need to pass `--logger "console;verbosity=normal"` to be able to see messages from xUnit.net, as they may be hidden by default.

> { .table-compact }
> |                  |
> | ---------------- | -----
> | Test framework   | v3 3.0+
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
