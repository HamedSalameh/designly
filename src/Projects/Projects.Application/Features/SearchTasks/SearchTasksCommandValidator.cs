using Designly.Filter;
using FluentValidation;

namespace Projects.Application.Features.SearchTasks
{
    public class SearchTasksCommandValidator : AbstractValidator<SearchTasksCommand>
    {
        public SearchTasksCommandValidator()
        {
            RuleFor(x => x.ProjectId).NotEmpty().WithMessage("Project id is required");
            RuleFor(x => x.TenantId).NotEmpty().WithMessage("Tenant id is required");

            RuleFor(x => x.Filters).Custom(FilterValidation);
        }

        private void FilterValidation(List<FilterCondition> list, ValidationContext<SearchTasksCommand> context)
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
