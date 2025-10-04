using System;

namespace DailyPlanner.Domain.Models
{
    /// <summary>
    /// Represents a summary of task performance over a given time range.  This
    /// structure can be extended to include additional KPI values as needed.
    /// </summary>
    public class KpiSummary
    {
        public int TotalTasks { get; set; }
        public int CompletedCount { get; set; }
        public int OverdueCount { get; set; }
        /// <summary>
        /// Total duration of tasks from start to completion; divided by number of
        /// completed tasks to compute an average.  When null, no tasks were
        /// completed in the range.
        /// </summary>
        public TimeSpan? AverageCompletionTime { get; set; }
        /// <summary>
        /// Total number of tasks that were completed on or before the due date.
        /// </summary>
        public int OnTimeCount { get; set; }
    }
}