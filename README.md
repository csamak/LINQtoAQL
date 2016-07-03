LINQ To AQL
===========

LINQ to AQL is a .NET client library and LINQ provider for [AsterixDB](http://asterixdb.apache.org/).

LINQ to AQL supports querying AsterixDB by generating [AQL](http://asterixdb.apache.org/docs/0.8.8-incubating/aql/manual.html) (Asterix Query Language) expressions.

The project is currently pre-release. Later, this document will contain usage instructions and examples. For now, more fine-grained documentation is available on the project's [wiki](https://github.com/csamak/LINQToAQL/wiki).

Dev Setup
---------
Get the [.NET Core](https://www.microsoft.com/net/core) SDK for your platform.


### Build

`dotnet build` in `src/LinqToAql` (build output is `src/LinqToAql/bin` by default).

### Test

Unit Tests: `dotnet test` in `test/LinqToAql.Tests.Unit`.
Integration Tests: `dotnet test` in `test/LinqToAql.Tests.Integration`.


Planned Work
------------

* Improve GroupBy support
* More AQL function support
* UDF support
* Add trace logs so users can (if enabled) see executed AQL queries with IntelliTrace or a custom writer
* Generate model classes given a dataverse
* Parameterized queries (when AsterixDB supports them)
