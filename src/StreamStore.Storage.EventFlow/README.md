# StreamStorage for EventFlow

Adapter of [`StreamStore`]'s [storage backends] to use as [`EventFlow`]'s event store.

## What

Implements [IEventPersistence] interface uses [IStreamStorage] interface implementation of any [`StreamStore`]'s [storage backends].

## Why

Why would you need this?

Despite the fact that [`EventFlow`] solution already contains many implementations of persistence, such as SQL, EventStore, ElasticSearch etc., there is not so many NoSQL backends provided.

So, **if you are using or decided to use [`EventFlow`] want to high available distributed storage, such as [`Cassandra`]**, you should consider to use this component. 

> [!WARNING]
  Since [`StreamStore`] does not support global positioning, methods using global positioning are not implemented.

> [!IMPORTANT]
> Component does not implement adapter of [`StreamStore`] solution, only the storage backend part, so functionality like automatic schema provisioning, earlier detection of event duplication etc. is not available.

## How

How to use it? It is pretty easy.

First, if you just decided to use [`EventFlow`], you must install EventFlow package:

```dotnetcli
  dotnet add package EventFlow
```

Then install adapter:

```dotnetcli
  dotnet add package StreamStore.Storage.EventFlow
```

After that install particular [`StreamStore`] storage backend, for instance Cassandra:

```dotnetcli
dotnet add package StreamStore.NoSql.Cassandra
```

Finally, register and configure [`EventFlow`] in DI container:

```csharp
using var serviceCollection = new ServiceCollection()
    // ...
    .AddEventFlow(e => 
    // ...
    // Register adapter
      e => e.UseStreamStorageEventStore(services =>
      // Register and configure particular storage backend, for instance Cassandra
      // How to configure storage backend, you can find in documentation of particular one.
      // For instance here:
      // https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.NoSql.Cassandra#configuration-options
        services.UseCassandra(...);
     )
    )
    // ...
    .BuildServiceProvider();
```

More information about [`EventFlow`] configuration you can find in [official documentation](https://geteventflow.net/additional/configuration/).

## Multitenancy

Despite the fact, that [`EventFlow`] itself does not have explicit support of multitenancy, this component provides such ability in a tricky way.

To be able to use adapter in multitenant mode, you must create and register implementation if [ITenantIdResolver] interface, responsible for resolution of tenant identifier based on current context, for example `HtttpContextTenantIdResolver` resolves tenant identifier based on HTTP request:

```csharp
using var serviceCollection = new ServiceCollection()
    // ...
    .AddEventFlow(e => 
    // ...
    // Register adapter
      e => e.UseStreamStorageEventStore<HtttpContextTenantIdResolver>(services =>
      // Register and configure particular storage backend, for instance Cassandra
      // How to configure storage backend, you can find in documentation of particular one.
      // For instance here:
      // https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.NoSql.Cassandra#configuration-options
        services.UseCassandraWithMultitenancy(...);
     )
    )
    // ...
    .BuildServiceProvider();
```

## Schema Provisioning

Automatic schema provisioning is a part of [`StreamStore`] solution implemented as background service and not available as part of component.

However, you can still use schema provisioning implementation of particular storage.

For single tenant mode:

```csharp
var serviceProvider = serviceCollection.BuildServiceProvider();

// Get provisioner from DI container
var provisioner = serviceProvider.GetRequiredService<ISchemaProvisioner>();

// Run schema provisioning
await provisioner.ProvisionSchemaAsync(token);

```

For multitenancy:

```csharp

var serviceProvider = serviceCollection.BuildServiceProvider();

// Get provisioner factory from DI container
var provisionerFactory = serviceProvider.GetRequiredService<ITenantSchemaProvisionerFactory>();

// Iterate tenants
foreach (var tenantId in tenants) {
  // Get provisioner for particular tenant
  var provisioner = provisionerFactory.Create(tenantId);
  // Run schema provisioning for tenant
  await provisioner.ProvisionSchemaAsync(token);
}
```

[`EventFlow`]: https://github.com/eventflow/EventFlow
[`StreamStore`]: https://github.com/kostiantyn-matsebora/streamstore 
[storage backends]: https://github.com/kostiantyn-matsebora/streamstore?tab=readme-ov-file#storage-packages
[IEventPersistence]: https://github.com/eventflow/EventFlow/blob/develop-v1/Source/EventFlow/EventStores/IEventPersistence.cs
[IStreamStorage]: https://github.com/kostiantyn-matsebora/streamstore/blob/master/src/StreamStore.Storage.Contracts/Storage/IStreamStorage.cs
[`Cassandra`]: https://cassandra.apache.org/_/index.html
[ITenantIdResolver]: https://github.com/kostiantyn-matsebora/streamstore/blob/master/src/StreamStore.Storage.EventFlow/ITenantIdResolver.cs

