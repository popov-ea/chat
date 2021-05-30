using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;
using UseCases.Interfaces.Services;

namespace Server.Controllers
{
	[ApiController]
	[Route("conversation")]
	public class ConversationController : ControllerBase
	{
		private readonly IConversationService _conversationService;

		public ConversationController(IConversationService conversationService)
		{
			_conversationService = conversationService;
		}

		[HttpGet("{conversationId}")]
		public async Task<ConversationDto> GetConversation(long conversationId) 
		{
			return await _conversationService.GetConversationAsync(conversationId);
		}

		[HttpPost]
		public async Task<ActionResult<ConversationDto>> CreateConversationAsync(ConversationDto conversationDto)
		{
			var creationResult = await _conversationService.CreateConversationAsync(conversationDto.Owner.Id, conversationDto.Users.Select(u => u.Id), conversationDto.Name);
			return HandleConversationServiceResult(creationResult);
		}

		[HttpPut("{toUpdateId}")]
		public async Task<ActionResult<ConversationDto>> UpdateConversation(long toUpdateId, ConversationDto newData)
		{
			var updateResult = await _conversationService.UpdateConversationAsync(toUpdateId, newData);
			return HandleConversationServiceResult(updateResult);
		}

		[HttpDelete("{conversationId}")]
		public async Task<ActionResult<ConversationDto>> Delete(long conversationId)
		{
			//TODO: сделай норм после создания всей хуйни для авторизации
			var userId = 0;
			var deleteResult = await _conversationService.DeleteConversationAsync(conversationId, userId);
			return HandleConversationServiceResult(deleteResult);
		}

		private ActionResult<ConversationDto> HandleConversationServiceResult(ConversationServiceResultDto result) => result.Success
			? result.Entity
			: HandleFail(result);

		private ActionResult<ConversationDto> HandleFail(ConversationServiceResultDto failResult) => failResult.FailCause switch
		{
			ConversationFailCause.BlockedUser => new ForbidResult(),
			ConversationFailCause.NoPermissions => new ForbidResult(),
			ConversationFailCause.NoUsersInvited => new BadRequestResult(),
			_ => new StatusCodeResult(500)
		};
	}
}
