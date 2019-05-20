# New Crawler Migration Notes

These notes are intended to help anyone migrating a crawler out of the [CluedIn](https://github.com/CluedIn-io/CluedIn) repository in to it's own repository.

What you will get:

* Azure CI for your new crawler repository
* Automated generation of NuGet packages on the CluedIn internal _develop_ and _release_ feeds.
* Latest tooling for internal dependency management via NuGet

## How to use these Notes

The notes were originally made when migrating the [CluedIn.Crawling.SqlServer](https://github.com/CluedIn-io/CluedIn.Crawling.SqlServer) codebase to a self contained repository.

__NB:__ All examples are provided in the context of creating a `CluedIn.Crawling.SqlServer` crawler. Use appropriate values for the crawler that you are creating.

## Creating a Git Repository

Perform the following tasks:

1. Create new repository `CluedIn.Crawling.SqlServer` in [CluedIn Git](https://github.com/CluedIn-io) organization

1. Clone the new repository to your development environment

   ```Shell
   git clone https://github.com/CluedIn-io/CluedIn.Crawling.SqlServer.git
   ```

1. Change directory to the cloned repository

   ```Shell
   cd CluedIn.Crawling.SqlServer
   ```

1. Initialize [Git Flow](https://nvie.com/posts/a-successful-git-branching-model/) branching model

   ```Shell
   git flow init -d
   ```

1. Push the _develop_ branch to _origin_

   ```Shell
   git push --set-upstream origin develop
   ```

## Migrating CluedIn Repository Code

Perform the following tasks:

1. Checkout a _feature_ branch to work in

   ```Shell
   git checkout -b feature/Migrate-CluedIn-Repository-Code
   ```

1. Run the [Yeoman Crawler Generator](https://github.com/CluedIn-io/yeoman-crawler-template) to create a skeleton solution for the new crawler

  Before running this you will need to know whether the crawler is to support the following features:

    * [Webhooks](https://en.wikipedia.org/wiki/Webhook)
    * [OAuth](https://oauth.net/)

   ```Shell
   docker run --rm -ti -v ${PWD}:/generated cluedin/generator-crawler-template
   ```

   Answer to crawler generator prompts:

    * `SqlServer` to Crawler Name
    * Yes to Web Hooks
    * No to OAuth

1. Commit the output of the crawler generator to you development branch

   ```Shell
   git add .
   git commit -m "docker run --rm -ti -v ${PWD}:/generated cluedin/generator-crawler-template"
   ```

1. Remove the redundant solution file for the crawler in the `src` folder

   ```Shell
   del .\src\Crawling.SqlServer.sln
   git commit -am "Remove the redundant solution file for the crawler in the src folder"
   ```

1. Remove crawler generated code and configuration files

   ```PowerShell
   Get-ChildItem . *.cs -Recurse -File | Remove-Item
   Get-ChildItem . App.config -Recurse -File | Remove-Item
   ```

1. Copy crawler code from `CluedIn` repository to new crawler repository

   Migrate contents of folders:

   * `Code\Crawlers\Crawler.SqlServer` to `src` recursively

   * `Code\Crawlers\Crawler.SqlServer\test\unit-test` to `test\unit-test` recursively

   * `code\Crawlers\Crawler.SqlServer\test\newman` to `test\newman`

   * `Code\Crawlers\Crawler.SqlServer\SqlServer.Crawling.Console` to `test\integration\Tests.Integration.SqlServer`

At this stage the following commands fail:

```Shell
# .NET CLI build
dotnet build

# MSBuild
 C:\windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe

# NuGet pack of sample project
NuGet.exe pack .\src\SqlServer.Core\
```

This is my git log for commits to resolve these failures.

```Text
a8e74a8 Branching strategy documented
848360e Migrating CluedIn repos code notes added
4fe08a0 docker run --rm -ti -v C:\Users\kevin\CluedIn.Crawling.SqlServer:/generated cluedin/generator-crawler-template
999b040 Remove the redundant solution file for the crawler in the src folder
16e806f Copy crawler code from CluedIn repository
deea08d newman tests migrated
d1f6823 Copy crawler code from `CluedIn` repository to new crawler repository
cd0f195 gci . *.csproj -recurse | ForEach-Object { dotnet migrate-2017 migrate $_.FullName } down to 6 dotnet build errors
01e4843 Consolidate Company property
0c3b332 Consolidate Copyright property
14417bb Add acceptance tests Add https://github.com/CluedIn-io/CluedIn.Crawling.MySql/blob/feature/Migrate-CluedIn-Repository-Code/test/acceptance/Project.Tests.ps1
cb387d4 VS build and dotnet build succeed
0bfecd0 HintPath test fixes
3e52f05 OutputPath test fixes Invoke-Pester now passes
cc81e6a Add acceptance test azure CI task
```

When you are done push your commits to _origin_

```Shell
git push --set-upstream origin feature/Migrate-CluedIn-Repository-Code
```

and start a Pull Request on the crawler repository

```PowerShell
start https://github.com/CluedIn-io/CluedIn.Crawling.SqlServer/pull/new/feature/Migrate-CluedIn-Repository-Code
```