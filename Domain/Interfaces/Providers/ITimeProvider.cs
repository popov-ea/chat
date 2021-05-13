using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Providers
{
	public interface ITimeProvider
	{
		public DateTime NowUtc();
	}
}
