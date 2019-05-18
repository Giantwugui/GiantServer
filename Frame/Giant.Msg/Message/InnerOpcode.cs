using System;
using System.Collections.Generic;
namespace Giant.Msg
{
	[Message(InnerOpcode.MA_Reload)]
	public partial class MA_Reload : IRequest {}

	[Message(InnerOpcode.RG_GetLoginKey)]
	public partial class RG_GetLoginKey : IRequest {}

	[Message(InnerOpcode.GR_GetLoginKey)]
	public partial class GR_GetLoginKey : IResponse {}

	[Message(InnerOpcode.GM_CreateUnit)]
	public partial class GM_CreateUnit : IRequest {}

	[Message(InnerOpcode.MG_CreateUnit)]
	public partial class MG_CreateUnit : IResponse {}

// 自己的unit id
// 所有的unit
//repeated UnitInfo Units = 2;
	[Message(InnerOpcode.GM_SessionDisconnect)]
	public partial class GM_SessionDisconnect : IActorLocationMessage {}

}
namespace Giant.Msg
{
	public static partial class InnerOpcode
	{
		 public const ushort MA_Reload = 1001;
		 public const ushort RG_GetLoginKey = 1002;
		 public const ushort GR_GetLoginKey = 1003;
		 public const ushort GM_CreateUnit = 1004;
		 public const ushort MG_CreateUnit = 1005;
		 public const ushort GM_SessionDisconnect = 1006;

		public static readonly Dictionary<ushort, Type> Opcode2Types = new Dictionary<ushort, Type>
		{
			{MA_Reload, typeof(MA_Reload)},
			{RG_GetLoginKey, typeof(RG_GetLoginKey)},
			{GR_GetLoginKey, typeof(GR_GetLoginKey)},
			{GM_CreateUnit, typeof(GM_CreateUnit)},
			{MG_CreateUnit, typeof(MG_CreateUnit)},
			{GM_SessionDisconnect, typeof(GM_SessionDisconnect)},
		};
	}
}
