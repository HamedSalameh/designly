using Projects.Application.LogicValidation;

namespace Projects.Application.Features.DeleteProject
{
    public class DeleteProjectValidationRequestHandler : IBusinessLogicValidationHandler<DeleteProjectValidationRequest>
    {
        public Task<Exception?> ValidateAsync(DeleteProjectValidationRequest request, CancellationToken cancellationToken)
        {
            // Project deletion validation based on business logic

            // MVP: Project deletion is allowed in case the user has the needed permissions.
            // In other cases, a project should be allowed for deletion, regardless of the project status

            // Hence, always valid for MVP
            return Task.FromResult<Exception?>(null);
        }
    }
}
    