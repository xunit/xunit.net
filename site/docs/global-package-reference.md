---
title: GlobalPackageReference and xUnit.net
---

# GlobalPackageReference and xUnit.net

Global package references are a part of [Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management#global-package-references) wherein you can specify that a package is used by every project in a solution.

As described in the documentation:

> A global package reference is used to specify that a package will be used by every project in a repository. This includes packages that do versioning, extend your build, or any other packages that are needed by all projects.

In essence, this feature is designed to be used for packages which _do some work during your build_, rather than packages that _have code that you want referenced in every project_. Even if for some reason you have a solution which would want xUnit.net referenced in every project, this still isn't an appropriate feature.

Again, according to the documentation:

> Global package references are added to the PackageReference item group with the following metadata:
>
> * `IncludeAssets="Runtime;Build;Native;contentFiles;Analyzers"`<br />
  This ensures that the package is only used as a development dependency and prevents it from being included as a compile-time assembly reference.
>
> * `PrivateAssets="All"`<br />
> This prevents global package references from being picked up by downstream dependencies.

What's notably missing in that `IncludeAssets` list is `Compile`, which is what lets you link against the libraries in the assembly.

The `GlobalPackageReference` feature was clearly designed to explicit exclude the idea of using it for any code related NuGet packages. This makes it unusable with xUnit.net, since you need code provided by the NuGet packages (assertions, attributes, etc.).
