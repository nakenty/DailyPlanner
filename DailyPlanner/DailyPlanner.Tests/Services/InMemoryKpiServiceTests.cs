using DailyPlanner.Application.Interfaces;
using DailyPlanner.Domain.Entities;
using DailyPlanner.Infrastructure.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TaskStatus = DailyPlanner.Domain.Entities.TaskStatus;

namespace DailyPlanner.Tests.Services
{
    public class InMemoryKpiServiceTests
    {
        private ITaskService CreateTaskServiceWithSampleData()
        {
            var taskService = new InMemoryTaskService();
            
            // Completed tasks
            var completedTask1 = new TaskItem
            {
                Title = "Completed Task 1",
                Status = TaskStatus.Completed,
                DueDate = DateTime.UtcNow.Date.AddDays(-5),
                CreatedDate = DateTime.UtcNow.Date.AddDays(-7),
                CompletedDate = DateTime.UtcNow.Date.AddDays(-5)
            };
            
            var completedTask2 = new TaskItem
            {
                Title = "Completed Task 2",
                Status = TaskStatus.Completed,
                DueDate = DateTime.UtcNow.Date.AddDays(-3),
                CreatedDate = DateTime.UtcNow.Date.AddDays(-5),
                CompletedDate = DateTime.UtcNow.Date.AddDays(-3)
            };

            // Overdue task
            var overdueTask = new TaskItem
            {
                Title = "Overdue Task",
                Status = TaskStatus.InProgress,
                DueDate = DateTime.UtcNow.Date.AddDays(-2)
            };

            // Pending task
            var pendingTask = new TaskItem
            {
                Title = "Pending Task",
                Status = TaskStatus.NotStarted,
                DueDate = DateTime.UtcNow.Date.AddDays(2)
            };

            taskService.AddTaskAsync(completedTask1).Wait();
            taskService.AddTaskAsync(completedTask2).Wait();
            taskService.AddTaskAsync(overdueTask).Wait();
            taskService.AddTaskAsync(pendingTask).Wait();

            return taskService;
        }

        [Fact]
        public async Task GetKpiSummaryAsync_ShouldCalculateTotalTasks()
        {
            // Arrange
            var taskService = CreateTaskServiceWithSampleData();
            var kpiService = new InMemoryKpiService(taskService);

            // Act
            var summary = await kpiService.GetKpiSummaryAsync(
                DateTime.UtcNow.Date.AddDays(-30),
                DateTime.UtcNow.Date.AddDays(30)
            );

            // Assert
            Assert.Equal(4, summary.TotalTasks);
        }

        [Fact]
        public async Task GetKpiSummaryAsync_ShouldCalculateCompletedCount()
        {
            // Arrange
            var taskService = CreateTaskServiceWithSampleData();
            var kpiService = new InMemoryKpiService(taskService);

            // Act
            var summary = await kpiService.GetKpiSummaryAsync(
                DateTime.UtcNow.Date.AddDays(-30),
                DateTime.UtcNow.Date.AddDays(30)
            );

            // Assert
            Assert.Equal(2, summary.CompletedCount);
        }

        [Fact]
        public async Task GetKpiSummaryAsync_ShouldCalculateOverdueCount()
        {
            // Arrange
            var taskService = CreateTaskServiceWithSampleData();
            var kpiService = new InMemoryKpiService(taskService);

            // Act
            var summary = await kpiService.GetKpiSummaryAsync(
                DateTime.UtcNow.Date.AddDays(-30),
                DateTime.UtcNow.Date.AddDays(30)
            );

            // Assert
            Assert.Equal(1, summary.OverdueCount);
        }

        [Fact]
        public async Task GetKpiSummaryAsync_ShouldCalculateOnTimeCount()
        {
            // Arrange
            var taskService = CreateTaskServiceWithSampleData();
            var kpiService = new InMemoryKpiService(taskService);

            // Act
            var summary = await kpiService.GetKpiSummaryAsync(
                DateTime.UtcNow.Date.AddDays(-30),
                DateTime.UtcNow.Date.AddDays(30)
            );

            // Assert
            Assert.Equal(2, summary.OnTimeCount);
        }

        [Fact]
        public async Task GetKpiSummaryAsync_WithNoTasks_ShouldReturnZeros()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            var kpiService = new InMemoryKpiService(taskService);

            // Act
            var summary = await kpiService.GetKpiSummaryAsync(
                DateTime.UtcNow.Date.AddDays(-30),
                DateTime.UtcNow.Date.AddDays(30)
            );

            // Assert
            Assert.Equal(0, summary.TotalTasks);
            Assert.Equal(0, summary.CompletedCount);
            Assert.Equal(0, summary.OverdueCount);
            Assert.Equal(0, summary.OnTimeCount);
        }

        [Fact]
        public async Task GetKpiSummaryAsync_ShouldCalculateAverageCompletionTime()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            
            var task1 = new TaskItem
            {
                Title = "Task 1",
                Status = TaskStatus.Completed,
                DueDate = DateTime.UtcNow.Date,
                CreatedDate = DateTime.UtcNow.Date.AddDays(-2),
                CompletedDate = DateTime.UtcNow.Date
            };
            
            var task2 = new TaskItem
            {
                Title = "Task 2",
                Status = TaskStatus.Completed,
                DueDate = DateTime.UtcNow.Date,
                CreatedDate = DateTime.UtcNow.Date.AddDays(-4),
                CompletedDate = DateTime.UtcNow.Date
            };

            await taskService.AddTaskAsync(task1);
            await taskService.AddTaskAsync(task2);

            var kpiService = new InMemoryKpiService(taskService);

            // Act
            var summary = await kpiService.GetKpiSummaryAsync(
                DateTime.UtcNow.Date.AddDays(-30),
                DateTime.UtcNow.Date.AddDays(30)
            );

            // Assert
            Assert.NotNull(summary.AverageCompletionTime);
            Assert.Equal(TimeSpan.FromDays(3), summary.AverageCompletionTime);
        }

        [Fact]
        public async Task GetKpiSummaryAsync_WithNullDates_ShouldUseDefaultRange()
        {
            // Arrange
            var taskService = CreateTaskServiceWithSampleData();
            var kpiService = new InMemoryKpiService(taskService);

            // Act
            var summary = await kpiService.GetKpiSummaryAsync(null, null);

            // Assert
            Assert.True(summary.TotalTasks > 0);
        }
    }
}
