namespace StreamStore.ExampleBase.Workers
{
    internal abstract class WorkerIdentifier
    {
        public string Role { get; }
        public int Number { get; } = 0;
        public string Tenant { get; }

        protected WorkerIdentifier(string role, int number, string tenant)
        {
            Role = role;
            Number = number;
            Tenant = tenant;
        }



        public override string ToString()
        {
            return $"{Tenant} {Role}-{Number}";
        }
    }

    internal class ReaderIdentifier : WorkerIdentifier
    {
        public ReaderIdentifier(int number, string tenant = Tenants.Default) : base(Roles.Reader, number, tenant)
        {
        }
    }

    internal class ReaderToEndIdentifier : WorkerIdentifier
    {
        public ReaderToEndIdentifier(int number, string tenant = Tenants.Default) : base(Roles.ReaderToEnd, number, tenant)
        {
        }
    }

    internal class WriterIdentifier : WorkerIdentifier
    {
        public WriterIdentifier(int number, string tenant = Tenants.Default) : base(Roles.Writer, number, tenant)
        {
        }
    }
}
