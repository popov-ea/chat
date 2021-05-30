using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Modules
{
	public static class ModulesExtensions
	{
		public static IServiceCollection RegisterApplicationModules(this IServiceCollection services)
		{
			MapperModule.Register(services);
			RepositoriesModule.Register(services);
			UseCasesModule.Register(services);
			SharedModule.Register(services);
			return services;
		}
	}
}
