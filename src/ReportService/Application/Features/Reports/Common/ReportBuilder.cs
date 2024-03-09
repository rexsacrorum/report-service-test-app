using System.IO;
using System.Text;
using System.Threading.Tasks;
using ReportService.Application.DTOs;
using ReportService.Application.Interfaces;

namespace ReportService.Application.Features.Reports.Common;

public abstract class ReportBuilder<T> : IReportBuilder<T>
{
    protected T Report { get; set; }
    protected StringBuilder ReportContent { get; set; }
    
    public abstract IReportBuilder<T> Build();

    public IReportBuilder<T> Init(T reportData)
    {
        Report = reportData;
        ReportContent = new StringBuilder();
        
        return this;
    }

    public async Task<ReportDto> ExportToTxtAsync(string reportName)
    {
        var stream = new MemoryStream(); // Get output stream from somewhere.
        using (var streamWriter = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true))
        {
            foreach (var chunk in ReportContent.GetChunks()) 
                streamWriter.Write(chunk);
        }
        stream.Position = 0;
        
        var fileName = reportName + ".txt";
        return new ReportDto(fileName, stream);
    }
}
