using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model
{
    public class ClientMessageHandler
    {
        public DateTime Time { get;}
        public string? Message { get;}
        public ClientModel? Client { get; }

        public ClientMessageHandler(string? message, ClientModel? client)
        {
            Time = DateTime.Now;
            Client = client;
            Message = message;
        }

        public override string ToString() => $"{Time.ToShortTimeString()} {Client.UserName}: {Message}";
    }
}
