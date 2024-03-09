using System.Threading.Tasks;
using ReportService.Application.DTOs;

namespace ReportService.Application.Interfaces;

public interface IReportBuilder<TReportData>
{
    IReportBuilder<TReportData> Init(TReportData reportData);
    IReportBuilder<TReportData> Build();
    Task<ReportDto> ExportToTxtAsync(string reportName = "report");
}
