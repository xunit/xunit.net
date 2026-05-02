---
analyzer: true
title: xUnit3006
description: Test case implementation must be serializable
severity: Error
v2: false
v3: true
aot: false
---

## Cause

A violation of this rule occurs when a test case implementation is known to not be serializable.

## Reason for rule

Test cases in v3 reflection-mode must be serializable, because their serializable contents are provided to VSTest so that they can later be run individually from Test Explorer.

## How to fix violations

To fix a violation of this rule, either implement `IXunitSerializable` on the test case, or support its serialization with a third party class that implements `IXunitSerializer`.

## Examples

### Violates

```csharp
using Xunit.v3;

public sealed class xUnit3006 : IXunitTestCase
{
    // ... implementation of IXunitTestCase ...
}
```

### Does not violate

```csharp
using Xunit.Sdk;
using Xunit.v3;

public sealed class xUnit3006 : IXunitTestCase, IXunitSerializable
{
    // ... implementation of IXunitTestCase ...

    // ... implementation of IXunitSerializable ...
}
```

```csharp
using Xunit.Sdk;
using Xunit.v3;

[assembly: RegisterXunitSerializer(typeof(xUnit3006Serializer), typeof(xUnit3006)]

public sealed class xUnit3006 : IXunitTestCase
{
    // ... implementation of IXunitTestCase ...
}

public class xUnit3006Serializer : IXunitSerializer
{
    // ... implementation of IXunitSerializer ...
}
```
