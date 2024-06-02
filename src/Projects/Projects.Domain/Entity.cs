#pragma warning disable IDE0070 // Use 'System.HashCode'

using Projects.Domain.StonglyTyped;

namespace Projects.Domain
{
    /// <summary>
    /// Implementation of the Entity base class
    /// Must implement the Equals method to compare entities, hence the IEquatable interface
    /// </summary>
    public abstract class Entity : IEquatable<Entity>
    {
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public virtual Guid Id { get; set; }
        public virtual TenantId TenantId { get; set; }

        protected Entity()
        {

        }

        protected Entity(TenantId TenantId) : this()
        {
            // TenantId is not nullable, so we can't use the ?? operator
            if (TenantId == TenantId.Empty)
            {
                throw new ArgumentException("TenantId cannot be null or default", nameof(TenantId));
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

        public bool Equals(Entity? other)
        {
            if (other is not Entity)
                return false;

            if (Object.ReferenceEquals(this, other))
                return true;

            if (this.GetType() != other.GetType())
                return false;

            Entity item = other;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return item.Id == Id;
        }

        public override bool Equals(object? obj) => Equals(obj as Entity);

        public static bool operator ==(Entity? left, Entity? right)
        {
            if (Equals(left, null))
                return Equals(right, null);
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}