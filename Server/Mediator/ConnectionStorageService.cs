using Server.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server.Mediator
{
    public class ConnectionStorageService : IConnectionStorage
    {
        readonly Dictionary<string, ClientModel>? clients = new Dictionary<string, ClientModel>();

        public ClientModel this[string id]
        {
            get
            {
                return clients[id];
            }
        }

        public void AddConnection(ClientModel clientModel)
        {
            clients.Add(clientModel.Id, clientModel);
        }

        public void CloseAndRemoveAll()
        {
            foreach (var c in clients)
            {
                c.Value.Close();
                RemoveConnection(c.Key);
            }
        }

        public IReadOnlyCollection<ClientModel> GetAllExceptId(string id)
        {
            return clients.Where(c => c.Key != id).Select(c => c.Value).ToList();
        }

        public void RemoveConnection(string id)
        {
            clients.Remove(id);
        }
    }
}
