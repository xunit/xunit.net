---
analyzer: true
title: xUnit9006
description: Tests cannot set both SkipUnless and SkipWhen
severity: Error
v2: false
v3: false
aot: true
---

## Cause

A violation of this rule occurs when the user sets both the `SkipUnless` and `SkipWhen` properties for conditional skipping.

## Reason for rule

It is only legal to set one or the other property; it is not legal to set both properties.

## How to fix violations

To fix a violation of this rule, remove on the declarations.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit9006
{
    public static bool AlwaysTrue => true;

    [Fact(Skip = "Don't run", SkipWhen = nameof(AlwaysTrue), SkipUnless = nameof(AlwaysTrue))]
    public void Fact()
    { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit9006
{
    public static bool AlwaysTrue => true;

    [Fact(Skip = "Don't run", SkipWhen = nameof(AlwaysTrue))]
    public void Fact()
    { }
}
```
