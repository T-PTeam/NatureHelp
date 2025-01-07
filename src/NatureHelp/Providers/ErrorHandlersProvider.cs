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
                new GeneralInternalExceptionHandler(),
            };
        }
    }
}
