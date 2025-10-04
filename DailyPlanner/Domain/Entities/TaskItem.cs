using System;

namespace DailyPlanner.Domain.Entities
{
    /// <summary>
    /// Represents a single task item in the planner.  Each task can have a due date,
    /// status, priority and timestamps to track its lifecycle.
    /// </summary>
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskStatus Status { get; set; } = TaskStatus.NotStarted;
        /// <summary>
        /// Arbitrary numeric priority; you can adapt this to your own scale.
        /// </summary>
        public int Priority { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? StartedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}