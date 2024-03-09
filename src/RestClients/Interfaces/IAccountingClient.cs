namespace RestClients.Interfaces;

public interface IAccountingClient
{
    /// <summary>
    /// Get salary by employee code.
    /// </summary>
    /// <param name="employeeCode">Employee code.</param>
    /// <returns>Salary.</returns>
    ValueTask<decimal> GetSalaryAsync(string employeeCode);
}