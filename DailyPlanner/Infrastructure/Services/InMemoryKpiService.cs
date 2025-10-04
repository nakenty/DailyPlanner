using DailyPlanner.Application.Interfaces;
using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyPlanner.Infrastructure.Services
{
    /// <summary>
    /// Computes KPI summaries using data available from the task service.  Since
    /// this implementation relies on the in‑memory task list, calculations are
    /// performed in memory as well.
    /// </summary>
    public class InMemoryKpiService : IKpiService
    {
        private readonly ITaskService _taskService;

        public InMemoryKpiService(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<KpiSummary> GetKpiSummaryAsync(DateTime? startDate, DateTime? endDate)
        {
            // The ITaskService only exposes a method to get tasks by date.  For a
            // quick in‑memory implementation we will iterate through a reasonable
            // date range one day at a time.  In a real implementation the
            // underlying data store should support range queries.
            DateTime start = startDate ?? DateTime.UtcNow.Date.AddMonths(-1);
            DateTime end = endDate ?? DateTime.UtcNow.Date;

            var allTasks = new List<TaskItem>();
            for (var d = start.Date; d <= end.Date; d = d.AddDays(1))
            {
                var tasks = await _taskService.GetTasksByDateAsync(d);
                allTasks.AddRange(tasks);
            }

            var summary = new KpiSummary();
            summary.TotalTasks = allTasks.Count;
            summary.CompletedCount = allTasks.Count(t => t.Status == TaskStatus.Completed);
            summary.OverdueCount = allTasks.Count(t =>
                t.Status != TaskStatus.Completed && t.DueDate.HasValue && t.DueDate.Value.Date < DateTime.UtcNow.Date);
            summary.OnTimeCount = allTasks.Count(t => t.Status == TaskStatus.Completed && t.DueDate.HasValue && t.CompletedDate.HasValue && t.CompletedDate.Value.Date <= t.DueDate.Value.Date);

            var completedDurations = new List<TimeSpan>();
            foreach (var t in allTasks)
            {
                if (t.Status == TaskStatus.Completed && t.CompletedDate.HasValue)
                {
                    DateTime startTime = t.StartedDate ?? t.CreatedDate;
                    DateTime endTime = t.CompletedDate.Value;
                    if (endTime > startTime)
                    {
                        completedDurations.Add(endTime - startTime);
                    }
                }
            }
            if (completedDurations.Any())
            {
                summary.AverageCompletionTime = TimeSpan.FromTicks((long)completedDurations.Average(ts => ts.Ticks));
            }
            return summary;
        }
    }
}