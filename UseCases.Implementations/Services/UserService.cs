using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCases.Interfaces.Dtos;
using UseCases.Interfaces.Services;

namespace UseCases.Implementations.Services
{
	public class UserService : IUserService
	{
		private readonly IRepository<User> _userRepository;
		private readonly Mapper _mapper;

		public UserService(IRepository<User> userRepository, Mapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}

		public async Task<UserDto> GetUserAsync(long userId)
		{
			var user = await _userRepository.FindAsync(userId);
			return MapToDto(user);
		}

		public async Task<IEnumerable<UserDto>> GetUsersAsync(IEnumerable<long> userIds)
		{
			var users = await _userRepository.AllAsync(u => userIds.Contains(u.Id));
			return users.Select(u => MapToDto(u));
		}
		
		private UserDto MapToDto(User user)
		{
			return _mapper.Map<User, UserDto>(user);
		}
	}
}
