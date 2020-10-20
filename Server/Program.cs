using Logic.Server.ClientCommandHandler;
using Logic.Server.DialogHandler;
using Logic.Server.DialogHandler.Model;
using Logic.Server.Mediator;
using Logic.Server.Model;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.Server
{
    class Program
    {
        static LifeChatServer? server;
        static Task? serverTask;
        static MessageHandler? messageHandler;

        static void Main(string[] args)
        {
            try
            {
                //Запускаем сервер
                server = new LifeChatServer(new ConnectionStorageService());
                //Инициализируем обработчик входящих сообщений
                messageHandler =
                new MessageHandler(new DialogHandler.DialogHandler(new DialogStorage("base.json")),
                new CommandHandler(),
                server);
                server.OnStarted += Console.WriteLine;
                server.OnClientCreated += (c) => Console.WriteLine($"Подключился новый клиент с Id \"{c?.Id}\"");
                server.OnClientGetsMessage += Console.WriteLine;
                server.OnClientGetsMessage += messageHandler.ProcessMessage;
                server.OnClientConnected += (c) => Console.WriteLine($"Клиент с Id {c.Id} указал свое имя \"{c.UserName}\"");
                server.OnClientDisconected += (c) => Console.WriteLine($"Клиент {c.UserName} отключился от сервера");
                server.OnException += Console.WriteLine;
                serverTask = Task.Factory.StartNew(server.Listen);
                serverTask.Wait();
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message);
            }
        }
    }
}
