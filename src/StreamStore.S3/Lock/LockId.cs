using System;


namespace StreamStore.S3.Lock
{
    class LockId : IEquatable<LockId>, IEquatable<Id>
    {
        public Id Id { get; set; }

        public LockId(Id id)
        {
            Id = id;
        }

        public LockId()
        {
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
        public bool Equals(Id other)
        {
            return Id.Equals(other);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
