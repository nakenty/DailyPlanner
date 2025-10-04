using DailyPlanner.Domain.Models;
using System;
using System.Threading.Tasks;

namespace DailyPlanner.Application.Interfaces
{
    /// <summary>
    /// Defines operations for computing key performance indicators from the
    /// user's tasks.  KPIs provide insight into productivity and task health.
    /// </summary>
    public interface IKpiService
    {
        /// <summary>
        /// Returns a summary of task performance within the specified date range.
        /// When startDate or endDate are null, the range is unbounded on that
        /// side.
        /// </summary>
        Task<KpiSummary> GetKpiSummaryAsync(DateTime? startDate, DateTime? endDate);
    }
}