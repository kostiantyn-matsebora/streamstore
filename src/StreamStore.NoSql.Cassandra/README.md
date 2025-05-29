# StreamStore.NoSql.Cassandra

[![NuGet version (StreamStore.NoSql.Cassandra)](https://img.shields.io/nuget/v/StreamStore.NoSql.Cassandra.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.NoSql.Cassandra/)

[Apache Cassandra] and [Azure Cosmos DB for Apache Cassandra] storage for [StreamStore] asynchronous event sourcing library built on top of [CassandraCSharpDriver].

> [!IMPORTANT]
> Does not support **event duplication detection based on event ID** due to lack of unique constraints.

## ACID compliance and considerations

Despite the fact that [Apache Cassandra] is not ACID compliant DBMS, library provides ACID guarantees for the operations that are performed within a single partition key - stream identifier.  

Such guarantees are achieved by using  following features:

* The `LWT` (Lightweight Transactions) feature of Cassandra.
* The [conditional](https://docs.datastax.com/en/developer/csharp-driver/3.22/features/components/mapper/batch/index.html#conditional-batches) [logged](https://docs.datastax.com/en/cql-oss/3.x/cql/cql_reference/cqlBatch.html#cqlBatch__cql-log-unlog) batches.
* The `ALL` [consistency level](https://docs.datastax.com/en/cassandra-oss/3.0/cassandra/dml/dmlConfigConsistency.html) for the read and write operations.
* The `SERIAL` [serial consistency level](https://docs.datastax.com/en/cassandra-oss/3.0/cassandra/dml/dmlConfigSerialConsistency.html) for the read  operations.

Taking into account that configuration above significantly affects the performance of the system, and that not all scenarios requires ACID compliance, it has been decided to add possibility to change consistency levels via configuration.

## Installation

To install the package, you can use the following command from the command line:

```dotnetcli
  dotnet add package StreamStore

  dotnet add package StreamStore.NoSql.Cassandra
```

or from Nuget Package Manager Console:

```powershell

  Install-Package StreamStore

  Install-Package StreamStore.NoSql.Cassandra
```

## Usage

### Storage

By default the library provisioning schema automatically, however you must create keyspace or configure library to use already existing one.  
By default the library uses `streamstore` keyspace in `single tenant` mode.  
For `multitenancy` mode you **must create keyspace per tenant in advance**.

If you want to create table manually, you can use the following script:

```sql
CREATE TABLE IF NOT EXISTS events
    (id text,
    stream_id text,
    revision int,
    timestamp timestamp,
    data blob,
    custom_properties map<text,text>,
    PRIMARY KEY(stream_id, revision)
    );
```

### Getting started

Since the library [CassandraCSharpDriver] as well as [Apache Cassandra] itself provides a lot of configuration options, it is not possible to cover all of them in the library configuration.  
In this regards, decision has been made to provide ability to configure connectivity options via original
API of [CassandraCSharpDriver].

However, in case if you decided to use library with [Azure Cosmos DB for Apache Cassandra], there is extension simplifies configuration by only connection string required.

```csharp
services.ConfigureStreamStore(x =>...
  
  // Register single tenant implementation
  x.WithSingleStorage(c => ...
      c.UseCassandra(x => {
          x.UseCosmosDb(connectionString);            // Optional. Required  if you want to use Azure Cosmos DB for Apache Cassandra
          x.ConfigureCluster(c =>                     // Required. Configure cluster options. Optional if you decided to use CosmosDB (see above).
              c.AddContactPoint("localhost"));        // Configure contact points.
         }
      )
  )

  // Or enable multitenancy
  x.WithMultitenancy(c => ...
      c.UseTenantProvider<MyTenantProvider>()          // Optional. Register your  ITenantProvider implementation.
                                                       // Required if you want schema to be provisioned for each tenant.
      c.UseCassandra(x => {
          x.WithKeyspaceProvider<Provider>();           // Required. Register your  ITenantKeyspaceProvider implementation.
          x.UseCosmosDb(connectionString);              // Optional. Required  if you want to use Azure Cosmos DB for Apache Cassandra
          x.ConfigureDefaultCluster(c =>                // Required. Configure cluster options. Optional if you decided to use CosmosDB (see above).
                  c.AddContactPoint("localhost"));      // Configure contact points.
        }
      )
  )
); 
```

Full list of configuration options you can find in the [Configuration options](#Configuration-options) section.

### Use in application code

How to use [StreamStore] in your application code you can find on StreamStore [page][Usage].

## Example

You can find an example of usage in the [StreamStore.NoSql.Example](https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.NoSql.Example) project.  
More information about how example application works you can find in [StreamStore](https://github.com/kostiantyn-matsebora/streamstore/tree/master/#example) documentation.

### Testing

In order to run tests placed in [StreamStore.NoSql.Tests/Cassandra/Storage](https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.NoSql.Tests/Cassandra/Storage), you need to have a running Cassandra cluster. You can use docker-compose [docker-compose.yaml](https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.NoSql.Tests/Cassandra/Storage/docker-compose.yaml) to run it.

## Configuration options

Below you can find the list of configuration options that can be used to configure the library.

```csharp
// Register single tenant implementation
  x.WithSingleStorage(c => ...
      c.UseCassandra(x => 
         {
            x.UseCosmosDb(connectionString);                                    // Optional. Required  if you want to use Azure Cosmos DB for Apache Cassandra.
            x.UseCosmosDb(Configuration, connectionStringName);                 // You can also provide IConfiguration and connection string name to Cosmos DB, by default "StreamStore".
            x.ConfigureCluster(c =>                                             // Required. Configure cluster options. Optional if you decided to use CosmosDB (see above).
                    c.AddContactPoint("localhost"));                            // Configure contact points at least.
                                                                                // There is much more cluster options available.
            x.ConfigureStorage(c =>                                             // Optional. Configure storage options.
                    c.WithKeyspaceName("keyspacename")                          // Optional. Keyspace name. Default is streamstore.
                     .WithEventsTableName("tablename")                          // Optional. Table name. Default is events.
                     .WithReadConsistencyLevel(ConsistencyLevel.Quorum)         // Optional. Read consistency level. Default is All.
                     .WithWriteConsistencyLevel(ConsistencyLevel.Quorum)        // Optional. Write consistency level. Default is All.
                     .WithSerialConsistencyLevel(ConsistencyLevel.SerialLocal)  // Optional. Serial consistency level. Default is Serial.
            );
            x.WithSessionFactory<TFactory>();                                   // Optional. Register your ISessionFactory implementation.
          }    
      )
  )

  // Or enable multitenancy
  x.WithMultitenancy(c => ...
                                                                                // More information about multitenancy configuration you can find in the StreamStore.
     c.UseCassandra(x => 
         {
            x.WithKeyspaceProvider<Provider>();                                 // Required. Register your ITenantKeyspaceProvider implementation.
            x.UseCosmosDb(connectionString);                                    // Optional. Required if you want to use Azure Cosmos DB for Apache Cassandra.
            x.UseCosmosDb(Configuration, connectionStringName);                 // You can also provide IConfiguration and connection string name to Cosmos DB, by default "StreamStore".
            x.ConfigureDefaultCluster(c =>                                      // Required. Configure default cluster options. Optional if you decided to use CosmosDB (see above).
                    c.AddContactPoint("localhost"));                            // Configure contact points at least.
                                                                                // There is much more cluster options available.
            x.ConfigureStoragePrototype(c =>                                    // Optional. Configure storage options as prototype for tenant storage configuration.
                    c.WithEventsTableName("tablename")                          // Optional. Table name. Default is events.
                     .WithReadConsistencyLevel(ConsistencyLevel.Quorum)         // Optional. Read consistency level. Default is All.
                     .WithWriteConsistencyLevel(ConsistencyLevel.Quorum)        // Optional. Write consistency level. Default is All.
                     .WithSerialConsistencyLevel(ConsistencyLevel.SerialLocal)  // Optional. Serial consistency level. Default is Serial.
            );
            x.WithTenantClusterConfigurator(c => ...);                          // Optional. Register delegate for configuring tenant cluster configuration based on default cluster.
            x.WithStorageConfigurationProvider<TProvider>();                    // Optional. Register your ITenantStorageConfigurationProvider implementation.
          }
      )
  )
```

## License

[`MIT License`](../../LICENSE)

[StreamStore]: https://github.com/kostiantyn-matsebora/streamstore/
[Apache Cassandra]: https://cassandra.apache.org/_/index.html
[Azure Cosmos DB for Apache Cassandra]: https://learn.microsoft.com/en-us/azure/cosmos-db/cassandra/introduction
[CassandraCSharpDriver]: https://docs.datastax.com/en/developer/csharp-driver/3.22/index.html
[Usage]: https://github.com/kostiantyn-matsebora/streamstore/tree/master#Usage
