using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace StreamStore.S3.Storage
{
    internal struct S3ContainerPath : IEquatable<string>, IEquatable<S3ContainerPath>
    {

        private const string Delimiter = "/";

        readonly string value;

        public string Value => value;

        public string Name => value.Substring(value.LastIndexOf(Delimiter) + 1);

        public static readonly S3ContainerPath Root = new S3ContainerPath(string.Empty);

        public S3ContainerPath(string? value = null)
        {
            this.value = value ?? string.Empty;
        }

        public S3ContainerPath Normalize()
        {
            return Value.Last() == Delimiter[0] ? this : (S3ContainerPath)$"{Value}{Delimiter}";
        }

        public S3ContainerPath Combine(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return this;
            }
            return $"{Value}{Delimiter}{name}";
        }

        public bool Equals(string other)
        {
            return Value == other;
        }

        public bool Equals(S3ContainerPath other)
        {
            return Value == other.Value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static implicit operator string(S3ContainerPath path) => path.Value;
        public static implicit operator S3ContainerPath(string path) => new S3ContainerPath(path);
        public static implicit operator S3ContainerPath(Id id) => new S3ContainerPath(id);
        public static implicit operator Id(S3ContainerPath path) => new Id(path);
    }
}
