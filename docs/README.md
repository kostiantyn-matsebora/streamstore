# StreamStore 
[![StreamStore](https://github.com/kostiantyn-matsebora/streamstore/actions/workflows/streamstore.yml/badge.svg)](https://github.com/kostiantyn-matsebora/streamstore/actions/workflows/streamstore.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=kostiantyn-matsebora_streamstore&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=kostiantyn-matsebora_streamstore)
[![NuGet version (StreamStore)](https://img.shields.io/nuget/v/StreamStore.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore/)

A lightweight library provides a logical layer for storing and querying events as a stream.

Heavily inspired by Greg Young's Event Store and [`Streamstone`](https://github.com/yevhen/Streamstone) solutions.

## Overview

The library itself does not provide any production-ready database storage implementation _yet_, but it is designed to be easily extended with custom database backends.

[In-memory][InMemoryStreamDatabase.cs] database implementation is provided **for testing and educational purposes only**.

## Features

The general idea is to highlight the general characteristics and features of event sourcing storage:

- [x] Event ordering.
- [x] Serialization/deserialization of events.
- [x] Optimistic concurrency control.
- [x] Event duplication detection based on event ID.
- [ ] Database agnostic test framework, including benchmarking test scenarios.
- [ ] Binary serialization/deserialization of events.
- [ ] Custom event properties (?).
- [ ] External transaction support (?).
- [ ] Transactional outbox pattern implementation (?).
- [ ] Multitenancy support.
- [ ] Automatic provisioning of storage schema.

Also add implementations of particular storage backends, such as:

- [x] [`In-memory`][IStreamUnitOfWork] - for testing purposes.
- [ ] [`Entity Framework`](https://www.microsoft.com/en-us/sql-server/sql-server-2022) - for SQL Server.
- [ ] [`Dapper`](https://github.com/DapperLib/Dapper) - for SQL Server, PostgreSQL, MySQL, SQLite etc.
- [ ] [`Cassandra DB`](https://cassandra.apache.org/_/index.html) -  for distributed storage.

## Installation

To install the package, you can use the following command from the command line:

```dotnetcli
  dotnet add package StreamStore
```

or from Nuget Package Manager Console:

```powershell
  Install-Package StreamStore
```

## Usage

```csharp
  // Create store
  var storage = new StreamStore(new InMemoryStreamDatabase());

  var streamId = new Id("stream-1"); // you also can use regular string

  //Append events to stream or create a new stream if it does not exist
  var events = new Event[] {...}; // your events

  await store.OpenStreamAsync()
              // EventObject property is where you store your event
              .AddAsync(new Event { Id = new Id("event-1"), Timestamp = DateTime.Now, EventObject = event1 }) 
              .AddAsync(new Event { Id = new Id("event-2"), Timestamp = DateTime.Now, EventObject = event2 })
              .AddAsync(new Id("event-3"), DateTime.Now, event3)
              .AddRangeAsync(events)
            .SaveChangesAsync(streamId);
  
  // Get stream read-only entity
  StreamEntity streamEntity = await store.GetAsync(streamId);

  // Delete stream
  store.DeleteAsync(streamId);

```

### Register in DI container
  
  ```csharp
    services.AddSingleton<IStreamDatabase, InMemoryStreamDatabase>();
    services.AddSingleton<IStreamUnitOfWork, InMemoryStreamUnitOfWork>();
    services.AddSingleton<IEventSerializer, JsonEventSerializer>();
  
    services.AddSingleton<IStreamStore, StreamStore>();
  ```

### Good to know

- _[`Id`][Id]  is a value object (immutable class) that has implicit conversion from and to string_.  

  Thus you don't need to create [Id] object explicitly and use `ToString()` to convert to string back.  
  Also implements `IEquatable`  for [itself][Id] and for `String`.

- _[`StreamEntity`][StreamEntity] returned by store is a read-only consistent object_, i.e.:
  - Contains only **unique events ordered by revision**.
  - Contains only **events that were committed**.
- _Stream revision is always the revision of an event with maximum revision value_.

- _Idempotency of get and delete operations fully depends on particular database implementation._

- _You don't need to retrieve stream entity to append events to the stream_.

  Appending stream and getting stream entity are separate operations.

## Customization

To implement your own database you do not need StreamStore package, all necessary interfaces are located in StreamStore.Contracts package from command line:

```dotnetcli
  dotnet add package StreamStore.Contracts
```

or from Nuget Package Manager Console:

```powershell
  Install-Package StreamStore.Contracts
```


### Create your own database implementation

To create your own database implementation, you need to implement the following interfaces:

- [`IStreamDatabase`][IStreamDatabase] - provides methods for working with streams.
- [`IStreamUnitOfWork`][IStreamUnitOfWork] - provides methods for appending events to the stream and saving changes.

```csharp
  // Example of Azure Blob Storage database implementation
  class AzureBlobStreamDatabase: IStreamDatabase
  {
    // Implement methods for working with streams in Azure blob Storage
  }

  class AzureBlobStreamUnitOfWork: IStreamUnitOfWork
  {
    // Implement methods for appending events to the stream and saving changes in Azure blob Storage
  }

  // Optionally you can create your own event serializer
  class TextJsonEventSerializer: IEventSerializer
  {
    // Default serializer is using Newtonsoft.Json, so you can create your own using System.Text.Json or any other
    // Implement methods for serializing/deserializing events to/from JSON
  }
```

### Considerations

- To implement your own database you do not need StreamStore package, all necessary interfaces are located in [StreamStore.Contracts](https://www.nuget.org/packages/StreamStore.Contracts/) package.
- _You can register your own database implementation in the DI container using any kind of lifetime (i.e. Singleton, Transient, Scoped, etc.)_  

  However, if you register it as a singleton, you should be aware that it should be thread-safe and preferably stateless.

- _Solution already provides optimistic concurrency and event duplication control mechanisms, as a **pre-check** during stream opening_.  

  However, if you need consistency guaranteed, you should implement your own mechanisms as a part of [IStreamUnitOfWork] implementation. For instance, you can use a transaction mechanism suppored by `ACID compliant DBMS`.  
  For educational purposes, [InMemoryStreamUnitOfWork.cs] already contains such mechanisms.  

- _Get and Delete operations must be implemented as idempotent by their nature._

### Example

Solution already contains [InMemoryStreamDatabase.cs] and [InMemoryStreamUnitOfWork.cs] implementations **for testing and educational purposes only**.

## Contributing

If you experience any issues, have a question or a suggestion, or if you wish
to contribute, feel free to [open an issue][issues] or
[start a discussion][discussions].

[issues]: https://github.com/kostiantyn-matsebora/workspace-cli/issues
[discussions]: https://github.com/kostiantyn-matsebora/workspace-cli/discussions

## License

[`MIT License`](../LICENSE)

[InMemoryStreamDatabase.cs]: ../src/StreamStore/InMemory/InMemoryStreamDatabase.cs
[InMemoryStreamUnitOfWork.cs]: ../src/StreamStore/InMemory/InMemoryStreamUnitOfWork.cs
[Id]: ../src/StreamStore.Contracts/Id.cs
[StreamEntity]: ../src/StreamStore/StreamEntity.cs
[IStreamUnitOfWork]: ../src/StreamStore.Contracts/IStreamUnitOfWork.cs
[IStreamDatabase]: ../src/StreamStore.Contracts/IStreamDatabase.cs
