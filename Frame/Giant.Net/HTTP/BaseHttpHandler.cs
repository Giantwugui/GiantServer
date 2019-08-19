using System;
using System.Collections.Generic;
using System.Text;

namespace Giant.Net
{
    public abstract class BaseHttpHandler
    {
        public HttpResult Error(HttpErrorCode code, string message)
        {
            return new HttpResult() { code = (int)code, msg = message };
        }

        public HttpResult Success(object data)
        {
            return new HttpResult() { code = (int)HttpErrorCode.Success, data = data, msg = ""};
        }
    }
}
