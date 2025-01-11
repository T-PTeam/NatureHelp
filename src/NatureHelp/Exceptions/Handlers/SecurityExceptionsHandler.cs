using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using NatureHelp.Interfaces;
using System.Net;

namespace NatureHelp.Exceptions.Handlers;

public class SecurityExceptionsHandler : IExceptionHandler
{
    public bool CanHandle(ExceptionContext exceptionContext)
    {
        return exceptionContext.Exception is not null;
    }

    public void Handle(ExceptionContext exceptionContext)
    {
        exceptionContext.Result = new ContentResult
        {
            Content = "Data access error: " + exceptionContext.Exception.Message,
            ContentType = "text/plain",
            StatusCode = (int)HttpStatusCode.InternalServerError
        };

        exceptionContext.ExceptionHandled = true;
    }
}