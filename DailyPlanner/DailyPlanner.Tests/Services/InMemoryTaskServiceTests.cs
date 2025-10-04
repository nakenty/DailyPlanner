using DailyPlanner.Domain.Entities;
using DailyPlanner.Infrastructure.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TaskStatus = DailyPlanner.Domain.Entities.TaskStatus;

namespace DailyPlanner.Tests.Services
{
    public class InMemoryTaskServiceTests
    {
        [Fact]
        public async Task AddTaskAsync_ShouldAddTask_AndAssignId()
        {
            // Arrange
            var service = new InMemoryTaskService();
            var task = new TaskItem
            {
                Title = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.UtcNow.AddDays(1)
            };

            // Act
            await service.AddTaskAsync(task);

            // Assert
            Assert.NotEqual(Guid.Empty, task.Id);
            var allTasks = await service.GetAllTasksAsync();
            Assert.Single(allTasks);
            Assert.Equal("Test Task", allTasks.First().Title);
        }

        [Fact]
        public async Task CompleteTaskAsync_ShouldMarkTaskAsCompleted()
        {
            // Arrange
            var service = new InMemoryTaskService();
            var task = new TaskItem
            {
                Title = "Task to Complete",
                Status = TaskStatus.InProgress
            };
            await service.AddTaskAsync(task);

            // Act
            await service.CompleteTaskAsync(task.Id);

            // Assert
            var allTasks = await service.GetAllTasksAsync();
            var completedTask = allTasks.First();
            Assert.Equal(TaskStatus.Completed, completedTask.Status);
            Assert.NotNull(completedTask.CompletedDate);
        }

        [Fact]
        public async Task CompleteTaskAsync_WithNonExistentTask_ShouldDoNothing()
        {
            // Arrange
            var service = new InMemoryTaskService();
            var nonExistentId = Guid.NewGuid();

            // Act & Assert (should not throw)
            await service.CompleteTaskAsync(nonExistentId);
        }

        [Fact]
        public async Task GetTasksByDateAsync_ShouldReturnTasksForSpecificDate()
        {
            // Arrange
            var service = new InMemoryTaskService();
            var targetDate = new DateTime(2025, 10, 15);
            var otherDate = new DateTime(2025, 10, 16);

            var task1 = new TaskItem { Title = "Task 1", DueDate = targetDate };
            var task2 = new TaskItem { Title = "Task 2", DueDate = targetDate };
            var task3 = new TaskItem { Title = "Task 3", DueDate = otherDate };

            await service.AddTaskAsync(task1);
            await service.AddTaskAsync(task2);
            await service.AddTaskAsync(task3);

            // Act
            var result = await service.GetTasksByDateAsync(targetDate);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, t => Assert.Equal(targetDate.Date, t.DueDate?.Date));
        }

        [Fact]
        public async Task GetTasksByDateAsync_WithNoTasksForDate_ShouldReturnEmpty()
        {
            // Arrange
            var service = new InMemoryTaskService();
            var date = new DateTime(2025, 10, 15);

            // Act
            var result = await service.GetTasksByDateAsync(date);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllTasksAsync_ShouldReturnAllTasks()
        {
            // Arrange
            var service = new InMemoryTaskService();
            await service.AddTaskAsync(new TaskItem { Title = "Task 1" });
            await service.AddTaskAsync(new TaskItem { Title = "Task 2" });
            await service.AddTaskAsync(new TaskItem { Title = "Task 3" });

            // Act
            var result = await service.GetAllTasksAsync();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllTasksAsync_WhenEmpty_ShouldReturnEmpty()
        {
            // Arrange
            var service = new InMemoryTaskService();

            // Act
            var result = await service.GetAllTasksAsync();

            // Assert
            Assert.Empty(result);
        }
    }
}
