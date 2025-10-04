using System;

namespace DailyPlanner.Domain.Entities
{
    /// <summary>
    /// Represents a note that can be linked to a task or stand on its own.  Notes are
    /// useful for jotting down ideas, meeting minutes, or project‑specific information.
    /// </summary>
    public class Note
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        /// <summary>
        /// Optional foreign key linking this note to a task item.  When null the note
        /// is considered a standalone entry.
        /// </summary>
        public Guid? TaskItemId { get; set; }
    }
}