# Uno Platform based SQLitePCLRaw provider for WebAssembly

This repository is the home for the SQLitePCLRaw provider for WebAssembly.

The package is based on [Uno.sqlite-wasm](https://github.com/nventive/Uno.sqlite-wasm), a 
WebAssembly built binary or the [SQLite](https://sqlite.org) database engine.

This package is providing an implementation for `SQLitePCL.ISQLite3Provider` used by many database
abstractions, such as [SQLite-net](https://github.com/praeclarum/sqlite-net) or [EntityFramework Core](https://docs.microsoft.com/en-us/ef/core/).

For the time being, this package requires the use of the [Uno.Wasm.Boostrap](https://github.com/nventive/Uno.Wasm.Bootstrap) nuget package.

You can see it in action here: http://sqliteefcore-wasm.platform.uno/

The Nuget package is available here: [Uno.SQLitePCLRaw.provider.wasm](https://www.nuget.org/packages/Uno.SQLitePCLRaw.provider.wasm)

## EFCoreSample

This sample demonstates the use of the SQLitePCLRaw provider for WebAssembly, along with EntityFramework Core and Roslyn.

The application is built with all the EntityFramework Core binaries, allowing for custom code to be run in the browser, to test EF Core database scenarios dynamically.