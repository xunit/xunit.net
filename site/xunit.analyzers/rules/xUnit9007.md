---
analyzer: true
title: xUnit9007
description: Test classes may not be decorated with ICollectionFixture<>
severity: Error
v2: false
v3: true
---

## Cause

A violation of this rule occurs when a test class is decorated with `IConnectionFixture<>`.

## Reason for rule

Collection fixtures may only be declared on collection definition classes.

The two most common scenarios for this to occur are:

- The developer mistakenly meant to use `IClassFixture<>`
- The developer is attempting to use the test class as its own collection definition

## How to fix violations

To fix a violation of this rule, either replace `ICollectionFixture<>` with `IClassFixture<>`, or separate the collection definition into its own class.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit9007 : ICollectionFixture<object>
{
    [Fact]
    public void TestMethod()
    { }
}
```

```csharp
using Xunit;

[CollectionDefinition]
[Collection(typeof(xUnit9007))]
public class xUnit9007 : ICollectionFixture<object>
{
    [Fact]
    public void TestMethod()
    { }
}
```


### Does not violate

```csharp
using Xunit;

public class xUnit9007 : IClassFixture<object>
{
    [Fact]
    public void TestMethod()
    { }
}
```

```csharp
using Xunit;

[CollectionDefinition]
public class xUnit9007Collection : ICollectionFixture<object>
{ }

[Collection(typeof(xUnit9007Collection))]
public class xUnit9007
{
    [Fact]
    public void TestMethod()
    { }
}
```
