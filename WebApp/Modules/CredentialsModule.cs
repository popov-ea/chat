using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth.Implementation;
using Auth.Interfaces;
using DataAccess.EFCore;

namespace WebApp.Modules
{
	public static class CredentialsModule
	{
		public static void Register(IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<IAuthService, AuthService>();
			serviceCollection.AddScoped<ICredentialsRepository, EFCoreCredentialsRepository>();
		}
	}
}
