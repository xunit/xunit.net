---
analyzer: true
title: xUnit1073
description: Collection definition is never used
severity: Warning
v2: true
v3: true
aot: true
---

## Cause

A violation of this rule occurs when a collection definition does not have any references from any tests.

## Reason for rule

This rule triggers most often in two situations:

* The collection definition name is misspelled.
* A previously used collection definition is no longer needed because tests that referenced it were deleted (or removed from the collection).

## How to fix violations

To fix a violation of this rule, either fix the spelling mistake or remove the unused collection definition.

## Examples

### Violates

```csharp
using Xunit;

[CollectionDefinition("MyCollection")]
public class xUnit1073Collection
{ }

[Collection("TheCollection")]
public class xUnit1073
{
    [Fact]
    public void TestMethod() { }
}

```

### Does not violate

```csharp
using Xunit;

[CollectionDefinition("MyCollection")]
public class xUnit1073Collection
{ }

[Collection("MyCollection")]
public class xUnit1073
{
    [Fact]
    public void TestMethod() { }
}
```

> [!NOTE]
> This is a post-compile analyzer, so it may not show up in real time (or highlighted in the source code). It will be reported when the test project is compiled.
