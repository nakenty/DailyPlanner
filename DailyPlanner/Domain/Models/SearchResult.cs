using System;

namespace DailyPlanner.Domain.Models
{
    /// <summary>
    /// Represents a single result returned by the search service.  Stores the
    /// identifier of the matched entity, its type and a brief title or snippet
    /// for display.
    /// </summary>
    public class SearchResult
    {
        public Guid Id { get; set; }
        public SearchResultType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Snippet { get; set; }
    }
}