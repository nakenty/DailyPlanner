using DailyPlanner.Application.Interfaces;
using DailyPlanner.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyPlanner.Infrastructure.Services
{
    /// <summary>
    /// Performs a basic keyword search across tasks, notes and favorite links.
    /// This implementation is case‑insensitive and performs a simple
    /// contains‑match on titles, descriptions and URLs.  For larger data sets
    /// consider using a full text search engine instead.
    /// </summary>
    public class InMemorySearchService : ISearchService
    {
        private readonly ITaskService _taskService;
        private readonly INoteService _noteService;
        private readonly IFavoriteLinkService _linkService;

        public InMemorySearchService(ITaskService taskService, INoteService noteService, IFavoriteLinkService linkService)
        {
            _taskService = taskService;
            _noteService = noteService;
            _linkService = linkService;
        }

        public async Task<IEnumerable<SearchResult>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Enumerable.Empty<SearchResult>();
            }
            query = query.Trim();
            var results = new List<SearchResult>();

            // Search tasks
            var allTasks = await _taskService.GetAllTasksAsync();
            foreach (var task in allTasks)
            {
                if (task.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    (task.Description != null && task.Description.Contains(query, StringComparison.OrdinalIgnoreCase)))
                {
                    results.Add(new SearchResult
                    {
                        Id = task.Id,
                        Type = SearchResultType.Task,
                        Title = task.Title,
                        Snippet = task.Description
                    });
                }
            }

            // Search notes
            var notes = await _noteService.GetAllNotesAsync();
            foreach (var note in notes)
            {
                if (note.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    note.Content.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    results.Add(new SearchResult
                    {
                        Id = note.Id,
                        Type = SearchResultType.Note,
                        Title = note.Title,
                        Snippet = note.Content.Length > 100 ? note.Content.Substring(0, 100) + "…" : note.Content
                    });
                }
            }

            // Search favorite links
            var links = await _linkService.GetAllLinksAsync();
            foreach (var link in links)
            {
                if (link.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    link.Url.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    (link.Category != null && link.Category.Contains(query, StringComparison.OrdinalIgnoreCase)))
                {
                    results.Add(new SearchResult
                    {
                        Id = link.Id,
                        Type = SearchResultType.FavoriteLink,
                        Title = link.Title,
                        Snippet = link.Url
                    });
                }
            }

            return results;
        }
    }
}