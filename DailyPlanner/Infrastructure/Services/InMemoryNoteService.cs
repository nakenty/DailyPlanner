using DailyPlanner.Application.Interfaces;
using DailyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyPlanner.Infrastructure.Services
{
    /// <summary>
    /// In‑memory implementation of <see cref="INoteService"/>.  Stores notes in a
    /// local list.  This implementation is not thread‑safe and should be
    /// replaced by a persistent storage mechanism in production.
    /// </summary>
    public class InMemoryNoteService : INoteService
    {
        private readonly List<Note> _notes = new();

        public Task AddNoteAsync(Note note)
        {
            note.Id = Guid.NewGuid();
            _notes.Add(note);
            return Task.CompletedTask;
        }

        public Task DeleteNoteAsync(Guid id)
        {
            var note = _notes.FirstOrDefault(n => n.Id == id);
            if (note != null)
            {
                _notes.Remove(note);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            return Task.FromResult<IEnumerable<Note>>(_notes.ToList());
        }

        public Task<IEnumerable<Note>> GetNotesByTaskIdAsync(Guid taskId)
        {
            var result = _notes.Where(n => n.TaskItemId == taskId);
            return Task.FromResult<IEnumerable<Note>>(result.ToList());
        }

        public Task UpdateNoteAsync(Note note)
        {
            var existing = _notes.FirstOrDefault(n => n.Id == note.Id);
            if (existing != null)
            {
                existing.Title = note.Title;
                existing.Content = note.Content;
                existing.TaskItemId = note.TaskItemId;
            }
            return Task.CompletedTask;
        }
    }
}