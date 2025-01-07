using Microsoft.AspNetCore.Mvc.Filters;

namespace NatureHelp.Interfaces
{
    public interface IExceptionHandler
    {
        bool CanHandle(ExceptionContext exceptionContext);

        void Handle(ExceptionContext exceptionContext);
    }
}
