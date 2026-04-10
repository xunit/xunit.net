---
analyzer: true
title: xUnit1054
description: Properties used for conditional skipping must be public, static, and return bool
severity: Error
v2: false
v3: true
aot: true
---

## Cause

A violation of this rule occurs when a public static property is required for `SkipUnless` or `SkipWhen` but one was not found (or it does not return `bool` as required).

## Reason for rule

There are a few ways you can violate this rule:

- The named property does not exist
- The named property is not static
- The named property is not public
- The named property does not return `bool`

This rules applies everywhere you see `SkipUnless` and `SkipWhen`; that is, `[Fact]` and `[Theory]` (and friends) as well as data attributes like `[MemberData]`.

## How to fix violations

To fix a violation of this rule, ensure the named property exists, is public, is static, and returns `bool`.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit1054
{
    protected string AlwaysTrue => "yes";

    [Fact(Skip = "Conditionally Skipped", SkipWhen = nameof(AlwaysTrue))]
    public void NonPublic_Fact() { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit1054
{
    public static bool AlwaysTrue => true;

    [Fact(Skip = "Conditionally Skipped", SkipWhen = nameof(AlwaysTrue))]
    public void NonPublic_Fact() { }
}
```
