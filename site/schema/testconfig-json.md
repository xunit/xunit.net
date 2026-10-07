---
title: JSON Schema for testconfig.json
---

# JSON Schema for `testconfig.json`

This page includes a complete list of schema versions that have been published.

The [configuration file](/docs/config-testconfig-json) documentation indicates which versions of the Core Framework supports which version of the schema. In general, the schema versions are additive, so using a newer configuration file with a test project targeting an older version of the Core Framework _should_ result in the test project simply ignoring any newly added configuration entries. If you are finding that some configuration items aren't being respected by your test project, once you've verified name and value validity against the schema, you may need to upgrade your Core Framework to take advantage of newer configuration items.

> [!NOTE]
> Using `testconfig.json` is only supported when running tests in Microsoft Testing Platform mode. Running tests any other way (including using our first party runners or any non-Microsoft Testing Platform third party runner) does not support `testconfig.json`, and you should rely on [xUnit.net's native JSON configuration files](/docs/config-xunit-runner-json) instead. For more information about v3 and Microsoft Testing Platform, see [our documentation page](/docs/getting-started/v3/microsoft-testing-platform).
>
> Only use this configuration file if:
>
> * You only run tests in Microsoft Testing Platform mode, or
>
> * You wish to set configuration values that are only applicable when running in Microsoft Testing Platform mode.

The primary recommended URL is [https://xunit.net/schema/current/xunit.testconfig.schema.json](current/xunit.testconfig.schema.json), which always points to the latest RTM version.

> [!IMPORTANT]
> Any prerelease version is subject to change.

## Schema versions

[current](current/xunit.testconfig.schema.json){: .release }
[4.0.2](v4.0.2/xunit.testconfig.schema.json){: .release }
