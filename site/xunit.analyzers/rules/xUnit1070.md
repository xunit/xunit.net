---
analyzer: true
title: xUnit1070
description: Cultured test methods cannot have null cultures
severity: Error
v2: false
v3: true
aot: true
---

## Cause

A violation of this rule occurs when one of the cultures declared on a cultured test is `null`.

## Reason for rule

Cultured test methods run one test for each declared culture, so every culture must be a valid culture name. A `null` culture cannot be turned into a culture, so the test framework fails with an error when it discovers the test.

## How to fix violations

To fix a violation of this rule, replace the `null` value with a valid culture name, or remove it.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit1070
{
    [CulturedFact(["en-US", null])]
    public void FactMethod()
    { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit1070
{
    [CulturedFact(["en-US", "fr-FR"])]
    public void FactMethod()
    { }
}
```
