using Server.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server.Model
{
    public class LifeChatServer: IDisposable
    {
        public event Action<string>? OnStarted;
        public event Action<string>? OnException;
        public event Action<ClientMessageHandler>? OnClientGetsMessage;
        public event Action<string>? OnClientDisconected;

        static TcpListener? tcpListener;

        public IConnectionStorage? connections;

        public LifeChatServer (IConnectionStorage connections)
        {
            this.connections = connections;
        }
        public void Listen()
        {
            try
            {
                tcpListener = new TcpListener(IPAddress.Any, 8888);
                tcpListener.Start();

                OnStarted?.Invoke("Сервер запущен, ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = tcpListener.AcceptTcpClient();

                    ClientModel clientModel = new ClientModel(tcpClient, this);
                    clientModel.OnGetMessage += ClientGetsMessage;
                    var clientTask = Task.Factory.StartNew(clientModel.Process);
                    clientTask.Wait();
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
        private void ClientGetsMessage(ClientMessageHandler handler)
        {
            OnClientGetsMessage?.Invoke(handler);
        }

        // трансляция сообщения всем подключенным клиентам, кроме отправителя.
        public void BroadcastMessage(string message, string id)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            foreach (var c in connections.GetAllExceptId(id))
            {
                c.Stream.Write(data, 0, data.Length);
            }
        }

        /// <summary>Отправить одно текстовое сообщение одному клиенту</summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="id"><see langword="Id"/> клиента получателя</param>
        public void SendMessageToClient (string message, string id)
        {
            connections[id].SendMessage(message);
        }

        // Удалить!
        public void Disconnect()
        {
            tcpListener.Stop();
            connections.CloseAndRemoveAll();
            Environment.Exit(0);
        }

        public void Dispose()
        {
            tcpListener.Stop();
            connections.CloseAndRemoveAll();
            Environment.Exit(0);
        }
    }
}
