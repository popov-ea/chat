using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;

namespace UseCases.Interfaces.Services
{
	public interface IBlackListService
	{
		public Task<BlackListResultDto> BlockUserAsync(UserDto initiator, UserDto toBlock);
		public Task<BlackListResultDto> UnblockUserAsync(UserDto initiator, UserDto blocked);
		public Task<bool> CheckExistsAsync(UserDto initiator, UserDto blocked);
		public Task<bool> CheckAnyBlocked(UserDto mightBeBlocked, IEnumerable<UserDto> mightBlock);
	}
}
