using Designly.Base;
using Designly.Base.Exceptions;
using Projects.Application.LogicValidation;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.UpdateTask
{
    public class UpdateTaskValidationRequestHandler : IBusinessLogicValidationHandler<UpdateTaskValidationRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTaskValidationRequestHandler(IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork, nameof(unitOfWork));

            _unitOfWork = unitOfWork;
        }

        public async Task<Exception?> ValidateAsync(UpdateTaskValidationRequest request, CancellationToken cancellationToken)
        {
            // Check if the task exists
            var task = await _unitOfWork.TaskItemsRepository.GetByIdAsync(request.TenantId, request.ProjectId, request.TaskItemId, cancellationToken).ConfigureAwait(false);

            if ( task == null )
            {
                return new BusinessLogicException(new Error("NotFound", "The task does not exist"));
            }

            return null;
        }
    }
}