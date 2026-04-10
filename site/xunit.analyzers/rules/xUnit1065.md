---
analyzer: true
title: xUnit1065
description: MemberData method is ambiguous
severity: Error
v2: true
v3: true
aot: true
---

## Cause

A violation of this rule occurs when `[MemberData]` points at an ambiguous member.

## Reason for rule

In reflection mode, ambiguous methods cannot be resolved at runtime.

In Native AOT mode, overloaded methods cannot be resolved at build time.

## How to fix violations

To fix a violation of this rule:

* In both modes, removing the additional overloads or renaming them will resolve the issue.
* In reflection mode, you may additionally attempt to change the parameters to resolve the ambiguity.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit1065
{
    public static TheoryData<int> DataSource() => DataSource(1);
    public static TheoryData<int> DataSource(int multiplier) => [42 * multiplier];

    [Theory]
    [MemberData(nameof(DataSource))]
    [MemberData(nameof(DataSource), 4)]
    public void TestMethod(int _)
    { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit1065
{
    public static TheoryData<int> DataSource(int multiplier = 1) => [42 * multiplier];

    [Theory]
    [MemberData(nameof(DataSource))]
    [MemberData(nameof(DataSource), 4)]
    public void TestMethod(int _)
    { }
}
```
