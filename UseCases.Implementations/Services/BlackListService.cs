using DataAccess.Interfaces;
using Domain.Entities;
using Domain.Enums;
using UseCases.Interfaces.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Implementation.Services
{
	public class BlackListService
	{
		private readonly IRepository<BlackList> _blackListRepository;

		public BlackListService(IRepository<BlackList> blackListRepository)
		{
			_blackListRepository = blackListRepository;
		}

		public async Task<BlackListResult> BlockUserAsync(User initiator, User toBlock)
		{
			bool alreadyBlocked = await CheckExistsAsync(initiator, toBlock);
			if (alreadyBlocked)
			{
				return Fail(BlackListFailCauses.AlreadyBlocked);
			}

			var created = _blackListRepository.Create(new BlackList
			{
				Initiator = initiator,
				Blocked = toBlock,
				InitiatorId = initiator.Id,
				BlockedId = toBlock.Id
			});

			return Ok(created);
		}

		public async Task<BlackListResult> UnblockUserAsync(User initiator, User blocked)
		{
			var blackListItem = await _blackListRepository.FirstOrDefaultAsync(bl => bl.Initiator.Id == initiator.Id && bl.BlockedId == blocked.Id);
			if (blackListItem == null)
			{
				return Fail(BlackListFailCauses.NotBlocked);
			}
			var deleted = await _blackListRepository.DeleteAsync(blackListItem);
			return Ok(deleted);
		}

		public async Task<bool> CheckExistsAsync(User initiator, User blocked)
		{
			var found = await _blackListRepository.FirstOrDefaultAsync(bl => bl.Initiator.Id == initiator.Id && bl.BlockedId == blocked.Id);
			return found != null;
		}

		public async Task<bool> CheckAnyBlocked(User mightBeBlocked, IEnumerable<User> mightBlock)
		{
			var mightBlockIds = mightBlock.Select((u) => u.Id);
			return await _blackListRepository.AnyAsync((bl) => bl.BlockedId == mightBeBlocked.Id && mightBlockIds.Contains(bl.InitiatorId));
		}

		private BlackListResult Fail(BlackListFailCauses failCause)
		{
			return new BlackListResult
			{
				Success = false,
				FailCause = failCause
			};
		}

		private BlackListResult Ok(BlackList blackList)
		{
			return new BlackListResult
			{
				Success = true,
				Entity = blackList
			};
		}
	}
}
