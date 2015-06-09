using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RainIt.Domain.DTO
{
    public class StatusMessage
    {
        
        public bool IsError { get; set; }
        public string Message { get; set; }

        public static StatusMessage WriteError(string errorMessage)
        {
            return new StatusMessage()
            {
                IsError = true,
                Message = errorMessage
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
