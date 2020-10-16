using System;
using System.Collections.Generic;
using System.Text;

namespace Data.TCPConnection
{
    public class MessageHandler
    {
        public DateTime Time { get; }
        public string? Message { get; }

        public MessageHandler(string? message)
        {
            Time = DateTime.Now;
            Message = message;
        }
    }
}
