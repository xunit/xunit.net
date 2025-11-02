---
title: Custom Test Class Construction
title-version: 2025 November 2
---

As of version `3.2.0`, we are now supporting a way to override the default test class construction behavior.

## Why Override?

By default, test class constructor arguments can be:

* [`ITestContextAccessor`](https://github.com/xunit/xunit/blob/9e422989c1a48ce35b821566c12d01ea418018bc/src/xunit.v3.core/ITestContextAccessor.cs)
* [`ITestOutputHelper`](https://github.com/xunit/xunit/blob/9e422989c1a48ce35b821566c12d01ea418018bc/src/xunit.v3.core/ITestOutputHelper.cs)
* Fixture value (class fixture, collection fixture, or assembly fixture)

Any constructor argument which cannot be resolved will generate an error when running the test:

> `The following constructor parameters did not have matching fixture data`

Some developers may want to use additional logic to resolve these missing constructor arguments. One common scenario might be to allow a dependency injection system to resolve those arguments; for example, to give tests access to services registered that the production code will be using.

Now they can provide an alternative construction system for test classes.

## Implementing `ITypeActivator`

The new [`ITypeActivator` interface](https://github.com/xunit/xunit/blob/9e422989c1a48ce35b821566c12d01ea418018bc/src/xunit.v3.core/Abstractions/Framework/ITypeActivator.cs) contains a single method:

```csharp
object CreateInstance(
    ConstructorInfo constructor,
    object?[]? arguments,
    Func<Type, IReadOnlyCollection<ParameterInfo>, string> missingArgumentMessageFormatter);
```

The activator is given the constructor that the test framework has selected, along with all the arguments that it could collect. Any argument value which could not be found will be represented in the array by [`Missing.Value`](https://learn.microsoft.com/dotnet/api/system.reflection.missing.value).

The activator should resolve all the missing values and then constructor and return the object. If one or more missing values cannot be resolved, it is expected that the activator will throw an instance of [`TestPipelineException`](https://github.com/xunit/xunit/blob/9e422989c1a48ce35b821566c12d01ea418018bc/src/xunit.v3.core/Exceptions/TestPipelineException.cs). A `missingArgumentMessageFormatter` function is provided to the activator, which can be called with (a) the `Type` that's being created, and (b) the list of `ParameterInfo` that are missing values (this allows the caller to express the context in which the creation failed; this is where the message above is provided by the test framework).

The default type activator cannot resolve any unknown parameters, so its implementation looks like this:

```csharp
object ITypeActivator.CreateInstance(
    ConstructorInfo constructor,
    object?[]? arguments,
    Func<Type, IReadOnlyCollection<ParameterInfo>, string> missingArgumentMessageFormatter)
{
    if (constructor is null)
        throw new ArgumentNullException(nameof(constructor));
    if (missingArgumentMessageFormatter is null)
        throw new ArgumentNullException(nameof(missingArgumentMessageFormatter));

    var type =
        constructor.ReflectedType
            ?? constructor.DeclaringType
            ?? throw new ArgumentException("Untyped constructors are not permitted", nameof(constructor));

    if (arguments is not null)
    {
        var parameters = constructor.GetParameters();
        if (parameters.Length != arguments.Length)
            throw new TestPipelineException(
                string.Format(
                    CultureInfo.CurrentCulture,
                    "Cannot create type '{0}' due to parameter count mismatch (needed {1}, got {2})",
                    type.FullName ?? type.Name,
                    parameters.Length,
                    arguments.Length
                )
            );

        var missingArguments =
            arguments
                .Select((a, idx) => a is Missing ? parameters[idx] : null)
                .WhereNotNull()
                .CastOrToReadOnlyCollection();

        if (missingArguments.Count != 0)
            throw new TestPipelineException(missingArgumentMessageFormatter(type, missingArguments));
    }

    return constructor.Invoke(arguments);
}
```

## Registering your type activator

It is assumed that things like dependency injection containers will be created by way of an early registration system like [`ITestPipelineStartup`](https://github.com/xunit/xunit/blob/9e422989c1a48ce35b821566c12d01ea418018bc/src/xunit.v3.core/Abstractions/Framework/ITestPipelineStartup.cs).

Once the container is fully configured and the type activator has been created, it is registered by calling:

```csharp
Xunit.v3.TypeActivator.Current = MY_TYPE_ACTIVATOR_INSTANCE;
```

It is assumed that a type activator will be created and registered once during the pipeline startup and left in place for the duration of the test assembly lifetime.
