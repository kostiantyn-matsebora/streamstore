# StreamStore.Sql.Sqlite

[![NuGet version (StreamStore.Sql.Sqlite)](https://img.shields.io/nuget/v/StreamStore.Sql.Sqlite.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Sql.Sqlite/)

[SQLite] backend database for [StreamStore] event stream library.

## Installation

To install the package, you can use the following command from the command line:

```dotnetcli
  dotnet add package StreamStore

  dotnet add package StreamStore.Sql.Sqlite
```

or from Nuget Package Manager Console:

```powershell

  Install-Package StreamStore

  Install-Package StreamStore.Sql.Sqlite
```

## Usage

### Storage

By default the library provisioning tables automatically, however you must create database or provide connection string to existing database.

If you want to create table manually, you can use the following script:

```sql
  CREATE TABLE IF NOT EXISTS main.Events (
      Id TEXT NOT NULL,
      StreamId TEXT NOT NULL,
      Timestamp datetime2 NOT NULL, 
      Revision INTEGER NOT NULL,
      Data BLOB NOT NULL,
      PRIMARY KEY (Id, StreamId)
  );

  CREATE INDEX IF NOT EXISTS main.ix_streams_stream_id ON Events(StreamId);
  CREATE INDEX IF NOT EXISTS main.ix_streams_stream_revision ON Events(Revision);
  CREATE UNIQUE INDEX IF NOT EXISTS main.ix_streams_stream_id_revision ON Events(StreamId, Revision);
```

### Configuration

You can define configuration of the library in `appsettings.json` file:

```json
{
  "ConnectionStrings": {
    "StreamStore": "Data Source=streamstore.db" // Required
  },
  "StreamStore": { // Optional
    "Sqlite": {
      "SchemaName": "main", // Optional
      "TableName": "Events", // Optional
      "ProvisionSchema": true, // Optional
      "ProfilingEnabled": false // Optional
    }
  }
}
```

### Register in DI container

```csharp
   // Adding StreamStore
   services.ConfigureStreamStore();

   // Adding  Sqlite stream database and getting configuration from appsettings.json
   services.UseSqliteStreamDatabase(Configuration);

  // Or configuring it manually
  services
    .ConfigureSqliteStreamDatabase()
      .WithSchema("main")
      .WithTableName("Events")
      .ProvisioningSchema(true)
      .EnableProfiling()
    .Configure();
```

### Use in application code

How to use [StreamStore] in your application code you can find on StreamStore [page][Usage].

## Example

You can find an example of usage in the [StreamStore.Sql.Example](https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.Sql.Example) project.


## License

[`MIT License`](../../LICENSE)

[StreamStore]: https://github.com/kostiantyn-matsebora/streamstore/tree/master
[Usage]: https://github.com/kostiantyn-matsebora/streamstore/tree/master#usage
[SQLite]: https://www.sqlite.org/index.html
