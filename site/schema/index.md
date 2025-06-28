---
title: JSON Schemas
---

# JSON Schemas

This page includes a complete list of schema versions that have been published.

The [configuration files](/docs/configuration-files) documentation indicates which versions of the Core Framework and associated first party runners support which version of the schema. In general, the schema versions are additive, so passing a newer configuration file to a runner which only understands an older version of the schema _should_ result in the runner simply ignoring any newly added configuration entries. If you are finding that some configuration items aren't being respected by a first or third party runner, once you've verified name and value validity against the schema, you may need to upgrade to a newer runner to take advantage of newer configuration items.

The primary recommended URL is [https://xunit.net/schema/current/xunit.runner.schema.json](current/xunit.runner.schema.json), which always points to the latest RTM version.

> [!IMPORTANT]
> Any prerelease version is subject to change.

## v3

[current](current/xunit.runner.schema.json){: .release }
[3.1](v3.1/xunit.runner.schema.json){: .release }
[3.0](v3.0/xunit.runner.schema.json){: .release }
[3.0-alpha-1](v3.0-alpha-1/xunit.runner.schema.json){: .prerelease }

## v2

[2.8](v2.8/xunit.runner.schema.json){: .release }
[2.5](v2.5/xunit.runner.schema.json){: .release }
[2.4](v2.4/xunit.runner.schema.json){: .release }
[2.3](v2.3/xunit.runner.schema.json){: .release }
[2.2](v2.2/xunit.runner.schema.json){: .release }
[2.1-rc1](v2.1-rc1/xunit.runner.schema.json){: .prerelease }

## v1

[1.9](v1/xunit.runner.schema.json){: .release }
