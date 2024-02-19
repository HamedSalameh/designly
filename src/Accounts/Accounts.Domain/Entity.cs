#pragma warning disable IDE0070 // Use 'System.HashCode'

namespace Accounts.Domain
{
    public abstract class Entity : IEquatable<Entity>
    {
        // Timestamps for entity creation and modification
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        // Unique identifier for the entity
        public virtual Guid Id { get; set; }

        // Constructor to set default values for timestamps
        protected Entity()
        {
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

        public override bool Equals(object? obj) => Equals(obj as Entity);
        public virtual bool Equals(Entity? other)
        {
            if (other is not Entity)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (GetType() != other.GetType())
                return false;

            Entity item = other;

            if (item.IsTransient() || IsTransient())
                return false;
            else
                return item.Id == Id;
        }
        public abstract override string ToString();
    }

}
