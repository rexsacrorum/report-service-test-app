using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using ReportService.UnitTests.Customizations;

namespace ReportService.UnitTests.Attributes;

public class SalaryReportDataSourceAttribute : AutoDataAttribute
{
    public SalaryReportDataSourceAttribute()
        : base(() => new Fixture()
            .Customize(new AutoMoqCustomization())
            .Customize(new SalaryReportDataSourceCustomization()))
    { }
}