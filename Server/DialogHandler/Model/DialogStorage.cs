using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.DialogHandler.Model
{
    public class DialogStorage : IDialogStorage
    {
        readonly List<ClientRequest> requests = new List<ClientRequest>();

        public DialogStorage(string PathToBase)
        {
            this.requests = requests;
        }

        public DialogStorage()
        {
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

        public void AddRequest(ClientRequest request)
        {
            requests.Add(request);
        }

        public void RemoveRequest(int id)
        {
            var req = this[id];
            if (req != null)
                requests.Remove(req);
        }

        public ClientRequest this[int id] => requests.FirstOrDefault(r => r.Id == id);

        public IReadOnlyCollection<ClientRequest> GetAllRequests() => requests;
    }
}
