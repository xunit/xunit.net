# About This Project

This project contains the public site for [https://xunit.net/](https://xunit.net/).

To open an issue for this project, please visit the [core xUnit.net project issue tracker](https://github.com/xunit/xunit/issues).

## Building and Contribution

This site is built with DocFX. We use a C#-based build system.

### Build prerequisites

In order to successfully build and view the content locally, you will need the following pre-requisites:

* [.NET SDK 9 or later](https://dotnet.microsoft.com/download/dotnet/)

If you wish to use `./build serve` for live regeneration during development, you will also need:

* [NodeJS 22 or later](https://nodejs.org/) (for the live rebuild monitoring)
* [Docker](https://docs.docker.com/engine/install/) (to host nginx)

### Editing the site pages

The site content lives in [`/site`](https://github.com/xunit/xunit.net/tree/main/site) and the output will placed in `/.site`. To build the static content, run the command `./build` from the root of your clone, via your command prompt (bash and PowerShell are supported). This will do a one-time transformation of the templates in `/site` to the static files in `/.site`.

For working interactively, you can run `./build serve` which will use NodeJS to run in a mode where it simultaneously builds the site and uses a Docker-based nginx server to serve the static content. It will automatically rebuild the site as you change the site's content files. You can point your browser to [http://127.0.0.1:4000/](http://127.0.0.1:4000/) while the server is running to view the rendered content. Once you're finished, you can press Ctrl+C in the command prompt and the server will shut down. (You may need to press Ctrl+C more than once on Windows.)

Text editors/IDEs which understand site hierarchy and linking while editing Markdown and HTML are strongly encouraged to open the `/site` folder and not the root folder when editing content, so that the editor understands where the content root lives. _For example, if you're using VSCode, you should run `code site` and not `code .` from the root of the repo._ You should only ever need to open the root of the repo in your editor if you're working on the build tools.

# About xUnit.net

xUnit.net is a free, open source, community-focused unit testing tool for C#, F#, and Visual Basic.

xUnit.net works with the [.NET SDK](https://dotnet.microsoft.com/download) command line tools, [Visual Studio](https://visualstudio.microsoft.com/), [Visual Studio Code](https://code.visualstudio.com/), [JetBrains Rider](https://www.jetbrains.com/rider/), [NCrunch](https://www.ncrunch.net/), and any development environment compatible with [Microsoft Testing Platform](https://learn.microsoft.com/dotnet/core/testing/microsoft-testing-platform-intro) (xUnit.net v3) or [VSTest](https://github.com/microsoft/vstest) (all versions of xUnit.net).

xUnit.net is part of the [.NET Foundation](https://www.dotnetfoundation.org/) and operates under their [code of conduct](https://www.dotnetfoundation.org/code-of-conduct). It is licensed under [Apache 2](https://opensource.org/licenses/Apache-2.0) (an OSI approved license). The project is [governed](https://xunit.net/governance) by a Project Lead.

For project documentation, please visit the [xUnit.net project home](https://xunit.net/).

* _New to xUnit.net? Get started with the [.NET SDK](https://xunit.net/docs/getting-started/v3/getting-started)._
* _Need some help building the source? See [BUILDING.md](https://github.com/xunit/xunit/tree/main/BUILDING.md)._
* _Want to contribute to the project? See [CONTRIBUTING.md](https://github.com/xunit/.github/tree/main/CONTRIBUTING.md)._
* _Want to contribute to the assertion library? See the [suggested contribution workflow](https://github.com/xunit/assert.xunit/tree/main/README.md#suggested-contribution-workflow) in the assertion library project, as it is slightly more complex due to code being spread across two GitHub repositories._

## Latest Builds

|                             | Latest stable                                                                                                                            | Latest CI ([how to use](https://xunit.net/docs/using-ci-builds))                                                                                                                                                              | Build status
| --------------------------- | ---------------------------------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------
| `xunit.v3`                  | [![](https://img.shields.io/nuget/v/xunit.v3.svg?logo=nuget)](https://www.nuget.org/packages/xunit.v3)                                   | [![](https://img.shields.io/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.v3/latest&logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.v3)                                   | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/xunit/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/xunit/goto?ref=main)
| `xunit`                     | [![](https://img.shields.io/nuget/v/xunit.svg?logo=nuget)](https://www.nuget.org/packages/xunit)                                         | [![](https://img.shields.io/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit/latest&logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit)                                         | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/xunit/badge%3Fref%3Dv2&amp;label=build)](https://actions-badge.atrox.dev/xunit/xunit/goto?ref=v2)
| `xunit.analyzers`           | [![](https://img.shields.io/nuget/v/xunit.analyzers.svg?logo=nuget)](https://www.nuget.org/packages/xunit.analyzers)                     | [![](https://img.shields.io/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.analyzers/latest&logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.analyzers)                     | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/xunit.analyzers/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/xunit.analyzers/goto?ref=main)
| `xunit.runner.visualstudio` | [![](https://img.shields.io/nuget/v/xunit.runner.visualstudio.svg?logo=nuget)](https://www.nuget.org/packages/xunit.runner.visualstudio) | [![](https://img.shields.io/endpoint.svg?url=https://f.feedz.io/xunit/xunit/shield/xunit.runner.visualstudio/latest&logo=nuget&color=f58142)](https://feedz.io/org/xunit/repository/xunit/packages/xunit.runner.visualstudio) | [![](https://img.shields.io/endpoint.svg?url=https://actions-badge.atrox.dev/xunit/visualstudio.xunit/badge%3Fref%3Dmain&amp;label=build)](https://actions-badge.atrox.dev/xunit/visualstudio.xunit/goto?ref=main)

*For complete CI package lists, please visit the [feedz.io package search](https://feedz.io/org/xunit/repository/xunit/search). A free login is required.*

## Sponsors

Help support this project by becoming a sponsor through [GitHub Sponsors](https://github.com/sponsors/xunit).
