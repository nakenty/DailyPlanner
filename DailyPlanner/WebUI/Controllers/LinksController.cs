using DailyPlanner.Application.Interfaces;
using DailyPlanner.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyPlanner.WebUI.Controllers
{
    /// <summary>
    /// API controller for managing favorite links.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class LinksController : ControllerBase
    {
        private readonly IFavoriteLinkService _linkService;

        public LinksController(IFavoriteLinkService linkService)
        {
            _linkService = linkService;
        }

        [HttpGet]
        public async Task<IEnumerable<FavoriteLink>> GetAll()
        {
            return await _linkService.GetAllLinksAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FavoriteLink link)
        {
            await _linkService.AddLinkAsync(link);
            return CreatedAtAction(nameof(GetAll), null, link);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _linkService.DeleteLinkAsync(id);
            return NoContent();
        }
    }
}