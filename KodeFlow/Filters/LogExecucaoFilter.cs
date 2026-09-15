using Microsoft.AspNetCore.Mvc.Filters;

namespace KodeFlow.Filters
{
    public class LogExecucaoFilter : IActionFilter
    {
        private readonly ILogger<LogExecucaoFilter> _logger;

        public LogExecucaoFilter(ILogger<LogExecucaoFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Executa antes da Action
            _logger.LogInformation("#### Executando -> OnActionExecuting");
            _logger.LogInformation("########################################################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
            _logger.LogInformation("########################################################################");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Executa depois da Action
            _logger.LogInformation("#### Executando -> OnActionExecuted");
            _logger.LogInformation("########################################################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"Status Code: {context.HttpContext.Response.StatusCode}");
            _logger.LogInformation("########################################################################");
        }

    }
}
