using Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Server.Dtos;
using Server.Filters;
using Server.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;
using UseCases.Interfaces.Services;

namespace Server.Controllers
{
	[ApiController]
	[Route("auth")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly IUserService _userService;
		private readonly IAuthorizationTokenGenerator _tokenGenerator;

		public AuthController(IAuthService authService, IUserService userService, IAuthorizationTokenGenerator tokenGenerator)
		{
			_authService = authService;
			_userService = userService;
			_tokenGenerator = tokenGenerator;
		}

		[HttpPost("register")]
		public async Task<ActionResult<LoginResultDto>> Register(RegisterDto registerDto)
		{
			var registerResult = await _authService.Register(registerDto.Login, registerDto.Password);

			if (!registerResult.Succeed)
			{
				return BadRequest();
			}

			await _userService.CreateUserAsync(new UserDto
			{
				UserName = registerDto.UserName
			});

			return await Login(registerDto.Login, registerDto.Password);
		}

		[HttpPost("login")]
		public async Task<ActionResult<LoginResultDto>> Login(LoginDto loginDto)
		{
			return await Login(loginDto.Login, loginDto.Password);
		}

		[AuthFilter]
		[HttpPost("verify-auth")]
		public void Verify()
		{
			//nothing to do here, checking user in auth filter
		}

		private async Task<ActionResult<LoginResultDto>> Login(string login, string password)
		{
			var loginResult = await _authService.Authenticate(login, password);

			if (!loginResult.Succeed)
			{
				return BadRequest();
			}

			var userDto = await _userService.GetUserAsync(loginResult.Credentials.UserId);
			var token = _tokenGenerator.GenerateToken(loginResult.Credentials);

			return new LoginResultDto
			{
				UserId = userDto.Id,
				UserName = userDto.UserName,
				Token = token
			};
		}
	}
}
