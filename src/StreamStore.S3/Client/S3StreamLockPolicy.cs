using System;

namespace StreamStore.S3.Client
{
    public abstract class S3StreamLockPolicy
    {
        internal static readonly S3RetainLockPolicy Default = new S3RetainLockPolicy(TimeSpan.FromSeconds(30));
    }

    internal sealed class S3HoldLockPolicy : S3StreamLockPolicy
    {
        internal S3HoldLockPolicy()
        {
        }
    }

    internal sealed class S3RetainLockPolicy : S3StreamLockPolicy
    {
        public TimeSpan Period { get; }

        internal S3RetainLockPolicy(TimeSpan period)
        {
            Period = period;
        }
    }
}