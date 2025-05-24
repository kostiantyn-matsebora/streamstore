using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using StreamStore.Exceptions;
using StreamStore.Validation;

namespace StreamStore.Storage.Validation
{
    public class DuplicateRevisionValidator : AbstractValidator<IEnumerable<IStreamEventMetadata>>, IDuplicateRevisionValidator
    {
        public DuplicateRevisionValidator()
        {
            RuleFor(x => x)

                .Custom((key, context) =>
                {
                    var duplicateRevisions = key
                          .GroupBy(e => e.Revision)
                          .Where(g => g.Count() > 1)
                          .Select(g => g.Key)
                         .ToArray();
                    if (duplicateRevisions.Any())
                    {
                        context.AddFailure(
                            new ValidationFailure("Events", $"Batch contains duplicate revision {duplicateRevisions[0]}")
                            {
                                CustomState = duplicateRevisions[0],
                                ErrorCode = ErrorCodes.DuplicateEvent
                            });
                    }
                });
        }

        public void ThrowIfNotValid(StreamMutationRequest request)
        {
            var result = Validate(request.Events);

            if (!result.IsValid)
                throw new DuplicateRevisionException(new Revision((int)result.Errors[0].CustomState), request.StreamId);
        }
    }
}
