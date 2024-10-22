using System;
using System.Collections.Generic;
using System.Text;

namespace StreamStore.Exceptions
{
    public class ReachedEndOfStreamException: StreamStoreException
    {
       public Revision LastRevision { get; }

        public ReachedEndOfStreamException(Id streamId, Revision lastRevision): base(streamId, "There is no more events to read") 
        {
            this.LastRevision = lastRevision;
        }
    }
}
