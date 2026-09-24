---
title: Extensibility Overview
title-version: 2026 September 24
---

xUnit.net v3 can be extended at many levels, from adding your own assertions to writing entirely new kinds of tests. This page collects the starting points for the most common extensibility scenarios, with links to detailed documentation and runnable samples.

## Writing custom assertions

The most common extensibility scenario is adding your own assertion logic. There are two approaches, both demonstrated in the [AssertExtensions sample](https://github.com/xunit/samples.xunit/tree/main/v3/AssertExtensions).

The first approach is writing extension methods that wrap the built-in assertions. The [ExtensionMethods sample](https://github.com/xunit/samples.xunit/tree/main/v3/AssertExtensions/ExtensionMethods) adds `ShouldBeTrue()` and `ShouldBeFalse()` extension methods on `bool` which call `Assert.True` and `Assert.False`, so that tests can read `value.ShouldBeTrue()` instead of `Assert.True(value)`.

The second approach is available when you reference the assertion library as source (the `xunit.v3.assert.source` package) rather than as a compiled library (the `xunit.v3.assert` package). When imported as source, the `Assert` class is `partial`, so you can add new assertion methods directly to it. The [ExtendingAssert sample](https://github.com/xunit/samples.xunit/tree/main/v3/AssertExtensions/ExtendingAssert) uses this to add a new `Assert.IsBrad(person)` assertion.

The [AssertExamples sample](https://github.com/xunit/samples.xunit/tree/main/v3/AssertExamples) also shows how to get more out of the built-in assertions, including passing custom `IEqualityComparer<T>` implementations to `Assert.Equal`.

## Custom traits

Traits let you attach name/value pairs to tests, which runners can filter and group on. To create your own trait attributes, implement `ITraitAttribute`. The [TraitExtensibility sample](https://github.com/xunit/samples.xunit/tree/main/v3/TraitExtensibilityExample) shows a `CategoryAttribute` which applies one or more traits to a test method, class, or assembly.

## Custom test frameworks

For deeper extensibility, you can customize how tests are discovered and executed by writing your own test framework components: custom fact attributes, test case discoverers, test cases, and test case runners. The [RetryFact sample](https://github.com/xunit/samples.xunit/tree/main/v3/RetryFactExample) illustrates all of these pieces with a `[RetryFact]` attribute that re-runs failing tests.

New extension projects can start from the extension project template (`dotnet new xunit3-extension`; see [Getting Started](/docs/getting-started/v3/getting-started) for instructions on installing the templates). Framework extensions reference the `xunit.v3.extensibility.core` package.

## Related documentation

* [Custom Theory Data Serialization](/docs/getting-started/v3/custom-serialization)
* [Writing a Custom Runner Reporter](/docs/getting-started/v3/custom-runner-reporter)
* [Custom Test Class Construction](/docs/getting-started/v3/custom-test-class-construction)
* [Migrating Extensibility from v2 to v3](/docs/getting-started/v3/migration-extensibility)

## Samples and API reference

* [v3 sample projects](https://github.com/xunit/samples.xunit/tree/main/v3)
* [Core Framework v3 API documentation](https://api.xunit.net/v3/)
