using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;

namespace UseCases.Interfaces.Services
{
	public interface IBlackListService
	{
		public Task<BlackListResultDto> BlockUserAsync(long initiatorId, long toBlockId);
		public Task<BlackListResultDto> UnblockUserAsync(long initiatorId, long blockedId);
		public Task<bool> CheckExistsAsync(long initiatorId, long blockedId);
		public Task<bool> CheckAnyBlocked(long mightBeBlockedId, IEnumerable<long> mightBlockId);
	}
}
