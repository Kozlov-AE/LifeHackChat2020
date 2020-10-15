using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;

namespace Server.Model
{
    public class ClientModel
    {
        static public event Action<ClientModel>? OnCreated;
        public event Action<ClientDataHandler>? OnDisconected;
        public event Action<ClientDataHandler>? OnConnected;
        public event Action<ClientMessageHandler>? OnGetMessage;
        public event Action<string>? OnException;

        public string? Id { get; private set; }
        public NetworkStream ?Stream { get; private set; }
        public string? UserName { get; private set; }
        TcpClient client;
        public ClientGroups Group { get; private set; }

        // Создаем новое подключение к клиентскому приложению
        public ClientModel(TcpClient tcpClient)
        {
            Id = Guid.NewGuid().ToString();
            client = tcpClient;
            OnCreated?.Invoke(this);
        }

        /// <summary>Начинаем слушать порт на предмет новых сообщений для клиента, в новом потоке</summary>
        public void Process()
        {
            try
            {
                Stream = client.GetStream();

                // Запрос имени пользователя
                SendMessage("Введите ваше имя");
                string message = GetMessage();
                UserName = message;
                if (UserName == "God")
                {
                    SendMessage("Ваш пароль администратора");
                    if (GetMessage() == "666")
                    {
                        Group = ClientGroups.admin;
                    }
                    else
                    {
                        Group = ClientGroups.user;
                        UserName += " -user";
                    }
                }
                // Событие подключения клиента к чату
                OnConnected?.Invoke(new ClientDataHandler(this));

                // В бесконечном цикле получаем сообщения от клиента
                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        OnGetMessage?.Invoke(new ClientMessageHandler(message, this));
                    }
                    catch
                    {
                        OnDisconected?.Invoke(new ClientDataHandler(this));
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
                OnDisconected?.Invoke(new ClientDataHandler(this));
                Close();
            }
        }

        /// <summary>Чтение входящего сообщения и преобразование его в строку</summary>
        /// <returns>Строку с текстовым сообщением</returns>
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
        public void Close()
        {
                Stream?.Close();
                client?.Close();
        }

        ~ClientModel()
        {
            Close();
        }
    }
}
