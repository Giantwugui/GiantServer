using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace GiantNode
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public enum LogType
    {
        Debug = 1,//调试
        Warning = 2,//警告
        Error = 3,//错误
        Crash = 4//奔溃
    }
}
