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

namespace UseCases.Implementations.Services
{
	public class MessageService : IMessageService
	{
		private readonly IRepository<Message> _messageRepository;
		private readonly IRepository<Attachment> _attachmentRepository;
		private readonly ITimeProvider _timeProvider;
		private readonly IAttachmentContentProvider _attachmentContentProvider;
		private readonly IBlackListService _blackListService;
		private readonly IConversationUserService _conversationUserService;
		private readonly Mapper _mapper;

		public event Action<MessageDto> OnMessageSent;
		public event Action<MessageDto[]> OnMessagesDeleted;

		public MessageService(IRepository<Message> messageRepository, IRepository<Attachment> attachmentRepository, ITimeProvider timeProvider,  
			IAttachmentContentProvider attachmentContentProvider, IBlackListService blackListService, IConversationUserService conversationUserService,
			Mapper mapper)
		{
			_messageRepository = messageRepository;
			_attachmentRepository = attachmentRepository;
			_timeProvider = timeProvider;
			_attachmentContentProvider = attachmentContentProvider;
			_blackListService = blackListService;
			_conversationUserService = conversationUserService;
			_mapper = mapper;
			// null obj pattern again
			InitEmptyEventHandlers();
		}

		private void InitEmptyEventHandlers()
		{
			OnMessageSent += (x) => { };
			OnMessagesDeleted += (x) => { };
		}

		public async Task<MessageServiceResultDto> DeleteByIdsAsync(long actorId, params long[] messageIds)
		{
			var deleted = await _messageRepository.DeleteByIdsAsync(messageIds);
			var attachments = await GetMessagesAttachments(messageIds);
			await _attachmentRepository.DeleteByIdsAsync(attachments.Select(a => a.Id).ToArray());

			OnMessagesDeleted(deleted.Select((m) => MapToDto(m)).ToArray());

			var methodResult = Ok(null);

			return methodResult;
		}

		public async Task<MessageServiceResultDto> SendMessageAsync(long senderId, long conversationId, string messageText, AttachmentDto[] attachments = null)
		{
			var conversationUsersIds = (await _conversationUserService.GetConversationUsers(conversationId)).Select((cu) => cu.UserId).ToList();

			if (conversationUsersIds.Count > 0 && await _blackListService.CheckAnyBlocked(senderId, conversationUsersIds))
			{
				return Fail(MessageFailCauses.UserBlocked);
			}

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

			OnMessageSent(MapToDto(newMsg));

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
				Entity = MapToDto(msg),
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

		private MessageDto MapToDto(Message msg)
		{
			return _mapper.Map<Message, MessageDto>(msg);
		}
	}
}
