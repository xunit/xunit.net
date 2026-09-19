---
analyzer: true
title: xUnit1071
description: Cultured test methods should not have duplicate cultures
severity: Warning
v2: false
v3: true
aot: true
---

## Cause

A violation of this rule occurs when the same culture is declared more than once on a cultured test. Culture names are compared case-insensitively, so `"en-US"` and `"en-us"` count as duplicates.

## Reason for rule

Cultured test methods run one test for each declared culture. A duplicated culture runs the same test more than once under the same culture, which adds no coverage and produces test cases with the same display name and unique ID.

## How to fix violations

To fix a violation of this rule, remove the duplicated culture.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit1071
{
    [CulturedFact(["en-US", "fr-FR", "en-US"])]
    public void FactMethod()
    { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit1071
{
    [CulturedFact(["en-US", "fr-FR"])]
    public void FactMethod()
    { }
}
```
