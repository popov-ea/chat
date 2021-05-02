using Domain.Adapters.Providers;
using Domain.Adapters.Repositories;
using Domain.Entities;
using Domain.Enums;
using Domain.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services
{
	public class ConversationService
	{
		private readonly IRepository<Conversation> _conversationRepository;
		private readonly IRepository<ConversationUser> _conversationUserRepository;
		private readonly IRepository<BlackList> _blackListRepository;
		private readonly IRepository<ChatAction> _chatActionRepository;
		private readonly ITimeProvider _timeProvider;
		private readonly MessageService _messageService;

		// :/
		public ConversationService(IRepository<Conversation> conversationRepository, IRepository<ConversationUser> conversationUserRepository, 
			IRepository<BlackList> blackListrepository, IRepository<ChatAction> chatActionRepository,
			MessageService messageService, ITimeProvider timeProvider)
		{
			_conversationRepository = conversationRepository;
			_conversationUserRepository = conversationUserRepository;
			_blackListRepository = blackListrepository;
			_chatActionRepository = chatActionRepository;
			_timeProvider = timeProvider;
			_messageService = messageService;
		}

		public async Task<ConversationServiceResult> CreateConversation(User initiator, IEnumerable<User> invitedUsers, string name = null)
		{
			var allUsers = invitedUsers.Union(new[] { initiator });

			if (invitedUsers.Count() == 0)
			{
				return Fail(ConversationFailCause.NoUsersInvited);
			}

			if (DidAnyoneBlock(invitedUsers, initiator))
			{
				return Fail(ConversationFailCause.BlockedUser);
			}

			if (name == null)
			{
				name = GenerateConversationName(allUsers);
			}

			var conversation = await _conversationRepository.CreateAsync(new Conversation { Name = name, Owner = initiator });

			await _conversationUserRepository.CreateManyAsync(allUsers.Select(u => new ConversationUser { Conversation = conversation, User = u }));
			await _chatActionRepository.CreateAsync(new ChatAction { Initiator = initiator, CreatedAt = _timeProvider.NowUtc(), Type = ChatActionType.NewChat });

			return Ok(conversation);
		}

		public async Task<ConversationServiceResult> DeleteConversation(Conversation conversation)
		{
			ClearConversation(conversation);

			var conversationUsers = _conversationUserRepository.AllAsync(cu => cu.ConversationId == conversation.Id);
			var chatActionsAsync =  _chatActionRepository.AllAsync(a => a.ConversationId == conversation.Id);

			await _conversationUserRepository.DeleteByIdsAsync((await conversationUsers).Select(c => c.Id).ToArray());
			await _chatActionRepository.DeleteByIdsAsync((await chatActionsAsync).Select(a => a.Id).ToArray());

			var deleted = await _conversationRepository.DeleteAsync(conversation);

			return Ok(deleted);
		}

		public async Task<ConversationServiceResult> ClearConversation(Conversation conversation)
		{
			await _messageService.DeleteAsync(conversation.Messages.Select(m => m.Id).ToArray());
			return Ok(await _conversationRepository.FindAsync(conversation.Id));
		}

		private string GenerateConversationName(IEnumerable<User> users)
		{
			return string.Concat(users.Select(u => u.Username), " ");
		}

		private ConversationServiceResult Fail(ConversationFailCause failCause)
		{
			return new ConversationServiceResult
			{
				Success = false,
				FailCause = failCause
			};
		}

		private ConversationServiceResult Ok(Conversation conversation)
		{
			return new ConversationServiceResult
			{
				Success = true,
				Entity = conversation
			};
		}

		private bool DidAnyoneBlock(IEnumerable<User> users, User maybeBlocked)
		{
			var usersIds = users.Select(u => u.Id);
			return _blackListRepository.Any(bl => bl.BlockedId == maybeBlocked.Id && usersIds.Contains(bl.InitiatorId));
		}
	}
}
