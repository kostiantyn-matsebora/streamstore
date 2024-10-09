# StreamStore.S3.B2

[![NuGet version (StreamStore.S3.AWS)](https://img.shields.io/nuget/v/StreamStore.S3.AWS.svg?style=flat-square)](https://www.nuget.org/packages/StreamStore.S3.B2/)

[Amazon S3] backend database for [StreamStore] event stream library.

## Installation

To install the package, you can use the following command from the command line:

```dotnetcli
  dotnet add package StreamStore

  dotnet add package StreamStore.S3.AWS
```

or from Nuget Package Manager Console:

```powershell

  Install-Package StreamStore

  Install-Package StreamStore.S3.AWS
```

## Usage

### Storage

To be able to use library you need to:

- Create an S3 bucket, by default named `streamstore` and have read, write, list and delete permissions to it.
- Enable bucket versioning.

### Configuration

Library is using an official Amazon [AWSSDK.S3] nuget package, so you must follow the [AWSSDK.S3] configuration steps.


or you can provide the configuration in code, see section below.

### Register in DI container

```csharp
   // Adding StreamStore
   services.ConfigureStreamStore();

   // Adding B2 database with configuration from appsettings.json
   services.ConfigureS3AmazonStreamStoreDatabase(Configuration);

  // Or configuring it manually
  services
    .UseS3AmazonStreamStoreDatabase()
      .WithBucketName("your-bucket-name")
    .Configure();

```

- For usage of StreamStore, please refer to the [StreamStore] documentation.

## Good to know

- The library implements `two-phase locking mechanism` for `pessimistic concurrency control` on stream level.  
First, it trying to lock in memory and if it is successful, it tries to exclusively lock stream in the storage for the duration of the transaction. If it fails to lock in storage, it will release the lock in memory.

- Since B2 does not provide locking mechanism for files, lock in storage implemented by creating a file with the same name as the stream id and trying to lock it by creating a file with the same name and checking if it is already exists.

- Committed and uncommitted events are stored in separate root directories.

- Each event is stored in a separate file with the name of the event id.

- Each stream is stored in a separate directory with the name of the stream id.

### Storage structure

- `persistent-streams` - committed streams
  - `[stream-id]` - directory with stream data
    - `events` - directory with events
      - `[event-id]` - file with event data
    - `__metadata` - file with stream metadata
  - `transient-streams` - uncommitted streams
    - `[stream-id]` - directory with stream transactions
      - `[transaction-id]` - directory with transaction data
        - `events` - directory with events
          - `[event-id]` - file with event data
        - `__metadata` - file with transaction metadata
  - `locks` - directory with locks
    - `[stream-id]` - file with lock data

## Example

You can find an example of usage in the [StreamStore.S3.Example](https://github.com/kostiantyn-matsebora/streamstore/tree/master/src/StreamStore.S3.Example) project.

## Testing

To be able to run tests from [StreamStore.S3.Tests](../StreamStore.S3.Tests/) project, you need to create empty `appsettings.Development.json` file.

## License

[`MIT License`](../../LICENSE)

[Amazon S3]: https://aws.amazon.com/s3/
[AWSSDK.S3]: https://www.nuget.org/packages/AWSSDK.S3/
[StreamStore]: https://github.com/kostiantyn-matsebora/streamstore/tree/master
