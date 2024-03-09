using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ReportService.Domain;

namespace ReportService.Application.Interfaces;

public interface IEmployeesRepository
{
    /// <summary>
    /// Get employees by department id.
    /// </summary>
    /// <returns>List of employees.</returns>
    Task<List<Employee>> GetDepartmentEmployeesAsync(int departmentId, CancellationToken cancellationToken);
}
