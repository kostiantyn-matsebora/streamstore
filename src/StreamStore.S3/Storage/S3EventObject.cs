﻿using System.Threading;
using System.Threading.Tasks;
using StreamStore.S3.Client;
using StreamStore.S3.Lock;
using StreamStore.Serialization;


namespace StreamStore.S3.Storage
{
    class S3EventObject : S3Object
    {
        EventRecord? record;
        public EventRecord? Event => record;

        public S3EventObject(S3ContainerPath path, IS3ClientFactory clientFactory) : base(path, clientFactory)
        {
        }

        public override async Task LoadAsync(CancellationToken token)
        {
            await base.LoadAsync(token);
            if (State == S3ObjectState.Loaded) record = Converter.FromByteArray<EventRecord>(Data)!;
        }

        public override async Task DeleteAsync(CancellationToken token)
        {
            await base.DeleteAsync(token);
            record = null;
        }

        public S3EventObject ReplaceBy(EventRecord record)
        {
            this.record = record;
            Data = Converter.ToByteArray(record!);
            return this;
        }

        public override void ResetState()
        {
            base.ResetState();
            record = null;
        }
    }
}
