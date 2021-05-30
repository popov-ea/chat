using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using UseCases.Interfaces.Providers;
using UseCases.Implementation.Services;
using UseCases.Interfaces.Dtos;
using AutoMapper;
using UseCases.Interfaces.Services;

namespace UseCases.Implementation.Services
{
	public class ConversationService : IConversationService
	{
		private readonly IRepository<Conversation> _conversationRepository;
		private readonly IRepository<ChatAction> _chatActionRepository;
		private readonly IRepository<ConversationUser> _conversationUserRepository;
		private readonly ITimeProvider _timeProvider;
		private readonly IMessageService _messageService;
		private readonly IBlackListService _blackListService;
		private readonly Mapper _mapper;

		// :/
		public ConversationService(IRepository<Conversation> conversationRepository, IRepository<ConversationUser> conversationUserRepository,
			IRepository<ChatAction> chatActionRepository, BlackListService blackListService, MessageService messageService, ITimeProvider timeProvider, Mapper mapper)
		{
			_conversationRepository = conversationRepository;
			_chatActionRepository = chatActionRepository;
			_timeProvider = timeProvider;
			_messageService = messageService;
			_conversationUserRepository = conversationUserRepository;
			_blackListService = blackListService;
			_mapper = mapper;
		}

		public async Task<ConversationServiceResultDto> CreateConversationAsync(long initiatorId, IEnumerable<long> invitedUserIds, string name = null)
		{
			var allUserIds = invitedUserIds.Union(new[] { initiatorId });

			if (invitedUserIds.Count() == 0)
			{
				return Fail(ConversationFailCause.NoUsersInvited);
			}

			if (await DidAnyoneBlock(invitedUserIds, initiatorId))
			{
				return Fail(ConversationFailCause.BlockedUser);
			}

			var conversation = await _conversationRepository.CreateAsync(
				new Conversation { Name = name, OwnerId = initiatorId }
			);

			await _conversationUserRepository.CreateManyAsync(
				allUserIds.Select(u => new ConversationUser { Conversation = conversation, UserId = u })
			);
			await _chatActionRepository.CreateAsync(new ChatAction { InitiatorId = initiatorId, CreatedAt = _timeProvider.NowUtc(), Type = ChatActionType.NewChat });

			var methodResult = Ok(conversation);

			return methodResult;
		}

		public async Task<ConversationServiceResultDto> ClearConversationAsync(long actorId, long conversationId)
		{
			if (!await IsOwner(conversationId, actorId))
			{
				return Fail(ConversationFailCause.NoPermissions);
			}

			var chatActionsAsync = _chatActionRepository.AllAsync(a => a.ConversationId == conversationId);
			var chatMessages = _messageService.GetMessagesByConversationId(conversationId);

			await _chatActionRepository.DeleteByIdsAsync((await chatActionsAsync).Select(a => a.Id).ToArray());
			await _messageService.DeleteByIdsAsync(actorId, (await chatMessages).Select(a => a.Id).ToArray());

			return Ok(await _conversationRepository.FindAsync(conversationId));
		}

		private ConversationServiceResultDto Fail(ConversationFailCause failCause)
		{
			return new ConversationServiceResultDto
			{
				Success = false,
				FailCause = failCause
			};
		}

		private ConversationServiceResultDto Ok(Conversation conversation)
		{
			return new ConversationServiceResultDto
			{
				Success = true,
				Entity = MapToDto(conversation)
			};
		}

		private async Task<bool> DidAnyoneBlock(IEnumerable<long> userIds, long maybeBlockedId)
		{
			return await _blackListService.CheckAnyBlocked(maybeBlockedId, userIds);
		}

		private async Task<bool> IsOwner(long conversationId, long maybeOwnerId)
		{
			var conversation = await _conversationRepository.FindAsync(conversationId);
			return conversation.OwnerId == maybeOwnerId;
		}

		public async Task<IEnumerable<ConversationDto>> GetAllConversationsAsync()
		{
			var allConversations = await _conversationRepository.AllAsync();
			return allConversations.Select((c) => MapToDto(c));
		}

		private ConversationDto MapToDto(Conversation conversation)
		{
			return _mapper.Map<Conversation, ConversationDto>(conversation);
		}

		public async Task<ConversationDto> GetConversationAsync(long conversationId)
		{
			var conversation = await _conversationRepository.FindAsync(conversationId);
			return MapToDto(conversation);
		}

		public async Task<ConversationServiceResultDto> UpdateConversationAsync(long conversationId, ConversationDto newData)
		{
			var conversation = await _conversationRepository.FindAsync(conversationId);
			conversation.Name = newData.Name;
			var updated = await _conversationRepository.UpdateAsync(conversation);
			return Ok(updated);
		}

		public async Task<ConversationServiceResultDto> DeleteConversationAsync(long actorId, long conversationId)
		{
			if (!await IsOwner(conversationId, actorId))
			{
				return Fail(ConversationFailCause.NoPermissions);
			}

			ClearConversationAsync(actorId, conversationId);

			var conversationUsers = _conversationUserRepository.AllAsync(cu => cu.ConversationId == conversationId);
			await _conversationUserRepository.DeleteByIdsAsync((await conversationUsers).Select(c => c.Id).ToArray());

			var deleted = await _conversationRepository.DeleteByIdsAsync(conversationId);

			return Ok(deleted.FirstOrDefault());
		}
	}
}
