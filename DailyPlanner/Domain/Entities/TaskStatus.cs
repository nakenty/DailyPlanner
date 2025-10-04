namespace DailyPlanner.Domain.Entities
{
    /// <summary>
    /// Represents the possible states of a task in the planner.
    /// </summary>
    public enum TaskStatus
    {
        NotStarted,
        InProgress,
        Completed,
        Overdue
    }
}