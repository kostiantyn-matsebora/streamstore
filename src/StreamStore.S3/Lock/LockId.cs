using System;


namespace StreamStore.S3.Lock
{
    class LockId : IEquatable<LockId>
    {
        public Guid Id { get; set; }

        public LockId(Guid id)
        {
            Id = id;
        }

        public LockId()
        {
            Id = Guid.NewGuid();
        }

        public bool Equals(LockId other)
        {
            if (other == null) return false;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != GetType()) return false;
            return Equals((LockId)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
