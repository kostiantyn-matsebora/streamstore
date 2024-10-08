# StreamStore 

[![StreamStore](https://github.com/kostiantyn-matsebora/streamstore/actions/workflows/streamstore.yml/badge.svg)](https://github.com/kostiantyn-matsebora/streamstore/actions/workflows/streamstore.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=kostiantyn-matsebora_streamstore&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=kostiantyn-matsebora_streamstore)
[![NuGet version (StreamStore)](https://img.shields.io/nuget/v/StreamStore.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore/)

A lightweight library provides a logical layer for storing and querying events as a stream.

Heavily inspired by Greg Young's Event Store and [`Streamstone`](https://github.com/yevhen/Streamstone) solutions.

## Overview

Designed to be easily extended with custom database backends.

## Databases

  | Package                  | Description                                                                        |
  | ------------------------ | ---------------------------------------------------------------------------------- |
  | [StreamStore.InMemory] | In-memory implementation is provided **for testing and educational purposes only** |
  | [StreamStore.S3.B2]    | [`Backblaze B2`] implementation                                                        |

## Features

The general idea is to highlight the general characteristics and features of event sourcing storage:

- [x] Event ordering.
- [x] Serialization/deserialization of events.
- [x] Optimistic concurrency control.
- [x] Event duplication detection based on event ID.
- [ ] Database agnostic test framework, including benchmarking test scenarios.
- [ ] Custom event properties (?).
- [ ] External transaction support (?).
- [ ] Transactional outbox pattern implementation (?).
- [ ] Multitenancy support.
- [ ] Automatic provisioning of storage schema.

Also add implementations of particular storage backends, such as:

- [x] [`In-memory`] - for testing purposes.
- [x] [`Backblaze B2`] - Backblaze B2.
- [ ] [`SQL`](https://github.com/DapperLib/Dapper) -  SQL Server, PostgreSQL, MySQL, SQLite etc.
- [ ] [`Cassandra DB`](https://cassandra.apache.org/_/index.html) - distributed storage.

## Installation

To install the package, you can use the following command from the command line:

```dotnetcli
  # Install StreamStore package
  
  dotnet add package StreamStore

  #Install package of particular database implementation, for instance InMemory

  dotnet add package StreamStore.InMemory
```

or from Nuget Package Manager Console:

```powershell
   # Install StreamStore package
  Install-Package StreamStore

   #Install package of particular database implementation, for instance InMemory
  Install-Package StreamStore.InMemory

```

## Usage

- Register store in DI container
  
```csharp
       services.ConfigureStreamStore();
```

- Add database implementation, see instructions for particular database in [Databases](#databases) section.
  
  For instance, to use in-memory database, you can add the following code:

```csharp
      services.UseInMemoryStreamDatabase();
```

- Use store in your application
  
```csharp

   // Inject IStreamStore in your service
    public class MyService
    {
        private readonly IStreamStore store;
  
        public MyService(IStreamStore store)
        {
            this.store = store;
        }
    }
 
  //Append events to stream or create a new stream if it does not exist
  var events = new Event[] {...}; // your events

  var streamId = new Id("stream-1");

  // Open stream like new since version is not provided.
  IStream stream = await store.OpenStreamAsync(streamId);

  try {
    stream
      // EventObject property is where you store your event
      .AddAsync(new Event { Id = new Id("event-1"), Timestamp = DateTime.Now, EventObject = events[0] }) 
      .AddAsync(new Event { Id = new Id("event-2"), Timestamp = DateTime.Now, EventObject = events[1] })
      .AddAsync(new Id("event-3"), DateTime.Now, event[2])
      .AddRangeAsync(events)
    .SaveChangesAsync(streamId);

  } catch (StreamConcurrencyException ex) {

    // Implement your logic for handling concurrency exception, or try to push with latest revision, like this
    ...
    store
        .OpenStreamAsync(streamId, ex.ActualRevision)
        .AddAsync(new Event { Id = new Id("event-4"), Timestamp = DateTime.Now, EventObject = events[3] })
        ...
        .SaveChangesAsync(streamId);
  }

  // Get stream read-only entity
  StreamEntity streamEntity = await store.GetAsync(streamId);

  // Delete stream
  store.DeleteAsync(streamId);

```

## Good to know

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

- _Get and Delete operations must be implemented as idempotent by their nature._

## Contributing

If you experience any issues, have a question or a suggestion, or if you wish
to contribute, feel free to [open an issue][issues] or
[start a discussion][discussions].

## License

[`MIT License`](../LICENSE)

[issues]: https://github.com/kostiantyn-matsebora/streamstore/issues
[discussions]: https://github.com/kostiantyn-matsebora/streamstore/discussions
[InMemoryStreamDatabase.cs]: ../src/StreamStore/InMemory/InMemoryStreamDatabase.cs
[InMemoryStreamUnitOfWork.cs]: ../src/StreamStore/InMemory/InMemoryStreamUnitOfWork.cs
[Id]: ../src/StreamStore.Contracts/Id.cs
[StreamEntity]: ../src/StreamStore/StreamEntity.cs
[IStreamUnitOfWork]: ../src/StreamStore.Contracts/IStreamUnitOfWork.cs
[IStreamDatabase]: ../src/StreamStore.Contracts/IStreamDatabase.cs
[StreamStore.S3.B2]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.S3.B2
[StreamStore.InMemory]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.InMemory
[`In-Memory`]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.InMemory
[`Backblaze B2`]: https://www.backblaze.com/b2/cloud-storage.html

