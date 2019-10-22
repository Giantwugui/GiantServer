using System;
using System.Collections.Generic;
namespace Giant.Msg
{
	[Message(InnerOpcode.Msg_GM_StopApp)]
	public partial class Msg_GM_StopApp : IMessage {}

	[Message(InnerOpcode.Msg_AG_ClientLogin)]
	public partial class Msg_AG_ClientLogin : IMessage {}

	[Message(InnerOpcode.Msg_GateA_GateInfo)]
	public partial class Msg_GateA_GateInfo : IMessage {}

	[Message(InnerOpcode.Msg_Service_Info)]
	public partial class Msg_Service_Info : IMessage {}

	[Message(InnerOpcode.Msg_HeartBeat_Ping)]
	public partial class Msg_HeartBeat_Ping : IRequest {}

	[Message(InnerOpcode.Msg_HeartBeat_Pong)]
	public partial class Msg_HeartBeat_Pong : IResponse {}

	[Message(InnerOpcode.Msg_RegistService_Req)]
	public partial class Msg_RegistService_Req : IRequest {}

	[Message(InnerOpcode.Msg_RegistService_Rep)]
	public partial class Msg_RegistService_Rep : IResponse {}

	[Message(InnerOpcode.Msg_RG_GetLoginKey)]
	public partial class Msg_RG_GetLoginKey : IRequest {}

	[Message(InnerOpcode.Msg_GetLoginKey)]
	public partial class Msg_GetLoginKey : IResponse {}

}
namespace Giant.Msg
{
	public static partial class InnerOpcode
	{
		 public const ushort Msg_GM_StopApp = 1001;
		 public const ushort Msg_AG_ClientLogin = 1002;
		 public const ushort Msg_GateA_GateInfo = 1003;
		 public const ushort Msg_Service_Info = 1004;
		 public const ushort Msg_HeartBeat_Ping = 1005;
		 public const ushort Msg_HeartBeat_Pong = 1006;
		 public const ushort Msg_RegistService_Req = 1007;
		 public const ushort Msg_RegistService_Rep = 1008;
		 public const ushort Msg_RG_GetLoginKey = 1009;
		 public const ushort Msg_GetLoginKey = 1010;

		public static readonly Dictionary<ushort, Type> Opcode2Types = new Dictionary<ushort, Type>
		{
			{Msg_GM_StopApp, typeof(Msg_GM_StopApp)},
			{Msg_AG_ClientLogin, typeof(Msg_AG_ClientLogin)},
			{Msg_GateA_GateInfo, typeof(Msg_GateA_GateInfo)},
			{Msg_Service_Info, typeof(Msg_Service_Info)},
			{Msg_HeartBeat_Ping, typeof(Msg_HeartBeat_Ping)},
			{Msg_HeartBeat_Pong, typeof(Msg_HeartBeat_Pong)},
			{Msg_RegistService_Req, typeof(Msg_RegistService_Req)},
			{Msg_RegistService_Rep, typeof(Msg_RegistService_Rep)},
			{Msg_RG_GetLoginKey, typeof(Msg_RG_GetLoginKey)},
			{Msg_GetLoginKey, typeof(Msg_GetLoginKey)},
		};
	}
}
