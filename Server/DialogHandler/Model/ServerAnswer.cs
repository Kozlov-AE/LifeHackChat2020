using System;
using System.Collections.Generic;
using System.Text;

namespace Server.DialogHandler.Model
{
    public class ServerAnswer
    {
        //Для разнообразия без GUID
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
