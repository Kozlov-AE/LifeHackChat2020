using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.DialogHandler.Model
{
    public class DialogStorage : IDialogStorage
    {
        protected List<ClientRequest> requests = new List<ClientRequest>();
        protected string pathToBase = "base.txt";

        public DialogStorage(string pathToBase)
        {
            this.pathToBase = pathToBase;
        }

        public async Task SaveBase()
        {
            using (FileStream fs = File.Create(pathToBase))
            {
                await JsonSerializer.SerializeAsync(
                    fs, requests, 
                    new JsonSerializerOptions { WriteIndented = true });
            }
        }

        public DialogStorage()
        {
        }

        public void AddRequest(ClientRequest request)
        {
            requests.Add(request);
        }

        public void RemoveRequest(int id)
        {
            if(this[id] is ClientRequest req)
                requests.Remove(req);
        }

        public ClientRequest this[int id] => requests.FirstOrDefault(r => r.Id == id);

        public IReadOnlyCollection<ClientRequest> GetAllRequests() => requests;

        public virtual async Task LoadBase()
        {
            if (!File.Exists(pathToBase))
            {
                File.Create(pathToBase);
            }
            using (FileStream fs = File.OpenRead(pathToBase))
            {
                requests = await JsonSerializer.DeserializeAsync<List<ClientRequest>>(fs);
            }
        }
    }
}
