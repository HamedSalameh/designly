using Projects.Domain.Tasks;

namespace Projects.Application.Features.CreateTask
{
    public sealed class CreateTaskRequestDto
    {
        public required string Name { get; set; }
        public required Guid ProjectId { get; set; }
        public Guid? AssignedTo { get; set; }
        public Guid? AssignedBy { get; set; }
        public DateTime? DueDate { get; set; }  
        public DateTime? CompletedAt { get; set; }
        public string? Description { get; set; }
        public TaskItemStatus taskItemStatus { get; set; }
    }
}
