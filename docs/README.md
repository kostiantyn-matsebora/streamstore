# <p align="center">StreamStore</p>

Lightweight library provides logical layer for storing and querying events as a stream.

Heavily inspired by Greg Young's Event Store and [`Streamstone`](https://github.com/yevhen/Streamstone) solutions.

## Overview

Library itself does not provide any storage implementation, but it is designed to be easily extended with custom storage backends.

As an example, in-memory storage implementation is provided in [`InMemoryEventStore`](../src/StreamDB/InMemoryEventStore.cs).

## Features

The general idea is to highlight the general characteristics and features of event sourcing storage:

- [x] Event ordering
- [x] Serialization/deserialization of events
- [x] Optimistic concurrency control
- [x] Event duplication detection based on event id
- [ ] Transactional outbox pattern implementation (?)
- [ ] Multitenancy support
- [ ] Automatic provisioning of storage schema

Also add implementations of particular storage backends, such as:

- [x] In-memory
- [ ] [`MSSQL`](https://www.microsoft.com/en-us/sql-server/sql-server-2022)
- [ ] [`Cassandra DB`](https://cassandra.apache.org/_/index.html)
