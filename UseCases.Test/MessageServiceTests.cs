using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Implementations;
using UseCases.Implementations.Services;
using UseCases.Interfaces.Dtos;
using Xunit;

namespace UseCases.Test
{
	public class MessageServiceTests
	{
		private readonly Mapper _mapper;
		public MessageServiceTests()
		{
			_mapper = MapperHelpers.GetConfiguredMapper();
		}

		[Fact]
		public async Task ShouldSendMessage()
		{
			var blackListRepository = new TestRepository<BlackList>();
			var messageRepository = new TestRepository<Message>();
			var conversationUserRepository = new TestRepository<ConversationUser>();
			var attachmentRepository = new TestRepository<Attachment>();
			var timeProvider = new TestTimeProvider();
			var attachmentContentProvider = new TestAttachmentContentProvider();

			var blackListService = new BlackListService(blackListRepository, _mapper);
			var conversationUserService = new ConversationUserService(conversationUserRepository, _mapper);

			var messageService = new MessageService(messageRepository, attachmentRepository, timeProvider, attachmentContentProvider, blackListService, conversationUserService, _mapper);
			var sender = new User()
			{
				Id = 1
			};
			var conversation = new Conversation()
			{
				Id = 1
			};

			await messageService.SendMessageAsync(sender.Id, conversation.Id, "test", new AttachmentDto[] { new AttachmentDto { Id = 1, MessageId = 1 } });

			var allAdded = messageRepository.Count == 1
				&& attachmentRepository.Count == 1;

			Assert.True(allAdded);
		}

		[Fact]
		public async Task ShouldDeleteMessage()
		{
			var blackListRepository = new TestRepository<BlackList>();
			var messageRepository = new TestRepository<Message>();
			var conversationRepository = new TestRepository<Conversation>();
			var conversationUserRepository = new TestRepository<ConversationUser>();
			var attachmentRepository = new TestRepository<Attachment>();
			var timeProvider = new TestTimeProvider();
			var attachmentContentProvider = new TestAttachmentContentProvider();
			var conversationUserService = new ConversationUserService(conversationUserRepository, _mapper);
			var blackListService = new BlackListService(blackListRepository, _mapper);
			var messageService = new MessageService(messageRepository, attachmentRepository, timeProvider, attachmentContentProvider, blackListService, conversationUserService, _mapper);
			var sender = new User()
			{
				Id = 1
			};
			var conversation = new Conversation()
			{
				Id = 1
			};

			var result = await messageService.SendMessageAsync(sender.Id,
				conversation.Id, "test", new AttachmentDto[] { new AttachmentDto { Id = 1, MessageId = 0 } });
			await messageService.DeleteByIdsAsync(sender.Id, result.Entity.Id);

			Assert.Empty(messageRepository.All());
			Assert.Empty(attachmentRepository.All());
		}

		[Fact]
		public async Task ShouldNotSendToBlockedUser()
		{
			var senderId = 1;
			var blockerId = 2;
			var conversationId = 1;
			var sender = new User()
			{
				Id = senderId
			};
			var conversation = new Conversation()
			{
				Id = conversationId
			};
			var conversationUsers = new List<ConversationUser>()
			{
				new ConversationUser()
				{
					ConversationId = conversationId,
					UserId = senderId
				},
				new ConversationUser()
				{
					ConversationId = conversationId,
					UserId = blockerId
				}
			};

			var blackListRepository = new TestRepository<BlackList>(new List<BlackList>() { new BlackList() { InitiatorId = blockerId, BlockedId = senderId } });
			var messageRepository = new TestRepository<Message>();
			var attachmentRepository = new TestRepository<Attachment>();

			var timeProvider = new TestTimeProvider();
			var attachmentContentProvider = new TestAttachmentContentProvider();

			var blackListService = new BlackListService(blackListRepository, _mapper);			
			var conversationUserService = new ConversationUserService(new TestRepository<ConversationUser>(conversationUsers), _mapper);

			var messageService = new MessageService(messageRepository, 
				attachmentRepository, 
				timeProvider, 
				attachmentContentProvider, 
				blackListService, 
				conversationUserService, 
				_mapper);
			

			await messageService.SendMessageAsync(sender.Id, conversation.Id, "test", new AttachmentDto[] { new AttachmentDto { Id = 1, MessageId = 1 } });

			Assert.Empty(messageRepository.All());
			Assert.Empty(attachmentRepository.All());
		}
	}
}
