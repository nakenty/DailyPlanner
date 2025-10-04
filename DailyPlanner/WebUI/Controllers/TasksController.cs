using DailyPlanner.Application.Interfaces;
using DailyPlanner.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace DailyPlanner.WebUI.Controllers
{
    /// <summary>
    /// API controller for managing tasks.  Demonstrates basic CRUD operations
    /// against the ITaskService.  The endpoints here can be expanded as more
    /// features (e.g. updating tasks, filtering by status) are required.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// Gets all tasks with a due date matching the provided date.
        /// </summary>
        /// <param name="date">Date for which to retrieve tasks (yyyy‑MM‑dd format)</param>
        [HttpGet("{date}")]
        public async Task<IEnumerable<TaskItem>> Get(DateTime date)
        {
            return await _taskService.GetTasksByDateAsync(date);
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TaskItem task)
        {
            await _taskService.AddTaskAsync(task);
            return CreatedAtAction(nameof(Get), new { date = task.DueDate?.ToString("yyyy-MM-dd") }, task);
        }

        /// <summary>
        /// Marks a task as completed.
        /// </summary>
        [HttpPut("{id}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            await _taskService.CompleteTaskAsync(id);
            return NoContent();
        }
    }
}