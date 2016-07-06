LINQ To AQL
===========

LINQ to AQL is a .NET client library and LINQ provider for [AsterixDB](http://asterixdb.apache.org/).

LINQ to AQL supports querying AsterixDB by generating [AQL](http://asterixdb.apache.org/docs/0.8.8-incubating/aql/manual.html) (Asterix Query Language) expressions.

Dev Setup
---------
Get the [.NET Core](https://www.microsoft.com/net/core) SDK for your platform.

There should be no warnings and no test failures in the following steps.

### Build

`dotnet restore` then `dotnet build` in `src/LinqToAql` (build output is `src/LinqToAql/bin` by default).

### Test

`dotnet restore` in `test/LinqToAql.Tests.Common`

Unit Tests: `dotnet restore` then `dotnet test` in `test/LinqToAql.Tests.Unit`.

Integration Tests: `dotnet restore` then `dotnet test` in `test/LinqToAql.Tests.Integration`.


Planned Work
------------

* Improve GroupBy support
* More AQL function support
* UDF support
* Add trace logs so users can (if enabled) see executed AQL queries with IntelliTrace or a custom writer
* Generate model classes given a dataverse
* Parameterized queries (when AsterixDB supports them)
