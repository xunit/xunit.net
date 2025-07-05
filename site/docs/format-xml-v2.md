---
title: xUnit.net XML Format v2
---

# xUnit.net XML Format v2

Several runners—including the console and MSBuild runners—are capable of generating XML reports after tests have been run. Some of those runners also support running XSL-T transformations against that XML (some built-in examples include transformations to HTML and NUnit format). This page documents the format of the XML emitted from v2 (and later) runners.

The `schema-version` attribute on the `assemblies` element identifies the schema version of the document. Since this attribute was introduced in schema version 2, it should be assumed that the absence of this attribute implies schema version 1. It's also important to note that schema versions are unrelated to versions of xUnit.net and their runners; you should only make assumptions about supported features in the XML file based on `schema-version` and _**not**_ based on the version of xUnit.net.

The top level element of the document is the [assemblies](#assemblies) element.

> [!NOTE]
> Any child element that doesn't link to specific documentation is an element that contains text, rather than child elements and/or attributes. Older runners will place this text in a CDATA block; newer runners will just put the text directly into the element. XSL-T (and most parsers) won't differentiate between naked text vs. a CDATA block.

> [!NOTE]
> If you are looking for documentation on the deprecated XML format supported by xUnit.net v1, see the [xUnit.net XML Format v1](format-xml-v1) documentation page.

## `<assemblies>`{ #assemblies }

The `assemblies` element is the top-level element of the document.

> | Attribute        | Schema | Value
> | ---------------- | ------ | -----
> | `computer`       | 2+     | [Optional] The name of the computer that ran this set of assemblies.<br />_**Data type:** String_
> | `finish-rtf`     | 2+     | The time the last assembly finished running, in round-trippable format.<br />_**Data type:** String (in `yyyy-mm-ddThh:mm:ss.fffffff[timezone]` format)_
> | `id`             | 2+     | A unique ID for this collection of assemblies. This ID is regenerated every time the test assemblies are run.<br />_**Data type:** GUID_
> | `schema-version` | 2+     | The schema version of the document.<br />_**Data type:** Integer_
> | `start-rtf`      | 2+     | The time the first assembly started running, in round-trippable format.<br />_**Data type:** String (in `yyyy-mm-ddThh:mm:ss.fffffff[timezone]` format)_
> | `timestamp`      | 2+     | The human-readable timestamp of when the last assembly finished running.<br />_**Data type:** String_
> | `user`           | 2+     | [Optional] The name of the user that ran this set of assemblies, if known.<br />_**Data type:** String_

> | Child                     | Schema | Cardinality | Purpose
> | ------------------------- | ------ | ----------- | -------
> | [`<assembly>`](#assembly) | 1+     | 0..*        | One `assembly` element for each test assembly

## `<assembly>`{ #assembly }

The `assembly` element contains information about the run of a single test assembly. This includes environmental information.

> | Attribute          | Schema | Value
> | ------------------ | ------ | -----
> | `config-file`      | 1+     | [Optional] The fully qualified path name of the test assembly configuration file.<br />_**Data type:** String (file path)_
> | `environment`      | 1+     | The runtime environment in which the tests were run.<br />_**Data type:** String_
> | `errors`           | 1+     | The total number of environmental errors experienced in the assembly. This is separate from failing tests (for example, errors that occurred while cleaning up, between or after tests).<br />_**Data type:** Integer_
> | `failed`           | 1+     | The total number of test cases in the assembly which failed.<br />_**Data type:** Integer_
> | `finish-rtf`       | 2+     | The time the test assembly finished running, in round-trippable format.<br />_**Data type:** String (in `yyyy-mm-ddThh:mm:ss.fffffff[timezone]` format)_
> | `id`               | 2+     | A unique ID for this test assembly. This ID is regenerated every time the assembly is run.<br />_**Data type:** GUID_
> | `name`             | 1+     | The fully qualified path name of the test assembly.<br />_**Data type:** String (file path)_
> | `not-run`          | 2+     | The total number of test cases in the assembly that were not run (because they did not match the user's request to run or not run explicit tests).<br />_**Data type:** Integer_
> | `passed`           | 1+     | The total number of test cases in the assembly which passed.<br />_**Data type:** Integer_
> | `run-date`         | 1+     | The date when the test run started.<br />_**Data type:** String (in `yyyy-mm-dd` format)_
> | `run-time`         | 1+     | The time when the test run started.<br />_**Data type:** String (in 24-hour `hh:mm:ss` format)_
> | `skipped`          | 1+     | The total number of test cases in the assembly that were skipped.<br />_**Data type:** Integer_
> | `start-rtf`        | 2+     | The time the test assembly started running, in round-trippable format.<br />_**Data type:** String (in `yyyy-mm-ddThh:mm:ss.fffffff[timezone]` format)_
> | `target-framework` | 2+     | [Optional] The target framework that the unit tests were compiled against.<br />_**Data type:** String (in `<FrameworkName>,Version=<FrameworkVersion>` format)_
> | `test-framework`   | 1+     | The display name of the test framework that ran the tests.<br />_**Data type:** String_
> | `time`             | 1+     | The number of seconds the assembly run took.<br />_**Data type:** Decimal_
> | `time-rtf`         | 2+     | The time spent running tests in the assembly (in a round-trippable format).<br />_**Data type:** String (in `hh:mm:ss.fffffff` format)_
> | `total`            | 1+     | The total number of test cases run in the assembly.<br />_**Data type:** Integer_

> | Child                         | Schema | Cardinality | Purpose
> | ----------------------------- | ------ | ----------- | -------
> | [`<collection>`](#collection) | 1+     | 0..*        | One `collection` element for every test collection in the test assembly.
> | [`<errors>`](#errors)         | 1+     | 0..1        | Container for the environmental errors experienced in the test assembly.

## `<collection>`{ #collection }

The `collection` element contains information about the run of a single test collection.

> | Attribute          | Schema | Value
> | ------------------ | ------ | -----
> | `failed`           | 1+     | The total number of test cases in the test collection which failed.<br />_**Data type:** Integer_
> | `id`               | 2+     | A unique ID for this test collection. This ID is regenerated every time the collection is run.<br />_**Data type:** GUID_
> | `name`             | 1+     | The display name of the test collection.<br />_**Data type:** String_
> | `not-run`          | 2+     | The total number of test cases in the collection that were not run (because they did not match the user's request to run or not run explicit tests).<br />_**Data type:** Integer_
> | `passed`           | 1+     | The total number of test cases in the test collection which passed.<br />_**Data type:** Integer_
> | `skipped`          | 1+     | The total number of test cases in the test collection that were skipped.<br />_**Data type:** Integer_
> | `time`             | 1+     | The number of seconds the test collection run took.<br />_**Data type:** Decimal_
> | `time-rtf`         | 2+     | The time spent running tests in the collection (in a round-trippable format).<br />_**Data type:** String (in `hh:mm:ss.fffffff` format)_
> | `total`            | 1+     | The total number of test cases run in the test collection.<br />_**Data type:** Integer_

> | Child             | Schema | Cardinality | Purpose
> | ----------------- | ------ | ----------- | -------
> | [`<test>`](#test) | 1+     | 1..*        | One `test` element for every test in the test collection.

## `<error>`{ #error }

The `error` element contains information about an environment failure that happened outside the scope of running a single unit test (for example, an exception thrown while disposing of a fixture object).

> | Attribute | Schema | Value
> | ----------| ------ | -----
> | `name`    | 1+     | [Optional] The name of the item that caused the failure, if known. The value depends on the type of error being reported (for example, for `assembly-cleanup` the name will be the assembly name, when known).<br />_**Data type:** String_
> | `type`    | 1+     | A code which indicates what kind of failure it is.<br />_**Data type:** Enum (values: `"assembly-cleanup"`, `"fatal"`, `"test-case-cleanup"`, `"test-class-cleanup"`, `"test-cleanup"`, `"test-collection-cleanup"`, `"test-method-cleanup"`)_

> | Child                   | Schema | Cardinality | Purpose
> | ----------------------- | ------ | ----------- | -------
> | [`<failure>`](#failure) | 1+     | 1           | Contains information about the failure.

## `<errors>`{ #errors }

The `errors` element is a container for 0 or more [`error`](#error) elements.

> [!NOTE]
> In schema 2+, the `errors` element will never be empty; if there are no errors in the collection, then the `errors` element itself will not be present. Defensively supporting both schema 1 and schema 2+ means you should still plan for `errors` elements with no children.

> | Child               | Schema | Cardinality | Purpose
> | ------------------- | ------ | ----------- | -------
> | [`<error>`](#error) | 1+     | 0..*        | One `error` element for every environmental error that occurred.

## `<failure>`{ #failure }

The `failure` element contains information a test failure.

> | Attribute        | Schema | Value
> | ---------------- | ------ | -----
> | `exception-type` | 1+     | [Optional] The fully qualified type name of the exception that caused the failure.<br />_**Data type:** String_

> | Child           | Schema | Cardinality | Purpose
> | --------------- | ------ | ----------- | -------
> | `<message>`     | 1+     | 0..1        | The composite failure message.
> | `<stack-trace>` | 1+     | 0..1        | The composite stack trace.

## `<test>`{ #test }

The `test` element contains information about the run of a single test.

> | Attribute     | Schema | Value
> | ------------- | ------ | -----
> | `finish-rtf`  | 3+     | [Optional] The time the test finished running, in round-trippable format.<br />_**Data type:** String (in `yyyy-mm-ddThh:mm:ss.fffffff[timezone]` format)_
> | `id`          | 2+     | A unique ID for this test. This ID is regenerated every time the test is run.<br />_**Data type:** GUID_
> | `method`      | 1+     | [Optional] The name of the method that contained the test.<br />_**Data type:** String_
> | `name`        | 1+     | The display name of the test.<br />_**Data type:** String_
> | `result`      | 1+     | The result of the test. (Note that while this attribute is present in schema 1, the enum value of `"NotRun"` was not introduced until schema 2, and only applies to tests written in xUnit.net v3 or later, when the explicit feature was introduced.)<br />_**Data type:** Enum (values: `"Pass"`, `"Fail"`, `"Skip"`, and `"NotRun"`)._
> | `source-file` | 1+     | [Optional] The source file that this test belongs to, if known.<br />_**Data type:** String (file path)_
> | `source-line` | 1+     | [Optional] The source line that this test belongs to, if known.<br />_**Data type:** Integer_
> | `start-rtf`   | 3+     | [Optional] The time the test started running, in round-trippable format.<br />_**Data type:** String (in `yyyy-mm-ddThh:mm:ss.fffffff[timezone]` format)_
> | `time`        | 1+     | The number of seconds the test run took.<br />_**Data type:** Decimal_
> | `time-rtf`    | 2+     | The time spent running the test (in a round-trippable format).<br />_**Data type:** String (in `hh:mm:ss.fffffff` format)_
> | `type`        | 1+     | [Optional] The fully qualified type name of the class that contained the test.<br />_**Data type:** String_

> | Child                     | Schema | Cardinality | Purpose
> | ------------------------- | ------ | ----------- | -------
> | [`<failure>`](#failure)   | 1+     | 0..1        | For failing tests, contains information about the failure.
> | `<output>`                | 1+     | 0..1        | Any captured output.
> | `<reason>`                | 1+     | 0..1        | For a skipped test, contains the reason text.
> | [`<traits>`](#traits)     | 1+     | 0..1        | Container for 1 or more [`trait`](#trait) elements.
> | [`<warnings>`](#warnings) | 2+     | 0..1        | Container for 1 or more `warning` elements.

## `<trait>`{ #trait }

The `trait` element contains a single trait name/value pair.

> | Attribute | Schema | Value
> | --------- | ------ | -----
> | `name`    | 1+     | The name of the trait.<br />_**Data type:** String_
> | `value`   | 1+     | The value of the trait.<br />_**Data type:** String_

## `<traits>`{ #traits }

The `traits` element is a container for 1 or more [`trait`](#trait) elements.

> | Child               | Schema | Cardinality | Purpose
> | ------------------- | ------ | ----------- | -------
> | [`<trait>`](#trait) | 1+     | 1..*        | One `trait` element for every trait name/value pair associated with the test.

## `<warnings>`{ #warnings }

The `warnings` element is a container for 1 or more `warning` elements.

> | Child       | Schema | Cardinality | Purpose
> | ----------- | ------ | ----------- | -------
> | `<warning>` | 2+     | 1..*        | One `warning` element for each warning message in the test.
