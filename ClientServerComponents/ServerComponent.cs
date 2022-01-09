using ClientServerComponents.Models;
using ClientServerComponents.Services;
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
    public class ServerComponent
    {
        public delegate void Notification(string message);
        public event Notification Log;
        public static ManualResetEvent allDone = new ManualResetEvent(false);
        private readonly RequestHandler RequestsHandlerService;
        private readonly IPAddress IP = IPAddress.Parse("127.0.0.1");
        private const int Port = 1337;
        private Socket Listener;
        private List<StateObject> Clients = new List<StateObject>();
        private bool Disposed;

        public ServerComponent()
        {
            RequestsHandlerService = new RequestHandler(new MessengerDbContext());
            RequestsHandlerService.Log += LogMessageOut;
            RequestsHandlerService.Log += WriteLog;
            Log += WriteLog;
        }
        private void LogMessageOut(string message) => Log?.Invoke(message);
        public async void Start()
        {
            await Task.Run(() =>
            {
                try
                {
                    Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    Disposed = false;
                    Listener.Bind(new IPEndPoint(IP, Port));
                    Listener.Listen();
                    Log?.Invoke($"Starting to listen at {IP}:{Port}");

                    do
                    {
                        allDone.Reset();
                        Log?.Invoke($"Waiting for connections...");
                        Listener.BeginAccept(new AsyncCallback(AcceptCallback), Listener);
                        allDone.WaitOne();
                    }
                    while (!Disposed);
                }
                catch (Exception e)
                {
                    Log?.Invoke($"Internal server exception: {e.Message}");
                }
            });
        }
        private void AcceptCallback(IAsyncResult asyncResult)
        {
            try
            {
                allDone.Set();
                Socket listener = (Socket)asyncResult.AsyncState;
                Socket client = Listener.EndAccept(asyncResult);
                StateObject state = new StateObject();
                state.WorkSocket = client;
                Clients.Add(state);
                try
                {
                    Log?.Invoke($"{client.RemoteEndPoint} connected");
                    client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReadCallback), state);
                }
                catch (Exception e)
                {
                    Log?.Invoke($"Client {client.RemoteEndPoint} disconnected");
                    UsersManager.WriteOffOnlineStatus(client);
                    Clients.Remove(state);
                }
            }
            catch (ObjectDisposedException)
            {
                Log?.Invoke($"Shutdown connection");
            }
            catch (Exception)
            {

            }
        }
        private void ReadCallback(IAsyncResult asyncResult)
        {
            String content = String.Empty;
            StateObject state = (StateObject)asyncResult.AsyncState;
            Socket client = state.WorkSocket;
            try
            {
                byte[] buf = new byte[client.ReceiveBufferSize];

                int bytesRead = client.EndReceive(asyncResult);

                if (bytesRead > 0)
                {
                    state.Sb.Append(Encoding.UTF8.GetString(state.Buffer, 0, bytesRead));
                    content = state.Sb.ToString();

                    int length = content.IndexOf("<EOF>");
                    if (length > -1)
                    {
                        content = content.Replace("<EOF>", "");
                        Messages message = Deserialize(content);
                        message.DeliveredToServerTime = DateTime.Now;
                        string senderName = message.Sender.Login ?? "Unauthorized";

                        Log?.Invoke($"Request from: {client.RemoteEndPoint}: Type: {message.RequestType}, Sender: {senderName}");

                        RequestsHandlerService.HandleRequest(client, message);
                    }
                    else
                    {
                        client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReadCallback), state);
                    }
                }
                state.Sb.Clear();
                client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, SocketFlags.None, new AsyncCallback(ReadCallback), state);
            }
            catch (Exception e)
            {
                if (e.Source == "Newtonsoft.Json")
                {
                    Log?.Invoke($"Unprotocolized request from {client.RemoteEndPoint}. Body: {content}");
                }
                Log?.Invoke($"Client {client.RemoteEndPoint} disconnected");
                UsersManager.WriteOffOnlineStatus(client);
            }
        }
        public void StopListening() // Stop Listening
        {
            Socket exListener = Interlocked.Exchange(ref Listener, null);
            if (exListener != null)
            {
                exListener.Close();
            }
            Disposed = true;
            UsersManager.UsersOnline.FirstOrDefault().Client.Shutdown(SocketShutdown.Both);
        }
        public void Dispose()
        {
            StopListening();
        }
        private async void WriteLog(string message)
        {
            using (MessengerDbContext context = new MessengerDbContext())
            {
                context.Logs.Add(new Logs { Date = DateTime.Now, Message = message });
                await context.SaveChangesAsync();
            }
        }
        private Messages Deserialize(string message) => JsonConvert.DeserializeObject<Messages>(message);

    }
}
