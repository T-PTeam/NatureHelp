using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NatureHelp.Interfaces;
using System.Net;

namespace NatureHelp.Exceptions.Handlers
{
    public class GeneralInternalExceptionHandler : IExceptionHandler
    {
        public bool CanHandle(ExceptionContext exceptionContext)
        {
            return exceptionContext.Exception is not null;
        }

        public void Handle(ExceptionContext exceptionContext)
        {
            exceptionContext.Result = new ContentResult
            {
                Content = "General Error: " + exceptionContext.Exception.Message,
                ContentType = "text/plain",
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            exceptionContext.ExceptionHandled = true;
        }
    }
}
