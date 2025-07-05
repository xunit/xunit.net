---
title: Using CI Builds
---

# Using CI Builds

## Is it safe to use CI builds?{ #safety }

In general, our pre-release builds and release builds have the same quality level. We do not have a separate phase of "stabilizing" the code; each push to the source repository (and thus each CI build) is intended to be RTW-level quality. On very rare occasions we will need to produce CI builds that don't meet this bar, but that is extremely rare and usually very short lived. If we recommend you try a CI build, it's because we believe it to be of RTW-level quality.

Does that mean they won't have bugs? No. But that's also true for RTW builds. 😄 Typically speaking, if we're recommending you use a CI build, it's because it fixes a bug that you're affected by in an RTW build.

You should feel confident to use CI builds, especially ones that we recommend.

## How to consume CI builds{ #consume }

The [packages table on the home page](/#packages) lists the currently available builds. The column titled "Latest CI" indicates the version of the most recent build that was pushed automatically as a result of new code being pushed to GitHub. These automated builds are currently sent to [feedz.io](https://feedz.io/org/xunit/repository/xunit/search) (that link will require you to log into your feedz.io account, which is free if you don't already have one).

In order to consume packages from this feed, you need to create or update the `NuGet.Config` file for your project. Here is an example:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org"
         value="https://api.nuget.org/v3/index.json"
         protocolVersion="3" />
    <add key="feedz.io/xunit/xunit"
         value="https://f.feedz.io/xunit/xunit/nuget/index.json"
         protocolVersion="3" />
  </packageSources>
</configuration>
```

This `NuGet.Config` file sets two feed sources: the official source from NuGet, and the feed from `feedz.io`. (Note that while you need an account to view the `feedz.io` website, you do not need an account to restore packages, so using this `NuGet.Config` should be relatively friction free.) We recommend that you place the `NuGet.Config` file into the top level folder of your source repository so that it will be used for all package operations in your project.

> [!NOTE]
> The order of the package sources matters: the first listed source (`nuget.org` in this case) has the highest priority. That means if the package is available from `nuget.org` with the version you requested, it will be sourced from there, even if the same version is available on a later package feed.

## Can I use newer individual dependencies?{ #newer }

A common question people ask when a CI package is available is whether they can use that package, when it's a dependency of the core `xunit` package.

If you look at the [dependencies](https://www.nuget.org/packages/xunit#dependencies-body-tab) list for `xunit`, you'll notice that both `xunit.assert` and `xunit.analyzers` are listed with a `>=` designation. This means that if you don't have an explicit reference to either of those packages, you'll get the version shown on that page:

![](/images/xunit-dependencies.png){ .border width=876 }

However, any version that is that _or newer_ satisfies the requirement. So in this example, I could pull in a newer `xunit.analyzers` reference simply by adding it with the version I want, like this:

```xml
<ItemGroup>
  <PackageReference Include="xunit" Version="2.6.3" />
  <PackageReference Include="xunit.analyzers" Version="1.8.0-pre.2" />
</ItemGroup>
```

This allows you to consume a CI build of just one dependency while leaving yourself on RTW versions of the other packages. This is also not just limited to CI packages, because from time to time we will release newer analyzer packages without an associated update to the core `xunit` package, and this would allow you to pull in a newer RTW analyzer package.

## Can I use older individual dependencies?{ #older }

This question occasionally comes up as well. Users will sometimes be affected by a bug in an existing release and want to roll back, or may choose to stay with older functionality (temporarily or permanently) due to breaking changes.

Using something older is possible, but not as simply. Since the dependencies are specified as `>=`, an older version would fail, like this:

```text
error NU1605: Warning As Error: Detected package downgrade: xunit.analyzers from 1.7.0 to 1.6.0. Reference the package directly from the project to select a different version.
error NU1605:  xunit 2.6.3 -> xunit.analyzers (>= 1.7.0)
error NU1605:  xunit.analyzers (>= 1.6.0)
```

The way to do this is to stop using the `xunit` (or `xunit.v3`) package and instead specify all the dependencies directly. So the correct way to this would be like this:

```xml
<ItemGroup>
  <PackageReference Include="xunit.core" Version="2.6.3" />
  <PackageReference Include="xunit.analyzers" Version="1.6.0" />
  <PackageReference Include="xunit.assert" Version="2.6.3" />
</ItemGroup>
```

Here we've taken the older analyzers and are using them with the newer framework. We could even combine this with an older assertion library as well:

```xml
<ItemGroup>
  <PackageReference Include="xunit.core" Version="2.6.3" />
  <PackageReference Include="xunit.analyzers" Version="1.6.0" />
  <PackageReference Include="xunit.assert" Version="2.4.1" />
</ItemGroup>
```
