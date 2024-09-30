using System;


namespace StreamStore
{
    public readonly struct Id : IEquatable<Id>, IEquatable<string>
    {

        public string Value => value ?? string.Empty;

        readonly string value;

        public static Id None => new Id(string.Empty);

        public Id(string? value = null)
        {
            this.value = value ?? string.Empty;
        }

        public bool Equals(Id other)
        {
            return Value == other.Value;
        }

        public bool Equals(string other)
        {
            return Value == other;
        }
        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) && obj is Id && Equals((Id)obj);
        }

        public static bool operator ==(Id left, Id right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Id left, Id right)
        {
            return !left.Equals(right);
        }
        public static bool operator ==(Id left, string right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Id left, string right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }

        public override string ToString() => Value;


        public static implicit operator string(Id id) => id.Value;
        public static implicit operator Id(string id) => new Id(id);
    }
}
