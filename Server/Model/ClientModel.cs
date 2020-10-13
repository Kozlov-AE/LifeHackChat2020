using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;

namespace Server.Model
{
    public class ClientModel
    {
        public event Action<string> ?OnConnected;
        public event Action<object, string> ?OnGetMessage;

        public string Id { get; private set; }
        public NetworkStream ?Stream { get; private set; }
        public string ?UserName { get; private set; }
        TcpClient client;
        ServerModel server;

        public ClientModel(TcpClient tcpClient, ServerModel serverModel)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            server = serverModel;
            serverModel.connections.AddConnection(this);
        }

        public void Process()
        {
            try
            {
                Stream = client.GetStream();
                // Запрос имени пользователя
                server.SendMessage("Введите ваше имя", Id);
                string message = GetMessage();
                UserName = message;

                // Событие подключения клиента к чату
                OnConnected?.Invoke($"Клиент с именем \"{UserName}\" подключился к чату");
                // в бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        OnGetMessage(this, message);
                    }
                    catch
                    {
                        message = String.Format("{0}: покинул чат", UserName);
                        Console.WriteLine(message);
                        server.BroadcastMessage(message, this.Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.connections.CloseAndRemoveAll();
                Close();
            }
        }

        // чтение входящего сообщения и преобразование в строку
        private string GetMessage()
        {
            byte[] data = new byte[64]; // буфер для получаемых данных
            StringBuilder builder = new StringBuilder();
            int bytes = 0;
            do
            {
                bytes = Stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (Stream.DataAvailable);

            return builder.ToString();
        }

        // закрытие подключения
        protected internal void Close()
        {
                Stream?.Close();
                client?.Close();
        }
    }
}
