# StreamStore

Asynchronous event sourcing.

A lightweight library provides a logical layer for storing and querying events as a stream.

Heavily inspired by Greg Young's Event Store and [`Streamstone`](https://github.com/yevhen/Streamstone) solutions.

Currently, the library provides the following database implementations:

  | Package                | Description                                                                          |                                                                                                                                                                            |
  | ---------------------- | ------------------------------------------------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
  | [StreamStore.Sql.PostgreSql] | [`PostgreSQL`](https://www.postgresql.org/) implementation | [![NuGet version (StreamStore.Sql.PostgreSql)](https://img.shields.io/nuget/v/StreamStore.Sql.PostgreSql.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Sql.PostgreSql/)
  | [StreamStore.Sql.Sqlite] | [`SQLite`](https://www.sqlite.org/index.html) implementation | [![NuGet version (StreamStore.Sql.Sqlite)](https://img.shields.io/nuget/v/StreamStore.Sql.Sqlite.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Sql.Sqlite/) 
  | [StreamStore.InMemory] | `In-memory` implementation is provided **for testing and educational purposes only** | [![NuGet version (StreamStore.InMemory)](https://img.shields.io/nuget/v/StreamStore.InMemory.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.InMemory/) |
  | [StreamStore.S3.AWS]   | [`Amazon S3`] implementation                                                         | [![NuGet version (StreamStore.S3.AWS)](https://img.shields.io/nuget/v/StreamStore.S3.AWS.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.S3.AWS/)       |
  | [StreamStore.S3.B2]    | [`Backblaze B2`] implementation                                                      | [![NuGet version (StreamStore.S3.B2)](https://img.shields.io/nuget/v/StreamStore.S3.B2.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.S3.B2/)          |

[StreamStore.Sql.PostgreSql]:https://www.nuget.org/packages/StreamStore.Sql.PostgreSql/
[StreamStore.Sql.Sqlite]:https://www.nuget.org/packages/StreamStore.Sql.Sqlite/
[StreamStore.InMemory]:https://www.nuget.org/packages/StreamStore.InMemory/
[StreamStore.S3.AWS]:https://www.nuget.org/packages/StreamStore.S3.AWS/
[StreamStore.S3.B2]:https://www.nuget.org/packages/StreamStore.S3.B2
[`Backblaze B2`]: https://www.backblaze.com/b2/cloud-storage.html
[`Amazon S3`]: https://aws.amazon.com/s3/
More information you can find in the [documentation](https://github.com/kostiantyn-matsebora/streamstore).
