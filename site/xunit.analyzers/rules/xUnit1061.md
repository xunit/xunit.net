---
analyzer: true
title: xUnit1061
description: Fact methods cannot be generic
severity: Error
v2: true
v3: true
aot: true
---

## Cause

A violation of this rule occurs when a test method decorated with `[Fact]` (or `[CulturedFact]` in v3) has been declared as generic.

## Reason for rule

Fact methods cannot be generic. They have no data over which to close the open-generic.

## How to fix violations

To fix a violation of this rule, remove the generic.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit1061
{
    [Fact]
    public void TestMethod<T>()
    { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit1061
{
    [Fact]
    public void TestMethod()
    { }
}
```
