using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using StreamStore.Exceptions;
using StreamStore.Validation;

namespace StreamStore.Storage.Validation
{
    public class DuplicateEventValidator : AbstractValidator<IEnumerable<IStreamEventMetadata>>, IDuplicateEventValidator
    {
        public DuplicateEventValidator()
        {
            RuleFor(x => x)

                .Custom((key, context) =>
                {
                    var duplicatedId = key
                       .GroupBy(e => e.Id)
                       .Where(g => g.Count() > 1)
                       .Select(g => g.Key)
                       .FirstOrDefault();

                    if (duplicatedId != Id.None)
                    {
                        context.AddFailure(
                            new ValidationFailure("Events", $"Batch contains duplicate event with id={duplicatedId}")
                            {
                                CustomState = duplicatedId,
                                ErrorCode = ErrorCodes.DuplicateEvent
                            });
                    }
                });
        }

        public void ThrowIfNotValid(StreamMutationRequest request)
        {
            var result = Validate(request.Events);

            if (!result.IsValid)
                throw new DuplicatedEventException((Id)result.Errors[0].CustomState, request.StreamId);
        }
    }
}
