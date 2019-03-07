using Giant.Model;
using System;

namespace Giant.App
{
    class Program
    {
        static void Main(string[] args)
        {

            Game.Screen.AddComponent<OpcodeComponent>();
            Game.Screen.AddComponent<TimerComponent>();

            while (true)
            {

                //做服务器每帧更新
                Game.EventSystem.Update();
            }
        }
    }
}
