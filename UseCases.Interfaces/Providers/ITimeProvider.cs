using System;
using System.Collections.Generic;
using System.Text;

namespace UseCases.Interfaces.Providers
{
	public interface ITimeProvider
	{
		public DateTime NowUtc();
	}
}
