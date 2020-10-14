using System;
using System.Collections.Generic;
using System.Text;

namespace Server.DialogHandler.Model
{
    class ServerAnswer
    {
        public Guid Id { get; private set; }
        public Guid ClientRequestId { get; set; }
        public string? Text { get; set; }
    }
}
