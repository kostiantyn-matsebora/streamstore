using System;
using System.Collections.Generic;
using System.Text;

namespace StreamStore.S3.Operations
{
    internal abstract class OperationBase
    {
        protected string CalculateDestinationKey(string sourceKey, string sourcePrefix, string destinationPrefix)
        {
            if (sourceKey == null)
                throw new ArgumentNullException(nameof(sourceKey));

            if (sourcePrefix == null)
                throw new ArgumentNullException(nameof(sourcePrefix));

            if (destinationPrefix == null)
                throw new ArgumentNullException(nameof(destinationPrefix));

            if (!sourceKey.StartsWith(sourcePrefix, StringComparison.InvariantCultureIgnoreCase))

                throw new ArgumentException("Source key does not start with source prefix.", nameof(sourceKey));

            return sourceKey.Replace(sourcePrefix, destinationPrefix, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
