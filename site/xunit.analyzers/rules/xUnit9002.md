---
analyzer: true
title: xUnit9002
description: Type must have public static property
severity: Error
v2: false
v3: true
---

## Cause

A violation of this rule occurs when a public static property is required for `SkipUnless` or `SkipWhen` but one was not found.

## Reason for rule

There are a few ways you can violate this rule:

- The named property does not exist
- The named property is not static
- The named property is not public
- The named property does not return `bool`

This rules applies everywhere you see `SkipUnless` and `SkipWhen`; that is, `[Fact]` and `[Theory]` (and friends) as well as data attributes like `[MemberData]`.

## How to fix violations

To fix a violation of this rule, ensure the named property exists and follows all the rules.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit9002
{
    static bool AlwaysTrue => true;

    [Fact(Skip = "Conditionally Skipped", SkipWhen = nameof(AlwaysTrue))]
    public void NonPublic_Fact() { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit9002
{
    public static bool AlwaysTrue => true;

    [Fact(Skip = "Conditionally Skipped", SkipWhen = nameof(AlwaysTrue))]
    public void NonPublic_Fact() { }
}
```
