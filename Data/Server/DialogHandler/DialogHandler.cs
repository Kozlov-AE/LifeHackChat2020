﻿using Logic.Server.ClientCommandHandler;
using Logic.Server.DialogHandler.Model;
using Logic.Server.Model;
using Logic.Server.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Server.DialogHandler
{
    /// <summary>Класс управляющий диалогами (не командами)</summary>
    public class DialogHandler : IDialogHandler
    {
        protected IDialogStorage dialogStorage;

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
            
            return dialogStorage.GetAllRequests()
                ?.FirstOrDefault(r => r.Text.ToLower() == message.ToLower())
                ?.GetRandomAnswer()?.Text ?? null;
        }

        public async Task LoadData()
        {
            await dialogStorage.LoadBase();
        }

        public IReadOnlyCollection<string> AllQuestions()
        {
            return dialogStorage?.GetAllRequests()?.Select(r => r.Text)?.ToList();
        }
    }
}
