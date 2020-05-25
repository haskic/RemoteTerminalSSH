using Renci.SshNet;
using System;

namespace SSH_USER_CORE
{
    public class User
    {
        private SshClient cSSH;
        public string UserId { get; set; }

        public bool CreateSSHConnection(string ipAddress,string login,string password) {
            try
            {
                this.cSSH = new SshClient(ipAddress, login, password);
                return true;
            }
            catch (Exception e)
            {
                return false;
                throw;
            }
        }
        public void SSHConnect() { 
            this.cSSH.Connect();

        }
        public string SSHRunCommand(string command) { 
            SshCommand ssh_cmd = cSSH.RunCommand(command);
            if (ssh_cmd.Result != "") { 
                return ssh_cmd.Result;
            }
            else
            {
                return ssh_cmd.Error;
            }
        }
        public void SSHDisconnect() {
            this.cSSH.Disconnect();
            this.cSSH.Dispose();
        
        }

    }
}
