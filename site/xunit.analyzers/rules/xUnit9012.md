---
analyzer: true
title: xUnit9012
description: MemberData member may not be overloaded
severity: Error
v2: false
v3: true
---

## Cause

A violation of this rule occurs when `[MemberData]` points at an ambiguous member.

## Reason for rule

The source generator must determine unambiguously which member a `[MemberData]` is referring to. When the named member is overloaded, it cannot determine this.

## How to fix violations

To fix a violation of this rule, either remove the additional overloads or given them a different name.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit9012
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

public class xUnit9012
{
    public static TheoryData<int> DataSource(int multiplier = 1) => [42 * multiplier];

    [Theory]
    [MemberData(nameof(DataSource))]
    [MemberData(nameof(DataSource), 4)]
    public void TestMethod(int _)
    { }
}
```
