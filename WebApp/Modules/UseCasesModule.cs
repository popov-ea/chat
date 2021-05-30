using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UseCases.Implementations.Services;
using UseCases.Interfaces.Services;

namespace WebApp.Modules
{
	public static class UseCasesModule
	{
		public static void Register(IServiceCollection services)
		{
			services.AddScoped<IConversationService, ConversationService>();
			services.AddScoped<IConversationUserService, ConversationUserService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IBlackListService, BlackListService>();
			services.AddScoped<IMessageService, MessageService>();
		}
	}
}
