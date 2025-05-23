
using StreamStore.Validation;

namespace StreamStore.Store
{
    internal class StreamUnitOfWorkFactory : IStreamUnitOfWorkFactory
    {
        readonly IStreamWriter writer;
        readonly IEventConverter converter;
        readonly IStreamMutationRequestValidator validator;

        public StreamUnitOfWorkFactory(IStreamWriter writer, IEventConverter converter, IStreamMutationRequestValidator validator)
        {
            this.writer = writer.ThrowIfNull(nameof(writer));
            this.converter = converter.ThrowIfNull(nameof(converter));
            this.validator = validator.ThrowIfNull(nameof(validator));
        }

        public IStreamUnitOfWork Create(Id streamId, Revision expectedRevision)
        {
           return new StreamUnitOfWork(streamId, expectedRevision, writer, converter, validator);
        }
    }
}
