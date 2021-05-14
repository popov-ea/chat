using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;
using DataAccess.Interfaces;
using UseCases.Interfaces.Providers;

namespace UseCases.Implementation.Services
{
	public class MessageService
	{
		private readonly IRepository<Message> _messageRepository;
		private readonly IRepository<Attachment> _attachmentRepository;
		private readonly ITimeProvider _timeProvider;
		private readonly IAttachmentContentProvider _attachmentContentProvider;

		public MessageService(IRepository<Message> messageRepository, IRepository<Attachment> attachmentRepository, ITimeProvider timeProvider, IAttachmentContentProvider attachmentContentProvider)
		{
			_messageRepository = messageRepository;
			_attachmentRepository = attachmentRepository;
			_timeProvider = timeProvider;
			_attachmentContentProvider = attachmentContentProvider;
		}

		public async Task<MessageServiceResult> DeleteByIdsAsync(User actor, params long[] messageIds)
		{
			await _messageRepository.DeleteByIdsAsync(messageIds);
			var attachments = await GetMessagesAttachments(messageIds);
			await _attachmentRepository.DeleteByIdsAsync(attachments.Select(a => a.Id).ToArray());

			var methodResult = Ok(null);

			return methodResult;
		}

		public async Task<MessageServiceResult> SendMessageAsync(User sender, Conversation conversation, string messageText, Attachment[] attachments = null)
		{
			if (attachments != null)
			{
				try
				{
					var attachmentLoadAsync = attachments.Select(a => _attachmentContentProvider.DidUpload(a) ? Task.CompletedTask : _attachmentContentProvider.WaitForContentLoading(a));
					Task.WaitAll(attachmentLoadAsync.ToArray());
					await _attachmentRepository.CreateManyAsync(attachments);
				}
				catch
				{
					return Fail(MessageFailCauses.AttachmentSaveFailed);
				}
			}

			var newMsg = await _messageRepository.CreateAsync(new Message
			{
				Sender = sender,
				Conversation = conversation,
				Attachments = attachments.ToList(),
				CreatedAt = _timeProvider.NowUtc()
			});

			var methodResult = Ok(newMsg);

			return methodResult;
		}

		public async Task<IEnumerable<Message>> GetMessagesByConversationId(long conversationId)
		{
			return await _messageRepository.AllAsync((m) => m.ConversationId == conversationId);
		}

		private async Task<IEnumerable<Attachment>> GetMessagesAttachments(params long[] messageIds)
		{
			return await _attachmentRepository.AllAsync((a) => messageIds.Contains(a.MessageId));
		}

		private MessageServiceResult Ok(Message msg)
		{
			return new MessageServiceResult
			{
				Entity = msg,
				Success = true
			};
		}

		private MessageServiceResult Fail(MessageFailCauses failCause)
		{
			return new MessageServiceResult
			{
				FailCause = failCause,
				Success = false
			};
		}
	}
}
