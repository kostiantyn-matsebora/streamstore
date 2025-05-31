using System;
using System.Collections.Generic;
using System.Linq;
using StreamStore.Extensions;

namespace StreamStore.Validation
{
    public class StreamMutationRequest
    {
        public Id StreamId { get; }
        public IStreamEventMetadata[] Events { get; }

        public StreamMutationRequest(Id streamId, IEnumerable<IStreamEventMetadata> uncommited, IEnumerable<IStreamEventMetadata>? commited = null)
        {
            if (uncommited == null)
                throw new ArgumentNullException(nameof(uncommited), "Uncommitted events cannot be null.");

            StreamId = streamId.ThrowIfHasNoValue(nameof(streamId));

            Events = commited != null && commited.Any() ? uncommited.Concat(commited).ToArray() : uncommited.ToArray();
        }
    }
}
