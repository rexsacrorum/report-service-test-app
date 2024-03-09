using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using ReportService.Application.Features.Reports.SalaryReport;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private IMediator Mediator { get; }
        private IContentTypeProvider ContentTypeProvider { get; }
        
        public ReportsController(IMediator mediator, IContentTypeProvider contentTypeProvider)
        {
            Mediator = mediator;
            ContentTypeProvider = contentTypeProvider;
        }
        
        [HttpGet]
        [Route("{year}/{month}")]
        public async Task<IActionResult> Download(int year, int month, CancellationToken cancellationToken)
        {
            var report = await Mediator.Send(new GetSalaryReport(year, month), cancellationToken);
            
            var contentType = ContentTypeProvider.TryGetContentType(report.FileName, out var type)
                ? type
                : "application/octet-stream";
            
            return File(report.File, contentType, report.FileName);
        }
    }
}
