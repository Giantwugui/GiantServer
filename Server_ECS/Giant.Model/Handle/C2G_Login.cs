using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Model
{
    [Message(Opcode.C2G_login)]
    public class C2G_Login
    {
        public string Account { get; set;}
        public string PassWord { get; set; }
    }
}
