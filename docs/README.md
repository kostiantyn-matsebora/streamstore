# <p align="center">StreamStore</p>

Lightweight library provides logical layer for storing and querying events as a stream.

Heavily inspired by Greg Young's Event Store and [`Streamstone`](https://github.com/yevhen/Streamstone) solutions.

## Overview

Library itself does not contain any production  ready database storage implementations _yet_, but it is designed to be easily extended with custom database backends.

`In-memory` database implementation is provided in [InMemoryStreamDatabase.cs] and [InMemoryStreamUnitOfWork.cs] **for testing purposes only**.

## Features

The general idea is to highlight the general characteristics and features of event sourcing storage:

- [x] Event ordering.
- [x] Serialization/deserialization of events.
- [x] Optimistic concurrency control.
- [x] Event duplication detection based on event id.
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

## Usage

```csharp
  // Create store
  var storage = new StreamStore(new InMemoryDatabase());

  // Create store with your own database implementation and/or event serializer
  var storage = new StreamStore(new YourDatabase(), new YourEventSerializer());

  var streamId = new Id("stream-1"); // you also can use regular string

  //Append events to stream or create new stream if it does not exist
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

- _[Id] is a value object, i.e. it is immutable class has **implicit conversion from and to string**_.  
  Thus you don't need to create [Id] object explicitly and use `ToString()` to convert to string back.  
  Also implements `IEquatable`  for [itself][Id] and for `String`.

- _[StreamEntity] returned by store is a read-only consistent object_, i.e.:
  - Contains only **unique events ordered by revision**.
  - Contains only **events that were committed**.
- _Stream revision is always revision of event with maximum revision value_.
- _Get and delete operations are idempotent._
- _You don't need to retrieve stream entity to append events to the stream_.  
  Appending stream and getting stream entity are separate operations.

## Customization

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

- _You can register own database implementation in the DI container it using any kind of lifetime (i.e. Singleton, Transient, Scoped etc.)_.  
  However, if your register it as a singleton, you should be aware that it should be thread-safe and preferably stateless.

- _Solution already provides optimistic concurrency and event duplication control mechanisms, as a **pre-check** during stream opening_.  
  However, if you need consistency guaranteed, you should implement your own mechanisms as a part of [IStreamUnitOfWork] implementation.  
  For instance, you can use transaction mechanism for implementing stream  database in `ACID complaint DBMS`.  
  In educational purposes, [InMemoryStreamUnitOfWork.cs] already contains such mechanisms.  
  
  

### Example

Solution already contains [InMemoryStreamDatabase.cs] and [InMemoryStreamUnitOfWork.cs] implementations **for testing purposes only**.

## Contributing

If you experience any issues, have a question or a suggestion, or if you wish
to contribute, feel free to [open an issue][issues] or
[start a discussion][discussions].

[issues]: https://github.com/kostiantyn-matsebora/workspace-cli/issues
[discussions]: https://github.com/kostiantyn-matsebora/workspace-cli/discussions

## License

[`MIT License`](../LICENSE.txt)

[InMemoryStreamDatabase.cs]: ../src/StreamStore/InMemory/InMemoryStreamDatabase.cs
[InMemoryStreamUnitOfWork.cs]: ../src/StreamStore/InMemory/InMemoryStreamUnitOfWork.cs
[Id]: ../src/StreamStore.Contracts/Id.cs
[StreamEntity]: ../src/StreamStore/StreamEntity.cs
[IStreamUnitOfWork]: ../src/StreamStore.Contracts/IStreamUnitOfWork.cs
[IStreamDatabase]: ../src/StreamStore.Contracts/IStreamDatabase.cs
