using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model
{
    public class ClientMessageHandler
    {
        public DateTime Time { get;}
        public string? ClientId { get;}
        public string? Name { get; }
        public string? Message { get;}
        public ClientGroups Group { get; }

        public ClientMessageHandler(string id, string name, string message)
        {
            Time = DateTime.Now;
            Name = name;
            ClientId = id;
            Message = message;
        }

        public override string ToString() => $"{Time.ToShortTimeString()} {Name}: {Message}";
    }
}
