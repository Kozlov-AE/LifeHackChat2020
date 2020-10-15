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
        static IDialogHandler dialogHandler = new DialogHandler.DialogHandler(new MemoryDialogStorage());

        static void Main(string[] args)
        {
            try
            {
                dialogHandler.LoadData();
                server = new LifeChatServer(new ConnectionStorageService());
                server.OnStarted += Console.WriteLine;
                server.OnClientCreated += (c) => Console.WriteLine($"Подключился новый клиент с Id \"{c?.Id}\"");
                server.OnClientGetsMessage += Console.WriteLine;
                server.OnClientGetsMessage += MessageProcessing;
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

        private static void MessageProcessing (ClientMessageHandler handler)
        {
            const string insertMessage = "\n Введите сообщение";
            const string noAnswer = "Я не знаю ответ, давайте попробуем еще раз.";

            if (dialogHandler.GetAnswer(handler.Message) is string answer)
            {
                answer.Concat(insertMessage);
            }
            else
            {
                answer = noAnswer;
            }
            server.SendMessageToClient(answer, handler.Client.Id);
        }
    }
}
