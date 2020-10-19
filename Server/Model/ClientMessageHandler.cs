using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model
{
    /// <summary>Хранитель сообщения и сопутствующих параметров</summary>
    public class ClientMessageHandler
    {
        /// <summary>Время</summary>
        public DateTime Time { get;}
        /// <summary>Текст сообщения</summary>
        public string? Message { get;}
        /// <summary>Объект клиента, приславшего сообщение</summary>
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
