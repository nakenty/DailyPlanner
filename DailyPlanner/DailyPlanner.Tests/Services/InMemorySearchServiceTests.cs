using DailyPlanner.Domain.Entities;
using DailyPlanner.Domain.Models;
using DailyPlanner.Infrastructure.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DailyPlanner.Tests.Services
{
    public class InMemorySearchServiceTests
    {
        [Fact]
        public async Task SearchAsync_ShouldFindTasksByTitle()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            var noteService = new InMemoryNoteService();
            var linkService = new InMemoryFavoriteLinkService();
            var searchService = new InMemorySearchService(taskService, noteService, linkService);

            await taskService.AddTaskAsync(new TaskItem { Title = "Complete project documentation", Description = "Write docs" });
            await taskService.AddTaskAsync(new TaskItem { Title = "Review code", Description = "Check pull requests" });

            // Act
            var results = await searchService.SearchAsync("project");

            // Assert
            Assert.Single(results);
            Assert.Equal(SearchResultType.Task, results.First().Type);
            Assert.Contains("project", results.First().Title, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task SearchAsync_ShouldFindTasksByDescription()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            var noteService = new InMemoryNoteService();
            var linkService = new InMemoryFavoriteLinkService();
            var searchService = new InMemorySearchService(taskService, noteService, linkService);

            await taskService.AddTaskAsync(new TaskItem { Title = "Task 1", Description = "Important meeting notes" });

            // Act
            var results = await searchService.SearchAsync("meeting");

            // Assert
            Assert.Single(results);
            Assert.Equal(SearchResultType.Task, results.First().Type);
        }

        [Fact]
        public async Task SearchAsync_ShouldFindNotesByTitle()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            var noteService = new InMemoryNoteService();
            var linkService = new InMemoryFavoriteLinkService();
            var searchService = new InMemorySearchService(taskService, noteService, linkService);

            await noteService.AddNoteAsync(new Note { Title = "Meeting notes", Content = "Discussion points" });
            await noteService.AddNoteAsync(new Note { Title = "Ideas", Content = "New features" });

            // Act
            var results = await searchService.SearchAsync("Meeting");

            // Assert
            Assert.Single(results);
            Assert.Equal(SearchResultType.Note, results.First().Type);
            Assert.Contains("Meeting", results.First().Title);
        }

        [Fact]
        public async Task SearchAsync_ShouldFindNotesByContent()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            var noteService = new InMemoryNoteService();
            var linkService = new InMemoryFavoriteLinkService();
            var searchService = new InMemorySearchService(taskService, noteService, linkService);

            await noteService.AddNoteAsync(new Note { Title = "Random Title", Content = "This contains important information" });

            // Act
            var results = await searchService.SearchAsync("important");

            // Assert
            Assert.Single(results);
            Assert.Equal(SearchResultType.Note, results.First().Type);
        }

        [Fact]
        public async Task SearchAsync_ShouldFindLinksByTitle()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            var noteService = new InMemoryNoteService();
            var linkService = new InMemoryFavoriteLinkService();
            var searchService = new InMemorySearchService(taskService, noteService, linkService);

            await linkService.AddLinkAsync(new FavoriteLink { Title = "GitHub Repository", Url = "https://github.com/user/repo" });
            await linkService.AddLinkAsync(new FavoriteLink { Title = "Documentation", Url = "https://docs.example.com" });

            // Act
            var results = await searchService.SearchAsync("github");

            // Assert
            Assert.Single(results);
            Assert.Equal(SearchResultType.FavoriteLink, results.First().Type);
        }

        [Fact]
        public async Task SearchAsync_ShouldFindLinksByUrl()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            var noteService = new InMemoryNoteService();
            var linkService = new InMemoryFavoriteLinkService();
            var searchService = new InMemorySearchService(taskService, noteService, linkService);

            await linkService.AddLinkAsync(new FavoriteLink { Title = "Example", Url = "https://stackoverflow.com/questions" });

            // Act
            var results = await searchService.SearchAsync("stackoverflow");

            // Assert
            Assert.Single(results);
            Assert.Equal(SearchResultType.FavoriteLink, results.First().Type);
        }

        [Fact]
        public async Task SearchAsync_ShouldFindLinksByCategory()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            var noteService = new InMemoryNoteService();
            var linkService = new InMemoryFavoriteLinkService();
            var searchService = new InMemorySearchService(taskService, noteService, linkService);

            await linkService.AddLinkAsync(new FavoriteLink 
            { 
                Title = "Dev Tool", 
                Url = "https://example.com",
                Category = "Development"
            });

            // Act
            var results = await searchService.SearchAsync("Development");

            // Assert
            Assert.Single(results);
            Assert.Equal(SearchResultType.FavoriteLink, results.First().Type);
        }

        [Fact]
        public async Task SearchAsync_ShouldFindMultipleResultsAcrossTypes()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            var noteService = new InMemoryNoteService();
            var linkService = new InMemoryFavoriteLinkService();
            var searchService = new InMemorySearchService(taskService, noteService, linkService);

            await taskService.AddTaskAsync(new TaskItem { Title = "Python tutorial", Description = "Learn Python" });
            await noteService.AddNoteAsync(new Note { Title = "Python notes", Content = "Key concepts" });
            await linkService.AddLinkAsync(new FavoriteLink { Title = "Python docs", Url = "https://python.org" });

            // Act
            var results = await searchService.SearchAsync("python");

            // Assert
            Assert.Equal(3, results.Count());
            Assert.Contains(results, r => r.Type == SearchResultType.Task);
            Assert.Contains(results, r => r.Type == SearchResultType.Note);
            Assert.Contains(results, r => r.Type == SearchResultType.FavoriteLink);
        }

        [Fact]
        public async Task SearchAsync_WithEmptyQuery_ShouldReturnEmpty()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            var noteService = new InMemoryNoteService();
            var linkService = new InMemoryFavoriteLinkService();
            var searchService = new InMemorySearchService(taskService, noteService, linkService);

            await taskService.AddTaskAsync(new TaskItem { Title = "Task", Description = "Description" });

            // Act
            var results = await searchService.SearchAsync("");

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public async Task SearchAsync_WithWhitespaceQuery_ShouldReturnEmpty()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            var noteService = new InMemoryNoteService();
            var linkService = new InMemoryFavoriteLinkService();
            var searchService = new InMemorySearchService(taskService, noteService, linkService);

            await taskService.AddTaskAsync(new TaskItem { Title = "Task", Description = "Description" });

            // Act
            var results = await searchService.SearchAsync("   ");

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public async Task SearchAsync_WithNoMatches_ShouldReturnEmpty()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            var noteService = new InMemoryNoteService();
            var linkService = new InMemoryFavoriteLinkService();
            var searchService = new InMemorySearchService(taskService, noteService, linkService);

            await taskService.AddTaskAsync(new TaskItem { Title = "Task", Description = "Description" });

            // Act
            var results = await searchService.SearchAsync("nonexistent");

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public async Task SearchAsync_ShouldBeCaseInsensitive()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            var noteService = new InMemoryNoteService();
            var linkService = new InMemoryFavoriteLinkService();
            var searchService = new InMemorySearchService(taskService, noteService, linkService);

            await taskService.AddTaskAsync(new TaskItem { Title = "IMPORTANT TASK", Description = "Description" });

            // Act
            var results = await searchService.SearchAsync("important");

            // Assert
            Assert.Single(results);
        }

        [Fact]
        public async Task SearchAsync_ShouldTruncateLongNoteSnippets()
        {
            // Arrange
            var taskService = new InMemoryTaskService();
            var noteService = new InMemoryNoteService();
            var linkService = new InMemoryFavoriteLinkService();
            var searchService = new InMemorySearchService(taskService, noteService, linkService);

            var longContent = new string('a', 150) + " searchterm";
            await noteService.AddNoteAsync(new Note { Title = "Note", Content = longContent });

            // Act
            var results = await searchService.SearchAsync("searchterm");

            // Assert
            Assert.Single(results);
            var snippet = results.First().Snippet;
            Assert.NotNull(snippet);
            Assert.True(snippet.Length <= 101); // 100 chars + ellipsis
            Assert.EndsWith("…", snippet);
        }
    }
}
