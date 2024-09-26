using System;


namespace StreamDB
{
    public interface IStreamItem: IEventMetadata
    {
        public object Event { get; }
    }
}
