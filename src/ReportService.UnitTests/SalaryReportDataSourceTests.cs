using AutoFixture;
using FluentAssertions;
using Moq;
using ReportService.Application.Features.Reports.SalaryReport;
using ReportService.Application.Interfaces;
using ReportService.Domain;
using ReportService.UnitTests.Attributes;

namespace ReportService.UnitTests;

public class SalaryReportDataSourceTests
{
    [Theory, SalaryReportDataSource]
    public async Task GetSalaryReportAsync_WithValidData_ShouldReturnSalaryReport(
        SalaryReportDataSource sut,
        Fixture fixture,
        GetSalaryReport query,
        CancellationToken cancellationToken,
        string monthName,
        List<Department> departments,
        
        Mock<IMonthNameResolver> monthNameResolverMock,
        Mock<IEmployeesRepository> employeesRepositoryMock,
        Mock<IDepartmentRepository> departmentRepositoryMock,
        Mock<IAccountingApi> accountingApiMock)
    {
        // 
        
        // Arrange
        monthNameResolverMock
            .Setup(x => x.GetMonthName(query.Month))
            .Returns(monthName);
        departmentRepositoryMock
            .Setup(x => x.GetDepartmentsAsync(true, cancellationToken))
            .ReturnsAsync(departments);

        var employeesByOrgId = departments
            .ToDictionary(k => k.Id, v => fixture.CreateMany<Employee>());
        employeesRepositoryMock
            .Setup(x => x.GetDepartmentEmployeesAsync(It.IsAny<int>(), cancellationToken))
            .ReturnsAsync((int departmentId, CancellationToken ct) => employeesByOrgId[departmentId].ToList());

        var salaryByInn = employeesByOrgId
            .SelectMany(s => s.Value)
            .ToDictionary(k => k.Inn, v => fixture.Create<decimal>());
        accountingApiMock.Setup(x => x.GetSalariesByInnsAsync(It.IsAny<IEnumerable<string>>(), cancellationToken))
            .ReturnsAsync(salaryByInn);

        // Act
        var report = await sut.GetDataAsync(query, cancellationToken);

        // Assert
        report.Should().NotBeNull();
        report.Month.Should().Be(monthName);
        report.Year.Should().Be(query.Year);
        
        // Verify departments
        report.Departments.Should().NotBeNullOrEmpty();
        report.Departments.Should().HaveCount(departments.Count);
        report.Departments.Select(s => s.DepartmentName)
            .Should().BeEquivalentTo(departments.Select(s => s.Name));
        
        
        // Verify employees
        report.Departments
            .ForEach(d => d.Employees.Should().NotBeNullOrEmpty());
        
        // var departmentNameById = departments.ToDictionary(k => k.Id, v => v.Name);
        var departmentIdByName = departments.ToDictionary(k => k.Name, v => v.Id);
        report.Departments.ForEach(d => d.Employees.Should().BeEquivalentTo( employeesByOrgId[departmentIdByName[d.DepartmentName]]
            .Select(s => new SalaryReportEmployee
            {
                EmployeeName = s.Name,
                Inn = s.Inn,
                Salary = salaryByInn[s.Inn]
            })));
        
        // Verify totals
        report.Total.Should().Be(report.Departments.Sum(s => s.Total));
        report.Departments
            .ForEach(d => d.Total.Should().Be(d.Employees.Sum(s => s.Salary)));
    }
    
    [Theory, SalaryReportDataSource]
    public async Task GetSalaryReportAsync_WithNoDepartments_ShouldReturnEmptyReport(
        SalaryReportDataSource sut,
        GetSalaryReport query,
        CancellationToken cancellationToken,
        string monthName,
        
        Mock<IMonthNameResolver> monthNameResolverMock,
        Mock<IDepartmentRepository> departmentRepositoryMock)
    {
        // Arrange
        monthNameResolverMock
            .Setup(x => x.GetMonthName(query.Month))
            .Returns(monthName);
        departmentRepositoryMock
            .Setup(x => x.GetDepartmentsAsync(true, cancellationToken))
            .ReturnsAsync([]);

        // Act
        var report = await sut.GetDataAsync(query, cancellationToken);

        // Assert
        report.Should().NotBeNull();
        report.Month.Should().Be(monthName);
        report.Year.Should().Be(query.Year);
        
        report.Departments.Should().BeEmpty();
        report.Total.Should().Be(0);
    }
    
    [Theory, SalaryReportDataSource]
    public async Task GetSalaryReportAsync_WithNoEmployees_ShouldReturnEmptyReport(
        SalaryReportDataSource sut,
        GetSalaryReport query,
        CancellationToken cancellationToken,
        string monthName,
        List<Department> departments,
        
        Mock<IMonthNameResolver> monthNameResolverMock,
        Mock<IEmployeesRepository> employeesRepositoryMock,
        Mock<IDepartmentRepository> departmentRepositoryMock)
    {
        // Arrange
        monthNameResolverMock
            .Setup(x => x.GetMonthName(query.Month))
            .Returns(monthName);
        departmentRepositoryMock
            .Setup(x => x.GetDepartmentsAsync(true, cancellationToken))
            .ReturnsAsync(departments);

        employeesRepositoryMock
            .Setup(x => x.GetDepartmentEmployeesAsync(It.IsAny<int>(), cancellationToken))
            .ReturnsAsync([]);

        // Act
        var report = await sut.GetDataAsync(query, cancellationToken);

        // Assert
        report.Should().NotBeNull();
        report.Month.Should().Be(monthName);
        report.Year.Should().Be(query.Year);
        
        report.Departments.Should().BeEmpty();
        report.Total.Should().Be(0);
    }

}