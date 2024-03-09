using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using ReportService.Application.Interfaces;
using ReportService.Domain;

namespace ReportService.Infrastructure.Persistence;

public class EmployeesRepository : IEmployeesRepository
{
    private Func<DbConnection> ConnectionFactory { get; }

    public EmployeesRepository(Func<DbConnection> connectionFactory)
    {
        ConnectionFactory = connectionFactory;
    }

    public async Task<List<Employee>> GetDepartmentEmployeesAsync(int departmentId, CancellationToken cancellationToken)
    {
        await using var connection = ConnectionFactory();
        
        var sql = "SELECT name, inn FROM public.emps WHERE deps_id = @DepartmentId";
        var command = new CommandDefinition(sql, new { DepartmentId = departmentId }, cancellationToken: cancellationToken);
        var employees = await connection.QueryAsync<Employee>(command);

        return employees.ToList();
    }
}
