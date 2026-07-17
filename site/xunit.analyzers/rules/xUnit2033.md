---
analyzer: true
title: xUnit2033
description: Use the assertion return value instead of re-deriving it
severity: Warning
v2: true
v3: true
aot: true
---

## Cause

A violation of this rule occurs when the developer uses a narrowing or converting assertion (which returns the narrowed or converted value), but does not utilize the return value and instead re-writes the narrowing or conversion.

## Reason for rule

The assertion framework will already provide the narrowing or conversion operation for you. It's less efficient to throw the value away and redo the work yourself.

## How to fix violations

To fix a violation of this rule, utilize the return value of the assertion.

## Examples

### Violates

```csharp
using System.Linq;
using Xunit;

public class xUnit2033
{
    [Fact]
    public void TestMethod()
    {
        var collection = new[] { 1 };

        Assert.Single(collection);
        Assert.Equal(1, collection.Single());
    }
}
```

```csharp
using Xunit;

public class xUnit2033
{
    [Theory]
    [InlineData("Hello world")]
    public void TestMethod(object value)
    {
        Assert.IsType<string>(value);
        Assert.StartsWith("Hello", (string)value);
    }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit2033
{
    [Fact]
    public void TestMethod()
    {
        var collection = new[] { 1 };

        var value = Assert.Single(collection);
        Assert.Equal(1, value);
    }
}
```

```csharp
using Xunit;

public class xUnit2033
{
    [Theory]
    [InlineData("Hello world")]
    public void TestMethod(object value)
    {
        var stringValue = Assert.IsType<string>(value);
        Assert.StartsWith("Hello", stringValue);
    }
}
```
