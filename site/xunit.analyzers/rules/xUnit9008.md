---
analyzer: true
title: xUnit9008
description: Cultured test methods must have at least one culture
severity: Error
v2: false
v3: true
---

## Cause

A violation of this rule occurs when a cultured test declares no cultures.

## Reason for rule

Cultured test methods run `n` tests, one for each declared culture. If the are no declared cultures, then the test method results in `0` tests.

## How to fix violations

To fix a violation of this rule, add at least one culture.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit9008
{
    [CulturedFact([])]
    public void FactMethod()
    { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit9008
{
    [CulturedFact(["en-US", "fr-FR"])]
    public void FactMethod()
    { }
}
```
