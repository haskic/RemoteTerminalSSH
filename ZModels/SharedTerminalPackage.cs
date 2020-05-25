using System;
using System.Collections.Generic;
using System.Text;

namespace ZModels
{
    public class SharedTerminalPackage
    {
        public string GuestId{ get; set; }
        public List<SharedTerminal> Terminals { get; set; }

    }
}
