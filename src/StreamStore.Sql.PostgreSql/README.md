# StreamStore.Sql.PostgreSql

[![NuGet version (StreamStore.Sql.PostgreSql)](https://img.shields.io/nuget/v/StreamStore.Sql.PostgreSql.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Sql.PostgreSql/)

[PostgreSQL] backend database for [StreamStore] asynchronous event sourcing library.

## Installation

To install the package, you can use the following command from the command line:

```dotnetcli
  dotnet add package StreamStore

  dotnet add package StreamStore.Sql.PostgreSql
```

or from Nuget Package Manager Console:

```powershell

  Install-Package StreamStore

  Install-Package StreamStore.Sql.PostgreSql
```

## Usage

### Storage

By default the library provisioning tables automatically, however you must create database or provide connection string to existing database.

If you want to create table manually, you can use the following script:

```sql
 CREATE TABLE IF NOT EXISTS public.Events (
     Id text NOT NULL,
     StreamId text NOT NULL,
     Timestamp timestamp NOT NULL,
     Revision integer NOT NULL,
     Data bytea NOT NULL,
     PRIMARY KEY (Id, StreamId)
 );

 CREATE INDEX IF NOT EXISTS ix_events_stream_id ON Events(StreamId);
 CREATE INDEX IF NOT EXISTS ix_events_stream_revision ON Events(Revision);
 CREATE UNIQUE INDEX IF NOT EXISTS ix_events_stream_id_revision ON Events(StreamId, Revision);
```

### Configuration

You can define configuration of the library in `appsettings.json` file:

```json
{
  "ConnectionStrings": {
    "StreamStore": "Host=localhost;Port=5432;Database=streamstore;Username=streamstore;Password=streamstore" // Required
  },
  "StreamStore": {             // Optional
    "PostgreSql": {
      "SchemaName": "public",  // Optional
      "TableName": "Events",   // Optional
      "ProvisionSchema": true, // Optional
    }
  }
}
```

### Register in DI container

```csharp
   // Adding StreamStore
   services.ConfigureStreamStore();
 // Adding PostgreSQL database with configuration from appsettings.json, requires ConnectionStrings:StreamStore
   services.ConfigureStreamStore(x => x.UsePostgresDatabase(Configuration));

  // Or configuring it manually
   services.ConfigureStreamStore(x =>
      x.UsePostgresDatabase(c => {
           x.WithConnectionString("your-connection-string") // Connection string, required.
           x.WithSchema("your-schema-name");                // Schema name, optional, default is "public"
           x.WithTableName("your-table-name");              // Table name, optional, default is "Events"
           x.ProvisionSchema(false);                        // Provision schema, optional, default is true
      })
   );
```

### Use in application code

How to use [StreamStore] in your application code you can find on StreamStore [page][Usage].

## Example

You can find an example of usage in the [StreamStore.Sql.Example](https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.Sql.Example) project.

### Testing

In order to run tests placed in [StreamStore.Sql.Tests/PostgreSql](https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.Sql.Tests/PostgreSql/), you need to have a running PostgreSQL instance. You can use docker-compose [docker-compose.yaml](https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.Sql.Tests/PostgreSql/docker-compose.yaml) to run it.

## License

[`MIT License`](../../LICENSE)

[StreamStore]: https://github.com/kostiantyn-matsebora/streamstore/tree/master
[Usage]: https://github.com/kostiantyn-matsebora/streamstore/tree/master#Usage
[PostgreSQL]: https://www.postgresql.org/