using DailyPlanner.Application.Interfaces;
using DailyPlanner.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DailyPlanner.Infrastructure.Services
{
    /// <summary>
    /// In‑memory implementation of <see cref="IFavoriteLinkService"/>.
    /// </summary>
    public class InMemoryFavoriteLinkService : IFavoriteLinkService
    {
        private readonly List<FavoriteLink> _links = new();

        public Task AddLinkAsync(FavoriteLink link)
        {
            link.Id = Guid.NewGuid();
            _links.Add(link);
            return Task.CompletedTask;
        }

        public Task DeleteLinkAsync(Guid id)
        {
            var link = _links.FirstOrDefault(l => l.Id == id);
            if (link != null)
            {
                _links.Remove(link);
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<FavoriteLink>> GetAllLinksAsync()
        {
            return Task.FromResult<IEnumerable<FavoriteLink>>(_links.ToList());
        }
    }
}