using Giant.Message;
namespace Giant.Message
{
	[Message(OuterOpcode.C2R_Login)]
	public partial class C2R_Login : IRequest {}

	[Message(OuterOpcode.R2C_Login)]
	public partial class R2C_Login : IResponse {}

	[Message(OuterOpcode.C2G_LoginGate)]
	public partial class C2G_LoginGate : IRequest {}

	[Message(OuterOpcode.G2C_LoginGate)]
	public partial class G2C_LoginGate : IResponse {}

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
namespace Giant.Message
{
	public static partial class OuterOpcode
	{
		 public const ushort C2R_Login = 101;
		 public const ushort R2C_Login = 102;
		 public const ushort C2G_LoginGate = 103;
		 public const ushort G2C_LoginGate = 104;
		 public const ushort G2C_TestHotfixMessage = 105;
		 public const ushort C2M_TestActorRequest = 106;
		 public const ushort M2C_TestActorResponse = 107;
		 public const ushort PlayerInfo = 108;
		 public const ushort C2G_PlayerInfo = 109;
		 public const ushort G2C_PlayerInfo = 110;
		 public const ushort C2M_TestRequest = 111;
		 public const ushort M2C_TestResponse = 112;
		 public const ushort Actor_TransferRequest = 113;
		 public const ushort Actor_TransferResponse = 114;
		 public const ushort C2G_EnterMap = 115;
		 public const ushort G2C_EnterMap = 116;
		 public const ushort UnitInfo = 117;
		 public const ushort M2C_CreateUnits = 118;
		 public const ushort Frame_ClickMap = 119;
		 public const ushort M2C_PathfindingResult = 120;
		 public const ushort C2R_Ping = 121;
		 public const ushort R2C_Ping = 122;
		 public const ushort G2C_Test = 123;
		 public const ushort C2M_Reload = 124;
		 public const ushort M2C_Reload = 125;
	}
}
