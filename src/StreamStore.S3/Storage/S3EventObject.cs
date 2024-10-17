using StreamStore.S3.Client;


namespace StreamStore.S3.Storage
{
    class S3EventObject : S3Object
    {
        EventRecord? record;
        byte[]? data;

        public S3EventObject(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory) { }

        public override byte[] Data
        {
            get
            {
                return data ?? new byte[0];
            }
            set
            {
                data = value;
                record = Converter.FromByteArray<EventRecord>(data)!;
            }
        }

        public EventRecord? Event
        {
            get
            {
                return record;
            }
            set
            {
                record = value;
                data = Converter.ToByteArray(record!);
            }
        }
    }
}
