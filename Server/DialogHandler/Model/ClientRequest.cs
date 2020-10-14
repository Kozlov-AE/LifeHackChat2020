using System;
using System.Collections.Generic;
using System.Text;

namespace Server.DialogHandler.Model
{
    class ClientRequest
    {
        public Guid Id { get; private set; }
        public string? Text { get; set; }
        readonly List<ServerAnswer> answers = new List<ServerAnswer>();
    }
}
