using Auth.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Server.Dtos;
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
		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			var registerResult = await _authService.Register(registerDto.Login, registerDto.Password);

			if (!registerResult.Succeed)
			{
				return BadRequest();
			}

			return await _userService.CreateUserAsync(registerDto.UserInfo);
		}

		[HttpPost("login")]
		public async Task<ActionResult<LoginResultDto>> Login(LoginDto loginDto)
		{
			var loginResult = await _authService.Authenticate(loginDto.Login, loginDto.Password);

			if (!loginResult.Succeed)
			{
				return BadRequest();
			}

			var userDto = await _userService.GetUserAsync(loginResult.Credentials.UserId);
			var token = _tokenGenerator.GenerateToken(loginResult.Credentials);

			return new LoginResultDto {
				UserDto = userDto,
				Token = token
			};
		}
	}
}
