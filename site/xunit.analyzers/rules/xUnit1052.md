---
analyzer: true
title: xUnit1052
description: Avoid using 'TheoryData<...>' with types that implement 'ITheoryDataRow'
category: Usage
severity: Warning
v2: false
v3: true
---

## Cause

A violation of this rule occurs when creating a data source from `TheoryData<...>` which includes `ITheoryDataRow` (or any type which implements it).

## Reason for rule

`ITheoryDataRule` (and derived types, like `TheoryDataRule<...>`) are intended to be used directly from a collection, like `IEnumerable<>` or `List<>`.

## How to fix violations

To fix a violation of this rule, replace `TheoryData` with `IEnumeable` (or a standard generic collection).

## Examples

### Violates

```csharp
using Xunit;

public class xUnit1052
{
    public static TheoryData<TheoryDataRow<int>> Data => [/*...*/];
}
```

### Does not violate

```csharp
using System.Collections.Generic;
using Xunit;

public class xUnit1052
{
    public static IEnumerable<TheoryDataRow<int>> Data => [/*...*/];
}
```
