using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.TCPConnection
{
    public class ExceptionHandler
    {
        public ExceptionHandler(string message)
        {
            Message = message;
            Date = DateTime.Now;
        }

        public string Message { get; }
        public DateTime Date { get; }
    }
}
