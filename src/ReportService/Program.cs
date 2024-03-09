using System;
using System.Data.Common;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Npgsql;
using ReportService.Application;
using ReportService.Application.Features.Reports.SalaryReport;
using ReportService.Application.Interfaces;
using ReportService.Application.Middlewares;
using ReportService.Application.Services;
using ReportService.Infrastructure.Persistence;
using ReportService.Infrastructure.Services;
using RestClients;
using RestClients.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

// Add options.
services.AddOptions<StartupOptions>()
    .BindConfiguration(StartupOptions.SectionName)
    .ValidateDataAnnotations()
    .ValidateOnStart();

// Add http clients.
services.AddHttpClient<IHrClient, HrClient>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<StartupOptions>>().Value;
    client.BaseAddress = new Uri(options.HrApiUrl);
});
services.AddHttpClient<IAccountingClient, AccountingClient>((serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<IOptions<StartupOptions>>().Value;
    client.BaseAddress = new Uri(options.AccountingApiUrl);
});

// Add services to the container.
services
    .AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>()
    .AddSingleton<IMonthNameResolver, MonthNameResolver>()
    .AddSingleton<IMemoryCache, MemoryCache>()
    .AddSingleton(() => DateTime.UtcNow)
    .AddSingleton(typeof(ICacheService<>), typeof(CacheService<>));

services.AddTransient<IAccountingApi, AccountingApi>();

services.AddTransient<IReportBuilder<SalaryReportData>, SalaryReportBuilder>()
    .AddTransient<ISalaryReportDataSource, SalaryReportDataSource>();


// Add db connection.
services.AddTransient<DbConnection>(sp =>
{
    var options = sp.GetRequiredService<IOptions<StartupOptions>>().Value;
    return new NpgsqlConnection(options.PostgresConnectionString);
});

// Connection factory.
services.AddTransient<Func<DbConnection>>(sp => sp.GetRequiredService<DbConnection>);

// Add repositories.
services
    .AddTransient<IEmployeesRepository, EmployeesRepository>()
    .AddTransient<IDepartmentRepository, DepartmentRepository>();

// Add mediatr
services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
services.AddTransient(
    typeof(IPipelineBehavior<,>),
    typeof(ValidationBehavior<,>));

services.AddValidatorsFromAssemblyContaining(typeof(Program));

services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapControllers();

app.Run();
