using Server.Model;
using System;
using System.Threading;

namespace Server
{
    class Program
    {
        static ServerModel server;
        static Thread listenThread;
        static void Main(string[] args)
        {
            try
            {
                server = new ServerModel();
                server.OnStarted += Console.WriteLine;
                server.OnClientGetsMessage += Console.WriteLine;
                server.OnException += Console.WriteLine;
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start();
            }
            catch (Exception ex)
            { 
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
