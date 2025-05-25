using FluentValidation;
using StreamStore.Validation;

namespace StreamStore.Storage.Validation
{
    public class StreamMutationRequestValidator : AbstractValidator<StreamMutationRequest>, IStreamMutationRequestValidator
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
