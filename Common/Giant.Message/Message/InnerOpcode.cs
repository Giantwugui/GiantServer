using Giant.Message;
namespace Giant.Message
{
	[Message(InnerOpcode.M2M_TrasferUnitResponse)]
	public partial class M2M_TrasferUnitResponse : IResponse {}

	[Message(InnerOpcode.M2A_Reload)]
	public partial class M2A_Reload : IRequest {}

	[Message(InnerOpcode.A2M_Reload)]
	public partial class A2M_Reload : IResponse {}

	[Message(InnerOpcode.G2G_LockRequest)]
	public partial class G2G_LockRequest : IRequest {}

	[Message(InnerOpcode.G2G_LockResponse)]
	public partial class G2G_LockResponse : IResponse {}

	[Message(InnerOpcode.G2G_LockReleaseRequest)]
	public partial class G2G_LockReleaseRequest : IRequest {}

	[Message(InnerOpcode.G2G_LockReleaseResponse)]
	public partial class G2G_LockReleaseResponse : IResponse {}

	[Message(InnerOpcode.DBSaveBatchResponse)]
	public partial class DBSaveBatchResponse : IResponse {}

	[Message(InnerOpcode.DBSaveResponse)]
	public partial class DBSaveResponse : IResponse {}

	[Message(InnerOpcode.DBQueryRequest)]
	public partial class DBQueryRequest : IRequest {}

	[Message(InnerOpcode.DBQueryBatchRequest)]
	public partial class DBQueryBatchRequest : IRequest {}

	[Message(InnerOpcode.DBQueryJsonRequest)]
	public partial class DBQueryJsonRequest : IRequest {}

	[Message(InnerOpcode.ObjectAddRequest)]
	public partial class ObjectAddRequest : IRequest {}

	[Message(InnerOpcode.ObjectAddResponse)]
	public partial class ObjectAddResponse : IResponse {}

	[Message(InnerOpcode.ObjectRemoveRequest)]
	public partial class ObjectRemoveRequest : IRequest {}

	[Message(InnerOpcode.ObjectRemoveResponse)]
	public partial class ObjectRemoveResponse : IResponse {}

	[Message(InnerOpcode.ObjectLockRequest)]
	public partial class ObjectLockRequest : IRequest {}

	[Message(InnerOpcode.ObjectLockResponse)]
	public partial class ObjectLockResponse : IResponse {}

	[Message(InnerOpcode.ObjectUnLockRequest)]
	public partial class ObjectUnLockRequest : IRequest {}

	[Message(InnerOpcode.ObjectUnLockResponse)]
	public partial class ObjectUnLockResponse : IResponse {}

	[Message(InnerOpcode.ObjectGetRequest)]
	public partial class ObjectGetRequest : IRequest {}

	[Message(InnerOpcode.ObjectGetResponse)]
	public partial class ObjectGetResponse : IResponse {}

	[Message(InnerOpcode.R2G_GetLoginKey)]
	public partial class R2G_GetLoginKey : IRequest {}

	[Message(InnerOpcode.G2R_GetLoginKey)]
	public partial class G2R_GetLoginKey : IResponse {}

	[Message(InnerOpcode.G2M_CreateUnit)]
	public partial class G2M_CreateUnit : IRequest {}

	[Message(InnerOpcode.M2G_CreateUnit)]
	public partial class M2G_CreateUnit : IResponse {}

// 自己的unit id
// 所有的unit
//repeated UnitInfo Units = 2;
	[Message(InnerOpcode.G2M_SessionDisconnect)]
	public partial class G2M_SessionDisconnect : IActorLocationMessage {}

}
namespace Giant.Message
{
	public static partial class InnerOpcode
	{
		 public const ushort M2M_TrasferUnitResponse = 1001;
		 public const ushort M2A_Reload = 1002;
		 public const ushort A2M_Reload = 1003;
		 public const ushort G2G_LockRequest = 1004;
		 public const ushort G2G_LockResponse = 1005;
		 public const ushort G2G_LockReleaseRequest = 1006;
		 public const ushort G2G_LockReleaseResponse = 1007;
		 public const ushort DBSaveBatchResponse = 1008;
		 public const ushort DBSaveResponse = 1009;
		 public const ushort DBQueryRequest = 1010;
		 public const ushort DBQueryBatchRequest = 1011;
		 public const ushort DBQueryJsonRequest = 1012;
		 public const ushort ObjectAddRequest = 1013;
		 public const ushort ObjectAddResponse = 1014;
		 public const ushort ObjectRemoveRequest = 1015;
		 public const ushort ObjectRemoveResponse = 1016;
		 public const ushort ObjectLockRequest = 1017;
		 public const ushort ObjectLockResponse = 1018;
		 public const ushort ObjectUnLockRequest = 1019;
		 public const ushort ObjectUnLockResponse = 1020;
		 public const ushort ObjectGetRequest = 1021;
		 public const ushort ObjectGetResponse = 1022;
		 public const ushort R2G_GetLoginKey = 1023;
		 public const ushort G2R_GetLoginKey = 1024;
		 public const ushort G2M_CreateUnit = 1025;
		 public const ushort M2G_CreateUnit = 1026;
		 public const ushort G2M_SessionDisconnect = 1027;
	}
}
