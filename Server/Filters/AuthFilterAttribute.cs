using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Filters
{
	class AuthFilterAttribute : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (context.HttpContext.User == null)
			{
				context.Result = new ForbidResult();
			}
		}
	}
}
