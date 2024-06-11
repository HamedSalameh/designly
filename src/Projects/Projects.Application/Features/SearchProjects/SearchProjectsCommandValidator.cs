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
