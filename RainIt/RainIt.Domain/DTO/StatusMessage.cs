using System;

namespace RainIt.Domain.DTO
{
    public class StatusMessage
    {
        
        public bool IsError { get; set; }
        public string Message { get; set; }
        public Exception Exception { get; set; }

        public static StatusMessage WriteError(string errorMessage)
        {
            return new StatusMessage()
            {
                IsError = true,
                Message = errorMessage
            };
        }

        public static StatusMessage WriteError(Exception exception)
        {
            return new StatusMessage()
            {
                IsError = true,
                Message = exception.Message,
                Exception = exception
            };
        }

        public static StatusMessage WriteMessage(string message)
        {
            return new StatusMessage()
            {
                IsError = false,
                Message = message
            };
        }
    }
}
