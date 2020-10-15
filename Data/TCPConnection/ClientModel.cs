using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Data.TCPConnection
{
    public class ClientModel : IDisposable
    {
        #region Events

        #endregion

        string host;
        int port;
        string? userName;
        NetworkStream? stream;

        public ClientModel(string host, int port)
        {
            this.host = host;
            this.port = port;
        }

        public void Connect()
        {
            using (var client = new TcpClient(host,port))
            {
                stream = client.GetStream();
            }
        }

        /// <summary>Отправить сообщение в поток (серверу)</summary>
        /// <param name="message">Текст сообщения</param>
        public void SendMessage(string message)
        {
            while (true)
            {
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }

        // получение сообщений
        public void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[256]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);
                    string message = builder.ToString();
                }
                catch
                {
                    this.Dispose();
                }
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }


}
