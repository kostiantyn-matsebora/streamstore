using System.Collections.Generic;
using System;
using System.Linq;

namespace StreamStore
{
    public static class IStreamUnitOfWorkExtension
    {
        public static IStreamUnitOfWork AddRange(this IStreamUnitOfWork unitOfWork, IEnumerable<EventRecord> records)
        {
            foreach (var record in records)
            {
                unitOfWork.Add(record.Id, record.Revision, record.Timestamp, record.Data!);
            }
            return unitOfWork;
        }
    }
}
