using Projects.Domain.Tasks;

namespace Projects.Application.Features.CreateTask
{
    public class CreateTaskRequestDto
    {
        public required string Name { get; set; }
        //public required Guid TaskGroupId { get; set; }
        //public TaskGroup TaskGroup { get; set; }
        public required Guid ProjectId { get; set; }
        public Guid? AssignedTo { get; set; }
        public Guid? AssignedBy { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Description { get; set; }
        public TaskItemStatus taskItemStatus { get; set; }
    }
}
