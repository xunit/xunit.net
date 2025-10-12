---
analyzer: true
title: xUnit1053
description: The static member used as theory data must be statically initialized
category: Usage
severity: Warning
v2: true
v3: true
---

## Cause

A violation of this rule occurs when a member referenced by `[MemberData]` is not statically initialized.

## Reason for rule

Data provided by a member data source must be statically initialized so that it can be used to provide data for the tests that reference it with `[MemberData]`. Failing to initialize the data source will result in no data rows.

## How to fix violations

To fix a violation of this rule, either initialize the value inline or via a static constructor.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit1053
{
    public static TheoryData<int> DataSource;

    [Theory]
    [MemberData(nameof(DataSource))]
    public void TestMethod(int x) =>
        Assert.NotEqual(0, x);
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit1053
{
    public static TheoryData<int> DataSource = [4, 9, 16];

    [Theory]
    [MemberData(nameof(DataSource))]
    public void TestMethod(int x) =>
        Assert.NotEqual(0, x);
}
```

```csharp
using Xunit;

public class xUnit1053
{
    public static TheoryData<int> DataSource;

    static xUnit1053() =>
        DataSource = [4, 9, 16];

    [Theory]
    [MemberData(nameof(DataSource))]
    public void TestMethod(int x) =>
        Assert.NotEqual(0, x);
}
```
