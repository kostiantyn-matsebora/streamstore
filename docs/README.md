# StreamDB

Lightweight library provides logical layer for storing and querying events in a stream.

Heavily inspired by Greg Young's Event Store and [Streamstone](https://github.com/yevhen/Streamstone) solutions.

## Overview

Library itself does not provide any storage implementation, but it is designed to be easily extended with custom storage backends.

As an example there is a simple [`SqlEventStore`](../src/StreamDB.Sql/SqlEventStore.cs) MSSQL storage implementation provided.

The general idea is to highlight the general characteristics and features of event sourcing storage, such as:

- Append-only storage
- Event ordering
- Serialization/deserialization of events
- Optimistic concurrency control
- Event duplication detection
