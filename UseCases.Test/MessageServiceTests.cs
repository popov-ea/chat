using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Implementation.Services;
using Xunit;

namespace UseCases.Test
{
	class MessageServiceTests
	{
		public async Task ShouldSendMessage()
		{
			var blackListRepository = new TestRepository<BlackList>();
			var messageRepository = new TestRepository<Message>();
			var attachmentRepository = new TestRepository<Attachment>();
			var timeProvider = new TestTimeProvider();
			var attachmentContentProvider = new TestAttachmentContentProvider();
			var messageService = new MessageService(messageRepository, attachmentRepository, timeProvider, attachmentContentProvider);
			var sender = new User()
			{
				Id = 1
			};
			var conversation = new Conversation()
			{
				Id = 1
			};

			await messageService.SendMessageAsync(sender, conversation, "test", new Attachment[] { new Attachment { Id = 1, MessageId = 1 } });

			var allAdded = messageRepository.Count == 1
				&& attachmentRepository.Count == 1;

			Assert.True(allAdded);
		}

		public async Task ShouldDeleteMessage()
		{
			var blackListRepository = new TestRepository<BlackList>();
			var messageRepository = new TestRepository<Message>();
			var attachmentRepository = new TestRepository<Attachment>();
			var timeProvider = new TestTimeProvider();
			var attachmentContentProvider = new TestAttachmentContentProvider();
			var messageService = new MessageService(messageRepository, attachmentRepository, timeProvider, attachmentContentProvider);
			var sender = new User()
			{
				Id = 1
			};
			var conversation = new Conversation()
			{
				Id = 1
			};

			var result =  await messageService.SendMessageAsync(sender, conversation, "test", new Attachment[] { new Attachment { Id = 1, MessageId = 1 } });
			await messageService.DeleteByIdsAsync(sender, result.Entity.Id);

			var allDeleted = messageRepository.Count == 0
				&& attachmentRepository.Count == 0;

			Assert.True(allDeleted);
		}

		public async Task ShouldNotSendToBlockedUser()
		{
			var senderId = 1;
			var blockerId = 2;
			var blackListRepository = new TestRepository<BlackList>(new List<BlackList>() { new BlackList() { InitiatorId = blockerId, BlockedId = senderId } });
			var messageRepository = new TestRepository<Message>();
			var attachmentRepository = new TestRepository<Attachment>();
			var timeProvider = new TestTimeProvider();
			var attachmentContentProvider = new TestAttachmentContentProvider();
			var messageService = new MessageService(messageRepository, attachmentRepository, timeProvider, attachmentContentProvider);
			var sender = new User()
			{
				Id = senderId
			};
			var conversation = new Conversation()
			{
				Id = 1,
				Users = new List<ConversationUser>()
				{
					new ConversationUser()
					{
						ConversationId = 1,
						UserId = sender.Id
					},
					new ConversationUser()
					{
						ConversationId = 1,
						UserId = blockerId
					}
				}
			};

			await messageService.SendMessageAsync(sender, conversation, "test", new Attachment[] { new Attachment { Id = 1, MessageId = 1 } });

			var notSent = messageRepository.Count == 0
				&& attachmentRepository.Count == 0;

			Assert.True(notSent);
		}
	}
}
