using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ReportService.Application.Attributes;

/// <summary>
/// This attribute is used to restrict access to the action based on the environment name.
/// </summary>
/// <remarks>
/// If the current environment name does not match the specified one, the action will return 404.
/// </remarks>
public class EnvironmentAttribute : ActionFilterAttribute
{
    private readonly string _environmentName;

    public EnvironmentAttribute(string environmentName)
    {
        _environmentName = environmentName;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var env = (IHostEnvironment)context.HttpContext.RequestServices.GetRequiredService(typeof(IHostEnvironment));
        
        if (env.EnvironmentName != _environmentName) 
            context.Result = new NotFoundResult();
    }
}