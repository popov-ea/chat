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

		public async Task<MessageServiceResult> DeleteAsync(params long[] messageIds)
		{
			await _messageRepository.DeleteByIdsAsync(messageIds);
			return Ok(null); // :(
		}

		public async Task<MessageServiceResult> SendMessage(User sender, Conversation conversation, string messageText, Attachment[] attachments = new Attachment[])
		{
			var newMsgAsync = _messageRepository.CreateAsync(new Message 
			{
				Sender = sender, 
				Conversation = conversation, 
				Attachments = attachments.ToList(), 
				CreatedAt = _timeProvider.NowUtc() 
			});

			try
			{
				var attachmentLoadAsync = attachments.Select(a => _attachmentContentProvider.DidUpload(a) ? Task.CompletedTask : _attachmentContentProvider.WaitForContentLoading(a));
				Task.WaitAll(attachmentLoadAsync.ToArray());
			}
			catch
			{
				return Fail(MessageFailCauses.FileSaveFailed);
			}

			return Ok(await newMsgAsync);
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
