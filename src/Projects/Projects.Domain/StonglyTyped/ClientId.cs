namespace Projects.Domain.StonglyTyped
{
    public readonly record struct ClientId(Guid Id) : IComparable<ClientId>, IEquatable<ClientId>
    {
        public override string ToString() => Id.ToString();

        // IComparable interface implementation
        public int CompareTo(ClientId other) => Id.CompareTo(other.Id);
        // IEquatable interface implementation
        public static bool operator <(ClientId left, ClientId right) => left.CompareTo(right) < 0;
        public static bool operator <=(ClientId left, ClientId right) => left.CompareTo(right) <= 0;
        public static bool operator >(ClientId left, ClientId right) => left.CompareTo(right) > 0;
        public static bool operator >=(ClientId left, ClientId right) => left.CompareTo(right) >= 0;

        // Implicit conversion operators for cases of direct assignment
        public static implicit operator Guid(ClientId id) => id.Id;
        public static implicit operator ClientId(Guid id) => new(id);

        // Utility methods
        public static ClientId Empty => new(Guid.Empty);
        public static ClientId New => new(Guid.NewGuid());
    }
}
