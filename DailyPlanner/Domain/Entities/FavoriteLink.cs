using System;

namespace DailyPlanner.Domain.Entities
{
    /// <summary>
    /// Represents a bookmarked URL that the user frequently visits.  Links can be
    /// grouped into categories for better organization.
    /// </summary>
    public class FavoriteLink
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Category { get; set; }
    }
}