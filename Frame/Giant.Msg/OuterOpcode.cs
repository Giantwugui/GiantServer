using System;
using System.Collections.Generic;
namespace Giant.Msg
{
	[Message(OuterOpcode.CR_Login)]
	public partial class CR_Login : IRequest {}

	[Message(OuterOpcode.RC_Login)]
	public partial class RC_Login : IResponse {}

	[Message(OuterOpcode.CG_Login)]
	public partial class CG_Login : IRequest {}

	[Message(OuterOpcode.GC_Login)]
	public partial class GC_Login : IResponse {}

	[Message(OuterOpcode.PlayerInfo)]
	public partial class PlayerInfo : IMessage {}

	[Message(OuterOpcode.CG_PlayerInfo)]
	public partial class CG_PlayerInfo : IRequest {}

	[Message(OuterOpcode.GC_PlayerInfo)]
	public partial class GC_PlayerInfo : IResponse {}

	[Message(OuterOpcode.CG_EnterMap)]
	public partial class CG_EnterMap : IRequest {}

	[Message(OuterOpcode.GC_EnterMap)]
	public partial class GC_EnterMap : IResponse {}

	[Message(OuterOpcode.UnitInfo)]
	public partial class UnitInfo {}

	[Message(OuterOpcode.MC_PathfindingResult)]
	public partial class MC_PathfindingResult : IActorMessage {}

	[Message(OuterOpcode.CR_Ping)]
	public partial class CR_Ping : IRequest {}

	[Message(OuterOpcode.RC_Ping)]
	public partial class RC_Ping : IResponse {}

	[Message(OuterOpcode.CM_Reload)]
	public partial class CM_Reload : IRequest {}

	[Message(OuterOpcode.MC_Reload)]
	public partial class MC_Reload : IResponse {}

	[Message(OuterOpcode.ZGC_Broadcast)]
	public partial class ZGC_Broadcast : IMessage {}

}
namespace Giant.Msg
{
	public static partial class OuterOpcode
	{
		 public const ushort CR_Login = 101;
		 public const ushort RC_Login = 102;
		 public const ushort CG_Login = 103;
		 public const ushort GC_Login = 104;
		 public const ushort PlayerInfo = 105;
		 public const ushort CG_PlayerInfo = 106;
		 public const ushort GC_PlayerInfo = 107;
		 public const ushort CG_EnterMap = 108;
		 public const ushort GC_EnterMap = 109;
		 public const ushort UnitInfo = 110;
		 public const ushort MC_PathfindingResult = 111;
		 public const ushort CR_Ping = 112;
		 public const ushort RC_Ping = 113;
		 public const ushort CM_Reload = 114;
		 public const ushort MC_Reload = 115;
		 public const ushort ZGC_Broadcast = 116;

		public static readonly Dictionary<ushort, Type> Opcode2Types = new Dictionary<ushort, Type>
		{
			{CR_Login, typeof(CR_Login)},
			{RC_Login, typeof(RC_Login)},
			{CG_Login, typeof(CG_Login)},
			{GC_Login, typeof(GC_Login)},
			{PlayerInfo, typeof(PlayerInfo)},
			{CG_PlayerInfo, typeof(CG_PlayerInfo)},
			{GC_PlayerInfo, typeof(GC_PlayerInfo)},
			{CG_EnterMap, typeof(CG_EnterMap)},
			{GC_EnterMap, typeof(GC_EnterMap)},
			{UnitInfo, typeof(UnitInfo)},
			{MC_PathfindingResult, typeof(MC_PathfindingResult)},
			{CR_Ping, typeof(CR_Ping)},
			{RC_Ping, typeof(RC_Ping)},
			{CM_Reload, typeof(CM_Reload)},
			{MC_Reload, typeof(MC_Reload)},
			{ZGC_Broadcast, typeof(ZGC_Broadcast)},
		};
	}
}
