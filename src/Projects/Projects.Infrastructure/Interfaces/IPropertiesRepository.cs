using Projects.Domain;
using Projects.Domain.StonglyTyped;

namespace Projects.Infrastructure.Interfaces
{
    public interface IPropertiesRepository
    {
        public Task<Guid> CreatePropertyAsync(Property property, CancellationToken cancellationToken = default);
        public Task<bool> PropertyExistsAsync(Guid propertyId, TenantId tenantId, CancellationToken cancellationToken = default);
        public Task DeleteAsync(Guid propertyId, TenantId tenantId, CancellationToken cancellationToken = default);
    }
}
