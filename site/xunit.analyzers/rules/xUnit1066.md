---
analyzer: true
title: xUnit1066
description: MemberData parameter cannot use params modifier in Native AOT
severity: Error
v2: false
v3: false
aot: true
---

## Cause

A violation of this rule occurs when a parameter of a `[MemberData]` method uses the `params` modifier in Native AOT.

## Reason for rule

When using the reflection-based version of xUnit.net, it will attempt to examine your `[MemberData]` arguments values at runtime and dynamically construct the `params` array on your behalf. This requires reflection features that are not available in Native AOT.

## How to fix violations

To fix a violation of this rule, remove the `params` modifier on the parameter, and update the `[MemberData]` to create the array yourself.

## Examples

### Violates

```csharp
using System.Linq;
using Xunit;

public class xUnit1066
{
    public static TheoryData<int> DataSource(params int[] multipliers) =>
        [multipliers.Aggregate(42, (left, right) => left * right)];

    [Theory]
    [MemberData(nameof(DataSource), 2)]
    public void TestMethod(int _)
    { }
}
```

### Does not violate

```csharp
using System.Linq;
using Xunit;

public class xUnit1066
{
    public static TheoryData<int> DataSource(int[] multipliers) =>
        [multipliers.Aggregate(42, (left, right) => left * right)];

    [Theory]
    [MemberData(nameof(DataSource), new[] { 2 })]
    public void TestMethod(int _)
    { }
}
```
