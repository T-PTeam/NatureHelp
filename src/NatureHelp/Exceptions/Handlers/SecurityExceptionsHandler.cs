using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NatureHelp.Interfaces;
using Shared.Dtos;
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
        exceptionContext.Result = new ObjectResult(new ErrorResponseDto
        {
            Message = exceptionContext.Exception.Message,
            StatusCode = (int)HttpStatusCode.Unauthorized,
            ErrorType = exceptionContext.Exception.GetType().Name
        })
        {
            StatusCode = (int)HttpStatusCode.Unauthorized
        };

        exceptionContext.ExceptionHandled = true;
    }
}