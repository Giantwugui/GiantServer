using Giant.Core;
using Giant.Logger;
using System.Runtime.InteropServices;

namespace Giant.Framework
{
    public class WindowsEventComponent : InitSystem
    {
        public override void Init()
        {
            //窗口关闭事件
            SetConsoleCtrlHandler(cancelHandler, true);
        }

        delegate bool ControlCtrlHandle(int ctrlType);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(ControlCtrlHandle HandlerRoutine, bool Add);
        private static readonly ControlCtrlHandle cancelHandler = new ControlCtrlHandle(HandleMathord);

        private static bool HandleMathord(int ctrlType)
        {
            switch (ctrlType)
            {
                case 0:
                    Log.Warn("无法使用 Ctrl+C 强制关闭窗口"); //Ctrl+C关闭
                    return true;
                case 2:
                    Log.Warn("工具被强制关闭");//按控制台关闭按钮关闭
                    return true;
            }

            return false;
        }
    }
}
