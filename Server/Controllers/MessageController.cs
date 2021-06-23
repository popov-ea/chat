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
	}
}
