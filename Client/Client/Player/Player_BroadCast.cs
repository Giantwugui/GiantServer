using Giant.Msg;
using System;

namespace Client
{
    partial class Player
    {
        public void OnNotify_BroadCast(ZGC_Broadcast broadcast)
        {
            Console.WriteLine($"Player {Uid} receive broadcast {broadcast.Message}");
        }
    }
}
