---
analyzer: true
title: xUnit1056
description: Type must have a single public non-static constructor
severity: Error
v2: true
v3: true
aot: true
---

## Cause

A violation of this rule occurs when a type does not a single public constructor.

## Reason for rule

When this rule is violated, it's because the system either found 0 public constructors, or found more than one, that met the requirements (public and non-static).

This rule applies to test classes, as well as types used as collection and/or class fixtures.

## How to fix violations

To fix a violation of this rule, ensure there is only a single public, non-static constructor.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit1056 : IClassFixture<MyFixture>
{
    [Fact]
    public void TestMethod() { }
}

public class MyFixture
{
    public MyFixture() { }
    public MyFixture(int _) { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit1056 : IClassFixture<MyFixture>
{
    [Fact]
    public void TestMethod() { }
}

public class MyFixture
{
    public MyFixture(int _) { }
}
```
