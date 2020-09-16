using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZModels
{
    public class Terminal
    {
        public int TerminalId { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string UserName{ get; set; }
        public string Password { get; set; }
        public string Sudo { get; set; }
        public string UserId{ get; set; }
        public string Shared { get; set; }
    }
}
