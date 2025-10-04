using DailyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyPlanner.Application.Interfaces
{
    /// <summary>
    /// Defines operations for managing favorite links.  These links allow the
    /// user to store and organize frequently visited URLs.
    /// </summary>
    public interface IFavoriteLinkService
    {
        Task<IEnumerable<FavoriteLink>> GetAllLinksAsync();
        Task AddLinkAsync(FavoriteLink link);
        Task DeleteLinkAsync(Guid id);
    }
}