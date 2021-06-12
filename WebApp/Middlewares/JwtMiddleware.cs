using Microsoft.AspNetCore.Http;
using Server.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Services;
using WebApp.Auth;

namespace WebApp.Middlewares
{
	class JwtMiddleware : IMiddleware
	{
		private readonly AuthManager _authManager;

		public JwtMiddleware(UseCases.Interfaces.Providers.ITimeProvider timeProvider)
		{
			_authManager = new AuthManager(timeProvider);
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			var token = context.Request.Headers["Authorization"].ToString().Split(" ").LastOrDefault();
			if (!string.IsNullOrEmpty(token))
			{
				var userInfo = _authManager.GetUserInfoByJwt(token);
				if (userInfo != null)
				{
					context.User = new System.Security.Claims.ClaimsPrincipal(new UserIdentity(userInfo.Login, true, "jwt")
					{
						UserId = userInfo.Id
					});
				}
			}
		}
	}
}
