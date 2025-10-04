using DailyPlanner.Application.Interfaces;
using DailyPlanner.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DailyPlanner.WebUI.Controllers
{
    /// <summary>
    /// API controller for performing keyword searches across tasks, notes and
    /// favorite links.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IEnumerable<SearchResult>> Get([FromQuery] string q)
        {
            return await _searchService.SearchAsync(q);
        }
    }
}