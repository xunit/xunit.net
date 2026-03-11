---
analyzer: true
title: xUnit9013
description: MemberData type must be either public or internal
severity: Error
v2: false
v3: true
---

## Cause

A violation of this rule occurs when the type referred to by `[MemberData]` is in a type that's not visible for the generated code.

## Reason for rule

There are situations the user defined code can be compiled, but generated code needs access to a type which is not reachable due to its declared visibility. This most commonly occurs with references to private nested types.

## How to fix violations

To fix a violation of this rule, declare the type as either `public` or `internal`.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit9013
{
    class DataSourceClass
    {
        public static TheoryData<int> DataSource => [42];
    }

    [Theory]
    [MemberData(nameof(DataSourceClass.DataSource), MemberType = typeof(DataSourceClass))]
    public void TestMethod(int _)
    { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit9013
{
    internal class DataSourceClass
    {
        public static TheoryData<int> DataSource => [42];
    }

    [Theory]
    [MemberData(nameof(DataSourceClass.DataSource), MemberType = typeof(DataSourceClass))]
    public void TestMethod(int _)
    { }
}
```
