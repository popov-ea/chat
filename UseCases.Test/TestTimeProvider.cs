using System;
using System.Collections.Generic;
using System.Text;
using UseCases.Interfaces.Providers;

namespace UseCases.Test
{
	class TestTimeProvider : ITimeProvider
	{
		public DateTime NowUtc() => DateTime.Now;
	}
}
