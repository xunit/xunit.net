---
analyzer: true
title: xUnit1059
description: Test classes may not be decorated with ICollectionFixture<>
severity: Warning
v2: true
v3: true
aot: true
---

## Cause

A violation of this rule occurs when a test class is decorated with `IConnectionFixture<>`.

## Reason for rule

Collection fixtures may only be declared on collection definition classes.

The two most common scenarios for this to occur are:

- The developer mistakenly meant to use `IClassFixture<>`
- The developer is attempting to use the test class as its own collection definition

Collection fixtures decorated on test classes will be ignored.

## How to fix violations

To fix a violation of this rule, either replace `ICollectionFixture<>` with `IClassFixture<>`, or separate the collection definition into its own class.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit1059 : ICollectionFixture<object>
{
    [Fact]
    public void TestMethod()
    { }
}
```

```csharp
using Xunit;

[CollectionDefinition]
[Collection(typeof(xUnit1059))]
public class xUnit1059 : ICollectionFixture<object>
{
    [Fact]
    public void TestMethod()
    { }
}
```


### Does not violate

```csharp
using Xunit;

public class xUnit1059 : IClassFixture<object>
{
    [Fact]
    public void TestMethod()
    { }
}
```

```csharp
using Xunit;

[CollectionDefinition]
public class xUnit1059Collection : ICollectionFixture<object>
{ }

[Collection(typeof(xUnit1059Collection))]
public class xUnit1059
{
    [Fact]
    public void TestMethod()
    { }
}
```
