using System;
using System.Collections.Generic;
namespace Giant.Msg
{
	[Message(InnerOpcode.CA_StopApp)]
	public partial class CA_StopApp : IMessage {}

	[Message(InnerOpcode.HeartBeat_Ping)]
	public partial class HeartBeat_Ping : IMessage {}

	[Message(InnerOpcode.HeartBeat_Pong)]
	public partial class HeartBeat_Pong : IMessage {}

	[Message(InnerOpcode.Msg_RegistService_Req)]
	public partial class Msg_RegistService_Req : IRequest {}

	[Message(InnerOpcode.Msg_RegistService_Rep)]
	public partial class Msg_RegistService_Rep : IResponse {}

	[Message(InnerOpcode.RG_GetLoginKey)]
	public partial class RG_GetLoginKey : IRequest {}

	[Message(InnerOpcode.GR_GetLoginKey)]
	public partial class GR_GetLoginKey : IResponse {}

}
namespace Giant.Msg
{
	public static partial class InnerOpcode
	{
		 public const ushort CA_StopApp = 1001;
		 public const ushort HeartBeat_Ping = 1002;
		 public const ushort HeartBeat_Pong = 1003;
		 public const ushort Msg_RegistService_Req = 1004;
		 public const ushort Msg_RegistService_Rep = 1005;
		 public const ushort RG_GetLoginKey = 1006;
		 public const ushort GR_GetLoginKey = 1007;

		public static readonly Dictionary<ushort, Type> Opcode2Types = new Dictionary<ushort, Type>
		{
			{CA_StopApp, typeof(CA_StopApp)},
			{HeartBeat_Ping, typeof(HeartBeat_Ping)},
			{HeartBeat_Pong, typeof(HeartBeat_Pong)},
			{Msg_RegistService_Req, typeof(Msg_RegistService_Req)},
			{Msg_RegistService_Rep, typeof(Msg_RegistService_Rep)},
			{RG_GetLoginKey, typeof(RG_GetLoginKey)},
			{GR_GetLoginKey, typeof(GR_GetLoginKey)},
		};
	}
}
