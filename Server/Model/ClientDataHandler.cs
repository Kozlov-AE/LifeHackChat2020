using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Model
{
    public class ClientDataHandler
    {
        public string? Id { get; private set; }
        public string? UserName { get; private set; }
        public ClientGroups Group { get; private set; }

        public ClientDataHandler(string id, string userName, ClientGroups group)
        {
            Id = id;
            UserName = userName;
            Group = group;
        }

        public ClientDataHandler(ClientModel client)
        {
            Id = client.Id;
            UserName = client.UserName;
            Group = client.Group;
        }
    }
}
