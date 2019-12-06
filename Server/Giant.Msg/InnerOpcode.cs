using System;
using System.Collections.Generic;
namespace Giant.Msg
{
    [Message(InnerOpcode.Msg_GM_StopApp)]
    public partial class Msg_GM_StopApp : IMessage { }

    [Message(InnerOpcode.Msg_AG_ClientLogin)]
    public partial class Msg_AG_ClientLogin : IMessage { }

    [Message(InnerOpcode.Msg_GateA_GateInfo)]
    public partial class Msg_GateA_GateInfo : IMessage { }

    [Message(InnerOpcode.Msg_GateM_BalanceZone)]
    public partial class Msg_GateM_BalanceZone : IRequest { }

    [Message(InnerOpcode.Msg_GateM_GetUid)]
    public partial class Msg_GateM_GetUid : IRequest { }

    [Message(InnerOpcode.Msg_MGate_BalanceZone)]
    public partial class Msg_MGate_BalanceZone : IResponse { }

    [Message(InnerOpcode.Msg_MGate_GetUid)]
    public partial class Msg_MGate_GetUid : IResponse { }

    [Message(InnerOpcode.Msg_Service_Info)]
    public partial class Msg_Service_Info : IMessage { }

    [Message(InnerOpcode.Msg_HeartBeat_Ping)]
    public partial class Msg_HeartBeat_Ping : IRequest { }

    [Message(InnerOpcode.Msg_HeartBeat_Pong)]
    public partial class Msg_HeartBeat_Pong : IResponse { }

    [Message(InnerOpcode.Msg_RegistService_Req)]
    public partial class Msg_RegistService_Req : IRequest { }

    [Message(InnerOpcode.Msg_RegistService_Rep)]
    public partial class Msg_RegistService_Rep : IResponse { }

    [Message(InnerOpcode.Msg_RG_GetLoginKey)]
    public partial class Msg_RG_GetLoginKey : IRequest { }

    [Message(InnerOpcode.Msg_GetLoginKey)]
    public partial class Msg_GetLoginKey : IResponse { }

    [Message(InnerOpcode.Msg_GateZ_EnterWorld)]
    public partial class Msg_GateZ_EnterWorld : IMessage { }

}
namespace Giant.Msg
{
    public static partial class InnerOpcode
    {
        public const ushort Msg_GM_StopApp = 1001;
        public const ushort Msg_AG_ClientLogin = 1002;
        public const ushort Msg_GateA_GateInfo = 1003;
        public const ushort Msg_GateM_BalanceZone = 1004;
        public const ushort Msg_GateM_GetUid = 1005;
        public const ushort Msg_MGate_BalanceZone = 1006;
        public const ushort Msg_MGate_GetUid = 1007;
        public const ushort Msg_Service_Info = 1008;
        public const ushort Msg_HeartBeat_Ping = 1009;
        public const ushort Msg_HeartBeat_Pong = 1010;
        public const ushort Msg_RegistService_Req = 1011;
        public const ushort Msg_RegistService_Rep = 1012;
        public const ushort Msg_RG_GetLoginKey = 1013;
        public const ushort Msg_GetLoginKey = 1014;
        public const ushort Msg_GateZ_EnterWorld = 1015;

        public static readonly Dictionary<ushort, Type> Opcode2Types = new Dictionary<ushort, Type>
        {
            {Msg_GM_StopApp, typeof(Msg_GM_StopApp)},
            {Msg_AG_ClientLogin, typeof(Msg_AG_ClientLogin)},
            {Msg_GateA_GateInfo, typeof(Msg_GateA_GateInfo)},
            {Msg_GateM_BalanceZone, typeof(Msg_GateM_BalanceZone)},
            {Msg_GateM_GetUid, typeof(Msg_GateM_GetUid)},
            {Msg_MGate_BalanceZone, typeof(Msg_MGate_BalanceZone)},
            {Msg_MGate_GetUid, typeof(Msg_MGate_GetUid)},
            {Msg_Service_Info, typeof(Msg_Service_Info)},
            {Msg_HeartBeat_Ping, typeof(Msg_HeartBeat_Ping)},
            {Msg_HeartBeat_Pong, typeof(Msg_HeartBeat_Pong)},
            {Msg_RegistService_Req, typeof(Msg_RegistService_Req)},
            {Msg_RegistService_Rep, typeof(Msg_RegistService_Rep)},
            {Msg_RG_GetLoginKey, typeof(Msg_RG_GetLoginKey)},
            {Msg_GetLoginKey, typeof(Msg_GetLoginKey)},
            {Msg_GateZ_EnterWorld, typeof(Msg_GateZ_EnterWorld)},
        };
    }
}
