---
analyzer: true
title: xUnit1068
description: MemberData cannot point to an open generic type
severity: Error
v2: false
v3: false
aot: true
---

## Cause

A violation of this rule occurs when a `[MemberData]` attribute points to an open generic class.

## Reason for rule

Open generic types cannot be closed at runtime in Native AOT.

## How to fix violations

To fix a violation of this rule, close the generic or move the data source to a non-generic type.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit1068<T>
{
    public static TheoryData<T> Data = /*...*/;

    [Theory]
    [MemberData(nameof(Data))]
    public void TestMethod(T _) { }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit1068
{
    public static TheoryData<int> Data = /*...*/;

    [Theory]
    [MemberData(nameof(Data))]
    public void TestMethod(int _) { }
}
```

```csharp
using Xunit;

public class DataSource<T>
{
    public static TheoryData<T> Data = /*...*/;
}

public class xUnit1068
{
    [Theory]
    [MemberData(nameof(Data), MemberType = typeof(DataSource<int>))]
    public void TestMethod(int _) { }
}
```
