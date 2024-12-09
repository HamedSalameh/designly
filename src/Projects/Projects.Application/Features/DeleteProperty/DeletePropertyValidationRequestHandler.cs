using Designly.Base;
using Designly.Base.Exceptions;
using Projects.Application.LogicValidation;
using Projects.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Exception?> ValidateAsync(DeletePropertyValidationRequest request, CancellationToken cancellationToken)
        {
            // when attempting to delete a property, we need to check if the property is already assigned to a project
            // if it is, we should not allow the deletion of the property
            // if it is not, we should allow the deletion of the property

            var isPropertyAttachedToProject = await _unitOfWork.PropertiesRepository.IsPropertyAttachedToProject(request.TenantId, request.PropertyId, 
                cancellationToken);

            if (!isPropertyAttachedToProject)
            {
                return new BusinessLogicException(new Error("AttachedProperty", "The property is already attached to a project"));
            }

            return null;
        }
    }
}
