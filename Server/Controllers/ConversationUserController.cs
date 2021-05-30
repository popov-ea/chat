using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;
using UseCases.Interfaces.Dtos.Results;
using UseCases.Interfaces.Services;

namespace Server.Controllers
{
	[ApiController]
	[Route("conversation-user")]
	class ConversationUserController : ControllerBase
	{
		private readonly IConversationUserService _conversationUserService;

		public ConversationUserController(IConversationUserService conversationService)
		{
			_conversationUserService = conversationService;
		}

		[HttpGet()]
		public async Task<ActionResult<ConversationUserDto>> CreateConversationUser(long conversationId, long userId)
		{
			var result = await _conversationUserService.AddUserAsync(conversationId, userId);
			return HandleConversationUserServiceResult(result);
		}

		[HttpDelete("{conversationId}/{userId}")]
		public async Task<ActionResult<ConversationUserDto>> DeleteConversationUserDto(long conversationId, long userId)
		{
			var result = await _conversationUserService.DeleteUserAsync(conversationId, userId);
			return HandleConversationUserServiceResult(result);
		}

		[HttpDelete("{conversationUserId}")]
		public async Task<ActionResult<ConversationUserDto>> DeleteConversationUserDto(long conversationUserId)
		{
			var result = await _conversationUserService.DeleteUserAsync(conversationUserId);
			return HandleConversationUserServiceResult(result);
		}

		private ActionResult<ConversationUserDto> HandleConversationUserServiceResult(ConversationUserServiceResultDto result) => result.Success
			? result.Entity
			: HandleFail(result);

		private ActionResult<ConversationUserDto> HandleFail(ConversationUserServiceResultDto result) => result.FailCause switch
		{
			ConversationUserServiceFailCauses.NoUsersFound => NotFound(),
			_ => new StatusCodeResult(500)
		};
	}
}
