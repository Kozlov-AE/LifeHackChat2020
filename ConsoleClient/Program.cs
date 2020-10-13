using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleClient
{
    class Program
    {
        static string ?userName;
        private const string ?host = "127.0.0.1";
        private const int port = 8888;
        static TcpClient ?client;
        static NetworkStream ?stream;

        static void Main(string[] args)
        {
            client = new TcpClient();
            try
            {
                Console.WriteLine("Попытка подключения");
                client.Connect(host, port);
                if (client.Connected) Console.WriteLine("Подключение успешно установлено");
                stream = client.GetStream();

                // запускаем новый поток для получения данных
                var receiveTask = Task.Factory.StartNew(ReceiveMessage);
                SendMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Попробуйте подключиться позже.");
            }
            finally
            {
                Disconnect();
            }
        }

        // отправка сообщений
        static void SendMessage()
        {
            Console.WriteLine("Введите сообщение: ");

            while (true)
            {
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }
        // получение сообщений
        static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    Console.WriteLine(message);//вывод сообщения
                }
                catch
                {
                    Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        static void Disconnect()
        {
            stream?.Close(); //Отключение потока
            client?.Close(); //Отключение клиента
            Environment.Exit(0); //Завершение процесса
        }
    }
}
