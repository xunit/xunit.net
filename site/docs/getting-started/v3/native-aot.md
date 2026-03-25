---
title: Testing with Native AOT
title-version: 2026 March 25
---

_Last updated for version `4.0.0-pre.33`_

Beginning with package version 4.0, xUnit.net now supports testing with Native AOT.

There is significant change to the underlying infrastructure of the test framework to support Native AOT, since the primary mechanism of test discovery and extensibility -- runtime reflection -- is severely limited in Native AOT. This document describes the differences between using xUnit.net in "reflection mode" vs. using it in "AOT mode".

Additionally, Native AOT mode drops support for Microsoft Testing Platform (MTP) version 1.x. To use MTP, you must use a 2.x version.

## .NET 9 and C#

A big part of the implementation that makes Native AOT support in xUnit.net possible is source generators which run while building your test project. These source generators inspect your code to discover things like tests, data sources, etc., to replace work that was previously done using runtime reflection. These source generators must inspect your source code, and in turn generate additional source code dynamically to be compiled into your test project.

Due to this source generation requirement, at this time the only language supported for Native AOT is C#.

Additionally, we make extensive use of `[OverloadResolutionPriority]`, so the minimum supported target framework for Native AOT is .NET 9.

## New NuGet packages

Every library package that we publish now includes an AOT variant, which contains libraries that are designed about Native AOT compatibility. Consult the table below to see which package names have changed:

Reflection Package                                                                                   | Native AOT Package
---------------------------------------------------------------------------------------------------- | ------------------
`xunit.v3`<br />`xunit.v3.mtp-v1`<br />`xunit.v3.mtp-v2`<br />`xunit.v3.mtp-off`                     | `xunit.v3.aot`<br />N/A (Native AOT does not support MTP v1)<br />`xunit.v3.aot.mtp-v2`<br />`xunit.v3.aot.mtp-off`
`xunit.v3.assert`                                                                                    | `xunit.v3.assert.aot`
`xunit.v3.assert.source`                                                                             | Use version 4.0+ and [define constant `XUNIT_AOT`](https://github.com/xunit/assert.xunit?tab=readme-ov-file#annotations)
`xunit.v3.common`                                                                                    | `xunit.v3.common.aot`
`xunit.v3.core`<br />`xunit.v3.core.mtp-v1`<br />`xunit.v3.core.mtp-v2`<br />`xunit.v3.core.mtp-off` | `xunit.v3.core.aot`<br />N/A (Native AOT does not support MTP v1)<br />`xunit.v3.core.aot.mtp-v2`<br />`xunit.v3.core.aot.mtp-off`
`xunit.v3.extensibility.core`                                                                        | `xunit.v3.extensibility.core.aot`
`xunit.v3.runner.common`                                                                             | `xunit.v3.runner.common.aot`
`xunit.v3.runner.console`                                                                            | Use version 4.0+
`xunit.v3.runner.inproc.console`                                                                     | `xunit.v3.runner.inproc.console.aot`
`xunit.v3.runner.msbuild`                                                                            | Use version 4.0+
`xunit.v3.runner.utility`                                                                            | `xunit.v3.runner.utility.aot`
`xunit.v3.templates`                                                                                 | Use version 4.0+
`xunit.analyzers`                                                                                    | Use version 2.0+
`xunit.runner.visualstudio`                                                                          | Use version 4.0+

## First- and third-party runners

### Unpublished projects

First- and third-party runners should be able to run Native AOT projects in unpublished form without any changes. This means, for example, that you should be able to build and run Native AOT tests in IDEs like Visual Studio or VS Code. This includes support for native xUnit.net runners, Microsoft Testing Platform v2 runners, and VSTest.

### Published projects

Once you've published your Native AOT test project (that is, by using `dotnet publish`), the produced executable is now a stand-alone executable that has no outwardly visible relationship to .NET. Third-party runners will, by and large, not be able to recognize or run tests from published binaries (though we are not aware of any runners which attempt to publish projects before running their binaries, so this is unlikely to come up).

Our first-party multi-assembly runners are still able to run published Native AOT projects (by passing the produced executable to them as the test assembly filename). That means you can continue to use runners like `xunit.v3.runner.console` or `xunit.v3.runner.msbuild` to validate your published projects and run multiple assemblies in parallel.

Third-party runners that support Microsoft Testing Platform will support your Native AOT test projects _in unpublished form_. That means you can build and run those tests in existing environments like Visual Studio or VS Code and be able to run your tests, whether they are using the reflection or AOT version of xUnit.net.

Once you've run `dotnet publish` against a Native AOT project, that published executable can be run directly, but MTP third-party runners will not consume such executables. Our first party multi-assembly runners (like `xunit.v3.runner.console` and `xunit.v3.runner.msbuild`) are still able to run published Native AOT executables, so you can use a first party runner to run multiple published AOT test projects in parallel.

If you define `<UseMicrosoftTestingPlatformRunner>true</UseMicrosoftTestingPlatformRunner>` to enable the Microsoft Testing Platform UX for `dotnet run`, this will also apply to published projects. This allows you to use any MTP-compatible extensions for published projects (assuming the extension is compatible with Native AOT).

## Behavioral changes

### Missing features

The following features are not available in AOT mode:

- EventSource reporting
- Support for default interface methods inherited by test classes
- Generic test methods
- Interface-based attributes (e.g., `IFactAttribute` vs. `FactAttribute`)
- Serialization
- Using a `.uniqueid` file to override the unique ID of a test assembly
- Using implicit/explicit operator methods to convert theory data items between types
- Stack traces from published projects will be significantly reduced

### Argument formatting

Argument formatting has changed, and some arguments/results will look different when present in AOT mode.

Assume you have a test like this:

```csharp
public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var foo1 = new Foo { IntValue = 42 };
        var foo2 = new Foo { IntValue = 2112 };

        Assert.Equal(foo1, foo2);
    }
}

public class Foo : IEquatable<Foo>
{
    public required int IntValue { get; set; }

    public bool Equals(Foo? other)
    {
        if (other is null)
            return false;

        return IntValue == other.IntValue;
    }
}
```

The failure message in reflection mode looks like this:

```shell
Assert.Equal() Failure: Values differ
Expected: Foo { IntValue = 42 }
Actual:   Foo { IntValue = 2112 }
```

The failure message in AOT mode looks like this:

```shell
Assert.Equal() Failure: Values differ
Expected: Foo { ··· }
Actual:   Foo { ··· }
```

Printing the property values of a class require reflection features that are not available to Native AOT, so the resulting failure message is modified to indicate that the inner contents of the `Foo` instance are unknown to the formatter.

## API updates

Several APIs have been updated in order to support Native AOT. We have attempted to minimize the changes required for developers doing  standard test authoring with the built-in features of xUnit.net. More extensive changes will be necessary for developers who extend xUnit.net itself, especially as it pertains to anything related to reflection.

## Assertion library updates

The following APIs have been added:

- `Assert.Equal<T>(IEnumerable<T>, ISet<T>?)`
- `Assert.Equal<T>(IEnumerable<T>, ISet<T>?, IEqualityComparer<T>)`
- `Assert.Equal<T>(IEnumerable<T>, IReadOnlySet<T>?)`
- `Assert.Equal<T>(IEnumerable<T>, IReadOnlySet<T>?, IEqualityComparer<T>)`
- `Assert.Equal<TKey, TValue>(KeyValuePair<TKey, TValue>, KeyValuePair<TKey, TValue>)`<br />_This replaces dynamic support for `KeyValuePair` in `Assert.Equal<T>(T?, T?)`_
- `Assert.NotEqual<T>(IEnumerable<T>, ISet<T>?)`
- `Assert.NotEqual<T>(IEnumerable<T>, ISet<T>?, IEqualityComparer<T>)`
- `Assert.NotEqual<T>(IEnumerable<T>, IReadOnlySet<T>?)`
- `Assert.NotEqual<T>(IEnumerable<T>, IReadOnlySet<T>?, IEqualityComparer<T>)`
- `Assert.NotEqual<TKey, TValue>(KeyValuePair<TKey, TValue>, KeyValuePair<TKey, TValue>)`<br />_This replaces dynamic support for `KeyValuePair` in `Assert.NotEqual<T>(T?, T?)`_

The following APIs have been removed:

- `Assert.Equal<T>(T[]?, T[]?)`<br />_Existing code should call `Assert.Equal<T>(IEnumerable<T>?, IEnumerable<T>?)`_
- `Assert.NotEqual<T>(T[]?, T[]?)`<br />_Existing code should call `Assert.NotEqual<T>(IEnumerable<T>?, IEnumerable<T>?)`_

The following APIs have been marked as `[Obsolete]`:

- `Assert.Equivalent`
- `Assert.EquivalentWithExclusions`
- `AssertEquivalenceComparer`

The following APIs have had signature changes:

- `Assert.NotStrictEqual<T>(T?, T?)` has become `Assert.NotStrictEqual(object?, object?)`
- `Assert.StrictEqual<T>(T?, T?)` has become `Assert.StrictEqual(object?, object?)`

The following APIs have had behavioral changes:

- `AssertEqualityComparer.GetDefaultComparer(Type)` always returns a comparer for `object`<br />_Closing an open generic requires reflection support that's unavailable in Native AOT_
- `AssertEqualityComparer.GetDefaultInnerComparer(Type)` always returns a comparer for `object`<br />_Closing an open generic requires reflection support that's unavailable in Native AOT_
- `AssertEqualityComparer<T>.Equals(T? x, CollectionTracker?, T? y, CollectionTracker?)`:
  - Supports `IEquatable<T>` but not `IEquatable<typeof y>` when the types of `x` and `y` differ
  - Supports `IComparable<T>` but not `IComparable<typeof y>` when the types of `x` and `y` differ
  - Does not support `KeyValuePair<TKey, TValue>` (this is handled as a top-level assertion now)
- `CollectionTracker` treats sets a linear containers (with [all the caveats here still applying](/docs/hash-sets-vs-linear-containers)), due to the lack of a non-generic set interface in .NET
- `CollectionTrackerExtensions.AsTracker(this IEnumerable?)` no longer supports dynamically constructing `CollectionTracker<T>` if it can determine that the provided enumerable also implements `IEnumerable<T>`, thus always returning `CollectionTracker<object>`. The existing API `CollectionTrackerExtensions.AsTracker<T>(this IEnumerable<T>?)` does continue to return `CollectionTracker<T>`.

## Core library updates

Many of the changes in the core library will be related to changes in the way tests are discovered and how extensibility changes in the new system.

- Attributes becoming unsealed in both reflection and AOT modes is typically in support of extensibility, where previously the developer could create a new attribute that implemented an existing interface, but to support AOT mode must derive from the attribute in question (e.g., `AssemblyFixtureAttribute` and `IAssemblyFixtureAttribute`).

- Attributes becoming sealed in AOT mode is typically due to the fact that attribute is no longer found via reflection at runtime, and instead a source generator must be written to discover it at build time and emit registration code.

### `Xunit` namespace

The following APIs have been marked as `[Obsolete]`:

- `CulturedFactAttributeDiscoverer`<br />_Discoverers have been replaced with source generators_
- `CulturedTheoryAttributeDiscoverer`<br />_Discoverers have been replaced with source generators_
- `FactDiscoverer`<br />_Discoverers have been replaced with source generators_
- `TheoryDiscoverer`<br />_Discoverers have been replaced with source generators_
- `FrontControllerSettings.ctor`<br />_The single constructor has been obsoleted and replaced with three factory methods that allow runnings test by unique ID as well as serialization_
- `TraitAttribute.GetTraits`<br />_The `ITraitAttribute` interface is now obsolete, so implementing `GetTraits` is unnecessary_

The following APIs have had behavioral changes:

- `AssemblyFixtureAttribute` has been unsealed in both reflection and AOT modes
- `ClassDataAttribute` is now sealed in AOT mode
- `ClassDataAttribute<T>` is now sealed in AOT mode
- `CulturedFactAttribute` is now sealed in AOT mode
- `CulturedTheoryAttribute` is now sealed in AOT mode
- `FactAttribute` is now sealed in AOT mode
- `InlineDataAttribute` is now sealed in AOT mode
- `ITheoryDataRow`
  - `SkipUnless` and `SkipWhen` have changed from `string?` to `Func<bool>?` in AOT mode
  - `SkipType` is obsolete in AOT mode
- `TheoryAttribute` is now sealed in AOT mode
- `TraitAttribute` is now unsealed in both reflection and AOT mode

### `Xunit.Runner.Common` namespace

The following APIs have been added:

- `MessageSinkMessageDeserializer.RegisterMessageSinkMessageType(string, Func<object?>)`

The following APIs have been replaced in both reflection and AOT:

- `RegisteredRunnerReporters.Get`<br />with `RegisteredRunnerConfig.GetRunnerReporters`

The following APIs have been marked as `[Obsolete]`:

- `MessageSinkMessageDeserializer.RegisterMessageSinkMessageType(Type)`
- `XunitProjectAssembly.TestCasesToRun`

### `Xunit.Sdk` namespace

The following APIs have been added:

- `AsyncUtility.Await(object?)` will await for the object if it happens to be `Task` or `ValueTask`, and NOOP otherwise.
- `TypeHelper.TryConvert<T>(object?, out T)` and `TypeHelper.TryConvertNullable<T>(object?, out T?)` have been added to do type-safe conversions of values, used primarily by the generated code for data attributes to convert data values into the appropriate type for the test method argument. This includes a few standard data-attribute conversions (`string` => `Guid`, `string` => `DateTime`, `string` => `DateTimeOffset`, `int` => `Enum`) as well as calling [`Convert.ChangeType`](https://learn.microsoft.com/dotnet/api/system.convert.changetype) to leverage any built-in system data conversions.

The following APIs have had signature changes:

- `ITestCaseDiscovered.Serialization` has changed type from `string` to `string?`, and will be `null` for test cases that originate in Native AOT.

The following APIs have had behavioral changes:

- `AsyncUtility.TryConvertToValueTask(object?)` only supports `Task` and `ValueTask` in AOT mode (support for F# async methods requires reflection features unavailable in Native AOT)
- `ExceptionUtility.ExtractMetadata(Exception)` behaves more strictly when determining the `FailureCause`, as support for "contractual" interface names is not supported in Native AOT:
  - `FailureCause.Timeout` is only returned for `Xunit.Sdk.TestTimeoutException`
  - `FailureCause.Assertion` is only returned for exceptions which derive (directly or indirectly) from `Xunit.Sdk.XunitException`.
- `ReflectionExtensions.GetDefaultValue(this Type)` will continue to return `null` for reference types, but will only return default values for a fixed set of known data types: `bool`, `byte`, `char`, `DateTime`, `DateTimeOffset`, `double`, `float`, `Guid`, `int`, `long`, `sbyte`, `short`, `TimeSpan`, `uint`, `ulong`, and `ushort` (and their `Nullable<>` counterparts). It will throw for any unsupported type.

### `Xunit.v3` namespace

The following APIs have been replaced in both reflection and AOT:

- `ExtensibilityPointFactory.GetAssemblyTestCaseOrderer`<br />with `RegisteredEngineConfig.GetAssemblyTestCaseOrderer`
- `ExtensibilityPointFactory.GetAssemblyTestCollectionOrderer`<br />with `RegisteredEngineConfig.GetAssemblyTestCollectionOrderer`
- `ExtensibilityPointFactory.GetClassTestCaseOrderer`<br />with `RegisteredEngineConfig.GetClassTestCaseOrderer`
- `ExtensibilityPointFactory.GetCollectionTestCaseOrderer`<br />with `RegisteredEngineConfig.GetCollectionTestCaseOrderer`
- `ExtensibilityPointFactory.GetTestFramework`<br />with `RegisteredEngineConfig.GetTestFramework`
- `ExtensibilityPointFactory.GetXunitTestCollectionFactory`<br />with `RegisteredEngineConfig.GetTestCollectionFactory`

The following APIs have been marked as `[Obsolete]`:

- `CulturedXunitDelayEnumeratedTheoryTestCase`<br />_Custom test cases are not required for this functionality_
- `CulturedXunitTestCase`<br />_Custom test cases are not required for this functionality_
- `DataAttribute.ConvertDataRow`<br />`DataAttribute.GetData`<br />`DataAttribute.SupportsDiscoveryEnumeration`<br/>_These methods have all been replaced by source generators_
- `ExtensibilityPointFactory.GetAssemblyBeforeAfterTestAttributes`<br />`ExtensibilityPointFactory.GetAssemblyFixture`<br />`ExtensibilityPointFactory.GetAssemblyTraits`<br />`ExtensibilityPointFactory.GetClassBeforeAfterTestAttributes`<br />`ExtensibilityPointFactory.GetClassClassFixtureTypes`<br />`ExtensibilityPointFactory.GetClassTraits`<br />`ExtensibilityPointFactory.GetCollectionBeforeAfterTestAttributes`<br />`ExtensibilityPointFactory.GetCollectionBehavior`<br />`ExtensibilityPointFactory.GetCollectionClassFixtureTypes`<br />`ExtensibilityPointFactory.GetCollectionCollectionFixtureTypes`<br />`ExtensibilityPointFactory.GetCollectionDefinitions`<br />`ExtensibilityPointFactory.GetCollectionTraits`<br />`ExtensibilityPointFactory.GetMethodBeforeAfterTestAttributes`<br />`ExtensibilityPointFactory.GetMethodDataAttributes`<br />`ExtensibilityPointFactory.GetMethodFactattributes`<br />`ExtensibilityPointFactory.GetMethodTraits`<br />`ExtensibilityPointFactory.GetXunitTestCaseDiscoverer`<br />_These methods has been replaced by source generators_
- `ISelfExecutingXunitTestCase`<br />_Self executing test cases are not supported in Native AOT_
- `ITestTimeoutException`<br />_Interface-based failure causes are not supported in Native AOT_
- `ITypeActivator` and `TypeActivator`<br />_Creating arbitrary types is not possible in Native AOT_
- `IXunitDelayEnumeratedTestCase`<br />`IXunitTest`<br />`IXunitTestAssembly`<br />`IXunitTestCase`<br />`IXunitTestClass`<br />`IXunitTestCollection`<br />`IXunitTestCollectionFactory`<br />`IXunitTestMethod`<br />_These reflection-based interfaces are replaced by `ICodeGenXyz` alternatives_
- `IXunitTestCaseDiscoverer`<br />_Discoverers have been replaced by source generators_
- `XunitDelayEnumeratedTestCase`<br />`XunitTest`<br />`XunitTestAssembly`<br />`XunitTestCase`<br />`XunitTestClass`<br />`XunitTestCollection`<br />`XunitTestMethod`<br />_These reflection-based classes are replaced by `CodeGenXyz` alternatives_
- `XunitTestCaseDiscovererAttribute`<br />_Discoverers have been replaced by source generators_
- `XunitTestFramework`<br />`XunitTestFrameworkDiscoverer`<br />`XunitTestFrameworkExecutor`<br />_These reflection-based class are replaced by `CodeGenTestFrameworkXyz` alternatives_
- Runner classes (`XunitTestXyzRunner`, `XunitTestXyzRunnerContext`) have been replaced by `CodeGenTestXyzRunner` alternatives

The following APIs have had signature changes:

- `BeforeAfterTestAttribute.After(MethodInfo, IXunitTest)`<br />to `BeforeAfterTestAttribute.After(ICodeGenTest)`
- `BeforeAfterTestAttribute.Before(MethodInfo, IXunitTest)`<br /> to `BeforeAfterTestAttribute.Before(ICodeGenTest)`
- `FixtureMappingManager.ctor` has an additional fixture factory parameter<br />_Fixture factories are created by the source generators, since Native AOT cannot depend on arbitrary constructor invocation via reflection_
- `TestCollectionFactoryBase` consumes `ICodeTestAssembly` and produces `ICodeGenTestCollection` instances
- `XunitRunnerHelper.RunXunitTestCase` is replaced by `XunitRunnerHelper.RunCodeGenTestCase`

## Source generators

There are two places where source generators are in play: engine configuration (targeting `xunit.v3.core.aot`) and runner configuration (targeting `xunit.v3.runner.common.aot`).

The pattern in both cases for source generators is to generate source for an attribute (which derives from either `EngineInitializationAttribute` or `RunnerInitializationAttribute`), and mark that as an assembly-level attribute. These attributes provide an abstract `InitializeAsync` method which must be implemented, and an optional `DisposeAsync` which may be implement if any cleanup needs to be done. These attributes run very early in the engine/runner startup and very late in cleanup.

### Engine configuration

Two particular extensibility points in `xunit.v3.core.aot` have had runtime reflection replaced by build time source generators:

- Test case discovery (e.g., `[Fact]`, `[Theory]`, etc.)
- Theory data discovery (e.g., `[InlineData]`, `[MemberData]`, etc.)

If you are extending these in reflection-mode, you must provide source generators for them in AOT mode. Registration initialization code should call the APIs on `RegisteredEngineConfig` to register the information discovered during build.

#### Test case discovery

A source generator used for test case discovery generally need to do three pieces of registration:

- `RegisteredEngineConfig.RegisterCodeGenTestClass` to register information about the test class
- `RegisteredEngineConfig.RegisterCodeGenTestMethod` to register information about the test method
- `RegisteredEngineConfig.RegisterCodeGenTestCaseFactory` to register a factory that can create 0 or more test cases

Test classes are registered by "type index" (in the form of `global::FullyQualifiedTypeName`), and include:

- The `Type` of the test class
- The `ITestCaseOrderer` for the test class, if there is one
- The `ITestMethodOrderer` for the test class, if there is one
- The class factory, in the form of `Func<FixtureMappingManager, ValueTask<CoreTestClassCreationResult>>`
- The class fixture factories, in the form of a dictionary mapping the fixture `Type` to `Func<FixtureMappingManager?, ValueTask<object>>`

Test methods are registered by "type index" and method name, and include:

- The test method's arity (how many generic types it has)
- The list of `BeforeAfterTestAttribute`s attached to the test method
- The "type index" of the declared type where the method originated (if it differs from the registered test class)
- A flag to indicate if the test method is static
- The source file where the test method lives (if known)
- The source line number where the test method lives (if known)
- The `ITestCaseOrderer` for the test method (if there is one)
- The traits attached explicitly to the test method (if any)

Test case factories are registered by "type index" and method name, and generate 0 or more test cases for the given test method. A factory is used rather than static registration, since some metadata about a test case may change based on the user's requested discovery options (e.g., enabling or disabling pre-enumeration, defaults used to create the test case display name, etc.).

#### Theory data discovery

A source generator used for theory data discovery generally only needs to do a single piece of registration:

- `RegisteredEngineConfig.RegisterTheoryDataRowFactory` to register the theory data row factory

The theory data row factory is a function which accepts `DisposalTracker`, and returns a collection of 0 or more instances of `ITheoryDataRow`. The registration targets a specific test method on a specific test class.

#### Additional registration APIs

There are several other registration APIs that are used by built-in source generators. They are documented here for completeness, in case an extension wishes to use alternative registration mechanisms for these engine configuration items.

- `RegisteredEngineConfig.RegisterAssemblyFixtureFactory`<br />_Used for `[assembly: AssemblyFixture]`_
- `RegisteredEngineConfig.RegisterAssemblyTestCaseOrdererFactory`<br />_Used for `[assembly: TestCaseOrderer]`_
- `RegisteredEngineConfig.RegisterAssemblyTestClassOrdererFactory`<br />_Used for `[assembly: TestClassOrderer]`_
- `RegisteredEngineConfig.RegisterAssemblyTestCollectionOrdererFactory`<br />_Used for `[assembly: TestCollectionOrderer]`_
- `RegisteredEngineConfig.RegisterAssemblyTestMethodOrdererFactory`<br />_Used for `[assembly: TestMethodOrderer]`_
- `RegisteredEngineConfig.RegisterCollectionDefinition`<br />_Used for `[CollectionDefinition]` on classes_
- `RegisteredEngineConfig.RegisterTestCollectionFactoryFactory`<br />_Used for `[assembly: CollectionBehavior]`_
- `RegisteredEngineConfig.RegisterTestFrameworkFactory`<br />_Used for `[assembly: TestFramework]`_
- `RegisteredEngineConfig.RegisterTestPipelineStartupFactory`<br />_Used for `[assembly: TestPipelineStartup]`_

### Runner configuration

There are three registration APIs which are currently used by built-in source generators. They are documented here for completeness, in case any extension wishes to use alternative registration mechanisms for these runner configuration items.

- `RegisteredRunnerConfig.RegisterConsoleResultWriter`<br />_Used for `[assembly: RegisterConsoleResultWriter]`_
- `RegisteredRunnerConfig.RegisterMicrosoftTestingPlatformResultWriter`<br />_Used for `[assembly: RegisterMicrosoftTestingPlatformResultWriter]`_
- `RegisteredRunnerConfig.RegisterRunnerReporter`<br />_Used for `[assembly: RegisterRunnerReporter]`_
