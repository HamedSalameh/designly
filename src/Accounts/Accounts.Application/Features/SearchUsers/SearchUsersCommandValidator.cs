using Designly.Filter;
using FluentValidation;

namespace Accounts.Application.Features.SearchUsers
{
    public class SearchUsersCommandValidator : AbstractValidator<SearchUsersCommand>
    {
        public SearchUsersCommandValidator()
        {
            RuleFor(x => x.TenantId).NotEqual(Guid.Empty).WithMessage("Tenant id is required");
            RuleFor(x => x.TenantId).NotNull().WithMessage("Tenant id is required");

            RuleFor(x => x.FilterConditions).Custom(FilterValidation);
        }

        private void FilterValidation(List<FilterCondition> list, ValidationContext<SearchUsersCommand> context)
        {
            if (list == null)
            {
                context.AddFailure("Filter conditions are required");
                return;
            }

            // validate each filter condition
            foreach (var filter in list)
            {
                if (filter == null)
                {
                    context.AddFailure("Filter condition is required");
                    return;
                }

                if (string.IsNullOrWhiteSpace(filter.Field))
                {
                    context.AddFailure("Filter field is required");
                }

            }
        }
    }
}
