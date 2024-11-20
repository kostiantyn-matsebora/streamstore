# StreamStore

[![Build](https://github.com/kostiantyn-matsebora/streamstore/actions/workflows/build.yml/badge.svg)](https://github.com/kostiantyn-matsebora/streamstore/actions/workflows/build.yml) [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=kostiantyn-matsebora_streamstore&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=kostiantyn-matsebora_streamstore)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=kostiantyn-matsebora_streamstore&metric=coverage)](https://sonarcloud.io/summary/new_code?id=kostiantyn-matsebora_streamstore)
[![NuGet version (StreamStore)](https://img.shields.io/nuget/v/StreamStore.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore/)

Asynchronous event sourcing.

Library provides a logical layer for storing and querying events as a stream.

Heavily inspired by Greg Young's Event Store and [`Streamstone`](https://github.com/yevhen/Streamstone) solutions.

## Overview

Designed to be easily extended with custom database backends.
Despite the fact that component implements a logical layer for storing and querying events as a stream,
 `it does not provide functionality of DDD aggregate`, such as state mutation, conflict resolution etc., but serves more as `persistence layer`  for it.

## Storage packages

  | Package                | Description                                                                            |        Multitenancy        |  Package   |
  | ---------------------------- | ------------------------------------------------------------------------------------ | ----------------- | --------------------------------------------------------------------------------------------------------------------------------------------------------- |
  | [StreamStore.Sql.PostgreSql] | [`PostgreSQL`](https://www.postgresql.org/) implementation | :white_check_mark: | [![NuGet version (StreamStore.Sql.PostgreSql)](https://img.shields.io/nuget/v/StreamStore.Sql.PostgreSql.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Sql.PostgreSql/)
  | [StreamStore.Sql.Sqlite]     | [`SQLite`](https://www.sqlite.org/index.html) implementation | :white_check_mark: | [![NuGet version (StreamStore.Sql.Sqlite)](https://img.shields.io/nuget/v/StreamStore.Sql.Sqlite.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Sql.Sqlite/)
  | [StreamStore.InMemory]       | `In-memory` implementation is provided **for testing and educational purposes only** | :white_check_mark: | [![NuGet version (StreamStore.InMemory)](https://img.shields.io/nuget/v/StreamStore.InMemory.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.InMemory/) |
  | [StreamStore.S3.AWS]         | [`Amazon S3`] implementation                                                         | :x: |[![NuGet version (StreamStore.S3.AWS)](https://img.shields.io/nuget/v/StreamStore.S3.AWS.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.S3.AWS/)       |
  | [StreamStore.S3.B2]          | [`Backblaze B2`] implementation                                                      | :x: |[![NuGet version (StreamStore.S3.B2)](https://img.shields.io/nuget/v/StreamStore.S3.B2.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.S3.B2/)          |

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
- [x] Database agnostic test framework, including benchmarking test scenarios.
- [x] Binary serialization support.

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
- [ ]  [`NoSQL`] based DBMS:
  - [ ] [`Cassandra DB`](https://cassandra.apache.org/_/index.html) - distributed storage.

## Roadmap

- [ ] Custom event properties (?).
- [ ] External transaction support (?).
- [ ] Transactional outbox pattern implementation (?).

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
    services.ConfigureStreamStore(x =>              // Register StreamStore
      x.EnableSchemaProvisioning()                  // Optional. Enable schema provisioning, default: false.
      
      // Register single database implementation, see details in documentation for particular database
      x.WithSingleDatabase(c =>                 
          c.UseSqliteDatabase(x =>                  // For instance, SQLite database backend
              x.ConfigureDatabase(c =>
                c.WithConnectionString(connectionString)
              )
          )
      )
      // Or enable multitenancy, see details in documentation for particular database.
      x.WithMultitenancy(c => 
          c.UseInMemoryDatabase()                   // For instance, InMemory database backend
           .UseTenantProvider<MyTenantProvider>()   // Optional. Register your  ITenantProvider implementation.
                                                    // Required if you want schema to be provisioned for each tenant.
      )
    ); 
```

- Use store in your application

```csharp
   // Inject IStreamStore in your service or controller for single database implementation
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
      .BeginWriteAsync("stream-1")       // Open stream like new since revision is not provided
         .AppendEventAsync(x =>          // Append events one by one using fluent API
            x.WithId("event-3")
             .Dated(DateTime.Now)
             .WithEvent(eventObject)
         )
        ...
        .AppendRangeAsync(events)       // Or append range of events by passing IEnumerable<Event>
      .CommitAsync(token);

  } catch (StreamConcurrencyException ex) {

    // Read from stream and implement your logic for handling optimistic concurrency exception
    await foreach(var @event in await store.BeginReadAsync("stream-1", token)) {
        ...
    }
    
    // Push result to the end of stream
    store
        .BeginWriteAsync("stream-1", ex.ActualRevision)
           .AppendEventAsync(x =>                // Append events one by one using fluent API
            x.WithId( "event-4")
             .Dated(DateTime.Now)
             .WithEvent(yourEventObject)
           )
        ...
        .CommitAsync(streamId);
  } catch (StreamLockedException ex) {
    // Some database backends like S3 do not support optimistic concurrency control
    // So, the only way to handle concurrency is to lock the stream
  }
```

More examples of reading and writing events you can find in test scenarios of [StreamStore.Testing](../src/StreamStore.Testing/StreamStore/Scenarios/) project.

## Example

Each type of storage has its own example project, for instance, you can find an example of usage in the [StreamStore.Sql.Example](../src/StreamStore.Sql.Example) project.

Example projects provides a simple console application that demonstrates how to **configure and use** [`StreamStore`] in your application as single database or multitenancy.

`Single database` examples demonstrates:

- optimistic concurrency control
- asynchronous reading and writing operations
  
`Multitenancy` examples, in turn, demonstrates asynchronous reading and writing operations in **isolated tenant storage**.

For getting all running options simply run the application with `--help` argument.

## Contributing

If you experience any issues, have a question or a suggestion, or if you wish
to contribute, feel free to [open an issue][issues] or
[start a discussion][discussions].

## License

[`MIT License`](../LICENSE)

[`StreamStore`]: https://github.com/kostiantyn-matsebora/streamstore/
[issues]: https://github.com/kostiantyn-matsebora/streamstore/issues
[discussions]: https://github.com/kostiantyn-matsebora/streamstore/discussions
[StreamStore.S3.B2]: ../src/StreamStore.S3.B2
[StreamStore.S3.AWS]: ../src/StreamStore.S3.AWS
[StreamStore.InMemory]: ../src/StreamStore.InMemory
[StreamStore.Sql.Sqlite]: ../src/StreamStore.Sql.Sqlite
[StreamStore.Sql.PostgreSql]:https://www.nuget.org/packages/StreamStore.Sql.PostgreSql/
[`In-Memory`]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.InMemory
[`Backblaze B2`]: https://www.backblaze.com/b2/cloud-storage.html
[`Amazon S3`]: https://aws.amazon.com/s3/
[`SQLite`]: https://www.sqlite.org/index.html
[`NoSQL`]: https://en.wikipedia.org/wiki/NoSQL
[`Binary Object`]: https://en.wikipedia.org/wiki/Object_storage
