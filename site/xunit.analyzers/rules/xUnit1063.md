---
analyzer: true
title: xUnit1063
description: Test class cannot be an open generic type
severity: Error
v2: true
v3: true
aot: true
---

## Cause

A violation of this rule occurs when a non-abstract test class is an open generic type.

## Reason for rule

Test classes cannot be open generic types, since the system does not know how to resolve the open type(s).

## How to fix violations

To fix a violation of this rule, one of two situations typically applies:

* If the test class is not meant to be a base class, then remove or close the generic type.
* If the test class is meant to be a base class, with test methods that operate on the generic type, then that class should be marked as abstract so the system will not try to create it directly.

## Examples

### Violates

```csharp
using Xunit;

public class OpenGenericTestClass<T>
{
    [Fact]
    public void TestMethod()
    { /* test code which uses type T */ }
}

public class MyTypeTestClass : OpenGenericTestClass<MyType>
{ }
```

### Does not violate

```csharp
using Xunit;

public abstract class OpenGenericTestClass<T>
{
    [Fact]
    public void TestMethod()
    { /* test code which uses type T */ }
}

public class MyTypeTestClass : OpenGenericTestClass<MyType>
{ }
```
