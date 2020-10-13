using Server.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static ServerModel server;
        static Task serverTask;
        static void Main(string[] args)
        {
            try
            {
                server = new ServerModel();
                server.OnStarted += Console.WriteLine;
                server.OnClientGetsMessage += Console.WriteLine;
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
