namespace StreamDB
{
    public interface IStreamEntity
    {
        string Id { get; }

        int Revision { get; }

        IEventEntity[] EventEntities { get; }

    }
}
