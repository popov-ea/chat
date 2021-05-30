using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;

namespace UseCases.Interfaces.Services
{
	public interface IUserService
	{
		public Task<UserDto> GetUserAsync(long userId);
		public Task<IEnumerable<UserDto>> GetUsersAsync(IEnumerable<long> userIds);
	}
}
