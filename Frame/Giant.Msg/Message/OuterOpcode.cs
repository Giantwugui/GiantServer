using System;
using System.Collections.Generic;
namespace Giant.Msg
{
	[Message(OuterOpcode.CA_StopApp)]
	public partial class CA_StopApp : IMessage {}

	[Message(OuterOpcode.CR_LOGIN)]
	public partial class CR_LOGIN : IRequest {}

	[Message(OuterOpcode.RC_LOGIN)]
	public partial class RC_LOGIN : IResponse {}

	[Message(OuterOpcode.CG_LOGIN)]
	public partial class CG_LOGIN : IRequest {}

	[Message(OuterOpcode.GC_LOGIN)]
	public partial class GC_LOGIN : IResponse {}

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
		 public const ushort CA_StopApp = 101;
		 public const ushort CR_LOGIN = 102;
		 public const ushort RC_LOGIN = 103;
		 public const ushort CG_LOGIN = 104;
		 public const ushort GC_LOGIN = 105;
		 public const ushort PlayerInfo = 106;
		 public const ushort CG_PlayerInfo = 107;
		 public const ushort GC_PlayerInfo = 108;
		 public const ushort CG_EnterMap = 109;
		 public const ushort GC_EnterMap = 110;
		 public const ushort UnitInfo = 111;
		 public const ushort MC_PathfindingResult = 112;
		 public const ushort CR_Ping = 113;
		 public const ushort RC_Ping = 114;
		 public const ushort CM_Reload = 115;
		 public const ushort MC_Reload = 116;
		 public const ushort ZGC_Broadcast = 117;

		public static readonly Dictionary<ushort, Type> Opcode2Types = new Dictionary<ushort, Type>
		{
			{CA_StopApp, typeof(CA_StopApp)},
			{CR_LOGIN, typeof(CR_LOGIN)},
			{RC_LOGIN, typeof(RC_LOGIN)},
			{CG_LOGIN, typeof(CG_LOGIN)},
			{GC_LOGIN, typeof(GC_LOGIN)},
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
