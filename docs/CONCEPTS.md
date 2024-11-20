# Basic concepts

## Concepts

### Event Stream

Stream is a sequence of events made available over time **ordered by revision**.  
Streams are used in many areas, here is just few examples of event streams:

- State of DDD aggregate.
- Financial transactions of user.
- Sequence of telemetry data from IoT device.
- etc.

### Event

An event is a fact. The domain may be updated to reflect the fact represented by the event.  
Each event has a unique identifier, a timestamp, and a payload.

### Stream ID

A stream identifier is a unique identifier of the stream.
It is an abstraction of the sequence of events, guaranteeing `uniqueness and order, as well isolation`.  
Example of stream IDs:

- ID of DDD aggregate.
- ID of user.
- ID of IoT device.
- etc.

### Event ID

Event identifier is a unique identifier of the event. Guarantee uniqueness of the event in the stream.

### Revision

Revision is a sequence number of the event in the stream. It is used to:

- Order events in the stream.
- Establish optimistic concurrency control.

Despite the fact that usually event timestamp is used to order events, it is not really reliable, since there could be a clock skew between different part of the system
in distributed architecture.

### StreamStore

Stream store is an abstraction of particular storage that allows to read and write events to the stream sequentially.

## Considerations

- _[`Id`][Id]  is a value object (immutable class) that has implicit conversion from and to string_.  

  Thus you don't need to create [Id] object explicitly and use `ToString()` to convert to string back.  
  Also implements `IEquatable`  for [itself][Id] and for `String`.
- _[`Revision`][Revision] is a value object (immutable class) that represents a revision of the stream._  
  It is used for optimistic concurrency control and event ordering.
  It has implicit conversion from and to `Int32` type.  
  Also implements `IEquatable` and `IComparable` for itself and for `Int32`.

- You can read from any stream starting from the provided revision.

- _`ReadToEnd` method  returns collection of events from the stream starting from the provided revision_:
  - Contains only **unique events ordered by revision**.
  - Contains only **events that were committed**.
  
- _Stream revision is always the revision of an event with maximum revision value_.

- _Idempotency of reading and deletion fully depends on particular storage implementation._

- _You don't need to retrieve stream  to add events to it_.  
  Appending to stream and getting stream  are separate operations.

- _Despite the fact that reading is declared as asynchronous and iterative operation, for the sake of performance it is implemented as paginated operation._

  You can define the page size by using `WithReadingPageSize` method of store configuration, by default it is 10 events.

- _Reading and writing operations are not thread-safe_.  
 Thus, it is not recommended to use the same instances of `IStreamWriter` or `IAsyncEnumerable<StreamEvent>` in multiple threads.

[Id]: ../src/StreamStore.Contracts/Id.cs
[Revision]: ../src/StreamStore.Contracts/Revision.cs
