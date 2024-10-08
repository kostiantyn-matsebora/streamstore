# StreamStore.InMemory

[![NuGet version (StreamStore.InMemory)](https://img.shields.io/nuget/v/StreamStore.InMemory.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.InMemory/)

In-memory database backend for [`StreamStore`](https://github.com/kostiantyn-matsebora/streamstore/tree/master) storage is provided **for testing and educational purposes only**.

## Installation

To install the package, you can use the following command from the command line:

```dotnetcli
  dotnet add package StreamStore

  dotnet add package StreamStore.InMemory
```

or from Nuget Package Manager Console:

```powershell
  Install-Package StreamStore

  Install-Package StreamStore.InMemory
```

## Usage


- Register store in DI container
  
```csharp
   // Adding StreamStore
   services.ConfigureStreamStore();
   // Adding InMemory database
   services.AddInMemoryStreamDatabase();
```

- How to use store in your application you can find in [StreamStore](https://github.com/kostiantyn-matsebora/streamstore/tree/master#usage) documentation

## License

[`MIT License`](../../LICENSE)
