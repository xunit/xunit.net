---
title: Sharing Context between Tests
---

# Sharing Context between Tests

It is common for unit test classes to share setup and cleanup code (often called "test context"). xUnit.net offers several methods for sharing this setup and cleanup code, depending on the scope of things to be shared, as well as the expense associated with the setup and cleanup code.

## Constructor and Dispose{ #constructor }

_**When to use:** when you want a clean test context for every test (sharing the setup and cleanup code, without sharing the object instance)._

xUnit.net creates a new instance of the test class for every test that is run, so any code which is placed into the constructor of the test class will be run for every single test. This makes the constructor a convenient place to put reusable context setup code where you want to share the code without sharing object instances (meaning, you get a clean copy of the context object(s) for every test that is run).

For context cleanup, add the `IDisposable` interface to your test class, and put the cleanup code in the `Dispose()` method.

> [!NOTE]
> You cannot call asynchronous methods in a constructor. You can implement `IAsyncLifetime` to get both an async startup and cleanup method (`InitializeAsync` and `DisposeAsync`). For xUnit.net v3, the `DisposeAsync` comes from `IAsyncDisposable`, which you can implement independently just for async cleanup (note: xUnit.net v2 does not support `IAsyncDisposable`, only v3; to get async cleanup in v2, you must implement the full `IAsyncLifetime` interface).

Here is a simple example:

```csharp
public class StackTests : IDisposable
{
    Stack<int> stack;

    public StackTests()
    {
        stack = new Stack<int>();
    }

    public void Dispose()
    {
        stack.Dispose();
    }

    [Fact]
    public void WithNoItems_CountShouldReturnZero()
    {
        var count = stack.Count;

        Assert.Equal(0, count);
    }

    [Fact]
    public void AfterPushingItem_CountShouldReturnOne()
    {
        stack.Push(42);

        var count = stack.Count;

        Assert.Equal(1, count);
    }
}
```

This structure is sometimes called the "test class as context" pattern, since the test class itself is a self-contained definition of the context setup and cleanup code. You can even name the test classes after the setup context so that it's easier to remember what your starting point is:

```csharp
public class StackTests
{
    public class EmptyStack
    {
        Stack<int> stack;

        public EmptyStack()
        {
            stack = new Stack<int>();
        }

        // ... tests for an empty stack ...
    }

    public class SingleItemStack
    {
        Stack<int> stack;

        public SingleItemStack()
        {
            stack = new Stack<int>();
            stack.Push(42);
        }

        // ... tests for a single-item stack ...
    }
}
```

At a high level, we're writing tests for the `Stack` class, and each context is a `Stack` in a given state. To reflect this, we've wrapped all the test context classes in a parent class named `StackTests`.

## Class Fixtures{ #class-fixture }

_**When to use:** when you want to create a single test context and share it among all the tests in the class, and have it cleaned up after all the tests in the class have finished._

Sometimes test context creation and cleanup can be very expensive. If you were to run the creation and cleanup code during every test, it might make the tests slower than you want. You can use the _class fixture_ feature of xUnit.net to share a single object instance among all tests in a test class.

We already know that xUnit.net creates a new instance of the test class for every test. When using a class fixture, xUnit.net will ensure that the fixture instance will be created before any of the tests have run, and once all the tests have finished, it will clean up the fixture object by calling `Dispose`, if present.

To use class fixtures, you need to take the following steps:

* Create the fixture class, and put the startup code in the fixture class constructor.
* If the fixture class needs to perform cleanup, implement `IDisposable` on the fixture class, and put the cleanup code in the `Dispose()` method.
* Add `IClassFixture<>` to the test class.
* If the test class needs access to the fixture instance, add it as a constructor argument, and it will be provided automatically.

<p>Here is a simple example:</p>

```csharp
public class DatabaseFixture : IDisposable
{
    public DatabaseFixture()
    {
        Db = new SqlConnection("MyConnectionString");

        // ... initialize data in the test database ...
    }

    public void Dispose()
    {
        // ... clean up test data from the database ...
    }

    public SqlConnection Db { get; private set; }
}

public class MyDatabaseTests : IClassFixture<DatabaseFixture>
{
    DatabaseFixture fixture;

    public MyDatabaseTests(DatabaseFixture fixture)
    {
        this.fixture = fixture;
    }

    // ... write tests, using fixture.Db to get access to the SQL Server ...
}
```

Just before the first test in `MyDatabaseTests` is run, xUnit.net will create an instance of `DatabaseFixture`. For each test, it will create a new instance of `MyDatabaseTests`, and pass the shared instance of `DatabaseFixture` to the constructor.

> [!NOTE]
> xUnit.net uses the presence of the interface `IClassFixture<>` to know that you want a class fixture to be created and cleaned up. It will do this whether you take the instance of the class as a constructor argument or not. Similarly, if you add the constructor argument but forget to add the interface, xUnit.net will let you know that it does not know how to satisfy the constructor argument.

If you need multiple fixture objects, you can implement the interface as many times as you want, and add constructor arguments for whichever of the fixture object instances you need access to. The order of the constructor arguments is unimportant.

Note that you cannot control the order that fixture objects are created, and fixtures cannot take dependencies on other fixtures. If you have need to control creation order and/or have dependencies between fixtures, you should create a class which encapsulates the other two fixtures, so that it can do the object creation itself.

## Collection Fixtures{ #collection-fixture }

_**When to use:** when you want to create a single test context and share it among tests in several test classes, and have it cleaned up after all the tests in the test classes have finished._

Sometimes you will want to share a fixture object among multiple test classes. The database example used for class fixtures is a great example: you may want to initialize a database with a set of test data, and then leave that test data in place for use by multiple test classes. You can use the _collection fixture_ feature of xUnit.net to share a single object instance among tests in several test classes.

To use collection fixtures, you need to take the following steps:

* Create the fixture class, and put the startup code in the fixture class constructor.
* If the fixture class needs to perform cleanup, implement `IDisposable` on the fixture class, and put the cleanup code in the `Dispose()` method.
* Create the collection definition class, decorating it with the `[CollectionDefinition]` attribute, giving it a unique name that will identify the test collection.
* Add `ICollectionFixture<>` to the collection definition class.
* Add the `[Collection]` attribute to all the test classes that will be part of the collection, using the unique name you provided to the test collection definition class's `[CollectionDefinition]` attribute.
* If the test classes need access to the fixture instance, add it as a constructor argument, and it will be provided automatically.

Here is a simple example:

```csharp
public class DatabaseFixture : IDisposable
{
    public DatabaseFixture()
    {
        Db = new SqlConnection("MyConnectionString");

        // ... initialize data in the test database ...
    }

    public void Dispose()
    {
        // ... clean up test data from the database ...
    }

    public SqlConnection Db { get; private set; }
}

[CollectionDefinition("Database collection")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

[Collection("Database collection")]
public class DatabaseTestClass1
{
    DatabaseFixture fixture;

    public DatabaseTestClass1(DatabaseFixture fixture)
    {
        this.fixture = fixture;
    }
}

[Collection("Database collection")]
public class DatabaseTestClass2
{
    // ...
}
```

xUnit.net treats collection fixtures in much the same way as class fixtures, except that the lifetime of a collection fixture object is longer: it is created before any tests are run in any of the test classes in the collection, and will not be cleaned up until all test classes in the collection have finished running.

Test collections can also be decorated with `IClassFixture<>`. xUnit.net treats this as though each individual test class in the test collection were decorated with the class fixture.

Test collections also influence the way xUnit.net runs tests when running them in parallel. For more information, see [Running Tests in Parallel](/docs/running-tests-in-parallel).

> [!NOTE]
> Fixtures can be shared across assemblies, but collection definitions **must be in the same assembly** as the test that uses them.

## Assembly Fixtures{ #assembly-fixture }

_**When to use:** when you want to create a single test context and share it among all the tests in your test assembly._

Newly introduced in Core Framework v3, you can now share a single instance of a fixture among all the test classes in your test assembly.

Here is the example from collection fixtures, but adapted to be used as an assembly fixture:

```csharp
[assembly: AssemblyFixture(typeof(DatabaseFixture))]

public class DatabaseFixture : IDisposable
{
    public DatabaseFixture()
    {
        Db = new SqlConnection("MyConnectionString");

        // ... initialize data in the test database ...
    }

    public void Dispose()
    {
        // ... clean up test data from the database ...
    }

    public SqlConnection Db { get; private set; }
}

public class DatabaseTestClass1
{
    DatabaseFixture fixture;

    public DatabaseTestClass1(DatabaseFixture fixture)
    {
        this.fixture = fixture;
    }

    // ...
}

public class DatabaseTestClass2
{
    DatabaseFixture fixture;

    public DatabaseTestClass2(DatabaseFixture fixture)
    {
       this.fixture = fixture;
    }

    // ...
}
```

Instance of assembly fixtures are created once before any test in your assembly is run, and cleaned up after all tests have finished running. Any test class may gain access to the assembly fixture simply by adding it as a constructor argument.

Note that unlike collection fixtures, there is no change in parallelization when using an assembly fixture. This means fixtures used as assembly fixtures may be used from multiple tests simultaneously, and must be designed for with this parallelism requirement in mind. Alternatively, you could disable all parallelism in your test assembly by setting the [`parallelizeTestCollections`](/docs/config-xunit-runner-json#parallelizeTestCollections) configuration setting to `false`.
