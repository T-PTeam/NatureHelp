namespace NatureHelp.Exceptions.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NatureHelp.Interfaces;
using System.Net;

public class CommonExceptionsHandler : IExceptionHandler
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