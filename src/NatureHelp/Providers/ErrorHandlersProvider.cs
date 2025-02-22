using NatureHelp.Exceptions.Handlers;
using NatureHelp.Interfaces;

namespace NatureHelp.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorHandlersProvider : IObjectsProvider<IExceptionHandler>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<IExceptionHandler> GetObjects()
        {
            return new List<IExceptionHandler>
            {
                new CommonExceptionsHandler(),
                new SerializationExceptionsHandler(),
                new SecurityExceptionsHandler(),
                new ThreadingExceptionsHandler(),
                new NetworkExceptionsHandler(),
                new FileExceptionsHandler(),
                new DataAccessExceptionsHandler(),
                new SystemExceptionsHandler(),
            };
        }
    }
}
