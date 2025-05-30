using System.Collections.Generic;
using FluentValidation;

namespace StreamStore.Validation
{
    public interface IDuplicateRevisionValidator: IValidator<IEnumerable<IStreamEventMetadata>>
    {
        void ThrowIfNotValid(StreamMutationRequest request);
    }
}
