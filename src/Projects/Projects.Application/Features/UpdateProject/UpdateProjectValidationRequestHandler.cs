using Designly.Base;
using Designly.Base.Exceptions;
using Projects.Application.LogicValidation;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.UpdateProject
{
    public class UpdateProjectValidationRequestHandler : IBusinessLogicValidationHandler<UpdateProjectValidationRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProjectValidationRequestHandler(IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);
            _unitOfWork = unitOfWork;
        }

        public async Task<Exception?> ValidateAsync(UpdateProjectValidationRequest request, CancellationToken cancellationToken)
        {
            // Check if the project exists
            var project = await _unitOfWork.ProjectsRepository.GetByIdAsync(request.ProjectId, request.TenantId, cancellationToken).ConfigureAwait(false);

            if (project == null)
            {
                return new BusinessLogicException(new Error("NotFound", "The project does not exist"));
            }
            return null;
        }
    }
}