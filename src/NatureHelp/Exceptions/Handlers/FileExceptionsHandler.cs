using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NatureHelp.Interfaces;
using Shared.Dtos;
using System.Net;

namespace NatureHelp.Exceptions.Handlers;

/// <summary>
/// 
/// </summary>
public class FileExceptionsHandler : IExceptionHandler
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
        exceptionContext.Result = new ObjectResult(new ErrorResponseDto
        {
            Message = exceptionContext.Exception.Message,
            StatusCode = (int)HttpStatusCode.InternalServerError,
            ErrorType = exceptionContext.Exception.GetType().Name
        })
        {
            StatusCode = (int)HttpStatusCode.InternalServerError
        };

        exceptionContext.ExceptionHandled = true;
    }
}