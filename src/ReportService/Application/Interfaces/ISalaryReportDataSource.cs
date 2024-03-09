using System.Threading;
using System.Threading.Tasks;
using ReportService.Application.Features.Reports.SalaryReport;

namespace ReportService.Application.Interfaces;

public interface ISalaryReportDataSource
{
    /// <summary>
    /// Get data for salary report.
    /// </summary>
    Task<SalaryReportData> GetDataAsync(GetSalaryReport query, CancellationToken cancellationToken);
}
