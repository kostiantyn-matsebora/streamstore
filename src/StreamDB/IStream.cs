namespace StreamDB
{
    public interface IStream
    {
        string Id { get; }

        int Revision { get; }

        IStreamItem[] Events { get; }

    }
}
