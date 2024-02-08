namespace Projects.Domain.StonglyTyped
{
    public readonly record struct ProjectId(Guid Id) : IComparable<ProjectId>, IEquatable<ProjectId>
    {
        public override string ToString() => Id.ToString();

        // IComparable interface implementation
        public int CompareTo(ProjectId other) => Id.CompareTo(other.Id);
        // IEquatable interface implementation
        public static bool operator <(ProjectId left, ProjectId right) => left.CompareTo(right) < 0;
        public static bool operator <=(ProjectId left, ProjectId right) => left.CompareTo(right) <= 0;
        public static bool operator >(ProjectId left, ProjectId right) => left.CompareTo(right) > 0;
        public static bool operator >=(ProjectId left, ProjectId right) => left.CompareTo(right) >= 0;

        // Implicit conversion operators for cases of direct assignment
        public static implicit operator Guid(ProjectId id) => id.Id;
        public static implicit operator ProjectId(Guid id) => new(id);

        // Utility methods
        public static ProjectId Empty => new(Guid.Empty);
        public static ProjectId New => new(Guid.NewGuid());
    }
}
