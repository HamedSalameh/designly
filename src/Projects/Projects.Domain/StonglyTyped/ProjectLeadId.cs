namespace Projects.Domain.StonglyTyped
{
    public readonly record struct ProjectLeadId(Guid Id) : IComparable<ProjectLeadId>, IEquatable<ProjectLeadId>
    {
        public override string ToString() => Id.ToString();

        // IComparable interface implementation
        public int CompareTo(ProjectLeadId other) => Id.CompareTo(other.Id);
        // IEquatable interface implementation
        public static bool operator <(ProjectLeadId left, ProjectLeadId right) => left.CompareTo(right) < 0;
        public static bool operator <=(ProjectLeadId left, ProjectLeadId right) => left.CompareTo(right) <= 0;
        public static bool operator >(ProjectLeadId left, ProjectLeadId right) => left.CompareTo(right) > 0;
        public static bool operator >=(ProjectLeadId left, ProjectLeadId right) => left.CompareTo(right) >= 0;

        // Implicit conversion operators for cases of direct assignment
        public static implicit operator Guid(ProjectLeadId id) => id.Id;
        public static implicit operator ProjectLeadId(Guid id) => new(id);

        // Utility methods
        public static ProjectLeadId Empty => new(Guid.Empty);
        public static ProjectLeadId New => new(Guid.NewGuid());
    }
}
