using System.Collections.Generic;
using System;
using System.Linq;

namespace StreamStore
{
    public static class IEventUnitOfWorkExtension
    {
        public static IEventUnitOfWork AddRange(this IEventUnitOfWork unitOfWork, IEnumerable<EventRecord> records)
        {
            Array.ForEach(records.ToArray(), e => unitOfWork.Add(e.Id, e.Revision, e.Timestamp, e.Data!));
            return unitOfWork;
        }
    }
}
