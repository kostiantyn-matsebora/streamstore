namespace StreamStore.Testing
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
            await unitOfWork.ContinueWith(async t =>
            {
                await t.Result.AddRangeAsync(records);
            }, token);
            return unitOfWork.Result;
        }

        public static async Task<IStreamUnitOfWork> SaveChangesAsync(this Task<IStreamUnitOfWork> unitOfWork, CancellationToken token = default)
        {
            await unitOfWork.ContinueWith(async t =>
            {
                await t.Result.SaveChangesAsync(token);
            }, token);
            return unitOfWork.Result;
        }
    }
}
