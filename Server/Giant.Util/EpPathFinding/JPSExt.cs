using System.Runtime.InteropServices;

namespace JumpPointSearch
{
    public class JPSExt
    {
        [DllImport(@".\JPSlibv2.dll")]//, CharSet=CharSet.Auto, CallingConvention=CallingConvention.Cdecl,EntryPoint = "ctz")]
        public static extern uint clz(uint x);

        [DllImport(@".\JPSlibv2.dll")]
        public static extern ulong clzl(ulong x);

        [DllImport(@".\JPSlibv2.dll")]
        public static extern ushort clzs(ushort x);

        [DllImport(@".\JPSlibv2.dll")]
        public static extern ulong ffs64(ulong x);

        [DllImport(@".\JPSlibv2.dll")]
        public static extern uint ffs(uint x);
    }
}
