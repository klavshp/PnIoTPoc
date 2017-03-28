using System;

namespace PnIotPoc.WebApi.Models
{
    /// <summary>
    /// Wraps error details to pass back to the caller of a WebAPI
    /// </summary>
    [Serializable()]
    public class Error
    {
        public enum ErrorType
        {
            Exception = 0,
            Validation = 1
        }

        public ErrorType Type { get; set; }
        public string Message { get; set; }

        public Error(Exception exception)
        {
            Type = ErrorType.Exception;
            Message = "An unexpected error occurred.";
        }

        public Error(string validationError)
        {
            Type = ErrorType.Validation;
            Message = validationError;
        }
    }
}