# StreamStore.S3.B2

[![NuGet version (StreamStore.S3.B2)](https://img.shields.io/nuget/v/StreamStore.S3.B2.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.S3.B2/)

[Backblaze B2] backend database for [StreamStore] event stream library.

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

To be able to use library you need to have already created a bucket in [Backblaze B2], by default named `streamstore` and have read and write permissions to it.

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
   // Adding StreamStore
   services.ConfigureStreamStore();

   // Adding B2 database with configuration from appsettings.json
   services.UseB2StreamStoreDatabase(Configuration);

  // Or configuring it manually
  services
    .ConfigureB2StreamStoreDatabase()
      .WithBucketId("your-bucket-id")
      .WithBucketName("your-bucket-name")
      .WithCredentials("your-application-key-id","your-application-key")
    .Configure();

```

- For usage of StreamStore, please refer to the [StreamStore] documentation.

## Good to know

- The library implements `two-phase locking mechanism` for optimistic concurrency control. First, it trying to lock in memory and if it is successful, it tries to lock in the storage.

- Since B2 does not provide locking mechanism for files, lock in storage implemented by creating a file with the same name as the stream id and trying to lock it by creating a file with the same name and checking if it is already exists.

- Each event is stored in a separate file with the name of the event id.

- Each stream is stored in a separate folder with the name of the stream id.

## Example

You can find an example of usage in the [StreamStore.S3.B2.Example](https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.S3.Example) project.

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
