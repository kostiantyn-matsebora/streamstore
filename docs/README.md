# StreamStore

[![Build](https://github.com/kostiantyn-matsebora/streamstore/actions/workflows/streamstore.yml/badge.svg)](https://github.com/kostiantyn-matsebora/streamstore/actions/workflows/streamstore.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=kostiantyn-matsebora_streamstore&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=kostiantyn-matsebora_streamstore)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=kostiantyn-matsebora_streamstore&metric=coverage)](https://sonarcloud.io/summary/new_code?id=kostiantyn-matsebora_streamstore)
[![NuGet version (StreamStore)](https://img.shields.io/nuget/v/StreamStore.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore/)

Asynchronous event sourcing.

Library provides a logical layer for storing and querying events as a stream.

Heavily inspired by Greg Young's Event Store and [`Streamstone`](https://github.com/yevhen/Streamstone) solutions.

## Overview

Designed to be easily extended with custom database backends.
Despite the fact that component implements a logical layer for storing and querying events as a stream,
 `it does not provide functionality of DDD aggregate`, such as state mutation, conflict resolution etc., but serves more as `persistence layer`  for it.

## Databases

  | Package                | Description                                                                            |                                                                                                                                                                            |
  | ------------------------ | ------------------------------------------------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
  | [StreamStore.Sql.Sqlite] | [`SQLite`] implementation                                                              | [![NuGet version (StreamStore.Sql.Sqlite)](https://img.shields.io/nuget/v/StreamStore.Sql.Sqlite.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Sql.Sqlite/) |
  | [StreamStore.InMemory]   | `In-memory` implementation is provided **for testing and educational purposes only** | [![NuGet version (StreamStore.InMemory)](https://img.shields.io/nuget/v/StreamStore.InMemory.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.InMemory/) |
  | [StreamStore.S3.AWS]     | [`Amazon S3`] implementation                                                         | [![NuGet version (StreamStore.S3.AWS)](https://img.shields.io/nuget/v/StreamStore.S3.AWS.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.S3.AWS/)       |
  | [StreamStore.S3.B2]      | [`Backblaze B2`] implementation                                                      | [![NuGet version (StreamStore.S3.B2)](https://img.shields.io/nuget/v/StreamStore.S3.B2.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.S3.B2/)          |

## Features

The general idea is to highlight the common characteristics and features of event sourcing storage:

- [x] Asynchronous read and write operations.
- [x] Event ordering.
- [x] Serialization/deserialization of events.
- [x] Optimistic concurrency control.
- [x] Event duplication detection based on event ID.
- [x] Database agnostic test framework, including benchmarking test scenarios.
- [x] Binary serialization support.
- [ ] Custom event properties (?).
- [ ] External transaction support (?).
- [ ] Transactional outbox pattern implementation (?).
- [ ] Multitenancy support.
- [x] Automatic provisioning of storage schema.

Also add implementations of particular storage backends, such as:

- [x] [`In-Memory`] - for testing purposes.
- [x] [`Backblaze B2`] - Backblaze B2.
- [x] [`Amazon S3`] - Amazon S3.
- [x] [`SQL`](https://github.com/DapperLib/Dapper) based DBMS:
  - [x] [`SQLite`]
  - [ ] [`PostgreSQL`](https://www.postgresql.org/)
  - [ ] [`Azure SQL`](https://azure.microsoft.com/en-us/services/sql-database/)
  - [ ] [`MySQL`](https://www.mysql.com/)
- [ ] [`Cassandra DB`](https://cassandra.apache.org/_/index.html) - distributed storage.

## Installation

To install the package, you can use the following command from the command line:

```dotnetcli
  # Install StreamStore package
  
  dotnet add package StreamStore

  # Install package of particular database implementation, for instance InMemory

  dotnet add package StreamStore.InMemory
```

or from NuGet Package Manager Console:

```powershell
   # Install StreamStore package
  Install-Package StreamStore

   # Install package of particular database implementation, for instance SQLite database backend
  Install-Package StreamStore.Sql.Sqlite
```

## Usage

- Register store in DI container
  
```csharp
       services.ConfigureStreamStore(x =>  // Register StreamStore
          x.UseSqliteDatabase(x => ...);   // Register database implementation,
                                           // more details you can fing in particular implementation documentation
        ); 
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
 
  // Append events to stream or create a new stream if it does not exist
  // EventObject property is where you store your event
  var events = new Event[]  {
        new Event { Id = "event-1", Timestamp = DateTime.Now, EventObject = eventObject } 
        ...
      };

  try {
    store
      .OpenStreamAsync("stream-1")                          // Open stream like new since revision is not provided
        .AddAsync("event-3", DateTime.Now, yourEventObject) // You can add events one by one
        .AddRangeAsync(events)                              // Or range of events by passing IEnumerable<Event>
      .SaveChangesAsync(token);

  } catch (StreamConcurrencyException ex) {

    // Read from stream and implement your logic for handling optimistic concurrency exception
    await foreach(var @event in await store.BeginReadAsync("stream-1", token)) {
        ...
    }
    
    // Push result to the end of stream
    store
        .OpenStreamAsync("stream-1", ex.ActualRevision)
        .AddAsync(new Event { Id = "event-4", Timestamp = DateTime.Now, EventObject = yourEventObject })
        ...
        .SaveChangesAsync(streamId);
  } catch (StreamLockedException ex) {
    // Some database backends like S3 do not support optimistic concurrency control
    // So, the only way to handle concurrency is to lock the stream
  }
```

More examples of reading and writing events you can find in test scenarios[StreamStore.Tests](../src/StreamStore.Testing/StreamStore/Scenarios/) project.

## Good to know

- _[`Id`][Id]  is a value object (immutable class) that has implicit conversion from and to string_.  

  Thus you don't need to create [Id] object explicitly and use `ToString()` to convert to string back.  
  Also implements `IEquatable`  for [itself][Id] and for `String`.
- _[`Revision`][Revision] is a value object (immutable class) that represents a revision of the stream._  
  It is used for optimistic concurrency control and event ordering.
  It has implicit conversion from and to `Int32` type.  
  Also implements `IEquatable` and `IComparable` for itself and for `Int32`.

- You can read from any stream starting from the provided revision.

- _`ReadToEnd` method  returns collection of events from the stream starting from the provided revision_:
  - Contains only **unique events ordered by revision**.
  - Contains only **events that were committed**.
  
- _Stream revision is always the revision of an event with maximum revision value_.

- _Idempotency of reading and deletion fully depends on particular database implementation._

- _You don't need to retrieve stream  to add events to it_.  
  Appending to stream and getting stream  are separate operations.

- _Despite the fact that reading is declared as asynchronous and iterative operation, for the sake of performance it is implemented as paginated operation._

  You can define the page size by using `WithReadingPageSize` method of store configuration, by default it is 10 events.

- _Reading and writing operations are not thread-safe_.  
 Thus, it is not recommended to use the same instances of `IStreamReader` or `IAsyncEnumerable<StreamEvent>` in multiple threads simultaneously.

## Customization

To implement your own database you do not need StreamStore package, all necessary interfaces are located in StreamStore.Contracts package from command line:

```dotnetcli
  dotnet add package StreamStore.Contracts
```

or from NuGet Package Manager Console:

```powershell
  Install-Package StreamStore.Contracts
```

### Serialization

About serialization you can read in [SERIALIZATION.md](SERIALIZATION.md) file.

### Create your own database implementation

To create your own database implementation, you need to implement the following interfaces:

- [`IStreamDatabase`][IStreamDatabase] - provides methods for working with streams.
  Create your own implementation based on [`StreamDatabaseBase`](../src/StreamStore.Contracts/Database/StreamDatabaseBase.cs) abstract class.

- [`IStreamUnitOfWork`][IStreamUnitOfWork] - provides methods for appending events to the stream and saving changes.  
  Create your own implementation based on [`StreamUnitOfWorkBase`](../src/StreamStore.Contracts/Database/StreamUnitOfWorkBase.cs)
  and override following methods:

  ```csharp
    class MyStreamUnitOfWork: StreamUnitOfWorkBase
    {
      protected override Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
      {
        // Implement saving logic
      }
  
      protected override Task OnEventAdded(EventRecord @event, CancellationToken token)
      {
            // Optionally implement logic for handling event added, 
            // such as instance logging, puting event to outbox or temporary storage etc.
      }

     protected override void Dispose(bool disposing)
     {
        // Optionally implement disposing logic
     }
    }
  ```

  Default serializer is using `Newtonsoft.Json` library, so you can create your own using `System.Text.Json` or any other, by
  implementing [`IEventSerializer`](../src/StreamStore.Contracts/Serialization/IEventSerializer.cs) interface.

### Considerations

- To implement your own database you do not need StreamStore package, all necessary interfaces are located in [StreamStore.Contracts](https://www.nuget.org/packages/StreamStore.Contracts/) package.
- _You can register your own database implementation in the DI container using any kind of lifetime (i.e. Singleton, Transient, Scoped, etc.)_  

  However, if you register it as a singleton, you should be aware that it should be thread-safe and preferably stateless.

- _Solution already provides optimistic concurrency and event duplication control mechanisms, as a **pre-check** during stream opening_.  

  However, if you need consistency guaranteed, you should implement your own mechanisms as a part of [IStreamUnitOfWork] implementation.  
  For instance, you can use a transaction mechanism supported by `ACID compliant DBMS`.

- _Get and Delete operations must be implemented as idempotent by their nature._

## Contributing

If you experience any issues, have a question or a suggestion, or if you wish
to contribute, feel free to [open an issue][issues] or
[start a discussion][discussions].

## License

[`MIT License`](../LICENSE)

[issues]: https://github.com/kostiantyn-matsebora/streamstore/issues
[discussions]: https://github.com/kostiantyn-matsebora/streamstore/discussions
[Id]: ../src/StreamStore.Contracts/Id.cs
[Revision]: ../src/StreamStore.Contracts/Revision.cs
[IStreamUnitOfWork]: ../src/StreamStore.Contracts/Database/IStreamUnitOfWork.cs
[IStreamDatabase]: ../src/StreamStore.Contracts/Database/IStreamDatabase.cs
[StreamStore.S3.B2]: ../src/StreamStore.S3.B2
[StreamStore.S3.AWS]: ../src/StreamStore.S3.AWS
[StreamStore.InMemory]: ../src/StreamStore.InMemory
[StreamStore.Sql.Sqlite]: ../src/StreamStore.Sql.Sqlite
[`In-Memory`]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.InMemory
[`Backblaze B2`]: https://www.backblaze.com/b2/cloud-storage.html
[`Amazon S3`]: https://aws.amazon.com/s3/
[`SQLite`]: https://www.sqlite.org/index.html
