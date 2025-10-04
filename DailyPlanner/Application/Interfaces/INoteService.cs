using DailyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyPlanner.Application.Interfaces
{
    /// <summary>
    /// Defines operations for managing notes.  Notes can be standalone or linked
    /// to a specific task.
    /// </summary>
    public interface INoteService
    {
        Task<IEnumerable<Note>> GetAllNotesAsync();
        Task<IEnumerable<Note>> GetNotesByTaskIdAsync(Guid taskId);
        Task AddNoteAsync(Note note);
        Task UpdateNoteAsync(Note note);
        Task DeleteNoteAsync(Guid id);
    }
}