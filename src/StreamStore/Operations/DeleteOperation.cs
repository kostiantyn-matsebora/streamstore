using System;
using System.Threading;
using System.Threading.Tasks;


namespace StreamStore.Operations
{
    internal class DeleteOperation
    {
        IEventDatabase database;
        Id streamId;

        public DeleteOperation(IEventDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException(nameof(database));
            this.database = database;
        }
        public DeleteOperation SetStreamId(Id streamId)
        {
            this.streamId = streamId;
            return this;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (streamId == Id.None)
                throw new ArgumentException("streamId is required.", nameof(streamId));

            await database.DeleteAsync(streamId, cancellationToken);
        }
    }
}
