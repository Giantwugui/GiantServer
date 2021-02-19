using Giant.Core;
using Giant.Logger;
using System;

namespace Giant.Framework
{
    public class KeyboardInputEventComponent : InitSystem
    {
        public override void Init()
        {
            OperatingSystem operatingSystem = Environment.OSVersion;

            Log.Info($"system version {operatingSystem}");

            if (operatingSystem.VersionString.StartsWith("Microsoft Windows"))
            {
                Scene.Pool.AddComponent<WindowsKeyboardInputCheckComponent>();
            }
        }
    }
}
