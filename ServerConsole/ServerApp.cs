using ClientServerComponents;
using System;

namespace ServerConsole
{
    public class ServerApp
    {
        static void Main(string[] args)
        {
            Console.Title = "Server";
            ServerComponent server = new ServerComponent();
            server.Log += Print;
            server.Start();
            Console.ReadKey();
        }
        private static void Print(string message) => Console.WriteLine(message);
    }
}
