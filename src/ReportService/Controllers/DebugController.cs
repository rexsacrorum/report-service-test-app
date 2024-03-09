using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReportService.Application.Attributes;

namespace ReportService.Controllers;

[Route("api/[controller]")]
[Environment("Development")]
public class DebugController : ControllerBase
{
    private ILogger<DebugController> Logger { get; }
    
    public DebugController(ILogger<DebugController> logger)
    {
        Logger = logger;
    }

    [HttpGet("api/inn/{inn}")]
    public IActionResult GetEmployeeCode(string inn)
    {
        Logger.LogInformation($"GetEmployeeCode: {inn}");
        return Ok($"EmployeeCode-{inn}");
    }
        
    [HttpGet("api/empcode/{employeeCode}")]
    public IActionResult GetEmployeeSalary(string employeeCode)
    {
        Logger.LogInformation($"GetEmployeeSalary: {employeeCode}");
        var random = new Random();
            
        return Ok(random.Next(70000, 150000));
    }
}