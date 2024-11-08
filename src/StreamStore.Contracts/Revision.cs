using System;


namespace StreamStore
{
    public readonly struct Revision : IEquatable<Revision>, IEquatable<int>, IComparable<Revision>, IComparable<int>
    {
        readonly int value;

        public int Value => value;
      
        public static readonly Revision Zero = new Revision(0);
        public static readonly Revision One = new Revision(1);

        public Revision(int revision)
        {
            if (revision < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(revision), "Revision must be greater or equal 0.");
            }
            this.value = revision;
        }

        public Revision Increment() => new Revision(Value + 1);

        public Revision Decrement() => new Revision(Value - 1);

        public bool Equals(Revision other)
        {
            return Value == other.Value;
        }

        public bool Equals(int other)
        {
            return Value == other;
        }
        public override bool Equals(object obj)
        {
            return !ReferenceEquals(null, obj) 
                && ((obj is int && Equals((int)obj)) || (obj is Revision && Equals((Revision)obj)));
        }

        public static bool operator ==(Revision left, Revision right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Revision left, Revision right)
        {
            return !left.Equals(right);
        }
        public static bool operator ==(Revision left, int right)
        {
            return left.Equals(right);
        }

        public static bool operator <(Revision left, Revision right)
        {
            return left.Value < right.Value;
        }
        public static bool operator >(Revision left, Revision right)
        {
            return left.Value > right.Value;
        }

        public static bool operator <=(Revision left, Revision right)
        {
            return left.Value <= right.Value;
        }

        public static bool operator >=(Revision left, Revision right)
        {
            return left.Value >= right.Value;
        }

        public static bool operator <(Revision left, int right)
        {
            return left.Value < right;
        }
        public static bool operator >(Revision left, int right)
        {
            return left.Value > right;
        }

        public static bool operator <=(Revision left, int right)
        {
            return left.Value <= right;
        }

        public static bool operator >=(Revision left, int right)
        {
            return left.Value >= right;
        }

        public static bool operator !=(Revision left, int right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString() => Value.ToString();

        public int CompareTo(Revision other)
        {
            return CompareTo(other.Value);
        }

        public int CompareTo(int other)
        {
            return Value.CompareTo(other);
        }

        public static implicit operator int(Revision revision) => revision.Value;
        public static implicit operator Revision(int revision) => new Revision(revision);
    }
}
