using System.Collections.Generic;
using FluentValidation;

namespace StreamStore.Validation
{
    public interface IDuplicateEventValidator: IValidator<IEnumerable<IStreamEventMetadata>>
    {
        public void ThrowIfNotValid(StreamMutationRequest request);
    }
}
