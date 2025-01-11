using NatureHelp.Exceptions.Handlers;
using NatureHelp.Interfaces;

namespace NatureHelp.Providers
{
    public class ErrorHandlersProvider : IObjectsProvider<IExceptionHandler>
    {
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
