<p align="center">
  <img src="icon.png?raw=true" alt="StreamStore"/>
</p>


[![Build](https://github.com/kostiantyn-matsebora/streamstore/actions/workflows/build.yml/badge.svg)](https://github.com/kostiantyn-matsebora/streamstore/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=kostiantyn-matsebora_streamstore&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=kostiantyn-matsebora_streamstore)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=kostiantyn-matsebora_streamstore&metric=coverage)](https://sonarcloud.io/summary/new_code?id=kostiantyn-matsebora_streamstore)
[![NuGet version (StreamStore)](https://img.shields.io/nuget/v/StreamStore.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore/)

Asynchronous event sourcing.

Library provides a logical layer for storing and querying events as a stream.

Heavily inspired by Greg Young's Event Store and [`Streamstone`](https://github.com/yevhen/Streamstone) solutions.

## Overview

Designed to be easily extended with custom storage backends.
Despite the fact that component implements a logical layer for storing and querying events as a stream,
 `it does not provide functionality of domain aggregate`, such as state mutation, conflict resolution etc., but serves more as `persistence layer`  for it.

 ![StreamStore class diagram](diagrams/store.png)

## Packages

| Name | Description | Package |
| ------ | ------------ | ------------ |
| [StreamStore] | Asynchronous event streaming | [![NuGet version (StreamStore)](https://img.shields.io/nuget/v/StreamStore.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore/) |
| [StreamStore.Storage] | Base abstract implementation of persistence layer of StreamStore component. | [![NuGet version (StreamStore.Storage)](https://img.shields.io/nuget/v/StreamStore.Storage.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Storage/) |
| [StreamStore.Storage.Contracts] | Persistence layer interfaces of StreamStore component.| [![NuGet version (StreamStore.Storage.Contracts)](https://img.shields.io/nuget/v/StreamStore.Storage.Contracts.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Storage.Contracts/) |
| [StreamStore.Serialization.Protobuf] | Protobuf event serializer/deserializer. | [![NuGet version (StreamStore.Testing)](https://img.shields.io/nuget/v/StreamStore.Serialization.Protobuf.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Serialization.Protobuf/) |
| [StreamStore.Storage.EventFlow] | Adapter of StreamStore's storage backends to use as event store in [`EventFlow`]. | [![NuGet version (StreamStore.Storage.EventFlow)](https://img.shields.io/nuget/v/StreamStore.Storage.EventFlow.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Storage.EventFlow/) |

### Storage packages

  | Name  | Description  | Concurrency Control  | Multitenancy | Event Duplication Detection |  Package  |
  | ------| ------------ |--------------------- | ------------ | ----------------------------- |-------------------|
  | [StreamStore.NoSql.Cassandra] | [`Apache Cassandra`] and [`Azure Cosmos DB for Apache Cassandra`] storage | [`Optimistic`] | :white_check_mark: | :x: |   [![NuGet version (StreamStore.NoSql.Cassandra)](https://img.shields.io/nuget/v/StreamStore.NoSql.Cassandra.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.NoSql.Cassandra/)
  | [StreamStore.Sql.PostgreSql] | [`PostgreSQL`](https://www.postgresql.org/) storage | [`Optimistic`] | :white_check_mark: | :white_check_mark: |   [![NuGet version (StreamStore.Sql.PostgreSql)](https://img.shields.io/nuget/v/StreamStore.Sql.PostgreSql.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Sql.PostgreSql/)
  | [StreamStore.Sql.Sqlite]     | [`SQLite`](https://www.sqlite.org/index.html) storage | [`Optimistic`] | :white_check_mark: |  :white_check_mark: |  [![NuGet version (StreamStore.Sql.Sqlite)](https://img.shields.io/nuget/v/StreamStore.Sql.Sqlite.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Sql.Sqlite/)
  | [StreamStore.InMemory]       | **In-memory** storage is provided **for testing and educational purposes only** | [`Optimistic`] | :white_check_mark: | :white_check_mark: |   [![NuGet version (StreamStore.InMemory)](https://img.shields.io/nuget/v/StreamStore.InMemory.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.InMemory/) |
  | [StreamStore.S3.AWS]         | [`Amazon S3`] storage                                                         | [`Pessimistic`] | :x: | :x: | [![NuGet version (StreamStore.S3.AWS)](https://img.shields.io/nuget/v/StreamStore.S3.AWS.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.S3.AWS/)       |
  | [StreamStore.S3.B2]          | [`Backblaze B2`] storage (not working at the moment!)                                                      | [`Pessimistic`] | :x: | :x: |  [![NuGet version (StreamStore.S3.B2)](https://img.shields.io/nuget/v/StreamStore.S3.B2.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.S3.B2/) |

## Concepts

About basic concepts you can read in [CONCEPTS.md](../docs/CONCEPTS.md).

## Features

The general idea is to highlight the common characteristics and features of event sourcing storage:

- [x] Asynchronous read and write operations.
- [x] Multitenancy support.
- [x] Automatic provisioning of storage schema.
- [x] Event ordering.
- [x] Serialization/deserialization of events.
- [x] Optimistic concurrency control.
- [x] Event duplication detection based on event ID.
- [x] Storage agnostic test framework.
- [x] Binary serialization support.
- [x] Custom event properties.
  
## Storages

Also add implementations of particular storage, such as:

- [x] [`In-Memory`] - for testing purposes.
- [x] [`Binary Object`] storages:
  - [x] [`Backblaze B2`] - Backblaze B2.
  - [x] [`Amazon S3`] - Amazon S3.
- [x] [`SQL`](https://github.com/DapperLib/Dapper) based DBMS:
  - [x] [`SQLite`]
  - [x] [`PostgreSQL`](https://www.postgresql.org/)
  - [ ] [`Azure SQL`](https://azure.microsoft.com/en-us/services/sql-database/)
  - [ ] [`MySQL`](https://www.mysql.com/)
- [x]  [`NoSQL`] based DBMS:
  - [x] [`Apache Cassandra`]
  - [x] [`Azure Cosmos DB for Apache Cassandra`]

## Roadmap

- [ ] Composite stream identifier
- [ ] External transaction support (?).
- [ ] Transactional outbox pattern implementation (?).

## Installation

To install the package, you can use the following command from the command line:

```dotnetcli
  # Install StreamStore package
  
  dotnet add package StreamStore

  # Install package of particular storage implementation, for instance InMemory

  dotnet add package StreamStore.InMemory
```

or from NuGet Package Manager Console:

```powershell
   # Install StreamStore package
  Install-Package StreamStore

   # Install package of particular storage implementation, for instance SQLite storage backend
  Install-Package StreamStore.Sql.Sqlite
```

## Usage

- Register store in DI container
  
```csharp
      // Register StreamStore
    services.AddStreamStore(x =>
      // Optional. Enable automatic provisioning of storage schema provisioning, default: false.
      x.EnableAutomaticProvisioning()
      // Optional. Enable multitenancy
      // By explicitly passing tenant identifiers
      .EnableMultitenancy("tenant-1", "tenant-2")
      // Or providing your own ITenantProvider implementation    
      .EnableMultitenancy<MyTenantProvider>()
      // Optional. Define size of the batch for reading events, default: 1000.
      .WithReadingPageSize(1000)
      // Optional. Define algorith of event reading, default: Queue
      .WithReadingMode(StreamReadingMode.Queue)
      // Optional. Configure custom serialization/deserialization of events, default: Newtonsoft.Json
      .ConfigureSerialization(...)
      // Configure event persistence layer, you can use one of the provided storage implementations
      x.ConfigurePersistence(c =>
      // For instance, SQLite storage backend in single-tenant mode
          c.UseSqlite(x =>
                c.WithConnectionString(connectionString))
      // Or SQLite in multi-tenant mode
          .UseSqliteWithMultitenancy(x =>
                c.WithConnectionString(connectionString))
      )
    );
```

- Use store in your application

```csharp
   // Inject IStreamStore in your service or controller for single storage implementation
    public class MyService
    {
        private readonly IStreamStore store;
  
        public MyService(IStreamStore store)
        {
            this.store = store;
        }
    }
 
  // Or IStreamStoreFactory for multitenancy
    public class MyService
    {
        private readonly IStreamStoreFactory storeFactory;
  
        public MyService(IStreamStoreFactory storeFactory)
        {
            this.storeFactory = storeFactory;
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
      .BeginAppendAsync("stream-1")   // Open stream like new since revision is not provided
         .AppendAsync(x =>            // Append events one by one using fluent API
            x.WithId("event-3")       // Specify event ID
             .Dated(DateTime.Now)     // Specify event timestamp
             .WithEvent(eventObject)  // Specify event object
             // Optional. Add custom property to event
             .WithCustomProperty("property-name", "property-value") 
             // Or use WithCustomProperties method to add multiple custom properties
             .WithCustomProperties(new Dictionary<string, object> { { "property-name", "property-value" } }) 
         )
        ...
        .AppendRangeAsync(events)  // Or append range of events by passing IEnumerable<Event>
      .SaveChangesAsync(token);

  } catch (ConcurrencyException ex) {


    // Read from stream and implement your logic for handling optimistic concurrency exception (optional)
    await foreach(var @event in await store.BeginReadAsync("stream-1", token)) {
        ...
    }
    
    // Get stream metadata to get actual revision
    var metadata = await store.GetMetadataAsync("stream-1", token);

    // Push result to the end of stream
    store
        // Open stream with revision to handle concurrency
        .BeginAppendAsync("stream-1", metadata.Revision) 
        // Append events one by one using fluent API
           .AppendAsync(x =>
            x.WithId( "event-4")
             .Dated(DateTime.Now)
             .WithEvent(yourEventObject)
           )
        ...
        .SaveChangesAsync(streamId);
  }
```

More examples of reading and writing events you can find in test scenarios of [StreamStore.Testing](../src/StreamStore.Testing/StreamStore/Scenarios/) project.

## Examples

Examples of using StreamStore with different storage implementations you can find in separate [`streamstore-examples`] repository.

## Customization

How to create your own persistence layer implementation you can find in [STORAGE.md](STORAGE.md).

How to create your own serialization/deserialization implementation you can find in [SERIALIZATION.md](SERIALIZATION.md).

## Error handling

How to handle errors and exceptions in your application you can find in [ERRORHANDLING.md](ERRORHANDLING.md).

## Contributing

If you experience any issues, have a question or a suggestion, or if you wish
to contribute, feel free to [open an issue][issues] or
[start a discussion][discussions].

## License

[`MIT License`](../LICENSE)

[`StreamStore`]: https://github.com/kostiantyn-matsebora/streamstore/
[issues]: https://github.com/kostiantyn-matsebora/streamstore/issues
[discussions]: https://github.com/kostiantyn-matsebora/streamstore/discussions
[StreamStore.S3.B2]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.S3.B2
[StreamStore.S3.AWS]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.S3.AWS
[StreamStore.InMemory]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.InMemory
[StreamStore.Sql.Sqlite]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.Sql.Sqlite
[StreamStore.Sql.PostgreSql]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.Sql.PostgreSql
[StreamStore.NoSql.Cassandra]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.NoSql.Cassandra
[`In-Memory`]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.InMemory
[`Backblaze B2`]: https://www.backblaze.com/b2/cloud-storage.html
[`Amazon S3`]: https://aws.amazon.com/s3/
[`SQLite`]: https://www.sqlite.org/index.html
[`NoSQL`]: https://en.wikipedia.org/wiki/NoSQL
[`Binary Object`]: https://en.wikipedia.org/wiki/Object_storage
[`Apache Cassandra`]: https://cassandra.apache.org/_/index.html
[`Azure Cosmos DB for Apache Cassandra`]: https://learn.microsoft.com/en-us/azure/cosmos-db/cassandra/introduction
[`Optimistic`]: https://en.wikipedia.org/wiki/Optimistic_concurrency_control
[`Pessimistic`]: https://en.wikipedia.org/wiki/Lock_(computer_science)
[StreamStore]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore
[StreamStore.Serialization.Protobuf]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.Serialization.Protobuf
[StreamStore.Storage]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.Storage
[StreamStore.Storage.Contracts]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.Storage.Contracts
[`streamstore-examples`]: https://github.com/kostiantyn-matsebora/streamstore-examples
[StreamStore.Storage.EventFlow]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.Storage.EventFlow
[`EventFlow`]: https://geteventflow.net/
