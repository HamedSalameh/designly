namespace Accounts.Domain
{
    public abstract class Entity
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

        // Check if the entity is transient (i.e., not yet persisted)
        public bool IsTransient()
        {
            return Id == Guid.Empty;
        }

        // Override the Equals method to compare entities
        public override bool Equals(object? obj)
        {
            if (obj is not Entity)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;

            // Check for transient entities or compare Ids
            return !(item.IsTransient() || IsTransient()) && item.Id == Id;
        }

        // Override the GetHashCode method for use in hash-based collections
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

        // Override the equality operators for convenient usage
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
