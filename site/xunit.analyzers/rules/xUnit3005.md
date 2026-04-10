---
analyzer: true
title: xUnit3005
description: Type must have an appropriate non-obsolete public constructor
severity: Error
v2: false
v3: true
aot: true
---

## Cause

A violation of this rule occurs when a type needs to have a non-obsolete, non-static public constructor that matches a specific pattern, but none of the available constructors meet the requirements.

## Reason for rule

The system needs to call the constructor with a specific set of arguments (or an empty argument list), but there is no constructor that meets the requirement.

## How to fix violations

To fix a violation of this rule, create a constructor that meets the requirements.

## Examples

### Violates

#### Assembly fixture

```csharp
using Xunit;

[assembly: AssemblyFixture(typeof(MyAssemblyFixture))]

public class MyAssemblyFixture
{
    public MyAssemblyFixture(int x)
    { /*...*/ }
}
```

```shell
error xUnit3005: Type 'MyAssemblyFixture' must have a non-obsolete public constructor: public MyAssemblyFixture()
```

#### Test framework

```csharp
[assembly: TestFramework(typeof(MyTestFramework))]

public class MyTestFramework : ITestFramework
{
    public MyTestFramework(int x)
    { /*...*/ }

    // ...implementation of ITestFramework...
}
```

```shell
error xUnit3005: Type 'MyTestFramework' must have a non-obsolete public constructor: public MyTestFramework(string? configFileName)
```

### Does not violate

#### Assembly fixture

```csharp
using Xunit;

[assembly: AssemblyFixture(typeof(MyAssemblyFixture))]

public class MyAssemblyFixture
{
    public MyAssemblyFixture()
    { /*...*/ }
}
```

#### Test framework

```csharp
[assembly: TestFramework(typeof(MyTestFramework))]

public class MyTestFramework : ITestFramework
{
    public MyTestFramework(string? configFileName)
    { /*...*/ }

    // ...implementation of ITestFramework...
}
```
