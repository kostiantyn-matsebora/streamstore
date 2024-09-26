namespace StreamDB
{
    public interface IUncommitedEvent: IUncommitedEventMetadata
    {
        public object Event { get; }
    }
}
