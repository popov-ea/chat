﻿using Domain.Entities;
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
		private readonly MessageService _messageService;
		private readonly BlackListService _blackListService;
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

		public async Task<ConversationServiceResultDto> CreateConversationAsync(UserDto initiator, IEnumerable<UserDto> invitedUsers, string name = null)
		{
			var allUsers = invitedUsers.Union(new[] { initiator });

			if (invitedUsers.Count() == 0)
			{
				return Fail(ConversationFailCause.NoUsersInvited);
			}

			if (await DidAnyoneBlock(invitedUsers, initiator))
			{
				return Fail(ConversationFailCause.BlockedUser);
			}

			if (name == null)
			{
				name = GenerateConversationName(allUsers);
			}

			var conversation = await _conversationRepository.CreateAsync(
				new Conversation { Name = name, Owner = _mapper.Map<UserDto,User>(initiator), OwnerId = initiator.Id }
			);

			await _conversationUserRepository.CreateManyAsync(
				allUsers.Select(u => new ConversationUser { Conversation = conversation, User = _mapper.Map<UserDto, User>(u), UserId = u.Id })
			);
			await _chatActionRepository.CreateAsync(new ChatAction { InitiatorId = initiator.Id, CreatedAt = _timeProvider.NowUtc(), Type = ChatActionType.NewChat });

			var methodResult = Ok(conversation);

			return methodResult;
		}

		public async Task<ConversationServiceResultDto> DeleteConversationAsync(UserDto actor, ConversationDto conversation)
		{
			if (!IsOwner(conversation, actor))
			{
				return Fail(ConversationFailCause.NoPermissions);
			}
			
			ClearConversationAsync(actor, conversation);

			var conversationUsers = _conversationUserRepository.AllAsync(cu => cu.ConversationId == conversation.Id);
			var chatActionsAsync =  _chatActionRepository.AllAsync(a => a.ConversationId == conversation.Id);
			var chatMessages = _messageService.GetMessagesByConversationId(conversation.Id);

			await _conversationUserRepository.DeleteByIdsAsync((await conversationUsers).Select(c => c.Id).ToArray());
			await _chatActionRepository.DeleteByIdsAsync((await chatActionsAsync).Select(a => a.Id).ToArray());
			await _messageService.DeleteByIdsAsync(actor, (await chatMessages).Select(a => a.Id).ToArray());

			var deleted = await _conversationRepository.DeleteByIdsAsync(conversation.Id);

			var methodResult = Ok(deleted.FirstOrDefault());

			return methodResult;
		}

		public async Task<ConversationServiceResultDto> ClearConversationAsync(UserDto actor, ConversationDto conversation)
		{
			if (!IsOwner(conversation, actor))
			{
				return Fail(ConversationFailCause.NoPermissions);
			}
			await _messageService.DeleteByIdsAsync(actor, conversation.Messages.Select(m => m.Id).ToArray());

			var methodResult = Ok(await _conversationRepository.FindAsync(conversation.Id));

			return Ok(await _conversationRepository.FindAsync(conversation.Id));
		}

		private string GenerateConversationName(IEnumerable<UserDto> users)
		{
			return string.Concat(users.Select(u => u.Username ?? ""), " ");
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
				Entity = _mapper.Map<Conversation, ConversationDto>(conversation)
			};
		}

		private async Task<bool> DidAnyoneBlock(IEnumerable<UserDto> users, UserDto maybeBlocked)
		{
			return await _blackListService.CheckAnyBlocked(maybeBlocked, users);
		}

		private bool IsOwner(ConversationDto conversation, UserDto maybeOwner)
		{
			return conversation.OwnerId == maybeOwner.Id;
		}
	}
}
