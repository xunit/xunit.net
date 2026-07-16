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

As of Core Framework v2 2.8, we've changed the default parallelism algorithm, with the ability to fall back to the original. The two algorithms are called `conservative` (the new default) and `aggressive` (the original algorithm). They have the following attributes:

### Conservative

> This will only _start_ as many tests as your max parallel threads setting. The system will wait for a test to finish before starting another test. This allows more accurate timing of the running of tests, which allows `Timeout` on `[Fact]` to work as expected even when running tests in parallel. It also creates less pressure on the task system, which has occasionally caused deadlocks in complex projects. The downside is that projects with lots of tests which spend a significant amount of time waiting for async operations to complete will under-utilize the maximum CPU potential, so this new algorithm may cause your overall test run to be slower.

### Aggressive

> This is the original parallelism algorithm, which starts as many tests as possible, regardless of your max parallel threads setting, and uses a [`SynchronizationContext`](https://learn.microsoft.com/dotnet/api/system.threading.synchronizationcontext) to limit the number of things that are running at any given time. Since tests in this system which encounter async awaits are put back into a pool to compete against all potential running tests, they may wait longer to resume which causes the inaccuracy of timing that makes `Timeout` problematic. On the flip side, projects with lots of tests which spend a significant amount of time waiting for async operations will make better use of CPU resources, assuming there are more tests ready to run, so this old algorithm may finish running your tests quicker than the new algorithm.

In general, the advice here is to try to use the new (default) algorithm, and only revert back to the original algorithm if you find your tests are significantly slower (understanding that other limitations around timing).

With Runners v2 2.8+, you can specify overrides for the parallelism algorithm in the following ways:

* You can specify a value in your [`xunit.runner.json`](/docs/config-xunit-runner-json#parallelAlgorithm)
* The console runner can specify the value via the `-parallelalgorithm` switch
* The MSBuild runner can specify the value via the `ParallelAlgorithm` property
* The Visual Studio runner can specify the value via [RunSettings](/docs/config-runsettings#ParallelAlgorithm)
* Microsoft Testing Platform can specify the value via [`testconfig.json`](/docs/config-testconfig-json#parallelAlgorithm)

> [!NOTE]
> * The algorithm is only used when the parallel mode is not `off`, and you are using a limited number of threads (i.e., not `unlimited` or `-1`).
> * You must be using Core Framework v2 2.8 or later (or Core Framework v3) for the new algorithm.
> * You must be using a runner linked against v2 2.8 or later (or v3) for the new algorithm configuration file support. This includes all in-box v2 runners 2.8 or later, all in-box v3 runners, and `xunit.runner.visualstudio` 2.8 or later. Third party runners will need to be linked against `xunit.runner.utility` 2.8.0 or later, or `xunit.v3.runner.utility`.

## Runners and Test Frameworks{ #runners-and-test-frameworks }

For the purposes of this section, it's important to separate the two actors that participate in running your unit tests.

The first is the _runner_, which is the program (or third party plugin to a program) that is responsible for looking for one or more test assemblies, and then activating the test frameworks that it finds therein. It generally contains very little knowledge about how the test frameworks work, and instead relies on the xUnit.net runner utility library to do most of the heavy lifting. Through the runner utility library, it can discover _test cases_ and then ask for them to be run. It does not itself understand how this discovery or execution works, but instead relies on the runner utility library to understand those details.

The second is the _test framework_, which is the code that has the detailed knowledge of how to discover and run unit tests. These libraries are the ones that the unit tests themselves link against, and so those DLLs live along side the unit test code itself. For xUnit.net v1, that is `xunit.dll`; for v2, it's `xunit.core.dll` (and, indirectly, `xunit.execution.*.dll`); for v3, it's `xunit.v3.core.dll`.

There is a third player here that does not have any code, but rather contains the abstractions that allow runners and v2 test projects to communicate: `xunit.abstractions.dll`.

## Parallelism in Test Frameworks{ #parallelism-in-test-frameworks }

When we say "Parallelism in Test Frameworks", what we mean specifically is how a test framework may choose to support running tests _within a single assembly_ in parallel with one another. The next section, "Parallelism in Runners", we mean how a test runner may choose to support running _test assemblies_ in parallel against each other.

As mentioned above, parallelism in the test framework is a feature that was introduced in Core Framework v2. Tests written in xUnit.net v1 cannot be run in parallel against each other in the same assembly, though multiple test assemblies linked against v1 are still able to participate in the runner parallelism feature described in the next sub-section.

### Parallel modes

In xUnit.net v2, and xUnit.net v3 prior to 4.0, only two parallel modes are supported (`none` and `collections`); in xUnit.net v3 starting with 4.0, a third mode is supported (`all`). The default mode for all v2 and v3 projects is `collections`.

#### Parallel mode: `none`

In parallel mode `none`, tests within a test assembly are not run in parallel. Each test is run sequentially. There is no need to protect against simultaneous tests accessing shared resources, since only a single test at a time will be running.

Let's assume you have two test classes:

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

When we run this test assembly, we see that the total time spent runnings the tests is approximately 8 seconds. These two tests are not run in parallel: one is run, then when it's finished, the other will be run.

#### Parallel mode: `collections`

In parallel mode `collections`, tests within a single test collection will not be run in parallel against each other, but tests in different test collections will run in parallel against each other.

By default, there is a _test collection per test class_. This means that tests in a test class will not be run in parallel against one another, but tests in different test classes will be. Shared resources may need to be protected if they could be accessed from several tests simultaneously.

Let's examine a very simple test assembly, one with a single test class:

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

Now when we run this test assembly, we see that the total time spent running the tests is approximately 5 seconds (assuming you have at least two threads available for parallelism). That's because `Test1` and `Test2` are in different test collections, so they are able to run in parallel against one another.

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
> For more information on test collections, including the ability to use them to share text context, see [Shared Context](/docs/shared-context).

#### Parallel mode: `all`

In parallel mode `all`, all tests are run in parallel against all other tests.

Let's examine a very simple test assembly, one with a single test class:

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

When we run this test assembly, we see that the total time spent running the tests is approximately 5 seconds, since both tests are run at the same time.

### Selectively opting out of parallelism

When parallelism is enabled (that is, for parallel mode `collections` or `all`), you can selectively opt out parallelism entirely. Any test which is opted out of parallelism will be guaranteed not to run in parallel against any other test.

#### Opting a test collection out of parallelism

_Valid for parallel modes `collections` and `all`, ignored by parallel mode `none`_

To opt all the tests from a test collection out of parallelism, create a collection definition with parallelism turned off, like this:

```csharp
[CollectionDefinition("Test Collection Name", DisableParallelization = true)]
public class OurTestCollectionDefinition { }

[Collection("Test Collection Name")]
public class TestClass1
{
    [Fact]
    public void Test1()
    {
        // ... test code here ...
    }
}
```

#### Opting a test class out of parallelism

_Valid for parallel mode `all`, ignored by parallel modes `collections` and `none`_

To opt all the tests from a test class out of parallelism, add `[TestClass]`, as illustrated here:

```csharp
[TestClass(DisableParallelism = true)]
public class TestClass1
{
    [Fact]
    public void Test1()
    {
        // ... test code here ...
    }
}
```

#### Opting a test method out of parallelism

_Valid for parallel mode `all`, ignored by parallel modes `collections` and `none`_

To opt all the tests from a test method out of parallelism, add `DisableParallelism` to the `[Fact]` or `[Theory]` attribute, as illustrated here:

```csharp
public class TestClass1
{
    [Fact(DisableParallelism = true)]
    public void Test1()
    {
        // ... test code here ...
    }
}
```

#### Opting a data source out of parallelism

_Valid for parallel mode `all`, ignored by parallel modes `collections` and `none`_

To opt all the tests from a theory data source out of parallelism, add `DisableParallelism` to the data attribute, as illustrated here:

```csharp
public class TestClass1
{
    [Theory]
    [InlineData(42, DisableParallelization = true)]  // Runs non-parallel
    [InlineData(2112)]                               // Runs in parallel
    public void Test1(int value)
    {
        // ... test code here ...
    }
}
```

### Opting a data row out of parallelism

_Valid for parallel mode `all`, ignored by parallel modes `collections` and `none`_

To opt an individual data row out of parallelism, use the `DisableParallelism` property on the theory data row object, as illustrated here:

```csharp
public class TestClass1
{
    public static IEnumerable<TheoryDataRow<int>> DataSource =
    [
        new(42) { DisableParallelization = true },  // Runs non-parallel
        new(2112),                                  // Runs in parallel
    ];

    [Theory]
    [MemberData(nameof(DataSource))]
    public void Test1(int value)
    {
        // ... test code here ...
    }
}
```

### Changing default behaviors

There are several default pieces of behavior that can be configured by the developer as relates to running tests in parallel. These are all changed by applying an assembly level attribute.

#### Set the default collection behavior:

_Core Framework v2, v3_

> `[assembly: CollectionBehavior(CollectionBehavior.CollectionPerAssembly)]`<br />
> `[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass)]`<br />
> &nbsp; &nbsp; _**Default:** `CollectionBehavior.CollectionPerClass`_

#### Set the maximum number of threads to use when running test in parallel:

_Core Framework v2, v3 (prior to 4.0)_

> `[assembly: CollectionBehavior(MaxParallelThreads = n)]`<br />
> &nbsp; &nbsp; _**Default**: number of CPU threads in the PC_

_Core Framework v3 (4.0 and later)_

> `[assembly: Parallelization(MaxThreads = n)]`<br />
> &nbsp; &nbsp; _**Default**: number of CPU threads in the PC_

#### Set the parallel mode:

_Core Framework v2, v3 (prior to 4.0)_

> `[assembly: CollectionBehavior(DisableTestParallelization = false)]`<br />
> `[assembly: CollectionBehavior(DisableTestParallelization = true)]`<br />
> &nbsp; &nbsp; _**Default**: `false`_<br />
> &nbsp; &nbsp; _Note: `false` is equivalent to `collections`, and `true` is equivalent to `none`)_

_Core Framework v3 (4.0 and later)_

> `[assembly: Parallelization(Mode = ParallelMode.None)]`<br />
> `[assembly: Parallelization(Mode = ParallelMode.Collections)]`<br />
> `[assembly: Parallelization(Mode = ParallelMode.All)]`<br />
> &nbsp; &nbsp; _**Default**: `ParallelMode.Collections`_

#### Set the parallel algorithm:

_Core Framework v2, v3 (prior to 4.0)_

> `[assembly: CollectionBehavior(ParallelAlgorithm = ParallelAlgorithm.Aggressive)]`<br />
> `[assembly: CollectionBehavior(ParallelAlgorithm = ParallelAlgorithm.Conservative)]`<br />
> &nbsp; &nbsp; _**Default**: `ParallelAlgorithm.Conservative`_

_Core Framework v3 (4.0 and later)_

> `[assembly: Parallelization(Algorithm = ParallelAlgorithm.Aggressive)]`<br />
> `[assembly: Parallelization(Algorithm = ParallelAlgorithm.Conservative)]`<br />
> &nbsp; &nbsp; _**Default**: `ParallelAlgorithm.Conservative`_

## Parallelism in Runners{ #parallelism-in-runners }

When we say "Parallelism in Runners", what we mean specifically is how a runner may choose to run multiple test assemblies in parallel against each other. The decision to do this is independent of whether or not any individual test assembly is running tests within itself in parallel. Runners are also allowed to override some of the behavior within a test framework (like number of threads, whether an assembly should run tests within itself in parallel, etc.); when they do so, it overrides whatever behavior has been otherwise specified (via code or configuration).

There are many runners, and just as many ways to configure parallelism values. This section will cover how to specify parallelism values for the built-in console and MSBuild runners; you should look at the documentation for any third party runners to learn how to configure them.

### Console Runner{ #console-runner }

The console runner (`xunit.runner.console` for v2 and `xunit.v3.runner.console` for v3) can run multiple assemblies at the same time, and command line options can be used to configure the parallelism options used when running the tests.

The following command line options can be used to influence parallelism:

| Option                         | Effect
| ------------------------------ | ------
| `-maxThreads <n>`              | Overrides the maximum number of threads used _per assembly._ For console runner v2 2.8.0 or later (and console runner v3), you can also use a multiplier syntax (i.e., `2.0x` will use a max thread count that is double the number of CPU threads). The default value is the number of CPU threads in the PC.
| `-parallelAlgorithm <option>`  | Changes the parallelism algorithm. Prior to Core Framework v2 2.8, the system always used the `aggressive` algorithm; for Core Framework v2 2.8 onward (and Core Framework v3), you can specify either `conservative` or `aggressive`.
| `-parallelAssemblies <option>` | Changes the inter-assembly parallel mode (valid values are `on` and `off`).
| `-parallelMode <option>`       | Changes the intra-assembly parallel mode (valid values are `none`, `collections`, and `all`).

> [!NOTE]
> If you set `-parallelMode all`, when running v2 test assemblies and/or v3 test assemblies (prior to 4.0), it will be treated by those test assemblies as though you had requested `-parallelMode collections`.

The `-parallelAssemblies` and `-parallelMode` switches are new to `xunit.v3.runner.console` (4.0 and later). `xunit.runner.console` (and `xunit.v3.runner.console` prior to 4.0) have a single `-parallel` option, with the following equivalencies:

Older switch            | New switches
----------------------- | ------------------
`-parallel none`        | `-parallelMode none -parallelAssemblies off`
`-parallel assemblies`  | `-parallelMode none -parallelAssemblies on`
`-parallel collections` | `-parallelMode collections -parallelAssemblies off`
`-parallel all`         | `-parallelMode collections -parallelAssemblies on`

### MSBuild Runner{ #msbuild-runner }

The MSBuild runner (`xunit.runner.msbuild` for v2 and `xunit.v3.runner.msbuild` for v3) can run multiple assemblies at the same time, and build file options can be used to configuration the parallelism options used when running the tests.

The following `xunit` task properties can be used to influence parallelism:

| Property                     | Effect
| ---------------------------- | ------
| `MaxParallelThreads`         | Overrides the maximum number of threads used _per assembly._ For MSBuild runner v2 2.8.0 or later (and MSBuild runner v3), you can also use a multiplier syntax (i.e., `2.0x` will use a max thread count that is double the number of CPU threads). The default value is the number of CPU threads in the PC.
| `ParallelAlgorithm`          | Changes the parallelism algorithm. Prior to Core Framework v2 2.8, the system always used the `aggressive` algorithm; for Core Framework v2 2.8 onward (and Core Framework v3), you can specify either `conservative` or `aggressive`.
| `ParallelizeAssemblies`      | Set to `true` to run the test assemblies in parallel against one other; set to `false` to run them sequentially. The default value is `false`.
| `ParallelMode`               | Changes the intra-assembly parallel mode (valid values are `none`, `collections`, and `all`).

> [!NOTE]
> If you set `ParallelMode='all'`, when running v2 test assemblies and/or v3 test assemblies (prior to 4.0), it will be treated by those test assemblies as though you had requested `ParallelMode='collections'`.

The `ParallelMode` property is new to `xunit.v3.runner.msbuild` (4.0 and later). `xunit.runner.msbuild` (and `xunit.v3.runner.msbuild` prior to 4.0) include a `ParallelizeTestCollections` property, which can be set to `true` or `false` (which is equivalent to parallel mode `collections` and `none`, respectively).

## Parallelism via Configuration{ #parallelism-via-configuration }

There are several configuration elements that can influence parallelism. Please see the appropriate configuration documentation for more information:

* [Config with `xunit.runner.json`](/docs/config-xunit-runner-json)
* [Config with `testconfig.json`](/docs/config-testconfig-json) (for Microsoft Testing Platform)
* [Config with RunSettings](/docs/config-runsettings) (for VSTest)
