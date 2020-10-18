using Server.ClientCommandHandler;
using Server.Mediator;
using Server.Parameters;
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
        public event Action<ClientDataHandler>? OnClientDisconected;
        public event Action<ClientDataHandler>? OnClientConnected;
        public event Action<ClientModel>? OnClientCreated;

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

                    ClientModel.OnCreated += connections.AddConnection;
                    ClientModel.OnCreated += OnClientCreated;
                    ClientModel clientModel = new ClientModel(tcpClient);

                    clientModel.OnConnected += CheckConnectionName;
                    clientModel.OnGetMessage += OnClientGetsMessage;
                    clientModel.OnDisconected += OnClientDisconected;
                    clientModel.OnDisconected += (c) => connections.RemoveConnection(c.Id);

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

        /// <summary>Если соединение с таким именем есть, то убиваем его.</summary>
        public void CheckConnectionName(ClientDataHandler handler)
        {
            var c = connections.FirstOrDefault(c => c.UserName == handler.UserName && c.Id != handler.Id);
            if (c != null)
            {
                OnException?.Invoke($"Клиент с именем {handler.UserName} уже подключен. Убиваем нового!");
                SendMessageToClient("Клиент с таким именем уже подключен. Переподключитесь с другим именем!", handler.Id);
                connections[handler.Id].Close();
                connections.RemoveConnection(handler.Id);
            }
            else
            {
                var message = handler.Group == ClientGroups.admin ?
                "Вы админ! Для получения списка команд, введите \" /help\", для выхода введите \"/Пока (да да, по русски)\"" :
                "Вы юзер! Для получения списка команд, введите \" /help\", для выхода введите \"/Пока (да да, по русски)\"";
                SendMessageToClient(message, handler.Id);
                OnClientConnected?.Invoke(handler);
            }
        }

        public void CloseUserConnection(string userId)
        {
            SendMessageToClient("Досвидания", userId);
            connections[userId].Close();
            connections.RemoveConnection(userId);
        }

        public void CloseUserConnectionByName(string userName)
        {
            var c = connections.FirstOrDefault(c => c.UserName == userName);
            CloseUserConnection(c?.Id);
        }

        public List<string> GetClientsNamesList()
        {
            return connections.GetAllClients().Select(c => c.UserName).ToList();
        }



        // Удалить!
        public void Disconnect()
        {
            //Перестаем слушать порт
            tcpListener.Stop();
            //Удаляем все подключения
            connections.CloseAndRemoveAll();
            //Закрываем приложение (в core нужно нажать энтер)
            Environment.Exit(0);
        }

        public void Dispose()
        {
            //Перестаем слушать порт
            tcpListener.Stop();
            //Удаляем все подключения
            connections.CloseAndRemoveAll();
            //Закрываем приложение (в core нужно нажать энтер)
            Environment.Exit(0);
        }
    }
}
