using ReportService.Application.Features.Reports.Common;
using ReportService.Application.Interfaces;

namespace ReportService.Application.Features.Reports.SalaryReport;

public class SalaryReportBuilder : ReportBuilder<SalaryReportData>
{
    private const string Delimiter = "---";
    private const string SalaryCurrency = "р";
    private const int MaxLineLength = 35;
    
    public override IReportBuilder<SalaryReportData> Build()
    {
        // Header
        ReportContent
            .AppendLine($"{Report.Month} {Report.Year}")
            .AppendLine();
        
        foreach (var department in Report.Departments)
        {
            ReportContent
                .AppendLine(Delimiter)
                .AppendLine();
            
            ReportContent.AppendLine(department.DepartmentName)
                .AppendLine();
            
            foreach (var employee in department.Employees)
            {
                var salaryLine = GetFormattedSalaryLine(employee.EmployeeName, employee.Salary);
                
                ReportContent.AppendLine(salaryLine);
            }
            
            var departmentTotalLine = GetFormattedSalaryLine("Всего по отделу", department.Total);
            
            ReportContent.AppendLine(departmentTotalLine)
                .AppendLine();
        }
        
        // Footer
        ReportContent
            .AppendLine(Delimiter)
            .AppendLine()
            .AppendLine(GetFormattedSalaryLine("Всего по предприятию", Report.Total));
        
        return this;
    }
    
    private string GetFormattedSalaryLine(string text, decimal salary)
    {
        var salaryInfo = $"{salary}{SalaryCurrency}";
        
        return text + salaryInfo.PadLeft(MaxLineLength - text.Length, ' ');
    }
}
