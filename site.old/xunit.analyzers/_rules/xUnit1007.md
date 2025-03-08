---
title: xUnit1007
description: ClassData must point at a valid class
category: Usage
severity: Error
v2: true
v3: true
---

## Cause

The type referenced by the `[ClassData]` attribute does not implement `IEnumerable<object[]>` or does not have a public parameterless constructor. For v3 projects, the class may also implement `IAsyncEnumerable<object[]>`, `IEnumerable<ITheoryDataRow>`, or `IAsyncEnumerable<ITheoryDataRow>`.

## Reason for rule

xUnit.net will attempt to instantiate and enumerate the type specified in `[ClassData]` in order to retrieve test data for the theory. In order for instantiation to succeed, there must be a public parameterless constructor. In order for enumeration to work, the type must implement `IEnumerable<object[]>`.

## How to fix violations

To fix a violation of this rule, make sure that the type specified in the `[ClassData]` attribute meets all of these requirements:

* Is a `class` or a `struct` type.
* Implements `IEnumerable<object[]>`.
* Defines a public parameterless constructor. (The C# and VB.NET compilers will implicitly provide a suitable default constructor if you do not define any constructors at all.)

## Examples

### Violates

```csharp
using Xunit;

class xUnit1007_TestData { }

public class xUnit1007
{
    [Theory]
    [ClassData(typeof(xUnit1007_TestData))]
    public void TestMethod(int quantity, string productType)
    { }
}
```

### Does not violate

#### For v2 and v3

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

class xUnit1007_TestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[] { 12, "book" };
        yield return new object[] { 9, "magnifying glass" };
    }

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
}

public class xUnit1007
{
    [Theory]
    [ClassData(typeof(xUnit1007_TestData))]
    public void TestMethod(int quantity, string productType)
    { }
}
```

#### For v3 only

```csharp
using System.Collections.Generic;
using System.Threading;
using Xunit;

class xUnit1007_TestData : IAsyncEnumerable<object[]>
{
    public async IAsyncEnumerator<object[]> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        yield return new object[] { 12, "book" };
        yield return new object[] { 9, "magnifying glass" };
    }
}

public class xUnit1007
{
    [Theory]
    [ClassData(typeof(xUnit1007_TestData))]
    public void TestMethod(int quantity, string productType)
    { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

class xUnit1007_TestData : IEnumerable<TheoryDataRow<int, string>>
{
    public IEnumerator<TheoryDataRow<int, string>> GetEnumerator()
    {
        yield return new TheoryDataRow<int, string>(12, "book");
        yield return new TheoryDataRow<int, string>(9, "magnifying glass");
    }

    IEnumerator IEnumerable.GetEnumerator() => throw new System.NotImplementedException();
}

public class xUnit1007
{
    [Theory]
    [ClassData(typeof(xUnit1007_TestData))]
    public void TestMethod(int quantity, string productType)
    { }
}
```

```csharp
using System.Collections.Generic;
using System.Threading;
using Xunit;

class xUnit1007_TestData : IAsyncEnumerable<TheoryDataRow<int, string>>
{
    public async IAsyncEnumerator<TheoryDataRow<int, string>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        yield return new TheoryDataRow<int, string>(12, "book");
        yield return new TheoryDataRow<int, string>(9, "magnifying glass");
    }
}

public class xUnit1007
{
    [Theory]
    [ClassData(typeof(xUnit1007_TestData))]
    public void TestMethod(int quantity, string productType)
    { }
}
```
