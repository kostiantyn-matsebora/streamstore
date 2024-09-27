using System;
using System.Threading;
using System.Threading.Tasks;

namespace StreamDB.Operations
{
    internal class DeleteOperation
    {
        IEventTable store;
        Id streamId;

        public DeleteOperation(IEventTable store)
        {
            if (store == null)
                throw new ArgumentNullException(nameof(store));
            this.store = store;
        }
        public DeleteOperation AddStreamId(Id streamId)
        {
            this.streamId = streamId;
            return this;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (streamId == Id.None)
                throw new ArgumentException("streamId is required.", nameof(streamId));

            await store.DeleteAsync(streamId, cancellationToken);
        }
    }
}
