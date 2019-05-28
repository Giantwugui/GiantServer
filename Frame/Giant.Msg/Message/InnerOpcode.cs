using System;
using System.Collections.Generic;
namespace Giant.Msg
{
	[Message(InnerOpcode.RG_GetLoginKey)]
	public partial class RG_GetLoginKey : IRequest {}

	[Message(InnerOpcode.GR_GetLoginKey)]
	public partial class GR_GetLoginKey : IResponse {}

	[Message(InnerOpcode.MG_CreateUnit)]
	public partial class MG_CreateUnit : IResponse {}

}
namespace Giant.Msg
{
	public static partial class InnerOpcode
	{
		 public const ushort RG_GetLoginKey = 1001;
		 public const ushort GR_GetLoginKey = 1002;
		 public const ushort MG_CreateUnit = 1003;

		public static readonly Dictionary<ushort, Type> Opcode2Types = new Dictionary<ushort, Type>
		{
			{RG_GetLoginKey, typeof(RG_GetLoginKey)},
			{GR_GetLoginKey, typeof(GR_GetLoginKey)},
			{MG_CreateUnit, typeof(MG_CreateUnit)},
		};
	}
}
