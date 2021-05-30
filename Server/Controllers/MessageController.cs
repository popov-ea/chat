using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;
using UseCases.Interfaces.Services;

namespace Server.Controllers
{
	[ApiController]
	[Route("message")]
	public class MessageController : ControllerBase
	{
		private readonly IMessageService _messageService;
		public MessageController(IMessageService messageService)
		{
			_messageService = messageService;
		}

		//TODO: paging
		[HttpGet]
		public Task<IEnumerable<MessageDto>> GetForConversation(long conversationId)
		{
			return _messageService.GetMessagesByConversationId(conversationId);
		}

		[HttpPost]
		public async Task<ActionResult<MessageDto>> SendMessage(long conversationId, long senderId, string messageText, AttachmentDto[] attachments)
		{
			var result = await _messageService.SendMessageAsync(senderId, conversationId, messageText, attachments);
			return HandleMessageServiceResult(result);
		}

		[HttpDelete]
		public async Task<ActionResult<MessageDto>> DeleteMessage(long messageId)
		{
			//TODO: сделай норм
			var userId = 0;
			var result = await _messageService.DeleteByIdsAsync(userId, messageId);
			return HandleMessageServiceResult(result);
		}

		private ActionResult<MessageDto> HandleMessageServiceResult(MessageServiceResultDto result) => result.Success
			? result.Entity
			: HanldeFail(result);
		private ActionResult<MessageDto> HanldeFail(MessageServiceResultDto result) => new StatusCodeResult(500);
	}
}
