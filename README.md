# Uno Platform based SQLitePCLRaw provider for WebAssembly

This repository is the home for the SQLitePCLRaw provider for WebAssembly, providing SQLite support for [EntityFramework Core](https://docs.microsoft.com/en-us/ef/core/), [SQLite-net](https://github.com/praeclarum/sqlite-net), or any [SQLitePCLRaw-based](https://github.com/ericsink/SQLitePCL.raw) library.

The package is using [Uno.sqlite-wasm](https://github.com/nventive/Uno.sqlite-wasm), a WebAssembly built binary for the [SQLite](https://sqlite.org) database engine.

You can see it in action here: http://sqliteefcore-wasm.platform.uno/

The Nuget package is available here: [Uno.SQLitePCLRaw.provider.wasm](https://www.nuget.org/packages/Uno.SQLitePCLRaw.provider.wasm)

## Usage

- Add a Nuget package reference to [`Uno.SQLitePCLRaw.provider.wasm`](https://www.nuget.org/packages/Uno.SQLitePCLRaw.provider.wasm)
- Add the following initialization line, early in the startup of an application:

```csharp
SQLitePCL.Batteries.Init();
```

## Usage With sqlite-net-pcl

When using the SQLitePCL.Raw with sqlite-net-pcl, you'll need additional configuration:

```xml
<PackageReference Include="sqlite-net-pcl" Version="1.7.335" />
<PackageReference Include="Uno.SQLitePCLRaw.provider.wasm" Version="3.0.15" />
<PackageReference Include="SQLitePCLRaw.bundle_green" Version="2.0.5-pre20210521085756" IncludeAssets="none" />
```

The `SQLitePCLRaw.bundle_green` must be exlcuded explicitly as it does not work properly with WebAssembly, but cannot be removed as it is a transitive dependency of sqlite-net-pcl. Adding this last reference ensure that the proper native library is loaded.

## Architecture

This package is providing the SQLite implementation used by many database abstractions, such as [SQLite-net](https://github.com/praeclarum/sqlite-net) or [EntityFramework Core](https://docs.microsoft.com/en-us/ef/core/).

The SQLite implementation is provided under the covers as a statically linkable bitcode file, and requires the use of the [Uno.Wasm.Boostrap](https://github.com/nventive/Uno.Wasm.Bootstrap).

## EFCoreSample

This sample demonstates the use of the SQLitePCLRaw provider for WebAssembly, along with EntityFramework Core and Roslyn using [Uno Platform](https://platform.uno).

The application is built with all the EntityFramework Core binaries, allowing for custom code to be compiled and run locally in the browser, to test EF Core database scenarios dynamically.

## Special considerations for Windows build servers when using Uno.Bootstrapper 2.x

If you're building a WebAssembly application with the `Uno.SQLitePCLRaw.provider.wasm` package on a Windows CI Server, you may get into an error like this one:

```
Error : System.InvalidOperationException: WSL is required for this build but could not be found (Searched for [C:\windows\sysnative\bash.exe]).
```

If your CI server does not have WSL enabled (e.g. Azure Devops Hosted Agents), you'll need to disable the static linking of the SQLite native library. It will generate an invalid package, but the build will finish properly. 

> Note that this restriction is temporary until msbuild supports solution filters (most likely VS 16.7+), where removing some projects from a solution will be lots easier.

Here's how to the static linking of the SQLite native library. In your `XX.Wasm.csproj` file, add the following:

```xml
<PropertyGroup>
	<CanUseAOT Condition="$([MSBuild]::IsOsPlatform('Linux')) or ( $([MSBuild]::IsOsPlatform('Windows')) and '$(BUILD_REPOSITORY_PROVIDER)'=='' )">true</CanUseAOT>
</PropertyGroup>

<ItemGroup Condition="'$(CanUseAOT)'==''">
	<PackageReference Include="Uno.sqlite-wasm" Version="1.1.0-dev.16828" IncludeAssets="none" />
</ItemGroup>
```
