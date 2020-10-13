using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server.Model
{
    public class ServerModel
    {
        public event Action<string> OnStarted;
        public event Action<string> OnClientGetsMessage;
        public event Action<string> OnException;


        static TcpListener tcpListener;
        Dictionary<string, ClientModel> clients = new Dictionary<string, ClientModel>();

        protected internal void AddConnection(ClientModel clientModel)
        {
            clients.Add(clientModel.Id, clientModel);
        }

        protected internal void RemoveConnection(string id)
        {
            ClientModel client = clients[id];
            if (client != null)
                clients.Remove(id);
        }

        // прослушивание входящих подключений
        protected internal void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8888);
                tcpListener.Start();

                //Console.WriteLine("Сервер запущен. Ожидание подключений...");
                OnStarted?.Invoke("Сервер запущен, ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientModel clientModel = new ClientModel(tcpClient, this);
                    clientModel.OnGetMessage += ClientGetsMessage;
                    Thread clientThread = new Thread(new ThreadStart(clientModel.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                OnException?.Invoke(ex.Message);
                Disconnect();
            }
        }

        /// <summary>Получение клиентом сообщения</summary>
        /// <param name="sender">Отправитель</param>
        /// <param name="message">Сообщение</param>
        private void ClientGetsMessage(object sender, string message)
        {
            string finalmessage = string.Concat($"Клиент {(sender as ClientModel).UserName}: {message}");
            OnClientGetsMessage?.Invoke(finalmessage);
        }

        // трансляция сообщения всем подключенным клиентам, кроме отправителя.
        protected internal void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            foreach (var c in clients)
            {
                if (c.Key != id) c.Value.Stream.Write(data, 0, data.Length);
            }
        }

        /// <summary>Отправить одно текстовое сообщение одному клиенту</summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="id"><see langword="Id"/> клиента получателя</param>
        protected internal void SendMessage (string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            clients[id].Stream.Write(data, 0, data.Length);
        }

        // отключение всех клиентов
        protected internal void Disconnect()
        {
            tcpListener.Stop();
            foreach (var c in clients)
            {
                c.Value.Close();
                RemoveConnection(c.Key);
            }
            Environment.Exit(0);
        }
    }
}
