using Designly.Base;

namespace Projects.Domain.Tasks
{
    public static class TaskItemErrors
    {
        public static readonly Error TaskItemNotCreatedYet = new("task_item_not_created_yet", "Task item is not created yet");
    }
}
