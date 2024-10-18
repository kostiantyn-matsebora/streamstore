﻿namespace StreamStore.Testing
{
    public static class IStreamUnitOfWorkExtension
    {
        public static async Task<IStreamUnitOfWork> AddRangeAsync(this IStreamUnitOfWork unitOfWork, IEnumerable<EventItem> records, CancellationToken token = default)
        {
            foreach (var record in records)
            {
                await unitOfWork.AddAsync(record.Id, record.Timestamp, record.Data!, token);
            }
            return unitOfWork;
        }

        public static async Task<IStreamUnitOfWork> AddRangeAsync(this Task<IStreamUnitOfWork> unitOfWork, IEnumerable<EventItem> records, CancellationToken token = default)
        {
            await unitOfWork.Result.AddRangeAsync(records);
            return unitOfWork.Result;
        }

        public static async Task<IStreamUnitOfWork> SaveChangesAsync(this Task<IStreamUnitOfWork> unitOfWork, CancellationToken token = default)
        {
            await unitOfWork.Result.SaveChangesAsync(token);
            return unitOfWork.Result;
        }
    }
}
