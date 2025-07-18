---
analyzer: true
title: xUnit2009
description: Do not use boolean check to check for substrings
category: Assertions
severity: Warning
v2: true
v3: true
---

## Cause

A violation of this rule occurs when `Assert.True` or `Assert.False` are used to check for substrings with string methods like `string.Contains`, `string.StartsWith` and `string.EndsWith`.

## Reason for rule

There are specialized assertions for substring checks.

## How to fix violations

To fix a violation of this rule, replace the offending assertion according to this:

- `Assert.True(str.Contains(word))` => `Assert.Contains(word, str)`
- `Assert.False(str.Contains(word))` => `Assert.DoesNotContain(word, str)`
- `Assert.True(str.StartsWith(word))` => `Assert.StartsWith(word, str)`
- `Assert.True(str.EndsWith(word))` => `Assert.EndsWith(word, str)`

## Examples

### Violates

```csharp
using Xunit;

public class xUnit2009
{
    [Fact]
    public void TestMethod()
    {
        var result = "foo bar baz";

        Assert.True(result.Contains("bar"));
    }
}
```

### Does not violate

```csharp
using Xunit;

public class xUnit2009
{
    [Fact]
    public void TestMethod()
    {
        var result = "foo bar baz";

        Assert.Contains("bar", result);
    }
}
```
