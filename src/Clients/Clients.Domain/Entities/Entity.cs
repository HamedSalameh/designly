#pragma warning disable IDE0070 // Use 'System.HashCode'

namespace Clients.Domain.Entities
{
    /// <summary>
    /// Implementation of the Entity base class
    /// Must implement the Equals method to compare entities, hence the IEquatable interface
    /// </summary>
    public abstract class Entity : IEquatable<Entity>
    {
        public DateTime CreatedAt { get; set;  }
        public DateTime ModifiedAt { get; set; }

        public virtual Guid Id { get; set; }
        public virtual Guid TenantId { get; set; }

        protected Entity()
        {
            
        }

        protected Entity(Guid TenantId) : this()
        {
            // TenantId is not nullable, so we can't use the ?? operator
            if (TenantId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(TenantId), "TenantId cannot be null");
            }
            
            this.TenantId = TenantId;
            CreatedAt = DateTime.UtcNow;
            ModifiedAt = DateTime.UtcNow;
        }

        public bool IsTransient()
        {
            return Id == Guid.Empty;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + CreatedAt.GetHashCode();
                return hash;
            }
        }

        public abstract override bool Equals(object? obj);
        public abstract bool Equals(Entity? other);
        public abstract override string ToString();
    }
}
