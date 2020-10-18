using Server.ClientCommandHandler;
using Server.DialogHandler;
using Server.DialogHandler.Model;
using Server.Mediator;
using Server.Model;
using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Server
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
                server = new LifeChatServer(new ConnectionStorageService());
                messageHandler =
                new MessageHandler(new DialogHandler.DialogHandler(new MemoryDialogStorage()),
                new CommandHandler(server),
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
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
