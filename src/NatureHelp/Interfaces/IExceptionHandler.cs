using Microsoft.AspNetCore.Mvc.Filters;

namespace NatureHelp.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExceptionHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionContext"></param>
        /// <returns></returns>
        bool CanHandle(ExceptionContext exceptionContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionContext"></param>
        void Handle(ExceptionContext exceptionContext);
    }
}
