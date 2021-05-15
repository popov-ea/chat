using DataAccess.Interfaces;
using Domain.Entities;
using Domain.Enums;
using UseCases.Interfaces.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using UseCases.Interfaces.Services;

namespace UseCases.Implementation.Services
{
	public class BlackListService : IBlackListService
	{
		private readonly IRepository<BlackList> _blackListRepository;
		private readonly Mapper _mapper;

		public BlackListService(IRepository<BlackList> blackListRepository, Mapper mapper)
		{
			_blackListRepository = blackListRepository;
			_mapper = mapper;
		}

		public async Task<BlackListResultDto> BlockUserAsync(UserDto initiator, UserDto toBlock)
		{
			bool alreadyBlocked = await CheckExistsAsync(initiator, toBlock);
			if (alreadyBlocked)
			{
				return Fail(BlackListFailCauses.AlreadyBlocked);
			}

			var created = _blackListRepository.Create(new BlackList
			{
				Initiator = _mapper.Map<UserDto, User>(initiator),
				Blocked =  _mapper.Map<UserDto, User>(toBlock),
				InitiatorId = initiator.Id,
				BlockedId = toBlock.Id
			});

			return Ok(created);
		}

		public async Task<BlackListResultDto> UnblockUserAsync(UserDto initiator, UserDto blocked)
		{
			var blackListItem = await _blackListRepository.FirstOrDefaultAsync(bl => bl.Initiator.Id == initiator.Id && bl.BlockedId == blocked.Id);
			if (blackListItem == null)
			{
				return Fail(BlackListFailCauses.NotBlocked);
			}
			var deleted = await _blackListRepository.DeleteAsync(blackListItem);
			return Ok(deleted);
		}

		public async Task<bool> CheckExistsAsync(UserDto initiator, UserDto blocked)
		{
			var found = await _blackListRepository.FirstOrDefaultAsync(bl => bl.Initiator.Id == initiator.Id && bl.BlockedId == blocked.Id);
			return found != null;
		}

		public async Task<bool> CheckAnyBlocked(UserDto mightBeBlocked, IEnumerable<UserDto> mightBlock)
		{
			var mightBlockIds = mightBlock.Select((u) => u.Id);
			return await _blackListRepository.AnyAsync((bl) => bl.BlockedId == mightBeBlocked.Id && mightBlockIds.Contains(bl.InitiatorId));
		}

		private BlackListResultDto Fail(BlackListFailCauses failCause)
		{
			return new BlackListResultDto
			{
				Success = false,
				FailCause = failCause
			};
		}

		private BlackListResultDto Ok(BlackList blackList)
		{
			return new BlackListResultDto
			{
				Success = true,
				Entity = _mapper.Map<BlackList,BlackListDto>(blackList)
			};
		}
	}
}
