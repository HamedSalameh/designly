namespace Projects.Domain.StonglyTyped
{
    public readonly record struct TenantId(Guid Id) : IComparable<TenantId>, IEquatable<TenantId>
    {
        public override string ToString() => Id.ToString();

        // IComparable interface implementation
        public int CompareTo(TenantId other) => Id.CompareTo(other.Id);
        // IEquatable interface implementation
        public static bool operator <(TenantId left, TenantId right) => left.CompareTo(right) < 0;
        public static bool operator <=(TenantId left, TenantId right) => left.CompareTo(right) <= 0;
        public static bool operator >(TenantId left, TenantId right) => left.CompareTo(right) > 0;
        public static bool operator >=(TenantId left, TenantId right) => left.CompareTo(right) >= 0;

        // Implicit conversion operators for cases of direct assignment
        public static implicit operator Guid(TenantId id) => id.Id;
        public static implicit operator TenantId(Guid id) => new(id);

        // Utility methods
        public static TenantId Empty => new(Guid.Empty);
        public static TenantId New => new(Guid.NewGuid());
    }
}
