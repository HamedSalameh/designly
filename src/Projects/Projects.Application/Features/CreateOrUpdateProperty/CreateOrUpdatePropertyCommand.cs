using Designly.Shared.ValueObjects;
using LanguageExt.Common;
using MediatR;
using Projects.Domain;

namespace Projects.Application.Features.CreateOrUpdateProperty
{
    public class CreateOrUpdatePropertyCommand : IRequest<Result<Property>>
    {
        public Guid? Id { get; set; } = null;   // We allow the id to nullable, to support create or update property
        public Guid TenantId { get; set; }
        public string? Name { get; set; }
        public int PropertyType { get; set; }
        public Address Address { get; set; }
        public List<Floor>? Floors { get; set; }
        public double TotalArea { get; set; }
    }
}
