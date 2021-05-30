using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UseCases.Implementations;

namespace WebApp.Modules
{
	public static class MapperModule
	{
		public static void Register(IServiceCollection services)
		{
			services.AddScoped<Mapper>((provider) =>
			{
				return MapperHelpers.GetConfiguredMapper();
			});
		}
	}
}
