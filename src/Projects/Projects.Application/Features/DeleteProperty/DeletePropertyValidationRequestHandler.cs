using Projects.Application.LogicValidation;
using Projects.Infrastructure.Interfaces;

namespace Projects.Application.Features.DeleteProperty
{
    public sealed class DeletePropertyValidationRequestHandler : IBusinessLogicValidationHandler<DeletePropertyValidationRequest>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePropertyValidationRequestHandler(IUnitOfWork unitOfWork)
        {
            ArgumentNullException.ThrowIfNull(unitOfWork);

            _unitOfWork = unitOfWork;
        }

        public Task<Exception?> ValidateAsync(DeletePropertyValidationRequest request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request);

            return Task.FromResult<Exception?>(null);
        }
    }
}
