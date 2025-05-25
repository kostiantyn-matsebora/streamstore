# StreamStore

Asynchronous event sourcing.

Library provides a logical layer for storing and querying events as a stream.

Heavily inspired by Greg Young's Event Store and [`Streamstone`](https://github.com/yevhen/Streamstone) solutions.

## Overview

Designed to be easily extended with custom storage backends.
Despite the fact that component implements a logical layer for storing and querying events as a stream,
 `it does not provide functionality of DDD aggregate`, such as state mutation, conflict resolution etc., but serves more as `persistence layer` for it.

## Storage packages

  | Package                | Description                                                                            |        Multitenancy        | Event Duplication Detection | Package   |
  | ---------------------------- | ------------------------------------------------------------------------------------ | ----------------- | -----------|--------------------------------------------------------------------------------------------------------------------------------------------------------- |
  | [StreamStore.NoSql.Cassandra] | [`Apache Cassandra`] and [`Azure Cosmos DB for Apache Cassandra`] port | :white_check_mark: | :x: | [![NuGet version (StreamStore.NoSql.Cassandra)](https://img.shields.io/nuget/v/StreamStore.NoSql.Cassandra.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.NoSql.Cassandra/)  
  | [StreamStore.Sql.PostgreSql] | [`PostgreSQL`](https://www.postgresql.org/) port | :white_check_mark: | :white_check_mark: | [![NuGet version (StreamStore.Sql.PostgreSql)](https://img.shields.io/nuget/v/StreamStore.Sql.PostgreSql.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Sql.PostgreSql/)
  | [StreamStore.Sql.Sqlite]     | [`SQLite`](https://www.sqlite.org/index.html) port | :white_check_mark: |  :white_check_mark: | [![NuGet version (StreamStore.Sql.Sqlite)](https://img.shields.io/nuget/v/StreamStore.Sql.Sqlite.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Sql.Sqlite/)
  | [StreamStore.InMemory]       | `In-memory` port is provided **for testing and educational purposes only** | :white_check_mark: | :white_check_mark: | [![NuGet version (StreamStore.InMemory)](https://img.shields.io/nuget/v/StreamStore.InMemory.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.InMemory/) |
  | [StreamStore.S3.AWS]         | [`Amazon S3`] port                                                         | :x: | :x: |[![NuGet version (StreamStore.S3.AWS)](https://img.shields.io/nuget/v/StreamStore.S3.AWS.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.S3.AWS/)       |
  | [StreamStore.S3.B2]          | [`Backblaze B2`] port                                                      | :x: | :x: |[![NuGet version (StreamStore.S3.B2)](https://img.shields.io/nuget/v/StreamStore.S3.B2.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.S3.B2/)          |


## Features

The general idea is to highlight the common characteristics and features of event sourcing storage:

- [x] Asynchronous read and write operations.
- [x] Multitenancy support.
- [x] Automatic provisioning of storage schema.
- [x] Event ordering.
- [x] Serialization/deserialization of events.
- [x] Optimistic concurrency control.
- [x] Event duplication detection based on event ID.
- [x] Storage agnostic test framework, including benchmarking test scenarios.
- [x] Binary serialization support.

More information you can find in the [documentation](https://github.com/kostiantyn-matsebora/streamstore).

[StreamStore.S3.B2]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.S3.B2
[StreamStore.S3.AWS]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.S3.AWS
[StreamStore.InMemory]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.InMemory
[StreamStore.Sql.Sqlite]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.Sql.Sqlite
[StreamStore.Sql.PostgreSql]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.Sql.PostgreSql
[StreamStore.NoSql.Cassandra]: https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.NoSql.Cassandra
[`Amazon S3`]: https://aws.amazon.com/s3/
[`Backblaze B2`]: https://www.backblaze.com/b2/cloud-storage.html
[`Apache Cassandra`]: https://cassandra.apache.org/_/index.html
[`Azure Cosmos DB for Apache Cassandra`]: https://learn.microsoft.com/en-us/azure/cosmos-db/cassandra/introduction
