---
analyzer: true
title: xUnit9005
description: Generic collection definitions are not supported
severity: Error
v2: false
v3: false
aot: true
---

## Cause

A violation of this rule occurs when a collection definition class is declared as generic.

## Reason for rule

Collection definition classes are most often declared as generic in order to support open generic fixture types. However, open generics require runtime reflection features that are not available in Native AOT.

## How to fix violations

To fix a violation of this rule, remove the generic declaration on the collection definition (and use non-generic or closed-generic fixture types instead).

## Examples

### Violates

```csharp
using Xunit;

[CollectionDefinition]
public class xUnit9005<T> : ICollectionFixture<MyFixture<T>>
{ }
```

### Does not violate

```csharp
using Xunit;

[CollectionDefinition]
public class xUnit9005 : ICollectionFixture<MyFixture<int>>
{ }
```
