using Designly.Base.Exceptions;

namespace Projects.Application.LogicValidation.Handlers
{
    public interface IBusinessLogicValidationHandler<in TBusinessLogicValidationRequest> 
        where TBusinessLogicValidationRequest : IBusinessLogicValidationRequest
    {
        Task<Exception?>? ValidateAsync(TBusinessLogicValidationRequest request, CancellationToken cancellationToken);
    }
}
