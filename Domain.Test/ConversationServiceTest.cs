using Domain.Entities;
using Domain.Enums;
using Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Domain.Test
{
	public class ConversationServiceTest // ;(
	{
		private readonly TestTimeProvider _timeProvider = new TestTimeProvider();
		private readonly TestAttachmentContentProvider _attachmentContentProvider = new TestAttachmentContentProvider();

		[Fact]
		public async Task ShouldCreateConversationCorrectly()
		{
			var conversationRepository = new TestRepository<Conversation>();
			var conversationUserRepository = new TestRepository<ConversationUser>();
			var blackListRepository = new TestRepository<BlackList>();
			var chatActionRepository = new TestRepository<ChatAction>();
			var messageRepository = new TestRepository<Message>();
			var attachmentRepository = new TestRepository<Attachment>();
			var messageService = new MessageService(messageRepository, attachmentRepository, _timeProvider, _attachmentContentProvider);
			var blackListService = new BlackListService(blackListRepository);
			var conversationService = new ConversationService(conversationRepository, conversationUserRepository, chatActionRepository, blackListService, messageService, _timeProvider);
			var initiator = new User { Id = 1, Username = "name 1" };
			var invited = new List<User>
			{
				new User
				{
					Id = 2,
					Username = "name 2"
				},
				new User
				{
					Id = 3,
					Username = "name 3"
				}
			};

			var response = await conversationService.CreateConversationAsync(initiator, invited);
			var conversationCreated = conversationRepository.AnyAsync(c => c.Id == response.Entity.Id);
			var chatActionCreated = chatActionRepository.AnyAsync(c => c.Type == ChatActionType.NewChat && c.ConversationId == response.Entity.Id);
			var conversationUsersCreated = invited.Union(new User[] { initiator })
				.All(u => conversationUserRepository.Any(cu => cu.ConversationId == response.Entity.Id && cu.UserId == u.Id));

			var allCreated = (new bool[] { await conversationCreated, await chatActionCreated, conversationUsersCreated }).All(i => i == true);

			Assert.True(allCreated);	
		}

		[Fact]
		public async Task ShouldDeleteConversationCorrectly()
		{
			const long conversationId = 1;
			const long messageId = 1;

			var initiator = new User { Id = 1 };
			var invited = new List<User>
			{
				new User
				{
					Id = 2
				},
				new User
				{
					Id = 3
				}
			};

			var conversationUsers = invited.Union(new List<User> { initiator }).Select(u => new ConversationUser() { ConversationId = conversationId, UserId = u.Id }).ToList();

			var conversationUserRepository = new TestRepository<ConversationUser>(conversationUsers);
			var blackListRepository = new TestRepository<BlackList>();
			var chatActionRepository = new TestRepository<ChatAction>(new List<ChatAction>()
			{
				new ChatAction()
				{
					ConversationId = conversationId
				}
			});
			var messageRepository = new TestRepository<Message>(new List<Message>()
			{
				new Message()
				{
					Id = messageId,
					ConversationId = conversationId,
					SenderId = initiator.Id,
					Text = "test"
				}
			});;
			var attachmentRepository = new TestRepository<Attachment>(new List<Attachment>() {
				new Attachment
				{
					Id = 1,
					MessageId = messageId
				}
			});

			var blackListService = new BlackListService(blackListRepository);
			var messageService = new MessageService(messageRepository, attachmentRepository, _timeProvider, _attachmentContentProvider);

			var conversation = new Conversation
			{
				Id = conversationId,
				OwnerId = initiator.Id,
				Users = conversationUsers
			};
			var conversationRepository = new TestRepository<Conversation>(new List<Conversation>()
			{
				conversation
			});
			var conversationService = new ConversationService(conversationRepository, conversationUserRepository, chatActionRepository, blackListService, messageService, _timeProvider);

			await conversationService.DeleteConversationAsync(initiator, conversation);

			var allHistoryCleared = conversationRepository.Count == 0
				&& messageRepository.Count == 0
				&& attachmentRepository.Count == 0
				&& conversationUserRepository.Count == 0;

			Assert.True(allHistoryCleared);
		}
	}
}
