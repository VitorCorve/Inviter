using ClientServerComponents.Infrastructure.ClientCommands;
using ClientServerComponents.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientServerComponents
{
    public class ClientComponent
    {
        public delegate void Notification(string message);
        public event Notification Log;
        private readonly IPAddress IP = IPAddress.Parse("127.0.0.1");
        private const int Port = 1337;
        private Socket Client;
        private byte[] Buffer = new byte[1024];
        public ClientComponent()
        {
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        public void Connect()
        {
            Log?.Invoke($"Client initialized");
            int attempts = 0;

            do
            {
                try
                {
                    Client.Connect(IP, Port);
                    Log?.Invoke($"\nConnected to server");
                    attempts++;
                    Thread thread = new Thread(ReceiveAsync);
                    thread.Start();
                }
                catch (Exception e)
                {
                    Log?.Invoke($"Error with connecting to server: {e.Message}\nAttempt:{attempts}\nTrying to reconnect...");
                    Thread.Sleep(500);
                }

            } while (!Client.Connected && attempts < 6);
        }
        private async void ReceiveAsync()
        {
            while (Client.Connected)
            {
                int bytes = await Client.ReceiveAsync(new ArraySegment<byte>(Buffer), SocketFlags.None);
                string response = Encoding.UTF8.GetString(Buffer, 0, bytes);
                Log?.Invoke(response);
            }
        }
        private async void SendMessage(string message)
        {
            try
            {
                message = message + "<EOF>";
                byte[] bytesToSend = Encoding.UTF8.GetBytes(message);
                await Client.SendAsync(bytesToSend, SocketFlags.None);
            }
            catch (Exception e)
            {
                Log?.Invoke($"Error with sending message to server {e.Message}");
            }
        }
        public void Deliver(Messages message)
        {
            message.SendedTime = DateTime.Now;
            string msg = JsonConvert.SerializeObject(message);
            SendMessage(msg);
        }
        public void Execute(ClientActions statement)
        {
            switch (statement)
            {
                case ClientActions.Close:
                    Client.Close();
                    Log?.Invoke($"Connection to {IP}:{Port} closed");
                    break;
                case ClientActions.Connect:
                    Connect();
                    break;
                default:
                    break;
            }
        }
    }
}
