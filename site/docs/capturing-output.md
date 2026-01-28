---
title: Capturing Output
---

# Capturing Output

In order to assist in debugging failing test (especially when running them on remote machines without access to a debugger), it can often be helpful to add diagnostic output that is separate from passing or failing test results. xUnit.net offers two such methods for adding output, depending on what kind of code you're trying to diagnose.

* xUnit.net v1 always captured output from `Console`, `Debug`, and `Trace`.

* xUnit.net v2 removed this ability and replaced it with an injectable `ITestOutputHelper` that could be used to capture output from a test.

* xUnit.net v3 reintroduced the ability to capture output, which is disabled by default for backward compatibility with v2.

## Output in unit tests

### xUnit.net v3

xUnit.net v3 reintroduces the ability to capture output that is written to `Console`, `Debug`, and `Trace` through two assembly-level attributes.

> [!NOTE]
> Since this is applied as an assembly-level attribute, the decision to capture output in this way affects all tests in a unit test project.

The `CaptureConsole` attribute will capture output to both standard out and standard error by default. You can choose to capture only one or the other through the use of attribute parameters:

```csharp
// Both standard output and standard error
[assembly: CaptureConsole]

// Only standard output
[assembly: CaptureConsole(CaptureError = false)]

// Only standard error
[assembly: CaptureConsole(CaptureOutput = false)]
```

Similarly, the `CaptureTrace` attribute will capture output to be `Trace` and `Debug` by default. However, due to the way `Trace` and `Debug` interact, you either capture both or none, so there are no parameters:

```csharp
// Captures both Trace and Debug
[assembly: CaptureTrace]
```

Also bear in mind that `Debug` output is only captured when running a Debug build of your unit tests; when running a Release build, all usage of `Debug` is turned off, including output.

> [!NOTE]
> Both the console and the trace system are process-wide shared resources. xUnit.net v3 uses an async-local context (`TestContext`) to be able to associate the current thread with the associated test. This allows test parallelization while routing the console and trace output to their appropriate tests.
>
> If you write console or trace output to a thread that is not associated with a test (such as a background worker thread created by your test or production code), then that output will silently discarded as there is no test to associate the output to.

xUnit.net v3 tests may also continue to use `ITestOutputHelper` as described below.

### xUnit.net v2

Unit tests in xUnit.net v2 have access to a special interface which replaces previous usage of `Console` and similar mechanisms: `ITestOutputHelper`. In order to take advantage of this, just add a constructor argument for this interface, and stash it so you can use it in the unit test.

```csharp
using Xunit;
using Xunit.Abstractions;

public class MyTestClass
{
    private readonly ITestOutputHelper output;

    public MyTestClass(ITestOutputHelper output)
    {
        this.output = output;
    }

    [Fact]
    public void MyTest()
    {
        var temp = "my class!";
        output.WriteLine("This is output from {0}", temp);
    }
}
```

As you can see in the example above, the `WriteLine` function on `ITestOutputHelper` supports formatting arguments, just as you were used to with `Console`.

In addition to being able to write to the output system during the unit test, you can also write to it during the constructor (and during your implementation of `IDisposable.Dispose`, if you choose to have one). This test output will be wrapped up into the XML output, and most test runners will surface the output for you as well.

![](/images/capturing-output/vs-runner-output.png){ .border .oversize }

To see output from `dotnet test`, pass the command line option `--logger "console;verbosity=detailed"`:

![](/images/capturing-output/dotnet-test-output.png){ .border .oversize width=800 }

### Showing live output

Starting with xUnit.net v2 2.8.1 (and runners linked against this version), we have added a new configuration option named [`showLiveOutput`](/docs/config-xunit-runner-json#showLiveOutput) to indicate that you would like to be able to see output from `ITestOutputHelper` live as it occurs, rather than just waiting for the test to finish. This is turned off by default as that was the behavior prior to 2.8.1, and showing live output can add significantly to the noise of the output when not needed. We anticipate that users will turn this option on temporarily while debugging through particular issues rather than be something that's left on all the time.

## Output in extensibility classes

Output for unit tests are grouped and displayed with the specific unit test. Output from extensibility classes, on the other hand, is considered diagnostic information. Most runners require you to enable diagnostic output either explicitly with a command line option, or implicitly on an assembly-by-assembly basis by using [configuration files](/docs/config-xunit-runner-json).

### xUnit.net v3

Due to the globally available test context, sending diagnostic messages in xUnit.net v3 is quite simple.

Here is an example of sending a diagnostic message in a test case orderer:

```csharp
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Sdk;
using Xunit.v3;

public class MyTestCaseOrderer : ITestCaseOrderer
{
    public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>(IReadOnlyCollection<TTestCase> testCases)
        where TTestCase : notnull, ITestCase
    {
        var result = testCases.ToList();  // ...perform ordering here...
        TestContext.Current.SendDiagnosticMessage("Ordered {0} test cases", result.Count);
        return result;
    }
}
```

xUnit.net v3 extensibility classes may also continue to use `IMessageSink` as described below.

### xUnit.net v2

Each extensibility class has its own individual constructor requirements. In addition, they can take _as their last constructor parameter_ an instance of `IMessageSink` that is designated solely for sending diagnostic messages. Diagnostic messages implement `IDiagnosticMessage` from `xunit.abstractions`. If you're linked against `xunit.execution`, there is a `DiagnosticMessage` class in the `Xunit.Sdk` namespace available for your use.

The extensibility interfaces which currently support this functionality are:

* `IClassFixture<>`
* `ICollectionFixture<>`
* `IDataDiscoverer`
* `ITestCaseOrderer`
* `ITestCollectionOrderer`
* `ITestFrameworkTypeDiscoverer`
* `ITraitDiscoverer`
* `IXunitTestCaseDiscoverer`
* `IXunitTestCollectionFactory`

Here is an example of using it in a test case orderer:

```csharp
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

public class MyTestCaseOrderer : ITestCaseOrderer
{
    private readonly IMessageSink diagnosticMessageSink;

    public MyTestCaseOrderer(IMessageSink diagnosticMessageSink)
    {
        this.diagnosticMessageSink = diagnosticMessageSink;
    }

    public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
        where TTestCase : ITestCase
    {
        var result = testCases.ToList();  // ...perform ordering here...
        var message = new DiagnosticMessage("Ordered {0} test cases", result.Count);
        diagnosticMessageSink.OnMessage(message);
        return result;
    }
}
```

### Viewing diagnostic messages in Visual Studio

After [enabling diagnostic messages in your configuration file](/docs/config-xunit-runner-json#diagnosticMessages), when run, Visual Studio's output window contains a Tests tab which contains the information from running the tests, including the diagnostic message.

```text
Starting test discovery for requested test run
========== Starting test discovery ==========
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.1.1+bf6400fd51 (64-bit .NET 8.0.17)
[xUnit.net 00:00:00.51]   Discovering: OutputExample (method display = ClassAndMethod, method display options = None)
[xUnit.net 00:00:00.53]   Discovered:  OutputExample (found 1 test case)
========== Test discovery finished: 1 Tests found in 1.4 sec ==========
========== Starting test run ==========
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.1.1+bf6400fd51 (64-bit .NET 8.0.17)
[xUnit.net 00:00:00.24]   Starting:    OutputExample (parallel test collections = on, max threads = 24)
[xUnit.net 00:00:00.26] OutputExample: Ordered 1 test cases
[xUnit.net 00:00:00.27]   Finished:    OutputExample
========== Test run finished: 1 Tests (1 Passed, 0 Failed, 0 Skipped) run in 282 ms ==========
```

> [!NOTE]
> To see this output, open the Output window in Visual Studio (from the main menu: View > Output), and in the "Show output from" drop down, select "Tests".
>
> Also note that the output may look slightly different depending on whether Test Explorer is using VSTest mode or Microsoft Testing Platform mode.
