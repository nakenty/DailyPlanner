namespace DailyPlanner.Domain.Models
{
    /// <summary>
    /// Distinguishes the type of entity returned by the search service.  This helps
    /// the client determine which controller or view to navigate to when a
    /// particular result is selected.
    /// </summary>
    public enum SearchResultType
    {
        Task,
        Note,
        FavoriteLink
    }
}