---
title: "Microsoft Testing Platform (xUnit.net v2)"
title-version: 2025 December 30
---

Support for running xUnit.net v2 tests via [Microsoft Testing Platform](https://learn.microsoft.com/dotnet/core/testing/unit-testing-platform-intro) is available via a third party NuGet package (`YTest.MTP.XUnit2`).

* [NuGet Package](https://www.nuget.org/packages/YTest.MTP.XUnit2)
* [GitHub](https://github.com/Youssef1313/YTest.MTP.XUnit2)

Note that this project is not officially supported by xUnit.net or Microsoft. Any issues using this package should be reported to the GitHub project linked above.

See the [README](https://github.com/Youssef1313/YTest.MTP.XUnit2/blob/main/README.md) for the current feature set list as well as installation instructions.

## What is Microsoft Testing Platform?

VSTest has been the underlying driver behind `dotnet test` and Test Explorer (and `vstest.console` and Test View before them) since it first launched in Visual Studio 2010. The new Microsoft Testing Platform aims to replace those with a new engine that is modernized, streamlined, performs better, and offers much greater extensibility for test framework authors.

Test projects for Microsoft Testing Platform are standalone executables. When a Microsoft Testing Platform test project is compiled, it can then be run directly (typically by invoking the already built executable, or using `dotnet run` to both build and run). This allows for a streamlined experience where the produced executable is all that's needed to run the tests.

When using the `YTest.MTP.XUnit2` package, running your executable will run your tests in Microsoft Testing Platform mode:

```shell
$ dotnet run
YTest.MTP.XUnit2 Runner version 1.0.0+e77a8357f5ea91b148d2f7ba8091d30ea45c5d81 (64-bit .NET 8.0.22)

Test run summary: Passed! - path/to/test-project.dll (net8.0|x64)
  total: 1
  failed: 0
  succeeded: 1
  skipped: 0
  duration: 56ms
```

(Make sure you've added `<OutputType>Exe</OutputType>` to your project properties if you want to run your test project directly.)

Now your test project can run anywhere that Microsoft Testing Platform is supported. This includes `dotnet test` (in MTP mode), Test Explorer, JetBrains Rider, and more!

If you no longer need VSTest support after adding MTP support, you can safely remove the package references to `xunit.runner.visualstudio` and `Microsoft.NET.Test.Sdk`.
