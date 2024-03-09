using RestClients.Interfaces;

namespace RestClients;

public class HrClient : RestClient, IHrClient
{
    public HrClient(HttpClient httpClient) 
        : base(httpClient)
    {}

    public async ValueTask<string> GetEmployeeCodeAsync(string inn)
    {
        return await GetAsync<string>("api/inn/" + inn);
    }
}