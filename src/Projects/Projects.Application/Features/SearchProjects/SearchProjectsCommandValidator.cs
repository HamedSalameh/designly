using Designly.Filter;
using FluentValidation;

namespace Projects.Application.Features.SearchProjects
{
    public class SearchProjectsCommandValidator : AbstractValidator<SearchProjectsCommand>
    {
        public SearchProjectsCommandValidator()
        {
            RuleFor(x => x.TenantId).NotEmpty().WithMessage("Tenant id is required");

            RuleFor(x => x.FilterConditions).Custom(FilterValidation);
        }

        private void FilterValidation(List<FilterCondition> list, ValidationContext<SearchProjectsCommand> context)
        {
            // validate each filter condition
            foreach (var filter in list)
            {
                if (filter == null)
                {
                    context.AddFailure("Filter condition is required");
                }
                else
                {
                    if (filter.Values == null || !filter.Values.Any())
                    {
                        context.AddFailure("Filter values are required");
                    }
                }
            }
        }
    }
}
