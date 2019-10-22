using System;
using System.Collections.Generic;
using System.Text;
using Giant.Data;
using Server.Frame;

namespace Server.Gate
{
    public class AccountServer : FrontendServer
    {
        public AccountServer(FrontendServerManager manager, AppConfig appConfig) : base(manager, appConfig)
        { }
    }
}
