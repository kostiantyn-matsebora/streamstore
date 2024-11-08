# StreamStore.S3.B2

[![NuGet version (StreamStore.S3.B2)](https://img.shields.io/nuget/v/StreamStore.S3.B2.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.S3.B2/)

[Backblaze B2] backend database for [StreamStore] asynchronous event sourcing library.

## Installation

To install the package, you can use the following command from the command line:

```dotnetcli
  dotnet add package StreamStore

  dotnet add package StreamStore.S3.B2
```

or from Nuget Package Manager Console:

```powershell

  Install-Package StreamStore

  Install-Package StreamStore.S3.B2
```

## Usage

### Storage

See storage requirements for [Amazon S3 database](https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.S3.AWS#storage) implementation.

### Configuration

For using [Backblaze B2] as a storage backend, you need to provide the following configuration in your `appsettings.json` file:

```json
{
  "StreamStore": {
    "B2": {
      "bucketName": "your-bucket-name",
      "bucketId": "your-bucket-id",
      "applicationKeyId": "your-application-key-id",
      "applicationKey": "your-application-key",
    }
  }
}
```

or you can provide the configuration in code, see section below.

### Register in DI container

```csharp
   // Adding B2 database with configuration from appsettings.json
   services.ConfigureStreamStore(x => x.UseB2Database(Configuration));

  // Or configuring it manually
   services.ConfigureStreamStore(x =>
      x.UseB2Database(c => {
           x.WithBucketId("your-bucket-id");
           x.WithBucketName("your-bucket-name");
           x.WithCredential("your-access-key-id","your-access-key");
      })
   );

```

- For usage of StreamStore, please refer to the [StreamStore] documentation.

## Good to know

See  [Amazon S3 database](https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.S3.AWS#good-to-know) documentation.

### Storage structure

See  [Amazon S3 database](https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.S3.AWS#storage-structure) documentation.

## Example

See  [Amazon S3 database](https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.S3.AWS#example) documentation.

## Testing

To be able to run tests from [StreamStore.S3.Tests](../StreamStore.S3.Tests/) project, you need to create and provide the following configuration in your `appsettings.Development.json` file:

```json
{
  "b2": {
      "bucketName": "your-bucket-name",
      "bucketId": "your-bucket-id",
      "applicationKeyId": "your-application-key-id",
      "applicationKey": "your-application-key",
    }
}
```

## License

[`MIT License`](../../LICENSE)


[Backblaze B2]: https://www.backblaze.com/b2/cloud-storage.html
[StreamStore]: https://github.com/kostiantyn-matsebora/streamstore/tree/master
