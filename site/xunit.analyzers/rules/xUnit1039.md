---
analyzer: true
title: xUnit1039
description: The theory argument type is not compatible with the type of the corresponding test method parameter
severity: Error
v2: true
v3: true
aot: true
---

## Cause

The type of argument given to `TheoryData<>`, `TheoryDataRow<>`, or a tuple is not compatible with the matching parameter in the test method.

## Reason for rule

When the data types aren't compatible, then the test will fail at runtime with a type mismatch, instead of running the test.

## How to fix violations

To fix a violation of this rule, make the types of the argument and parameter compatible.

## Examples

### Violates

#### Using `TheoryData<>` (for v2 and v3)

```csharp
using Xunit;

public class xUnit1039
{
    public static TheoryData<int> PropertyData =>
        new() { 1, 2, 3 };

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(string _) { }
}
```

#### Using `TheoryDataRow<>` (for v3 only)

```csharp
using System.Collections.Generic;
using Xunit;

public class xUnit1039
{
    public static IEnumerable<TheoryDataRow<int>> PropertyData =>
        [new(1), new(2), new(3)];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(string _) { }
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

public class xUnit1039
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

public class xUnit1039
{
    public static IEnumerable<(int, int)> PropertyData =>
        [(1, 10), (2, 20), (3, 30)];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(string _1, int _2) { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

public class ClassRowData : IEnumerable<(int, int)>
{
    public IEnumerator<(int, int)> GetEnumerator()
    {
        yield return (1, 10);
        yield return (2, 20);
        yield return (3, 30);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1039
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(string _1, int _2) { }
}
```

### Does not violate

#### Using `TheoryData<>` (for v2 and v3)

_Fix by changing the parameter type:_

```csharp
using Xunit;

public class xUnit1039
{
    public static TheoryData<int> PropertyData =>
        new() { 1, 2, 3 };

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(int _) { }
}
```

_Fix by changing the argument type:_

```csharp
using Xunit;

public class xUnit1039
{
    public static TheoryData<string> PropertyData =>
        new() { "1", "2", "3" };

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(string _) { }
}
```

#### Using `TheoryDataRow<>` (for v3 only)

_Fix by changing the parameter type:_

```csharp
using System.Collections.Generic;
using Xunit;

public class xUnit1039
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

_Fix by changing the argument type:_

```csharp
using System.Collections.Generic;
using Xunit;

public class xUnit1039
{
    public static IEnumerable<TheoryDataRow<string>> PropertyData =>
        [new("1"), new("2"), new("3")];

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
        yield return new("1");
        yield return new("2");
        yield return new("3");
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1039
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(string _) { }
}
```

#### Using a tuple (for v3 only)

_Fix by changing the parameter type:_

```csharp
using System.Collections.Generic;
using Xunit;

public class xUnit1039
{
    public static IEnumerable<(int, int)> PropertyData =>
        [(1, 10), (2, 20), (3, 30)];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(int _1, int _2) { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

public class ClassRowData : IEnumerable<(int, int)>
{
    public IEnumerator<(int, int)> GetEnumerator()
    {
        yield return (1, 10);
        yield return (2, 20);
        yield return (3, 30);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1039
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(int _1, int _2) { }
}
```

_Fix by changing the argument type:_

```csharp
using System.Collections.Generic;
using Xunit;

public class xUnit1039
{
    public static IEnumerable<(string, int)> PropertyData =>
        [("1", 10), ("2", 20), ("3", 30)];

    [Theory]
    [MemberData(nameof(PropertyData))]
    public void TestMethod(string _1, int _2) { }
}
```

```csharp
using System.Collections;
using System.Collections.Generic;
using Xunit;

public class ClassRowData : IEnumerable<(string, int)>
{
    public IEnumerator<(string, int)> GetEnumerator()
    {
        yield return ("1", 10);
        yield return ("2", 20);
        yield return ("3", 30);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public class xUnit1039
{
    [Theory]
    [ClassData(typeof(ClassRowData))]
    public void TestMethod(string _1, int _2) { }
}
```
