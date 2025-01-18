using FluentValidation;

namespace Projects.Application.Features.GetProperty
{
    public class GetRealestatePropertyCommandValidator : AbstractValidator<GetRealestatePropertyCommand>
    {
        public GetRealestatePropertyCommandValidator()
        {
            RuleFor(x => x.TenantId).NotEmpty().WithMessage("Tenant id is required");
            RuleFor(x => x.FilterConditions).NotEmpty();
        }
    }
}
