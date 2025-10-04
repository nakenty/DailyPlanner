using DailyPlanner.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyPlanner.Application.Interfaces
{
    /// <summary>
    /// Defines operations for searching across tasks, notes and favorite links.
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Searches for the given keyword in titles and descriptions of tasks,
        /// notes and links.  Returns a collection of search results annotated
        /// with their type for routing in the client.
        /// </summary>
        Task<IEnumerable<SearchResult>> SearchAsync(string query);
    }
}