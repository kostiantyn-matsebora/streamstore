using System;


namespace StreamDB
{
    public readonly struct Id : IEquatable<Id>
    {
        public readonly string Value;

        Id(string value)
        {
            Value = value;
        }

        public bool Equals(Id other)
        {
            return Value == other.Value;
        }

        public static bool operator ==(Id left, Id right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Id left, Id right)
        {
            return !left.Equals(right);
        }
        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) && obj is Id && Equals((Id)obj);
        }

        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }

        public static implicit operator string(Id id) => id.Value;
        public static implicit operator Id(string id) => new Id(id);
    }
}
