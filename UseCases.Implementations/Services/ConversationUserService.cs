using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;
using UseCases.Interfaces.Dtos.Results;
using UseCases.Interfaces.Services;

namespace UseCases.Implementations.Services
{
	public class ConversationUserService : IConversationUserService
	{
		private readonly IRepository<ConversationUser> _conversationUserRepository;
		private readonly Mapper _mapper;

		public event Action<ConversationUserDto> OnUserAdded;
		public event Action<ConversationUserDto> OnUserDeleted;

		public ConversationUserService(IRepository<ConversationUser> conversationUserRepository, Mapper mapper)
		{
			_conversationUserRepository = conversationUserRepository;
			_mapper = mapper;
			// null obj pattern
			InitEmptyEventHandlers();
		}

		private void InitEmptyEventHandlers()
		{
			OnUserAdded += (x) => { };
			OnUserDeleted += (x) => { };
		}

		public async Task<ConversationUserServiceResultDto> AddUserAsync(long conversationId, long userId)
		{
			var conversationUser = await _conversationUserRepository.CreateAsync(new ConversationUser()
			{
				UserId = userId,
				ConversationId = conversationId
			});

			OnUserAdded(MapToDto(conversationUser));

			return Ok(conversationUser);
		}

		public async Task<ConversationUserServiceResultDto> DeleteUserAsync(long conversationId, long userId)
		{
			var conversationUser = await _conversationUserRepository.FirstOrDefaultAsync((cu) => cu.ConversationId == conversationId && cu.UserId == userId);

			OnUserDeleted(MapToDto(conversationUser));

			return await DeleteUserAsync(conversationUser);
		}

		private ConversationUserServiceResultDto Fail(ConversationUserServiceFailCauses failCause)
		{
			return new ConversationUserServiceResultDto()
			{
				FailCause = failCause
			};
		}

		private ConversationUserServiceResultDto Ok(ConversationUser conversationUser)
		{
			return new ConversationUserServiceResultDto()
			{
				Entity = MapToDto(conversationUser),
				Success = true
			};
		}

		private ConversationUserDto MapToDto(ConversationUser conversationUser)
		{
			return _mapper.Map<ConversationUser, ConversationUserDto>(conversationUser);
		}

		public async Task<ConversationUserServiceResultDto> DeleteUserAsync(long conversationUserId)
		{
			return await DeleteUserAsync(await _conversationUserRepository.FindAsync(conversationUserId));
		}

		private async Task<ConversationUserServiceResultDto> DeleteUserAsync(ConversationUser conversationUser)
		{ 
			if (conversationUser == null)
			{
				return Fail(ConversationUserServiceFailCauses.NoUsersFound);
			}
			var deleted = await _conversationUserRepository.DeleteAsync(conversationUser);
			return Ok(deleted);
		}

		public async Task<IEnumerable<ConversationUserDto>> GetConversationUsers(long conversationId)
		{
			var conversationUsers = await _conversationUserRepository.AllAsync((cu) => cu.ConversationId == conversationId);
			return conversationUsers.Select((cu) => MapToDto(cu));
		}
	}
}
