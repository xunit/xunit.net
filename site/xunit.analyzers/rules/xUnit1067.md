---
analyzer: true
title: xUnit1067
description: There is no matching MemberData method argument
severity: Error
v2: true
v3: true
aot: true
---

## Cause

A violation of this rule occurs when `[MemberData]` references a method, and not enough arguments are provided for the method.

## Reason for rule

You must provide arguments for all `[MemberData]` parameters.

## How to fix violations

To fix a violation of this rule, do one of:

- Remove parameters
- Make parameters optional
- Add arguments

## Examples

### Violates

```csharp
using Xunit;

public class xUnit1067
{
    public static TheoryData<int> DataSource(int multiplier) => [42 * multiplier];

    [Theory]
    [MemberData(nameof(DataSource))]
    public void TestMethod(int _)
    { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit1067
{
    public static TheoryData<int> DataSource() => [42];

    [Theory]
    [MemberData(nameof(DataSource))]
    public void TestMethod(int _)
    { }
}
```

```csharp
using Xunit;

public class xUnit1067
{
    public static TheoryData<int> DataSource(int multiplier = 1) => [42 * multiplier];

    [Theory]
    [MemberData(nameof(DataSource))]
    public void TestMethod(int _)
    { }
}
```

```csharp
using Xunit;

public class xUnit1067
{
    public static TheoryData<int> DataSource(int multiplier) => [42 * multiplier];

    [Theory]
    [MemberData(nameof(DataSource), 1)]
    public void TestMethod(int _)
    { }
}
```
