using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;

namespace UseCases.Interfaces.Services
{
	public interface IMessageService
	{
		public Task<MessageServiceResultDto> DeleteByIdsAsync(long actorId, params long[] messageIds);
		public Task<MessageServiceResultDto> SendMessageAsync(long senderId, long conversationId, string messageText, AttachmentDto[] attachments = null);
		public Task<IEnumerable<MessageDto>> GetMessagesByConversationId(long conversationId);
	}
}
