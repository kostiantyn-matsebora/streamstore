# <p align="center">StreamDB</p>

Lightweight library provides logical layer for storing and querying events in a stream.

Heavily inspired by Greg Young's Event Store and [Streamstone](https://github.com/yevhen/Streamstone) solutions.

## Overview

Library itself does not provide any storage implementation, but it is designed to be easily extended with custom storage backends.

As an examples:

- In-memory storage implementation is provided in [`InMemoryEventStore`](../src/StreamDB/InMemoryEventStore.cs).
- MSSQL storage implementation provided in [`SqlEventStore`](../src/StreamDB.Sql/SqlEventStore.cs).

The general idea is to highlight the general characteristics and features of event sourcing storage, such as:

- Event ordering
- Serialization/deserialization of events
- Optimistic concurrency control
- Event duplication detection based on event id

## Roadmap

- Multitenancy support
- Cassandra storage implementation
- Automatic provisioning of storage schema 
