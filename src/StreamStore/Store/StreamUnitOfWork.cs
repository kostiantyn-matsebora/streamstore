using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using StreamStore.Exceptions;
using StreamStore.Storage;
using StreamStore.Validation;


namespace StreamStore
{
    class StreamUnitOfWork : IStreamUnitOfWork
    {
        readonly IStreamWriter writer;
        readonly IEventConverter converter;
        readonly IStreamMutationRequestValidator validator;
        readonly List<IStreamEventRecord> uncommited = new List<IStreamEventRecord>();
        readonly Id streamId;

        Revision revision;

        public StreamUnitOfWork(Id streamId, Revision expectedRevision, IStreamWriter writer, IEventConverter converter, IStreamMutationRequestValidator validator)
        {
          this.streamId = streamId.ThrowIfHasNoValue(nameof(streamId));
          this.revision = expectedRevision;
          this.writer = writer ?? throw new ArgumentNullException(nameof(writer));
          this.converter = converter ?? throw new ArgumentNullException(nameof(converter));
          this.validator = validator.ThrowIfNull(nameof(validator));
        }

        public Task<IStreamUnitOfWork> AppendAsync(IEventEnvelope envelope, CancellationToken cancellationToken = default)
        {
            envelope.ThrowIfNull(nameof(envelope));
            envelope.Id.ThrowIfHasNoValue(nameof(envelope.Id));
            envelope.Timestamp.ThrowIfMinValue(nameof(envelope.Timestamp));
            envelope.Event.ThrowIfNull(nameof(envelope.Event));

            // Since revision is immutable, we need to assign the new value to revision
            revision = revision.Next();

            var eventRecord = new StreamEventRecord
            {
                Id = envelope.Id,
                Revision = revision,
                Timestamp = envelope.Timestamp,
                Data = converter.ConvertToByteArray(envelope.Event)
            };

            uncommited.Add(eventRecord);

            return Task.FromResult<IStreamUnitOfWork>(this);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
           validator.ThrowIfNotValid(new StreamMutationRequest(streamId, uncommited.ToArray()));
           await writer.WriteAsync(streamId, uncommited, cancellationToken);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
