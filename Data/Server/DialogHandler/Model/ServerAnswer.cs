using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Server.DialogHandler.Model
{
    /// <summary>Ответ сервена на запрос клиента</summary>
    public class ServerAnswer
    {
        /// <summary>Статик int для формирования Id</summary>
        static int globalId;
        static ServerAnswer()
        {
            globalId = 0;
        }

        public int Id { get; set; }
        public int ClientRequestId { get; set; }
        public string? Text { get; set; }

        public ServerAnswer()
        { }
        public ServerAnswer(int requestId, string answer)
        {
            Id = NexId();
            ClientRequestId = requestId;
            Text = answer;
        }

        static int NexId()
        {
            return ++globalId;
        }
    }
}
