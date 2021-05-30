using System;
using UseCases.Interfaces.Providers;

namespace Shared
{
	public class TimeProvider : ITimeProvider
	{
		public DateTime NowUtc()
		{
			return DateTime.Now;
		}
	}
}
