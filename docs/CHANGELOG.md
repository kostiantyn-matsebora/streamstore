# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog],
and this project adheres to [Semantic Versioning].

## [0.22.0] - 2025-06-11

### Fixed

- Fixed references to projects in the solution file.
  
## [0.21.0] - 2025-06-11

### Changed

- Reimplement DI container registration for StreamStore and storage implementations.

## [0.20.0] - 2025-05-31

### Changed

- Separate storage and serialization contracts into separate projects.

## [0.19.0] - 2025-05-30

### Changed

- Updated dependencies to the latest versions.

## [0.18.0] - 2025-05-29

### Added

- Added support of custom event properties.
  
### Changed

- Switched to FluentMigrator for SQL-based database migrations.
- Switched to Cassandra.FluentMigrator for Cassandra database migrations.

### Fixed

- Fixed issue with storing stream metadata in S3 storages.
  
## [0.17.0] - 2025-05-25

### Added

- Added support for custom storage implementations via `StreamStore.Storage` package.

### Changed

- Moved all consistency logic from `IStreamStorage` to `IStreamUnitOfWork`.
- Added getting stream metadata to `IStreamStore`.

### Fixed

- Cassandra bug with not registered 'ICluster' in DI container.
- Fixed multiple issues with example applications.

## [0.16.0] - 2025-04-04

### Added

- Added StreamStore.Storage package to support custom storage implementations.

### Changed

- Renamed 'Database' to 'Storage' in all packages and namespaces.

## [0.15.0] - 2025-03-14

### Changed

- Changed default page size to 1000
- Updated example application, beautify output

### Fixed

- Fixed issue when page size was not taken into account when reading events from the stream

### Added

- Added editor config file

## [0.14.0] - 2024-11-29

### Added

- Support of Azure CosmosDB for Apache Cassandra

### Changed

- Minor improvement and fixes

## [0.13.0] - 2024-11-25

### Added

- Added Apache Cassandra support.

### Changed

- Optimized database implementation by getting rid of metadata retrieval for each event and replacing it with getting max revision for the stream.

## [0.12.0] - 2024-11-15

### Fixed

- Fixed typo in ConfigureSingleDatabase method name.

## [0.11.0] - 2024-11-15

### Added

- Multitenancy support for SQL backends.

### Changed

- Unified configuration of storage by separating concerns and creating separate implementation for database backends, serialization and storage configurations.
  
## [0.10.0] - 2024-11-11

### Added

- Added support for PostgreSQL backend.

### Fixed

- Fixed bug in extensions when AggregateException was thrown instead of the original exception.

## [0.9.0] - 2024-11-09

### Added

- Added fluent API for appending events to the stream.

## [0.8.0] - 2024-11-08

### Changed

- Changed reading of events, now it is done asynchronously, and implemented as `IAsyncEnumerable`.
- Changed the way how unit tests are built, now they are scenario-based, using BDD approach.

## [0.7.0] - 2024-10-20

### Added

- Added support for SQLite backend.

## [0.6.0] - 2024-10-18

### Changed

- Serialization is changed the way to support binary serialization.

### Added

- Added protobuf-net library based serializer to support binary serialization.
- Added benchmarks for serialization and in-memory store.
- Added optional compression for event serialization.

## [0.5.0]

### Changed

- Changed public interfaces signatures to asynchronous methods.

### Added

- Added Revision value object.

## [0.4.0] - 2024-10-08

### Changed

- Changed signature of IStreamUnitOfWork interface, so now it calculates stream revision automatically.
- Changed persistence of S3 database backend, now persistent and transient data are fully separated.
  
### Added

- Added test for IStreamUnitOfWork implementations.

## [0.3.0] - 2024-10-08

### Added

- Add support of Backblaze B2 storage backend.
- Add testing framework for StreamStore.

### Changed

- Signatures of IStreamStore and IStreamDatabase interfaces, as well implementations.
- Documentation, added instructions how to use Backblaze B2 storage backend.
  
## [0.2.2] - 2024-10-01

### Fixed

- Fixed code smells and warnings reported by SonarCloud.
- Convert EventEntities field to readonly property.
- Add different kind of badges to README.md

## [0.2.1] - 2024-10-01

### Fixed

- Fixed code smells and warnings reported by SonarCloud.

## [0.2.0] - 2024-09-30

### Fixed

- Fixed bug in the StreamStore class, when converter was using default event serializer instead of the provided one.

### Changed

- Change the way how events are serialized/deserialized, now it is using converter instead of event serializer.

## [0.1.2] - 2024-09-30

### Added

- Github action workflow to build, test and publish the package to nuget.org.
- Additional short README.md for nuget.org.

### Changed

- Documentation, added instructions how to install the package.
  
## [0.1.0] - 2024-09-30

- Initial implementation

<!-- Links -->
[keep a changelog]: https://keepachangelog.com/en/1.0.0/
[semantic versioning]: https://semver.org/spec/v2.0.0.html
