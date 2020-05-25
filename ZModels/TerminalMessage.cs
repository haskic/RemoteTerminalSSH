using System;
using System.Collections.Generic;
using System.Text;

namespace ZModels
{
    public class TerminalMessage
    {
        public TerminalErrorModel Error { get; set; }
        public TerminalSuccessModel Success { get; set; }

    }
}
