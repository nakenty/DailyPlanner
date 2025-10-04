using DailyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyPlanner.Application.Interfaces
{
    /// <summary>
    /// Defines operations for managing tasks.  Concrete implementations live in the
    /// Infrastructure layer and can persist data in memory, files, databases or cloud
    /// services.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Retrieves all tasks in the system.  Implementations should return a
        /// copy of the underlying collection to avoid modification by callers.
        /// </summary>
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        /// <summary>
        /// Retrieves all tasks with a due date on the specified day.
        /// </summary>
        Task<IEnumerable<TaskItem>> GetTasksByDateAsync(DateTime date);

        /// <summary>
        /// Adds a new task to the system.  Implementations should assign an ID and
        /// set any default fields (e.g. CreatedDate).
        /// </summary>
        Task AddTaskAsync(TaskItem task);

        /// <summary>
        /// Marks the task with the given identifier as completed.
        /// </summary>
        Task CompleteTaskAsync(Guid taskId);
    }
}