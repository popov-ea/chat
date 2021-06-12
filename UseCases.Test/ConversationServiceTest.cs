using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCases.Implementations.Services;
using UseCases.Implementations;
using UseCases.Interfaces.Dtos;
using Xunit;

namespace UseCases.Test
{
	public class ConversationServiceTest // ;(
	{
		private readonly TestTimeProvider _timeProvider = new TestTimeProvider();
		private readonly TestAttachmentContentProvider _attachmentContentProvider = new TestAttachmentContentProvider();
		private readonly Mapper _mapper = MapperHelpers.GetConfiguredMapper();

		[Fact]
		public async Task ShouldCreateConversationCorrectly()
		{
			var firedEventsCount = 0;
			var conversationRepository = new TestRepository<Conversation>();
			var conversationUserRepository = new TestRepository<ConversationUser>();
			var blackListRepository = new TestRepository<BlackList>();
			var chatActionRepository = new TestRepository<ChatAction>();
			var messageRepository = new TestRepository<Message>();
			var attachmentRepository = new TestRepository<Attachment>();

			var blackListService = new BlackListService(blackListRepository, _mapper);
			var conversationUserService = new ConversationUserService(conversationUserRepository, _mapper);


			var messageService = new MessageService(messageRepository, attachmentRepository, _timeProvider, _attachmentContentProvider, blackListService, conversationUserService, _mapper);
			var conversationService = new ConversationService(conversationRepository, conversationUserRepository, chatActionRepository, blackListService, messageService, _timeProvider, _mapper);

			conversationService.OnConversationCreated += (conversationService) => firedEventsCount++;

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

			var response = await conversationService.CreateConversationAsync(initiator.Id, invited.Select(u => u.Id));

			Assert.Contains(conversationRepository.All(), c => c.Id == response.Entity.Id);
			Assert.Contains(chatActionRepository.All(), c => c.Type == ChatActionType.NewChat && c.ConversationId == response.Entity.Id);
			Assert.Contains(invited.Union(new User[] { initiator }), u => conversationUserRepository.Any(cu => cu.ConversationId == response.Entity.Id && cu.UserId == u.Id));
			Assert.Equal(1, firedEventsCount);
		}

		[Fact]
		public async Task ShouldDeleteConversationCorrectly()
		{
			const long conversationId = 1;
			const long messageId = 1;

			var firedEventsCount = 0;

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

			var blackListService = new BlackListService(blackListRepository, _mapper);
			var conversationUserService = new ConversationUserService(conversationUserRepository, _mapper);


			var messageService = new MessageService(messageRepository, attachmentRepository, _timeProvider, _attachmentContentProvider, blackListService, conversationUserService, _mapper);

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
			var conversationService = new ConversationService(conversationRepository, conversationUserRepository, chatActionRepository, blackListService, messageService, _timeProvider, _mapper);

			conversationService.OnConversationDeleted += (c) => firedEventsCount++;

			await conversationService.DeleteConversationAsync(initiator.Id, conversation.Id);

			Assert.Empty(conversationRepository.All());
			Assert.Empty(messageRepository.All());
			Assert.Empty(attachmentRepository.All());
			Assert.Empty(conversationUserRepository.All());
			Assert.Equal(1, firedEventsCount);
		}
	}
}
