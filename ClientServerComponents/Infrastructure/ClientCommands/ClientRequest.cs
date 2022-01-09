using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServerComponents.Infrastructure.ClientCommands
{
    public enum ClientRequest
    {
        UserInfoRequest,
        Authorization,
        Registration,
        CommunicationRequest,
        Message,
        BlockUserRequest,
        MediaTypeMessage,
        ChangePassword
    }
}
