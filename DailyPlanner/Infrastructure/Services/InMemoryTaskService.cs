using DailyPlanner.Application.Interfaces;
using DailyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyPlanner.Infrastructure.Services
{
    /// <summary>
    /// A simple in‑memory implementation of <see cref="ITaskService"/>.  This is
    /// primarily intended for prototyping and testing.  For production use, swap
    /// this service out for one that persists tasks to a data store (e.g. a
    /// OneDrive file, database or cloud storage).
    /// </summary>
    public class InMemoryTaskService : ITaskService
    {
        private readonly List<TaskItem> _tasks = new();

        public Task AddTaskAsync(TaskItem task)
        {
            task.Id = Guid.NewGuid();
            _tasks.Add(task);
            return Task.CompletedTask;
        }

        public Task CompleteTaskAsync(Guid taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.Status = TaskStatus.Completed;
                task.CompletedDate = DateTime.UtcNow;
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<TaskItem>> GetTasksByDateAsync(DateTime date)
        {
            var result = _tasks.Where(t => t.DueDate?.Date == date.Date);
            return Task.FromResult(result.AsEnumerable());
        }

        public Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            // Return a shallow copy to prevent external modification.
            return Task.FromResult(_tasks.ToList().AsEnumerable());
        }
    }
}