using System;
using System.Globalization;
using ReportService.Application.Interfaces;

namespace ReportService.Application;

public class MonthNameResolver : IMonthNameResolver
{
    public string GetMonthName(int month)
    {
        if (month is < 1 or > 12)
            throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12");
        
        return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
    }
}