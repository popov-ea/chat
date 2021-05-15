using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Implementation.Services
{
	public class UserService
	{
		private readonly IRepository<User> _userRepository;
		private readonly Mapper _mapper;

		public UserService(IRepository<User> userRepository, Mapper mapper)
		{
			_userRepository = userRepository;
			_mapper = mapper;
		}
	}
}
