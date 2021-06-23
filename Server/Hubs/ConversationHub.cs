using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;
using UseCases.Interfaces.Services;

namespace Server.Hubs
{
	public class ConversationHub : Hub
	{
		private readonly IConversationService _conversationService;
		private readonly IMessageService _messageService;
		private readonly IConversationUserService _conversationUserService;
		private static readonly ConnectionMapping<long> _connectionMapping = new ConnectionMapping<long>();

		public ConversationHub(IConversationService conversationService,
			IMessageService messageService,
			IConversationUserService conversationUserService)
		{
			_conversationService = conversationService;
			_messageService = messageService;
			_conversationUserService = conversationUserService;
		}

		public async Task Enter(string userId)
		{
			if (long.TryParse(userId, out long parsedUserId))
			{
				var conversationIds = await _conversationService.GetUserConversationIdsAsync(parsedUserId);
				_connectionMapping.Add(parsedUserId, Context.ConnectionId);
				foreach (var convId in conversationIds)
				{
					await Groups.AddToGroupAsync(Context.ConnectionId, convId.ToString());
				}
			}
		}

		public async Task SendMessage(MessageDto message)
		{
			var result = await _messageService.SendMessageAsync(message.SenderId, message.ConversationId, message.Text, message.Attachments.ToArray());

			if (result.Success)
			{
				await Clients.Group(message.ConversationId.ToString()).SendAsync("messageRecieved", result.Entity);
				return;
			}

			await Clients.Caller.SendAsync("messsageSendingError", result.FailCause);
		}

		public async Task DeleteMessages(long[] messageIds, long conversationId)
		{
			var user = (Identity.UserIdentity)Context.User.Identity;
			var result = await _messageService.DeleteByIdsAsync(user.UserId, messageIds);

			if (result.Success)
			{
				await Clients.Group(conversationId.ToString()).SendAsync("messagesDeleted", messageIds);
				return;
			}

			await Clients.Caller.SendAsync("messageDeletionError", result.FailCause);
		}

		public async Task DeleteUser(ConversationUserDto conversationUser)
		{
			var result = await _conversationUserService.DeleteUserAsync(conversationUser.Id);

			if (result.Success)
			{
				await Clients.Group(conversationUser.ConversationId.ToString()).SendAsync("userDeletedFromConversation", result.Entity);
				return;
			}

			await Clients.Caller.SendAsync("userDeletionError", result.FailCause);
		}

		public async Task AddUser(ConversationUserDto conversationUser)
		{
			var result = await _conversationUserService.AddUserAsync(conversationUser.ConversationId, conversationUser.UserId);

			if (result.Success)
			{
				await Clients.Group(conversationUser.ConversationId.ToString()).SendAsync("userAddedToConversation", result.Entity);
				return;
			}

			await Clients.Caller.SendAsync("userAddError", result.FailCause);
		}

		public async Task CreateConversation(ConversationDto conversationDto)
		{
			var user = (Identity.UserIdentity)Context.User.Identity;
			var result = await _conversationService.CreateConversationAsync(user.UserId, conversationDto.Users.Select(u => u.Id), conversationDto.Name);

			if (result.Success)
			{
				var conversationUsers = result.Entity.Users;
				foreach (var cu in conversationUsers)
				{
					var connections = _connectionMapping.GetConnections(cu.Id);
					var tasks = new List<Task>();
					foreach (var connection in connections)
					{
						tasks.Add(Clients.Client(connection).SendAsync("conversationCreated", result.Entity));
					}
					Task.WaitAll(tasks.ToArray());
				}
				return;
			}

			await Clients.Caller.SendAsync("conversationCreationError", result.FailCause);
		}

		public async Task UpdateConversation(ConversationDto conversation)
		{
			var result = await _conversationService.UpdateConversationAsync(conversation.Id, conversation);

			if (result.Success)
			{
				await Clients.Group(conversation.Id.ToString()).SendAsync("conversationUpdated", conversation);
				return;
			}

			await Clients.Caller.SendAsync("conversationUpdateError", result.FailCause);
		}

		public async Task DeleteConversation(ConversationDto conversation)
		{
			var user = (Identity.UserIdentity)Context.User.Identity;
			var result = await _conversationService.DeleteConversationAsync(user.UserId, conversation.Id);

			if (result.Success)
			{
				await Clients.Group(conversation.Id.ToString()).SendAsync("conversationUpdated", conversation);
				return;
			}

			await Clients.Caller.SendAsync("conversationDeleteError", result.FailCause);
		}

		public async Task ClearConversation(ConversationDto conversation)
		{
			var user = (Identity.UserIdentity)Context.User.Identity;
			var result = await _conversationService.ClearConversationAsync(user.UserId, conversation.Id);

			if (result.Success)
			{
				await Clients.Group(conversation.Id.ToString()).SendAsync("conversationCleared", conversation);
				return;
			}

			await Clients.Caller.SendAsync("conversationClearError", result.FailCause);
		}
	}
}
