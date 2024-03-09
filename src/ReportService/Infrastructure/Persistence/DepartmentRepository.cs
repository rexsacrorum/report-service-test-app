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

public class DepartmentRepository : IDepartmentRepository
{
    private Func<DbConnection> ConnectionFactory { get; }

    public DepartmentRepository(Func<DbConnection> connectionFactory)
    {
        ConnectionFactory = connectionFactory;
    }

    public async Task<List<Department>> GetDepartmentsAsync(bool isActive, CancellationToken cancellationToken)
    {
        await using var connection = ConnectionFactory();
        
        var sql = "SELECT id, name FROM public.deps WHERE active = @IsActive";
        var command = new CommandDefinition(sql, new { IsActive = isActive }, cancellationToken: cancellationToken);
        var departments = await connection.QueryAsync<Department>(command);

        return departments.ToList();
    }
}
