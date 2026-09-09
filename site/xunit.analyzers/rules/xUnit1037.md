---
analyzer: true
title: xUnit1037
description: There are fewer arguments than required by the parameters of the test method
severity: Error
v2: true
v3: true
aot: true
---

## Cause

When you use `[MemberData]` or `[ClassData]` with `TheoryData`, `TheoryDataRow`, or a tuple, the number of arguments must match the number of parameters in the test method. In this case, you have provided too few arguments.

## Reason for rule

You must provide the correct number of arguments to the test method to run the test.

## How to fix violations

To fix a violation of this rule, either add more arguments to match the method signature, or remove parameters from the test method.

## Examples

### Violates

#### Using `TheoryData<>` (for v2 and v3)

```csharp
using Xunit;

public class xUnit1037
{
    public static TheoryData<int> PropertyData =>
        new() { 1, 2, 3 };

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(int _1, string _2) { }
}
```

#### Using `TheoryDataRow<>` (for v3 only)

```csharp
using Xunit;

public class xUnit1037
{
    public static IEnumerable<TheoryDataRow<int>> PropertyData =>
        [new(1), new(2), new(3)];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(int _1, string _2) { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

public class ClassRowData : IEnumerable<TheoryDataRow<int>>
{
    public IEnumerator<TheoryDataRow<int>> GetEnumerator()
    {
        yield return new(1);
        yield return new(2);
        yield return new(3);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1037
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(int _1, string _2) { }
}
```

#### Using a tuple (for v3 only)

```csharp
using Xunit;

public class xUnit1037
{
    public static IEnumerable<(int, string)> PropertyData =>
        [(1, "Hello"), (2, "there"), (3, "world")];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(int _1, string _2, double _3) { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

public class ClassRowData : IEnumerable<(int, string)>
{
    public IEnumerator<(int, string)> GetEnumerator()
    {
        yield return (1, "Hello");
        yield return (2, "there");
        yield return (3, "world");
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1037
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(int _1, string _2, double _3) { }
}
```

### Does not violate

#### Using `TheoryData<>` (for v2 and v3)

_Fix by adding an argument:_

```csharp
using Xunit;

public class xUnit1037
{
    public static TheoryData<int, string> PropertyData =>
        new() { { 1, "Hello" }, { 2, "World" } };

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(int _1, string _2) { }
}
```

_Fix by removing a parameter:_

```csharp
using Xunit;

public class xUnit1037
{
    public static TheoryData<int> PropertyData =>
        new() { 1, 2, 3 };

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(int _) { }
}
```

#### Using `TheoryDataRow<>` (for v3 only)

_Fix by adding an argument:_

```csharp
using System.Collections.Generic;
using Xunit;

public class xUnit1037
{
    public static IEnumerable<TheoryDataRow<int, string>> PropertyData =>
        [new(1, "Hello"), new(2, "World")];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(int _1, string _2) { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

public class ClassRowData : IEnumerable<TheoryDataRow<int, string>>
{
    public IEnumerator<TheoryDataRow<int, string>> GetEnumerator()
    {
        yield return new(1, "Hello");
        yield return new(2, "World");
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1037
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(int _1, string _2) { }
}
```

_Fix by removing a parameter:_

```csharp
using Xunit;

public class xUnit1037
{
    public static IEnumerable<TheoryDataRow<int>> PropertyData =>
        [new(1), new(2), new(3)];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(int _) { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

public class ClassRowData : IEnumerable<TheoryDataRow<int>>
{
    public IEnumerator<TheoryDataRow<int>> GetEnumerator()
    {
        yield return new(1);
        yield return new(2);
        yield return new(3);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1037
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(int _1) { }
}
```

#### Using a tuple (for v3 only)

_Fix by adding an argument:_

```csharp
using Xunit;

public class xUnit1037
{
    public static IEnumerable<(int, string)> PropertyData =>
        [(1, "Hello", 21.12), (2, "there", 42.24), (3, "world", 63.36)];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(int _1, string _2, double _3) { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

public class ClassRowData : IEnumerable<(int, string)>
{
    public IEnumerator<(int, string)> GetEnumerator()
    {
        yield return (1, "Hello", 21.12);
        yield return (2, "there", 42.24);
        yield return (3, "world", 63.36);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1037
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(int _1, string _2, double _3) { }
}
```

_Fix by removing a parameter:_

```csharp
using Xunit;

public class xUnit1037
{
    public static IEnumerable<(int, string)> PropertyData =>
        [(1, "Hello"), (2, "there"), (3, "world")];

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
    public IEnumerator<(int, string)> GetEnumerator()
    {
        yield return (1, "Hello");
        yield return (2, "there");
        yield return (3, "world");
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1037
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(int _1, string _2) { }
}
```

