# Basic concepts

## Concepts

### Event Stream

A stream is a sequence of events, organized by revision.
Streams are utilized in various domains. Here are a few examples of event streams:

* The state changes of a domain aggregate
* A user's financial transactions
* A series of telemetry readings from an IoT device
* And more.

### Event

An event is a fact. The domain may be updated to reflect the fact represented by the event.  
Each event has a unique identifier, a timestamp, and a payload.

### Stream ID

A stream identifier is a unique identifier of the stream.
It is an abstraction of the sequence of events, guaranteeing `uniqueness and order, as well isolation`.
Example of stream IDs:

* Identifier of domain aggregate root.
* Identifier of user.
* Identifier of IoT device.
* etc.

### Event ID

Event identifier is a unique identifier of the event. Guarantee uniqueness of the event in the stream.

### Revision

Revision is a sequence number of the event in the stream. It is used to:

* Order events in the stream.
* Establish concurrency control.

Despite the fact that event timestamp can be used to order events, it is not really reliable, since there could be a clock skew between different part of the system in distributed architecture.

### Custom properties

Collection of additional data you can store and retrieve for event. Can be used to store event metadata specific for particular solution.

### Stream Storage

Storage is a physical implementation of the event stream i.e. persistence layer. It is responsible for storing and retrieving events from the stream.

## Considerations

* _[`Id`][Id]  is a value object (immutable) that has implicit conversion from and to string_.  

  Thus you don't need to create [Id] object explicitly and use `ToString()` to convert to string back.  
  Also implements `IEquatable`  for [itself][Id] and for `String`.
* _[`Revision`][Revision] is a value object (immutable) that represents a revision of the stream._  
  It is used for optimistic concurrency control and event ordering.
  It has implicit conversion from and to `Int32` type.  
  Also implements `IEquatable` and `IComparable` for itself and for `Int32`.

* You can read from any stream starting from the provided revision.

* _`ReadToEnd` method  returns collection of events from the stream starting from the provided revision_:
  * Contains only **unique events ordered by revision**.
  * Contains only **events that were committed**.
  
* _Stream revision is always the revision of an event with maximum revision value_.

* _Idempotency of reading and deletion fully depends on particular storage implementation._

* _You don't need to retrieve stream  to add events to it_.
  Appending to stream and getting stream  are separate operations.

* _Despite the fact that reading is declared as asynchronous and iterative operation, for the sake of performance it is implemented as paginated operation._

  You can define the page size by using `WithReadingPageSize` method of store configuration, by default it is 10 events.

* _Reading and writing operations are not thread-safe_.  
 Thus, it is not recommended to use the same instances of `IStreamUnitOfWork` or `IAsyncEnumerable<StreamEvent>` in multiple threads.

[Id]: ../src/StreamStore.Contracts/Id.cs
[Revision]: ../src/StreamStore.Contracts/Revision.cs
