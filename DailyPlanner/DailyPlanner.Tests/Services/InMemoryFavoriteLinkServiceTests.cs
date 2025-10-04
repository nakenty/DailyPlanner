using DailyPlanner.Domain.Entities;
using DailyPlanner.Infrastructure.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DailyPlanner.Tests.Services
{
    public class InMemoryFavoriteLinkServiceTests
    {
        [Fact]
        public async Task AddLinkAsync_ShouldAddLink_AndAssignId()
        {
            // Arrange
            var service = new InMemoryFavoriteLinkService();
            var link = new FavoriteLink
            {
                Title = "GitHub",
                Url = "https://github.com",
                Category = "Development"
            };

            // Act
            await service.AddLinkAsync(link);

            // Assert
            Assert.NotEqual(Guid.Empty, link.Id);
            var allLinks = await service.GetAllLinksAsync();
            Assert.Single(allLinks);
            Assert.Equal("GitHub", allLinks.First().Title);
        }

        [Fact]
        public async Task DeleteLinkAsync_ShouldRemoveLink()
        {
            // Arrange
            var service = new InMemoryFavoriteLinkService();
            var link = new FavoriteLink
            {
                Title = "Google",
                Url = "https://google.com"
            };
            await service.AddLinkAsync(link);

            // Act
            await service.DeleteLinkAsync(link.Id);

            // Assert
            var allLinks = await service.GetAllLinksAsync();
            Assert.Empty(allLinks);
        }

        [Fact]
        public async Task DeleteLinkAsync_WithNonExistentLink_ShouldDoNothing()
        {
            // Arrange
            var service = new InMemoryFavoriteLinkService();
            var link = new FavoriteLink
            {
                Title = "Existing Link",
                Url = "https://example.com"
            };
            await service.AddLinkAsync(link);
            var nonExistentId = Guid.NewGuid();

            // Act
            await service.DeleteLinkAsync(nonExistentId);

            // Assert
            var allLinks = await service.GetAllLinksAsync();
            Assert.Single(allLinks);
        }

        [Fact]
        public async Task GetAllLinksAsync_ShouldReturnAllLinks()
        {
            // Arrange
            var service = new InMemoryFavoriteLinkService();
            await service.AddLinkAsync(new FavoriteLink { Title = "Link 1", Url = "https://example1.com" });
            await service.AddLinkAsync(new FavoriteLink { Title = "Link 2", Url = "https://example2.com" });
            await service.AddLinkAsync(new FavoriteLink { Title = "Link 3", Url = "https://example3.com" });

            // Act
            var result = await service.GetAllLinksAsync();

            // Assert
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllLinksAsync_WhenEmpty_ShouldReturnEmpty()
        {
            // Arrange
            var service = new InMemoryFavoriteLinkService();

            // Act
            var result = await service.GetAllLinksAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task AddMultipleLinks_ShouldMaintainAll()
        {
            // Arrange
            var service = new InMemoryFavoriteLinkService();
            var links = new[]
            {
                new FavoriteLink { Title = "GitHub", Url = "https://github.com", Category = "Dev" },
                new FavoriteLink { Title = "Stack Overflow", Url = "https://stackoverflow.com", Category = "Dev" },
                new FavoriteLink { Title = "Reddit", Url = "https://reddit.com", Category = "Social" }
            };

            // Act
            foreach (var link in links)
            {
                await service.AddLinkAsync(link);
            }

            // Assert
            var allLinks = await service.GetAllLinksAsync();
            Assert.Equal(3, allLinks.Count());
            Assert.All(allLinks, link => Assert.NotEqual(Guid.Empty, link.Id));
        }
    }
}
