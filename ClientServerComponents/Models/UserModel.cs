using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerComponents.Models
{
    public class UserModel
    {
        public User User { get; set; }
        public Socket Client { get; set; }
    }
}
