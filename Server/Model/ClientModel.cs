using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;

namespace Server.Model
{
    public class ClientModel
    {
        public event Action<string>? OnDisconected;
        public event Action<string>? OnConnected;
        public event Action<ClientMessageHandler>? OnGetMessage;
        public event Action<string>? OnException;

        public string Id { get; private set; }
        public NetworkStream ?Stream { get; private set; }
        public string ?UserName { get; private set; }
        TcpClient client;
        LifeChatServer server;

        public ClientModel(TcpClient tcpClient, LifeChatServer serverModel)
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
                SendMessage("Введите ваше имя");
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
                        OnGetMessage?.Invoke(new ClientMessageHandler(Id, UserName, message));
                    }
                    catch
                    {
                        OnDisconected?.Invoke($"{UserName}: покинул чат");
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                OnException?.Invoke(e.Message);
            }
            finally
            {
                // в случае выхода из цикла закрываем ресурсы
                server.connections.RemoveConnection(Id);
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

        /// <summary>Отправить одно текстовое сообщение этому клиенту</summary>
        /// <param name="message">Текст сообщения</param>
        public void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            Stream.Write(data, 0, data.Length);
        }

        // закрытие подключения
        public void void Close()
        {
                Stream?.Close();
                client?.Close();
        }
    }
}
