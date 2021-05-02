using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Adapters.Providers
{
	public interface ITimeProvider
	{
		public DateTime NowUtc();
	}
}
