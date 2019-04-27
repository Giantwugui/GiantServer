using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Giant.Model
{
    abstract class BaseService
    {
        public Action<BChannel> AcceptCallBack { get; set; }

    }
}
