# StreamStore.Sql.Sqlite

[![NuGet version (StreamStore.Sql.Sqlite)](https://img.shields.io/nuget/v/StreamStore.Sql.Sqlite.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.Sql.Sqlite/)

[SQLite] storage for [StreamStore] asynchronous event sourcing library.

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
      CustomProperties TEXT,
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
    "StreamStore": "Data Source=streamstore.db" // Required for single tenant configuration
  },
  "StreamStore": { // Optional
    "Sqlite": {
      "SchemaName": "main", // Optional
      "TableName": "Events", // Optional
      "ProvisionSchema": true, // Optional
    }
  }
}
```

### Register in DI container

```csharp
services.ConfigureStreamStore(x =>...

  // Register single storage implementation
  x.WithSingleStorage(c => ...
      c.UseSqliteStorage(x =>
          c => c.ConfigureStorage(x =>                        // Configure storage options.
            x.WithConnectionString("your-connection-string")   // Required. Connection string.
            x.WithSchema("your-schema-name");                  // Optional. Schema name, default is "main".
            x.WithTableName("your-table-name");                // Optional. Table name, default is "Events".
      )
  )

  // Or enable multitenancy
  x.WithMultitenancy(c => ...
      c.UseSqliteStorage(x => 
          x.WithConnectionStringProvider<Provider>()          // Required. Register your 
                                                              // ISqlTenantConnectionStringProvider implementation.
          c => c.ConfigureStorage(x =>...)                   // Optional. Configure storage options will be used as 
                                                              // template for tenant storage configuration, optional.
      )
  )
); 
```

### Use in application code

How to use [StreamStore] in your application code you can find on StreamStore [page][Usage].

## Example

You can find an example of usage in the [StreamStore.Sql.Example](https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.Sql.Example) project.

## License

[`MIT License`](../../LICENSE)

[StreamStore]: https://github.com/kostiantyn-matsebora/streamstore/tree/master
[Usage]: https://github.com/kostiantyn-matsebora/streamstore/tree/master#Usage
[SQLite]: https://www.sqlite.org/index.html
