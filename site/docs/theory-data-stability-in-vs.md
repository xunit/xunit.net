---
title: Theory Data Stability in Test Explorer
title-version: 2016 August 17
---

> [!NOTE]
> Some of the information in this article applies to behavior in older versions of Visual Studio, and may no longer be relevant, especially when using xUnit.net v3 (due to changes in the way Test Explorer in VSTest vs. Microsoft Testing Platform behave).

## Why isn't my test running?

I recently received a tweet from an xUnit.net user wondering why their theory tests using `DateTime.Now` don't run in Visual Studio. Most of their tests show as run, but this one never does. Even stranger, if they run the test individually, it runs fine; it's only when they use "Run All" that the test does not appear to run.

Using this as sample code:

```csharp
using System;
using System.Collections.Generic;
using Xunit;

namespace VsRunnerNotRunTestRepro;

public class Repro
{
    public static IEnumerable<object[]> TestData
        => new object[][] {
            new object[] { 42 },
            new object[] { 21.12 },
            new object[] { DateTime.Now },
            new object[] { null }
        };

    [Theory]
    [MemberData(nameof(TestData))]
    public void UnrunTestRepro(object data)
    {
        Assert.NotNull(data);
    }
}
```

This is what the test discovery looks like inside Visual Studio:

![](/images/theory-data-stability-in-vs/pre-run.png){ .border width=360 }

When you click "Run All", this is what Visual Studio shows:

![](/images/theory-data-stability-in-vs/post-run.png){ .border width=400 }

If you look at the Output window, you'll see a curious message that is your hint as to what's going on:

```text
------ Run test started ------
[xUnit.net 00:00:00.1476501]   Discovering: VsRunnerNotRunTestRepro (app domain = on [shadow copy], method display = Method)
[xUnit.net 00:00:00.2152233]   Discovered:  VsRunnerNotRunTestRepro (running 4 test cases)
[xUnit.net 00:00:00.3744754]   Starting:    VsRunnerNotRunTestRepro (parallel test collections = on, max threads = 8)
[xUnit.net 00:00:00.5224541]   Finished:    VsRunnerNotRunTestRepro
Test adapter sent back a result for an unknown test case. Ignoring result for 'UnrunTestRepro(data: 2016-08-17T09:32:21.7708662-07:00)'.
========== Run test finished: 3 run (0:00:00.56059) ==========
```

## Discovery vs. Execution in Visual Studio's test runner

Unit testing systems are generally split into two phases: test discovery and execution. In the case of the Visual Studio test runner (regardless of the underlying test framework), it runs the discovery phase in order to populate the list of tests in Test Explorer, and it runs the execution phase to run the tests the user wants to run.

When the user wants to run just a selected few tests, it instructs the unit testing framework to run those specific tests by saying "Remember these tests you discovered? Please run them.". However, if the user clicks "Run All", then Visual Studio says "I'm not going to give you a list of tests to run; they just want to run them all". The object which tracks each individual test you see in the Visual Studio Test Explorer UI is what's called a "test case".

When we discovered the tests, `DateTime.Now` returned the current date & time _at the time of discovery_. If Visual Studio hands xUnit.net back the test case and says "run this", then we know what the date & time was that we discovered (it's encoded into the test case), and we run exactly what it expects. However, when Visual Studio says "just run everything" without giving us the test cases, we must re-perform the discovery phase before running the tests. The value returned from `DateTime.Now` is, of course, different, so the test we're running is not _exactly the same_ as the test that we gave to Visual Studio earlier. So we run the test with the new date & time and report that back to Visual Studio. When it attempts to line the test results up with the test cases it knows about, it doesn't find a match.

What we're experiencing here is "unstable theory data"; that is, the data we retrieve each time we enumerate the tests during discovery is different, and therefore not repeatable. We are running a test _very much like_ the ones we originally discovered, but not identical.

This concept of theory data stability isn't unique to `DateTime.Now`. Imagine you were instead performing [fuzz testing](https://en.wikipedia.org/wiki/Fuzz_testing) which returned seemingly random data every time you enumerated it. Every time you rebuild in Visual Studio, the list of test values changes, and the "Run All" button becomes effectively useless.

The most common way to fix this issue is to give xUnit.net a hint that your data is not stable by telling it not to perform data enumeration during discovery:

```csharp
[MemberData(nameof(TestData), DisableDiscoveryEnumeration = true)]
```

Test Explorer will only show a single entry for your test method now, and when you run it, all the results of the individual data elements will be shown when you click on the test in the tree:

![](/images/theory-data-stability-in-vs/run-with-disabled-discovery.png){ .border width=400 }

This allows you to continue to successfully run and report all the test results, albeit at the expense of being able to run any one individual data row.
