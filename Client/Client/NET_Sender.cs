using Giant.Msg;
using Giant.Net;
using System;

namespace Client
{
    partial class NET
    {
        public static async void DoLogin(string account)
        {
            if (!IsConnected)
            {
                return;
            }

            Msg_CG_Login login = new Msg_CG_Login
            {
                Account = account,
            };

            Msg_GC_Login result = await session.Call(login) as Msg_GC_Login;
            if (result.Error == ErrorCode.Success)
            {
                Console.WriteLine($"Client login success {login.Account}");
            }
        }
    }
}
