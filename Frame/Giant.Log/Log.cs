using NLog;
using System;
using System.Collections.Generic;

namespace Giant.Log
{
    /// <summary>
    /// 自定义日志输出类
    /// </summary>
    public static class Logger
    {
        private static bool writeToConsole = false;
        private static readonly LogAdapter logAdapter = new LogAdapter();

        public static void Init(bool write2Console, Dictionary<string, string> param)
        {
            writeToConsole = write2Console;

            //添加自定义变量以及其他规则
            //对于不同的日志使用场景，需要做不同的实现
            foreach (var kv in param)
            {
                LogManager.Configuration.Variables[kv.Key] = kv.Value;
            }
        }

        public static void Init(bool write2Console, string appType, int appId)
        {
            writeToConsole = write2Console;
            LogManager.Configuration.Variables["appType"] = appType;
            LogManager.Configuration.Variables["appId"] = appId.ToString();
        }

        public static void Debug(object message)
        {
#if DEBUG
            logAdapter.Debug(message);
            WriteToConsole(message);
#endif
        }

        public static void Info(object message)
        {
            logAdapter.Info(message);
            WriteToConsole(message);
        }

        public static void Trace(object message)
        {
            logAdapter.Trace(message);
            WriteToConsole(message);
        }

        public static void Warn(object message)
        {
            logAdapter.Warn(message);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{DateTime.Now.ToString()} {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Error(object message)
        {
            logAdapter.Error(message);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now.ToString()} {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void Fatal(object message)
        {
            logAdapter.Fatal(message);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"{DateTime.Now.ToString()} {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void WriteToConsole(object message)
        {
#if DEBUG
            Console.WriteLine($"{DateTime.Now.ToString()} {message}");
#else
            if (writeToConsole)
            {
                Console.WriteLine(message);
            }
#endif
        }
    }
}