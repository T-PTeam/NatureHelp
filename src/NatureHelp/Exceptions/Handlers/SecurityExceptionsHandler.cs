using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NatureHelp.Interfaces;
using System.Net;

namespace NatureHelp.Exceptions.Handlers;

/// <summary>
/// 
/// </summary>
public class SecurityExceptionsHandler : IExceptionHandler
{
    /// <inheritdoc/>
    public bool CanHandle(ExceptionContext exceptionContext)
    {
        return exceptionContext.Exception is not null;
    }

    /// <inheritdoc/>
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