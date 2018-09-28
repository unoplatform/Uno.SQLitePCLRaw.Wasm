# Uno Platform based SQLitePCLRaw provider for WebAssembly

This repository is the home for the SQLitePCLRaw provider for WebAssembly, providing SQLite support for [EntityFramework Core](https://docs.microsoft.com/en-us/ef/core/), [SQLite-net](https://github.com/praeclarum/sqlite-net), or any [SQLitePCLRaw-based](https://github.com/ericsink/SQLitePCL.raw) application.

The package is using [Uno.sqlite-wasm](https://github.com/nventive/Uno.sqlite-wasm), a WebAssembly built binary for the [SQLite](https://sqlite.org) database engine.

You can see it in action here: http://sqliteefcore-wasm.platform.uno/

The Nuget package is available here: [Uno.SQLitePCLRaw.provider.wasm](https://www.nuget.org/packages/Uno.SQLitePCLRaw.provider.wasm)

## Architecture

This package is providing an implementation for the `SQLitePCL.ISQLite3Provider` interface used by many database
abstractions, such as [SQLite-net](https://github.com/praeclarum/sqlite-net) or [EntityFramework Core](https://docs.microsoft.com/en-us/ef/core/).

It can be used as follows, early in the startup of an application:

```csharp
SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_WebAssembly());
```

For the time being, this package requires the use of the [Uno.Wasm.Boostrap](https://github.com/nventive/Uno.Wasm.Bootstrap) nuget package to function properly, until [this Nuget issue is resolved](https://github.com/NuGet/NuGet.Client/pull/2159).

## EFCoreSample

This sample demonstates the use of the SQLitePCLRaw provider for WebAssembly, along with EntityFramework Core and Roslyn.

The application is built with all the EntityFramework Core binaries, allowing for custom code to be run in the browser, to test EF Core database scenarios dynamically.