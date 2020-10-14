using System;
using System.Collections.Generic;
using System.Text;

namespace Server.DialogHandler.Model
{
    public class ServerAnswer
    {
        //Для разнообразия
        static int globalId;
        static ServerAnswer()
        {
            globalId = 1;
        }

        public int Id { get; private set; }
        public int ClientRequestId { get; set; }
        public string? Text { get; set; }

        public ServerAnswer(int requestId, string answer)
        {
            Id = NexId();
            ClientRequestId = requestId;
            Text = answer;
        }

        static int NexId() => ++globalId;
    }
}
