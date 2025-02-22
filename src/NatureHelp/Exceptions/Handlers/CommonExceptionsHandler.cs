using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NatureHelp.Interfaces;
using System.Net;

namespace NatureHelp.Exceptions.Handlers;

/// <summary>
/// 
/// </summary>
public class CommonExceptionsHandler : IExceptionHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="exceptionContext"></param>
    /// <returns></returns>
    public bool CanHandle(ExceptionContext exceptionContext)
    {
        return exceptionContext.Exception is not null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exceptionContext"></param>
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