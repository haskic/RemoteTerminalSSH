using System;
using System.Collections.Generic;
using System.Text;

namespace ZModels
{
    public class TerminalErrorModel
    {
        public string ErrorType { get; set; }
        public string ErrorText { get; set; }
        public string TerminalName { get; set; }
        public string UserId { get; set; }
    }
}
