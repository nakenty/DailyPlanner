using DailyPlanner.Application.Interfaces;
using DailyPlanner.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyPlanner.WebUI.Controllers
{
    /// <summary>
    /// API controller for managing notes.  Notes can be created, updated,
    /// retrieved and deleted.  Notes may optionally be linked to a task via
    /// TaskItemId.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<IEnumerable<Note>> GetAll()
        {
            return await _noteService.GetAllNotesAsync();
        }

        [HttpGet("task/{taskId}")]
        public async Task<IEnumerable<Note>> GetByTask(Guid taskId)
        {
            return await _noteService.GetNotesByTaskIdAsync(taskId);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Note note)
        {
            await _noteService.AddNoteAsync(note);
            return CreatedAtAction(nameof(GetAll), null, note);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] Note note)
        {
            if (id != note.Id)
            {
                return BadRequest("Note ID mismatch.");
            }
            await _noteService.UpdateNoteAsync(note);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _noteService.DeleteNoteAsync(id);
            return NoContent();
        }
    }
}