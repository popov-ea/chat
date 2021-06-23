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

		
	}
}
