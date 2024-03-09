using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ReportService.Application.DTOs;
using ReportService.Application.Interfaces;

namespace ReportService.Application.Features.Reports.SalaryReport;

public record GetSalaryReport(int Year, int Month) : IRequest<ReportDto>;

public class GetSalaryReportValidator : AbstractValidator<GetSalaryReport>
{
    private Func<DateTime> DateTimeProvider { get; }
    
    public GetSalaryReportValidator(Func<DateTime> dateTimeProvider)
    {
        DateTimeProvider = dateTimeProvider;
        RuleFor(x => x.Year).Must(y => y >= 1990 && y <= DateTimeProvider().Year + 1)
            .WithMessage("The year must be from 1990 to the current year + 1");
        RuleFor(x => x.Month).Must(m => m is >= 1 and <= 12)
            .WithMessage("The month must be from 1 to 12");
    }
}

public class GetSalaryReportHandler : IRequestHandler<GetSalaryReport, ReportDto>
{
    private IReportBuilder<SalaryReportData> ReportBuilder { get; }
    private ISalaryReportDataSource DataSource { get; set; }
    
    public GetSalaryReportHandler(IReportBuilder<SalaryReportData> builder,
        ISalaryReportDataSource dataSource)
    {
        ReportBuilder = builder;
        DataSource = dataSource;
    }
    
    public async Task<ReportDto> Handle(GetSalaryReport request, CancellationToken cancellationToken)
    {
        var data = await DataSource.GetDataAsync(request, cancellationToken);

        return await ReportBuilder.Init(data)
            .Build()
            .ExportToTxtAsync();
    }
}
