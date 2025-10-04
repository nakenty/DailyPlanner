using DailyPlanner.Application.Interfaces;
using DailyPlanner.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DailyPlanner.WebUI.Controllers
{
    /// <summary>
    /// API controller for retrieving key performance indicators.  Clients can
    /// specify a start and end date to bound the metrics; when omitted, the
    /// implementation chooses sensible defaults.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class KpiController : ControllerBase
    {
        private readonly IKpiService _kpiService;

        public KpiController(IKpiService kpiService)
        {
            _kpiService = kpiService;
        }

        [HttpGet]
        public async Task<KpiSummary> Get([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            return await _kpiService.GetKpiSummaryAsync(startDate, endDate);
        }
    }
}