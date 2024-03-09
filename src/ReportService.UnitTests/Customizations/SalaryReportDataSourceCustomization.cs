using AutoFixture;
using Moq;
using ReportService.Application.Interfaces;

namespace ReportService.UnitTests.Customizations;

public class SalaryReportDataSourceCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        // By default AutoFixture creates a canceled token.
        fixture.Register(() => CancellationToken.None);
        
        fixture.Freeze<Mock<IMonthNameResolver>>();
        fixture.Freeze<Mock<IEmployeesRepository>>();
        fixture.Freeze<Mock<IDepartmentRepository>>();
        fixture.Freeze<Mock<IAccountingApi>>();
    }
}
