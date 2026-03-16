---
analyzer: true
title: xUnit9004
description: Type must be public or internal
severity: Error
v2: false
v3: false
aot: true
---

## Cause

A violation of this rule occurs when a type used by code generation is not visible to the generated code.

## Reason for rule

There are situations the user defined code can be compiled, but generated code needs access to a type which is not reachable due to its declared visibility. This most commonly occurs with references to private nested types.

## How to fix violations

To fix a violation of this rule, declare the type as either `public` or `internal`.

## Examples

### Violates

```csharp
using Xunit;
using Xunit.v3;

public class xUnit9004_BeforeAfter
{
    [Fact, MyBeforeAfter]
    public void TestMethod() { }

    class MyBeforeAfter : BeforeAfterTestAttribute { }
}
```

### Does not violate

```csharp
using Xunit;
using Xunit.v3;

public class xUnit9004_BeforeAfter
{
    [Fact, MyBeforeAfter]
    public void TestMethod() { }

    internal class MyBeforeAfter : BeforeAfterTestAttribute { }
}
```
