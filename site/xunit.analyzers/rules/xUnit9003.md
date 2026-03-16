---
analyzer: true
title: xUnit9003
description: Type must have a single public non-static constructor
severity: Error
v2: false
v3: false
aot: true
---

## Cause

A violation of this rule occurs when a type does not a single public constructor.

## Reason for rule

When this rule is violated, it's because the source generator either found 0 public constructors, or found more than one, that met the requirements (public, non-static, non-`[Obsolete]`).

This is most commonly shown for types used as fixtures.

## How to fix violations

To fix a violation of this rule, ensure there is only a single public, non-static, non-`[Obsolete]` constructor.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit9003 : IClassFixture<MyFixture>
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

public class xUnit9003 : IClassFixture<MyFixture>
{
    [Fact]
    public void TestMethod() { }
}

public class MyFixture
{
    public MyFixture(int _) { }
}
```
