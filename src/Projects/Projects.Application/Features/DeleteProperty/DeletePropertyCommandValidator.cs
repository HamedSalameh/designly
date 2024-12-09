using FluentValidation;

namespace Projects.Application.Features.DeleteProperty
{
    public class DeletePropertyCommandValidator : AbstractValidator<DeletePropertyCommand>
    {
        public DeletePropertyCommandValidator()
        {
            RuleFor(x => x.TenantId).NotEmpty().WithMessage("Tenant Id is required");
            RuleFor(x => x.PropertyId).NotEmpty().WithMessage("Property Id is required");
        }
    }
}
