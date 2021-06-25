using DataAccess;
using DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.EFCore;
using Domain.Entities;

namespace WebApp.Modules
{
	public static class RepositoriesModule
	{
		public static void Register(IServiceCollection services)
		{
			services.AddScoped<IRepository<Attachment>, EFCoreRepository<Attachment>>();
			services.AddScoped<IRepository<BlackList>, EFCoreRepository<BlackList>>();
			services.AddScoped<IRepository<ChatAction>, EFCoreRepository<ChatAction>>();
			services.AddScoped<IRepository<Conversation>, EFCoreRepository<Conversation>>();
			services.AddScoped<IRepository<ConversationUser>, EFCoreRepository<ConversationUser>>();
			services.AddScoped<IRepository<Message>, EFCoreRepository<Message>>();
			services.AddScoped<IRepository<User>, EFCoreRepository<User>>();
		}
	}
}
