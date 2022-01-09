using ClientServerComponents;
using ClientServerComponents.Infrastructure.ClientCommands;
using ClientServerComponents.Models;
using System;

namespace ClientConsole
{
    public class ClientApp
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";
            ClientComponent client = new ClientComponent();
            client.Log += Print;

            client.Execute(ClientActions.Connect);

            Authorize(client);

            while (true)
            {
                SendMessageToTarget(client, Console.ReadLine());
            }


            //CreateUser(client);
            System.Threading.Thread.Sleep(3000);

            SendMessageToTarget(client);

            System.Threading.Thread.Sleep(3000);

            SendMessageToTarget(client);

            Console.ReadLine();
        }
        private static void Authorize(ClientComponent client)
        {
            User user = new User()
            {
                Nickname = "VitorCorve",
                DateCreated = DateTime.Now,
                LastSeen = DateTime.Now.ToString(),
                Login = "Admin",
                Password = "Querty",
                Id = Guid.NewGuid(),
                Status = ClientStatus.Online
            };

            Messages message = new Messages()
            {
                MessageId = Guid.NewGuid(),
                RequestType = ClientRequest.Authorization,
                Sender = user,
            };

            client.Deliver(message);
        }
        private static void CreateUser(ClientComponent client)
        {
            User user = new User()
            {
                Nickname = "VitorCorve",
                DateCreated = DateTime.Now,
                LastSeen = DateTime.Now.ToString(),
                Login = "Admin",
                Password = "Querty",
                Id = Guid.NewGuid(),
                Status = ClientStatus.Online
            };

            Messages message = new Messages()
            {
                MessageId = Guid.NewGuid(),
                RequestType = ClientRequest.Registration,
                Sender = user,
            };

            client.Deliver(message);
        }
        private static void SendMessageToTarget(ClientComponent client)
        {
            User sender = new User()
            {
                Login = "Admin",
                Id = new Guid("8CB1AC7A-EB0E-42E4-95B9-3371A3B37332"),
            };
            Messages message = new Messages()
            {
                MessageId = Guid.NewGuid(),
                RequestType = ClientRequest.Message,
                Sender = sender,
                SenderId = new Guid("8CB1AC7A-EB0E-42E4-95B9-3371A3B37332"),
                TargetId = new Guid("8CB1AC7A-EB0E-42E4-95B9-3371A3B37332"),
                Text = "Hello admin",
            };
            client.Deliver(message);
        }
        private static void SendMessageToTarget(ClientComponent client, string msg)
        {
            User sender = new User()
            {
                Login = "Admin",
                Id = new Guid("8CB1AC7A-EB0E-42E4-95B9-3371A3B37332"),
            };
            Messages message = new Messages()
            {
                MessageId = Guid.NewGuid(),
                RequestType = ClientRequest.Message,
                Sender = sender,
                SenderId = new Guid("8CB1AC7A-EB0E-42E4-95B9-3371A3B37332"),
                TargetId = new Guid("8CB1AC7A-EB0E-42E4-95B9-3371A3B37332"),
                Text = msg,
            };
            client.Deliver(message);
        }
        private static void Print(string message) => Console.WriteLine(message);
    }
}
