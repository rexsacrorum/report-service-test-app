using System.ComponentModel.DataAnnotations;

namespace ReportService.Application;

public class StartupOptions
{
    public const string SectionName = nameof(StartupOptions);
    
    [Required]
    public string PostgresConnectionString { get; set; }
    
    [Required]
    public string HrApiUrl { get; set; }
    
    [Required]
    public string AccountingApiUrl { get; set; }
}