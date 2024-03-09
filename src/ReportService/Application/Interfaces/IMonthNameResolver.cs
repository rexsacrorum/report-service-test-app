namespace ReportService.Application.Interfaces;

public interface IMonthNameResolver
{
    /// <summary>
    /// Get month name by number.
    /// </summary>
    public string GetMonthName(int month);
}