using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StreamStore
{
    public struct CompositeId : IEquatable<CompositeId>, IEquatable<string>
    {
        const string delimiter = "|";

        public string Value => value ?? string.Empty;

        readonly string value;
        public static CompositeId None => new CompositeId(string.Empty);

        public CompositeId(params string[] ids)
        {
            if (ids.Any())
            {
                value = string.Join(delimiter, ids);
            }
            else
            {
                value = string.Empty;
            }
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
            return !ReferenceEquals(null, obj) &&
                ((obj is Id && Equals((CompositeId)obj)) || (obj is string && Equals((string)obj)));
        }

        public bool Equals(CompositeId other)
        {
            throw new NotImplementedException();
        }


        public static bool operator ==(CompositeId left, Id right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(CompositeId left, Id right)
        {
            return !left.Equals(right);
        }
        public static bool operator ==(CompositeId left, string right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(CompositeId left, string right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode() : 0;
        }

        public override string ToString() => Value;


        public static implicit operator string(CompositeId id) => id.Value;
        public static implicit operator CompositeId(string id) => new CompositeId(id.Split(delimiter).ToArray());
    }
}
