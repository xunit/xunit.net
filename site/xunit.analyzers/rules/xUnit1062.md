---
analyzer: true
title: xUnit1062
description: Theory methods cannot be generic in Native AOT
severity: Error
v2: false
v3: false
aot: true
---

## Cause

A violation of this rule occurs when a test method decorated with `[Theory]` is declared as generic in Native AOT.

## Reason for rule

When using the reflection-based version of xUnit.net, you can declare a theory method as generic, use the generic type for one of the arguments, and xUnit.net will attempt to resolve the correct generic type at runtime. However, open generics require runtime reflection features that are not available in Native AOT.

## How to fix violations

To fix a violation of this rule, remove the generic (and update any parameters that were declared with the generic type to some non-generic type that is compatible with all the data elements).

## Examples

### Violates

```csharp
using Xunit;

public class xUnit1062
{
    [Theory]
    [InlineData(42)]
    [InlineData(0L)]
    public void TheoryMethod<T>(T value)
    { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit1062
{
    [Theory]
    [InlineData(42)]
    [InlineData(0L)]
    public void TheoryMethod(long value)
    { }
}
```
