using System;
using System.Collections.Generic;
namespace Giant.Msg
{
	[Message(OuterOpcode.CG_TEST)]
	public partial class CG_TEST : IRequest {}

	[Message(OuterOpcode.GC_TEST)]
	public partial class GC_TEST : IResponse {}

	[Message(OuterOpcode.C2R_LOGIN)]
	public partial class C2R_LOGIN : IRequest {}

	[Message(OuterOpcode.R2C_LOGIN)]
	public partial class R2C_LOGIN : IResponse {}

	[Message(OuterOpcode.CG_LOGIN)]
	public partial class CG_LOGIN : IRequest {}

	[Message(OuterOpcode.GC_LOGIN)]
	public partial class GC_LOGIN : IResponse {}

	[Message(OuterOpcode.G2C_TestHotfixMessage)]
	public partial class G2C_TestHotfixMessage : IMessage {}

	[Message(OuterOpcode.C2M_TestActorRequest)]
	public partial class C2M_TestActorRequest : IActorLocationRequest {}

	[Message(OuterOpcode.M2C_TestActorResponse)]
	public partial class M2C_TestActorResponse : IActorLocationResponse {}

	[Message(OuterOpcode.PlayerInfo)]
	public partial class PlayerInfo : IMessage {}

	[Message(OuterOpcode.C2G_PlayerInfo)]
	public partial class C2G_PlayerInfo : IRequest {}

	[Message(OuterOpcode.G2C_PlayerInfo)]
	public partial class G2C_PlayerInfo : IResponse {}

	[Message(OuterOpcode.C2M_TestRequest)]
	public partial class C2M_TestRequest : IActorLocationRequest {}

	[Message(OuterOpcode.M2C_TestResponse)]
	public partial class M2C_TestResponse : IActorLocationResponse {}

	[Message(OuterOpcode.Actor_TransferRequest)]
	public partial class Actor_TransferRequest : IActorLocationRequest {}

	[Message(OuterOpcode.Actor_TransferResponse)]
	public partial class Actor_TransferResponse : IActorLocationResponse {}

	[Message(OuterOpcode.C2G_EnterMap)]
	public partial class C2G_EnterMap : IRequest {}

	[Message(OuterOpcode.G2C_EnterMap)]
	public partial class G2C_EnterMap : IResponse {}

// 自己的unit id
// 所有的unit
	[Message(OuterOpcode.UnitInfo)]
	public partial class UnitInfo {}

	[Message(OuterOpcode.M2C_CreateUnits)]
	public partial class M2C_CreateUnits : IActorMessage {}

	[Message(OuterOpcode.Frame_ClickMap)]
	public partial class Frame_ClickMap : IActorLocationMessage {}

	[Message(OuterOpcode.M2C_PathfindingResult)]
	public partial class M2C_PathfindingResult : IActorMessage {}

	[Message(OuterOpcode.C2R_Ping)]
	public partial class C2R_Ping : IRequest {}

	[Message(OuterOpcode.R2C_Ping)]
	public partial class R2C_Ping : IResponse {}

	[Message(OuterOpcode.G2C_Test)]
	public partial class G2C_Test : IMessage {}

	[Message(OuterOpcode.C2M_Reload)]
	public partial class C2M_Reload : IRequest {}

	[Message(OuterOpcode.M2C_Reload)]
	public partial class M2C_Reload : IResponse {}

}
namespace Giant.Msg
{
	public static partial class OuterOpcode
	{
		 public const ushort CG_TEST = 101;
		 public const ushort GC_TEST = 102;
		 public const ushort C2R_LOGIN = 103;
		 public const ushort R2C_LOGIN = 104;
		 public const ushort CG_LOGIN = 105;
		 public const ushort GC_LOGIN = 106;
		 public const ushort G2C_TestHotfixMessage = 107;
		 public const ushort C2M_TestActorRequest = 108;
		 public const ushort M2C_TestActorResponse = 109;
		 public const ushort PlayerInfo = 110;
		 public const ushort C2G_PlayerInfo = 111;
		 public const ushort G2C_PlayerInfo = 112;
		 public const ushort C2M_TestRequest = 113;
		 public const ushort M2C_TestResponse = 114;
		 public const ushort Actor_TransferRequest = 115;
		 public const ushort Actor_TransferResponse = 116;
		 public const ushort C2G_EnterMap = 117;
		 public const ushort G2C_EnterMap = 118;
		 public const ushort UnitInfo = 119;
		 public const ushort M2C_CreateUnits = 120;
		 public const ushort Frame_ClickMap = 121;
		 public const ushort M2C_PathfindingResult = 122;
		 public const ushort C2R_Ping = 123;
		 public const ushort R2C_Ping = 124;
		 public const ushort G2C_Test = 125;
		 public const ushort C2M_Reload = 126;
		 public const ushort M2C_Reload = 127;

		public static readonly Dictionary<ushort, Type> Opcode2Types = new Dictionary<ushort, Type>
		{
			{CG_TEST, typeof(CG_TEST)},
			{GC_TEST, typeof(GC_TEST)},
			{C2R_LOGIN, typeof(C2R_LOGIN)},
			{R2C_LOGIN, typeof(R2C_LOGIN)},
			{CG_LOGIN, typeof(CG_LOGIN)},
			{GC_LOGIN, typeof(GC_LOGIN)},
			{G2C_TestHotfixMessage, typeof(G2C_TestHotfixMessage)},
			{C2M_TestActorRequest, typeof(C2M_TestActorRequest)},
			{M2C_TestActorResponse, typeof(M2C_TestActorResponse)},
			{PlayerInfo, typeof(PlayerInfo)},
			{C2G_PlayerInfo, typeof(C2G_PlayerInfo)},
			{G2C_PlayerInfo, typeof(G2C_PlayerInfo)},
			{C2M_TestRequest, typeof(C2M_TestRequest)},
			{M2C_TestResponse, typeof(M2C_TestResponse)},
			{Actor_TransferRequest, typeof(Actor_TransferRequest)},
			{Actor_TransferResponse, typeof(Actor_TransferResponse)},
			{C2G_EnterMap, typeof(C2G_EnterMap)},
			{G2C_EnterMap, typeof(G2C_EnterMap)},
			{UnitInfo, typeof(UnitInfo)},
			{M2C_CreateUnits, typeof(M2C_CreateUnits)},
			{Frame_ClickMap, typeof(Frame_ClickMap)},
			{M2C_PathfindingResult, typeof(M2C_PathfindingResult)},
			{C2R_Ping, typeof(C2R_Ping)},
			{R2C_Ping, typeof(R2C_Ping)},
			{G2C_Test, typeof(G2C_Test)},
			{C2M_Reload, typeof(C2M_Reload)},
			{M2C_Reload, typeof(M2C_Reload)},
		};
	}
}
