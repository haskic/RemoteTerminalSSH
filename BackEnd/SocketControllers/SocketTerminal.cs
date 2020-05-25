using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebSocketManager;
using SSH_USER_CORE;

namespace BackEnd.SocketControllers
{
    //public class SocketTerminal:WebSocketHandler
    //{
    //    public SocketTerminal(WebSocketConnectionManager webSocketConnectionManager) : base(webSocketConnectionManager)
    //    {
            
    //    }
    //    public async Task SendMessage(string socketId, string message)
    //    {
    //        await InvokeClientMethodToAllAsync("get_message", socketId, message);
    //    }
    //    public async Task ConnectSSH(string socketId,string userId,string ) {
    //        if (UserContainer.getUserById(userId) == null) {
    //            User newUser = new User();
    //            newUser.UserId = userId;
    //            UserContainer.Users.Add(newUser);
    //            UserContainer.getUserById(userId).CreateSSHConnection();
    //            UserContainer.getUserById(userId).SSHConnect();

    //        }
    //        else
    //        {
    //            UserContainer.getUserById(userId).CreateSSHConnection();
    //            UserContainer.getUserById(userId).SSHConnect();
    //        }
    //        string[] ar = { $"SSH CONNECTION FOR USER ID={userId} was created." };

    //        await InvokeClientMethodAsync(socketId, "GetResponse", ar);
    //    }

    //    public async Task CommandSSH(string socketId, string userId,string sshCommand)
    //    {
    //        if (UserContainer.getUserById(userId) == null)
    //        {
    //            string[] ar = { $"Error, user was not founded. ID={userId} doesn't exist." };
    //            await InvokeClientMethodAsync(socketId, "GetResponse", ar);

    //        }
    //        else
    //        {

    //            string sshResponse = UserContainer.getUserById(userId).SSHRunCommand(sshCommand);
    //            string[] ar = { $"SSH COMMAND:{sshCommand} FOR USER ID={userId} was execute.",sshResponse };
    //            await InvokeClientMethodAsync(socketId, "GetResponseCommand", ar);

    //        }
    //    }
    //    public async Task DissconectSSH(string socketId, string userId)
    //    {
    //        if (UserContainer.getUserById(userId) == null)
    //        {
    //            string[] ar = { $"Error, user was not founded. ID={userId} doesn't exist." };
    //            await InvokeClientMethodAsync(socketId, "GetResponse", ar);

    //        }
    //        else
    //        {

    //            UserContainer.getUserById(userId).SSHDisconnect();
    //            string[] ar = { $"SSH CONNECTION FOR USER ID={userId} was deleted." };
    //            await InvokeClientMethodAsync(socketId, "GetResponse", ar);

    //        }
    //    }
    //    public async Task GetUserNameSSHTerminal(string socketId, string userId)
    //    {
    //        if (UserContainer.getUserById(userId) == null)
    //        {
    //            string[] ar = { $"Error, user was not founded. ID={userId} doesn't exist." };
    //            await InvokeClientMethodAsync(socketId, "GetResponse", ar);

    //        }
    //        else
    //        {

    //            string name = UserContainer.getUserById(userId).SSHRunCommand("whoami");
    //            string[] ar = { $"TERMINAL NAME:{name}.", name };
    //            await InvokeClientMethodAsync(socketId, "GetNameTerminal", ar);

    //        }
    //    }
    //}
}
