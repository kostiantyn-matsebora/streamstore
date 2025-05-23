using StreamStore.Exceptions;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Generic;


namespace StreamStore.Storage
{
    //public abstract class StreamWriterBase : IStreamWriter
    //{
    //    protected readonly Id streamId;
    //    protected readonly Revision expectedRevision;
    //    readonly StreamEventRecordCollection events = new StreamEventRecordCollection();
    //    readonly StreamEventRecordCollection uncommited = new StreamEventRecordCollection();

    //    Revision revision;

    //    protected StreamWriterBase(Id streamId, Revision expectedRevision, StreamEventRecordCollection? existing)
    //    {
    //        this.streamId = streamId.ThrowIfHasNoValue(nameof(streamId));

    //        if (existing != null && existing.Any())
    //        {
    //            events.AddRange(existing);
    //        }

    //        this.expectedRevision = expectedRevision;
    //        revision = expectedRevision;
            
    //    }

 
    //    public abstract Task WriteAsync(Id streamId, IEnumerable<IStreamEventRecord> records, CancellationToken token);
    //}
}
