# Customization

To implement your own database you do not need StreamStore package, all necessary interfaces are located in StreamStore.Contracts package from command line:

```dotnetcli
  dotnet add package StreamStore.Contracts
```

or from NuGet Package Manager Console:

```powershell
  Install-Package StreamStore.Contracts
```

## Serialization

About serialization you can read in [SERIALIZATION](SERIALIZATION.md) file.

## Create your own storage implementation

To create your own database implementation, you need to implement the following interfaces:

- [`IStreamDatabase`][IStreamDatabase] - provides methods for working with streams.
  Create your own implementation based on [`StreamDatabaseBase`](../src/StreamStore.Contracts/Database/StreamDatabaseBase.cs) abstract class.

- [`IStreamUnitOfWork`][IStreamUnitOfWork] - provides methods for appending events to the stream and saving changes.  
  Create your own implementation based on [`StreamUnitOfWorkBase`](../src/StreamStore.Contracts/Database/StreamUnitOfWorkBase.cs)
  and override following methods:

  ```csharp
    class MyStreamUnitOfWork: StreamUnitOfWorkBase
    {
      protected override Task SaveChangesAsync(EventRecordCollection uncommited, CancellationToken token)
      {
        // Implement saving logic
      }
  
      protected override Task OnEventAdded(EventRecord @event, CancellationToken token)
      {
            // Optionally implement logic for handling event added, 
            // such as instance logging, puting event to outbox or temporary storage etc.
      }

     protected override void Dispose(bool disposing)
     {
        // Optionally implement disposing logic
     }
    }
  ```

  Default serializer is using `Newtonsoft.Json` library, so you can create your own using `System.Text.Json` or any other, by
  implementing [`IEventSerializer`](../src/StreamStore.Contracts/Serialization/IEventSerializer.cs) interface.

### Considerations

- To implement your own database you do not need StreamStore package, all necessary interfaces are located in [StreamStore.Contracts](https://www.nuget.org/packages/StreamStore.Contracts/) package.
- _You can register your own database implementation in the DI container using any kind of lifetime (i.e. Singleton, Transient, Scoped, etc.)_  

  However, if you register it as a singleton, you should be aware that it should be thread-safe and preferably stateless.

- _Solution already provides optimistic concurrency and event duplication control mechanisms, as a **pre-check** during stream opening_.  

  However, if you need consistency guaranteed, you should implement your own mechanisms as a part of [IStreamUnitOfWork] implementation.  
  For instance, you can use a transaction mechanism supported by `ACID compliant DBMS`.

- _Get and Delete operations must be implemented as idempotent by their nature._

[IStreamUnitOfWork]: ../src/StreamStore.Contracts/Database/IStreamUnitOfWork.cs
[IStreamDatabase]: ../src/StreamStore.Contracts/Database/IStreamDatabase.cs
