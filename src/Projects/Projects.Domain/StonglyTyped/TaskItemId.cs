using System.Runtime.CompilerServices;

namespace Projects.Domain.StonglyTyped
{
    public readonly record struct TaskItemId(Guid Id) : IComparable<TaskItemId>, IEquatable<TaskItemId>
    {
           public override string ToString() => Id.ToString();

        // IComparable interface implementation
        public int CompareTo(TaskItemId other) => Id.CompareTo(other.Id);
        // IEquatable interface implementation
        public static bool operator <(TaskItemId left, TaskItemId right) => left.CompareTo(right) < 0;
        public static bool operator <=(TaskItemId left, TaskItemId right) => left.CompareTo(right) <= 0;
        public static bool operator >(TaskItemId left, TaskItemId right) => left.CompareTo(right) > 0;
        public static bool operator >=(TaskItemId left, TaskItemId right) => left.CompareTo(right) >= 0;

        // Implicit conversion operators for cases of direct assignment
        public static implicit operator Guid(TaskItemId id) => id.Id;
        public static implicit operator TaskItemId(Guid id) => new(id);

        // Utility methods
        public static TaskItemId Empty => new(Guid.Empty);
        public static TaskItemId New => new(Guid.NewGuid());
    }
}
