﻿
namespace Clients.Domain.Entities
{
    public abstract class Entity
    {
        public DateTime Created { get; }
        public DateTime Modified { get; set; }

        int? _requestedHashCode;
        public virtual Guid Id { get; set; }
        public virtual Guid TenantId { get; }

        protected Entity() : this(Guid.NewGuid())
        {
        }

        protected Entity(Guid TenantId)
        {
            // TenantId is not nullable, so we can't use the ?? operator
            if (TenantId == Guid.Empty || TenantId == default)
            {
                throw new ArgumentNullException(nameof(TenantId), "TenantId cannot be null");
            }
            
            this.TenantId = TenantId;
            Created = DateTime.UtcNow;
            Modified = DateTime.UtcNow;
        }

        public bool IsTransient()
        {
            return this.Id == default;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not Entity)
                return false;

            if (Object.ReferenceEquals(this, obj))
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;

            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }

        public override int GetHashCode()
        {
            // Ref: https://ericlippert.com/2011/02/28/guidelines-and-rules-for-gethashcode/
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode =
                        this.Id.GetHashCode() ^
                        31; 

                return _requestedHashCode.Value;
            }
            else
                return base.GetHashCode();
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
