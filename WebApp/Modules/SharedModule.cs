using Microsoft.Extensions.DependencyInjection;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UseCases.Interfaces.Providers;

namespace WebApp.Modules
{
	public static class SharedModule
	{
		public static void Register(IServiceCollection services)
		{
			services.AddSingleton<ITimeProvider, TimeProvider>();
			services.AddScoped<IAttachmentContentProvider, AttachmentContentProvider>();
		}
	}
}
