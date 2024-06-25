using Projects.Application.LogicValidation.Requests;

namespace Projects.Application.LogicValidation.Handlers
{
    public sealed class CreateTasksValidationRequestHandler : IBusinessLogicValidationHandler<CreateTasksValidationRequest>
    {
        public Task<Exception?> ValidateAsync(CreateTasksValidationRequest request, CancellationToken cancellationToken)
        {
            // Always valid
            return Task.FromResult<Exception?>(null);
        }
    }
}
