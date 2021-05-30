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
using AutoMapper;
using UseCases.Interfaces.Services;

namespace UseCases.Implementation.Services
{
	public class MessageService : IMessageService
	{
		private readonly IRepository<Message> _messageRepository;
		private readonly IRepository<Attachment> _attachmentRepository;
		private readonly ITimeProvider _timeProvider;
		private readonly IAttachmentContentProvider _attachmentContentProvider;
		private readonly Mapper _mapper;

		public MessageService(IRepository<Message> messageRepository, IRepository<Attachment> attachmentRepository, ITimeProvider timeProvider,  
			IAttachmentContentProvider attachmentContentProvider, Mapper mapper)
		{
			_messageRepository = messageRepository;
			_attachmentRepository = attachmentRepository;
			_timeProvider = timeProvider;
			_attachmentContentProvider = attachmentContentProvider;
			_mapper = mapper;
		}

		public async Task<MessageServiceResultDto> DeleteByIdsAsync(long actorId, params long[] messageIds)
		{
			await _messageRepository.DeleteByIdsAsync(messageIds);
			var attachments = await GetMessagesAttachments(messageIds);
			await _attachmentRepository.DeleteByIdsAsync(attachments.Select(a => a.Id).ToArray());

			var methodResult = Ok(null);

			return methodResult;
		}

		public async Task<MessageServiceResultDto> SendMessageAsync(long senderId, long conversationId, string messageText, AttachmentDto[] attachments = null)
		{
			var createdAttachments = new List<Attachment>();
			if (attachments != null)
			{
				try
				{
					var attachmentLoadAsync = attachments.Select(a => _attachmentContentProvider.DidUpload(a) ? Task.CompletedTask : _attachmentContentProvider.WaitForContentLoading(a));
					Task.WaitAll(attachmentLoadAsync.ToArray());
					createdAttachments = (await _attachmentRepository.CreateManyAsync(
						attachments.Select((a) => _mapper.Map<AttachmentDto, Attachment>(a))
					)).ToList();
				}
				catch
				{
					return Fail(MessageFailCauses.AttachmentSaveFailed);
				}
			}

			var newMsg = await _messageRepository.CreateAsync(new Message
			{
				SenderId = senderId,
				ConversationId = conversationId,
				Attachments = createdAttachments,
				CreatedAt = _timeProvider.NowUtc()
			});

			var methodResult = Ok(newMsg);

			return methodResult;
		}

		public async Task<IEnumerable<MessageDto>> GetMessagesByConversationId(long conversationId)
		{
			var messages = await _messageRepository.AllAsync((m) => m.ConversationId == conversationId);
			return messages.Select(m => _mapper.Map<Message, MessageDto>(m));
		}

		private async Task<IEnumerable<Attachment>> GetMessagesAttachments(params long[] messageIds)
		{
			return await _attachmentRepository.AllAsync((a) => messageIds.Contains(a.MessageId));
		}

		private MessageServiceResultDto Ok(Message msg)
		{
			return new MessageServiceResultDto
			{
				Entity = _mapper.Map<Message, MessageDto>(msg),
				Success = true
			};
		}

		private MessageServiceResultDto Fail(MessageFailCauses failCause)
		{
			return new MessageServiceResultDto
			{
				FailCause = failCause,
				Success = false
			};
		}
	}
}
