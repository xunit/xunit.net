---
title: xUnit.net XML Format v1
---

# xUnit.net v1 XML Format

> [!NOTE]
> This documentation for the v1 XML format is provided for historical purposes only. New transformations should use the [XML Format v2](format-xml-v2), which is the XML format used for transformations by xUnit.net v2 and v3.

xUnit.net v1 exposed XML in several ways:

* As the native xUnit.net XML output;
* As the XML document used for output transformations;
* As the callback from `Executor` for version-resilient test runner authors.

The top level element of the document during transformation is the [assembly](#assembly) element, and represents a single test assembly. During execution, the top level element will be the element currently being reported on.

## `<assembly>`{ #assembly }

The `assembly` node contains information about the run of a test assembly. Transformations are handed a single assembly node as their XML document to transform against.

> | Attribute     | Value
> | ------------- | -----
> | `environment` | The environment the tests were run in (32- vs. 64-bit and .NET version)
> | `failed`      | The number of tests that failed
> | `name`        | The fully qualified pathname of the assembly
> | `passed`      | The number of tests that passed
> | `run-date`    | The date the test run began
> | `run-time`    | The time the test run began
> | `skipped`     | The number of tests that were skipped
> | `time`        | The time, in fractional seconds, spent running tests
> | `total`       | The number of tests run

> | Child               | Cardinality | Purpose
> | ------------------- | ----------- | -------
> | [`<class>`](#class) | 0..*        | The classes run within the assembly

## `<class>`{ #class }

The `class` node contains information about the tests run in a single test class. It also contains information about failures related to the class itself (i.e., during the creation or disposable of fixture data associated with `IUseFixture<T>`).

> | Attribute | Value
> | --------- | -----
> | `failed`  | The number of tests that failed
> | `name`    | The full type name of the class
> | `passed`  | The number of tests that passed
> | `skipped` | The number of tests that were skipped
> | `time`    | The time, in fractional seconds, spent running tests
> | `total`   | The number of tests run from the class

> | Child                   | Cardinality | Purpose
> | ----------------------- | ----------- | -------
> | [`<failure>`](#failure) | 0..1        | Contains fixture related failure info
> | [`<test>`](#test)       | 0..*        | Contains the tests run within this class

## `<failure>`{ #failure }

A `failure` node describes a failure of a [`class`](#class) or [`test`](#test).

> | Attribute        | Value
> | ---------------- | -----
> | `exception-type` | The full type name of the exception that was thrown

> | Child                   | Cardinality | Purpose
> | ----------------------- | ----------- | -------
> | `<message>`             | 1           | The exception message
> | `<stack-trace>`         | 0..1        | The stack trace of the exception

## `<reason>`{ #reason }

A reason node contains information about why a test was skipped.

> | Child       | Cardinality | Purpose
> | ----------- | ----------- | -------
> | `<message>` | 1           | The reason the test was skipped

## `<start>`{ #start }

The start node is only available via Executor, not for Transformations. It indicates that a test is about to start running, and can be used to update runner status to indicate such.

> | Attribute | Value
> | --------- | -----
> | `method`  | The name of the method
> | `name`    | The display name of the test
> | `type`    | The full type name of the class

## `<test>`{ #test }

The test node contains information about a single test execution.

> | Attribute | Value
> | --------- | -----
> | `method`  | The name of the method
> | `name`    | The display name of the test
> | `result`  | One of `Pass`, `Fail`, or `Skip`
> | `time`    | The time, in fractional seconds, spent running the test (not present for `Skip` results)
> | `type`    | The full type name of the class

> | Child                   | Cardinality | Purpose
> | ----------------------- | ----------- | -------
> | [`<failure>`](#failure) | 0..1        | Present if the test result is `Fail`
> | `<output>`              | 0..1        | Contains any text was written to `Console.Out` or `Console.Error`
> | [`<reason>`](#reason)   | 0..1        | Present if the test result is `Skip`
> | [`<traits>`](#traits)   | 0..1        | Present if the test has any trait metadata

## `<trait>`{ #trait }

The trait node contains information about a single name/value pair of metadata about a [`test`](#test).

> | Attribute | Value
> | --------- | -----
> | `name`    | The name of the trait
> | `value`   | The value of the trait

## `<traits>`{ #traits }

The traits node contains zero or more [`trait`](#trait) nodes which contain the metadata about a [`test`](#test).

> | Child               | Cardinality | Purpose
> | ------------------- | ----------- | -------
> | [`<trait>`](#trait) | 0..*        | Nodes representing the traits of the [test](#test)
