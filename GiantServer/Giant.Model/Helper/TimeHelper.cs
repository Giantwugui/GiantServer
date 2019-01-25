using System;

namespace Giant.Model
{
	public static class TimeHelper
	{

		public static long Now()
		{
			return (DateTime.UtcNow.Ticks - epoch) / 10000;
		}

		public static long NowSeconds()
		{
			return (DateTime.UtcNow.Ticks - epoch) / 10000000;
		}

		private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;
    }
}