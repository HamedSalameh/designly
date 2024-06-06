using Designly.Base;

namespace Projects.Domain.Tasks
{
    public static class TaskItemErrors
    {
        public static readonly Error TaskItemNotCreatedYet = new("task_item_not_created_yet", "Task item is not created yet");
        public static readonly Error DueDateCannotBeInThePast = new("due_date_cannot_be_in_the_past", "Due date cannot be in the past");
    }
}
