---
title: xUnit1025
description: InlineData should be unique within the Theory it belongs to
category: Usage
severity: Warning
v2: true
v3: true
---

## Cause

Test data provided with `InlineDataAttribute` is duplicated in other `InlineDataAttribute` occurence(s).

## Reason for rule

Having test data duplicated leads to duplication of test ID which may result in incorrect or unexpected behavior.
This usually comes from:

* typos
* test data copying and not updating
* not taking into account default values or `params` defined parameters

## How to fix violations

Remove duplicated `InlineDataAttribute` occurrences.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit1025
{
    [Theory]
    [InlineData(2)]
    [InlineData(2)]
    public void TestMethod(int x)
    { }
}
```

```csharp
using Xunit;

public class xUnit1025
{
    [Theory]
    [InlineData(2)]
    [InlineData(2, 0)]
    public void TestMethod(int x, int y = 0)
    { }
}
```

```csharp
using Xunit;

public class xUnit1025
{
    [Theory]
    [InlineData(1, 2, 3)]
    [InlineData(new object[] { 1, 2, 3 })]
    public void TestMethod(params int[] args)
    { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit1025
{
    [Theory]
    [InlineData(2)]
    public void TestMethod(int x)
    { }
}
```

```csharp
using Xunit;

public class xUnit1025
{
    [Theory]
    [InlineData(2)]
    public void TestMethod(int x, int y = 0)
    { }
}
```

```csharp
using Xunit;

public class xUnit1025
{
    [Theory]
    [InlineData(1, 2, 3)]
    public void TestMethod(params int[] args)
    { }
}
```
