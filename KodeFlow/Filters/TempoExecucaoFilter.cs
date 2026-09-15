using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace KodeFlow.Filters
{
    public class TempoExecucaoFilter : IAsyncActionFilter
    {
        private readonly ILogger<TempoExecucaoFilter> _logger;

        public TempoExecucaoFilter(ILogger<TempoExecucaoFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Código: antes que a action executa
            var actioName = context.ActionDescriptor.DisplayName;
            var stopWatch = Stopwatch.StartNew();

            _logger.LogInformation("#### [Executando -> OnActionExecutionAsync (ANTES)]");
            _logger.LogInformation("########################################################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation("#### [INICIO] {action}",actioName);
            _logger.LogInformation("########################################################################");

            await next();
            //Código: depois que a action executa

            stopWatch.Stop();

            _logger.LogInformation("#### Executando -> OnActionExecutionAsync (DEPOIS)");
            _logger.LogInformation("########################################################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation("### [FIM] {action}, levou: {tempo}ms",actioName, stopWatch.ElapsedMilliseconds);
            _logger.LogInformation("########################################################################");
        }
    }
}
