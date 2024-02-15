using Projects.Domain.StonglyTyped;

namespace Projects.Domain
{
    public abstract class Entity
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
            if (TenantId == TenantId.Empty || TenantId == default)
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

        public override bool Equals(object? obj)
        {
            if (obj is not Entity)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return item.Id == Id;
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
