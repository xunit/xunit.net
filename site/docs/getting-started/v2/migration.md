---
title: Migrating Unit Tests from v1 to v2
---

# Migrating Unit Tests from v1 to v2

The xUnit.net team tried to ensure that migration of unit tests from v1 to v2 would be as painless as possible. Most of the migration tasks should be fairly straightforward and mechanical (replacing NuGet packages, doing simple search & replace, etc.).

## Update the xUnit.net binaries{ #update-binaries }

Binaries for xUnit.net are now distributed exclusively through NuGet. Updating the binaries differs based on whether you originally used CodePlex or NuGet to acquire them. Please choose from one of the two options below.

### If you installed xUnit.net v1 via CodePlex{ #v1-codeplex }

1. You will need to manually remove any references to `xunit.dll` and/or `xunit.extensions.dll`.

2. Then, add the new `xunit` NuGet package. Start by right clicking on the project in Solution Explorer, and then choosing the `Manage NuGet Packages...` menu item:

  ![](/images/manage-nuget-packages.png){ .border width=357 }

3. Click on `Browse` in the upper left corner. In the search box on the upper right, type `xunit`. The search should yield results like this:

  ![](/images/add-xunit.png){ .border width=845 }

4. Locate the xUnit.net entry, and click `Install`.

### If you installed xUnit.net v1 via NuGet{ #v1-nuget }

1. Right click on the project in Solution Explorer, and then choose the `Manage NuGet Packages...` menu item:

  ![](/images/manage-nuget-packages.png){ .border width=357 }

1. Click on `Installed` along the top. If you see `xUnit.net: Extensions` installed, please click the `Uninstall` button. (If NuGet offers to uninstall the `xunit` package for you, you should decline. You're going to upgrade that package in the next step.)

  ![](/images/test-migration/uninstall-extensions.png){ .border width=982 }

1. Click on `Updates` along the top. Locate `xUnit.net` in the list of packages, and click `Update`:

  ![](/images/test-migration/update-xunit.png){ .border width=981 }

## Update the unit tests{ #update-tests }

Build your solution. If everything compiles, then you're done!

If it doesn't compile, here are some of the things that may need to upgraded by hand:

* **Compiler error:** `'Xunit.Extensions.PropertyDataAttribute' is obsolete: 'Please replace [PropertyData] with [MemberData]'`

  If you get this compiler error, change all instances of `[PropertyData]` to `[MemberData]`. The new `MemberDataAttribute` class can read data from static properties (just like `PropertyDataAttribute`), but now also supports data from static fields and static methods. You can even provide parameter values to static methods!

* **Compiler error:** `The type or namespace name 'IUseFixture<T>' could not be found (are you missing a using directive or an assembly reference?)`

  The interface has been renamed, and its behavior is slightly different now.

  1. Change from `IUseFixture` to `IClassFixture`.

  1. Remove the `SetFixture` method. If you need access to your fixture object, you can accept it as a constructor argument instead.

  See [Sharing Context between Tests](/docs/shared-context#class-fixture) for more information about `IClassFixture`.
