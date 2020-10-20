using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace Logic.Server.DialogHandler.Model
{
    public class DialogStorage : IDialogStorage
    {
        protected List<ClientRequest> requests = new List<ClientRequest>();
        protected string pathToBase = "base.json";

        public DialogStorage(string pathToBase)
        {
            this.pathToBase = pathToBase;
            LoadBase().Wait();
        }

        public virtual void SaveBase()
        {
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize<List<ClientRequest>>(requests, options);
            File.WriteAllText(pathToBase, json);
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
            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
                WriteIndented = true
            };
            var jsonString = File.ReadAllText(pathToBase);
            requests = JsonSerializer.Deserialize<List<ClientRequest>>(jsonString, options);
        }
    }
}
