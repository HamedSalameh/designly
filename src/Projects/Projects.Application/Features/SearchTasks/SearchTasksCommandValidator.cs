﻿using FluentValidation;
using Projects.Application.Filter;

namespace Projects.Application.Features.SearchTasks
{
    public class SearchTasksCommandValidator : AbstractValidator<SearchTasksCommand>
    {
        public SearchTasksCommandValidator()
        {
            RuleFor(x => x.projectId).NotEmpty().WithMessage("Project id is required");
            RuleFor(x => x.tenantId).NotEmpty().WithMessage("Tenant id is required");

            RuleFor(x => x.filters).Custom(FilterValidation);
        }

        private void FilterValidation(List<FilterCondition> list, ValidationContext<SearchTasksCommand> context)
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
                    if (string.IsNullOrEmpty(filter.Field))
                    {
                        context.AddFailure("Filter field is required");
                    }
                    if (filter.Values == null || filter.Values.Count() == 0)
                    {
                        context.AddFailure("Filter values are required");
                    }
                    if (filter.Field != null && filter.Field.Length > Domain.Constants.FieldMaxLength)
                    {
                        context.AddFailure($"Filter value must not exceed  {Domain.Constants.FieldMaxLength}  charactersrs");
                    }
                }
            }
        }
    }
}
