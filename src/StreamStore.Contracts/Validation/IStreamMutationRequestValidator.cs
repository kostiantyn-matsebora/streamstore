using FluentValidation;

namespace StreamStore.Validation
{
    public interface IStreamMutationRequestValidator: IValidator<StreamMutationRequest>
    {
        void ThrowIfNotValid(StreamMutationRequest request);
    }
}
