# Serialization

Serialization is quite important aspect of event sourcing solution.  
`StreamStore` supports binary serialization and JSON serialization out of the box, as well provides compression optionally:

| Method                                                                                                            | Library                                                                             | Description      |
|-------------------------- |---------------------------------------------------------------------------------------|-----------------------------------------------------------------------------------------------------------|
| [SystemTextJsonEventSerializer](../src/StreamStore.Serialization/SystemTextJsonEventSerializer.cs)                     | Built-in    | Based on [System.Text.Json.JsonSerializer](https://learn.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializer?view=net-8.0). Fastest JSON serializer, but does not support circular references.                          |
| [NewtonsoftEventSerializer](../src/StreamStore.Serialization/NewtonsoftEventSerializer.cs)                        | Built-in    | Based on [Newtonsoft.Json](https://www.newtonsoft.com/json). The slowest one but used by default as proven solution.                                     |
| `ProtobufEventSerializer`                                                                                         | [StreamStore.Serialization.Protobuf](../src/StreamStore.Serialization.Protobuf/)    | Based on [protobuf-net](https://github.com/protobuf-net/protobuf-net). Serializes directly to binary form, does not require conversion from/to string. Fastest deserialization. |

## Process

Taking into account that events needs to be deserialized to original type, serialization (and deserialization) is two step process:

* First, event is serialized to byte array.
* Then, it is wrapped to `EventEnvelope` object, which contains  information about event type, and this object is serialized as well.

## Usage

```csharp
  // Register configure serialization in DI container, all configuration is optional

  services.ConfigureStreamStore(c =>
  {
    c => c.ConfigureSerialization(s =>
    {
          s.EnableCompression();                                // Enable compression. Default: false
          s.UseTypeRegistry<MyCustomTypeRegistry>();            // Custom type registry. Default: TypeRegistry

          // Register serializer. Default is Newtonsoft.Json
          s.UseSystemTextJsonSerializer(compression: true);     // SystemTextJson
          s.UseProtobufSerializer(compression: true);           // Protobuf 
          s.UseSerializer<YourCustomEventSerializer>();         // Custom serializer
    });
  });
```

### ProtobufEventSerializer

* Install package `StreamStore.Serialization.Protobuf`:

```dotnetcli
  dotnet add package StreamStore.Serialization.Protobuf
```

## Customization

To be able to use your own serialization/deserialization implementation you need to implement `IEventSerializer` interface:

```csharp
  public interface IEventSerializer
  {
    byte[] Serialize(object @event);
    object Deserialize(byte[] data);
  }
```

However, there is already two abstract classes that provides partial implementation (including compression) you can inherit from:

* [EventSerializerBase](../src/StreamStore.Serialization/EventSerializerBase.cs) - for binary serialization.
* [StringEventSerializerBase](../src/StreamStore.Serialization/StringEventSerializerBase.cs) - for string(JSON/XML etc.) serialization.

### Type resolution

Provided serializers are using [TypeRegistry](../src/StreamStore.Serialization/TypeRegistry.cs) to resolve type by name and vice versa.  
It is pretty simple in-memory cache, however it does pre-cache all types in the domain, so it can be slow on startup for large systems.

You can implement your own type resolution mechanism by implementing `ITypeRegistry` interface:

```csharp
  public interface ITypeRegistry
  {
    string ResolveNameByType(Type type);
    Type ResolveTypeByName(string name);
  }
```

Then register it in DI container:

```csharp
  services.AddSingleton<ITypeRegistry, MyCustomTypeRegistry>();
```

## Benchmarking

### Legends and hardware

```text
// * Summary *

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.4317/23H2/2023Update/SunValley3)
Intel Core i9-10900K CPU 3.70GHz, 1 CPU, 20 logical and 10 physical cores
.NET SDK 8.0.401
  [Host]     : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2

// * Legends *
  Compress : Value of the 'Compress' parameter
  Mean     : Arithmetic mean of all measurements
  Error    : Half of 99.9% confidence interval
  StdDev   : Standard deviation of all measurements
  1 us     : 1 Microsecond (0.000001 sec)

```

### Serialization

| Method                    | Compress | Mean      | Error     | StdDev    |
|-------------------------- |--------- |----------:|----------:|----------:|
| **SystemTextJsonSerializer**  | **False**    |  **5.141 μs** | **0.0541 μs** | **0.0423 μs** |
| NewtonsoftEventSerializer | False    |  8.535 μs | 0.0715 μs | 0.0669 μs |
| ProtobufEventSerializer   | False    |  9.459 μs | 0.0583 μs | 0.0546 μs |
| **SystemTextJsonSerializer**  | **True**     | **35.208 μs** | **0.1236 μs** | **0.1032 μs** |
| NewtonsoftEventSerializer | True     | 38.868 μs | 0.1315 μs | 0.1230 μs |
| ProtobufEventSerializer   | True     | 32.255 μs | 0.2896 μs | 0.2709 μs |

### Deserialization

| Method                    | Compress | Mean      | Error     | StdDev    |
|-------------------------- |--------- |----------:|----------:|----------:|
| **SystemTextJsonSerializer**  | **False**    |  **5.913 μs** | **0.0264 μs** | **0.0206 μs** |
| NewtonsoftEventSerializer | False    | 12.228 μs | 0.0788 μs | 0.0658 μs |
| ProtobufEventSerializer   | False    |  2.808 μs | 0.0164 μs | 0.0153 μs |
| **SystemTextJsonSerializer**  | **True**     | **12.449 μs** | **0.0636 μs** | **0.0496 μs** |
| NewtonsoftEventSerializer | True     | 19.059 μs | 0.0814 μs | 0.0722 μs |
| ProtobufEventSerializer   | True     |  8.252 μs | 0.0444 μs | 0.0346 μs |

## TypeRegistry

| Method            | Mean     | Error   | StdDev  |
|------------------ |---------:|--------:|--------:|
| ResolveNameByType | 141.4 ns | 0.46 ns | 0.41 ns |
| ResolveTypeByName | 160.6 ns | 0.82 ns | 0.76 ns |
