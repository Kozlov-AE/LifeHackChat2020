using System;
using System.Collections.Generic;
using System.Text;

namespace Server.DialogHandler
{
    public interface IDialogHandler
    {
        public string? GetAnswer(string message);
    }
}
