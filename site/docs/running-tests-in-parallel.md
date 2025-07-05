---
title: Running Tests in Parallel
---

# Running Tests in Parallel

## Background{ #background }

Running unit tests in parallel is a new feature in xUnit.net version 2. There are two essential motivations that drove us to not only enable parallelization, but also for it to be a feature that's enabled by default:

1. As unit testing has become more prevalent, so too have the number of unit tests. It is not unusual for a project to have thousands—or tens of thousands—of unit tests. Developers want the safety of being able to quickly run all these tests before committing their code.

2. A typical developer machine in 2006 (when we first started working on xUnit.net) had a single or dual core CPU, no hyper-threading, and perhaps 2 GB of RAM. Today's modern developer machine is likely to have a CPU with at least 8 threads and at least 16GB of RAM (and usually more of both). These CPUs go to waste when only one of their threads can be used for a given task.

There are really two ways to take advantage of all these extra resources: write tests which themselves use parallelization (so that when the system is only running a single test, it still takes advantage of all the resources); or, let the unit test framework run many tests at the same time, to help take advantage of the available resources.

The typical structure of a unit test is to test a single thing in relative isolation. This doesn't give much opportunity for the test itself to become parallelized, unless the code under test is itself parallelized. Therefore, the best way to ensure that unit tests can run at the full speed of the host computer is to run many of them at the same time.

## Algorithms{ #algorithms }

As of Core Framework v2 2.8, we've changed the default parallelism algorithm, with the ability to fall back to the original. The two algorithms are called <code>conservative</code> (the new default) and <code>aggressive</code> (the original algorithm). They have the following attributes:

### Conservative

> This will only <em>start</em> as many tests as your max parallel threads setting. The system will wait for a test to finish before starting another test. This allows more accurate timing of the running of tests, which allows <code>Timeout</code> on <code>[Fact]</code> to work as expected even when running tests in parallel. It also creates less pressure on the task system, which has occasionally caused deadlocks in complex projects. The downside is that projects with lots of tests which spend a significant amount of time waiting for async operations to complete will under-utilize the maximum CPU potential, so this new algorithm may cause your overall test run to be slower.

### Aggressive

> This is the original parallelism algorithm, which starts as many tests as possible, regardless of your max parallel threads setting, and uses a <a href="https://learn.microsoft.com/dotnet/api/system.threading.synchronizationcontext"><code>SynchronizationContext</code></a> to limit the number of things that are running at any given time. Since tests in this system which encounter async awaits are put back into a pool to compete against all potential running tests, they may wait longer to resume which causes the inaccuracy of timing that makes <code>Timeout</code> problematic. On the flip side, projects with lots of tests which spend a significant amount of time waiting for async operations will make better use of CPU resources, assuming there are more tests ready to run, so this old algorithm may finish running your tests quicker than the new algorithm.

In general, the advice here is to try to use the new (default) algorithm, and only revert back to the original algorithm if you find your tests are significantly slower (understanding that other limitations around timing).

With Runners v2 2.8+, you can specify overrides for the parallelism algorithm in the following ways:

* You can specify a value in your [`xunit.runner.json`](config-xunit-runner-json#parallelAlgorithm)
* The console runner can specify the value via the <code>-parallelalgorithm</code> switch
* The MSBuild runner can specify the value via the <code>ParallelAlgorithm</code> property
* The Visual Studio runner can specify the value via [RunSettings](config-runsettings#ParallelAlgorithm)
* Microsoft Testing Platform can specify the value via [`testconfig.json`](config-testconfig-json#parallelAlgorithm)

> [!NOTE]
> * The algorithm is only used when you have enabled test collection parallelism, and you are using a limited number of threads (i.e., not <code>unlimited</code> or <code>-1</code>).
> * You must be using Core Framework v2 2.8 or later (or Core Framework v3) for the new algorithm.
> * You must be using a runner linked against v2 2.8 or later (or v3) for the new algorithm configuration file support. This includes all in-box v2 runners 2.8 or later, all in-box v3 runners, and <code>xunit.runner.visualstudio</code> 2.8 or later. Third party runners will need to be linked against <code>xunit.runner.utility</code> 2.8.0 or later, or `xunit.v3.runner.utility`.

## Runners and Test Frameworks{ #runners-and-test-frameworks }

For the purposes of this section, it's important to separate the two actors that participate in running your unit tests.

The first is the <em>runner</em>, which is the program (or third party plugin to a program) that is responsible for looking for one or more test assemblies, and then activating the test frameworks that it finds therein. It generally contains very little knowledge about how the test frameworks work, and instead relies on the xUnit.net runner utility library to do most of the heavy lifting. Through the runner utility library, it can discover <em>test cases</em> and then ask for them to be run. It does not itself understand how this discovery or execution works, but instead relies on the runner utility library to understand those details.

The second is the <em>test framework</em>, which is the code that has the detailed knowledge of how to discover and run unit tests. These libraries are the ones that the unit tests themselves link against, and so those DLLs live along side the unit test code itself. For xUnit.net v1, that is <code>xunit.dll</code>; for v2, it's <code>xunit.core.dll</code> (and, indirectly, <code>xunit.execution.*.dll</code>); for v3, it's `xunit.v3.core.dll`.

There is a third player here that does not have any code, but rather contains the abstractions that allow runners and v2 test projects to communicate: <code>xunit.abstractions.dll</code>.

## Parallelism in Test Frameworks{ #parallelism-in-test-frameworks }

When we say "Parallelism in Test Frameworks", what we mean specifically is how a test framework may choose to support running tests <em>within a single assembly</em> in parallel with one another. The next section, "Parallelism in Runners", we mean how a test runner may choose to support running <em>test assemblies</em> in parallel against each other.

As mentioned above, parallelism in the test framework is a feature that was introduced in Core Framework v2. Tests written in xUnit.net v1 cannot be run in parallel against each other in the same assembly, though multiple test assemblies linked against v1 are still able to participate in the runner parallelism feature described in the next sub-section.

### Parallelism in xUnit.net v2/v3

#### Test Collections

How does xUnit.net v2/v3 decide which tests can run against each other in parallel? It uses a concept called <em>test collections</em> to make that decision.

By default, each test class is a unique test collection. Tests within the same test class will not run in parallel against each other. Let's examine a very simple test assembly, one with a single test class:

```csharp
public class TestClass1
{
    [Fact]
    public void Test1()
    {
        Thread.Sleep(3000);
    }

    [Fact]
    public void Test2()
    {
        Thread.Sleep(5000);
    }
}
```

When we run this test assembly, we see that the total time spent running the tests is approximately 8 seconds. These two tests are in the same test class, which means that they are in the same test collection, so they cannot be run in parallel against one another.

If we were to put these two tests into separate test classes, like this:

```csharp
public class TestClass1
{
    [Fact]
    public void Test1()
    {
        Thread.Sleep(3000);
    }
}

public class TestClass2
{
    [Fact]
    public void Test2()
    {
        Thread.Sleep(5000);
    }
}
```

Now when we run this test assembly, we see that the total time spent running the tests is approximately 5 seconds (assuming you have at least two threads available for parallelism). That's because <code>Test1</code> and <code>Test2</code> are in different test collections, so they are able to run in parallel against one another.

#### Custom Test Collections

If we need to indicate that multiple test classes should not be run in parallel against one another, then we place them into the same test collection. This is simply a matter of decorating each test class with an attribute that places them into the same uniquely named test collection:

```csharp
[Collection("Our Test Collection #1")]
public class TestClass1
{
    [Fact]
    public void Test1()
    {
        Thread.Sleep(3000);
    }
}

[Collection("Our Test Collection #1")]
public class TestClass2
{
    [Fact]
    public void Test2()
    {
        Thread.Sleep(5000);
    }
}
```

This instructs xUnit.net not run these two classes against each other in parallel. Our total run time now goes back to approximately 8 seconds, which indicates that the tests did indeed run one after another.

> [!NOTE]
> For more information on test collections, including the ability to use them to share text context, see <a href="shared-context">Shared Context</a>.

#### Changing Default Behavior

There are several default pieces of behavior that can be configured by the developer as relates to running tests in parallel. These are all changed by applying an assembly level attribute:

* Put all test classes into a single test collection by default:

  `[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]`

  <em>**Default**: CollectionBehavior.CollectionPerClass</em>

* Set the maximum number of threads to use when running test in parallel:

  `[assembly: CollectionBehavior(MaxParallelThreads = <em>n</em>)]`

  <em>**Default**: number of CPU threads in the PC</em>

* Turn off parallelism inside the assembly:

  <code>[assembly: CollectionBehavior(DisableTestParallelization = true)]</code>

  <em>**Default**: false</em>

* Turn off parallelism for specific Test Collection

  <code>[CollectionDefinition(DisableParallelization = true)]</code> (placed on the collection definition class)

  <em>**Default**: false</em><br />

  Parallel-capable test collections will be run first (in parallel), followed by parallel-disabled test collections (run sequentially).

For the assembly-level attributes, you can only have one of these attributes per assembly; if you want to combine multiple behaviors, do it with a single attribute. Also be aware that these values affect only this assembly; if multiple assemblies are running in parallel against one another, they have their own independent values.

## Parallelism in Runners{ #parallelism-in-runners }

When we say "Parallelism in Runners", what we mean specifically is how a runner may choose to run multiple test assemblies in parallel against each other. The decision to do this is independent of whether or not any individual test assembly is running tests within itself in parallel. Runners are also allowed to override some of the behavior within a test framework (like number of threads, whether an assembly should run tests within itself in parallel, etc.); when they do so, it overrides whatever behavior has been otherwise specified (via code or configuration).

There are many runners, and just as many ways to configure parallelism values. This section will cover how to specify parallelism values for the built-in console and MSBuild runners; you should look at the documentation for any third party runners to learn how to configure them.

### Console Runner{ #console-runner }

The console runner (`xunit.runner.console` for v2 and `xunit.v3.runner.console` for v3) can run multiple assemblies at the same time, and command line options can be used to configure the parallelism options used when running the tests.

The following command line options can be used to influence parallelism:

| Option                        | Effect
| ----------------------------- | ------
| `-maxThreads <n>`             | Overrides the maximum number of threads used <em>per assembly.</em> For console runner v2 2.8.0 or later (and console runner v3), you can also use a multiplier syntax (i.e., <code>2.0x</code> will use a max thread count that is double the number of CPU threads). The default value is the number of CPU threads in the PC.
| `-parallel <option>`          | Allows the user to specify which kinds of parallelization should be allowed for the test run. The valid option values are `none` (turns off all parallelization), `collections` (parallelizes collections but not assemblies), `assemblies` (parallelizes assemblies but not collections), and `all` (parallelizes both collections and assemblies). The default value is `collections`.
| `-parallelAlgorithm <option>` | Changes the parallelism algorithm. Prior to Core Framework v2 2.8, the system always used the <code>aggressive</code> algorithm; for Core Framework v2 2.8 onward (and Core Framework v3), you can specify either <code>conservative</code> or <code>aggressive</code>.

### MSBuild Runner{ #msbuild-runner }

Like the console runner, the MSBuild runner can run multiple assemblies at the same time, and build file options can be used to configuration the parallelism options used when running the tests.

The following <code>xunit</code> task properties can be used to influence parallelism:

| Property                     | Effect
| ---------------------------- | ------
| `MaxParallelThreads`         | Overrides the maximum number of threads used <em>per assembly.</em> For MSBuild runner v2 2.8.0 or later (and MSBuild runner v3), you can also use a multiplier syntax (i.e., <code>2.0x</code> will use a max thread count that is double the number of CPU threads). The default value is the number of CPU threads in the PC.
| `ParallelAlgorithm`          | Changes the parallelism algorithm. Prior to Core Framework v2 2.8, the system always used the <code>aggressive</code> algorithm; for Core Framework v2 2.8 onward (and Core Framework v3), you can specify either <code>conservative</code> or <code>aggressive</code>.
| `ParallelizeAssemblies`      | Set to <code>true</code> to run the test assemblies in parallel against one other; set to <code>false</code> to run them sequentially. The default value is <code>false</code>.
| `ParallelizeTestCollections` | Set to <code>true</code> to run the test collections in parallel against one other; set to <code>false</code> to run them sequentially. The default value is <code>true</code>

## Parallelism via Configuration{ #parallelism-via-configuration }

There are several configuration elements that can influence parallelism. Please see the appropriate configuration documentation for more information:

* [Config with `xunit.runner.json`](config-xunit-runner-json)
* [Config with `testconfig.json`](config-testconfig-json) (for Microsoft Testing Platform)
* [Config with RunSettings](config-runsettings) (for VSTest)
