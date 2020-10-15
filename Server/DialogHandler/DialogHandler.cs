using Server.DialogHandler.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.DialogHandler
{
    public class DialogHandler : IDialogHandler
    {
        IDialogStorage dialogStorage;

        public DialogHandler(IDialogStorage dialogStorage)
        {
            this.dialogStorage = dialogStorage;
        }

        public DialogHandler()
        {
            dialogStorage = new DialogStorage();
        }

        public string? GetAnswer(string message)
        {
            //return dialogStorage.GetAllRequests()
            //    .FirstOrDefault(r => r.Text.ToLower() == message.ToLower())
            //    ?.GetRandomAnswer().Text
            //    ?? null;
            string result = "";
            if (dialogStorage.GetAllRequests()
                .FirstOrDefault(r => r.Text.ToLower() == message.ToLower())
                is ClientRequest req)
            {
                if (req.GetRandomAnswer() is ServerAnswer sa)
                {
                    result = sa.Text;
                }
                else result = null;
            }
            else result = null;
            return result;
        }
    }
}
