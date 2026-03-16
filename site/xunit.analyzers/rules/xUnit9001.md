---
analyzer: true
title: xUnit9001
description: Type must implement appropriate interface
severity: Error
v2: false
v3: false
aot: true
---

## Cause

A violation of this rule occurs when a type is missing a required interface implementation.

## Reason for rule

In order for the type to be used by the system (and correctly consumed by the source generator), it must implement a specific interface.

## How to fix violations

Implement the required interface.

## Examples

### Violates

```csharp
using Xunit;
using Xunit.v3;

[assembly: CollectionBehavior(typeof(MyTestCollectionFactory))]

class MyTestCollectionFactory
{ }
```

```shell
error xUnit9001: Type 'MyTestCollectionFactory' must implement interface 'Xunit.v3.ICodeGenTestCollectionFactory'
```

### Does not violate

```csharp
using Xunit;
using Xunit.v3;

[assembly: CollectionBehavior(typeof(MyTestCollectionFactory))]

class MyTestCollectionFactory : ICodeGenTestCollectionFactory
{
    // ...implementation of ICodeGenTestCollectionFactory...
}
```
