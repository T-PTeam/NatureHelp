using Microsoft.AspNetCore.Mvc.Filters;
using NatureHelp.Interfaces;

namespace NatureHelp.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class AppExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly IReadOnlyList<IExceptionHandler> _handlers;
        private readonly ILogger<AppExceptionFilterAttribute> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_provider"></param>
        /// <param name="logger"></param>
        public AppExceptionFilterAttribute(IObjectsProvider<IExceptionHandler> _provider, ILogger<AppExceptionFilterAttribute> logger)
        {
            _handlers = _provider.GetObjects();
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);

            try
            {
                _handlers.First(e => e.CanHandle(context)).Handle(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception can not be processed...");
            }
        }
    }
}
