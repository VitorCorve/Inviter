using ClientServerComponents.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace ClientServerComponents.Services
{
    public class UsersManager
    {
        public static List<UserModel> UsersOnline { get; private set; } = new List<UserModel>();
        public static void RegisterOnlineStatus(UserModel user) => UsersOnline.Add(user);
        public static void WriteOffOnlineStatus(Socket client)
        {
            UserModel unsignedUser = UsersOnline.Where(x => x.Client.RemoteEndPoint == client.RemoteEndPoint).FirstOrDefault();
            UsersOnline.Remove(unsignedUser);
        }
        public static bool IsAuthorized(User user) => UsersOnline.Where(x => x.User.Id == user.Id).FirstOrDefault() != null;
        public static List<UserModel> Find(User user) => UsersOnline.Where(x => x.User.Id == user.Id).ToList();
    }
}
