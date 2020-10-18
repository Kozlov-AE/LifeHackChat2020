using Logic.TCPConnection;
using System;
using System.Threading.Tasks;

namespace LifeHackChat2020_Console
{
    class Program
    {
        static ClientModel client;
        static void Main(string[] args)
        {
            try
            {
            client = new ClientModel("127.0.0.1", 8888);
            client.Connect();
            client.ExceptionEvent += ExceptionPrint;
            client.Connected += PrintConnected;
            client.Disconected += PrintDisconnected;
            client.ReceivedMessage += MessagePrint;
            SendMessage(client);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();
        }

        private static void PrintDisconnected()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Отключились от сервера");
        }

        private static void PrintConnected()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Подключение выполнено");
        }

        private static void MessagePrint(object sender, MessageHandler message)
        {
            if ((string)sender == "Сервер")
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{message.Time.ToLongTimeString()}   {(string)sender}: {message.Message}");
                Console.ResetColor();
            }
        }

        static void ExceptionPrint(object sender, ExceptionHandler args)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"{args.Date.ToLongTimeString()}   {(string)sender}: {args.Message}");
            Console.ResetColor();
        }

        static void SendMessage(ClientModel client)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                string message = Console.ReadLine();
                if (message.ToLower() == "пока" || message.ToLower() == "/пока")
                {
                    client.Dispose();
                    Environment.Exit(0);
                }
                client.SendMessage(message);
            }
        }
    }
}
