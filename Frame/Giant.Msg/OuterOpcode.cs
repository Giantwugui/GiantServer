using System;
using System.Collections.Generic;
namespace Giant.Msg
{
	[Message(OuterOpcode.Msg_CT_Login)]
	public partial class Msg_CT_Login : IRequest {}

	[Message(OuterOpcode.Msg_TC_Login)]
	public partial class Msg_TC_Login : IResponse {}

	[Message(OuterOpcode.Msg_CG_HeartBeat_Ping)]
	public partial class Msg_CG_HeartBeat_Ping : IRequest {}

	[Message(OuterOpcode.Msg_CG_Login)]
	public partial class Msg_CG_Login : IRequest {}

	[Message(OuterOpcode.Msg_CG_Get_SecretKey)]
	public partial class Msg_CG_Get_SecretKey : IRequest {}

	[Message(OuterOpcode.Msg_CG_PlayerInfo)]
	public partial class Msg_CG_PlayerInfo : IRequest {}

	[Message(OuterOpcode.Msg_GC_HeartBeat_Pong)]
	public partial class Msg_GC_HeartBeat_Pong : IResponse {}

	[Message(OuterOpcode.Msg_GC_Login)]
	public partial class Msg_GC_Login : IResponse {}

	[Message(OuterOpcode.Msg_GC_Get_SecretKey)]
	public partial class Msg_GC_Get_SecretKey : IResponse {}

	[Message(OuterOpcode.Msg_GC_PlayerInfo)]
	public partial class Msg_GC_PlayerInfo : IResponse {}

	[Message(OuterOpcode.PlayerInfo)]
	public partial class PlayerInfo : IMessage {}

	[Message(OuterOpcode.Msg_CG_EnterMap)]
	public partial class Msg_CG_EnterMap : IRequest {}

	[Message(OuterOpcode.Msg_GC_EnterMap)]
	public partial class Msg_GC_EnterMap : IResponse {}

	[Message(OuterOpcode.Msg_UnitInfo)]
	public partial class Msg_UnitInfo {}

	[Message(OuterOpcode.Msg_MC_PathfindingResult)]
	public partial class Msg_MC_PathfindingResult : IRequest {}

	[Message(OuterOpcode.CM_Reload)]
	public partial class CM_Reload : IRequest {}

	[Message(OuterOpcode.MC_Reload)]
	public partial class MC_Reload : IResponse {}

	[Message(OuterOpcode.ZGC_Broadcast)]
	public partial class ZGC_Broadcast : IMessage {}

	[Message(OuterOpcode.Msg_CG_TestMap)]
	public partial class Msg_CG_TestMap : IMessage {}

}
namespace Giant.Msg
{
	public static partial class OuterOpcode
	{
		 public const ushort Msg_CT_Login = 101;
		 public const ushort Msg_TC_Login = 102;
		 public const ushort Msg_CG_HeartBeat_Ping = 103;
		 public const ushort Msg_CG_Login = 104;
		 public const ushort Msg_CG_Get_SecretKey = 105;
		 public const ushort Msg_CG_PlayerInfo = 106;
		 public const ushort Msg_GC_HeartBeat_Pong = 107;
		 public const ushort Msg_GC_Login = 108;
		 public const ushort Msg_GC_Get_SecretKey = 109;
		 public const ushort Msg_GC_PlayerInfo = 110;
		 public const ushort PlayerInfo = 111;
		 public const ushort Msg_CG_EnterMap = 112;
		 public const ushort Msg_GC_EnterMap = 113;
		 public const ushort Msg_UnitInfo = 114;
		 public const ushort Msg_MC_PathfindingResult = 115;
		 public const ushort CM_Reload = 116;
		 public const ushort MC_Reload = 117;
		 public const ushort ZGC_Broadcast = 118;
		 public const ushort Msg_CG_TestMap = 119;

		public static readonly Dictionary<ushort, Type> Opcode2Types = new Dictionary<ushort, Type>
		{
			{Msg_CT_Login, typeof(Msg_CT_Login)},
			{Msg_TC_Login, typeof(Msg_TC_Login)},
			{Msg_CG_HeartBeat_Ping, typeof(Msg_CG_HeartBeat_Ping)},
			{Msg_CG_Login, typeof(Msg_CG_Login)},
			{Msg_CG_Get_SecretKey, typeof(Msg_CG_Get_SecretKey)},
			{Msg_CG_PlayerInfo, typeof(Msg_CG_PlayerInfo)},
			{Msg_GC_HeartBeat_Pong, typeof(Msg_GC_HeartBeat_Pong)},
			{Msg_GC_Login, typeof(Msg_GC_Login)},
			{Msg_GC_Get_SecretKey, typeof(Msg_GC_Get_SecretKey)},
			{Msg_GC_PlayerInfo, typeof(Msg_GC_PlayerInfo)},
			{PlayerInfo, typeof(PlayerInfo)},
			{Msg_CG_EnterMap, typeof(Msg_CG_EnterMap)},
			{Msg_GC_EnterMap, typeof(Msg_GC_EnterMap)},
			{Msg_UnitInfo, typeof(Msg_UnitInfo)},
			{Msg_MC_PathfindingResult, typeof(Msg_MC_PathfindingResult)},
			{CM_Reload, typeof(CM_Reload)},
			{MC_Reload, typeof(MC_Reload)},
			{ZGC_Broadcast, typeof(ZGC_Broadcast)},
			{Msg_CG_TestMap, typeof(Msg_CG_TestMap)},
		};
	}
}
