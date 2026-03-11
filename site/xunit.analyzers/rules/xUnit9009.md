---
analyzer: true
title: xUnit9009
description: Fact methods cannot be generic
severity: Error
v2: false
v3: true
---

## Cause

A violation of this rule occurs when a test method decorated with `[Fact]` has been declared as generic.

## Reason for rule

`[Fact]` methods cannot be generic. They have no data over which to close the open-generic.

## How to fix violations

To fix a violation of this rule, remove the generic.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit9009
{
    [Fact]
    public void TestMethod<T>()
    { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit9009
{
    [Fact]
    public void TestMethod()
    { }
}
```
