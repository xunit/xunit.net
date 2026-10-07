---
title: Theory Data Attributes in xUnit.net v3
title-version: 2026 October 6
---

In this document, we will discuss the three built-in data source attributes (including their trade-offs), how to include metadata along with the data, and discuss some helpers that make returning type-safe data easier.

## Built-in Attributes

### InlineData

The first and most commonly used data source attribute is `[InlineData]`. This allows the developer to provide their data inline in the attribute, hence the name. Let's start with the [Getting Started](/docs/getting-started/v3/getting-started) example:

```csharp
[Theory]
[InlineData(3)]
[InlineData(5)]
[InlineData(6)]
public void MyFirstTheory(int value)
{
    Assert.True(IsOdd(value));
}

bool IsOdd(int value)
{
    return value % 2 == 1;
}
```

Here we have a theory with a single parameter (`int value`), and three data rows (providing values `3`, `5`, and `6` respectively). This will result in the test being run three times, once with each data row.

You can add additional parameters and arguments:

```csharp
[Theory]
[InlineData(3, true)]
[InlineData(5, true)]
[InlineData(6, false)]
public void MyFirstTheory(int value, bool expectedResult)
{
    var actualResult = IsOdd(value);

    Assert.Equal(expectedResult, actualResult);
}
```

You can provide _metadata about the data row_ via properties on `InlineData`, similar to the way you can provide _metadata about the test method_ via the `[Fact]` and `[Theory]` attribute. This includes:

* Disabling parallelization (e.g., `DisableParallelization = true`)
* Marking the data row as explicit (e.g., `Explicit = true`)
* Giving the data row a label (e.g., `Label = "My Label Value"`)
* Skipping the data row (e.g., `Skip = "My Skip Reason"`)
* Skipping conditionally at run time (via `SkipWhen` or `SkipUnless`, optionally with `SkipType`)
* Giving the data row a custom display name (e.g., `TestDisplayName = "My display name"`)
* Setting a timeout for the data row (e.g., `Timeout = 10_000` for 10 second timeout)
* Adding custom traits for the data row (e.g., `Traits = ["key1", "value1", "key2", "value2"]`)

### MemberData

While `InlineData` is convenient for simple data, it has two significant downsides:

* Attributes can accept a very limited number of data types as arguments. Returning data that isn't constant isn't possible, and some data types that seem constant aren't supported (like `decimal`).

* If you want to use the same set of data for multiple tests, you'd need to duplicate the `InlineData` per test (and remember to keep them in sync when adding/removing/updating data).

The solution to this is typically `MemberData`. To use this, you create a public static member (property, method, or field) which returns the data rows, and then reference that member with `[MemberData]`. Let's convert our second sample above to member data:

```csharp
public static IEnumerable<TheoryDataRow<int, bool>> DataSource =>
[
    new(3, true),
    new(5, true),
    new(6, false),
];

[Theory]
[MemberData(nameof(DataSource))]
public void MyFirstTheory(int value, bool expectedResult)
{
    var actualResult = IsOdd(value);

    Assert.Equal(expectedResult, actualResult);
}
```

We've moved the data into a public static property that is returning a collection of `TheoryDataRow<int, bool>` (a helper type that allows us to ensure we're correctly matching the the parameter types of the test method, which integrates well with the analyzers that ship with xUnit.net to provide additional compile-time validation). Each row of data is returning an instance of `TheoryDataRow<int, bool>` (with the target-typed `new(...)` syntax introduced in C# 9, which is the equivalent of writing `new TheoryDataRow<int, bool>(...)`.

In addition to providing good compile-time type safety, using `TheoryDataRow` also allows you to set per-data row metadata via property setters. For example, to skip a data row, you could write something like:

```csharp
new(6, false) { Skip = "This is a flaky data row, figure out what's wrong later" },
```

The same metadata properties that are available on `InlineData` are also available via `TheoryDataRow`.

> [!NOTE]
> As of the writing on this article, there are generic versions of `TheoryDataRow` available for between 1 and 15 values.

#### Moving the member to another type

You can reuse member data across multiple test classes, in addition to reusing it across multiple test methods in the same test class. The `MemberData` attribute supports the `MemberType` property to point to the type that contains the member data.

Here is the sample now converted to have the data live in another class:

```csharp
public class DataSources
{
    public static IEnumerable<TheoryDataRow<int, bool>> OddValues =>
    [
        new(3, true),
        new(5, true),
        new(6, false),
    ];
}

public class UnitTest
{
    [Theory]
    [MemberData(nameof(DataSources.OddValues), MemberType = typeof(DataSources))]
    public void MyFirstTheory(int value, bool expectedResult)
    {
        var actualResult = IsOdd(value);

        Assert.Equal(expectedResult, actualResult);
    }
}
```

### ClassData

Like `MemberData`, developers can use `ClassData` as a data source. Rather the implementing a member, you can provide the data via a class implementing the collection.

Converting the example to `ClassData` might look something like this:

```csharp
public class DataSource : IEnumerable<TheoryDataRow<int, bool>>
{
    public IEnumerator<TheoryDataRow<int, bool>> GetEnumerator()
    {
        yield return new(3, true);
        yield return new(5, true);
        yield return new(6, false);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class UnitTest
{
    [Theory]
    [ClassData(typeof(DataSource))]
    public void MyFirstTheory(int value, bool expectedResult)
    {
        var actualResult = IsOdd(value);

        Assert.Equal(expectedResult, actualResult);
    }
}
```

This a more complex and less convenient alternative to `MemberData`, so we expect this to be chosen less often.

## Alternative return types

While the examples above use `IEnumerable<TheoryDataRow<...>>`, there are some available alternatives that we consider to be less desirable. The base requirement for data sources is that the collection must yield items that are compatible with one of three base types:

* `IEnumerable<object?[]>`

  If you're coming from v2, this was the only signature that was previously supported. You may also be familiar with `TheoryData<...>` which was our compile time type safety helper in v2. Both `IEnumerable<object?[]>` and `TheoryData<...>` should be considered to be deprecated and only supported for backward compatibility reasons.

* `IEnumerable<ITheoryDataRow>`

  This is new in v3, and is the preferred signature. In addition, using `TheoryDataRow<...>` provides compile-time type safety and integrates well with xUnit.net's analyzers.

* `IEnumerable<ITuple>` (including inline tuple values; e.g., `IEnumerable<(int, bool)>`)

  This is also new in v3, and is offered as a simplifying alternative to `object?[]`. However, it still has the disadvantage of not being able to provide metadata along with the data row, so we don't generally recommend it.

We _**strongly encourage**_ that all new data sources should be written using `IEnumerable<TheoryDataRow<...>>` for maximum type safety, support for per-row metadata, and analyzer support.

## Disabling pre-enumeration

The system attempts to pre-enumerate the data when discovering tests. This has the advantage in IDEs like Visual Studio where you can see and run each individual data row.

Since data sources are customizable by developers, you may not want to pre-enumerate data when discovering tests. Reasons for this might include that the data set is large, that the data set is expensive to compute/retrieve (for example, reading data from a remote database server), etc. You can disable pre-enumeration with `[Theory(DisableDiscoveryEnumeration = true)]`.

> [!NOTE]
> For VSTest compatibility, pre-enumeration has limitations around serializing data. Any data which is provided by a data row that's not serializable will cause the system to revert back to non-pre-enumerated behavior (in which case in IDEs like Visual Studio, you will only see the test method, and not the individual data rows).
>
> For more information about serialization for data rows, see [Custom Theory Data Serialization](/docs/getting-started/v3/custom-serialization).

## Extensibility

### ITheoryDataRow

Types which represent data rows can implement `ITheoryDataRow`. There are two different variants for reflection-mode and Native AOT-mode, but they essentially represent the things that you can do with `TheoryDataRow<...>` today: provide a data row that includes metadata about the data row itself, in addition to the data for the data row.

Implementers might include anybody who wants to provide a source of data rows where `TheoryDataRow` or `TheoryDataRow<...>` is not sufficient or appropriate.

### DataAttribute

There is a base class that all data attributes derive from: `DataAttribute`. For reflection-mode, there is also an interface that can be implemented: `IDataAttribute`.

Implementers might include anybody who wants to provide data from a source that isn't easy to express as a static member.

Samples:

* [SqlDataExample](https://github.com/xunit/samples.xunit/tree/main/v3/SqlDataExample) is a reflection-mode data attribute that uses [`System.Data.SqlClient`](https://learn.microsoft.com/dotnet/api/system.data.sqlclient) to pull theory data rows from a SQL table
* [AotCsvDataSource](https://github.com/xunit/samples.xunit/tree/main/v3/AotCsvDataSource) is a Native AOT-mode data attribute that uses [Sep](https://github.com/nietras/Sep/) to pull theory data rows from a CSV file
