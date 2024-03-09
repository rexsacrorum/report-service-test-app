using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReportService.Domain;

namespace ReportService.Application.Interfaces;

public interface IDepartmentRepository
{
    Task<List<Department>> GetDepartmentsAsync(bool isActive, CancellationToken cancellationToken);
}