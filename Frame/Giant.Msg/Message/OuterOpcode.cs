using System;
using System.Collections.Generic;
namespace Giant.Msg
{
	[Message(OuterOpcode.CG_TEST)]
	public partial class CG_TEST : IRequest {}

	[Message(OuterOpcode.GC_TEST)]
	public partial class GC_TEST : IResponse {}

	[Message(OuterOpcode.CR_LOGIN)]
	public partial class CR_LOGIN : IRequest {}

	[Message(OuterOpcode.RC_LOGIN)]
	public partial class RC_LOGIN : IResponse {}

	[Message(OuterOpcode.CG_LOGIN)]
	public partial class CG_LOGIN : IRequest {}

	[Message(OuterOpcode.GC_LOGIN)]
	public partial class GC_LOGIN : IResponse {}

	[Message(OuterOpcode.GC_TestHotfixMessage)]
	public partial class GC_TestHotfixMessage : IMessage {}

	[Message(OuterOpcode.CM_TestActorRequest)]
	public partial class CM_TestActorRequest : IActorLocationRequest {}

	[Message(OuterOpcode.MC_TestActorResponse)]
	public partial class MC_TestActorResponse : IActorLocationResponse {}

	[Message(OuterOpcode.PlayerInfo)]
	public partial class PlayerInfo : IMessage {}

	[Message(OuterOpcode.CG_PlayerInfo)]
	public partial class CG_PlayerInfo : IRequest {}

	[Message(OuterOpcode.GC_PlayerInfo)]
	public partial class GC_PlayerInfo : IResponse {}

	[Message(OuterOpcode.CM_TestRequest)]
	public partial class CM_TestRequest : IActorLocationRequest {}

	[Message(OuterOpcode.MC_TestResponse)]
	public partial class MC_TestResponse : IActorLocationResponse {}

	[Message(OuterOpcode.Actor_TransferRequest)]
	public partial class Actor_TransferRequest : IActorLocationRequest {}

	[Message(OuterOpcode.Actor_TransferResponse)]
	public partial class Actor_TransferResponse : IActorLocationResponse {}

	[Message(OuterOpcode.CG_EnterMap)]
	public partial class CG_EnterMap : IRequest {}

	[Message(OuterOpcode.GC_EnterMap)]
	public partial class GC_EnterMap : IResponse {}

// 自己的unit id
// 所有的unit
	[Message(OuterOpcode.UnitInfo)]
	public partial class UnitInfo {}

	[Message(OuterOpcode.MC_CreateUnits)]
	public partial class MC_CreateUnits : IActorMessage {}

	[Message(OuterOpcode.Frame_ClickMap)]
	public partial class Frame_ClickMap : IActorLocationMessage {}

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

}
namespace Giant.Msg
{
	public static partial class OuterOpcode
	{
		 public const ushort CG_TEST = 101;
		 public const ushort GC_TEST = 102;
		 public const ushort CR_LOGIN = 103;
		 public const ushort RC_LOGIN = 104;
		 public const ushort CG_LOGIN = 105;
		 public const ushort GC_LOGIN = 106;
		 public const ushort GC_TestHotfixMessage = 107;
		 public const ushort CM_TestActorRequest = 108;
		 public const ushort MC_TestActorResponse = 109;
		 public const ushort PlayerInfo = 110;
		 public const ushort CG_PlayerInfo = 111;
		 public const ushort GC_PlayerInfo = 112;
		 public const ushort CM_TestRequest = 113;
		 public const ushort MC_TestResponse = 114;
		 public const ushort Actor_TransferRequest = 115;
		 public const ushort Actor_TransferResponse = 116;
		 public const ushort CG_EnterMap = 117;
		 public const ushort GC_EnterMap = 118;
		 public const ushort UnitInfo = 119;
		 public const ushort MC_CreateUnits = 120;
		 public const ushort Frame_ClickMap = 121;
		 public const ushort MC_PathfindingResult = 122;
		 public const ushort CR_Ping = 123;
		 public const ushort RC_Ping = 124;
		 public const ushort CM_Reload = 125;
		 public const ushort MC_Reload = 126;

		public static readonly Dictionary<ushort, Type> Opcode2Types = new Dictionary<ushort, Type>
		{
			{CG_TEST, typeof(CG_TEST)},
			{GC_TEST, typeof(GC_TEST)},
			{CR_LOGIN, typeof(CR_LOGIN)},
			{RC_LOGIN, typeof(RC_LOGIN)},
			{CG_LOGIN, typeof(CG_LOGIN)},
			{GC_LOGIN, typeof(GC_LOGIN)},
			{GC_TestHotfixMessage, typeof(GC_TestHotfixMessage)},
			{CM_TestActorRequest, typeof(CM_TestActorRequest)},
			{MC_TestActorResponse, typeof(MC_TestActorResponse)},
			{PlayerInfo, typeof(PlayerInfo)},
			{CG_PlayerInfo, typeof(CG_PlayerInfo)},
			{GC_PlayerInfo, typeof(GC_PlayerInfo)},
			{CM_TestRequest, typeof(CM_TestRequest)},
			{MC_TestResponse, typeof(MC_TestResponse)},
			{Actor_TransferRequest, typeof(Actor_TransferRequest)},
			{Actor_TransferResponse, typeof(Actor_TransferResponse)},
			{CG_EnterMap, typeof(CG_EnterMap)},
			{GC_EnterMap, typeof(GC_EnterMap)},
			{UnitInfo, typeof(UnitInfo)},
			{MC_CreateUnits, typeof(MC_CreateUnits)},
			{Frame_ClickMap, typeof(Frame_ClickMap)},
			{MC_PathfindingResult, typeof(MC_PathfindingResult)},
			{CR_Ping, typeof(CR_Ping)},
			{RC_Ping, typeof(RC_Ping)},
			{CM_Reload, typeof(CM_Reload)},
			{MC_Reload, typeof(MC_Reload)},
		};
	}
}
