using Projects.Domain.StonglyTyped;

namespace Projects.Infrastructure.Interfaces
{
    public interface IPropertiesRepository
    {
        public Task<bool> PropertyExistsAsync(Guid propertyId, TenantId tenantId, CancellationToken cancellationToken = default);
    }
}
