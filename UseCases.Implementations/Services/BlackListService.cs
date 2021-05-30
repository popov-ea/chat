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

		public async Task<BlackListResultDto> BlockUserAsync(long initiatorId, long toBlockId)
		{
			bool alreadyBlocked = await CheckExistsAsync(initiatorId, toBlockId);
			if (alreadyBlocked)
			{
				return Fail(BlackListFailCauses.AlreadyBlocked);
			}

			var created = _blackListRepository.Create(new BlackList
			{
				InitiatorId = initiatorId,
				BlockedId = toBlockId
			});

			return Ok(created);
		}

		public async Task<BlackListResultDto> UnblockUserAsync(long initiatorId, long blockedId)
		{
			var blackListItem = await _blackListRepository.FirstOrDefaultAsync(bl => bl.Initiator.Id == initiatorId && bl.BlockedId == blockedId);
			if (blackListItem == null)
			{
				return Fail(BlackListFailCauses.NotBlocked);
			}
			var deleted = await _blackListRepository.DeleteAsync(blackListItem);
			return Ok(deleted);
		}

		public async Task<bool> CheckExistsAsync(long initiatorId, long blockedId)
		{
			var found = await _blackListRepository.FirstOrDefaultAsync(bl => bl.Initiator.Id == initiatorId && bl.BlockedId == blockedId);
			return found != null;
		}

		public async Task<bool> CheckAnyBlocked(long mightBeBlockedId, IEnumerable<long> mightBlockIds)
		{
			return await _blackListRepository.AnyAsync((bl) => bl.BlockedId == mightBeBlockedId && mightBlockIds.Contains(bl.InitiatorId));
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
