using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ReportService.Application.Interfaces;

public interface IAccountingApi
{
    ValueTask<decimal> GetSalaryByInnAsync(string inn);
    
    /// <summary>
    /// Get salaries by INN.
    /// </summary>
    /// <param name="inns">List of INNs.</param>
    /// <returns>Dictionary with INN as key and salary as value.</returns>
    Task<IDictionary<string, decimal>> GetSalariesByInnsAsync(IEnumerable<string> inns, CancellationToken cancellationToken);
}