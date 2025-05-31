using FluentValidation;
using StreamStore.Extensions;

namespace StreamStore.Validation
{
    internal class StreamMutationRequestValidator : AbstractValidator<StreamMutationRequest>, IStreamMutationRequestValidator
    {
        readonly IDuplicateEventValidator duplicateEventValidator;
        readonly IDuplicateRevisionValidator duplicateRevisionValidator;

        public StreamMutationRequestValidator(IDuplicateEventValidator duplicateEventValidator, IDuplicateRevisionValidator duplicateRevisionValidator)
        {
            this.duplicateEventValidator = duplicateEventValidator.ThrowIfNull(nameof(duplicateEventValidator));
            this.duplicateRevisionValidator = duplicateRevisionValidator.ThrowIfNull(nameof(duplicateRevisionValidator));

            RuleFor(x => x.Events)
                .SetValidator(duplicateEventValidator)
                .SetValidator(duplicateRevisionValidator);
        }

        public void ThrowIfNotValid(StreamMutationRequest request)
        {
           duplicateEventValidator.ThrowIfNotValid(request);
           duplicateRevisionValidator.ThrowIfNotValid(request);
        }
    }
}
