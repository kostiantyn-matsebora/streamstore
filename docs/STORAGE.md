# Storage

Storage is a persistence layer for the StreamStore library.

## Create your own storage implementation

### Implement your own storage from scratch

To create your own storage implementation, you need to implement the following interfaces:

### Single tenant mode

- [IStreamStorage] - provides methods for working with streams.
- [ISchemaProvisioner] - provides methods for provisioning storage schema.

### Multitenant mode

- [ITenantStreamStorageProvider] interface, provider of IStreamStorage for particular tenant.
- [ITenantSchemaProvisionerFactory] interface, factory of ISchemaProvisioner for particular tenant.

![Storage class diagram](diagrams/storage.png)

However, there is much easier way to do so.

## Use base implementations

You can use base implementations of the interfaces provided by `StreamStore.Storage` package.

For to do so, you need to create implementation of the following abstract classes:

* [StorageConfiguratorBase] - provides methods for configuring storage and registering it in DI container.

  <details>
  <summary><b>Example: SQLite storage configurator</b></summary>
  
  ```csharp
  // Example of SQLite storage configurator
  internal class StorageConfigurator: StorageConfiguratorBase
      {
  
          readonly SqlStorageConfiguration config = SqliteConfiguration.DefaultConfiguration;
  
          public StorageConfigurator()
          {
          }
  
          public StorageConfigurator(SqlStorageConfiguration configuration)
          {
              config = configuration.ThrowIfNull(nameof(configuration));
          }
  
  
          protected override void ConfigureStorage(StorageDependencyRegistrator registrator)
          {
              registrator.RegisterStorage<SqlStreamStorage>();
          }
  
          protected override void ConfigureSchemaProvisioner(SchemaProvisionerRegistrator registrator)
          {
              // Register the PostgreSQL schema provisioner
              registrator.RegisterSchemaProvisioner<SqlSchemaProvisioner>();
          }
  
          protected override void ConfigureAdditionalDependencies(IServiceCollection services)
          {
              services.AddSingleton(config);
              services.AddSingleton<IDbConnectionFactory, SqliteDbConnectionFactory>();
              services.AddSingleton<IDapperCommandFactory, DefaultDapperCommandFactory>();
              services.AddSingleton<ISqlExceptionHandler, SqliteExceptionHandler>();
              services.AddSingleton<ISqlQueryProvider, DefaultSqlQueryProvider>();
              services.AddSingleton<IMigrator, SqliteMigrator>();
              services.AddSingleton(new MigrationConfiguration { MigrationAssembly = typeof(SqliteMigrator).Assembly });
          }
      }
  ```
  
  </details>

* [MultitenancyConfiguratorBase] - provides methods for configuring multitenant aspect of storage and registering it in DI container.

  <details>
  <summary><b>Example: SQLite multitenancy configurator</b></summary>
  
  ```csharp
   internal class MultitenancyConfigurator : MultitenancyConfiguratorBase
    {
        readonly Action<SqlMultitenancyConfigurator> configure;

        public MultitenancyConfigurator(Action<SqlMultitenancyConfigurator> configure)
        {
            this.configure = configure.ThrowIfNull(nameof(configure));
        }

        protected override void ConfigureStorageProvider(StorageProviderRegistrator registrator)
        {
            registrator.RegisterStorageProvider(serviceProvider => serviceProvider.GetRequiredService<SqliteTenantStorageProvider>().GetStorage);
        }

        protected override void ConfigureSchemaProvisionerFactory(SchemaProvisionerFactoryRegistrator registrator)
        {
            registrator.RegisterSchemaProvisioningFactory(provider =>
                provider.GetRequiredService<SqliteSchemaProvisionerFactory>().Create);
        }

        protected override void ConfigureAdditionalDependencies(IServiceCollection services)
        {
            services.ThrowIfNull(nameof(services));
            services.AddSingleton<ISqlTenantStorageConfigurationProvider,SqlTenantStorageConfigurationProvider>();
            services.AddSingleton<SqliteTenantStorageProvider>();
            configure(new SqlMultitenancyConfigurator(services));
            services.AddSingleton<SqliteSchemaProvisionerFactory>();
        }
    }
  ```

  </details>

After that you can register your storage implementation in the DI container by creating extension method for `IServiceCollection` and using existing [extensions](../src/StreamStore.Storage/Extensions/ServiceCollectionExtension.cs).

<details>
<summary><b>Example: SQLite storage service collection extensions</b></summary>

```csharp
  public static class ServiceCollectionExtension
  {
      public static IServiceCollection ConfigurePersistenceMultitenancy(this IServiceCollection services, StorageConfiguratorBase configurator, MultitenancyConfiguratorBase multitenancyConfigurator)
      {
              configurator.ThrowIfNull(nameof(configurator));
              multitenancyConfigurator.ThrowIfNull(nameof(multitenancyConfigurator));
  
              var storageServices = 
                  new StorageDependencyBuilder()
                      .WithStorageConfigurator(configurator)
                      .WithMultitenancyConfigurator(multitenancyConfigurator)
                      .Build();
              services.CopyFrom(storageServices);
              return services;
          }
          public static IServiceCollection ConfigurePersistence(this IServiceCollection services, StorageConfiguratorBase configurator)
          {
              configurator.ThrowIfNull(nameof(configurator));
  
              var storageServices =
                  new StorageDependencyBuilder()
                      .WithStorageConfigurator(configurator)
                      .Build();
              services.CopyFrom(storageServices);
              return services;
          }

  }
```

</details>


## Considerations

- To implement your own storage you do not need StreamStore package, all necessary interfaces are located in [StreamStore.Storage.Contracts](https://www.nuget.org/packages/StreamStore.Contracts/) and base implementations in  [StreamStore.Storage](https://www.nuget.org/packages/StreamStore.Storage/) package.

- _You can register your own storage implementation in the DI container using any kind of lifetime (i.e. Singleton, Transient, Scoped, etc.)_  
  However, if you register it as a singleton, you should be aware that it should be thread-safe and preferably stateless.

- _Solution already provides optimistic concurrency and event duplication control mechanisms, as a **pre-check** during stream opening_.  
  However, if you need consistency guaranteed, you should implement your own mechanisms as a part of [IStreamWriter] implementation.  
  For instance, you can use a transaction mechanism supported by `ACID compliant DBMS`.
- _Get and Delete operations must be implemented as idempotent by their nature._

[IStreamWriter]:../src/StreamStore.Storage.Contracts/Storage/IStreamWriter.cs
[IStreamStorage]:../src/StreamStore.Storage.Contracts/Storage/IStreamStorage.cs
[ISchemaProvisioner]:../src/StreamStore.Storage.Contracts/Provisioning/ISchemaProvisioner.cs
[ITenantStreamStorageProvider]:../src/StreamStore.Storage.Contracts/Multitenancy/ITenantStreamStorageProvider.cs
[ITenantSchemaProvisionerFactory]:../src/StreamStore.Storage.Contracts/Multitenancy/ITenantSchemaProvisionerFactory.cs
[StorageConfiguratorBase]:../src/StreamStore.Storage/Configuration/StorageConfiguratorBase.cs

[MultitenancyConfiguratorBase]:../src/StreamStore.Storage/Configuration/MultitenancyConfiguratorBase.cs
