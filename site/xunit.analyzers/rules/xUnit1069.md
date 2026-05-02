---
analyzer: true
title: xUnit1069
description: Test methods with a Timeout should reference TestContext.Current.CancellationToken
severity: Warning
v2: false
v3: true
aot: true
---

## Cause

A violation of this rule occurs when a test with a timeout does not consume the cancellation token from the test context.

## Reason for rule

The cancellation token in the test context is signaled when the test framework wants the test to stop running. This includes when the test has timed out from running too long.

## How to fix violations

To fix a violation of this rule, either pass the cancellation token to async methods which consume it, or directly consume it in the test.

## Examples

### Violates

```csharp
using Xunit;

public class xUnit1069
{
    [Fact(Timeout = 42)]
    public void TestMethod()
    {
        // ... test code ...
    }
}
```

### Does not violate

```csharp
using System.Threading.Tasks;
using Xunit;

public class xUnit1069
{
    [Fact(Timeout = 42)]
    public async Task TestMethod()
    {
        // ... test code ...
        await Task.Delay(100, TestContext.Current.CancellationToken);
        // ... test code ...
    }
}
```

```csharp
using Xunit;

public class xUnit1069
{
    [Fact(Timeout = 42)]
    public void TestMethod()
    {
        // ... test code ...
        TestContext.Current.CancellationToken.ThrowIfCancellationRequested();
        // ... test code ...
    }
}
```
