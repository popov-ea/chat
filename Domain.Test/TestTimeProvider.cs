using Domain.Interfaces.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Test
{
	class TestTimeProvider : ITimeProvider
	{
		public DateTime NowUtc() => DateTime.Now;
	}
}
