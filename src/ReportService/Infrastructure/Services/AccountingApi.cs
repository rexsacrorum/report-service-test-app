using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ReportService.Application.Interfaces;
using RestClients.Interfaces;

namespace ReportService.Infrastructure.Services;

public class AccountingApi : IAccountingApi
{
    private IAccountingClient AccountingClient { get; }
    private IHrClient HrClient { get; }
    private ICacheService<AccountingApi> Cache { get; }

    public AccountingApi(IAccountingClient accountingClient, ICacheService<AccountingApi> cache, IHrClient hrClient)
    {
        AccountingClient = accountingClient;
        Cache = cache;
        HrClient = hrClient;
    }

    public async ValueTask<decimal> GetSalaryByInnAsync(string inn)
    {
        if (Cache.TryGet(inn, out decimal salary))
            return salary;

        var employeeCode = await HrClient.GetEmployeeCodeAsync(inn);
        salary = await AccountingClient.GetSalaryAsync(employeeCode);
        Cache.Set(inn, salary);

        return salary;
    }

    public async Task<IDictionary<string, decimal>> GetSalariesByInnsAsync(IEnumerable<string> inns,
        CancellationToken cancellationToken)
    {
        var ctsSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var cts = ctsSource.Token;
        
        // 100k inns
        // ~250MS for hot cache
        // For cold cache:
        // 19.19s for 1 thread simple foreach loop
        // 12.64 for 2 threads 
        // 8.39 for 4 threads
        // 6.99 for 6 threads
        // 6.34s for 8 threads. Optimal for 8 cores CPU
        // 6.12s for 10 threads
        var semaphore = new SemaphoreSlim(8, 8); // Adjust this number to change the level of parallelism
        var tasks = inns.Select(async inn =>
        {
            await semaphore.WaitAsync(cts);
            try
            {
                var salary = await GetSalaryByInnAsync(inn);
                return new { inn, salary };
            }
            catch
            {
                // Cancel all other tasks if one of them failed
                await ctsSource.CancelAsync();
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        });
        
        var pairs = await Task.WhenAll(tasks);
        
        return pairs.ToDictionary(pair => pair.inn, pair => pair.salary);
    }
}
