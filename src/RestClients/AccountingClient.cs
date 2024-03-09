using RestClients.Interfaces;

namespace RestClients;

public class AccountingClient : RestClient, IAccountingClient
{
    public AccountingClient(HttpClient httpClient) 
        : base(httpClient)
    { }
    
    public async ValueTask<decimal> GetSalaryAsync(string employeeCode)
    {
        return await GetAsync<decimal>("api/empcode/" + employeeCode);
    }
}