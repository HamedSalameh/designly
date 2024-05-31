using Designly.Base.Exceptions;

namespace Projects.Application.LogicValidation
{
    public interface IBusinessLogicValidator
    {
        Task<Exception?> ValidateAsync(IBusinessLogicValidationRequest request, CancellationToken cancellationToken);
    }
}