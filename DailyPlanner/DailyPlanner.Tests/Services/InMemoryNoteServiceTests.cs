using DailyPlanner.Domain.Entities;
using DailyPlanner.Infrastructure.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DailyPlanner.Tests.Services
{
    public class InMemoryNoteServiceTests
    {
        [Fact]
        public async Task AddNoteAsync_ShouldAddNote_AndAssignId()
        {
            // Arrange
            var service = new InMemoryNoteService();
            var note = new Note
            {
                Title = "Test Note",
                Content = "Test Content"
            };

            // Act
            await service.AddNoteAsync(note);

            // Assert
            Assert.NotEqual(Guid.Empty, note.Id);
            var allNotes = await service.GetAllNotesAsync();
            Assert.Single(allNotes);
            Assert.Equal("Test Note", allNotes.First().Title);
        }

        [Fact]
        public async Task DeleteNoteAsync_ShouldRemoveNote()
        {
            // Arrange
            var service = new InMemoryNoteService();
            var note = new Note { Title = "Note to Delete", Content = "Content" };
            await service.AddNoteAsync(note);

            // Act
            await service.DeleteNoteAsync(note.Id);

            // Assert
            var allNotes = await service.GetAllNotesAsync();
            Assert.Empty(allNotes);
        }

        [Fact]
        public async Task DeleteNoteAsync_WithNonExistentNote_ShouldDoNothing()
        {
            // Arrange
            var service = new InMemoryNoteService();
            var note = new Note { Title = "Existing Note", Content = "Content" };
            await service.AddNoteAsync(note);
            var nonExistentId = Guid.NewGuid();

            // Act
            await service.DeleteNoteAsync(nonExistentId);

            // Assert
            var allNotes = await service.GetAllNotesAsync();
            Assert.Single(allNotes);
        }

        [Fact]
        public async Task UpdateNoteAsync_ShouldUpdateExistingNote()
        {
            // Arrange
            var service = new InMemoryNoteService();
            var note = new Note { Title = "Original Title", Content = "Original Content" };
            await service.AddNoteAsync(note);

            // Act
            note.Title = "Updated Title";
            note.Content = "Updated Content";
            await service.UpdateNoteAsync(note);

            // Assert
            var allNotes = await service.GetAllNotesAsync();
            var updatedNote = allNotes.First();
            Assert.Equal("Updated Title", updatedNote.Title);
            Assert.Equal("Updated Content", updatedNote.Content);
        }

        [Fact]
        public async Task UpdateNoteAsync_WithNonExistentNote_ShouldDoNothing()
        {
            // Arrange
            var service = new InMemoryNoteService();
            var note = new Note
            {
                Id = Guid.NewGuid(),
                Title = "Non-existent Note",
                Content = "Content"
            };

            // Act & Assert (should not throw)
            await service.UpdateNoteAsync(note);
        }

        [Fact]
        public async Task GetNotesByTaskIdAsync_ShouldReturnNotesForSpecificTask()
        {
            // Arrange
            var service = new InMemoryNoteService();
            var taskId = Guid.NewGuid();
            var otherTaskId = Guid.NewGuid();

            var note1 = new Note { Title = "Note 1", Content = "Content 1", TaskItemId = taskId };
            var note2 = new Note { Title = "Note 2", Content = "Content 2", TaskItemId = taskId };
            var note3 = new Note { Title = "Note 3", Content = "Content 3", TaskItemId = otherTaskId };

            await service.AddNoteAsync(note1);
            await service.AddNoteAsync(note2);
            await service.AddNoteAsync(note3);

            // Act
            var result = await service.GetNotesByTaskIdAsync(taskId);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.All(result, n => Assert.Equal(taskId, n.TaskItemId));
        }

        [Fact]
        public async Task GetAllNotesAsync_ShouldReturnAllNotes()
        {
            // Arrange
            var service = new InMemoryNoteService();
            await service.AddNoteAsync(new Note { Title = "Note 1", Content = "Content 1" });
            await service.AddNoteAsync(new Note { Title = "Note 2", Content = "Content 2" });
            await service.AddNoteAsync(new Note { Title = "Note 3", Content = "Content 3" });

            // Act
            var result = await service.GetAllNotesAsync();

            // Assert
            Assert.Equal(3, result.Count());
        }
    }
}
