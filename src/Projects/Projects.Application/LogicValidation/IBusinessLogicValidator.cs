using Designly.Base.Exceptions;

namespace Projects.Application.LogicValidation
{
    public interface IBusinessLogicValidator
    {
        Task<BusinessLogicException?> ValidateAsync(IBusinessLogicValidationRequest request, CancellationToken cancellationToken);
    }
}