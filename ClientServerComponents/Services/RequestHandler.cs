using ClientServerComponents.Infrastructure.ClientCommands;
using ClientServerComponents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using static ClientServerComponents.ServerComponent;

namespace ClientServerComponents.Services
{
    public class RequestHandler
    {
        public event Notification Log;
        public MessengerDbContext Context { get; set; }
        public RequestHandler(MessengerDbContext context)
        {
            Context = context;
        }
        public async void HandleRequest(Socket requestorClient, Messages message)
        {
            string result = "";

            if (!CanRequest(message.RequestType, message.Sender, requestorClient))
            {
                result = "Not authorized";
            }
            else
            {
                try
                {
                    switch (message.RequestType)
                    {
                        case ClientRequest.UserInfoRequest:
                            break;
                        case ClientRequest.Authorization:
                            if (AuthorizeUser(message.Sender, requestorClient))
                                result = message.Sender.Login + " succesfully logged in";
                            else
                                result = "Wrong password or username";
                            break;
                        case ClientRequest.Registration:
                            if (RegisterUser(message.Sender))
                                result = message.Sender.Login + " succesfully registered";
                            else
                                result = $"Cannot register user: {message.Sender.Login} Login already exists";
                            break;
                        case ClientRequest.CommunicationRequest:
                            break;
                        case ClientRequest.Message:
                            if (RouteMessage(message, requestorClient))
                            {
                                result = "Delivered";
                                Context.Messages.Add(message);
                            }
                            else
                                result = "User not found";
                            break;
                        case ClientRequest.BlockUserRequest:
                            break;
                        case ClientRequest.MediaTypeMessage:
                            break;
                        default:
                            result = "Error with request handling";
                            break;
                    }
                }
                catch (Exception e)
                {
                    Log?.Invoke($"Request handling error: {e.Message}. Cannot handle request");
                }
            }
            try
            {
                string serverResponse = result;
                byte[] responseData = Encoding.UTF8.GetBytes(serverResponse);
                await requestorClient.SendAsync(responseData, SocketFlags.None, System.Threading.CancellationToken.None);
            }
            catch (Exception e)
            {
                Log?.Invoke($"Client communication error. Cannot send data: {e.Message}");
            }
            try
            {
                Context.Messages.Add(message);
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Log?.Invoke($"Error with saving message to database: {e.Message}");
            }

        }
        private bool CanRequest(ClientRequest clientRequest, User requestor, Socket requestorClient)
        {
            if (clientRequest != ClientRequest.Authorization && clientRequest != ClientRequest.Registration)
            {
                if (!UsersManager.IsAuthorized(requestor))
                {
                    Log?.Invoke($"Unauthorized request from: {requestorClient.RemoteEndPoint}: Type: {clientRequest}, Sender: {requestor.Nickname}");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (UsersManager.IsAuthorized(requestor))
                {
                    Log?.Invoke($"Double authorization attempt from: {requestorClient.RemoteEndPoint}: Type: {clientRequest}, Sender: {requestor.Nickname}");
                    return false;
                }
                return true;
            }
        }
        private bool RouteMessage(Messages message, Socket requestorClient)
        {
            try
            {
                User target = Context.Users.Where(x => x.Id == message.TargetId).FirstOrDefault();
                if (target is null)
                {
                    Log?.Invoke($"Message request error from {requestorClient.RemoteEndPoint}: Target not found");
                    return false;
                }
                else
                {
                    List<UserModel> users = UsersManager.Find(target);
                    if (target is null)
                    {
                        return true;
                    }
                    else
                    {
                        byte[] content = Encoding.UTF8.GetBytes(message.Text);

                        foreach (UserModel user in users)
                            user.Client.SendAsync(content, SocketFlags.None, System.Threading.CancellationToken.None);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Log?.Invoke($"Message request internal error: {e.Message}. Requestor: {requestorClient.RemoteEndPoint}");
                return false;
            }
        }
        private bool RegisterUser(User requestor)
        {
            try
            {
                User user = Context.Users.Where(x => x.Login == requestor.Login).FirstOrDefault();
                if (user is null)
                {
                    Context.Users.Add(requestor);
                    Log?.Invoke(requestor.Login + " succesfully registered");
                    return true;
                }
                else
                {
                    Log?.Invoke($"User registration error: User with login {user.Login} already exists");
                    return false;
                }
            }
            catch (Exception e)
            {
                Log?.Invoke($"Registration error: {e.Message}. Cannot register user {requestor.Nickname}");
                return false;
            }

        }
        private bool AuthorizeUser(User requestor, Socket requestorClient)
        {
            Log?.Invoke($"Attempt to authorize from {requestorClient.RemoteEndPoint}, Login: {requestor.Login}");
            try
            {
                if (UsersManager.IsAuthorized(requestor))
                {
                    Log?.Invoke($"Double authorization attempt from: {requestorClient.RemoteEndPoint}");
                    return false;
                }
                User user = Context.Users.Where(x => x.Login == requestor.Login).FirstOrDefault();

                if (user is null)
                {
                    Log?.Invoke("Authorization error: " + user.Login + $" user no exists. Requestor: {requestorClient.RemoteEndPoint}");
                    return false;
                }
                else
                {
                    if (user.Password == requestor.Password)
                    {
                        UsersManager.RegisterOnlineStatus(new UserModel() { Client = requestorClient, User = user });
                        var users = UsersManager.UsersOnline;
                        Log?.Invoke($"User {requestor.Login} succesfuly logged in. Requestor: {requestorClient.RemoteEndPoint}");
                        return true;
                    }
                    else
                    {
                        Log?.Invoke($"User {requestor.Login} wrong password attempt from {requestorClient.RemoteEndPoint}");
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                Log?.Invoke($"User {requestor.Login} authorization error from {requestorClient.RemoteEndPoint}: {e.Message}");
                return false;
            }
        }
    }
}
