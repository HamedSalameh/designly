using Clients.Application.Queries;
using FluentValidation;

namespace Clients.Application.Validators
{
    public class  SearchClientsQueryValidator : AbstractValidator<SearchClientsQuery>
    {
        public SearchClientsQueryValidator()
        {
            RuleFor(query => query.TenantId)
                .NotEmpty()
                .WithMessage($"Invalid value for {nameof(SearchClientsQuery.TenantId)}");
        }
    }
}