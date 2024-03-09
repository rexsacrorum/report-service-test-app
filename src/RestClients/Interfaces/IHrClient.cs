namespace RestClients.Interfaces;

public interface IHrClient
{
    /// <summary>
    /// Get employee code by INN.
    /// </summary>
    /// <param name="inn">Employee INN.</param>
    /// <returns>Employee code.</returns>
    public ValueTask<string> GetEmployeeCodeAsync(string inn);
}