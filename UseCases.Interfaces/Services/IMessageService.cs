using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;

namespace UseCases.Interfaces.Services
{
	public interface IMessageService
	{
		public Task<MessageServiceResultDto> DeleteByIdsAsync(UserDto actor, params long[] messageIds);
		public Task<MessageServiceResultDto> SendMessageAsync(UserDto sender, ConversationDto conversation, string messageText, AttachmentDto[] attachments = null);
		public Task<IEnumerable<MessageDto>> GetMessagesByConversationId(long conversationId);
	}
}
