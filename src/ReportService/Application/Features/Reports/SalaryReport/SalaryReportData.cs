using System.Collections.Generic;

namespace ReportService.Application.Features.Reports.SalaryReport;

public class SalaryReportData
{
    public int Year { get; set; }
    public string Month { get; set; }
    
    public decimal Total { get; set; }

    public List<SalaryReportDepartment> Departments { get; set; }
        = [];
}

public class SalaryReportDepartment
{
    public string DepartmentName { get; set; }
    
    public decimal Total { get; set; }
    
    public List<SalaryReportEmployee> Employees { get; set; }
        = [];
}

public class SalaryReportEmployee
{
    public string EmployeeName { get; set; }
    
    public string Inn { get; set; }
    
    public decimal Salary { get; set; }
}