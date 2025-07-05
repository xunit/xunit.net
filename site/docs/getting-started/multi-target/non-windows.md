---
title: Multi-targeting with non-Windows OSes
---

# Multi-targeting with non-Windows OSes

This article describes the process that is necessary to enable multi-platform, multi-target builds; specifically, it's designed for users who are attempting to enable multi-targeting of .NET Framework when running on Linux or macOS. It leverages the .NET SDK command line tool (`dotnet`) to do builds and test execution.

> [!NOTE]
> This documentation will include links and version numbers that were valid at the time it was created. Newer package versions and links may become available in the future, so your results may be slightly different.

## Pre-Requisites{ #prereq }

The requirements to support multi-targeting on Windows and non-Windows OSes are different. For Windows OSes, it is assumed that the (one and only one version) of .NET Framework will be installed as part of your Windows installation, so the only required step is to enable it. The build system for the .NET SDK already understands how to build .NET Framework applications on Windows, and `dotnet test` already understands how to run them.

On non-Windows OSes, you're missing two key components: the libraries that are required to compile your applications, and the .NET Framework runtime. The former are added through NuGet packages, and the latter comes from Mono: an open source implementation of the .NET Framework that's available for Linux and macOS (the other two officially supported platforms for the .NET SDK).

So, in addition to installing the [.NET SDK](https://dotnet.microsoft.com/download), you will also need to install [Mono](https://www.mono-project.com/docs/getting-started/install/).

## Updating Your Project File{ #update-project }

There are two (or three) changes to make to your project (i.e., `.csproj`) file to enable multi-targeting, supporting both Windows and non-Windows machines.

### Setting TargetFrameworks{ #target-frameworks }

Make sure you've updated your `<TargetFrameworks>` element to include the .NET Framework version you're planning to target. Note that if your project was previously single-targeting .NET Core, you will need to change `<TargetFramework>` (singular) to `<TargetFrameworks>` (plural). In our example, we're adding `net452` to an existing `netcoreapp2.1` test project:

```xml
<PropertyGroup>
  <TargetFrameworks>net452;netcoreapp2.1</TargetFrameworks>
</PropertyGroup>
```

### Adding NuGet package reference{ #package-reference }

You will need a NuGet package reference for the .NET Framework reference libraries. You will add this to a conditional `<ItemGroup>` inside your `.csproj` file:

```xml
<ItemGroup Condition=" '$(TargetFrameworkIdentifier)' == '.NETFramework' ">
  <PackageReference Include="Microsoft.NETFramework.ReferenceAssemblies" Version="1.0.3" />
</ItemGroup>
```

### Adding references to system libraries (as needed){ #system-libraries }

Normally there is a batch of system libraries that automatically get referenced when building .NET Framework applications. You may find as you build on non-Windows OSes that you're missing some of these references that might otherwise have been included automatically on Windows. In our example, we were missing one reference:

```text
$ dotnet build
Microsoft (R) Build Engine version 15.9.20+g88f5fadfbe for .NET Core
Copyright (C) Microsoft Corporation. All rights reserved.

  Restore completed in 30.82 ms for ~/src/MyFirstUnitTests/MyFirstUnitTests.csproj.
  MyFirstUnitTests -> ~/src/MyFirstUnitTests/bin/Debug/netcoreapp2.1/MyFirstUnitTests.dll
Class1.cs(7,10): error CS0012: The type 'Attribute' is defined in an assembly that is not
referenced. You must add a reference to assembly 'System.Runtime, Version=4.0.0.0,
Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a'.
[~/src/MyFirstUnitTests/MyFirstUnitTests.csproj]
```

You can add them as `<Reference>` elements in the `<ItemGroup>` you created when adding your NuGet packages, since it already contains the conditional statement. To fix our error above, we needed to add one reference:

```xml
<ItemGroup Condition=" '$(TargetFrameworkIdentifier)' == '.NETFramework' ">
  <PackageReference Include="Microsoft.NETFramework.ReferenceAssemblies" Version="1.0.3" />
  <Reference Include="System.Runtime" />
</ItemGroup>
```

Note that non-Windows OSes, file systems are often case sensitive, so when adding references, make sure the names <em>exactly</em> match the casing of the files on your file system (without the `.dll` extension). You can find those files in your NuGet cache folder. The `Microsoft.NETFramework.ReferenceAssemblies` package will automatically bring in a reference to the version-specific package that contains your reference assemblies. For our example, we found the files here (your versions/paths may differ slightly):

```text
$ ls ~/.nuget/packages/microsoft.netframework.referenceassemblies.net452/1.0.3/build/.NETFramework/v4.5.2
Accessibility.dll                System.Net.NetworkInformation.dll
CustomMarshalers.dll             System.Net.Primitives.dll
ISymWrapper.dll                  System.Net.Requests.dll
...
System.Net.dll                   WindowsBase.dll
System.Net.Http.dll              WindowsFormsIntegration.dll
System.Net.Http.WebRequest.dll   XamlBuildTask.dll
```

## Running Tests{ #running-tests }

If all of these steps have been performed successfully, then your normal command line tools should all run successfully: `dotnet restore`, `dotnet build`, and `dotnet test`. Give them a try! Here are a few tips:

* If you run into restore errors, please make sure you've added the PropertyGroup and ItemGroup correctly.

* If your build fails, check the messages, as you may need to add references.

* Your tests may also fail when running on a non-Windows OS because of environmental changes, so be on the lookout for things related to file paths (`\` vs. `/`) and line ending differences (CRLF on Windows vs. LF on Linux and macOS). Make sure to use the `Path` class when creating and manipulating file paths, and use `Environment.NewLine` to know which line endings are in use. You can also pass a flag to `Assert.Equal` when comparing strings to tell it to ignore line ending differences:

  `Assert.Equal(expected, actual, ignoreLineEndingDifferences: true);`).

As previously mentioned, when running on non-Windows OSes, `dotnet test` knows how to launch your .NET Framework tests with Mono. If all goes to plan, when you run your tests, you should be able to successfully run your .NET Framework tests. The stack trace from Mono will look somewhat different from a stack trace generated by .NET Framework:

```text
Assert.Equal() Failure
Expected: 5
Actual:   4
Stack Trace:
  at MyFirstUnitTests.Class1.FailingTest () [0x0000a] in <ea38f081094e407290795149a3e20d66>:0
```

Happy testing!
