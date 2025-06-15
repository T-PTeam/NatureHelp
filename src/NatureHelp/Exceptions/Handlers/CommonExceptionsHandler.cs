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
        bool needSeeInnerException = exceptionContext.Exception.Message == "An error occurred while saving the entity changes. See the inner exception for details.";

        exceptionContext.Result = new ContentResult
        {
            Content = "Common error: " + (needSeeInnerException && exceptionContext.Exception.InnerException != null
                ? exceptionContext.Exception.InnerException.Message
                : exceptionContext.Exception.Message),
            ContentType = "text/plain",
            StatusCode = (int)HttpStatusCode.BadRequest
        };

        exceptionContext.ExceptionHandled = true;
    }
}