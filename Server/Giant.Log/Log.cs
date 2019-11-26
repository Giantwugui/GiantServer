using NLog;
using System;
using System.Collections.Generic;

namespace Giant.Logger
{
    /// <summary>
    /// 自定义日志输出类
    /// </summary>
    public static class Log
    {
        private static bool writeToConsole = false;
        public static string nowStringWithSeconds = "yyyy-MM-dd HH:mm:ss.fff";
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

        public static void Init(bool write2Console, string appType, int appId, int subId)
        {
            writeToConsole = write2Console;
            LogManager.Configuration.Variables["appType"] = appType;
            LogManager.Configuration.Variables["appId"] = appId.ToString();
            LogManager.Configuration.Variables["subId"] = subId.ToString();
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
#if DEBUG
            WriteToConsole(message);
#endif
        }

        public static void Trace(object message)
        {
            logAdapter.Trace(message);
#if DEBUG
            WriteToConsole(message);
#endif
        }

        public static void Warn(object message)
        {
            logAdapter.Warn(message);
            WriteToConsole(message, ConsoleColor.Yellow);
        }

        public static void Error(object message)
        {
            logAdapter.Error(message);
            WriteToConsole(message, ConsoleColor.Red);
        }

        public static void Fatal(object message)
        {
            logAdapter.Fatal(message);
            WriteToConsole(message, ConsoleColor.DarkRed);
        }

        public static void WriteToConsole(object message, ConsoleColor consoleColor = ConsoleColor.White)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine($"{DateTime.Now.ToString(nowStringWithSeconds)} {message}");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}