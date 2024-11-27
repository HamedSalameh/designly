using Designly.Shared.ValueObjects;
using LanguageExt.Common;
using MediatR;
using Projects.Domain;

namespace Projects.Application.Features.CreateOrUpdateProperty
{
    public class CreateOrUpdatePropertyCommand : IRequest<Result<Property>>
    {
        public Guid TenantId { get; set; }
        public string? Name { get; set; }
        public int PropertyType { get; set; }
        public Address Address { get; set; }
        public List<Floor>? Floors { get; set; }
    }
}
