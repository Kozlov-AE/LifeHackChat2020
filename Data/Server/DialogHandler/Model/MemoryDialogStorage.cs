using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Server.DialogHandler.Model
{
    public class MemoryDialogStorage : DialogStorage
    {
        public override async Task LoadBase()
        {
            requests = new List<ClientRequest>();
            ClientRequest r1 = new ClientRequest("Привет", new List<ServerAnswer>());
            r1.AddAnswer("Здравствуйте.");
            r1.AddAnswer("Привет!");
            r1.AddAnswer("Доброе время суток)");
            requests.Add(r1);
            ClientRequest r2 = new ClientRequest("Как дела?", new List<ServerAnswer>());
            r2.AddAnswer("Отлично.");
            r2.AddAnswer("Не жалуюсь)");
            r2.AddAnswer("Получше ваших будут))");
            requests.Add(r2);
            ClientRequest r3 = new ClientRequest("куку", new List<ServerAnswer>());
            requests.Add(r3);
        }
    }
}
