using BackEnd.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using SSH_USER_CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZModels;

namespace BackEnd.Controllers
{
    public class TerminalHub:Hub
    {
        private UserDbContext db;
        public ILogger<Program> Logger { get; }

        public TerminalHub(UserDbContext context, ILogger<Program> logger)
        {
            db = context;
            Logger = logger;
            
        }
        public async Task CreateSSH(string connectionId,string UserId,string terminalName)
        {
            Logger.LogError("I'm in connectSSH");

            bool shared = false;
            Desktop result;
            //string[] ar = { $"SSH CONNECTION FOR USER ID={connectionId} was created." };

            //await this.Clients.Client(connectionId).SendAsync("GetResponse", ar);
            Logger.LogError("TerminalName = " + terminalName);
            Logger.LogError("UserId = " + UserId);


            
            result = db.Desktops.Where(v => v.UserId == UserId).Where(v => v.Name == terminalName).FirstOrDefault();
            if (result == null)
            {
                result = db.Desktops.Where(v => v.Shared == UserId).Where(v => v.Name == terminalName).FirstOrDefault();

            }
            
            if (result != null)
            {
                if (UserContainer.getUserById(UserId) == null)
                {
                    User newUser = new User();
                    newUser.UserId = UserId;
                    UserContainer.Users.Add(newUser);
                    Logger.LogError("IP = " + result.Ip);
                    Logger.LogError("Login = " + result.UserName);
                    Logger.LogError("Password = " + result.Password);
                    if (UserContainer.getUserById(UserId).CreateSSHConnection(result.Ip, result.UserName, result.Password)) { 
                        UserContainer.getUserById(UserId).SSHConnect();

                    }
                    else
                    {
                        string res = $"SSH CONNECTION FOR USER ID={connectionId} wasn't created. Ip or login or password is incorrect";

                        await this.Clients.Client(connectionId).SendAsync("GetResponse", res);
                    }

                }
                else
                {
                    UserContainer.getUserById(UserId).CreateSSHConnection(result.Ip, result.UserName, result.Password);
                    UserContainer.getUserById(UserId).SSHConnect();
                }
                string ar =  $"SSH CONNECTION FOR USER ID={connectionId} was created." ;

                await this.Clients.Client(connectionId).SendAsync("GetResponse", ar);
            }
            else
            {
                string ar = $"SSH CONNECTION FOR USER ID={connectionId} wasn't created. Error 'USER NOT FOUNDED'";

                await this.Clients.Client(connectionId).SendAsync("GetResponse", ar);

            }
        }

        public async Task CommandSSH(string connectionId, string UserId, string sshCommand)
        {
            Logger.LogError("I'm in CommandSSH");

            if (UserContainer.getUserById(UserId) == null)
            {
                string ar = $"Error, user was not founded. ID={UserId} doesn't exist.";
                await this.Clients.Client(connectionId).SendAsync("GetResponse", ar);
            }
            else
            {
                string sshResponse = UserContainer.getUserById(UserId).SSHRunCommand(sshCommand);
                string ar = $"SSH COMMAND:{sshCommand} FOR USER ID={UserId} was execute.";
                await this.Clients.Client(connectionId).SendAsync("GetResponseCommand", ar, sshResponse);
            }
        }

        public async Task DissconectSSH(string connectionId, string UserId)
        {
            Logger.LogError("I'm in DISCONNECTEDSSH");

            if (UserContainer.getUserById(UserId) == null)
            {
                string ar = $"Error, user was not founded. ID={UserId} doesn't exist.";
                await this.Clients.Client(connectionId).SendAsync("GetResponse", ar);
            }
            else
            {
                UserContainer.getUserById(UserId).SSHDisconnect();
                string ar = $"SSH CONNECTION FOR USER ID={UserId} was deleted.";
                await this.Clients.Client(connectionId).SendAsync("GetResponse", ar);
            }
        }

        public async Task GetUserNameSSHTerminal(string connectionId, string UserId)
        {
            Logger.LogError("I'm in GETUSERSSHNAME");

            
            if (UserContainer.getUserById(UserId) == null)
            {
                string ar = $"Error, user was not founded. ID={UserId} doesn't exist." ;
                await this.Clients.Client(connectionId).SendAsync("GetResponse", ar);
            }
            else
            {
                string name = UserContainer.getUserById(UserId).SSHRunCommand("whoami");
                string ar = $"TERMINAL NAME:{name}.";
                await this.Clients.Client(connectionId).SendAsync("GetNameTerminal", ar,name);
            }
        }


    }
}
