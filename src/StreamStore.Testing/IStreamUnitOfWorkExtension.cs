namespace StreamStore.Testing
{
    public static class IStreamUnitOfWorkExtension
    {
        public static IStreamUnitOfWork AddRange(this IStreamUnitOfWork unitOfWork, IEnumerable<EventItem> records)
        {
            foreach (var record in records)
            {
                unitOfWork.Add(record.Id, record.Timestamp, record.Data!);
            }
            return unitOfWork;
        }
    }
}
