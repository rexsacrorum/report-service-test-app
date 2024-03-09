using System.IO;

namespace ReportService.Application.DTOs;

public record ReportDto(string FileName, Stream File);