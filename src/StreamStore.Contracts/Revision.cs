using System;


namespace StreamStore
{
    public readonly struct Revision : IEquatable<Revision>, IEquatable<int>, IComparable<Revision>, IComparable<int>
    {
        readonly int value;

        public int Value => value;
        public string StringValue => value.ToString();

        public static readonly Revision Zero = new Revision(0.ToString());
        public static readonly Revision One = new Revision(1.ToString());

  
        public Revision(string? stringValue)
        {
            if (string.IsNullOrEmpty(stringValue))
            {
                throw new ArgumentNullException(nameof(stringValue), "Revision must be greater than 0.");
            }
            if (!int.TryParse(stringValue, out var revision))
            {
                throw new ArgumentException("Revision must be a valid integer.");
            }
            value = revision;
        }

        public Revision Increment() => New(Value + 1);

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
        public static bool operator !=(Revision left, int right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return StringValue.GetHashCode();
        }

        public override string ToString() => StringValue.ToString();

        public int CompareTo(Revision other)
        {
            return CompareTo(other.Value);
        }

        public int CompareTo(int other)
        {
            return Value.CompareTo(other);
        }

        public static implicit operator int(Revision revision) => revision.Value;
        public static implicit operator Revision(int revision)
        {
          return New(revision);
        }

        public static Revision New(int revision)
        {
            if (revision == 0) return Zero;
            if (revision == 1) return One;
            return new Revision(revision.ToString());
        }
    }
}
