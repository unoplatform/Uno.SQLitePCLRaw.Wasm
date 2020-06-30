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

That's it !

## Architecture

This package is providing the SQLite implementation used by many database abstractions, such as [SQLite-net](https://github.com/praeclarum/sqlite-net) or [EntityFramework Core](https://docs.microsoft.com/en-us/ef/core/).

The SQLite implementation is provided under the covers as a statically linkable bitcode file, and requires the use of the [Uno.Wasm.Boostrap](https://github.com/nventive/Uno.Wasm.Bootstrap).

## EFCoreSample

This sample demonstates the use of the SQLitePCLRaw provider for WebAssembly, along with EntityFramework Core and Roslyn using [Uno Platform](https://platform.uno).

The application is built with all the EntityFramework Core binaries, allowing for custom code to be compiled and run locally in the browser, to test EF Core database scenarios dynamically.