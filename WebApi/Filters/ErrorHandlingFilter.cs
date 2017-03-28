using System.Diagnostics;
using System.Web.Mvc;

namespace PnIotPoc.WebApi.Filters
{
    public class ErrorHandlingFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            Trace.TraceError("Unhandled Exception : {0}", exception.Message);
        }
    }
}