using Projects.Application.LogicValidation;

namespace Projects.Application.Features.DeleteTask
{
    public sealed class DeleteTaskValdationRequestHandler : IBusinessLogicValidationHandler<DeleteTasksValidationRequest>
    {
        public Task<Exception?> ValidateAsync(DeleteTasksValidationRequest request, CancellationToken cancellationToken)
        {
            // Always valid
            return Task.FromResult<Exception?>(null);
        }
    }
}
