using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetStandartData
{
   /// <summary>Упаковка для сообщения</summary>
	public class ClientModel
	{
      #region Events
      /// <summary>Получено сообщение </summary>
      public event Action<object, MessageHandler> ReceivedMessage;
      /// <summary>Отправлено сообщение </summary>
      public event Action<object, MessageHandler> SendedMessage;

      /// <summary>Произошло исключение </summary>
      public event Action<object, ExceptionHandler> ExceptionEvent;
      /// <summary>Подключились к серверу</summary>
      public event Action Connected;
      /// <summary> Отключились от сервера</summary>
      public event Action Disconected;
      #endregion

      public bool IsConnected;
      string host;
      int port;
      NetworkStream stream;
      static TcpClient client;

      public ClientModel(string host, int port)
      {
         this.host = host;
         this.port = port;
      }

      /// <summary> Подключиться к серверу </summary>
      public async Task Connect()
      {
         client = new TcpClient();
         while (!client.Connected)
         {
            try
            {
               client.Connect(host, port);
               stream = client.GetStream();
               var receiveTask = Task.Factory.StartNew(ReceiveMessage);
            }
            catch (Exception ex)
            {
               ExceptionEvent?.Invoke("Метод подключения", new ExceptionHandler(ex.Message));
               Debug.WriteLine(ex.Message);
               Thread.Sleep(1000);
            }
         }
      }

      /// <summary>Отправить сообщение в поток (серверу)</summary>
      /// <param name="message">Текст сообщения</param>
      public void SendMessage(string message)
      {
         SendedMessage?.Invoke("Я", new MessageHandler(message));
         byte[] data = Encoding.Unicode.GetBytes(message);
         stream.Write(data, 0, data.Length);
      }

      /// <summary>Получение сообщений в цикле</summary>
      public void ReceiveMessage()
      {
         IsConnected = true;
         while (IsConnected)
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

               if (builder.Length > 0)
                  ReceivedMessage?.Invoke("Сервер", new MessageHandler(builder.ToString()));
            }
            catch (Exception ex)
            {
               ExceptionEvent?.Invoke("Метод получения сообщений", new ExceptionHandler(ex.Message));
               this.Dispose();
            }
         }
      }

      public void Dispose()
      {
         stream?.Close(); //Отключение потока
         client?.Close(); //Отключение клиента
         IsConnected = false;
      }
   }
}
