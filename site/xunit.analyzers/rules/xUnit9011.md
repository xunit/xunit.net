---
analyzer: true
title: xUnit9011
description: Theory parameter cannot use params modifier
severity: Error
v2: false
v3: false
aot: true
---

## Cause

A violation of this rule occurs when a theory parameter uses the `params` modifier.

## Reason for rule

When using the reflection-based version of xUnit.net, it will attempt to examine your data values at runtime and dynamically construct the `params` array on your behalf. This requires reflection features that are not available in Native AOT.

## How to fix violations

To fix a violation of this rule, remove the `params` modifier on the parameter, and update your theory data to create the array yourself.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit9011
{
    [Theory]
    [InlineData("Hello world", 42)]
    [InlineData("Hello world", 2112, 2600)]
    public void TheoryMethod(string greeting, params int[] values)
    { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit9011
{
    [Theory]
    [InlineData("Hello world", new[] { 42 })]
    [InlineData("Hello world", new[] { 2112, 2600 })]
    public void TheoryMethod(string greeting, int[] values)
    { }
}
```
