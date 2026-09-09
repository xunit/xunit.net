---
analyzer: true
title: xUnit1040
description: The theory argument type is nullable, while the type of the corresponding test method parameter is not
severity: Warning
v2: true
v3: true
aot: true
---

## Cause

The type of argument given to `TheoryData<>`, `TheoryDataRow<>`, or tuple is marked as nullable, and the test method argument is marked as non-nullable.

## Reason for rule

Passing `null` data to a test method that isn't expecting it could cause runtime errors or unpredictable test results
(either false positives or false negatives).

## How to fix violations

To fix a violation of this rule, either make the argument non-nullable, or make the test method parameter nullable.

## Examples

### Violates

#### Using `TheoryData<>` (for v2 and v3)

```csharp
using Xunit;

public class xUnit1040
{
    public static TheoryData<string?> PropertyData =>
        new() { "Hello", "World", default(string) };

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(string _) { }
}
```

#### Using `TheoryDataRow<>` (for v3 only)

```csharp
using System.Collections.Generic;
using Xunit;

public class xUnit1040
{
    public static IEnumerable<TheoryDataRow<string?>> PropertyData =>
        [new("Hello"), new("World"), new(null)];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(string _) { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

public class ClassRowData : IEnumerable<TheoryDataRow<string?>>
{
    public IEnumerator<TheoryDataRow<string?>> GetEnumerator()
    {
        yield return new("Hello");
        yield return new("World");
        yield return new(null);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1040
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(string _) { }
}
```

#### Using a tuple (for v3 only)

```csharp
using System.Collections.Generic;
using Xunit;

public class xUnit1040
{
    public static IEnumerable<(int, string?)> PropertyData =>
        [(1, "Hello"), (2, "World"), (3, null)];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(int _1, string _2) { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

public class ClassRowData : IEnumerable<(int, string?)>
{
    public IEnumerator<(int, string?)> GetEnumerator()
    {
        yield return (1, "Hello");
        yield return (2, "World");
        yield return (3, null);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1040
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(int _1, string _2) { }
}
```

### Does not violate

#### Using `TheoryData<>` (for v2 and v3)

_Fix by changing the argument type:_

```csharp
using Xunit;

public class xUnit1040
{
    public static TheoryData<string> PropertyData =>
        new() { "Hello", "World" };

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(string _) { }
}
```

_Fix by changing the parameter type:_

```csharp
using Xunit;

public class xUnit1040
{
    public static TheoryData<string?> PropertyData =>
        new() { "Hello", "World", default(string) };

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(string? _) { }
}
```

#### Using `TheoryDataRow<>` (for v3 only)

_Fix by changing the argument type:_

```csharp
using System.Collections.Generic;
using Xunit;

public class xUnit1040
{
    public static IEnumerable<TheoryDataRow<string>> PropertyData =>
        [new("Hello"), new("World")];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(string _) { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

public class ClassRowData : IEnumerable<TheoryDataRow<string>>
{
    public IEnumerator<TheoryDataRow<string>> GetEnumerator()
    {
        yield return new("Hello");
        yield return new("World");
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1040
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(string _) { }
}
```

_Fix by changing the parameter type:_

```csharp
using System.Collections.Generic;
using Xunit;

public class xUnit1040
{
    public static IEnumerable<TheoryDataRow<string?>> PropertyData =>
        [new("Hello"), new("World"), new(null)];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(string? _) { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

public class ClassRowData : IEnumerable<TheoryDataRow<string?>>
{
    public IEnumerator<TheoryDataRow<string?>> GetEnumerator()
    {
        yield return new("Hello");
        yield return new("World");
        yield return new(null);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1040
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(string? _) { }
}
```

#### Using a tuple (for v3 only)

_Fix by changing the argument type:_

```csharp
using System.Collections.Generic;
using Xunit;

public class xUnit1040
{
    public static IEnumerable<(int, string)> PropertyData =>
        [(1, "Hello"), (2, "World")];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(int _1, string _2) { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

public class ClassRowData : IEnumerable<(int, string)>
{
    public IEnumerator<(int, string?)> GetEnumerator()
    {
        yield return (1, "Hello");
        yield return (2, "World");
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1040
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(int _1, string _2) { }
}
```

_Fix by changing the parameter type:_

```csharp
using System.Collections.Generic;
using Xunit;

public class xUnit1040
{
    public static IEnumerable<(int, string?)> PropertyData =>
        [(1, "Hello"), (2, "World"), (3, null)];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(int _1, string? _2) { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

public class ClassRowData : IEnumerable<(int, string?)>
{
    public IEnumerator<(int, string?)> GetEnumerator()
    {
        yield return (1, "Hello");
        yield return (2, "World");
        yield return (3, null);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1040
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(int _1, string? _2) { }
}
```
