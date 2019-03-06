using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Model
{
    [Message(OpcodeType.Login)]
    public class C2G_Login
    {
        public string Account { get; set;}
        public string PassWord { get; set; }
    }
}
