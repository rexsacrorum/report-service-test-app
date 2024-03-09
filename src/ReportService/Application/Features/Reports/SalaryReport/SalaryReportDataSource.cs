using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReportService.Application.Interfaces;

namespace ReportService.Application.Features.Reports.SalaryReport;

/// <summary>
/// Salary report data source.
/// </summary>
public class SalaryReportDataSource : ISalaryReportDataSource
{
    private IMonthNameResolver MonthResolver { get; }
    private IDepartmentRepository DepartmentRepository { get; }
    private IEmployeesRepository EmployeesRepository { get; }
    private IAccountingApi AccountingApi { get; }
    
    public SalaryReportDataSource(IMonthNameResolver monthResolver,
        IEmployeesRepository employeesRepository,
        IAccountingApi accountingApi, IDepartmentRepository departmentRepository)
    {
        MonthResolver = monthResolver;
        EmployeesRepository = employeesRepository;
        AccountingApi = accountingApi;
        DepartmentRepository = departmentRepository;
    }


    public async Task<SalaryReportData> GetDataAsync(GetSalaryReport query, CancellationToken cancellationToken)
    {
        var report = new SalaryReportData
        {
            Month = MonthResolver.GetMonthName(query.Month),
            Year = query.Year
        };
        
        // Get active departments
        var departments = await DepartmentRepository.GetDepartmentsAsync(true, cancellationToken);
        report.Departments = new List<SalaryReportDepartment>(departments.Count);

        foreach (var department in departments)
        {
            var employees = await EmployeesRepository.GetDepartmentEmployeesAsync(department.Id, cancellationToken);
            if (employees.Count == 0)
                continue;
            
            var inns = employees.Select(s => s.Inn);
            var salaryByInn = await AccountingApi.GetSalariesByInnsAsync(inns, cancellationToken);
            
            var departmentData = new SalaryReportDepartment
            {
                DepartmentName = department.Name,
                Employees = employees.Select(e => new SalaryReportEmployee
                {
                    EmployeeName = e.Name,
                    Inn = e.Inn,
                    Salary = salaryByInn[e.Inn]
                }).ToList()
            };
            departmentData.Total = departmentData.Employees.Sum(e => e.Salary);
            report.Departments.Add(departmentData);
        }
        
        report.Total = report.Departments.Sum(d => d.Total);
        
        return report;
    }
    
}
